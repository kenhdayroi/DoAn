using System;
using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace LuxStay.Dao
{
    public class SendMailDao
    {
        private readonly IConfiguration _configuration;

        public SendMailDao(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void SendMail(string toEmailAddress, string subject, string content)
        {
            // Cấu hình trực tiếp địa chỉ email và thông tin SMTP
            var fromEmailAddress = "luxstaywebsite@zohomail.com";
            var fromEmailDisplayName = "LUXSTAY"; // Bạn có thể thay đổi tên hiển thị nếu cần
            var fromEmailPassword = "iVhwsrKb1p6A";
            var smtpHost = "smtp.zoho.com";
            var smtpPort = 587; // Cổng SMTP cho Zoho thường là 587

            if (string.IsNullOrEmpty(fromEmailAddress))
                throw new ArgumentNullException(nameof(fromEmailAddress), "From email address cannot be null or empty.");
            if (string.IsNullOrEmpty(toEmailAddress))
                throw new ArgumentNullException(nameof(toEmailAddress), "To email address cannot be null or empty.");

            using (var message = new MailMessage(new MailAddress(fromEmailAddress, fromEmailDisplayName), new MailAddress(toEmailAddress))
            {
                Subject = subject,
                IsBodyHtml = true,
                Body = content
            })
            using (var client = new SmtpClient
            {
                Credentials = new NetworkCredential(fromEmailAddress, fromEmailPassword),
                Host = smtpHost,
                EnableSsl = true,
                Port = smtpPort
            })
            {
                try
                {
                    client.Send(message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error sending email: {ex.Message}");
                }
            }
        }

        public string RandomCode(int lengthOfCode)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var code = new char[lengthOfCode];
            var random = new Random();

            for (int i = 0; i < lengthOfCode; i++)
            {
                code[i] = chars[random.Next(chars.Length)];
            }

            return new string(code);
        }

        public void SendVerificationLinkEmail(string emailId)
        {
            string subject = "Tạo tài khoản thành công";
            string body = "<br/><br/>We are excited to tell you that your account is" +
                          " successfully created. Please click on the below link to verify your account" +
                          " <br/><br/><a href='"  + "'>" + "Verify" + "</a>";

            // Gọi hàm SendMail để gửi email
            SendMail(emailId, subject, body);
        }

    }
}
