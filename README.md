To test the correctness of the code execution, you need to add the appsettings.json file with the following parameters:

```
{
  "EmailSettings": {
    "SmtpHost": "smtp.gmail.com",
    "SmtpPort": 587,
    "ImapHost": "imap.gmail.com",
    "ImapPort": 993,
    "PopHost": "pop.gmail.com",
    "PopPort": 995,
    "SenderEmail": "SENDER_EMAIL",
    "SenderPassword": "SENDER_PASSWORD",
    "ReceiverEmail": "RECEIVER_EMAIL"
  }
}
```
