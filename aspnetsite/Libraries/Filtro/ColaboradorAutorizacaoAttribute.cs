﻿using aspnetsite.Libraries.Login;
using aspnetsite.Models.Constant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace aspnetsite.Libraries.Filtro
{
    public class ColaboradorAutorizacaoAttribute : Attribute, IAuthorizationFilter
    {


        private string _tipoColaboradorAutorizado;
        public ColaboradorAutorizacaoAttribute(string TipoColaboradorAutorizado = ColaboradorTipoConstant.Comum)
        {
            _tipoColaboradorAutorizado = TipoColaboradorAutorizado;
        }

        LoginColaborador _loginColaborador;
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            _loginColaborador = (LoginColaborador)context.HttpContext.RequestServices.GetService(typeof(LoginColaborador));
            Models.Colaborador colaborador = _loginColaborador.GetColaborador();
            if (colaborador == null)
            {
                context.Result = new RedirectToActionResult("Login", "Home", null);
            }
            else
            {
                if (colaborador.Tipo == ColaboradorTipoConstant.Comum && _tipoColaboradorAutorizado == ColaboradorTipoConstant.Gerente)
                {
                    context.Result = new ForbidResult();
                }
            }
        }
    }
}
