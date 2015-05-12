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
    public partial class PagoAbonos : UserControl
    {
        private Double deudaNota = 0;
        private Double deudaLarguillo = 0;
        private Double deudaTotal = 0;

        private Cocarsa1.Entidades.Cliente cliente = null;
        
        public PagoAbonos()
        {
            InitializeComponent();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                Popup popup = new Popup();

                if (popup.ShowDialog() == DialogResult.OK)
                {
                    cliente = popup.ClienteSeleccionado;
                    cargaDeudaCliente();
                }                
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) {
                limpiarPantalla();
            }
        }
        
        public void limpiarPantalla()
        {
            textBox1.Text = "";
            textBox6.Text = "0.0";
            textBox7.Text = "0.0";
            textBox8.Text = "0.0";

            deudaNota = 0;
            deudaLarguillo = 0;
            deudaTotal = 0;
        }

        public void cargaDeudaCliente()
        {
            textBox1.Text = cliente.Nombre + " " + cliente.APaterno + " " + cliente.AMaterno;
            ClienteDao dao = new ClienteDao();

            List<Venta> deudaCliente = dao.adeudoCliente(cliente.IdCliente);

            foreach(Venta deuda in deudaCliente) 
            {
                DataGridViewRow row = null;
                                
                if (deuda is VentaNota) 
                {
                    deudaNota += deuda.Adeudo;
                    row = (DataGridViewRow)dataGridView2.Rows[0].Clone();
                    row.Cells[0].Value = deuda.FolioNota;
                    row.Cells[1].Value = deuda.FechaVenta;
                    row.Cells[2].Value = deuda.Adeudo;
                    row.Cells[3].Value = deuda.Total;
                    if (deuda.Liquidada)
                        row.Cells[4].Value = "SI";
                    else
                        row.Cells[4].Value = "NO";
                    dataGridView2.Rows.Add(row);
                  
                }
                else if (deuda is VentaLarguillo) 
                {
                    deudaLarguillo += deuda.Adeudo;
                    row = (DataGridViewRow)dataGridView1.Rows[0].Clone();
                    row.Cells[0].Value = deuda.FolioNota;
                    row.Cells[1].Value = deuda.FechaVenta;
                    row.Cells[2].Value = deuda.Adeudo;
                    row.Cells[3].Value = deuda.Total;
                    if (deuda.Liquidada)
                        row.Cells[4].Value = "SI";
                    else
                        row.Cells[4].Value = "NO";
                    dataGridView1.Rows.Add(row);
                }
            }

            textBox7.Text = String.Format("{0:0.00}",deudaNota);
            textBox8.Text = String.Format("{0:0.00}",deudaLarguillo);

            deudaTotal = deudaNota + deudaLarguillo;
            textBox6.Text = String.Format("{0:0.00}",deudaTotal);
        }
    }
}
