using FourtitudeTest.Model.Dto;
using System.Security.Cryptography;
using System.Text;

namespace FourtitudeTest.Helper
{
    public static class Reuseable
    {
        public static string GenerateSignatureTransaction(PartnerSubmitTransactionPostDto request)
        {
            var flatParams = new List<string>();

            // Step 1: Alphabetically sorted top-level properties (excluding timestamp and sig)
            var topLevelDict = new Dictionary<string, string>
            {
                { nameof(request.partnerkey), request.partnerkey },
                { nameof(request.partnerRefno), request.partnerRefno },
                { nameof(request.totalAmount), request.totalAmount.ToString() },
                { nameof(request.partnerPassword), request.partnerPassword }
            };

            foreach (var kvp in topLevelDict.OrderBy(k => k.Key.ToLower()))
            {
                flatParams.Add(kvp.Value ?? "");
            }

            // Step 2: Add timestamp at the end
            if (DateTime.TryParse(request.timeStamp, out DateTime parsedTimestamp))
            {
                flatParams.Prepend(parsedTimestamp.ToString("yyyyMMddHHmmss"));
            }
            else
            {
                throw new FormatException("Invalid timestamp format.");
            }

            // Step 3: Concatenate, hash with SHA256, lowercase hex, then base64
            var concatenated = string.Concat(flatParams);
            using var sha256 = SHA256.Create();
            var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(concatenated));
            var hex = BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
            var base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(hex));

            return base64;
        }

        public static string DecodeBase64(string base64Encoded)
        {
            byte[] data = Convert.FromBase64String(base64Encoded);
            return Encoding.UTF8.GetString(data);
        }

    }
}
