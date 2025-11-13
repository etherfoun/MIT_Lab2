using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

public class SpeedTest
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

    public static async Task RunSpeedTest()
    {
        EmailSettings settings = LoadEmailSettings();

        string[] attachmentsToTest = { null!, "test_files/small.txt", "test_files/large.txt" };

        Stopwatch sw = new Stopwatch();

        foreach (var attachment in attachmentsToTest)
        {
            string attachmentName = attachment ?? "No attachment";
            Console.WriteLine($"\n--- Testing: {attachmentName} ---");

            using (var client = new SmtpClient())
            {
                MimeMessage m = CreateMessage(settings, attachment!);
                sw.Restart();
                try
                {
                    client.Connect(settings.SmtpHost, settings.SmtpPort, SecureSocketOptions.StartTls);
                    client.Authenticate(settings.SenderEmail, settings.SenderPassword);
                    client.Send(m);
                    sw.Stop();
                    Console.WriteLine($"  Send() (Synchronous): {sw.ElapsedMilliseconds} ms");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"  Send() Error: {ex.Message}");
                }
                finally
                {
                    client.Disconnect(true);
                }
            }

            using (var client = new SmtpClient())
            {
                MimeMessage m = CreateMessage(settings, attachment!);
                sw.Restart();
                try
                {
                    await client.ConnectAsync(settings.SmtpHost, settings.SmtpPort, SecureSocketOptions.StartTls);
                    await client.AuthenticateAsync(settings.SenderEmail, settings.SenderPassword);
                    await client.SendAsync(m);
                    sw.Stop();
                    Console.WriteLine($"  SendAsync() (Asynchronous): {sw.ElapsedMilliseconds} ms");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"  SendAsync() Error: {ex.Message}");
                }
                finally
                {
                    await client.DisconnectAsync(true);
                }
            }
        }
    }

    private static MimeMessage CreateMessage(EmailSettings settings, string attachmentPath)
    {
        var m = new MimeMessage();
        m.From.Add(new MailboxAddress("Speed Test", settings.SenderEmail));
        m.To.Add(new MailboxAddress("Test Receiver", settings.ReceiverEmail));
        m.Subject = "Speed Test";

        var bodyBuilder = new BodyBuilder();
        bodyBuilder.TextBody = "Speed test body";

        if (!string.IsNullOrEmpty(attachmentPath))
        {
            bodyBuilder.Attachments.Add(attachmentPath);
        }
        m.Body = bodyBuilder.ToMessageBody();
        return m;
    }
}