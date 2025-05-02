using System.Net.Mail;
using System.Net;
using BasicSocialMedia.Application.Helpers;
using Microsoft.Extensions.Options;
using BasicSocialMedia.Core.Interfaces.ServicesInterfaces.AuthServices;

namespace BasicSocialMedia.Application.Services.AuthServices
{
	public class EmailSender(IOptions<Mail> mail) : IEmailSender
	{
		private readonly Mail _mail = mail.Value;

	
		public async Task SendEmailAsync(string email, string subject, string htmlMessage)
		{
			var smtpClient = new SmtpClient(_mail.Host)
			{
				Port = int.Parse(_mail.Port),
				Credentials = new NetworkCredential(_mail.From, _mail.Password),
				EnableSsl = true,
			};

			var mailMessage = new MailMessage
			{
				From = new MailAddress(_mail.From),
				Subject = subject,
				Body = htmlMessage,
				IsBodyHtml = true,
			};
			mailMessage.To.Add(email);

			await smtpClient.SendMailAsync(mailMessage);
		}
	}
}
