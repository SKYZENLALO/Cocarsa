using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cocarsa1.Entidades
{
    class ProductoE
    {
        private int idProducto;
        private String nombre;
        private Double precioVenta;

        public int IdProducto
        {
            get { return idProducto; }
            set { idProducto = value; }
        }

        public String Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }
        
        public Double PrecioVenta
        {
            get { return precioVenta; }
            set { precioVenta = value; }
        }

    }
}
