using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cocarsa1.Entidades;
using MySql.Data.MySqlClient;

namespace Cocarsa1.ConexionBD
{
    class ProductoDao
    {
        public int obtenerIdProducto() {
            int ans = -1;

            Conexion bd = new Conexion();
            String query = "SELECT idProducto FROM producto ORDER BY idProducto desc limit 1";

            MySqlCommand cmd = new MySqlCommand(query, bd.abrirConexion());
            MySqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read()) {
                ans = reader.GetInt32("idProducto");
                ans++;
            }

            bd.cerrarConexion();
            return ans;
        }
        
        public ProductoE obtenerProducto(int idProducto) {
            ProductoE producto = null;

            Conexion bd = new Conexion();
            String query = "SELECT * FROM producto WHERE idProducto = ?idProducto";

            MySqlCommand cmd = new MySqlCommand(query, bd.abrirConexion());
            cmd.Parameters.AddWithValue("?idProducto", idProducto);

            MySqlDataReader reader = cmd.ExecuteReader();
            if(reader.Read()) 
            {
                producto = new ProductoE();
                producto.IdProducto = reader.GetInt32("idProducto");
                producto.Nombre = reader.GetString("nombre");
                producto.PrecioVenta = reader.GetDouble("precioVenta");
            }

            return producto;
        }

        public int registrarProducto(ProductoE producto) {
            int ans = -1;

            Conexion bd = new Conexion();
            String query = "INSERT INTO producto(idProducto, nombre, precioVenta) VALUES(?idProducto, ?nombre, ?precioVenta)";

            MySqlCommand cmd = new MySqlCommand(query, bd.abrirConexion());
            cmd.Parameters.AddWithValue("idProducto", producto.IdProducto);
            cmd.Parameters.AddWithValue("nombre", producto.Nombre);
            cmd.Parameters.AddWithValue("precioVenta", producto.PrecioVenta);

            ans = cmd.ExecuteNonQuery();

            return ans;
        }

        public int guardarCambios(ProductoE producto)
        {
            int ans = -1;

            Conexion bd = new Conexion();
            String query = "UPDATE producto SET nombre=?nombre, precioVenta=?precioVenta WHERE idProducto=?idProducto";

            MySqlCommand cmd = new MySqlCommand(query, bd.abrirConexion());

            cmd.Parameters.AddWithValue("idProducto", producto.IdProducto);
            cmd.Parameters.AddWithValue("nombre", producto.Nombre);
            cmd.Parameters.AddWithValue("precioVenta", producto.PrecioVenta);

            ans = cmd.ExecuteNonQuery();

            return ans;
        }

        public List<HistorialPrecio> historialCambioPrecios(int mes, int annio) {

            List<HistorialPrecio> historial = new List<HistorialPrecio>();

            Conexion bd = new Conexion();
            String query = "SELECT t1.IdProducto, t1.fechaCambioPrecio, t1.precioProducto, t2.nombre, t2.precioVenta " +
                            "FROM historialprecio t1 INNER JOIN producto t2 ON t1.IdProducto = t2.IdProducto " +
                            "WHERE MONTH(t1.fechaCambioPrecio) = ?mes AND YEAR(t1.fechaCambioPrecio) = ?annio;";

            try
            {
                MySqlCommand cmd = new MySqlCommand(query, bd.abrirConexion());
                cmd.Parameters.AddWithValue("mes", mes);
                cmd.Parameters.AddWithValue("annio", annio);

                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    HistorialPrecio registro = new HistorialPrecio();

                    registro.IdProducto = reader.GetInt32("IdProducto");
                    registro.Fecha = reader.GetDateTime("fechaCambioPrecio");
                    registro.PrecioAnterior = reader.GetDouble("precioProducto");
                    registro.Producto = reader.GetString("nombre");
                    registro.PrecioActual = reader.GetDouble("precioVenta");

                    historial.Add(registro);
                }
            }catch(Exception e) {
                System.Windows.Forms.MessageBox.Show("Error de conexion a la base de datos");
            }
            return historial;
        }
    }
}
