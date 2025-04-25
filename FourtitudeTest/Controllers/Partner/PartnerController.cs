using FourtitudeTest.Model.Dbm;
using FourtitudeTest.Model.Dto;
using FourtitudeTest.Service;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FourtitudeTest.Controllers.Partner
{
    public class PartnerController : Controller
    {
        private readonly DataContext _context;
        private readonly PartnerService _partnerService;

        public PartnerController(DataContext context, PartnerService partnerService)
        {
            _context = context;            
            _partnerService = partnerService;
        }

        [HttpPost("[action]")]
        public ActionResult submittrxmessage([FromBody] PartnerSubmitTransactionPostDto request)
        {
            if (request == null)
            {
                return BadRequest(SerializeWithoutNulls(new PartnerSubmitTransactionResponseDto
                {
                    result = 0,
                    resultMessage = "Invalid request"
                }));
            }

            try
            {

                // Step 1: Retrieve partner by partnerkey (or any identifier)
                PartnerDbm currentPartner = _partnerService.GetPartnerByRefNo(request.partnerRefno);

                if(currentPartner == null)
                {
                    currentPartner = Helper.Reuseable.Partners.FirstOrDefault(x => x.partnerRefNo == request.partnerRefno);

                    if(currentPartner == null)
                    {
                        return BadRequest(SerializeWithoutNulls (new PartnerSubmitTransactionResponseDto
                        {
                            result = 0,
                            resultMessage = "Access Denied!"
                        }));
                    }
                }

                // Step 2: Decode password and compare
                var decodedPassword = Helper.Reuseable.DecodeBase64(request.partnerPassword);
                if (currentPartner.partnerPassword != decodedPassword)
                {
                    return BadRequest(SerializeWithoutNulls(new PartnerSubmitTransactionResponseDto
                    {
                        result = 0,
                        resultMessage = "Access Denied!"
                    }));
                }

                // Step 3: Signature validation
                var calculatedSig = Helper.Reuseable.GenerateSignatureTransaction(request);
                if (calculatedSig != request.sig)
                {
                    return BadRequest(SerializeWithoutNulls(new PartnerSubmitTransactionResponseDto
                    {
                        result = 0,
                        resultMessage = "Access Denied!"
                    }));
                }

                if(request.items != null)
                {
                    long totalAmountItems = 0;
                    foreach (var item in request.items)
                    {
                        totalAmountItems += item.unitprice * item.qty;
                    }

                    if(request.totalAmount != totalAmountItems)
                    {
                        return BadRequest(SerializeWithoutNulls(new PartnerSubmitTransactionResponseDto
                        {
                            result = 0,
                            resultMessage = "Invalid Total Amount."
                        }));
                    }
                }

                long totalDiscount = CalculateDiscount(request.totalAmount);

                long finalAmount = request.totalAmount - totalDiscount;

                // Step 4: Business logic or response
                return Ok(SerializeWithoutNulls(new PartnerSubmitTransactionResponseDto
                {
                    result = 1,
                    totalAmount = request.totalAmount,
                    totalDiscount = totalDiscount,
                    finalAmount = finalAmount,
                }));

            }
            catch (Exception ex)
            {
                return BadRequest(SerializeWithoutNulls(new PartnerSubmitTransactionResponseDto
                {
                    result = 0,
                    resultMessage = ex.Message.ToString()
                }));
            }
        }

        public long CalculateDiscount(long totalAmount)
        {
            double baseDiscountPercentage = 0;

            // Base discount logic
            if (totalAmount >= 20000 && totalAmount <= 50000)
            {
                baseDiscountPercentage = 0.05;
            }
            else if (totalAmount >= 50100 && totalAmount <= 80000)
            {
                baseDiscountPercentage = 0.07;
            }
            else if (totalAmount >= 80100 && totalAmount <= 120000)
            {
                baseDiscountPercentage = 0.10;
            }
            else if (totalAmount > 120000)
            {
                baseDiscountPercentage = 0.15;
            }

            double conditionalDiscountPercentage = 0;

            // Prime number check
            if (totalAmount > 50000 && IsPrime(totalAmount/100))
            {
                conditionalDiscountPercentage += 0.08;
            }

            // Ends with 5 check
            if (totalAmount > 90000 && (totalAmount/100) % 10 == 5)
            {
                conditionalDiscountPercentage += 0.10;
            }

            double totalDiscountPercentage = baseDiscountPercentage + conditionalDiscountPercentage;

            // Cap the total discount to 20%
            if (totalDiscountPercentage > 0.20)
            {
                totalDiscountPercentage = 0.20;
            }

            long totalDiscount = (long)Math.Floor(totalAmount * totalDiscountPercentage);

            return totalDiscount;
        }

        private bool IsPrime(long number)
        {
            if (number < 2)
            {
                return false;
            } 

            if (number == 2 || number == 3)
            {
                return true;
            } 

            if (number % 2 == 0 || number % 3 == 0)
            {
                return false;
            } 

            for (long i = 5; i * i <= number; i += 6)
            {
                if (number % i == 0 || number % (i + 2) == 0)
                {
                    return false;
                }                    
            }

            return true;
        }

        private static string SerializeWithoutNulls(object obj)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };

            return JsonSerializer.Serialize(obj, options);
        }
    }
}
