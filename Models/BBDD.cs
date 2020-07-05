using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Proyecto.Models;
using System.Web.Mvc;

namespace Proyecto.Models
{
    public class BBDD : DbContext
    {
        public BBDD() { }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Articulo> Articulos { get; set; }

        public void CrearUsuario(string nombre, string apellidos, DateTime fechaNacimiento, string email, string password, bool admin)
        {
            this.Usuarios.Add(new Usuario { Nombre = nombre, Apellidos = apellidos, FechaNacimiento = fechaNacimiento, Email = email, Password = password, Admin = admin });
            this.SaveChanges();
        }

        public void CrearArticulo(string nombre, string categoria, int stock, double precio, string detalles)
        {
            this.Articulos.Add(new Articulo { Nombre = nombre, Categoria = categoria, Stock = stock, Precio = precio, Detalles = detalles, Imagen = "/Imagenes/" + categoria + ".png" });
            this.SaveChanges();
        }

        public void CrearTresArticulos()
        {
            this.CrearArticulo("Super Impresora", "impresora", 2, 99, "La mejor del mercado, imprime incluso a color");
            this.CrearArticulo("Ipad 6º Generación", "tablet", 4, 299, "Super oferta");
            this.CrearArticulo("PC Gaming", "sobremesa", 1, 6, "Los mejores componentes");
            this.CrearArticulo("Samsung S8", "smartphone", 4, 499, "Tranquilo, no va a explotar");
            this.CrearArticulo("Portatil ASUS ROG", "portatil", 1, 6, "El mejor portatil gaming");
        }


        public void CrearAdmin()
        {
            this.CrearUsuario("Emmanuel", "Gonzalez", DateTime.Parse("17/09/1989"), "admin@gmail.com", "admin", true);
        }

        public IQueryable ConsultarUsuarios()
        {
            var resultado = from e in this.Usuarios
                            select new UsuariosViewModel
                            {
                                Nombre = e.Nombre,
                                Apellidos = e.Apellidos,
                                FechaNacimiento = e.FechaNacimiento,
                                Email = e.Email,
                                Password = e.Password,
                                Admin = e.Admin
                            };
            return resultado;
        }

        public IQueryable ConsultarArticulos()
        {
            var resultado = from e in this.Articulos
                            select new ArticulosViewModel
                            {
                                ID = e.ID,
                                Nombre = e.Nombre,
                                Categoria = e.Categoria,
                                Stock = e.Stock,
                                Precio = e.Precio,
                                Detalles = e.Detalles,
                                Imagen = e.Imagen
                            };
       
            return resultado;
        }

        public bool ConsultarSiHayArticulos()
        {
            var resultado = from e in this.Articulos
                            select new ArticulosViewModel
                            {
                                ID = e.ID,
                                Nombre = e.Nombre,
                                Categoria = e.Categoria,
                                Stock = e.Stock,
                                Precio = e.Precio,
                                Detalles = e.Detalles,
                                Imagen = e.Imagen
                            };
            return resultado.Any() ? true : false;
        }

        public USER ComprobarUsuario(HttpContextBase HttpContext, string email, string password)
        {
            bool existeUsuario, loginCorrecto, existeAdmin;
            if (!Cookie.UsuarioBloqueado(HttpContext, email))
            {
                var resultExisteUsuario = from e in this.Usuarios
                                          where e.Email == email
                                          select e;
                existeUsuario = resultExisteUsuario.Any() ? true : false;
                if (existeUsuario)
                {
                    var resultLoginCorrecto = from e in resultExisteUsuario
                                              where e.Password == password
                                              select e;
                    loginCorrecto = resultLoginCorrecto.Any() ? true : false;
                    if (loginCorrecto)
                    {
                        var listadoAdministradores = from e in resultExisteUsuario
                                                     where e.Admin == true
                                                     select e;
                        existeAdmin = listadoAdministradores.Any() ? true : false;
                        return existeAdmin ? USER.ADMIN : USER.NORMAL;
                    }
                    else
                    {
                        return USER.PASS_INCORRECTO;
                    }
                }
                else
                {
                    return USER.INEXISTENTE;
                }
            }
            else
            {
                return USER.BLOQUEADO;
            }
        }

        public bool ComprobarSiExistenAdministradores()
        {
            var resultado = from e in this.Usuarios
                            where e.Admin == true
                            select e;
            return resultado.Any() ? true : false;
        }

        public IQueryable ConsultaDatosPerfil(string email)
        {

            var resultado = from e in this.Usuarios
                            where e.Email == email
                            select new UsuariosViewModel
                            {
                                Nombre = e.Nombre,
                                Apellidos = e.Apellidos,
                                FechaNacimiento = e.FechaNacimiento,
                                Email = e.Email,
                                Password = e.Password,
                                Admin = e.Admin
                            };

            return resultado;
        }

        public void ActualizarUsuario(string nombre, string apellidos, string email, string password)
        {
            var result = this.Usuarios.SingleOrDefault(e => e.Email == email);
            if (result != null)
            {
                result.Nombre = nombre;
                result.Apellidos = apellidos;
                result.Password = password;
                this.SaveChanges();
            }
        }

        public Usuario BorrarUsuario(string email)
        {
            var result = this.Usuarios.SingleOrDefault(e => e.Email == email);
            if (result != null)
            {
                this.Usuarios.Remove(result);
                this.SaveChanges();
            }
            return result;
        }

        public Articulo BorrarArticulo(int id)
        {
            var result = this.Articulos.SingleOrDefault(e => e.ID == id);
            if (result != null)
            {
                this.Articulos.Remove(result);
                this.SaveChanges();
            }
            return result;
        }

        public void CambiarStock(int id, int cantidad)
        {
            var Articulo = this.Articulos.SingleOrDefault(e => e.ID == id);
            if (Articulo != null)
            {
                if(Articulo.Stock + cantidad >= 0)
                {
                    Articulo.Stock = Articulo.Stock + cantidad;
                    this.SaveChanges();
                }
              
            }
        }
    }


}