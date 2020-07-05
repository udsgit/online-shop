using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Proyecto.Models
{
    public class Articulo
    {
        public int ID { get; set; }
        public string Nombre { get; set; }
        public string Categoria { get; set; }
        public int Stock { get; set; }
        public double Precio { get; set; }
        public string Detalles { get; set; }
        public string Imagen { get; set; }
    }

}