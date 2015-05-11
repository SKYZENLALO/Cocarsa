using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cocarsa1.Entidades
{
    class VentaLarguillo : Venta
    {
        private int idLarguillo;

        public int IdLarguillo {
            get { return idLarguillo; }
            set { idLarguillo = value; }
        }

        public VentaLarguillo() { }

        public VentaLarguillo(int idLarguillo) {
            this.idLarguillo = idLarguillo;
        }
    }
}
