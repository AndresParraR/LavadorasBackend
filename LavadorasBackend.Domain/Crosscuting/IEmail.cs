
namespace Lavadoras.Domain.Crosscuting;

public interface IEmail
{
    void SendWelcome(string email, string firstName);
    void SendCredentials(string email, string username, string firstName, string password);
    void SendNewPassword(string email, string firstName, string password);
    void SendChangePasswordConfirmation(string email, string firstName);
    public void SendCodeVerification(string email, string firstName, string code);
}
