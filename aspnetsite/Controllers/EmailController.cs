using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Mvc;

public class EmailController : Controller
{
    [HttpPost]
    public IActionResult SendEmail(string email)
    {
        try
        {
            var fromAddress = new MailAddress("techflexnotebook@gmail.com", "TechFlexNotebook");
            var toAddress = new MailAddress(email); // Use "email" aqui
            const string fromPassword = "paxm jhna djbx tmlg";
            const string subject = "Promoção Especial para Você!";
            const string body = @"<h1 style=""text-align: center;"">Promoção Especial</h1>
                              <h2>Confira nossa promoção imperdível!</h2>
                              <a href=""https://thiago-costa1.github.io/E-commerce/"">
                              <img src=""cid:promo_image"" style=""width:800px; height:auto;"">
                              </a>
                              <p style=""font-size: 20px; font-weight: bold;"">Não perca essa oportunidade!</p>";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };

            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
            {
                message.AlternateViews.Add(AlternateView.CreateAlternateViewFromString("Confira nossa promoção exclusiva!", null, "text/plain"));

                // Adiciona a imagem embutida
                var linkedResource = new LinkedResource("wwwroot/Imagens/1sliide-carrossel.png", "image/png")
                {
                    ContentId = "promo_image"
                };
                var htmlView = AlternateView.CreateAlternateViewFromString(body, null, "text/html");
                htmlView.LinkedResources.Add(linkedResource);
                message.AlternateViews.Add(htmlView);

                smtp.Send(message);
            }

            return Ok("E-mail enviado com sucesso!");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro ao enviar e-mail: {ex.Message}");
        }
    }
}
