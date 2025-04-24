using System.ComponentModel.DataAnnotations.Schema;

namespace FourtitudeTest.Model.Dbm
{
    public class ItemDetailDbm
    {
        [ForeignKey(nameof(PartnerDbm))]
        public string partnerDbmId { get; set; }
        public PartnerDbm partner { get; set; }
        public string name { get; set; }
        public int qty { get; set; }
        public long unitPrice { get; set; }
    }
}
