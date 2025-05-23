using System.Net;
using System.Net.Mail;

namespace Supermaket.Areas.Admin.Repository
{
	public class EmailSender : IEmailSender
	{
		public Task SendEmailAsync(string email, string subject, string message)
		{
			var client = new SmtpClient("smtp.gmail.com", 587)
			{
				EnableSsl = true, 
				UseDefaultCredentials = false,
                Credentials = new NetworkCredential("ntphuca23059@cusc.ctu.edu.vn", "wtgpqzgibpmbwwmp")

            };

			return client.SendMailAsync(
				new MailMessage(from: "ntphuca23059@cusc.ctu.edu.vn",
								to: email,
								subject,
								message
								));
		}
	}
}
