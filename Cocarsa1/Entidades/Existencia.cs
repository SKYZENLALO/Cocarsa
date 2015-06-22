using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cocarsa1.Entidades
{
    class Existencia
    {
        private int idRegistro;
        private int idProducto;
        private DateTime fecha;
        private Double cantidad;

        public int IdRegistro {
            get { return idRegistro; }
            set { idProducto = value; }
        }

        public int IdProducto {
            get { return idProducto; }
            set { idProducto = value; }
        }

        public DateTime Fecha {
            get { return fecha; }
            set { fecha = value; }
        }
        public Double Cantidad {
            get { return cantidad; }
            set { cantidad = value; }
        }
    }
}
