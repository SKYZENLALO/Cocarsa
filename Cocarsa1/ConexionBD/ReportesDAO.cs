﻿using System;
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
        public String nombreP;

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

        public int numeroProductos(String excepto)
        {
            int cantidadRegistros = 0;
            MySqlConnection conn;
            Conexion conexion = new Conexion();
            conn = conexion.abrirConexion();
            String query = excepto;
            try
            {
                MySqlCommand cmd = new MySqlCommand(query, conn);
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

        public OrdenNota[] consultaOrden(String excepto, int cantidadProductos) {
            OrdenNota[] ordenNota = new OrdenNota[cantidadProductos];
            MySqlConnection conn;
            Conexion conexion = new Conexion();
            conn = conexion.abrirConexion();
            int i = 0;
            String query = excepto;
            try
            {
                MySqlCommand cmd = new MySqlCommand(query, conn);
                consulta = cmd.ExecuteReader();
                cmd.Dispose();
                while (consulta.Read())
                {
                    ordenNota[i] = new OrdenNota();
                    ordenNota[i].IdProducto = consulta.GetInt32(0);
                    ordenNota[i].Cantidad = consulta.GetDouble(1);
                    ordenNota[i].Importe = consulta.GetDouble(2);
                    i++;
                }
            }
            finally
            {
                conexion.cerrarConexion();
            }
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

        public int numeroConsulta(String fecha, int opc)
        {
            int cantidadRegistros = 0;
            MySqlConnection conn;
            Conexion conexion = new Conexion();
            conn = conexion.abrirConexion();
            String query = ""; 
            switch (opc) { 
                case 0:
                    query= "SELECT count(*) FROM frio WHERE fecha=?fecha;";
                    break;
                case 1:
                    query = "SELECT count(*) FROM fresco WHERE fecha=?fecha;";
                    break;
                case 2:
                    query = "SELECT count(*) FROM existencia WHERE fecha=?fecha;";
                    break;
                    
            }
            try
            {
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("?fecha", fecha);
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

        public Existencia[] consultaExistencia(String fecha, int numReg, int opc) {
            Existencia[] consultasExistencia = new Existencia[numReg];
            String query = "";
            MySqlConnection conn;
            Conexion conexion = new Conexion();
            conn = conexion.abrirConexion();
            int i = 0;
            switch (opc) { 
                case 0:
                    query = "SELECT * FROM frio WHERE fecha=?fecha;";
                    break;
                case  1:
                    query = "SELECT * FROM fresco WHERE fecha=?fecha;";
                    break;
                case 2:
                    query = "SELECT * FROM existencia WHERE fecha=?fecha;";
                    break;
            }
            try
            {
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("?fecha", fecha);
                consulta = cmd.ExecuteReader();
                cmd.Dispose();
                while (consulta.Read())
                {
                    consultasExistencia[i] = new Existencia();
                    consultasExistencia[i].IdRegistro = consulta.GetInt32(0);
                    consultasExistencia[i].IdProducto = consulta.GetInt32(1);
                    consultasExistencia[i].Fecha = consulta.GetDateTime(2);
                    consultasExistencia[i].Cantidad = consulta.GetDouble(3);
                    i++;
                }
            }
            finally
            {
                conexion.cerrarConexion();
            }
            return consultasExistencia;
        }
    }
}
