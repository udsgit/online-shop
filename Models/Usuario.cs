using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace Proyecto.Models
{
    public class Usuario
    {
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public DateTime FechaNacimiento { get; set; }
        [Key]
        public string Email { get; set; }
        public string Password { get; set; }
        public bool Admin { get; set; }
    }


}