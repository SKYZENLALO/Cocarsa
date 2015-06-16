using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cocarsa1.Entidades;
using MySql.Data.MySqlClient;
using System.Data;
using System.Windows.Forms;

namespace Cocarsa1.ConexionBD
{
    class ReportesDAO
    {
        MySqlDataReader consulta;

        public OrdenNota[] consultaOrden(int idInicio, int idFinal) {
            OrdenNota[] ordenNota=new OrdenNota[60];
            String query = "SELECT IdProducto,sum(cantidad),sum(importe) FROM cocarsa.ordenventa where idNota > 77&& idNota <92 group by IdProducto;";
            return ordenNota;
        }

        public VentaNota[] consultaNota (String fechaVenta, int numeroReg){
            VentaNota[] ventaNota = new VentaNota[numeroReg];
            MySqlConnection conn;
            Conexion conexion = new Conexion();
            conn = conexion.abrirConexion();
            int i = 0;
            String query = "SELECT folioNota,idCliente,subtotal,IVA,total,estado,idNota FROM nota WHERE fechaVenta=?fechaVenta;";
            try
            {
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("?fechaVenta", fechaVenta);
                consulta = cmd.ExecuteReader();
                cmd.Dispose();
                while (consulta.Read()){
                    ventaNota[i]=new VentaNota();
                    ventaNota[i].FolioNota=consulta.GetInt32(0);
                    ventaNota[i].IdCliente=consulta.GetInt32(1);
                    ventaNota[i].Subtotal=consulta.GetDouble(2);
                    ventaNota[i].Iva=consulta.GetDouble(3);
                    ventaNota[i].Total=consulta.GetDouble(4);
                    ventaNota[i].Estado=consulta.GetInt32(5);
                    ventaNota[i].IdNota = consulta.GetInt32(6);
                    i++;
                }
            }
            finally
            {
                conexion.cerrarConexion();
            }
            return ventaNota;
        }

        public int numeroRegistros(String fechaVenta)
        {
            int cantidadRegistros = 0;
            MySqlConnection conn;
            Conexion conexion = new Conexion();
            conn = conexion.abrirConexion();
            String query = "SELECT count(*) FROM nota WHERE fechaVenta=?fechaVenta;";
            try
            {
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("?fechaVenta", fechaVenta);
                consulta = cmd.ExecuteReader();
                cmd.Dispose();
                if (consulta.Read())
                {
                    cantidadRegistros = consulta.GetInt32(0);
                }
                else
                {
                    cantidadRegistros = 0;
                }
            }
            finally
            {
                conexion.cerrarConexion();
            }
            return cantidadRegistros;
        }
    }
}
