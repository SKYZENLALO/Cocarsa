using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Cocarsa1.Entidades;


namespace Cocarsa1.ConexionBD
{
    class FajillasDAO
    {

        MySqlDataReader consulta;
        public String nombreC;

        public String nombreCajera(int idCajera) {
            nombreC = "";
            MySqlConnection conn;
            Conexion conexion = new Conexion();
            conn = conexion.abrirConexion();
            String query = "SELECT nombre, aPaterno, aMaterno from`cocarsa`.`cajera`  WHERE idCajera = ?idCajera;";
            try
            {
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("?idCajera", idCajera);
                consulta = cmd.ExecuteReader();
                cmd.Dispose();

                while (consulta.Read())
                {
                    nombreC += consulta.GetString(0);
                    nombreC += " " + consulta.GetString(1);
                    nombreC += " " + consulta.GetString(2);
                }
            }
            finally
            {
                conexion.cerrarConexion();
            }
            return nombreC;
        }

        public int numeroCajeras() {
            int totalCajeras = 0;
            MySqlConnection conn;
            Conexion conexion = new Conexion();
            conn = conexion.abrirConexion();

            String query = "SELECT count(*) FROM cajera;";
            try
            {
                MySqlCommand cmd = new MySqlCommand(query, conn);
                consulta = cmd.ExecuteReader();
                cmd.Dispose();

                if (consulta.Read())
                {
                    totalCajeras = consulta.GetInt32(0);
                }
                else
                {
                    totalCajeras = 0;
                }
            }
            finally
            {
                conexion.cerrarConexion();
            }            
            return totalCajeras;
        }

        public Cajera[] cajeras(int numCajeras) {
            Cajera[] cajera = new Cajera[numCajeras];
            int i = 0;
            MySqlConnection conn;
            Conexion conexion = new Conexion();
            conn = conexion.abrirConexion();

            String query = "SELECT * FROM cocarsa.cajera;";
            try
            {
                MySqlCommand cmd = new MySqlCommand(query, conn);
                consulta = cmd.ExecuteReader();
                cmd.Dispose();

                while (consulta.Read())
                {
                    cajera[i] = new Cajera();
                    cajera[i].IdCajera = consulta.GetInt32(0);
                    cajera[i].Nombre = consulta.GetString(1);
                    cajera[i].APaterno = consulta.GetString(2);
                    cajera[i].AMaterno = consulta.GetString(3);
                    i++;
                }
            }
            finally
            {
                conexion.cerrarConexion();
            }            
            return cajera;
        }

        public int numeroFajillas(String fecha, int opc)
        {
            int totalFajillas = 0;
            String query="";
            MySqlConnection conn;
            Conexion conexion = new Conexion();
            conn = conexion.abrirConexion();
            switch(opc){
                case 1:             //Busqueda Fajillas por dia 
                    query = "SELECT count(*) FROM cocarsa.fajilla WHERE fecha = ?fecha;";
                    break;
                case 2:             //Busqueda Fajillas en Caja
                    query = "SELECT count(*) FROM cocarsa.fajilla WHERE enCaja = TRUE;";
                    break;
                case  3:
                    query = "SELECT count(*) FROM cocarsa.fajilla WHERE fechaCorte = ?fecha;";
                    break;
            }
            try
            {
                MySqlCommand cmd = new MySqlCommand(query, conn);
                if (opc != 2){
                    cmd.Parameters.AddWithValue("?fecha", fecha);
                }
                consulta = cmd.ExecuteReader();
                cmd.Dispose();

                if (consulta.Read())
                {
                    totalFajillas = consulta.GetInt32(0);
                }
            }
            finally
            {
                conexion.cerrarConexion();
            }
            return totalFajillas;
        }

        public Fajilla[] fajillas(int numFajillas, String fecha, int opc) {
            Fajilla[] fajilla = new Fajilla[numFajillas];
            int i = 0;
            String query = "";
            MySqlConnection conn;
            Conexion conexion = new Conexion();
            conn = conexion.abrirConexion();
            switch (opc) { 
                case 1:
                    query = "SELECT * FROM cocarsa.fajilla WHERE fecha = ?fecha;";
                    break;
                case 2:
                    query = "SELECT * FROM cocarsa.fajilla WHERE enCaja = TRUE;";
                    break;
                case 3:
                    query = "SELECT * FROM cocarsa.fajilla WHERE fechaCorte = ?fecha;";
                    break;
            }
            
            try
            {
                MySqlCommand cmd = new MySqlCommand(query, conn);
                if (opc != 2)
                {
                    cmd.Parameters.AddWithValue("?fecha", fecha);
                }
                consulta = cmd.ExecuteReader();
                cmd.Dispose();

                while (consulta.Read())
                {
                    fajilla[i] = new Fajilla();
                    fajilla[i].IdFajilla = consulta.GetInt32(0);
                    fajilla[i].IdCajera = consulta.GetInt32(1);
                    fajilla[i].Monto = consulta.GetDouble(2);
                    fajilla[i].FechaRegistro = consulta.GetDateTime(3);
                    fajilla[i].EnCaja = consulta.GetBoolean(4);
                    try
                    {
                        fajilla[i].FechaCorte = consulta.GetDateTime(5);
                    }
                    catch (Exception error) {
                        fajilla[i].FechaCorte = Convert.ToDateTime("21-sep-1992");
                    }
                    i++;
                }
            }
            finally
            {
                conexion.cerrarConexion();
            }
            return fajilla;
        }

        public Boolean insertarFajillas(Fajilla fajilla)
        {
            Boolean resp = false;
            MySqlConnection conn;
            Conexion conexion = new Conexion();
            conn = conexion.abrirConexion();

            String query = "INSERT INTO fajilla (idCajera, monto, fecha, enCaja) VALUES (?idCajera, ?monto, current_date(), TRUE);";
            try
            {
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("?idCajera", fajilla.IdCajera);
                cmd.Parameters.AddWithValue("?monto", fajilla.Monto);
                cmd.ExecuteNonQuery();
                resp = true;
                //cmd.Dispose();
            }
            finally
            {
                conexion.cerrarConexion();
            }
            return resp;
        }

        public Boolean corteCaja() {
            Boolean resp = false;
            MySqlConnection conn;
            Conexion conexion = new Conexion();
            conn = conexion.abrirConexion();

            String query = "UPDATE fajilla SET enCaja = false, fechaCorte = current_date() WHERE enCaja = true;";
            try
            {
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                resp = true;
            }
            finally
            {
                conexion.cerrarConexion();
            }
            return resp;
        }
    }
}
