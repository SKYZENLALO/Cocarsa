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

        public long registrarPago(Abono pagoAbono, Double deuda, String tipoPago) {
            
            this.tipoPago = tipoPago;
            this.pagoAbono = pagoAbono;
            this.deuda = deuda;

            long ans = -1;

            Conexion conexion = new Conexion();
            MySqlConnection conn = conexion.abrirConexion();            
            MySqlTransaction tx = null;

            try 
            {
                tx = conn.BeginTransaction();

                actualizarDeuda(conn);
                ans = registrarAbono(conn);                

                tx.Commit();                
            } 
            catch(Exception e) 
            {
                Console.WriteLine(e.ToString());
                tx.Rollback();
                ans = -1;
            }

            conexion.cerrarConexion();
            return ans;
        }
        
        public long registrarAbono(MySqlConnection conexion) 
        {
            String query = "";
            
            if (tipoPago.Equals("nota"))
                query = "INSERT INTO pagoAbonoNota(idNota,idCliente,idCajera,montoAbono,fechaAbono) VALUES(?idNota,?idCliente,?idCajera,?montoAbono,?fechaAbono)";
            else if(tipoPago.Equals("larguillo"))
                query = "INSERT INTO pagoAbonoLarguillo(idLarguillo,idCliente,idCajera,montoAbono,fechaAbono) VALUES(?idLarguillo,?idCliente,?idCajera,?montoAbono,?fechaAbono)";

            MySqlCommand cmd = new MySqlCommand(query, conexion);
            
            if(tipoPago.Equals("nota"))
                cmd.Parameters.AddWithValue("?idNota",pagoAbono.IdGeneral);
            else if (tipoPago.Equals("larguillo"))
                cmd.Parameters.AddWithValue("?idLarguillo", pagoAbono.IdGeneral);

            cmd.Parameters.AddWithValue("?idCliente", pagoAbono.IdCliente);
            cmd.Parameters.AddWithValue("?idCajera", pagoAbono.IdCajera);
            cmd.Parameters.AddWithValue("?montoAbono", pagoAbono.MontoAbono);
            cmd.Parameters.AddWithValue("?fechaAbono", pagoAbono.FechaAbono);

            cmd.ExecuteNonQuery();
            return cmd.LastInsertedId;            
        }

        public void actualizarDeuda(MySqlConnection conexion) 
        {
            String query = "";

            if (tipoPago.Equals("nota"))
                query = "UPDATE nota SET adeudo = ?adeudo, liquidada = ?liquidada WHERE idNota=?idNota";
            else if (tipoPago.Equals("larguillo"))
                query = "UPDATE larguillo SET adeudo = ?adeudo, liquidada = ?liquidada WHERE idLarguillo=?idLarguillo";

            Boolean liquidada = deuda == 0 ? true : false; 

            MySqlCommand cmd = new MySqlCommand(query, conexion);
            if (tipoPago == "nota")
                cmd.Parameters.AddWithValue("?idNota", pagoAbono.IdGeneral);
            else
                cmd.Parameters.AddWithValue("?idLarguillo", pagoAbono.IdGeneral);
            cmd.Parameters.AddWithValue("?adeudo", deuda);
            cmd.Parameters.AddWithValue("?liquidada", liquidada);

            cmd.ExecuteNonQuery();
        }
    }
}
