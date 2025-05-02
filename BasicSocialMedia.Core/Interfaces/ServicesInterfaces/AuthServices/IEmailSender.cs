namespace BasicSocialMedia.Core.Interfaces.ServicesInterfaces.AuthServices
{
	public interface IEmailSender
	{
		Task SendEmailAsync(string toEmail, string subject, string message);
	}
}
