namespace DevSkill.Inventory.Web.Models
{
    public interface IEmailSender
    {
        void SendEmail(string email, string subject, string body);
    }
}
