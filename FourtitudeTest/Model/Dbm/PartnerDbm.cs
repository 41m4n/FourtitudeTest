using System.ComponentModel.DataAnnotations;

namespace FourtitudeTest.Model.Dbm
{
    public class PartnerDbm
    {
        [Key]
        public string Id { get; set; }
        public string partnerRefNo { get; set; }
        public string allowedPartner { get; set; }
        public string partnerPassword { get; set; }
    }
}
