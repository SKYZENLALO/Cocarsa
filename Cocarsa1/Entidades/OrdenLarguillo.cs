using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cocarsa1.Entidades
{
    class OrdenLarguillo : Orden
    {
        private int idOrden;

        public int IdOrden
        {
            get { return idOrden; }
            set { idOrden = value; }
        }  
    }
}
