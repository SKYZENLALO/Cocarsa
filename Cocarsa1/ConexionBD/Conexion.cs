﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Cocarsa1.ConexionBD
{
    class Conexion
    {
        static String conn = "SERVER=localhost; DATABASE=cocarsa; UID=root; PASSWORD=pass";

        public MySqlConnection conexion = null;

        public MySqlConnection abrirConexion()
        {
            try
            {
                conexion = new MySqlConnection(conn);
                conexion.Open();
                return conexion;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public MySqlDataReader obtenerDatos(String query)
        {

            MySqlCommand cmd = null;

            try
            {
                cmd = new MySqlCommand(query, conexion);
                return cmd.ExecuteReader();
            }
            catch (Exception e)
            {
            }
            return null;
        }

        public Boolean cerrarConexion()
        {
            try
            {
                conexion.Close();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }


    }
}
