using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Cocarsa1.Entidades;
using Cocarsa1.ConexionBD;
using MySql.Data.MySqlClient;
namespace Cocarsa1.ControlUsuario
{
    public partial class Fajillas : UserControl
    {
        MySqlDataReader consulta;
        public Fajillas()
        {
            InitializeComponent();
            cargaFajillaCaja();
            cargaFajillaDelDia();
            cargaFajillaPorDia();
            cargaFajillaPorCorte();

        }

        void cargaFajillaCaja() {
            MySqlConnection conn;
            Conexion conexion = new Conexion();
            conn = conexion.abrirConexion();
            MySqlCommand cmdDataBase = new MySqlCommand("SELECT idCajera as 'Cajera',monto as 'Monto',fecha as 'Fecha' FROM `cocarsa`.`fajilla` WHERE enCaja = '1';",conn);
            //MySqlCommand cmdDataBase = new MySqlCommand("SELECT idCajera,monto FROM `cocarsa`.`fajilla`;", conn);
            try
            {
                MySqlDataAdapter sda = new MySqlDataAdapter();
                sda.SelectCommand = cmdDataBase;
                DataTable dbdataset = new DataTable();
                sda.Fill(dbdataset);
                BindingSource bSource = new BindingSource();
                bSource.DataSource = dbdataset;
                dataGridView3.DataSource = bSource;
                sda.Update(dbdataset);
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            
            }
        }
        void cargaFajillaDelDia()
        {
            MySqlConnection conn;
            Conexion conexion = new Conexion();
            conn = conexion.abrirConexion();
            //MySqlCommand cmdDataBase = new MySqlCommand("SELECT idCajera as 'Cajera',monto as 'Monto',fecha as 'Fecha' FROM `cocarsa`.`fajilla` WHERE enCaja = '1';",conn);
            MySqlCommand cmdDataBase = new MySqlCommand("SELECT idCajera as 'Cajera',monto as 'Monto' FROM `cocarsa`.`fajilla` WHERE fecha = current_date();", conn);
            try
            {
                MySqlDataAdapter sda = new MySqlDataAdapter();
                sda.SelectCommand = cmdDataBase;
                DataTable dbdataset = new DataTable();
                sda.Fill(dbdataset);
                BindingSource bSource = new BindingSource();
                bSource.DataSource = dbdataset;
                dataGridView2.DataSource = bSource;
                sda.Update(dbdataset);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }

        void cargaFajillaPorDia()
        {
            string fechaRegistro = dateTimePicker1.Value.ToString("yyyy-MM-dd");

            MySqlConnection conn;
            Conexion conexion = new Conexion();
            conn = conexion.abrirConexion();

            try
            {
               MySqlCommand cmdDataBase2 = new MySqlCommand("SELECT SUM(monto) as 'TOTAL' FROM `cocarsa`.`fajilla` WHERE fecha = '" + fechaRegistro + "';",conn);
                consulta=cmdDataBase2.ExecuteReader();
                if (consulta.Read()) {
                    textBox4.Text = consulta.GetDouble(0).ToString();                               
                }
                cmdDataBase2.Dispose();
                consulta.Close();

                MySqlCommand cmdDataBase = new MySqlCommand("SELECT idCajera as 'Cajera',monto as 'Monto',fecha as 'Fecha' FROM `cocarsa`.`fajilla` WHERE fecha = '" + fechaRegistro + "';", conn);
                MySqlDataAdapter sda = new MySqlDataAdapter();
                sda.SelectCommand = cmdDataBase;
                DataTable dbdataset = new DataTable();
                sda.Fill(dbdataset);
                BindingSource bSource = new BindingSource();
                bSource.DataSource = dbdataset;
                dataGridView1.DataSource = bSource;
                sda.Update(dbdataset);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }

        }

        void cargaFajillaPorCorte()
        {
            string fechaRegistro = dateTimePicker2.Value.ToString("yyyy-MM-dd");

            MySqlConnection conn;
            Conexion conexion = new Conexion();
            conn = conexion.abrirConexion();

            try
            {

                MySqlCommand cmdDataBase = new MySqlCommand("SELECT idCajera as 'Cajera',monto as 'Monto',fecha as 'Fecha' FROM `cocarsa`.`fajilla` WHERE fecha = '" + fechaRegistro + "';", conn);
                MySqlDataAdapter sda = new MySqlDataAdapter();
                sda.SelectCommand = cmdDataBase;
                DataTable dbdataset = new DataTable();
                sda.Fill(dbdataset);
                BindingSource bSource = new BindingSource();
                bSource.DataSource = dbdataset;
                dataGridView4.DataSource = bSource;
                sda.Update(dbdataset);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }
        
    
        private void textBox14_TextChanged(object sender, EventArgs e)
        {

            //DateTime fechaRegistro = dateTimePicker3.Value;
           // Console.WriteLine(fechaRegistro);
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

            cargaFajillaPorDia();

        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            cargaFajillaPorCorte();
        }

        private void comboBox1_Enter(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();

            try
            {
                MySqlConnection conn;
                Conexion conexion = new Conexion();
                conn = conexion.abrirConexion();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT nombre FROM `cocarsa`.`cajera`";
                cmd.ExecuteNonQuery();
                DataTable dt = new DataTable();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    comboBox1.Items.Add(dr["nombre"].ToString());


                }

                /*                              
                MySqlDataReader myReader = cmd.ExecuteReader();
                while (myReader.Read())
                {
                    comboBox1.Items.Add(myReader.GetString("nombre"));
                    Console.WriteLine(myReader.GetString("nombre"));
                }*/
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        



 
    }
}
