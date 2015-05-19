﻿using System;
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

        private List<Venta> deudaCliente = null;
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
            
            deudaNota = 0;
            deudaLarguillo = 0;
            deudaTotal = 0;

            groupBox2.Enabled = false;
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
                groupBox2.Enabled = false;

            textBox4.Text = deudaTotal + "";
            textBox2.Text = deudaNota + "";
            textBox3.Text = deudaLarguillo + "";
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
                MessageBox.Show("El cliente no tiene adeudos.");

                return;
            }
            else
            {
                groupBox2.Enabled = true;
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

            textBox2.Text = String.Format("{0:0.00}",deudaNota);
            textBox3.Text = String.Format("{0:0.00}",deudaLarguillo);
            dataGridView1.ClearSelection();
            dataGridView2.ClearSelection();

            deudaTotal = deudaNota + deudaLarguillo;
            textBox4.Text = String.Format("{0:0.00}",deudaTotal);
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
                    monto = Math.Round(monto,2);
                } 
                catch(Exception exception) 
                {                    
                    exception.ToString();
                    MessageBox.Show("Debes ingresar un monto válido");
                    textBox9.Text = "0.0";
                    textBox9.Focus();
                    textBox9.Select(0, textBox9.Text.Length);
                    return;
                }
                if (monto <= 0)
                {
                    MessageBox.Show("El monto debe ser mayor a 0.");
                    textBox9.Select(0, textBox9.Text.Length);
                    return;
                }

                if (seleccionNota || seleccionLarguillo)
                {
                    if (monto > Convert.ToDouble(textBox7.Text))
                    {
                        String texto = seleccionLarguillo ? "del larguillo." : "de la nota.";

                        MessageBox.Show("El monto es mayor a la deuda " + texto);
                        textBox9.Select(0, textBox9.Text.Length);
                        return;
                    }
                }
                else 
                {
                    if (monto > Convert.ToDouble(textBox4.Text))
                    {
                        MessageBox.Show("El monto es mayor a la deuda total del cliente.");
                        textBox9.Select(0, textBox9.Text.Length);
                        return;
                    }
                }

                DialogResult result = MessageBox.Show("¿Confirmas el pago de abono por $" + String.Format("{0:0.00}",monto) +"?", "Confirmación", MessageBoxButtons.YesNo);
                if (result == DialogResult.No)
                {
                    return;
                }                

                Abono abono = new Abono();
                abono.IdCajera = 1;
                abono.IdCliente = cliente.IdCliente;
                abono.MontoAbono = Math.Round(monto,2);
                abono.FechaAbono = DateTime.Now;
                
                AbonoDao dao = new AbonoDao();
                if (seleccionNota || seleccionLarguillo)
                {
                    Double deudaFila = Convert.ToDouble(textBox7.Text);
                    deudaFila = Math.Round(deudaFila, 2) - Math.Round(monto,2);

                    if (seleccionNota)
                    {
                        abono.IdGeneral = deudaCliente[filaSeleccionada].IdGeneral;
                        
                        if (dao.registrarPago(abono, Math.Round(deudaFila,2), "nota"))
                        {
                            dataGridView2.Rows[filaSeleccionada].Cells[2].Value = deudaFila;
                            if (deudaFila == 0)
                                dataGridView2.Rows[filaSeleccionada].Cells[4].Value = "SI";
                        }
                        else {
                            MessageBox.Show("No se pudo registrar abono a nota.");   
                        }                        
                    }
                    else if (seleccionLarguillo)
                    {
                        abono.IdGeneral = deudaCliente[filaSeleccionada+dataGridView2.Rows.Count].IdGeneral;

                        if (dao.registrarPago(abono, Math.Round(deudaFila, 2), "larguillo"))
                        {
                            dataGridView1.Rows[filaSeleccionada].Cells[2].Value = deudaFila;
                            if (deudaFila == 0)
                                dataGridView1.Rows[filaSeleccionada].Cells[4].Value = "SI";
                        }
                        else {
                            MessageBox.Show("No se pudo registrar abono a larguillo.");
                        }                        
                    }                    
                }
                else
                {
                    int filaLarguillo = 0;
                    int filaNotas = 0;

                    while (monto > 0)
                    {
                        if (filaLarguillo < dataGridView1.Rows.Count)
                        {
                            Double deudaFila = Convert.ToDouble(dataGridView1.Rows[filaLarguillo].Cells[2].Value);
                            if (deudaFila > 0)
                            {
                                abono.IdGeneral = deudaCliente[filaLarguillo + dataGridView2.Rows.Count].IdGeneral;
                                if (Math.Round(monto, 2) < Math.Round(deudaFila, 2))
                                {
                                    abono.MontoAbono = Math.Round(monto, 2);
                                    if (dao.registrarPago(abono, Math.Round(deudaFila - monto, 2), "larguillo"))
                                    {
                                        dataGridView1.Rows[filaLarguillo].Cells[2].Value = deudaFila - monto;
                                        monto = 0;
                                    }
                                    else {
                                        MessageBox.Show("No se pudo registrar abono a larguillo.");
                                    }
                                }
                                else
                                {
                                    abono.MontoAbono = Math.Round(deudaFila, 2);
                                    if (dao.registrarPago(abono, 0, "larguillo"))
                                    {
                                        monto = Math.Round(monto, 2) - Math.Round(deudaFila, 2);
                                        dataGridView1.Rows[filaLarguillo].Cells[2].Value = 0;                                        
                                        dataGridView1.Rows[filaLarguillo].Cells[4].Value = "SI";
                                    }
                                    else
                                    {
                                        MessageBox.Show("No se pudo registrar abono a larguillo.");
                                    }
                                }
                            }
                            filaLarguillo++;
                        }
                        else if (filaNotas < dataGridView2.Rows.Count)
                        {
                            Double deudaFila = Convert.ToDouble(dataGridView2.Rows[filaNotas].Cells[2].Value);

                            if (filaNotas < dataGridView2.Rows.Count)
                            {
                                if (deudaFila > 0)
                                {
                                    abono.IdGeneral = deudaCliente[filaNotas].IdGeneral;
                                    if (Math.Round(monto,2) < Math.Round(deudaFila,2))
                                    {
                                        abono.MontoAbono = Math.Round(monto, 2);
                                        if (dao.registrarPago(abono, Math.Round(deudaFila-monto,2), "nota"))
                                        {
                                            dataGridView2.Rows[filaNotas].Cells[2].Value = deudaFila - monto;
                                            monto = 0;
                                        }
                                        else
                                        {
                                            MessageBox.Show("No se pudo registrar abono a nota.");
                                        } 
                                    }
                                    else
                                    {
                                        abono.MontoAbono = Math.Round(deudaFila,2);
                                        if (dao.registrarPago(abono, 0, "nota"))
                                        {
                                            dataGridView2.Rows[filaNotas].Cells[2].Value = 0;
                                            monto = Math.Round(monto, 2) - Math.Round(deudaFila, 2);
                                            dataGridView2.Rows[filaNotas].Cells[4].Value = "SI";
                                        }
                                        else
                                        {
                                            MessageBox.Show("No se pudo registrar abono a nota.");
                                        }
                                    }
                                }
                            }
                            filaNotas++;
                        }
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
                    MessageBox.Show("La nota ya está liquidada.");
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
                    MessageBox.Show("El larguillo ya está liquidado.");
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
    }
}
