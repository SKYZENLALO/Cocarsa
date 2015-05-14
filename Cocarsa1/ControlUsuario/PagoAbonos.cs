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

        private int filaSeleccionada = 0;
        private Boolean seleccionNota = false;
        private Boolean seleccionLarguillo = false;

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
            
            dataGridView1.Rows.Clear();
            dataGridView2.Rows.Clear();
            filaSeleccionada = 0;
            seleccionNota = false;
            seleccionLarguillo = false;

        }

        public void cargaDeudaCliente()
        {
            dataGridView1.Rows.Clear();
            dataGridView2.Rows.Clear();
            
            textBox1.Text = cliente.Nombre + " " + cliente.APaterno + " " + cliente.AMaterno;
            ClienteDao dao = new ClienteDao();

            List<Venta> deudaCliente = dao.adeudoCliente(cliente.IdCliente);

            if (deudaCliente.Count == 0)
            {
                groupBox2.Enabled = false;
                textBox5.Text = "Pago General";
                textBox6.Text = "0.0";
                textBox7.Text = "0.0";
                textBox8.Text = "0.0"; 
                MessageBox.Show("El cliente no tiene adeudos.");                

                return;
            }
            else
            {
                groupBox2.Enabled = true;
                textBox2.Focus();
                textBox2.Select(0, textBox2.Text.Length);
            }

            foreach(Venta deuda in deudaCliente) 
            {
                if (deuda is VentaNota) 
                {
                    deudaNota += deuda.Adeudo;
                    String liquidada = deuda.Liquidada ? "SI" : "NO";

                    dataGridView2.Rows.Add(deuda.FolioNota, 
                                           deuda.FechaVenta.ToString("dd-MMMM-yyyy"),
                                           deuda.Adeudo,
                                           deuda.Total,
                                           liquidada); 
                  
                }
                else if (deuda is VentaLarguillo)
                {
                    deudaLarguillo += deuda.Adeudo;
                    String liquidada = deuda.Liquidada ? "SI" : "NO";

                    dataGridView1.Rows.Add(deuda.FolioNota,
                                           deuda.FechaVenta.ToString("dd-MMMM-yyyy"),
                                           deuda.Adeudo,
                                           deuda.Total,
                                           liquidada);
                }
            }

            textBox7.Text = String.Format("{0:0.00}",deudaNota);
            textBox8.Text = String.Format("{0:0.00}",deudaLarguillo);
            dataGridView1.ClearSelection();
            dataGridView2.ClearSelection();

            deudaTotal = deudaNota + deudaLarguillo;
            textBox6.Text = String.Format("{0:0.00}",deudaTotal);
        }

        private void registrar_abono(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down) 
            {
                dataGridView2.Focus();
                dataGridView2.Rows[0].Selected = true;
                return;
            }            
            
            if (e.KeyCode == Keys.Escape) 
            {
                textBox3.Text = "";
                textBox4.Text = "";
                textBox5.Text = "Pago General";
                filaSeleccionada = 0;
                seleccionNota = false;
                seleccionLarguillo = false;
                return;
            }
            
            if (e.KeyCode == Keys.F10) 
            {                
                Double monto = 0;
                try 
                {
                    monto = Convert.ToDouble(textBox2.Text);
                } catch(Exception exception) {
                    MessageBox.Show("Debes ingresar un monto válido");
                    textBox2.Text = "0.0";
                    textBox2.Focus();
                    textBox2.Select(0, textBox2.Text.Length);
                    return;
                }
                if (monto == 0)
                {
                    MessageBox.Show("El monto debe ser mayor a 0.");
                    textBox2.Select(0, textBox2.Text.Length);
                    return;
                }
                if (monto > Convert.ToInt32(textBox6.Text)) {
                    MessageBox.Show("El monto es mayor a la deuda total del cliente.");
                    textBox2.Select(0, textBox2.Text.Length);
                    return;
                }

                Abono abono = new Abono();
                abono.IdCajera = 1;
                abono.IdCliente = cliente.IdCliente;
                abono.MontoAbono = monto;
                abono.FechaAbono = DateTime.Now;

                AbonoDao dao = new AbonoDao();
                if (seleccionNota || seleccionLarguillo)
                {                    
                    abono.IdFolio = Convert.ToInt32(textBox3.Text);

                    if (seleccionNota)
                        dao.registrarAbono(abono, "nota");
                    else if (seleccionLarguillo)
                        dao.registrarAbono(abono, "larguillo");
                }
                else 
                { 
                
                }

            }
        }

        private void dataGridView2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Right && dataGridView2.Rows.Count > 0)
            {
                dataGridView1.Focus();
                dataGridView1.Rows[0].Selected = true;
                dataGridView1.CurrentCell = dataGridView1[0, 0];
                dataGridView2.ClearSelection();
            }

            if (e.KeyCode == Keys.Enter && dataGridView2.Rows.Count > 0)
            {
                e.Handled = true;
                filaSeleccionada = dataGridView2.CurrentCell.RowIndex;
                textBox3.Text = dataGridView2.Rows[filaSeleccionada].Cells[0].Value.ToString();
                textBox4.Text = dataGridView2.Rows[filaSeleccionada].Cells[1].Value.ToString();
                textBox5.Text = "Pago de Nota";
                seleccionLarguillo = false;
                seleccionNota = true;
                textBox2.Focus();
                textBox2.Select(0, textBox2.Text.Length);
                dataGridView2.ClearSelection();
            }

            if (e.KeyCode == Keys.Escape)
            {
                textBox5.Text = "Pago General";
                textBox3.Text = "";
                textBox4.Text = "";
                filaSeleccionada = 0;
                seleccionNota = false;
                seleccionLarguillo = false;

                dataGridView2.ClearSelection();
                textBox2.Focus();
                textBox2.Select(0, textBox2.Text.Length);
            }        
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left && dataGridView1.Rows.Count > 0)
            {
                dataGridView2.Focus();
                dataGridView2.Rows[0].Selected = true;
                dataGridView2.CurrentCell = dataGridView2[0, 0];
                dataGridView1.ClearSelection();
            }

            if (e.KeyCode == Keys.Enter && dataGridView1.Rows.Count > 0)
            {
                e.Handled = true;
                filaSeleccionada = dataGridView1.CurrentCell.RowIndex;
                textBox3.Text = dataGridView1.Rows[filaSeleccionada].Cells[0].Value.ToString();
                textBox4.Text = dataGridView1.Rows[filaSeleccionada].Cells[1].Value.ToString();
                textBox5.Text = "Pago de Larguillo";
                seleccionLarguillo = true;
                seleccionNota = false;
                textBox2.Focus();
                textBox2.Select(0, textBox2.Text.Length);
                dataGridView1.ClearSelection();
            }

            if (e.KeyCode == Keys.Escape)
            {
                textBox5.Text = "Pago General";
                textBox3.Text = "";
                textBox4.Text = "";
                filaSeleccionada = 0;
                seleccionNota = false;
                seleccionLarguillo = false;

                dataGridView1.ClearSelection();
                textBox2.Focus();
                textBox2.Select(0, textBox2.Text.Length);
            } 
        }

        private void dataGridView2_Enter(object sender, EventArgs e)
        {
            dataGridView1.ClearSelection();
            if (dataGridView2.Rows.Count > 0)
            {
                dataGridView2.Rows[0].Selected = true;
                dataGridView2.CurrentCell = dataGridView2[0, 0];
            }
            else 
            {
                textBox1.Focus();
            }
        }

        private void dataGridView1_Enter(object sender, EventArgs e)
        {
            dataGridView2.ClearSelection();
            if (dataGridView1.Rows.Count > 0)
            {
                dataGridView1.Rows[0].Selected = true;
                dataGridView1.CurrentCell = dataGridView1[0, 0];
            }
            else
            {
                textBox1.Focus();
            }
        }
    }
}
