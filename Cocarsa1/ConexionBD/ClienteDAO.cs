using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cocarsa1.Entidades;
using Cocarsa1.ConexionBD;
using System.Collections;
using MySql.Data.MySqlClient;

namespace Cocarsa1.ConexionBD
{
    class ClienteDao
    {
        public List<Cliente> busquedaClientes(String parametro)
        {
            List<Cliente> listaClientes = new List<Cliente>();
            
            String query = "";
            try
            {
                int idCliente = Convert.ToInt32(parametro);
                query = "SELECT idCliente, nombre, aPaterno, aMaterno, calle, colonia FROM cliente WHERE idCliente = " + idCliente;
            } catch(Exception e){
                query = "SELECT idCliente, nombre, aPaterno, aMaterno, calle, colonia FROM cliente WHERE nombre LIKE '%" + parametro + "%'";
            }

            try
            {
                Conexion conexion = new Conexion();
                
                MySqlCommand cmd = new MySqlCommand(query, conexion.abrirConexion());
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Cliente cliente = new Cliente();
                    cliente.IdCliente = reader.GetInt32("idCliente");
                    cliente.Nombre = reader.GetString("nombre");
                    cliente.APaterno = reader.GetString("aPaterno");
                    cliente.AMaterno = reader.GetString("aMaterno");
                    cliente.Calle = reader.GetString("calle");
                    cliente.Colonia = reader.GetString("colonia");

                    listaClientes.Add(cliente);
                }
                conexion.cerrarConexion();

            } catch(Exception e) {
                System.Windows.Forms.MessageBox.Show("Error : " + e);
            }
            
            return listaClientes;
        }

        public List<Venta> adeudoCliente(int idCliente) {
            List<Venta> deudaCliente = new List<Venta>();

            Conexion conexion = new Conexion();
            MySqlConnection conn = conexion.abrirConexion();

            String query = "SELECT * FROM nota WHERE idCliente = ?idCliente and liquidada = false order by fechaVenta asc";

            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("?idCliente", idCliente);

            MySqlDataReader reader = cmd.ExecuteReader();
            while(reader.Read()) {
                Venta nota = new VentaNota();
                nota.IdGeneral = reader.GetInt32("idNota");
                nota.IdCliente = reader.GetInt32("idCliente");
                nota.FolioNota = reader.GetInt32("folioNota");
                nota.Adeudo = reader.GetDouble("adeudo");
                nota.Estado = reader.GetInt32("estado");
                nota.FechaVenta = reader.GetDateTime("fechaVenta");
                nota.Iva = reader.GetDouble("IVA");
                nota.Subtotal = reader.GetDouble("subtotal");
                nota.Total = reader.GetDouble("total");
                nota.Liquidada = reader.GetBoolean("liquidada");

                deudaCliente.Add(nota);
            }
            reader.Close();
            
            query = "SELECT * FROM larguillo WHERE idCliente = ?idCliente and liquidada = false order by fechaVenta asc";
            cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("?idCliente", idCliente);

            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Venta nota = new VentaLarguillo();
                nota.IdGeneral = reader.GetInt32("idLarguillo");
                nota.IdCliente = reader.GetInt32("idCliente");
                nota.FolioNota = reader.GetInt32("folioLarguillo");
                nota.Adeudo = reader.GetDouble("adeudo");
                nota.FechaVenta = reader.GetDateTime("fechaVenta");
                nota.Iva = reader.GetDouble("IVA");
                nota.Subtotal = reader.GetDouble("subtotal");
                nota.Total = reader.GetDouble("total");
                nota.Liquidada = reader.GetBoolean("liquidada");

                deudaCliente.Add(nota);
            }

            conexion.cerrarConexion();
            return deudaCliente;

        }
    }
}
