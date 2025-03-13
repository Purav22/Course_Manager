namespace CourseManagerApp.Services
{
    public interface IEmail
    {
        public void SendEmail(string subject, string body, string toAddress);
    }
}
