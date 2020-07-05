using Proyecto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Proyecto.Controllers
{
    public class UserController : Controller

    {

        public ActionResult NormalMenu(BUTTON id)
        {
            string email = HttpContext.Request.Cookies.Get("usuario").Value;
            switch (id)
            {
                case BUTTON.HOME:
                    return View("~/Views/Shared/NormalHome.cshtml");
                case BUTTON.DISCONNECT:
                    return RedirectToAction("Disconnect", "User");
                case BUTTON.SHOP:
                    ViewBag.Articulos = HomeController.db.ConsultarArticulos();
                    ViewBag.HayArticulos = HomeController.db.ConsultarSiHayArticulos();
                    return View("~/Views/Shared/UserShop.cshtml");
                case BUTTON.VIEW_PROFILE:
                    ViewBag.DatosPerfil = HomeController.db.ConsultaDatosPerfil(email);
                    return View("~/Views/Shared/ViewProfile.cshtml");
                case BUTTON.EDIT_PROFILE:
                    ViewBag.Mensaje = "";
                    ViewBag.DatosPerfil = HomeController.db.ConsultaDatosPerfil(email);
                    return View("~/Views/Shared/EditProfile.cshtml");
                default:
                    return View("~/Views/Shared/NormalMenu.cshtml");
            }
        }

        public ActionResult AdminMenu(BUTTON id)
        {
            switch (id)
            {
                case BUTTON.HOME:
                    return View("~/Views/Shared/AdminHome.cshtml");
                case BUTTON.DISCONNECT:
                    return RedirectToAction("Disconnect", "User");
                case BUTTON.USERS_TABLE:
                    ViewBag.Usuarios = HomeController.db.ConsultarUsuarios();
                    ViewBag.Borrado = "";
                    return View("~/Views/Shared/UsersTable.cshtml");
                case BUTTON.ARTICLES_TABLE:
                    ViewBag.Articulos = HomeController.db.ConsultarArticulos();
                    ViewBag.HayArticulos = HomeController.db.ConsultarSiHayArticulos();
                    return View("~/Views/Shared/ArticlesTable.cshtml");
                case BUTTON.ARTICLE_REGISTER:
                    return View("~/Views/Shared/ArticleRegister.cshtml");
                default:
                    return View("~/Views/Shared/AdminHome.cshtml");
            }
        }


        public ActionResult DeleteUser(string email)
        {
            if (HomeController.db.BorrarUsuario(email).Admin)
            {
                ViewBag.Borrado = "";
                return RedirectToAction("Disconnect","User");
            };
            ViewBag.Usuarios = HomeController.db.ConsultarUsuarios();
            ViewBag.Borrado = "<p class='mt-3 mb-3 text-center'>Se ha borrado el usuario " + email + "</p>";
            return View("~/Views/Shared/UsersTable.cshtml");
        }

        public ActionResult Disconnect()
        {
            Cookie.Nueva(HttpContext, "tipoUsuario", "visitante");
            Cookie.BorrarCookie(HttpContext, "usuario");
            return View("~/Views/Shared/GuestHome.cshtml");
        }

        public ActionResult EditProfile()
        {
            string nombre = Request.Form["nombre"].ToString();
            string apellidos = Request.Form["apellidos"].ToString();
            string email = Request.Cookies.Get("usuario").Value;
            string password = Request.Form["password"].ToString();
            HomeController.db.ActualizarUsuario(nombre, apellidos, email, password);
            ViewBag.DatosPerfil = HomeController.db.ConsultaDatosPerfil(email);
            ViewBag.Mensaje = "<p class='mt-3 text-center'>Datos actualizados</p>";
            return View("~/Views/Shared/EditProfile.cshtml");

        
        }

        public ActionResult ArticleRegister()
        {
            string nombre = Request.Form["nombre"].ToString();
            string categoria = Request.Form["categoria"].ToString();
            int stock = Convert.ToInt32(Request.Form["stock"].ToString());
            double precio = Convert.ToDouble(Request.Form["precio"].ToString());
            string detalles = Request.Form["detalles"].ToString();
            HomeController.db.CrearArticulo(nombre, categoria, stock, precio, detalles);
            ViewBag.Info = "<p class='mt-3 text-center text-success'>Articulo creado</p>";
            return View("~/Views/Shared/ArticleRegister.cshtml");
        }

        public ActionResult DeleteArticle(string id)
        {
            Articulo articulo = HomeController.db.BorrarArticulo(Convert.ToInt32(id));
            ViewBag.Articulos = HomeController.db.ConsultarArticulos();
            ViewBag.HayArticulos = HomeController.db.ConsultarSiHayArticulos();
            ViewBag.Borrado = "<p class='mt-3 mb-3 text-center'>Se ha borrado el articulo " + articulo.Nombre + "</p>";
            return View("~/Views/Shared/ArticlesTable.cshtml");
        }

        public ActionResult CambiarStock(string id, string cantidad)
        {
            HomeController.db.CambiarStock(Convert.ToInt32(id), Convert.ToInt32(cantidad));
            ViewBag.HayArticulos = HomeController.db.ConsultarSiHayArticulos();
            ViewBag.Articulos = HomeController.db.ConsultarArticulos();
            
            return View("~/Views/Shared/ArticlesTable.cshtml");
        }

        public ActionResult Comprar(string id, string cantidad)
        {
            HomeController.db.CambiarStock(Convert.ToInt32(id), Convert.ToInt32(cantidad));
            ViewBag.Articulos = HomeController.db.ConsultarArticulos();
            ViewBag.HayArticulos = HomeController.db.ConsultarSiHayArticulos();
            return View("~/Views/Shared/UserShop.cshtml");
        }



    }
}