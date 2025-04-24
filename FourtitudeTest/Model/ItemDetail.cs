using System.ComponentModel.DataAnnotations.Schema;

namespace FourtitudeTest.Model
{
    public class ItemDetail
    {
        [ForeignKey(nameof(Partner))]
        public string companyDbmId { get; set; }
        public Partner partner { get; set; }
        public string name { get; set; }
        public int qty { get; set; }
        public long unitPrice { get; set; }
    }
}
