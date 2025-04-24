using System.ComponentModel.DataAnnotations;

namespace FourtitudeTest.Model
{
    public class Partner
    {
        [Key]
        public string Id { get; set; }
        public string partnerRefNo { get; set; }
        public string partnerPassword { get; set; }
        public long totalAmount { get; set; }
        private List<ItemDetail> items { get; set; }
        public DateTime timeStamp { get; set; }
        public string sig { get; set; }
    }
}
