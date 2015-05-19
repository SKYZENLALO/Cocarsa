using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cocarsa1.Entidades;
using MySql.Data.MySqlClient;

namespace Cocarsa1.ConexionBD
{
    class AbonoDao
    {
        private String tipoPago;
        private Abono pagoAbono;
        private Double deuda;

        public Boolean registrarPago(Abono pagoAbono, Double deuda, String tipoPago) {
            
            this.tipoPago = tipoPago;
            this.pagoAbono = pagoAbono;
            this.deuda = deuda;

            Boolean ans = false;

            Conexion conexion = new Conexion();
            MySqlConnection conn = conexion.abrirConexion();            
            MySqlTransaction tx = null;

            try 
            {
                tx = conn.BeginTransaction();
                
                registrarAbono(conn);
                actualizarDeuda(conn);

                tx.Commit();
            } 
            catch(Exception e) 
            {
                e.ToString();
                tx.Rollback();                
            }

            conexion.cerrarConexion();
            return ans;
        }
        
        public void registrarAbono(MySqlConnection conexion) 
        {
            String query = "";
            
            if (tipoPago.Equals("nota"))
                query = "INSERT INTO pagoAbonoNota VALUES(?idNota,?idCliente,?idCajera,?montoAbono,?fechaAbono)";
            else if(tipoPago.Equals("larguillo"))
                query = "INSERT INTO pagoAbonoLarguillo VALUES(?idLarguillo,?idCliente,?idCajera,?montoAbono,?fechaAbono)";

            MySqlCommand cmd = new MySqlCommand(query, conexion);
            
            if(tipoPago.Equals("nota"))
                cmd.Parameters.AddWithValue("?idNota",pagoAbono.IdFolio);
            else if (tipoPago.Equals("larguillo"))
                cmd.Parameters.AddWithValue("?idLarguillo", pagoAbono.IdFolio);

            cmd.Parameters.AddWithValue("?idCliente", pagoAbono.IdCliente);
            cmd.Parameters.AddWithValue("?idCajera", pagoAbono.IdCajera);
            cmd.Parameters.AddWithValue("?montoAbono", pagoAbono.MontoAbono);
            cmd.Parameters.AddWithValue("?fechaAbono", pagoAbono.FechaAbono);

            cmd.ExecuteNonQuery();
            
        }

        public void actualizarDeuda(MySqlConnection conexion) 
        {
            String query = "";

            if (tipoPago.Equals("nota"))
                query = "UPDATE nota VALUES(?idNota,?idCliente,?idCajera,?montoAbono,?fechaAbono)";
            else if (tipoPago.Equals("larguillo"))
                query = "INSERT INTO pagoAbonoLarguillo VALUES(?idLarguillo,?idCliente,?idCajera,?montoAbono,?fechaAbono)";

            MySqlCommand cmd = new MySqlCommand(query, conexion);
        }
    }
}
