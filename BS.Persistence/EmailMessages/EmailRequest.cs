namespace BS.API.Controllers
{
    public class EmailRequest
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}