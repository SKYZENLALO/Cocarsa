using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cocarsa1.Entidades
{
    class Cajera
    {
        private int idCajera;
        private String nombre;
        private String aPaterno;
        private String aMaterno;

        public int IdCajera {
            get { return idCajera; }
            set { idCajera = value; }
        }

        public String Nombre {
            get { return nombre; }
            set { nombre = value; }
        }

        public String APaterno {
            get { return aPaterno; }
            set { aPaterno = value; }
        }

        public String AMaterno {
            get { return aMaterno; }
            set { aMaterno = value; }
        }
    }
}
