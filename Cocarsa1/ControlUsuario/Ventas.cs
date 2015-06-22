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
using System.Drawing.Printing; 
using System.Management;

namespace Cocarsa1.ControlUsuario
{
    public partial class Ventas : UserControl
    {
        private Boolean flag = false;
        private int columna = 0;
        private int fila = 0;
        private VentaNota cargarNota = new VentaNota();

        public Ventas()
        {
            InitializeComponent();
            limpiarPantalla();
            dataGridView1.Enabled = false;
            checkBox1.Enabled = false;
            checkBox2.Enabled = false;
            textBox2.Focus();
        }

        private void limpiarPantalla()
        {
            dataGridView1.Rows.Clear();
            textBox7.Text = "0";
            textBox1.Text = "0";
            textBox5.Text = "0";
            textBox6.Text = "0";
            textBox3.Clear();
            textBox4.Text = "0";
            textBox8.Clear();
            //cargarNota = null;
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            VentasDAO ventasDao = new VentasDAO();
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;

                if (textBox2.Text.Trim().Equals(""))
                {
                    //insertar nuevo folio                    
                    textBox2.Text = ventasDao.nuevoFolio().ToString();
                    //Moverse al sector siguiente
                    limpiarPantalla();
                    textBox8.Text = "Nota Pendiente";
                    textBox3.Text = "Contado";
                    cargarNota.IdCliente = 1;
                    cargarNota.Adeudo = 0;
                    cargarNota.Subtotal = 0;
                    cargarNota.Total = 0;
                    dataGridView1.Enabled = true;
                    dataGridView1.Focus();
                    dataGridView1.Rows[0].Cells[0].Selected = true;
                    textBox2.Enabled = false;
                    checkBox1.Enabled = true;
                    checkBox1.Checked = true;
                    checkBox2.Enabled = false;
                    checkBox2.Checked = false;
                }
                else
                {
                    //buscar folio
                    VentaNota ventaBuscar = null;
                    //------------cargarNota = null;
                    //Validar numeros
                    try
                    {
                        ventaBuscar = ventasDao.buscarFolio(Convert.ToInt32(textBox2.Text));
                    }
                    catch (Exception error)
                    {
                        textBox2.Clear();
                        MessageBox.Show("SoloNumeros");
                        return;
                    }

                    if (ventaBuscar == null)
                    {
                        MessageBox.Show("No existe la Nota");
                        textBox2.Clear();
                    }
                    else
                    {
                        cargarNota = ventaBuscar;
                        //Mostrar Descripcion Nota
                        limpiarPantalla();
                        textBox7.Text = cargarNota.Adeudo.ToString();
                        textBox1.Text = cargarNota.Subtotal.ToString();
                        textBox5.Text = cargarNota.Iva.ToString();
                        textBox6.Text = cargarNota.Total.ToString();
                        checkBox1.Checked = cargarNota.Liquidada;
                        if (cargarNota.IdCliente == 1)
                        {
                            textBox3.Text = "Contado";
                        }
                        else
                        {
                            textBox3.Text = ventasDao.nombreCliente(cargarNota.IdCliente);
                        }
                        switch (cargarNota.Estado)
                        {
                            case 1:
                                textBox8.Text = "Nota Impresa";
                                dataGridView1.Enabled = false;
                                checkBox1.Enabled = false;
                                checkBox2.Enabled = true;
                                break;
                            case 2:
                                textBox8.Text = "Nota Pendiente";
                                dataGridView1.Enabled = true;
                                dataGridView1.Focus();
                                dataGridView1.Rows[0].Cells[0].Selected = true;
                                textBox2.Enabled = false;
                                checkBox1.Enabled = true;
                                checkBox1.Checked = true;
                                checkBox2.Enabled = false;
                                checkBox2.Checked = false;
                                break;
                            case 3:
                                textBox8.Text = "Nota Facturada";
                                checkBox2.Enabled = false;
                                dataGridView1.Enabled = false;
                                break;
                            case 4:
                                textBox8.Text = "Nota Cancelada";
                                dataGridView1.Enabled = false;
                                checkBox1.Enabled = false;
                                checkBox2.Enabled = false;
                                break;
                        }

                        int i = 0;
                        OrdenNota[] ordenNota = null;

                        ordenNota = ventasDao.cargarNota(cargarNota.IdNota);

                        while (ordenNota[i] != null)
                        {
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
            if (e.KeyCode == Keys.F9)       //CANCELAR NOTA
            {
                if (!textBox2.Text.Trim().Equals(""))
                {
                    if (cargarNota.Estado == 1)
                    {
                        var result = MessageBox.Show("Cancelar", "Desea Cancelar la Nota", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                        if (result == DialogResult.OK)
                        {
                            cargarNota.Estado = 4;
                            Boolean actualizar = ventasDao.updateVenta(cargarNota);
                            limpiarPantalla();
                        }
                    }
                }
            }
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            VentasDAO ventasDao = new VentasDAO();
            Double cantidadTemp;
            Double precioTemp;
            Double importeTemp;
            if (dataGridView1.CurrentCell.Value == null)
            {
                return;
            }

            columna = dataGridView1.CurrentCell.ColumnIndex;
            fila = dataGridView1.CurrentCell.RowIndex;
            if (fila > 10)
            {
                MessageBox.Show("NO SE PUEDEN MÁS DE 11 PRODUCTOS");
                dataGridView1.Rows.RemoveAt(fila);
                calculaTotal();
                columna = 0;
                flag = true;
            }
            else
            {
                switch (dataGridView1.SelectedCells[0].ColumnIndex)
                {
                    case 0:
                        ProductoE productoNuevo = null;
                        int clave;
                        try
                        {
                            clave = Convert.ToInt32(dataGridView1.CurrentCell.Value);
                        }
                        catch (Exception error)
                        {
                            clave = 0;
                        }
                        if (clave != 0)
                        {
                            productoNuevo = ventasDao.cargarProducto(clave);
                            if (productoNuevo == null)
                            {
                                int filaActual = dataGridView1.CurrentCell.RowIndex;
                                MessageBox.Show("No Existe el Producto");
                                dataGridView1.Rows.RemoveAt(filaActual);
                                calculaTotal();
                                columna = 0;
                                flag = true;
                            }
                            else
                            {
                                dataGridView1.CurrentRow.Cells[1].Value = productoNuevo.Nombre;
                                dataGridView1.CurrentRow.Cells[3].Value = productoNuevo.PrecioVenta;
                                columna = 2;
                                flag = true;
                            }
                        }
                        else
                        {
                            int filaActual = dataGridView1.CurrentCell.RowIndex;
                            dataGridView1.Rows.RemoveAt(filaActual);
                            MessageBox.Show("No Existe el Producto");
                            calculaTotal();
                            columna = 0;
                            flag = true;
                        }
                        break;
                    case 2:
                        if (dataGridView1.CurrentRow.Cells[0].Value == null)
                        {
                            MessageBox.Show("Inserta Clave de Producto");
                            int filaActual = dataGridView1.CurrentCell.RowIndex;
                            columna = 0;
                            flag = true;
                            dataGridView1.Rows.RemoveAt(filaActual);
                        }
                        else
                        {
                            try
                            {
                                cantidadTemp = Convert.ToDouble(dataGridView1.CurrentRow.Cells[2].Value.ToString());
                                if (cantidadTemp < 0) {
                                    MessageBox.Show("Solo cifras positivas");
                                    dataGridView1.CurrentRow.Cells[2].Value = 0;
                                    dataGridView1.CurrentRow.Cells[4].Value = 0;
                                    calculaTotal();
                                    flag = true;
                                    return;
                                }
                            }
                            catch (Exception error)
                            {
                                MessageBox.Show("Ingresar Solo numeros");
                                dataGridView1.CurrentRow.Cells[2].Value = 0;
                                dataGridView1.CurrentRow.Cells[4].Value = 0;
                                calculaTotal();
                                flag = true;
                                return;
                            }
                            precioTemp = Convert.ToDouble(dataGridView1.CurrentRow.Cells[3].Value.ToString());
                            importeTemp = precioTemp * cantidadTemp;
                            dataGridView1.CurrentRow.Cells[4].Value = importeTemp;
                            columna = 3;
                            calculaTotal();
                            flag = true;
                        }
                        break;
                    case 3:
                        if (dataGridView1.CurrentRow.Cells[0].Value == null)
                        {
                            MessageBox.Show("Inserta Clave de Producto");
                            columna = 0;
                            flag = true;
                            int filaActual = dataGridView1.CurrentCell.RowIndex;
                            dataGridView1.Rows.RemoveAt(filaActual);
                        }
                        else
                        {
                            cantidadTemp = Convert.ToDouble(dataGridView1.CurrentRow.Cells[2].Value.ToString());
                            try
                            {
                                precioTemp = Convert.ToDouble(dataGridView1.CurrentRow.Cells[3].Value.ToString());
                                if (precioTemp < 0) {
                                    MessageBox.Show("Solo cifras positivas");
                                    dataGridView1.CurrentRow.Cells[3].Value = 0;
                                    dataGridView1.CurrentRow.Cells[4].Value = 0;
                                    calculaTotal();
                                    flag = true;
                                    return;
                                }
                            }
                            catch (Exception error)
                            {
                                MessageBox.Show("Ingresar Solo numeros");
                                dataGridView1.CurrentRow.Cells[3].Value = 0;
                                dataGridView1.CurrentRow.Cells[4].Value = 0;
                                calculaTotal();
                                flag = true;
                                return;
                            }
                            importeTemp = precioTemp * cantidadTemp;
                            dataGridView1.CurrentRow.Cells[4].Value = importeTemp;
                            columna = 0;
                            calculaTotal();
                            fila++;
                            flag = true;
                        }
                        break;
                }
            }
        }

        private void calculaTotal()
        {
            int filas = dataGridView1.Rows.Count;
            double importe = 0;
            for (int i = 0; i < filas; i++)
            {
                importe += Convert.ToDouble(dataGridView1[4, i].Value);
            }
            textBox1.Text = importe.ToString();
            textBox5.Text = "0";
            textBox6.Text = importe.ToString();
            cargarNota.Subtotal = importe;
            cargarNota.Iva = 0;
            cargarNota.Total = importe;
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (flag)
            {
                dataGridView1.CurrentCell = dataGridView1[columna, fila];
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
            if (e.KeyCode == Keys.F10)
            {
                if (cargarNota.Total == 0)
                {
                    MessageBox.Show("No se puede Guardar Nota Vacia");
                    return;
                }
                else
                {
                    terminarNota(2);
                }

            }
            if (e.KeyCode == Keys.F5)
            {
                if (cargarNota.Total == 0)
                {
                    MessageBox.Show("No se puede Guardar Nota Vacia");
                    return;
                }
                else
                {
                    terminarNota(1);
                }
            }
            if (e.KeyCode == Keys.Escape)
            {
                if (cargarNota.Total == 0)
                {
                    dataGridView1.Rows.Clear();
                    textBox2.Enabled = true;
                    textBox2.Clear();
                    textBox2.Focus();
                    dataGridView1.Enabled = false;
                    checkBox1.Enabled = false;
                    checkBox2.Enabled = false;
                }
                else
                {
                    MessageBox.Show("Se necesita eliminar los elementos");
                    return;
                }
            }
            if (e.KeyCode == Keys.F9)
            {
                MessageBox.Show("Solo puede cancelar notas impresas");
            }
        }

        private void terminarNota(int opcion)
        {
            int filas = dataGridView1.Rows.Count - 1;
            int nuevoId = 0;
            OrdenNota[] ordenNota = new OrdenNota[12];
            VentasDAO ventasDAO = new VentasDAO();
            VentaNota ventaNota = new VentaNota();
            VentaNota buscarNota = new VentaNota();

            //Busca si la nota existe
            buscarNota = ventasDAO.buscarFolio(Convert.ToInt32(textBox2.Text));
            //Si no existe aun la nota
            if (buscarNota == null)
            {
                ventaNota.FolioNota = Convert.ToInt32(textBox2.Text);
                ventaNota.Iva = Convert.ToDouble(textBox5.Text);
                ventaNota.Total = Convert.ToDouble(textBox1.Text);
                ventaNota.Subtotal = Convert.ToDouble(textBox6.Text);
                ventaNota.Estado = opcion;
                ventaNota.Adeudo = Convert.ToDouble(textBox7.Text);
                ventaNota.IdCliente = cargarNota.IdCliente;
                ventaNota.Liquidada = checkBox1.Checked;
                nuevoId = ventasDAO.insertarVenta(ventaNota);
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
                //Si existe y hay que actualizarla 
                buscarNota.Iva = Convert.ToDouble(textBox5.Text);
                buscarNota.Total = Convert.ToDouble(textBox1.Text);
                buscarNota.Subtotal = Convert.ToDouble(textBox6.Text);
                buscarNota.Estado = opcion;
                buscarNota.Adeudo = Convert.ToDouble(textBox7.Text);
                buscarNota.IdCliente = cargarNota.IdCliente;
                buscarNota.Liquidada = checkBox1.Checked;
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
            if (opcion == 1)
            {
                MessageBox.Show("DEBERIA DE ESTAR IMRPIMIENDO--->");
                /* AHORITA NO JOVEN
                //Comprobar impresora en linea
                String nombreImpresora = "Citizen GSX-190";
                bool disponible = IsPrinterOnline(nombreImpresora);
                while (!disponible) {
                    MessageBox.Show("No hay impresora");
                    disponible = IsPrinterOnline(nombreImpresora);
                }
                printDocument1.PrinterSettings.PrinterName = nombreImpresora;
                printDocument1.Print();
                 */
            }
            //Se limpia todo y se regresa al inicio
            dataGridView1.Rows.Clear();
            limpiarPantalla();
            textBox2.Enabled = true;
            textBox2.Clear();
            textBox2.Focus();
            dataGridView1.Enabled = false;
            checkBox1.Enabled = false;
            checkBox2.Enabled = false;
            MessageBox.Show("Nota " + textBox2.Text + " Guardada");
        }

        private void textBox4_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                try
                {
                    Double abono = Convert.ToDouble(textBox4.Text);
                    if (abono < 0) {
                        MessageBox.Show("Numeros positivos");
                        textBox4.Clear();
                        return;
                    }
                }catch(Exception error){
                    MessageBox.Show("Solo numeros");
                    textBox4.Clear();
                    return;
                }
                if (Convert.ToDouble(textBox6.Text) < Convert.ToDouble(textBox4.Text))
                {
                    MessageBox.Show("El abono exede la deuda");
                    textBox4.Clear();
                    return;
                }
                Double deuda = (Convert.ToDouble(textBox6.Text)) - (Convert.ToDouble(textBox4.Text));
                textBox7.Text = String.Format("{0:0.00}", deuda);
                cargarNota.Adeudo = Convert.ToDouble(textBox7.Text);
                //Darle la opcion de acabarla nota IMPRIMIR o seguir editando
                var result = MessageBox.Show("Continuar", "Desea Terminar la Nota", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (result == DialogResult.OK)
                {
                    //Imprimir
                    checkBox2.Visible = true;
                    textBox4.Enabled = false;
                    terminarNota(1);
                    checkBox1.Checked = true;
                    limpiarPantalla();
                }
                else
                {
                    textBox4.Text = "0";
                    textBox7.Text = "0";
                    cargarNota.Adeudo = 0;
                    textBox3.Text = "Contado";
                    cargarNota.IdCliente = 1;
                    textBox4.Enabled = false;
                    checkBox1.Enabled = true;
                    checkBox1.Checked = true;
                    checkBox2.Visible = true;
                    dataGridView1.Focus();
                    dataGridView1.CurrentCell.Selected = true;
                }

            }
        }

        private void checkBox1_MouseDown(object sender, MouseEventArgs e)
        {
            Boolean estadoCheckBox1 = checkBox1.Checked;
            Entidades.Cliente cliente = null;

            if (Convert.ToDouble(textBox6.Text) == 0)
            {
                MessageBox.Show("Necesitas llenar la nota");
                checkBox1.Checked = true;
                return;
            }
            checkBox1.Checked = !estadoCheckBox1;
            if (checkBox1.Checked == false)
            {
                Popup popup = new Popup();
                checkBox1.Enabled = false;
                if (popup.ShowDialog() == DialogResult.OK)
                {
                    cliente = popup.ClienteSeleccionado;
                    cargarNota.IdCliente = cliente.IdCliente;
                    textBox3.Text = cliente.Nombre + " " + cliente.APaterno + " " + cliente.AMaterno;
                    checkBox2.Visible = false;
                    textBox4.Enabled = true;
                    textBox4.Focus();
                }
                else
                {
                    MessageBox.Show("Selecciona Cliente");
                    textBox4.Text = "0";
                    textBox7.Text = "0";
                    cargarNota.Adeudo = 0;
                    textBox3.Text = "Contado";
                    cargarNota.IdCliente = 1;
                    textBox4.Enabled = false;
                    checkBox1.Enabled = true;
                    checkBox1.Checked = true;
                    checkBox2.Visible = true;
                    dataGridView1.Focus();
                    dataGridView1.CurrentCell.Selected = true;
                }
            }
        }

        public string enletras(string num)
        {
            string res, dec = "";
            Int64 entero;
            int decimales;
            double nro;

            try
            {
                nro = Convert.ToDouble(num);
            }
            catch
            {
                return "";
            }

            entero = Convert.ToInt64(Math.Truncate(nro));
            decimales = Convert.ToInt32(Math.Round((nro - entero) * 100, 2));
            if (decimales > 0)
            {
                dec = " CON " + decimales.ToString() + "/100 M.N.";
            }
            res = toText(Convert.ToDouble(entero)) + dec;
            return res;
        }

        private string toText(double value)
        {
            string Num2Text = "";
            value = Math.Truncate(value);
            if (value == 0) Num2Text = "CERO";
            else if (value == 1) Num2Text = "UNO";
            else if (value == 2) Num2Text = "DOS";
            else if (value == 3) Num2Text = "TRES";
            else if (value == 4) Num2Text = "CUATRO";
            else if (value == 5) Num2Text = "CINCO";
            else if (value == 6) Num2Text = "SEIS";
            else if (value == 7) Num2Text = "SIETE";
            else if (value == 8) Num2Text = "OCHO";
            else if (value == 9) Num2Text = "NUEVE";
            else if (value == 10) Num2Text = "DIEZ";
            else if (value == 11) Num2Text = "ONCE";
            else if (value == 12) Num2Text = "DOCE";
            else if (value == 13) Num2Text = "TRECE";
            else if (value == 14) Num2Text = "CATORCE";
            else if (value == 15) Num2Text = "QUINCE";
            else if (value < 20) Num2Text = "DIECI" + toText(value - 10);
            else if (value == 20) Num2Text = "VEINTE";
            else if (value < 30) Num2Text = "VEINTI" + toText(value - 20);
            else if (value == 30) Num2Text = "TREINTA";
            else if (value == 40) Num2Text = "CUARENTA";
            else if (value == 50) Num2Text = "CINCUENTA";
            else if (value == 60) Num2Text = "SESENTA";
            else if (value == 70) Num2Text = "SETENTA";
            else if (value == 80) Num2Text = "OCHENTA";
            else if (value == 90) Num2Text = "NOVENTA";
            else if (value < 100) Num2Text = toText(Math.Truncate(value / 10) * 10) + " Y " + toText(value % 10);
            else if (value == 100) Num2Text = "CIEN";
            else if (value < 200) Num2Text = "CIENTO " + toText(value - 100);
            else if ((value == 200) || (value == 300) || (value == 400) || (value == 600) || (value == 800)) Num2Text = toText(Math.Truncate(value / 100)) + "CIENTOS";
            else if (value == 500) Num2Text = "QUINIENTOS";
            else if (value == 700) Num2Text = "SETECIENTOS";
            else if (value == 900) Num2Text = "NOVECIENTOS";
            else if (value < 1000) Num2Text = toText(Math.Truncate(value / 100) * 100) + " " + toText(value % 100);
            else if (value == 1000) Num2Text = "MIL";
            else if (value < 2000) Num2Text = "MIL " + toText(value % 1000);
            else if (value < 1000000)
            {
                Num2Text = toText(Math.Truncate(value / 1000)) + " MIL";
                if ((value % 1000) > 0) Num2Text = Num2Text + " " + toText(value % 1000);
            }

            else if (value == 1000000) Num2Text = "UN MILLON";
            else if (value < 2000000) Num2Text = "UN MILLON " + toText(value % 1000000);
            else if (value < 1000000000000)
            {
                Num2Text = toText(Math.Truncate(value / 1000000)) + " MILLONES ";
                if ((value - Math.Truncate(value / 1000000) * 1000000) > 0) Num2Text = Num2Text + " " + toText(value - Math.Truncate(value / 1000000) * 1000000);
            }

            else if (value == 1000000000000) Num2Text = "UN BILLON";
            else if (value < 2000000000000) Num2Text = "UN BILLON " + toText(value - Math.Truncate(value / 1000000000000) * 1000000000000);

            else
            {
                Num2Text = toText(Math.Truncate(value / 1000000000000)) + " BILLONES";
                if ((value - Math.Truncate(value / 1000000000000) * 1000000000000) > 0) Num2Text = Num2Text + " " + toText(value - Math.Truncate(value / 1000000000000) * 1000000000000);
            }
            return Num2Text;

        }

        public bool IsPrinterOnline(string printerName)
        {
            string str = "";
            bool online = false;
            //set the scope of this search to the local machine
            ManagementScope scope = new ManagementScope(ManagementPath.DefaultPath);
            //connect to the machine
            scope.Connect();
            //query for the ManagementObjectSearcher
            SelectQuery query = new SelectQuery("select * from Win32_Printer");
            ManagementClass m = new ManagementClass("Win32_Printer");
            ManagementObjectSearcher obj = new ManagementObjectSearcher(scope, query);
            //get each instance from the ManagementObjectSearcher object
            using (ManagementObjectCollection printers = m.GetInstances())
                //now loop through each printer instance returned
                foreach (ManagementObject printer in printers)
                {
                    //first make sure we got something back
                    if (printer != null)
                    {
                        //get the current printer name in the loop
                        str = printer["Name"].ToString().ToLower();
                        //check if it matches the name provided
                        if (str.Equals(printerName.ToLower()))
                        {
                            //since we found a match check it's status
                            if (printer["WorkOffline"].ToString().ToLower().Equals("true") || printer["PrinterStatus"].Equals(7))
                                //it's offline
                                online = false;
                            else
                                //it's online
                                online = true;
                        }
                    }
                    else
                        throw new Exception("No printers were found");
                }
            return online;
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Font NewFont = new Font("Arial", 14, FontStyle.Regular);
            Font fuente2 = new Font("Arial", 10, FontStyle.Regular);
            int filas = dataGridView1.Rows.Count - 1;
            String folio = textBox2.Text;
            e.Graphics.DrawString(folio, NewFont, Brushes.Black, 340, 50);
            if (checkBox1.Checked == false)
            {
                String cliente = textBox3.Text;
                e.Graphics.DrawString(cliente, NewFont, Brushes.Black, 25, 50);
                String adeudo = textBox7.Text;
                e.Graphics.DrawString("Adeudo: " + adeudo, NewFont, Brushes.Black, 25, 70);
                String abono = textBox4.Text;
                e.Graphics.DrawString("Abono: " + abono, NewFont, Brushes.Black, 25, 90);
            }
            String fecha = dateTimePicker4.Text;
            e.Graphics.DrawString(fecha, NewFont, Brushes.Black, 25, 120);
            String total = textBox6.Text;
            String totalEscrito = enletras(total);
            //Recorrer todo el grid
            for (int i = 0; i < filas; i++)
            {
                String IdProducto = dataGridView1[0, i].Value.ToString();
                String Nombre = dataGridView1[1, i].Value.ToString();
                String Cantidad = dataGridView1[2, i].Value.ToString();
                String PrecioVenta = dataGridView1[3, i].Value.ToString();
                String Importe = dataGridView1[4, i].Value.ToString();
                e.Graphics.DrawString(Cantidad, fuente2, Brushes.Black, 25, 300 + (i * 13));
                e.Graphics.DrawString(IdProducto, fuente2, Brushes.Black, 122, 300 + (i * 13));
                e.Graphics.DrawString(Nombre, fuente2, Brushes.Black, 170, 300 + (i * 13));
                e.Graphics.DrawString(PrecioVenta, fuente2, Brushes.Black, 600, 300 + (i * 13));
                e.Graphics.DrawString(Importe, fuente2, Brushes.Black, 675, 300 + (i * 13));
            }
            e.Graphics.DrawString(total, NewFont, Brushes.Black, 675, 500);
            e.Graphics.DrawString(totalEscrito, fuente2, Brushes.Black, 122, 510);
            limpiarPantalla();
        }

        private void checkBox2_MouseDown(object sender, MouseEventArgs e)
        {
            Boolean estadoCheckBox2 = checkBox2.Checked;
            VentasDAO ventasDAO = new VentasDAO();
            VentaNota ventaNota = new VentaNota();
            if (Convert.ToDouble(textBox7.Text) != 0)
            {
                MessageBox.Show("Las Notas con adeudo no se puede Facturar");
                textBox2.Focus();
            }
            else
            {
                Entidades.Cliente cliente = null;
                // De lo contrario solo cambia el estado de la nota a Facturada
                if (cargarNota.IdCliente == 1)
                {
                    // Se ejecuta solo si el cliente es diferente de contado
                    Popup popup = new Popup();
                    checkBox2.Enabled = false;
                    if (popup.ShowDialog() == DialogResult.OK)
                    {
                        cliente = popup.ClienteSeleccionado;
                        //Revisar que el Cliente no sea Contado
                        cargarNota.IdCliente = cliente.IdCliente;
                        textBox3.Text = cliente.Nombre + " " + cliente.APaterno + " " + cliente.AMaterno;
                    }
                    else
                    {
                        MessageBox.Show("Selecciona Cliente");
                        textBox3.Text = "Contado";
                        cargarNota.IdCliente = 1;
                        checkBox2.Enabled = true;
                        checkBox2.Checked = false;
                        textBox2.Focus();
                        return;
                    }
                }
                //Actualizar estado de la nota */* IdCliente
                cargarNota.Estado = 3;
                Boolean actualizar = ventasDAO.updateVenta(cargarNota);
                checkBox2.Enabled = false;
                checkBox2.Checked = false;
                limpiarPantalla();
                MessageBox.Show("NOTA FACTURADA");
                textBox2.Focus();
            }
        }

    }
}