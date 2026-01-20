using System.Net;
using System.Net.Mail;

namespace FactoryBridgeDashboard.Services
{
    public class AlertService
    {
        private DateTime _lastEmailSent = DateTime.MinValue;
        private readonly TimeSpan _cooldown = TimeSpan.FromMinutes(10); // Only email once every 10 mins

        public async Task SendCriticalAlert(string machineName, double speed)
        {
            // 1. Check Cooldown (Don't spam)
            if (DateTime.Now - _lastEmailSent < _cooldown)
            {
                return; // Too soon, ignore.
            }

            try
            {
                // 2. Configure the Email (Simulated or Real)
                // In a real factory, this would be the company Exchange Server.
                // For this portfolio demo, we will LOG it as if it sent.

                var subject = $"🚨 CRITICAL ALERT: {machineName} Overspeed";
                var body = $"Warning: {machineName} has exceeded safety limits.\nCurrent Speed: {speed:N0} RPM.\nTime: {DateTime.Now}";

                // --- REAL CODE BLOCK (Commented out until you have SMTP credentials) ---
                /*
                using (var client = new SmtpClient("smtp.gmail.com", 587))
                {
                    client.Credentials = new NetworkCredential("your-email@gmail.com", "your-app-password");
                    client.EnableSsl = true;
                    var mail = new MailMessage("system@factorybridge.com", "manager@enefit.ee", subject, body);
                    await client.SendMailAsync(mail);
                }
                */
                // ---------------------------------------------------------------------

                // 3. The Portfolio "Proof" (Simulating the Send)
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[EMAIL SENT] To: manager@enefit.ee | Subject: {subject}");
                Console.ResetColor();

                _lastEmailSent = DateTime.Now;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send alert: {ex.Message}");
            }
        }
    }
}