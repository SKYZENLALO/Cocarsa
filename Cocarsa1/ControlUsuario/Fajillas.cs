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
            cargaFajillas();
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
        void cargaFajillas() {
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
            //dataGridView2.Rows.Remove(dataGridView2.Rows[dataGridView2.Rows.Count-1]);
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
                        MessageBox.Show("se Ingresa: " + monto+" Con la Cajera: "+idCajera);
                    }
                }
                catch (Exception error) {
                    MessageBox.Show("Solo Cifras validas");
                    textBox1.Clear();
                    return;
                }
            }
        }
    }
}
