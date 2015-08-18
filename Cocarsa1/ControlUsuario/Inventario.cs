using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Cocarsa1.Entidades;
using Cocarsa1.ConexionBD;

namespace Cocarsa1.ControlUsuario
{
    public partial class Inventario : UserControl
    {
        private int columna1 = 0;
        private int fila1 = 0;

        private int columna2 = 0;
        private int fila2 = 0;

        private Boolean flag1 = false;
        private Boolean flag2 = false;

        private InventarioDao inv_dao = null;
        private ProductoDao prod_dao = null;

        public Inventario()
        {
            InitializeComponent();

            inv_dao = new InventarioDao();
            prod_dao = new ProductoDao();

            inv_dao.copiaRegistroExistencia();

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                row.Height = 32;
            }

            foreach (DataGridViewRow row in dataGridView2.Rows)
            {
                row.Height = 32;
            }

            foreach (DataGridViewRow row in dataGridView3.Rows)
            {
                row.Height = 32;
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (flag1)
            {
                dataGridView1.CurrentCell = dataGridView1[columna1, fila1];
                flag1 = false;
            }
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            int id = 0;

            if (dataGridView1.Columns[e.ColumnIndex].Name == "frio_col1")
            {
                if (dataGridView1.CurrentCell.Value == null)
                {
                    return;
                }

                fila1 = dataGridView1.CurrentCell.RowIndex;
                dataGridView1.Rows[fila1].DefaultCellStyle.BackColor = Color.White;

                try
                {
                    id = Convert.ToInt32(dataGridView1.CurrentCell.Value);
                }
                catch (Exception error)
                {
                    id = 0;
                    error.ToString();

                    MessageBox.Show("Ingresa un valor numérico.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    dataGridView1.Rows.RemoveAt(fila1);
                    flag1 = true;
                    return;
                }

                ProductoE prod = prod_dao.obtenerProducto(id);

                if (prod != null)
                {
                    dataGridView1.CurrentRow.Cells[1].Value = prod.Nombre;
                    columna1 = 2;
                    flag1 = true;
                }
                else
                {
                    MessageBox.Show("El producto no existe.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    dataGridView1.Rows.RemoveAt(fila1);
                    columna1 = 0;
                    flag1 = true;
                    return;
                }
            }
            else if (dataGridView1.Columns[e.ColumnIndex].Name == "frio_col3")
            {
                double kilogramos = 0;
                fila1 = dataGridView1.CurrentCell.RowIndex;
                columna1 = dataGridView1.CurrentCell.ColumnIndex;

                dataGridView1.Rows[fila1].DefaultCellStyle.BackColor = Color.White;

                if (dataGridView1.CurrentCell.Value == null)
                {
                    return;
                }
                else
                {
                    if (dataGridView1.CurrentRow.Cells[0].Value == null)
                    {
                        MessageBox.Show("Ingresa la clave del producto.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        dataGridView1.Rows.RemoveAt(fila1);
                        columna1 = 0;
                        flag1 = true;
                        return;
                    }

                    try
                    {
                        kilogramos = Convert.ToDouble(dataGridView1.CurrentCell.Value);
                        if (kilogramos <= 0)
                        {
                            MessageBox.Show("Ingresa una cantidad mayor a cero.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            dataGridView1.CurrentCell.Value = null;
                            flag1 = true;
                            return;
                        }

                    }
                    catch (Exception error)
                    {
                        MessageBox.Show("Ingresa un valor numérico.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        dataGridView1.CurrentCell.Value = null;
                        error.ToString();
                        flag1 = true;
                        return;
                    }
                }
                columna1 = 0;
                fila1 += 1;
                flag1 = true;
            }
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                if (dataGridView1.CurrentRow.Cells[0].Value != null && dataGridView1.CurrentRow.Cells[2].Value == null)
                {
                    MessageBox.Show("Ingresa la cantidad de kilos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    columna1 = 2;
                    flag1 = true;
                    return;
                }
            }
            else if (e.KeyCode == Keys.F10)
            {
                if (dataGridView1.Rows.Count == 1)
                {
                    MessageBox.Show("Al menos debes agregar un registro para guardar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.Cells[2].Value == null && row.Cells[0].Value != null)
                    {
                        MessageBox.Show("Hay registros sin completar en la tabla.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                DialogResult ans = MessageBox.Show("¿Desea guardar los cambios?", "Guardar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (ans == DialogResult.No)
                    return;
                else
                {
                    bool err = false;
                    int indicador = 0;

                    for (int x = 0; x < dataGridView1.Rows.Count - 1; x++)
                    {
                        DataGridViewRow row = dataGridView1.Rows[x];

                        Existencia registro = new Existencia();
                        registro.IdProducto = Convert.ToInt32(row.Cells[0].Value);
                        registro.Cantidad = Convert.ToDouble(row.Cells[2].Value);
                        registro.Fecha = DateTime.Now;

                        if ((indicador = inv_dao.guardaRegistroFrio(registro)) == 2)
                        {
                            dataGridView1.Rows.RemoveAt(x);
                        }
                        else if (indicador == 1)
                        {
                            dataGridView1.Rows[x].DefaultCellStyle.BackColor = Color.FromArgb(255, 246, 223);
                        }
                        else if (indicador == 0)
                        {
                            dataGridView1.Rows[x].DefaultCellStyle.BackColor = Color.FromArgb(255, 228, 228);
                        }
                        else if (indicador == -1)
                            err = true;
                    }
                    if (err)
                    {
                        MessageBox.Show("Error de conexion con la base de datos.", "MySQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void dataGridView1_Enter(object sender, EventArgs e)
        {
            dataGridView2.ClearSelection();
            dataGridView3.ClearSelection();
        }

        private void dataGridView2_Enter(object sender, EventArgs e)
        {
            dataGridView1.ClearSelection();
            dataGridView3.ClearSelection();
        }

        private void dataGridView3_Enter(object sender, EventArgs e)
        {
            dataGridView1.ClearSelection();
            dataGridView2.ClearSelection();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataGridView3.Rows.Clear();
        }

        private void dateTimePicker2_CloseUp(object sender, EventArgs e)
        {
            dataGridView3.Rows.Clear();
            consultarExistencia();
        }

        private void dateTimePicker2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                dataGridView3.Rows.Clear();
                consultarExistencia();
            }
            if (e.KeyCode == Keys.F7)
            {
                int filas = dataGridView3.Rows.Count - 1;
                if (filas < 1)
                {
                    MessageBox.Show("No existe Registro");
                }
                else
                {
                    imprimirExistencia();
                }
            }
        }

        private void consultarExistencia()
        {
            ReportesDAO reportesDAO = new ReportesDAO();
            Existencia[] existencia = null;
            int i = 0;
            dateTimePicker2.Format = DateTimePickerFormat.Custom;
            dateTimePicker2.CustomFormat = "yyyy-MM-dd";
            String fechaSel = dateTimePicker2.Text;
            dateTimePicker2.Format = DateTimePickerFormat.Long;
            int numeroReg = 0;
            numeroReg = reportesDAO.numeroConsulta(fechaSel, comboBox1.SelectedIndex);
            existencia = reportesDAO.consultaExistencia(fechaSel, numeroReg, comboBox1.SelectedIndex);
            while (i < numeroReg)
            {
                DataGridViewRow row = (DataGridViewRow)dataGridView3.Rows[0].Clone();
                row.Cells[0].Value = existencia[i].IdProducto;
                row.Cells[1].Value = reportesDAO.nombreProducto(existencia[i].IdProducto);
                row.Cells[2].Value = existencia[i].Cantidad;
                dataGridView3.Rows.Add(row);
                i++;
            }
        }

        private void imprimirExistencia()
        {
            Document document = new Document(PageSize.LETTER);
            PdfWriter writer;
            try
            {
                writer = PdfWriter.GetInstance(document, new FileStream(@"D:\Reportes\ " + comboBox1.SelectedItem.ToString() + " " + dateTimePicker2.Value.ToLongDateString() + ".pdf", FileMode.Create));
            }
            catch (Exception errorDoc)
            {
                MessageBox.Show("No se puede crear documento, esta siendo utilizado por otro programa");
                return;
            }
            document.AddTitle("Reportes Cocarsa");
            document.AddCreator("Cocarsa Tecamac");
            document.Open();
            iTextSharp.text.Font _standardFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
            // Escribimos el encabezamiento en el documento
            document.Add(new Paragraph("Registro de " + comboBox1.SelectedItem.ToString() + " de " + dateTimePicker2.Value.ToLongDateString()));
            document.Add(Chunk.NEWLINE);

            // Creamos una tabla que contendrá el nombre, apellido y país
            // de nuestros visitante.
            PdfPTable tablaExistencia = new PdfPTable(3);
            tablaExistencia.WidthPercentage = 100;

            // Configuramos el título de las columnas de la tabla
            PdfPCell clClave = new PdfPCell(new Phrase("Clave", _standardFont));
            clClave.BorderWidth = 0;
            clClave.BorderWidthBottom = 0.75f;

            PdfPCell clNombre = new PdfPCell(new Phrase("Nombre", _standardFont));
            clNombre.BorderWidth = 0;
            clNombre.BorderWidthBottom = 0.75f;

            PdfPCell clCantidad = new PdfPCell(new Phrase("Cantidad", _standardFont));
            clCantidad.BorderWidth = 0;
            clCantidad.BorderWidthBottom = 0.75f;

            // Añadimos las celdas a la tabla
            tablaExistencia.AddCell(clClave);
            tablaExistencia.AddCell(clNombre);
            tablaExistencia.AddCell(clCantidad);

            int filas = dataGridView3.Rows.Count - 1;

            for (int i = 0; i < filas; i++)
            {
                // Llenamos la tabla con información
                clClave = new PdfPCell(new Phrase(dataGridView3[0, i].Value.ToString(), _standardFont));
                clClave.BorderWidth = 0;

                clNombre = new PdfPCell(new Phrase(dataGridView3[1, i].Value.ToString(), _standardFont));
                clNombre.BorderWidth = 0;

                clCantidad = new PdfPCell(new Phrase(dataGridView3[2, i].Value.ToString(), _standardFont));
                clCantidad.BorderWidth = 0;

                // Añadimos las celdas a la tabla
                tablaExistencia.AddCell(clClave);
                tablaExistencia.AddCell(clNombre);
                tablaExistencia.AddCell(clCantidad);
            }
            // Finalmente, añadimos la tabla al documento PDF y cerramos el documento
            document.Add(tablaExistencia);
            document.Close();
            writer.Close();
            MessageBox.Show("Registro Generado !!!");
        }

        private void dataGridView2_SelectionChanged(object sender, EventArgs e)
        {
            if (flag2)
            {
                dataGridView2.CurrentCell = dataGridView2[columna2, fila2];
                flag2 = false;
            }
        }

        private void dataGridView2_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            int id = 0;

            if (dataGridView2.Columns[e.ColumnIndex].Name == "fresco_col1")
            {
                if (dataGridView2.CurrentCell.Value == null)
                {
                    return;
                }

                fila2 = dataGridView2.CurrentCell.RowIndex;
                dataGridView2.Rows[fila2].DefaultCellStyle.BackColor = Color.White;

                try
                {
                    id = Convert.ToInt32(dataGridView2.CurrentCell.Value);
                }
                catch (Exception error)
                {
                    id = 0;
                    error.ToString();

                    MessageBox.Show("Ingresa un valor numérico.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    dataGridView2.Rows.RemoveAt(fila2);
                    flag2 = true;
                    return;
                }

                ProductoE prod = prod_dao.obtenerProducto(id);

                if (prod != null)
                {
                    dataGridView2.CurrentRow.Cells[1].Value = prod.Nombre;
                    columna2 = 2;
                    flag2 = true;
                }
                else
                {
                    MessageBox.Show("El producto no existe.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    dataGridView2.Rows.RemoveAt(fila2);
                    columna2 = 0;
                    flag2 = true;
                    return;
                }
            }
            else if (dataGridView2.Columns[e.ColumnIndex].Name == "fresco_col3")
            {
                double kilogramos = 0;
                fila2 = dataGridView2.CurrentCell.RowIndex;
                columna2 = dataGridView2.CurrentCell.ColumnIndex;

                dataGridView2.Rows[fila2].DefaultCellStyle.BackColor = Color.White;

                if (dataGridView2.CurrentCell.Value == null)
                {
                    return;
                }
                else
                {
                    if (dataGridView2.CurrentRow.Cells[0].Value == null)
                    {
                        MessageBox.Show("Ingresa la clave del producto.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        dataGridView2.Rows.RemoveAt(fila1);
                        columna2 = 0;
                        flag2 = true;
                        return;
                    }

                    try
                    {
                        kilogramos = Convert.ToDouble(dataGridView2.CurrentCell.Value);
                        if (kilogramos <= 0)
                        {
                            MessageBox.Show("Ingresa una cantidad mayor a cero.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            dataGridView2.CurrentCell.Value = null;
                            flag2 = true;
                            return;
                        }

                    }
                    catch (Exception error)
                    {
                        MessageBox.Show("Ingresa un valor numérico.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        dataGridView2.CurrentCell.Value = null;
                        error.ToString();
                        flag2 = true;
                        return;
                    }
                }
                columna2 = 0;
                fila2 += 1;
                flag2 = true;
            }
        }

        private void dataGridView2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                if (dataGridView2.CurrentRow.Cells[0].Value != null && dataGridView2.CurrentRow.Cells[2].Value == null)
                {
                    MessageBox.Show("Ingresa la cantidad de kilos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    columna2 = 2;
                    flag2 = true;
                    return;
                }
            }
            else if (e.KeyCode == Keys.F10)
            {
                if (dataGridView2.Rows.Count == 1)
                {
                    MessageBox.Show("Al menos debes agregar un registro para guardar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                foreach (DataGridViewRow row in dataGridView2.Rows)
                {
                    if (row.Cells[2].Value == null && row.Cells[0].Value != null)
                    {
                        MessageBox.Show("Hay registros sin completar en la tabla.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                DialogResult ans = MessageBox.Show("¿Desea guardar los cambios?", "Guardar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (ans == DialogResult.No)
                    return;
                else
                {
                    bool err = false;
                    int indicador = 0;

                    for (int x = 0; x < dataGridView2.Rows.Count - 1; x++)
                    {
                        DataGridViewRow row = dataGridView2.Rows[x];

                        Existencia registro = new Existencia();
                        registro.IdProducto = Convert.ToInt32(row.Cells[0].Value);
                        registro.Cantidad = Convert.ToDouble(row.Cells[2].Value);
                        registro.Fecha = DateTime.Now;

                        if ((indicador = inv_dao.guardaRegistroFrio(registro)) == 2)
                        {
                            dataGridView2.Rows.RemoveAt(x);
                        }
                        else if (indicador == 1)
                        {
                            dataGridView2.Rows[x].DefaultCellStyle.BackColor = Color.FromArgb(255, 246, 223);
                        }
                        else if (indicador == 0)
                        {
                            dataGridView2.Rows[x].DefaultCellStyle.BackColor = Color.FromArgb(255, 228, 228);
                        }
                        else if (indicador == -1)
                            err = true;
                    }
                    if (err)
                    {
                        MessageBox.Show("Error de conexion con la base de datos.", "MySQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

    }
}
