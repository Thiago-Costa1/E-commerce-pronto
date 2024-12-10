using aspnetsite.Libraries.Login;
using aspnetsite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Cryptography.X509Certificates;

namespace aspnetsite.Libraries.Filtro
{
    public class ClienteAutorizacaoAttribute : Attribute, IAuthorizationFilter
    {
        LoginCliente _loginCliente;
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            _loginCliente = (LoginCliente)context.HttpContext.RequestServices.GetService(typeof(LoginCliente));
            Cliente cliente = _loginCliente.GetCliente();
            if(cliente == null)
            {
                // context.Result = new ContentResult() { Content = "Acesso negado." };
                context.Result = new RedirectToActionResult("Login", "Home", null);
            }
        }
    }
}