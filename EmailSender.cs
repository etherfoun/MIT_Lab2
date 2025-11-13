using Microsoft.Extensions.Configuration;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

public class EmailSender
{
    private static EmailSettings LoadEmailSettings()
    {
        var configBuilder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

        IConfigurationRoot configuration = configBuilder.Build();

        var settings = new EmailSettings();
        configuration.GetSection("EmailSettings").Bind(settings);

        return settings;
    }

    public static void SendEmail(string subject, string body, string attachmentPath = null!)
    {
        EmailSettings settings = LoadEmailSettings();

        if (string.IsNullOrEmpty(settings.SenderPassword))
        {
            Console.WriteLine("Error: SenderPassword not set in appsettings.json!");
            return;
        }

        var m = new MimeMessage();
        m.From.Add(new MailboxAddress("Lab2Sender", settings.SenderEmail));
        m.To.Add(new MailboxAddress("Lab2Receiver", settings.ReceiverEmail));
        m.Subject = subject;

        var bodyBuilder = new BodyBuilder();
        bodyBuilder.TextBody = body;

        if (!string.IsNullOrEmpty(attachmentPath))
        {
            bodyBuilder.Attachments.Add(attachmentPath);
        }
        m.Body = bodyBuilder.ToMessageBody();

        using (var client = new SmtpClient())
        {
            try
            {
                Console.WriteLine("Connecting to Gmail with MailKit...");
                client.Connect(settings.SmtpHost, settings.SmtpPort, SecureSocketOptions.StartTls);

                Console.WriteLine("Authenticating...");
                client.Authenticate(settings.SenderEmail, settings.SenderPassword);

                Console.WriteLine("Sending email...");
                client.Send(m);
                Console.WriteLine("Email sent successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
            }
            finally
            {
                client.Disconnect(true);
            }
        }
    }
}