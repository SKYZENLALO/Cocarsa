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
        int idCajera = 1;

        public Fajillas()
        {
            InitializeComponent();
            dateTimePicker1.Value = DateTime.Today;
            dateTimePicker2.Value = DateTime.Today;
            dateTimePicker1.MaxDate = DateTime.Today;
            dateTimePicker2.MaxDate = DateTime.Today;
            cargaCajeras();
            FajillasDelDia();
            FajillasEnCaja();
            FajillasPorDia();
        }
        void cargaCajeras() {
            FajillasDAO fajillasDAO = new FajillasDAO();
            Cajera[] cajera = null;
            int i = 0;
            int numCajeras = fajillasDAO.numeroCajeras();
            cajera = fajillasDAO.cajeras(numCajeras);
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox1.Items.Clear();
            while (i < numCajeras) {
                comboBox1.Items.Add(cajera[i].Nombre+" "+cajera[i].APaterno+" "+cajera[i].AMaterno);
                i++;
            }
            comboBox1.SelectedIndex = 0;
        }
        void FajillasDelDia() {
            dataGridView2.Rows.Clear();
            FajillasDAO fajillasDAO=new FajillasDAO();
            Fajilla[] fajilla = null;
            int i = 0;
            Double montoTotal = 0;
            DateTime hoy = DateTime.Today;
            String fecha = hoy.Year.ToString()+"-"+hoy.Month.ToString()+"-"+hoy.Day.ToString();
            int numeroFajillas = fajillasDAO.numeroFajillas(fecha);
            fajilla = fajillasDAO.fajillas(numeroFajillas,fecha);
            while (i < numeroFajillas) {
                DataGridViewRow row = (DataGridViewRow)dataGridView2.Rows[0].Clone();
                row.Cells[0].Value = fajillasDAO.nombreCajera(fajilla[i].IdCajera);
                row.Cells[1].Value = fajilla[i].Monto;
                montoTotal += fajilla[i].Monto;
                dataGridView2.Rows.Add(row);
                i++;
            }
            textBox2.Text = montoTotal.ToString();
           
        }

        void FajillasPorDia()
        {
            dataGridView5.Rows.Clear();
            FajillasDAO fajillasDAO = new FajillasDAO();
            Fajilla[] fajilla = null;
            int i = 0;
            Double montoTotal = 0;
            string fecha = dateTimePicker1.Value.ToString("yyyy-MM-dd");
            Console.Write(fecha);            

            int numeroFajillas = fajillasDAO.numeroFajillas(fecha);
            fajilla = fajillasDAO.fajillas(numeroFajillas, fecha);
            while (i < numeroFajillas)
            {
                DataGridViewRow row = (DataGridViewRow)dataGridView5.Rows[0].Clone();
                row.Cells[0].Value = fajillasDAO.nombreCajera(fajilla[i].IdCajera);
                row.Cells[1].Value = fajilla[i].Monto;
                montoTotal += fajilla[i].Monto;
                dataGridView5.Rows.Add(row);
                i++;
            }
            textBox4.Text = montoTotal.ToString();
            
        }

        void FajillasPorCorte()
        {
            dataGridView1.Rows.Clear();
            FajillasDAO fajillasDAO = new FajillasDAO();
            Fajilla[] fajilla = null;
            int i = 0;
            Double montoTotal = 0;
            string fecha = dateTimePicker2.Value.ToString("yyyy-MM-dd");
            Console.Write(fecha);

            int numeroFajillas = fajillasDAO.numeroFajillasFechaCorte(fecha);
            fajilla = fajillasDAO.fajillasFechaCorte(numeroFajillas, fecha);
            while (i < numeroFajillas)
            {
                DataGridViewRow row = (DataGridViewRow)dataGridView1.Rows[0].Clone();
                row.Cells[0].Value = fajillasDAO.nombreCajera(fajilla[i].IdCajera);
                row.Cells[1].Value = fajilla[i].Monto;
                row.Cells[2].Value = fajilla[i].FechaCorte;
                montoTotal += fajilla[i].Monto;
                dataGridView1.Rows.Add(row);
                i++;
            }
            textBox5.Text = montoTotal.ToString();

        }



        void FajillasEnCaja()
        {
            dataGridView3.Rows.Clear();
            FajillasDAO fajillasDAO = new FajillasDAO();
            Fajilla[] fajilla = null;
            int i = 0;
            Double montoTotal = 0;
            int numeroFajillas = fajillasDAO.numeroFajillasCaja();
            fajilla = fajillasDAO.fajillasCaja(numeroFajillas);
            while (i < numeroFajillas)
            {
                DataGridViewRow row = (DataGridViewRow)dataGridView3.Rows[0].Clone();
                row.Cells[0].Value = fajillasDAO.nombreCajera(fajilla[i].IdCajera);
                row.Cells[1].Value = fajilla[i].Monto;
                row.Cells[2].Value = fajilla[i].FechaRegistro;
                montoTotal += fajilla[i].Monto;
                dataGridView3.Rows.Add(row);
                i++;
            }
            textBox3.Text = montoTotal.ToString();
            
        }

        void corteDeCaja()
        {

            if (MessageBox.Show("Desea realizar el corte de caja?", "AVISO", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {

                MySqlConnection conn;
                Conexion conexion = new Conexion();
                conn = conexion.abrirConexion();
                try
                {
                    MySqlCommand cmdDataBase = new MySqlCommand("UPDATE `cocarsa`.`fajilla` SET enCaja = '0',fechaCorte=current_date() WHERE enCaja = '1';", conn);
                    cmdDataBase.ExecuteNonQuery();
                    MessageBox.Show("Corte de caja realizado exitosamente");
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
        }      
       

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            idCajera = comboBox1.SelectedIndex + 1;
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F10){
                try
                {
                    Double monto = Convert.ToDouble(textBox1.Text);
                    if (monto <= 0)
                    {
                        MessageBox.Show("Solo cifras validas");
                        textBox1.Clear();
                        return;
                    }
                    else {
                        //Agregar Box de confirmacion 
                        string selected = this.comboBox1.GetItemText(this.comboBox1.SelectedItem);
                        if (MessageBox.Show("Desea registrar la fajilla con un Monto de: " + monto + " Con la Cajera: " + selected + "?", "AVISO", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            string fechaRegistro = dateTimePicker3.Value.ToString("yyyy-MM-dd");
                            MySqlConnection conn;
                            Conexion conexion = new Conexion();
                            conn = conexion.abrirConexion();
                            try
                            {
                                MySqlCommand cmdDataBase = new MySqlCommand("INSERT INTO `cocarsa`.`fajilla` (idCajera,monto,fecha,enCaja) values ('" + idCajera + "','" + monto + "','" + fechaRegistro + "','1'); ;", conn);
                                cmdDataBase.ExecuteNonQuery();
                                MessageBox.Show("Fajilla Registrada");
                                textBox1.Clear();

                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Ingrese cantidades validas");
                                //textBox1.Clear();         
                                MessageBox.Show(ex.Message);
                            }

                            //MessageBox.Show("se Ingresa: " + monto + " Con la Cajera: " + idCajera);
 
                        }

                    }
                    //Actualizar Datos
                    FajillasDelDia();
                    FajillasEnCaja();

                }
                catch (Exception error) {
                    MessageBox.Show("Solo Cifras validas");
                    textBox1.Clear();
                    return;
                }
            }

            if (e.KeyCode == Keys.F11)
            {
                corteDeCaja();
                FajillasEnCaja();

            }
        }



        private void dateTimePicker1Press(object sender, EventArgs e)
        {
            FajillasPorDia();
        }

        private void dateTimePicker2Press(object sender, EventArgs e)
        {
            FajillasPorCorte();
        }
    }
}
