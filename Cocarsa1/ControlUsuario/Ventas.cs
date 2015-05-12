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
    public partial class Ventas : UserControl
    {
        private Boolean flag = false;
        private int columna=0;
        private int fila=0;

        public Ventas()
        {
            InitializeComponent();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                checkBox2.Visible = true;
                textBox4.Enabled = false;                
            }
            else {
                checkBox2.Visible = false;
                textBox4.Enabled = true;
                textBox4.Focus();                
            }
        }

        private void limpiarPantalla() {
            dataGridView1.Rows.Clear();
            textBox7.Clear();
            textBox1.Clear();
            textBox5.Clear();
            textBox6.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox8.Clear();
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                VentasDAO ventasDao = new VentasDAO();

                if (textBox2.Text.Trim().Equals("")){
                    //insertar nuevo folio                    
                    textBox2.Text= ventasDao.nuevoFolio().ToString();  
                    //Moverse al sector siguiente
                    limpiarPantalla();
                    textBox8.Text = "Nota Pendiente";
                    textBox3.Text = "Contado";
                    dataGridView1.Enabled = true;
                    dataGridView1.Focus();
                    dataGridView1.Rows[0].Cells[0].Selected = true;
                }
                else
                {
                    //buscar folio
                    VentaNota notaCargar = null;
                    //Validar numeros
                    notaCargar=ventasDao.buscarFolio(Convert.ToInt32(textBox2.Text));

                    if (notaCargar == null)                    {
                        MessageBox.Show("No existe la Nota");
                    }
                    else { 
                        //Mostrar Descripcion Nota
                        limpiarPantalla();
                        textBox7.Text = notaCargar.Adeudo.ToString();
                        textBox1.Text = notaCargar.Subtotal.ToString();
                        textBox5.Text = notaCargar.Iva.ToString();
                        textBox6.Text = notaCargar.Total.ToString();

                        if (notaCargar.IdCliente == 1)
                        {
                            textBox3.Text = "Contado";
                        }
                         else
                        {
                            textBox3.Text = ventasDao.nombreCliente(notaCargar.IdCliente);
                        }
                        switch (notaCargar.Estado) { 
                            case 1:
                                textBox8.Text = "Nota Impresa";
                                dataGridView1.Enabled = false;
                                break;
                            case 2:
                                textBox8.Text = "Nota Pendiente";
                                dataGridView1.Enabled = true;
                                dataGridView1.Focus();
                                dataGridView1.Rows[0].Cells[0].Selected = true;
                                break;
                            case 3:
                                textBox8.Text = "Nota Facturada";
                                dataGridView1.Enabled = false;
                                break;
                            case 4:
                                textBox8.Text = "Nota Cancelada";
                                dataGridView1.Enabled = false;
                                break;
                        }

                        int i = 0;
                        OrdenNota[] ordenNota = null;

                        ordenNota = ventasDao.cargarNota(notaCargar.IdNota);

                        while (ordenNota[i]!=null) {
                            DataGridViewRow row = (DataGridViewRow)dataGridView1.Rows[0].Clone();
                            
                            row.Cells[0].Value = ordenNota[i].IdProducto;
                            row.Cells[1].Value = ventasDao.nombreProducto(ordenNota[i].IdProducto);
                            row.Cells[2].Value = ordenNota[i].Cantidad;
                            row.Cells[3].Value = ordenNota[i].PrecioVenta;
                            row.Cells[4].Value = ordenNota[i].Importe;
                            dataGridView1.Rows.Add(row);
                            i++;
                        }
                    }
                }
            }
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            VentasDAO ventasDao = new VentasDAO();

            columna = dataGridView1.CurrentCell.ColumnIndex;
            fila = dataGridView1.CurrentCell.RowIndex;
            
            switch (dataGridView1.SelectedCells[0].ColumnIndex){
                case 0:
                    //MessageBox.Show("Columna Clave valor: "+valor);
                    ProductoE productoNuevo = null;
                    productoNuevo = ventasDao.cargarProducto(Convert.ToInt32(dataGridView1.CurrentCell.Value));
                    dataGridView1.CurrentRow.Cells[1].Value = productoNuevo.Nombre;
                    dataGridView1.CurrentRow.Cells[3].Value = productoNuevo.PrecioVenta;
                    columna = 2;
                    flag = true;
                    break;
                case 2:
                    Double cantidadTemp = Convert.ToDouble(dataGridView1.CurrentRow.Cells[2].Value.ToString());
                    Double precioTemp = Convert.ToDouble(dataGridView1.CurrentRow.Cells[3].Value.ToString());
                    Double importeTemp = precioTemp*cantidadTemp;
                    dataGridView1.CurrentRow.Cells[4].Value = importeTemp;
                    columna = 3;
                    calculaTotal();
                    flag = true;
                    break;
                case 3:
                    dataGridView1.CurrentRow.Cells[4].Value = (Convert.ToDecimal(dataGridView1.CurrentRow.Cells[2].Value)) * (Convert.ToDecimal(dataGridView1.CurrentRow.Cells[3].Value));
                    columna = 0;
                    calculaTotal();
                    fila++;
                    flag = true;
                    break;
            }

        }

        private void calculaTotal() {
            int filas = dataGridView1.Rows.Count;
            double importe = 0;
            for (int i = 0; i < filas; i++) {
                importe += Convert.ToDouble( dataGridView1[4, i].Value);
            }
            textBox1.Text = importe.ToString();
            textBox5.Text = "0";
            textBox6.Text = importe.ToString();
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (flag) {
                dataGridView1.CurrentCell=dataGridView1[columna,fila];
                flag = false;
            }
        }


        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                if (dataGridView1.SelectedCells[0].ColumnIndex == 3)
                {
                    columna = 0;
                    fila++;
                    dataGridView1.CurrentCell = dataGridView1[columna, fila];
                }
            }
            if (e.KeyCode == Keys.F10) {
                int filas = dataGridView1.Rows.Count - 1;
                OrdenNota[] ordenNota = new OrdenNota[11];
                VentasDAO ventasDAO = new VentasDAO();
                VentaNota ventaNota = new VentaNota();
                VentaNota buscarNota = new VentaNota();

                buscarNota = ventasDAO.buscarFolio(Convert.ToInt32(textBox2.Text));

                if (buscarNota == null)
                {
                    ventaNota.FolioNota = Convert.ToInt32(textBox2.Text);
                    ventaNota.Iva = Convert.ToDouble(textBox5.Text);
                    ventaNota.Total = Convert.ToDouble(textBox1.Text);
                    ventaNota.Subtotal = Convert.ToDouble(textBox6.Text);
                    ventaNota.Estado = 2;
                    ventaNota.Adeudo = 0;
                    ventaNota.IdCliente = 1;
                    ventaNota.Liquidada = false;
                    int nuevoId = ventasDAO.insertarVenta(ventaNota);
                    for (int i = 0; i < filas; i++)
                    {
                        ordenNota[i] = new OrdenNota();
                        ordenNota[i].IdNota = nuevoId;
                        ordenNota[i].IdProducto = Convert.ToInt32(dataGridView1[0, i].Value);
                        ordenNota[i].Cantidad = Convert.ToDouble(dataGridView1[2, i].Value);
                        ordenNota[i].PrecioVenta = Convert.ToDouble(dataGridView1[3, i].Value);
                        ordenNota[i].Importe = Convert.ToDouble(dataGridView1[4, i].Value);
                    }

                }
                else
                {
                    buscarNota.Iva = Convert.ToDouble(textBox5.Text);
                    buscarNota.Total = Convert.ToDouble(textBox1.Text);
                    buscarNota.Subtotal = Convert.ToDouble(textBox6.Text);
                    buscarNota.Estado = 2;
                    buscarNota.Adeudo = 0;
                    buscarNota.IdCliente = 1;
                    buscarNota.Liquidada = false;
                    Boolean actulizarNota = ventasDAO.updateVenta(buscarNota);
                    for (int i = 0; i < filas; i++)
                    {
                        ordenNota[i] = new OrdenNota();
                        ordenNota[i].IdNota = buscarNota.IdNota; ;
                        ordenNota[i].IdProducto = Convert.ToInt32(dataGridView1[0, i].Value);
                        ordenNota[i].Cantidad = Convert.ToDouble(dataGridView1[2, i].Value);
                        ordenNota[i].PrecioVenta = Convert.ToDouble(dataGridView1[3, i].Value);
                        ordenNota[i].Importe = Convert.ToDouble(dataGridView1[4, i].Value);
                    }
                    Boolean borraOrden = ventasDAO.borrarOrden(buscarNota.IdNota);
                }
                Boolean insOrden = ventasDAO.insertarOrden(ordenNota);
                dataGridView1.Rows.Clear();
                textBox2.Focus();
            }
        }
    }
}
