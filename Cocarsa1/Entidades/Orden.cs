using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cocarsa1.Entidades
{
    class Orden
    {
        private int idNota;
        private int idProducto;
        private Double precioVenta;
        private Double cantidad;
        private Double importe;

        public int IdNota {
            get { return idNota; }
            set { idNota = value; }
        }

        public int IdProducto {
            get { return idProducto; }
            set { idProducto = value; }
        }

        public Double PrecioVenta {
            get { return precioVenta; }
            set { precioVenta = value; }
        }

        public Double Cantidad {
            get { return cantidad; }
            set { cantidad = value; }
        }

        public Double Importe {
            get { return importe; }
            set { importe = value; }
        }
    }
}
