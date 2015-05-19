using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cocarsa1.Entidades
{
    class Venta
    {
        private int idGeneral;
        private int idCliente;
        private int folioNota;
        private DateTime fechaVenta;
        private Double subtotal;
        private Double iva;
        private Double total;
        private Boolean liquidada;
        private Double adeudo;
        private int estado;

        public int IdGeneral {
            get { return idGeneral; }
            set { idGeneral = value; }
        }

        public int IdCliente {
            get { return idCliente; }
            set { idCliente = value; }
        }

        public int FolioNota {
            get { return folioNota; }
            set { folioNota = value; }
        }

        public DateTime FechaVenta {
            get { return fechaVenta; }
            set { fechaVenta = value; }
        }

        public Double Subtotal {
            get { return subtotal; }
            set { subtotal = value; }
        }

        public Double Iva {
            get { return iva; }
            set { iva = value; }
        }

        public Double Total {
            get { return total; }
            set { total = value; }
        }

        public Boolean Liquidada {
            get { return liquidada; }
            set { liquidada = value; }
        }
        public Double Adeudo {
            get { return adeudo; }
            set { adeudo = value; }
        }
        public int Estado {
            get { return estado; }
            set { estado = value; }
        }

    }
}
