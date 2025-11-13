public class EmailSettings
{
    public string? SmtpHost { get; set; }
    public int SmtpPort { get; set; }
    public string? ImapHost { get; set; }
    public int ImapPort { get; set; }
    public string? PopHost { get; set; }
    public int PopPort { get; set; }
    public string? SenderEmail { get; set; }
    public string? SenderPassword { get; set; }
    public string? ReceiverEmail { get; set; }
}