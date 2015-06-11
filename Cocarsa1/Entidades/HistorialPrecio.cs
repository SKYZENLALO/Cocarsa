using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cocarsa1.Entidades
{
    class HistorialPrecio
    {
        private int idProducto;
        private String producto;
        private DateTime fecha;
        private Double precioAnterior;
        private Double precioActual;

        public int IdProducto {
            get { return idProducto; }
            set { idProducto = value; }
        }

        public String Producto {
            get { return producto; }
            set { producto = value; }
        }

        public DateTime Fecha {
            get { return fecha; }
            set { fecha = value; }
        }

        public Double PrecioAnterior {
            get { return precioAnterior; }
            set { precioAnterior = value; }
        }

        public Double PrecioActual {
            get { return precioActual; }
            set { precioActual = value; }
        }
            
    }
}
