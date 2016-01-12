using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MedNeg.Ensambles
{
    public class EnsambleProductos
    {
        public EnsambleProductos() { 
        }

        public int idEnsamble { get; set; }
        public int idProducto { get; set; }
        public int cantidad { get; set; }
        public string clave1 { get; set; }
        public string nombre { get; set; }
        public string presentacion { get; set; }
        public string precioPublico { get; set; }
    }
}
