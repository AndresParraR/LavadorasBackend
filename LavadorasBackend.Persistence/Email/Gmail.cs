using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Options;
using Lavadoras.Domain.Crosscuting;

namespace Lavadoras.Persistence.Email;

public class Gmail : IEmail
{
    public string SmtpServer { get; set; }
    public string SmtpUsername { get; set; }
    public string SmtpPassword { get; set; }
    public int SmtpPort { get; set; }
    public string SmtpFrom { get; set; }

    private readonly GmailSettings _gmailSettings;

    public Gmail(IOptions<GmailSettings> gmailOptions)
    {
        _gmailSettings = gmailOptions.Value;
    }

    public void SendCredentials(string email, string username, string firstName, string password)
    {
        try
        {
            var mm = new MailMessage()
            {
                From = new MailAddress(_gmailSettings.SmtpFrom, _gmailSettings.SmtpFrom),
                IsBodyHtml = true,
                Subject = $"Credenciales Lavafix",
                Body = $"Hola {firstName}, Estas son tus credenciales de ingreso a la plataforma de Lavafix.<br /><br />Correo: {username}<br />Contraseña: {password}"
            };
            mm.To.Add(email);
            Send(mm);
        }
        catch (Exception ex)
        {
        }
    }

    public void SendNewPassword(string email, string firstName, string password)
    {
        try
        {
            var mm = new MailMessage()
            {
                From = new MailAddress(_gmailSettings.SmtpFrom, _gmailSettings.SmtpFrom),
                IsBodyHtml = true,
                Subject = $"Nuevas credenciales",
                Body = $"Hola {firstName}, Esta es tu nueva contraseña: {password}"
            };
            mm.To.Add(email);
            Send(mm);
        }
        catch (Exception ex)
        {
        }
    }

    public void SendChangePasswordConfirmation(string email, string firstName)
    {
        try
        {
            var mm = new MailMessage()
            {
                From = new MailAddress(_gmailSettings.SmtpFrom, _gmailSettings.SmtpFrom),
                IsBodyHtml = true,
                Subject = $"Confirmación de cambio de contraseña",
                Body = $"Hola {firstName}, Este correo es para notificarte que tu contraseña ha cambiado. Si no has sido tu por favor contacta a un administrador del sistema."
            };
            mm.To.Add(email);
            Send(mm);
        }
        catch (Exception ex)
        {
        }
    }

    public void SendCodeVerification(string email, string firstName, string code)
    {
        try
        {
            var mm = new MailMessage()
            {
                From = new MailAddress(_gmailSettings.SmtpFrom, _gmailSettings.SmtpUsername),
                IsBodyHtml = true,
                Subject = $"Codigo de Verificación",
                Body = $"Hola {firstName}, Este es tu codigo de verificación: {code}"
            };
            mm.To.Add(email);
            Send(mm);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public void SendWelcome(string email, string firstName)
    {
        try
        {
            var mm = new MailMessage()
            {
                From = new MailAddress(_gmailSettings.SmtpFrom, _gmailSettings.SmtpFrom),
                IsBodyHtml = true,
                Subject = $"Bienvenido Lavafix",
                Body = $"Hola {firstName}, Queremos darte la bienvenida a Lavafix"
            };
            mm.To.Add(email);
            Send(mm);
        }
        catch (Exception ex)
        {
        }
    }

    private void Send(MailMessage mail)
    {

        var client = new SmtpClient(_gmailSettings.SmtpServer)
        {
            EnableSsl = true,
            UseDefaultCredentials = false,
            Port = Convert.ToInt32(_gmailSettings.SmtpPort),
            Credentials = new NetworkCredential(_gmailSettings.SmtpUsername, _gmailSettings.SmtpPassword),
        };

        client.Send(mail);
    }
}
