using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Proyecto.Models
{
    public class ArticulosViewModel
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