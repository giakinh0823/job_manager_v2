namespace server.Dto.Payment
{
    public class PaymentResponse
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public string Url { get; set; }
        public string PaymentInfo { get; set; }
        public string NumberOfSchedulers { get; set; }
    }
}
