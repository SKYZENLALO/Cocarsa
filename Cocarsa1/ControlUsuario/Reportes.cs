using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Cocarsa1.ConexionBD;
using Cocarsa1.Entidades;

namespace Cocarsa1.ControlUsuario
{
    public partial class Reportes : UserControl
    {

        public Reportes()
        {
            InitializeComponent();
            dateTimePicker1.MaxDate = DateTime.Today;
        }

        private void dateTimePicker1_CloseUp(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            consultar();
        }

        private void dateTimePicker1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                dataGridView1.Rows.Clear();
                consultar();
            }
        }

        private void consultar() {
            String estado = "";
            int idIni = 0;
            int idFin = 0;
            int numImp = 0;
            int numPen = 0;
            int numFac = 0;
            int numCan = 0;
            Double totSub = 0;
            Double totImp = 0;
            ReportesDAO reportesDAO = new ReportesDAO();
            String fechaHoy = DateTime.Today.ToShortDateString();
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "yyyy-MM-dd";
            String fechaSel = dateTimePicker1.Text;
            dateTimePicker1.Format = DateTimePickerFormat.Long;
            int numeroReg = 0;
            numeroReg = reportesDAO.numeroRegistros(fechaSel);
            textBox5.Text = numeroReg.ToString();
            int i = 0;
            VentaNota[] ventaNota = null;
            ventaNota = reportesDAO.consultaNota(fechaSel,numeroReg);
            try
            {
                idIni = ventaNota[i].IdNota;
            }
            catch (Exception ex) {
                idIni = 0;
            }
            while (i < numeroReg) {
                DataGridViewRow row = (DataGridViewRow)dataGridView1.Rows[0].Clone();
                row.Cells[0].Value = ventaNota[i].FolioNota;
                row.Cells[1].Value = ventaNota[i].IdCliente;
                row.Cells[2].Value = ventaNota[i].Subtotal;
                totSub += ventaNota[i].Subtotal;
                row.Cells[3].Value = ventaNota[i].Iva;
                row.Cells[4].Value = ventaNota[i].Total;
                totImp += ventaNota[i].Total;
                switch (ventaNota[i].Estado)
                {
                    case 1:
                        estado = "Impresa";
                        numImp++;
                        break;
                    case 2:
                        estado = "Pendiente";
                        numPen++;
                        break;
                    case 3:
                        estado = "Facturada";
                        numFac++;
                        break;
                    case 4:
                        estado = "Cancelada";
                        numCan++;
                        break;
                }
                row.Cells[5].Value = estado;
                idFin = ventaNota[i].IdNota;
                dataGridView1.Rows.Add(row);
                i++;
            }
            textBox1.Text = numCan.ToString();
            textBox2.Text = numFac.ToString();
            textBox3.Text = numImp.ToString();
            textBox4.Text = numPen.ToString();
            textBox6.Text = totSub.ToString();
            textBox7.Text = totImp.ToString();
            MessageBox.Show("Inicio: " + idIni + " Fin: " + idFin);
            //MessageBox.Show("Hoy: "+fechaHoy+" Picker: "+dateTimePicker1.Text);

            //Falta tabla de kilos
            //Usar SUM de MySql sobre Importes y kilos buscando sobre fecha
            if (idIni != 0) { 
            
            }
        }
    }
}
