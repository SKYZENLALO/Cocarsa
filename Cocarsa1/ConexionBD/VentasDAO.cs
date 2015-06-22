using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cocarsa1.Entidades;
using MySql.Data.MySqlClient;

namespace Cocarsa1.ConexionBD
{
    class VentasDAO
    {

        public String nombreC;
        public String nombreP;

        MySqlDataReader consulta;

        public VentaNota buscarFolio(int folio) {
            VentaNota notaFolio = new VentaNota();

            MySqlConnection conn;
            Conexion conexion = new Conexion();
            conn = conexion.abrirConexion();

            String query = "SELECT * from`cocarsa`.`nota`  WHERE fechaVenta = CURRENT_DATE() and folioNota = ?folio;";
            try
            {
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("?folio", folio);
                consulta = cmd.ExecuteReader();
                cmd.Dispose();

                if (consulta.Read())
                {
                    notaFolio.IdNota = consulta.GetInt32(0);
                    notaFolio.IdCliente = consulta.GetInt32(1);
                    notaFolio.FolioNota = consulta.GetInt32(2);
                    notaFolio.FechaVenta = consulta.GetDateTime(3);
                    notaFolio.Subtotal = consulta.GetDouble(4);
                    notaFolio.Iva = consulta.GetDouble(5);
                    notaFolio.Total = consulta.GetDouble(6);
                    notaFolio.Liquidada = consulta.GetBoolean(7);
                    notaFolio.Adeudo = consulta.GetDouble(8);
                    notaFolio.Estado = consulta.GetInt32(9);
                }
                else
                {
                    notaFolio = null;
                }
            }
            finally
            {
                conexion.cerrarConexion();
            }

            return notaFolio; 
        }

        public OrdenNota[] cargarNota(int idNota) {
            OrdenNota[] notaDesc = new OrdenNota[12];
            int i=0;
            MySqlConnection conn;
            Conexion conexion = new Conexion();
            conn = conexion.abrirConexion();

            String query = "SELECT * FROM `cocarsa`.`ordenventa` WHERE idNota = ?idNota;";
            try
            {
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("?idNota", idNota);
                consulta = cmd.ExecuteReader();
                cmd.Dispose();

                while (consulta.Read())
                {
                    notaDesc[i] = new OrdenNota();
                    
                    notaDesc[i].IdOrden = consulta.GetInt32(0);
                    notaDesc[i].IdNota = consulta.GetInt32(1);
                    notaDesc[i].IdProducto = consulta.GetInt32(2);
                    notaDesc[i].PrecioVenta = consulta.GetDouble(3);
                    notaDesc[i].Cantidad = consulta.GetDouble(4);
                    notaDesc[i].Importe = consulta.GetDouble(5);
                    i++;
                }
            }
            finally
            {
                conexion.cerrarConexion();
            }
            
            return notaDesc;
        }
        public Boolean updateVenta(VentaNota ventaNota)
        {
            Boolean ans = false;
            MySqlConnection conn;
            Conexion conexion = new Conexion();
            conn = conexion.abrirConexion();

            String query = "UPDATE nota SET idCliente = ?idCliente ,subtotal=?subtotal ,IVA=?IVA , total=?total ,liquidada=?liquidada ,adeudo=?adeudo ,estado=?estado WHERE idNota = ?idNota;";
            try
            {
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("?idCliente", ventaNota.IdCliente);
                cmd.Parameters.AddWithValue("?subtotal", ventaNota.Subtotal);
                cmd.Parameters.AddWithValue("?IVA", ventaNota.Iva);
                cmd.Parameters.AddWithValue("?total", ventaNota.Total);
                cmd.Parameters.AddWithValue("?liquidada", ventaNota.Liquidada);
                cmd.Parameters.AddWithValue("?adeudo", ventaNota.Adeudo);
                cmd.Parameters.AddWithValue("?estado", ventaNota.Estado);
                cmd.Parameters.AddWithValue("?idNota", ventaNota.IdNota);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                ans = true;
            }         
            finally
            {
                conexion.cerrarConexion();
            }
            return ans;
        }
        public Boolean borrarOrden(int idNota)
        {
            Boolean ans = false;
            MySqlConnection conn;
            Conexion conexion = new Conexion();
            conn = conexion.abrirConexion();

            String query = "DELETE FROM ordenVenta WHERE idNota=?idNota;";
            try
            {
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("?idNota", idNota);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                ans = true;
            }
            finally
            {
                conexion.cerrarConexion();
            }
            return ans;
        }

        public int nuevoFolio() {
            int folio = 0;
            MySqlConnection conn;
            Conexion conexion = new Conexion();
            conn=conexion.abrirConexion();
            
            String query = "SELECT folioNota from`cocarsa`.`nota`  WHERE fechaVenta = CURRENT_DATE() order by folioNota desc limit 1;";
            try
            {
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                consulta = cmd.ExecuteReader();
                while (consulta.Read())
                {
                    folio = consulta.GetInt32(0);
                }
            }
            finally
            {
                conexion.cerrarConexion();
            }

            folio = folio + 1;
            return folio;
        }

