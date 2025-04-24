namespace FourtitudeTest.Model.Dto
{
    public class PartnerSubmitTransactionResponseDto
    {
        public int result {  get; set; }
        public long totalAmount { get; set; }
        public long totalDiscount { get; set; }
        public long finalAmount { get; set; }
        public string resultMessage { get; set; }
    }
}
