using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cocarsa1.Entidades
{
    class Abono
    {
        private int idConcepto;
        private int idCliente;
        private int idCajera;
        private Double montoAbono;
        private DateTime fechaAbono;

        public int IdConcepto
        {
            get { return idConcepto; }
            set { idConcepto = value; }
        }

        public int IdCliente {
            get { return idCliente; }
            set { idCliente = value; }
        }

        public int IdCajera
        {
            get { return idCajera; }
            set { idCajera = value; }
        }

        public Double MontoAbono
        {
            get { return montoAbono; }
            set { montoAbono = value; }
        }

        public DateTime FechaAbono
        {
            get { return fechaAbono; }
            set { fechaAbono = value; }
        }

    }
}
