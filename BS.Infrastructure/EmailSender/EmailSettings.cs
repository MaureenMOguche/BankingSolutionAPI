namespace BS.Infrastructure.EmailSender
{
    public class EmailSettings
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string SmtpServer { get; set; }
        public int Port { get; set; }
        public string DisplayName { get; set; }
    }
}