        public ProductoE cargarProducto(int idProducto){
            ProductoE producto = new ProductoE();

            MySqlConnection conn;
            Conexion conexion = new Conexion();
            conn = conexion.abrirConexion();

            String query = "SELECT * from`cocarsa`.`producto`  WHERE idProducto = ?idProducto;";
            try
            {
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("?idProducto", idProducto);
                consulta = cmd.ExecuteReader();
                cmd.Dispose();

                if (consulta.Read())
                {
                    producto.IdProducto = consulta.GetInt32(0);
                    producto.Nombre = consulta.GetString(1);
                    producto.PrecioVenta = consulta.GetDouble(2);
                }
                else {
                    producto = null;
                }
            }
            finally
            {
                conexion.cerrarConexion();
            }
            return producto;
        }

        public String nombreProducto(int idProducto)
        {
            nombreP = "";
            MySqlConnection conn;
            Conexion conexion = new Conexion();
            conn = conexion.abrirConexion();

            String query = "SELECT nombre from`cocarsa`.`producto`  WHERE idProducto = ?idProducto;";
            try
            {
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("?idProducto", idProducto);
                consulta = cmd.ExecuteReader();
                cmd.Dispose();

                while (consulta.Read())
                {
                    nombreP = consulta.GetString(0);
                }
            }
            finally
            {
                conexion.cerrarConexion();
            }
            return nombreP;
        }

        public String nombreCliente(int idCliente) {
            nombreC = "";
            MySqlConnection conn;
            Conexion conexion = new Conexion();
            conn = conexion.abrirConexion();

            String query = "SELECT nombre, aPaterno, aMaterno from`cocarsa`.`cliente`  WHERE idCliente = ?idCliente;";
            try
            {
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("?idCliente", idCliente);
                consulta = cmd.ExecuteReader();
                cmd.Dispose();

                while (consulta.Read())
                {
                    nombreC += consulta.GetString(0);
                    nombreC += " "+consulta.GetString(1);
                    nombreC += " "+consulta.GetString(2);
                }
            }
            finally
            {
                conexion.cerrarConexion();
            }
            return nombreC;
        }

        public Boolean insertarOrden(OrdenNota[] ordenNota) {
            Boolean ans = false;
            MySqlConnection conn;
            Conexion conexion = new Conexion();
            conn = conexion.abrirConexion();
            int i = 0;

            String query = "INSERT INTO ordenVenta (idNota, idProducto, precioVenta, cantidad, importe) VALUES (?idNota, ?idProducto, ?precioVenta, ?cantidad, ?importe);";
            try
            {
                while (ordenNota[i] != null){
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("?idNota", ordenNota[i].IdNota);
                    cmd.Parameters.AddWithValue("?idProducto", ordenNota[i].IdProducto);
                    cmd.Parameters.AddWithValue("?precioVenta", ordenNota[i].PrecioVenta);
                    cmd.Parameters.AddWithValue("?cantidad", ordenNota[i].Cantidad);
                    cmd.Parameters.AddWithValue("?importe", ordenNota[i].Importe);
                    cmd.ExecuteNonQuery();
                    //cmd.Dispose();
                    i++;
                    ans = true;
                }
            }
            finally
            {
                conexion.cerrarConexion();
            }
            return ans;
        }

        public int insertarVenta(VentaNota ventaNota) {

            int ans = 0;
            MySqlConnection conn;
            Conexion conexion = new Conexion();
            conn = conexion.abrirConexion();

            String query = "INSERT INTO nota (idCliente, folioNota, fechaVenta, subtotal, IVA, total, liquidada, adeudo, estado) VALUES (?idCliente, ?folioNota, current_date(), ?subtotal, ?IVA, ?total, ?liquidada, ?aduedo, ?estado);";
            try
            {
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("?idCliente", ventaNota.IdCliente);
                cmd.Parameters.AddWithValue("?folioNota", ventaNota.FolioNota);
                cmd.Parameters.AddWithValue("?subtotal", ventaNota.Subtotal);
                cmd.Parameters.AddWithValue("?IVA", ventaNota.Iva);
                cmd.Parameters.AddWithValue("?total", ventaNota.Total);
                cmd.Parameters.AddWithValue("?liquidada", ventaNota.Liquidada);
                cmd.Parameters.AddWithValue("?aduedo", ventaNota.Adeudo);
                cmd.Parameters.AddWithValue("?estado", ventaNota.Estado);
                cmd.ExecuteNonQuery();
                //cmd.Dispose();

                query = "SELECT idNota FROM `cocarsa`.`nota` order by  idNota desc limit 1;";
                MySqlCommand cmd2 = new MySqlCommand(query, conn);
                consulta= cmd2.ExecuteReader();
                cmd2.Dispose();
                if (consulta.Read())
                {
                    ans = consulta.GetInt32(0);
                }
            }
            finally
            {
                conexion.cerrarConexion();
            }
            return ans;
        }
    
    }
}
