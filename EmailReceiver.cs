using Microsoft.Extensions.Configuration;
using MailKit.Net.Imap;
using MailKit.Net.Pop3;
using MailKit;

public class EmailReceiver
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

    public static void ReceiveWithImap()
    {
        EmailSettings settings = LoadEmailSettings();

        using (var client = new ImapClient())
        {
            try
            {
                client.Connect(settings.ImapHost, settings.ImapPort, true);
                client.Authenticate(settings.SenderEmail, settings.SenderPassword);
                client.Inbox.Open(FolderAccess.ReadOnly);

                Console.WriteLine($"IMAP: Total messages: {client.Inbox.Count}");

                int count = Math.Min(client.Inbox.Count, 5);
                Console.WriteLine($"IMAP: Showing last {count} emails:");

                for (int i = 0; i < count; i++)
                {
                    var message = client.Inbox.GetMessage(client.Inbox.Count - 1 - i);
                    Console.WriteLine($"--- Email {i + 1} ---");
                    Console.WriteLine($"From: {message.From}");
                    Console.WriteLine($"Subject: {message.Subject}");
                }

                client.Disconnect(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"IMAP Error: {ex.Message}");
            }
        }
    }

    public static void ReceiveWithPop3()
    {
        EmailSettings settings = LoadEmailSettings();

        using (var client = new Pop3Client())
        {
            try
            {
                client.Connect(settings.PopHost, settings.PopPort, true);
                client.Authenticate(settings.SenderEmail, settings.SenderPassword);

                int count = client.GetMessageCount();
                Console.WriteLine($"\nPOP3: Total messages: {count}");

                for (int i = 1; i < 6; i++)
                {
                    var message = client.GetMessage(i);
                    Console.WriteLine($"--- Email {i} ---");
                    Console.WriteLine($"From: {message.From}");
                    Console.WriteLine($"Subject: {message.Subject}");
                }

                client.Disconnect(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"POP3 Error: {ex.Message}");
            }
        }
    }
}