using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cocarsa1.Entidades
{
    class Fajilla
    {
        private int idFajilla;
        private int idCajera;
        private Double monto;
        private DateTime fechaRegistro;
        private Boolean enCaja;
        private DateTime fechaCorte;

        public int IdFajilla
        {
            get { return idFajilla; }
            set { idFajilla = value; }
        }

        public int IdCajera
        {
            get { return idCajera; }
            set { idCajera = value; }
        }

        public Double Monto
        {
            get { return monto; }
            set { monto = value; }
        }

        public DateTime FechaRegistro
        {
            get { return fechaRegistro; }
            set { fechaRegistro = value; }
        }

        public Boolean EnCaja
        {
            get { return enCaja; }
            set { enCaja = value; }
        }

        public DateTime FechaCorte
        {
            get { return fechaCorte; }
            set { fechaCorte = value; }
        }

    }
}