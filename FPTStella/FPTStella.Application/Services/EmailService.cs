using FPTStella.Application.Common.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FPTStella.Application.Services
{
    public class EmailService : IEmailService
    {
        private readonly string _apiKey = "46ef8dba8ace4687a2ff5382a83c7414";
        private readonly string _secretKey = "17f8bb78b01c9453a0982d41e20c81ad";
        public async Task<bool> SendEmailAsyncMailJet(string to, string subject, string htmlBody)
        {
            using var client = new HttpClient();
            var byteArray = Encoding.ASCII.GetBytes($"{_apiKey}:{_secretKey}");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

            var content = new
            {
                Messages = new[]
                {
                new {
                    From = new { Email = "thucnmse170295@fpt.edu.vn", Name = "Stella Team" },
                    To = new[] { new { Email = to, Name = "Recipient" } },
                    Subject = subject,
                    HTMLPart = htmlBody
                }
            }
            };

            var response = await client.PostAsync(
                "https://api.mailjet.com/v3.1/send",
                new StringContent(JsonSerializer.Serialize(content), Encoding.UTF8, "application/json")
            );

            var responseBody = await response.Content.ReadAsStringAsync();
            Console.WriteLine(responseBody);
            return response.IsSuccessStatusCode;
        }
    }
}
