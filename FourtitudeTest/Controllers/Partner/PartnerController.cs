using FourtitudeTest.Model.Dto;
using Microsoft.AspNetCore.Mvc;

namespace FourtitudeTest.Controllers.Partner
{
    public class PartnerController : Controller
    {
        private readonly DataContext _context;

        public PartnerController(DataContext context)
        {
            _context = context;            
        }

        [HttpPost("[action]")]
        public ActionResult submittrxmessage([FromBody] PartnerSubmitTransactionPostDto request)
        {
            if (request == null)
            {
                return BadRequest(new PartnerSubmitTransactionResponseDto
                {
                    result = 0,
                    resultMessage = "Invalid request"
                });
            }

            try
            {

                // Step 1: Retrieve partner by partnerkey (or any identifier)
                var partner = _context.partnerDbm.FirstOrDefault(p => p.partnerRefNo == request.partnerRefno);
                if (partner == null)
                {
                    return BadRequest(new PartnerSubmitTransactionResponseDto
                    {
                        result = 0,
                        resultMessage = "Partnet not exist"
                    });
                }

                // Step 2: Decode password and compare
                var decodedPassword = Helper.Reuseable.DecodeBase64(request.partnerPassword);
                if (partner.partnerPassword != decodedPassword)
                {
                    return BadRequest(new PartnerSubmitTransactionResponseDto
                    {
                        result = 0,
                        resultMessage = "Access Denied!"
                    });
                }

                // Step 3: Signature validation
                var calculatedSig = Helper.Reuseable.GenerateSignatureTransaction(request);
                if (calculatedSig != request.sig)
                {
                    return BadRequest(new PartnerSubmitTransactionResponseDto
                    {
                        result = 0,
                        resultMessage = "Access Denied!"
                    });
                }

                // Step 4: Business logic or response
                return Ok(new PartnerSubmitTransactionResponseDto
                {
                    result = 1,
                    totalAmount = request.totalAmount,
                    totalDiscount = request.t,
                    finalAmount = finalAmount,
                });

            }
            catch (Exception ex)
            {
                return BadRequest(new PartnerSubmitTransactionResponseDto
                {
                    result = 0,
                    resultMessage = ex.Message
                });
            }
        }
    }
}
