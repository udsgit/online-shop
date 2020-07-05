using System;
using System.Web;
using System.Web.Mvc;

namespace Proyecto.Models
{
    public static class Cookie
    {

        public static string numIntentos = "3";
        public static void Nueva(HttpContextBase HttpContext, string nombre, string valor, DateTime expiracion = new DateTime())
        {
            try
            {
                if (HttpContext.Request.Cookies.Get(nombre).Value != valor)
                {
                    HttpContext.Response.Cookies.Get(nombre).Value = valor;
                    HttpContext.Response.Cookies.Get(nombre).Expires = expiracion;
                }
            }
            catch
            {
                HttpCookie cookie = new HttpCookie(nombre, valor);
                cookie.Expires = expiracion;
                HttpContext.Response.SetCookie(cookie);
            }
        }

        public static string RestarIntentos(HttpContextBase HttpContext, string email)
        {
            int intentosRestantes = Convert.ToInt32(HttpContext.Request.Cookies.Get(email).Value);
            Cookie.Nueva(HttpContext, email, (intentosRestantes - 1).ToString());
            if (intentosRestantes <= 1)
            {
                Cookie.Nueva(HttpContext, email, "bloqueado");
                return "<p class='mt-3 text-center text-danger'>Usuario bloqueado</p>";
            }
            else
            {
                return "<p class='mt-3 text-center text-danger'>Contraseña incorrecta, queda(n) " + (intentosRestantes - 1) + " intento(s)</p>";
            }
        }

        public static bool UsuarioBloqueado(HttpContextBase HttpContext, string email)
        {
            try
            {
                return HttpContext.Request.Cookies.Get(email).Value == "bloqueado" ? true : false;
            }
            catch
            {
                return false;
            }
        }

        public static void CrearCookiesIniciales(HttpContextBase HttpContext)
        {
            try
            {
                string tipoUsuario = HttpContext.Request.Cookies.Get("tipoUsuario").Value;
            }
            catch
            {
                Cookie.Nueva(HttpContext, "tipoUsuario", "visitante");
            }
        }

        public static string IntentosRestantes(HttpContextBase HttpContext, string email)
        {
            try
            {
                return HttpContext.Request.Cookies.Get(email).Value;
            }
            catch
            {
                Cookie.Nueva(HttpContext, email, numIntentos);
                return numIntentos;
            }

        }

        public static USER TipoUsuario(HttpContextBase HttpContext)
        {
            try
            {
                switch (HttpContext.Request.Cookies.Get("tipoUsuario").Value)
                {
                    case "admin":
                        return USER.ADMIN;
                    case "normal":
                        return USER.NORMAL;
                    default:
                        return USER.VISITANTE;
                }
            }
            catch
            {
                Cookie.Nueva(HttpContext, "tipoUsuario", "visitante");
                return USER.VISITANTE;
            }

        }

        public static void BorrarCookie(HttpContextBase HttpContext, string cookie)
        {
            HttpContext.Response.Cookies[cookie].Expires = DateTime.Now.AddDays(-1);
        }
    }
}