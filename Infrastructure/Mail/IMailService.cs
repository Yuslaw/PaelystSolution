namespace PaelystSolution.Infrastructure.Mail
{
    public interface IMailService
    {
        void SendEMailAsync(MailRequest mailRequest);
        void GetRecievers(List<MailRequest> mailRequests);
    }
}
