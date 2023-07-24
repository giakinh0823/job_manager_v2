namespace client.Dto.Payment
{
    public class PaymentResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public string Status { get; set; }
        public string Url { get; set; }
        public string PaymentInfo { get; set; }
        public string NumberOfSchedulers { get; set; }
    }
}
