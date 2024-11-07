
namespace Lavadoras.Persistence.Email;

public class GmailSettings
{
    public const string SectionName = "SmtpSettings";
    public string? SmtpUsername { get; init; }
    public string? SmtpFrom { get; init; }
    public string? SmtpServer { get; init; }
    public int? SmtpPort { get; init; }
    public string? SmtpPassword { get; init; }
}
