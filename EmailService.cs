using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace StockQuoteAlert;

public class EmailService {
    private readonly EmailSettings _emailSettings;

    public EmailService(IOptions<EmailSettings> senhas) {
        _emailSettings = senhas.Value;
    }

    public async Task EnviarEmail(string assunto, string mensagem) {
        var mail = new MailMessage();
        mail.From = new MailAddress(_emailSettings.EmailOrigem); // quem manda
        mail.To.Add(_emailSettings.EmailDestino); // quem recebe
        mail.Subject = assunto;
        mail.Body = mensagem;

        using var smtp = new SmtpClient(_emailSettings.ServidorSmtp, _emailSettings.Porta);
        smtp.Credentials = new NetworkCredential(_emailSettings.EmailOrigem, _emailSettings.Senha);
        smtp.EnableSsl = true;

        await smtp.SendMailAsync(mail); // "botão" de enviar
    }
}