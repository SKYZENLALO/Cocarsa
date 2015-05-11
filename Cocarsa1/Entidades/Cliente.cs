using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cocarsa1.Entidades
{
    public class Cliente
    {
        private int idCliente;
        private String nombre;
        private String aPaterno;
        private String aMaterno;
        private String rfc;
        private String correo;
        private String telefono;
        private String celular;
        private String calle;
        private String colonia;
        private String estado;
        private String municipio;
        private String numExt;
        private String numInt;

        public int IdCliente {
            get { return idCliente; }
            set { idCliente = value; }
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

        public String RFC {
            get { return rfc; }
            set { rfc = value; }
        }

        public String Correo {
            get { return correo; }
            set { correo = value; }
        }

        public String Telefono {
            get { return telefono; }
            set { telefono = value; }
        }

        public String Celular {
            get { return celular; }
            set { celular = value; }
        }

        public String Calle {
            get { return calle; }
            set { calle = value; }
        }

        public String Colonia {
            get { return colonia; }
            set { colonia = value; }
        }

        public String Estado {
            get { return estado; }
            set { estado = value; }
        }

        public String Municipio {
            get { return municipio; }
            set { municipio = value; }
        }

        public String NumExt {
            get { return numExt; }
            set { numExt = value; }
        }

        public String NumInt {
            get { return numInt; }
            set { numInt = value; }
        }
    }
}
