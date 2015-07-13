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
        private String datasource = "SERVER = " + Properties.Settings.Default.server + ";" + 
                                    "DATABASE = "+ Properties.Settings.Default.database +";"+
                                    "UID = " + Properties.Settings.Default.username + ";" +
                                    "PASSWORD = " + Properties.Settings.Default.password;
        private String query = "";
        
        private void guardaRegistroExistencia(MySqlConnection conexion, Existencia registro)
        {
            query = "INSERT INTO existencia(idProducto,fecha,cantidad) VALUES(?idProducto,?fecha,?cantidad);";

            using (MySqlCommand cmd = new MySqlCommand(query, conexion))
            {
                cmd.Parameters.AddWithValue("?idProducto", registro.IdProducto);
                cmd.Parameters.AddWithValue("?fecha", registro.Fecha);
                cmd.Parameters.AddWithValue("?cantidad", registro.Cantidad);

                cmd.ExecuteNonQuery();
            }
        }
        
        public int guardaRegistroFrio(Existencia salida) 
        {
            int ans = -1, id_registro = 0;
            Double existencia_cantidad = 0;

            try 
            {
                using (var conexion = new MySqlConnection(datasource)) 
                {
                    conexion.Open();
                    query = "SELECT idRegistro, cantidad FROM existencia WHERE idProducto = ?idProducto AND fecha = ?fecha;";

                    using (MySqlCommand cmd = new MySqlCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("?idProducto", salida.IdProducto);
                        cmd.Parameters.AddWithValue("?fecha", DateTime.Now.Date);

                        using(MySqlDataReader reader = cmd.ExecuteReader()) 
                        {
                            if (reader.Read())
                            {
                                id_registro = reader.GetInt32("idRegistro");
                                existencia_cantidad = reader.GetDouble("cantidad");                                
                            }
                            else {                                
                                conexion.Close();
                                return 0;
                            }
                        }
                    }

                    existencia_cantidad -= salida.Cantidad;
                    if (existencia_cantidad < 0) {
                        conexion.Close();
                        return 1;
                    }

                    query = "UPDATE existencia SET cantidad = ?cantidad WHERE idRegistro = ?idRegistro;";

                    using (MySqlCommand cmd = new MySqlCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("?cantidad", existencia_cantidad);
                        cmd.Parameters.AddWithValue("?idRegistro", id_registro);                        

                        cmd.ExecuteNonQuery();                        
                    }

                    query = "INSERT INTO frio(idProducto,fecha,cantidad) VALUES(?idProducto,?fecha,?cantidad);";

                    using (MySqlCommand cmd = new MySqlCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("?idProducto", salida.IdProducto);
                        cmd.Parameters.AddWithValue("?fecha", salida.Fecha);
                        cmd.Parameters.AddWithValue("?cantidad", salida.Cantidad);

                        cmd.ExecuteNonQuery();
                        ans = 2;
                    }
                }
            } 
            catch(MySqlException e) 
            {
                e.ToString();
                ans = -1;
            }
            return ans;
        }

        public Boolean guardaRegistroFresco(Existencia entrada)
        {
            Boolean ans = false;
            try
            {
                using (var conexion = new MySqlConnection(datasource))
                {
                    conexion.Open();
                    query = "INSERT INTO fresco(idProducto,fecha,cantidad) VALUES(?idProducto,?fecha,?cantidad);";

                    using (MySqlCommand cmd = new MySqlCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("?idProducto", entrada.IdProducto);
                        cmd.Parameters.AddWithValue("?fecha", entrada.Fecha);
                        cmd.Parameters.AddWithValue("?cantidad", entrada.Cantidad);

                        cmd.ExecuteNonQuery();
                        ans = true;
                    }
                }
            }
            catch (MySqlException e)
            {
                e.ToString();
                ans = false;
            }
            return ans;
        }

        public void copiaRegistroExistencia() 
        {
            try
            {
                List<Existencia> existencia = new List<Existencia>();

                using (var conexion = new MySqlConnection(datasource))
                {
                    conexion.Open();
                    query = "SELECT idRegistro FROM existencia where fecha = current_date() limit 1;";

                    using(MySqlCommand cmd = new MySqlCommand(query, conexion)) 
                    {
                        using(MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read()) {
                                return;
                            }
                        }
                    }                    
                }

                using (var conexion = new MySqlConnection(datasource))
                {
                    conexion.Open();
                    query = "SELECT idProducto, cantidad FROM existencia WHERE fecha = ?fecha;";

                    using (MySqlCommand cmd = new MySqlCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("?fecha", DateTime.Now.Date.AddDays(-1));
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Existencia registro = new Existencia();
                                registro.IdProducto = reader.GetInt32("idProducto");
                                registro.Cantidad = reader.GetDouble("cantidad");
                                registro.Fecha = DateTime.Now;
                                existencia.Add(registro);
                            }
                        }
                    }                     
                }

                using (var conexion = new MySqlConnection(datasource))
                {
                    conexion.Open();
                    using (MySqlTransaction tx = conexion.BeginTransaction())
                    {
                        try
                        {
                            foreach(Existencia registro in existencia)
                            {
                                guardaRegistroExistencia(conexion, registro);
                            }
                            tx.Commit();
                        }
                        catch (MySqlException e)
                        {
                            tx.Rollback();
                            Console.WriteLine(e.ToString());
                        }
                    }
                }
                
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e.ToString());               
            }
        }
    }
}
