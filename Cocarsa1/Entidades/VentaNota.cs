using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cocarsa1.Entidades
{
    class VentaNota : Venta
    {
        private int idNota;

        public int IdNota {
            get { return idNota; }
            set { idNota = value; }
        }

        public VentaNota() { 
        
        }

        public VentaNota(int idNota) {
            this.idNota = idNota;
        }
    }
}
