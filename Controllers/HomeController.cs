using Proyecto.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Proyecto.Controllers
{
    public class HomeController : Controller
    {
        public static BBDD db = new BBDD();

        public ActionResult Index()
        {
            Cookie.CrearCookiesIniciales(HttpContext);
            if (!db.ComprobarSiExistenAdministradores())
            {
                db.CrearAdmin();
            }
            switch (Cookie.TipoUsuario(HttpContext))
            {
                case USER.ADMIN:
                    return View("~/Views/Shared/AdminHome.cshtml");
                case USER.NORMAL:
                    return View("~/Views/Shared/NormalHome.cshtml");
                default:
                    return View("~/Views/Shared/GuestHome.cshtml");
            }
        }

        public ActionResult GuestMenu(BUTTON id)
        {
            switch (id)
            {
                case BUTTON.HOME:
                    return View("~/Views/Shared/GuestHome.cshtml");
                case BUTTON.LOGIN:
                    return View("~/Views/Shared/Login.cshtml");
                case BUTTON.REGISTER:
                    ViewBag.fechaNacimiento = "2000-01-01";
                    return View("~/Views/Shared/UserRegister.cshtml");
                case BUTTON.ABOUT:
                    return View("~/Views/Shared/About.cshtml");
                default:
                    return View("~/Views/Shared/GuestHome.cshtml");
            }
        }
    
        public ActionResult Login()
        {
            string email = Request.Form["email"].ToString();
            string password = Request.Form["password"].ToString();

            Cookie.Nueva(HttpContext, email, Cookie.IntentosRestantes(HttpContext, email));

            switch (db.ComprobarUsuario(HttpContext, email, password))
            {
                case USER.ADMIN:
                    Cookie.BorrarCookie(HttpContext, email);
                    Cookie.Nueva(HttpContext, "usuario", email, DateTime.Now.AddMinutes(60));
                    Cookie.Nueva(HttpContext, "tipoUsuario", "admin", DateTime.Now.AddMinutes(60));
                    return View("~/Views/Shared/AdminHome.cshtml");
                case USER.NORMAL:
                    Cookie.BorrarCookie(HttpContext, email);
                    Cookie.Nueva(HttpContext, "usuario", email, DateTime.Now.AddMinutes(60));
                    Cookie.Nueva(HttpContext, "tipoUsuario", "normal", DateTime.Now.AddMinutes(60));
                    return View("~/Views/Shared/NormalHome.cshtml");
                case USER.INEXISTENTE:
                    ViewBag.Info = "<p class='mt-3 text-center text-danger'>Usuario inexistente, registrese</p>";
                    return View("~/Views/Shared/Login.cshtml");
                case USER.PASS_INCORRECTO:
                    ViewBag.info = Cookie.RestarIntentos(HttpContext, email);
                    ViewBag.Email = email;
                    return View("~/Views/Shared/Login.cshtml");
                case USER.BLOQUEADO:
                    ViewBag.Info = "<p class='mt-3 text-center text-danger'>Usuario bloqueado</p>";
                    return View("~/Views/Shared/Login.cshtml");
                default:
                    return View("~/Views/Shared/Login.cshtml");
            }
        }

        public ActionResult UserRegister()
        {
            string nombre = Request.Form["nombre"].ToString();
            string apellidos = Request.Form["apellidos"].ToString();
            string fechaNacimiento = Request.Form["fechaNacimiento"].ToString();
            string email = Request.Form["email"].ToString();
            string password = Request.Form["password"].ToString();

            if (db.ComprobarUsuario(HttpContext,email, null) != USER.PASS_INCORRECTO) { 
                db.CrearUsuario(nombre, apellidos, DateTime.Parse(fechaNacimiento), email, password, false);
                ViewBag.fechaNacimiento = "2000-01-01";
                ViewBag.Info = "<p class='mt-3 text-center text-success'>Usuario creado</p>";
            }
            else
            {
                ViewBag.Nombre = nombre;
                ViewBag.Apellidos = apellidos;
                ViewBag.FechaNacimiento = fechaNacimiento;
                ViewBag.Email = email;
                ViewBag.Password = password;
                ViewBag.Info = "<p class='mt-3 text-center text-danger'>Email ya registrado</p>";
            }
            return View("~/Views/Shared/UserRegister.cshtml");
        }

    }
}