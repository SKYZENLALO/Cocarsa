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
        Font fuente1 = new Font("Algerian", 18, FontStyle.Regular);
        Font fuente2 = new Font("Arial Narrow", 11, FontStyle.Bold);
        Font fuente3 = new Font("Arial Narrow", 11, FontStyle.Regular);
        Font fuente4 = new Font("Arial Narrow", 10, FontStyle.Regular);
        Font fuente5 = new Font("Arial Narrow", 8, FontStyle.Regular);
        Font fuente6 = new Font("Arial Narrow", 12, FontStyle.Regular);
        Font fuente7 = new Font("Arial Narrow", 12, FontStyle.Bold);
        Font fuente8 = new Font("Arial Narrow", 10, FontStyle.Bold);
        Font fuente9 = new Font("Arial Narrow", 8, FontStyle.Regular);

        private long idAbono = -1; 
        private String folioCadena = "", concepto = "";
        private Double montoImpresion = 0;

        private Double deudaNota = 0;
        private Double deudaLarguillo = 0;
        private Double deudaTotal = 0;

        private int filaSeleccionada = 0;
        private Boolean seleccionNota = false;
        private Boolean seleccionLarguillo = false;

        private List<Venta> deudaCliente = null;
        private Cocarsa1.Entidades.Cliente cliente = null;
        
        public PagoAbonos()
        {
            InitializeComponent();
            printDocument1.PrinterSettings.PrinterName = "EPSON TM-T20II Receipt";

            label6.Text = DateTime.Today.ToLongDateString();
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
                textBox1.Text = "";            
                limpiarPantalla();
            }
        }

        public void reiniciaPago() 
        {
            textBox8.Text = "GENERAL";            
            textBox7.Text = "0.0";
            textBox5.Text = "";
            textBox6.Text = "";
            filaSeleccionada = 0;
            seleccionNota = false;
            seleccionLarguillo = false;

            dataGridView1.ClearSelection();
            dataGridView2.ClearSelection();
            textBox9.Text = "0.0";
            textBox9.Focus();
            textBox9.Select(0, textBox9.Text.Length);            
        }

        public void limpiarPantalla()
        {
            textBox2.Text = "0.0";
            textBox3.Text = "0.0";
            textBox4.Text = "0.0";
            textBox7.Text = "0.0";
            textBox9.Text = "0.0";
            textBox5.Text = "";
            textBox6.Text = "";
            textBox8.Text = "GENERAL";
            textBox9.Enabled = false;
            comboBox1.Enabled = false;
            
            deudaNota = 0;
            deudaLarguillo = 0;
            deudaTotal = 0;

            dataGridView1.Rows.Clear();
            dataGridView2.Rows.Clear();
            filaSeleccionada = 0;
            seleccionNota = false;
            seleccionLarguillo = false;
        }

        private void calcularTotales() {

            deudaNota = 0;
            deudaLarguillo = 0;
            deudaTotal = 0;

            foreach (DataGridViewRow row in dataGridView1.Rows) 
            {
                deudaLarguillo += Convert.ToDouble(row.Cells[2].Value);
            }

            foreach (DataGridViewRow row in dataGridView2.Rows)
            {
                deudaNota += Convert.ToDouble(row.Cells[2].Value);
            }

            deudaNota = Math.Round((deudaNota), 2);
            deudaLarguillo = Math.Round((deudaLarguillo),2);
            deudaTotal = Math.Round((deudaNota + deudaLarguillo),2);

            if (deudaTotal == 0)
                textBox9.Enabled = false;
            else
                textBox9.Enabled = true;

            textBox4.Text = deudaTotal.ToString("N2");
            textBox2.Text = deudaNota.ToString("N2");
            textBox3.Text = deudaLarguillo.ToString("N2");
        }

        public void cargaDeudaCliente()
        {
            dataGridView1.Rows.Clear();
            dataGridView2.Rows.Clear();
            deudaNota = 0;
            deudaLarguillo = 0;
            deudaTotal = 0;
            
            textBox1.Text = cliente.Nombre + " " + cliente.APaterno + " " + cliente.AMaterno;
            ClienteDao dao = new ClienteDao();

            deudaCliente = dao.adeudoCliente(cliente.IdCliente);

            if (deudaCliente.Count == 0)
            {
                limpiarPantalla();                
                MessageBox.Show("El cliente no tiene adeudos.", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            else
            {
                comboBox1.Enabled = true;
                textBox9.Enabled = true;
                textBox9.Focus();
                textBox9.Select(0, textBox9.Text.Length);
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

            textBox2.Text = deudaNota.ToString("N2");
            textBox3.Text = deudaLarguillo.ToString("N2");
            dataGridView1.ClearSelection();
            dataGridView2.ClearSelection();

            deudaTotal = deudaNota + deudaLarguillo;
            textBox4.Text = deudaTotal.ToString("N2");
        }

        private void registrar_abono(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down) 
            {
                if (dataGridView2.Rows.Count > 0) 
                {                 
                    dataGridView2.Focus();
                    dataGridView2.Rows[0].Selected = true;
                    return;
                }

                if (dataGridView1.Rows.Count > 0)
                {
                    dataGridView1.Focus();
                    dataGridView1.Rows[0].Selected = true;
                    return;
                }
            }            
            
            if (e.KeyCode == Keys.Escape) 
            {
                textBox5.Text = "";
                textBox6.Text = "";
                textBox7.Text = "0.0";
                textBox8.Text = "GENERAL";
                filaSeleccionada = 0;
                seleccionNota = false;
                seleccionLarguillo = false;
                return;
            }
            
            if (e.KeyCode == Keys.F10) 
            {
                e.SuppressKeyPress = true;
                Double monto = 0;

                try
                {
                    monto = Convert.ToDouble(textBox9.Text);
                }
                catch (Exception exception)
                {
                    monto = 0;
                    textBox9.Text = "0.0";
                    MessageBox.Show("Ingresa una cantidad correcta.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (monto <= 0)
                {
                    MessageBox.Show("El monto debe ser mayor a 0.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBox9.Select(0, textBox9.Text.Length);
                    return;
                }

                if (seleccionNota || seleccionLarguillo)
                {
                    if (monto > Convert.ToDouble(textBox7.Text))
                    {
                        String texto = seleccionLarguillo ? "del larguillo." : "de la nota.";

                        MessageBox.Show("El monto es mayor a la deuda " + texto, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        textBox9.Select(0, textBox9.Text.Length);
                        return;
                    }
                }
                else 
                {
                    if (monto > Convert.ToDouble(textBox4.Text))
                    {
                        MessageBox.Show("El monto es mayor a la deuda total del cliente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        textBox9.Select(0, textBox9.Text.Length);
                        return;
                    }
                }

                DialogResult result = MessageBox.Show("¿Confirmas el pago por $" + monto.ToString("N2") +"?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.No)
                {
                    return;
                }                

                Abono abono = new Abono();
                abono.IdCajera = 1;
                abono.IdCliente = cliente.IdCliente;
                abono.MontoAbono = Math.Round(monto,2);
                abono.FechaAbono = DateTime.Now;

                montoImpresion = abono.MontoAbono;
                
                AbonoDao dao = new AbonoDao();
                if (seleccionNota || seleccionLarguillo)
                {
                    Double deudaFila = Convert.ToDouble(textBox7.Text);
                    deudaFila = Math.Round(deudaFila, 2) - Math.Round(monto,2);

                    if (seleccionNota)
                    {
                        abono.IdGeneral = deudaCliente[filaSeleccionada].IdGeneral;
                        folioCadena = abono.IdGeneral.ToString();
                        concepto = "NOTA";                        

                        if ((idAbono = dao.registrarPago(abono, Math.Round(deudaFila,2), "nota")) != -1)
                        {
                            dataGridView2.Rows[filaSeleccionada].Cells[2].Value = deudaFila;
                            if (deudaFila == 0)
                                dataGridView2.Rows[filaSeleccionada].Cells[4].Value = "SI";                           
                            
                            printDocument1.Print();
                        }
                        else {
                            MessageBox.Show("No se pudo registrar abono a nota.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);   
                        }                        
                    }
                    else if (seleccionLarguillo)
                    {
                        abono.IdGeneral = deudaCliente[filaSeleccionada+dataGridView2.Rows.Count].IdGeneral;
                        folioCadena = abono.IdGeneral.ToString();
                        concepto = "LARGUILLO";

                        if ((idAbono = dao.registrarPago(abono, Math.Round(deudaFila, 2), "larguillo")) != -1)
                        {
                            dataGridView1.Rows[filaSeleccionada].Cells[2].Value = deudaFila;
                            if (deudaFila == 0)
                                dataGridView1.Rows[filaSeleccionada].Cells[4].Value = "SI";

                            printDocument1.Print();
                        }
                        else {
                            MessageBox.Show("No se pudo registrar abono a larguillo.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }                        
                    }                    
                }
                else
                {
                    int filaLarguillo = 0;
                    int filaNotas = 0;
                    int contadorError = 0;

                    while (monto > 0)
                    {
                        if (filaLarguillo < dataGridView1.Rows.Count)
                        {
                            concepto = "LARGUILLO";
                            Double deudaFila = Convert.ToDouble(dataGridView1.Rows[filaLarguillo].Cells[2].Value);

                            if (deudaFila > 0)
                            {
                                abono.IdGeneral = deudaCliente[filaLarguillo + dataGridView2.Rows.Count].IdGeneral;
                                folioCadena = abono.IdGeneral.ToString();

                                if (Math.Round(monto, 2) < Math.Round(deudaFila, 2))
                                {
                                    abono.MontoAbono = Math.Round(monto, 2);
                                    montoImpresion = abono.MontoAbono;
                                    
                                    if ((idAbono = dao.registrarPago(abono, Math.Round(deudaFila - monto, 2), "larguillo")) != -1)
                                    {
                                        dataGridView1.Rows[filaLarguillo].Cells[2].Value = deudaFila - monto;
                                        monto = 0;

                                        printDocument1.Print();
                                    }
                                    else {
                                        contadorError++;
                                    }
                                }
                                else
                                {
                                    abono.MontoAbono = Math.Round(deudaFila, 2);
                                    montoImpresion = abono.MontoAbono;

                                    if ((idAbono = dao.registrarPago(abono, 0, "larguillo")) != -1)
                                    {
                                        monto = Math.Round(monto, 2) - Math.Round(deudaFila, 2);
                                        dataGridView1.Rows[filaLarguillo].Cells[2].Value = 0;                                        
                                        dataGridView1.Rows[filaLarguillo].Cells[4].Value = "SI";

                                        printDocument1.Print();
                                    }
                                    else
                                    {
                                        contadorError++;
                                    }
                                }
                            }
                            filaLarguillo++;
                        }
                        else if (filaNotas < dataGridView2.Rows.Count)
                        {
                            concepto = "NOTA";
                            Double deudaFila = Convert.ToDouble(dataGridView2.Rows[filaNotas].Cells[2].Value);

                            if (filaNotas < dataGridView2.Rows.Count)
                            {
                                if (deudaFila > 0)
                                {
                                    abono.IdGeneral = deudaCliente[filaNotas].IdGeneral;
                                    folioCadena = abono.IdGeneral.ToString();

                                    if (Math.Round(monto,2) < Math.Round(deudaFila,2))
                                    {
                                        abono.MontoAbono = Math.Round(monto, 2);
                                        montoImpresion = abono.MontoAbono;

                                        if ((idAbono = dao.registrarPago(abono, Math.Round(deudaFila-monto,2), "nota")) != -1)
                                        {
                                            dataGridView2.Rows[filaNotas].Cells[2].Value = deudaFila - monto;
                                            monto = 0;

                                            printDocument1.Print();
                                        }
                                        else
                                        {
                                            contadorError++;
                                        } 
                                    }
                                    else
                                    {
                                        abono.MontoAbono = Math.Round(deudaFila,2);
                                        montoImpresion = abono.MontoAbono;

                                        if ((idAbono = dao.registrarPago(abono, 0, "nota")) != -1)
                                        {
                                            dataGridView2.Rows[filaNotas].Cells[2].Value = 0;
                                            monto = Math.Round(monto, 2) - Math.Round(deudaFila, 2);
                                            dataGridView2.Rows[filaNotas].Cells[4].Value = "SI";

                                            printDocument1.Print();
                                        }
                                        else
                                        {
                                            contadorError++;
                                        }
                                    }
                                }
                            }
                            filaNotas++;
                        }
                    }
                    if (contadorError > 0) {
                        MessageBox.Show("No se pudo registrar abono general.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                calcularTotales();
                reiniciaPago();
            }
        }

        private void dataGridView2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Right && dataGridView1.Rows.Count > 0)
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
                
                if (dataGridView2.Rows[filaSeleccionada].Cells[2].Value.ToString() == "0")
                {
                    MessageBox.Show("La nota ya está liquidada.", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                textBox5.Text = dataGridView2.Rows[filaSeleccionada].Cells[0].Value.ToString();
                textBox6.Text = dataGridView2.Rows[filaSeleccionada].Cells[1].Value.ToString();
                textBox7.Text = dataGridView2.Rows[filaSeleccionada].Cells[2].Value.ToString();
                textBox8.Text = "NOTA";                
                seleccionLarguillo = false;
                seleccionNota = true;
                textBox9.Focus();
                textBox9.Select(0, textBox9.Text.Length);
                dataGridView2.ClearSelection();
            }

            if (e.KeyCode == Keys.Escape)
            {
                reiniciaPago();
            }        
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left && dataGridView2.Rows.Count > 0)
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

                if (dataGridView1.Rows[filaSeleccionada].Cells[2].Value.ToString() == "0") {
                    MessageBox.Show("El larguillo ya está liquidado.", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                textBox5.Text = dataGridView1.Rows[filaSeleccionada].Cells[0].Value.ToString();
                textBox6.Text = dataGridView1.Rows[filaSeleccionada].Cells[1].Value.ToString();
                textBox7.Text = dataGridView1.Rows[filaSeleccionada].Cells[2].Value.ToString();
                textBox8.Text = "LARGUILLO";
                seleccionLarguillo = true;
                seleccionNota = false;
                textBox9.Focus();
                textBox9.Select(0, textBox9.Text.Length);
                dataGridView1.ClearSelection();
            }

            if (e.KeyCode == Keys.Escape)
            {
                reiniciaPago();
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

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            StringFormat formato = new StringFormat();
            formato.Alignment = StringAlignment.Center;
            Image puerquito = Properties.Resources.porky;
            
            e.Graphics.DrawImage(puerquito,0,0);
            e.Graphics.DrawString("COCARSA", fuente1, Brushes.Black, 90, 42);
            e.Graphics.DrawString("PORCINOS MEXICANOS S.A. DE C.V.", fuente7, Brushes.Black, 25, 72);
            e.Graphics.DrawString("CARNES SELECTAS DE CERDO MAYOREO Y MENUDEO", fuente8, Brushes.Black, 10, 102);
            e.Graphics.DrawString("CARRET. MEXICO PACHUCA KM. 38.5 ESQ. CALLE DE LA", fuente9, Brushes.Black, 16, 122);
            e.Graphics.DrawString("LEGUA, TECAMAC DE FELIPE VILLANUEVA,  C.P. 55740", fuente9, Brushes.Black, 19, 137);
            e.Graphics.DrawString("ESTADO DE  MEXICO TELS : 5934-7171  Y  5934-7172", fuente9, Brushes.Black, 22, 152);

            e.Graphics.DrawString("Abono ID : ", fuente2, Brushes.Black, 10, 200);
            e.Graphics.DrawString(idAbono.ToString(), fuente3, Brushes.Black, 80, 200);
            e.Graphics.DrawString("Folio : ", fuente2, Brushes.Black, 160, 200);
            e.Graphics.DrawString(folioCadena, fuente3, Brushes.Black, 205, 200);
            e.Graphics.DrawString("Concepto : ", fuente2, Brushes.Black, 10, 220);
            e.Graphics.DrawString(concepto, fuente3, Brushes.Black, 80, 220);
            e.Graphics.DrawString("Cajera : ", fuente2, Brushes.Black, 10, 240);
            e.Graphics.DrawString(comboBox1.Text, fuente3, Brushes.Black, 70, 240);
            e.Graphics.DrawString("Cliente ID: ", fuente2, Brushes.Black, 10, 280);
            e.Graphics.DrawString(cliente.IdCliente.ToString(), fuente3, Brushes.Black, 80, 280);
            e.Graphics.DrawString("Cliente : ", fuente2, Brushes.Black, 10, 300);
            e.Graphics.DrawString(textBox1.Text, fuente3, Brushes.Black, 70, 300);
            e.Graphics.DrawString("Monto de Pago : ", fuente2, Brushes.Black, 10, 320);
            e.Graphics.DrawString("$ " + montoImpresion.ToString("N2"), fuente6, Brushes.Black, 180, 320);
            e.Graphics.DrawString(String.Format("{0:dd - MMMM - yyyy HH:mm tt}", DateTime.Now), fuente4, Brushes.Black, 60, 410);
            e.Graphics.DrawString(".", fuente4, Brushes.Black, 10, 440);
                        
        }

       
        
    }
}
