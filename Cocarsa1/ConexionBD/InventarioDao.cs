using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cocarsa1.Entidades;
using Cocarsa1.ConexionBD;
using MySql.Data.MySqlClient;

namespace Cocarsa1.ConexionBD
{
    class InventarioDao
    {
        public void agregaRegistroFrio(MySqlConnection conexion, Existencia frio) 
        {            
            String query = "INSERT INTO frio(idProducto,fecha,cantidad) VALUES(?idProducto,?fecha,?cantidad)";

            MySqlCommand cmd = new MySqlCommand(query, conexion);
            cmd.Parameters.AddWithValue("?idProducto", frio.IdProducto);
            cmd.Parameters.AddWithValue("?fecha", frio.Fecha);
            cmd.Parameters.AddWithValue("?cantidad", frio.Cantidad);

            cmd.ExecuteNonQuery();            
        }

        public void agregaRegistroFresco(MySqlConnection conexion, Existencia fresco)
        {
            String query = "INSERT INTO fresco(idProducto,fecha,cantidad) VALUES(?idProducto,?fecha,?cantidad)";

            MySqlCommand cmd = new MySqlCommand(query, conexion);
            cmd.Parameters.AddWithValue("?idProducto", fresco.IdProducto);
            cmd.Parameters.AddWithValue("?fecha", fresco.Fecha);
            cmd.Parameters.AddWithValue("?cantidad", fresco.Cantidad);

            cmd.ExecuteNonQuery();
        }

        public Boolean guardarFrio(List<Existencia> salida) 
        {
            Boolean ans = false;

            MySqlConnection conn = null;
            MySqlTransaction tx = null;

            Conexion conexion = new Conexion();
            conn = conexion.abrirConexion();            

            try {
                
                tx = conn.BeginTransaction();

                foreach(Existencia registro in salida) {
                    agregaRegistroFrio(conn, registro);
                }

                tx.Commit();
                ans = true;

            } catch (Exception e) {
                ans = false;
                tx.Rollback();                
            }
            conexion.cerrarConexion();
            return ans;
        }

        public Boolean guardarFresco(List<Existencia> entrada)
        {
            Boolean ans = false;

            MySqlConnection conn = null;
            MySqlTransaction tx = null;

            Conexion conexion = new Conexion();
            conn = conexion.abrirConexion();

            try
            {

                tx = conn.BeginTransaction();

                foreach (Existencia registro in entrada)
                {
                    agregaRegistroFresco(conn, registro);
                }

                tx.Commit();
                ans = true;

            }
            catch (Exception e)
            {
                ans = false;
                tx.Rollback();
            }
            conexion.cerrarConexion();
            return ans;
        }
    }
}
