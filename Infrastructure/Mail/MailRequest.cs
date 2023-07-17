using sib_api_v3_sdk.Model;

namespace PaelystSolution.Infrastructure.Mail
{
    public class MailRequest
    {
        public string ToEmail { get; set; }
        public string ToName { get; set; }
        public string AttachmentName { get; set; }
        public string HtmlContent { get; set; }
        public string Subject { get; set; }
        public List<SendSmtpEmailAttachment> Attachments { get; set; }
    }
}
