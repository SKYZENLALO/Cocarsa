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
        private Double deudaFila;

        private List<Abono> lista_abonos = null;
        private List<Double> lista_deudas = null;
        private List<String> lista_conceptos = null;

        public AbonoDao() {                        
            lista_abonos = new List<Abono>();
            lista_deudas = new List<Double>();
            lista_conceptos = new List<String>();
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
                cmd.Parameters.AddWithValue("?idNota",pagoAbono.IdConcepto);
            else if (tipoPago.Equals("larguillo"))
                cmd.Parameters.AddWithValue("?idLarguillo", pagoAbono.IdConcepto);

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

            Boolean liquidada = deudaFila == 0 ? true : false; 

            MySqlCommand cmd = new MySqlCommand(query, conexion);
            if (tipoPago == "nota")
                cmd.Parameters.AddWithValue("?idNota", pagoAbono.IdConcepto);
            else
                cmd.Parameters.AddWithValue("?idLarguillo", pagoAbono.IdConcepto);
            cmd.Parameters.AddWithValue("?adeudo", deudaFila);
            cmd.Parameters.AddWithValue("?liquidada", liquidada);

            cmd.ExecuteNonQuery();
        }

        public void agregaPagoTransaccion(Abono datosPago, Double adeudo, String concepto)
        {
            Abono abono = new Abono();
            abono.IdConcepto = datosPago.IdConcepto;
            abono.IdCliente = datosPago.IdCliente;
            abono.IdCajera = datosPago.IdCajera;
            abono.FechaAbono = datosPago.FechaAbono;
            abono.MontoAbono = datosPago.MontoAbono;

            Double deuda = adeudo;
            String tipo = concepto;

            lista_abonos.Add(abono);
            lista_deudas.Add(deuda);
            lista_conceptos.Add(tipo);            
        }

        public List<long> registraPago() 
        {
            List<long> id_abono = new List<long>();

            Conexion conexion = new Conexion();
            MySqlConnection conn = conexion.abrirConexion();
            MySqlTransaction tx = null;
            tx = conn.BeginTransaction();

            try
            {
                for (int i = 0; i < lista_abonos.Count; i++ )
                {
                    pagoAbono = lista_abonos[i];
                    tipoPago = lista_conceptos[i];
                    deudaFila = lista_deudas[i];

                    actualizarDeuda(conn);
                    long id = registrarAbono(conn);                    

                    id_abono.Add(id);
                }

                tx.Commit();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                tx.Rollback();
                id_abono = null;
            }

            conexion.cerrarConexion();

            return id_abono;
        }
    }
}
