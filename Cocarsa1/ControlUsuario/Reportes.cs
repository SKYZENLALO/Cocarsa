using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO; 
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
            dateTimePicker2.MaxDate = DateTime.Today;
            dateTimePicker1.Value = DateTime.Today;
            dateTimePicker2.Value = DateTime.Today;
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox1.SelectedIndex = 0;
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
            if (e.KeyCode == Keys.F6) {
                int filas = dataGridView1.Rows.Count - 1;
                if (filas < 1) {
                    MessageBox.Show("No existe Registro");
                }
                else
                {
                    imprimirPDF();
                }
            }
        }
        private void consultar() {
            String estado = "";
            String canceladas = "";
            int idIni = 0;
            int idFin = 0;
            int numImp = 0;
            int numPen = 0;
            int numFac = 0;
            int numCan = 0;
            Double totSub = 0;
            Double totImp = 0;
            Double kilos = 0;
            Double importe = 0;
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
            // No sumar notas canceladas
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
                row.Cells[3].Value = ventaNota[i].Iva;
                row.Cells[4].Value = ventaNota[i].Total;
                if (ventaNota[i].Estado != 4)
                {
                    totSub += ventaNota[i].Subtotal;
                    totImp += ventaNota[i].Total;
                }
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
                        canceladas += " && idNota != "+ventaNota[i].IdNota;
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
            dataGridView2.Rows.Clear();
            if (idIni != 0) {
                i = 0;
                idIni--;
                idFin++;
                String query = "SELECT count(distinct(IdProducto)) FROM ordenventa where idNota > "+idIni+" && idNota < "+idFin+canceladas+";";
                String query2 = "SELECT IdProducto,sum(cantidad),sum(importe) FROM ordenventa where idNota > "+idIni+" && idNota < "+idFin+canceladas+" group by IdProducto;";
                int numProductos = reportesDAO.numeroProductos(query);
                OrdenNota[] ordenNota = null;
                ordenNota = reportesDAO.consultaOrden(query2,numProductos);
                while (i  < numProductos)
                {
                    DataGridViewRow row = (DataGridViewRow)dataGridView2.Rows[0].Clone();
                    row.Cells[0].Value = ordenNota[i].IdProducto;
                    row.Cells[1].Value = reportesDAO.nombreProducto(ordenNota[i].IdProducto);
                    row.Cells[2].Value = ordenNota[i].Cantidad;
                    kilos += ordenNota[i].Cantidad;
                    row.Cells[3].Value = ordenNota[i].Importe;
                    importe += ordenNota[i].Importe;
                    dataGridView2.Rows.Add(row);
                    i++;
                }
            }
            textBox8.Text = kilos.ToString();
            textBox9.Text = importe.ToString();
        }
        private void imprimirPDF() {
            Document document = new Document(PageSize.LETTER);
            PdfWriter writer;
            try
            {
                writer = PdfWriter.GetInstance(document, new FileStream(@"D:\Archivos\Eduardo\Desktop\Reporte " + dateTimePicker1.Value.ToLongDateString() + ".pdf", FileMode.Create));
            }catch(Exception errorDoc){
                MessageBox.Show("No se puede crear archivo, esta siendo usado por otro programa");
                return;
            }
            document.AddTitle("Reportes Cocarsa");
            document.AddCreator("Cocarsa Tecamac");
            document.Open();
            iTextSharp.text.Font _standardFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
            // Escribimos el encabezamiento en el documento
            document.Add(new Paragraph("Reporte de Ventas de " + dateTimePicker1.Value.ToLongDateString()));
            document.Add(Chunk.NEWLINE);

            // Creamos una tabla que contendrá el nombre, apellido y país
            // de nuestros visitante.
            PdfPTable tablaVentas = new PdfPTable(6);
            tablaVentas.WidthPercentage = 100;

            // Configuramos el título de las columnas de la tabla
            PdfPCell clFolio = new PdfPCell(new Phrase("Folio", _standardFont));
            clFolio.BorderWidth = 0;
            clFolio.BorderWidthBottom = 0.75f;

            PdfPCell clCliente = new PdfPCell(new Phrase("Cliente", _standardFont));
            clCliente.BorderWidth = 0;
            clCliente.BorderWidthBottom = 0.75f;

            PdfPCell clSubtotal = new PdfPCell(new Phrase("Subtotal", _standardFont));
            clSubtotal.BorderWidth = 0;
            clSubtotal.BorderWidthBottom = 0.75f;

            PdfPCell clIva = new PdfPCell(new Phrase("IVA", _standardFont));
            clIva.BorderWidth = 0;
            clIva.BorderWidthBottom = 0.75f;

            PdfPCell clImporte = new PdfPCell(new Phrase("Importe", _standardFont));
            clImporte.BorderWidth = 0;
            clImporte.BorderWidthBottom = 0.75f;

            PdfPCell clImpresa = new PdfPCell(new Phrase("Impresa", _standardFont));
            clImpresa.BorderWidth = 0;
            clImpresa.BorderWidthBottom = 0.75f;
            
            // Añadimos las celdas a la tabla
            tablaVentas.AddCell(clFolio);
            tablaVentas.AddCell(clCliente);
            tablaVentas.AddCell(clSubtotal);
            tablaVentas.AddCell(clIva);
            tablaVentas.AddCell(clImporte);
            tablaVentas.AddCell(clImpresa);

            int filas = dataGridView1.Rows.Count - 1;

            for (int i = 0; i < filas; i++)
            {
                // Llenamos la tabla con información
                clFolio = new PdfPCell(new Phrase(dataGridView1[0, i].Value.ToString(), _standardFont));
                clFolio.BorderWidth = 0;

                clCliente = new PdfPCell(new Phrase(dataGridView1[1, i].Value.ToString(), _standardFont));
                clCliente.BorderWidth = 0;

                clSubtotal = new PdfPCell(new Phrase(dataGridView1[2, i].Value.ToString(), _standardFont));
                clSubtotal.BorderWidth = 0;

                clIva = new PdfPCell(new Phrase(dataGridView1[3, i].Value.ToString(), _standardFont));
                clIva.BorderWidth = 0;

                clImporte = new PdfPCell(new Phrase(dataGridView1[4, i].Value.ToString(), _standardFont));
                clImporte.BorderWidth = 0;
                String estadoImpresa = "";
                if (dataGridView1[5, i].Value.ToString().Equals("Pendiente"))
                {
                    estadoImpresa = "No";
                }
                else {
                    estadoImpresa = "Si";
                }
                clImpresa = new PdfPCell(new Phrase(estadoImpresa, _standardFont));
                clImpresa.BorderWidth = 0;

                // Añadimos las celdas a la tabla
                tablaVentas.AddCell(clFolio);
                tablaVentas.AddCell(clCliente);
                tablaVentas.AddCell(clSubtotal);
                tablaVentas.AddCell(clIva);
                tablaVentas.AddCell(clImporte);
                tablaVentas.AddCell(clImpresa);
            }
            // Finalmente, añadimos la tabla al documento PDF y cerramos el documento
            document.Add(tablaVentas);
            document.Add(new Paragraph("Total de Ventas: $" + textBox7.Text, _standardFont));
            document.Add(Chunk.NEWLINE);

            document.Add(new Paragraph("Reporte de Productos de " + dateTimePicker1.Value.ToLongDateString()));
            document.Add(Chunk.NEWLINE);

            PdfPTable tablaProductos = new PdfPTable(4);
            tablaVentas.WidthPercentage = 120;

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

            PdfPCell clImporte2 = new PdfPCell(new Phrase("Importe", _standardFont));
            clImporte2.BorderWidth = 0;
            clImporte2.BorderWidthBottom = 0.75f;

            // Añadimos las celdas a la tabla
            tablaProductos.AddCell(clClave);
            tablaProductos.AddCell(clNombre);
            tablaProductos.AddCell(clCantidad);
            tablaProductos.AddCell(clImporte2);

            int filas2 = dataGridView2.Rows.Count - 1;

            for (int i = 0; i < filas2; i++)
            {
                // Llenamos la tabla con información
                clClave = new PdfPCell(new Phrase(dataGridView2[0, i].Value.ToString(), _standardFont));
                clClave.BorderWidth = 0;

                clNombre = new PdfPCell(new Phrase(dataGridView2[1, i].Value.ToString(), _standardFont));
                clNombre.BorderWidth = 0;

                clCantidad = new PdfPCell(new Phrase(dataGridView2[2, i].Value.ToString(), _standardFont));
                clCantidad.BorderWidth = 0;

                clImporte2 = new PdfPCell(new Phrase(dataGridView2[3, i].Value.ToString(), _standardFont));
                clImporte2.BorderWidth = 0;

                // Añadimos las celdas a la tabla
                tablaProductos.AddCell(clClave);
                tablaProductos.AddCell(clNombre);
                tablaProductos.AddCell(clCantidad);
                tablaProductos.AddCell(clImporte2);
            }
            // Finalmente, añadimos la tabla al documento PDF y cerramos el documento
            document.Add(tablaProductos);
            document.Add(new Paragraph("Total de Ventas: $" + textBox9.Text, _standardFont));
            document.Add(Chunk.NEWLINE);

            document.Close();
            writer.Close();
            MessageBox.Show("Reporte Generado !!!");
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
        private void consultarExistencia() {
            ReportesDAO reportesDAO = new ReportesDAO();
            Existencia[] existencia = null;
            int i = 0;
            dateTimePicker2.Format = DateTimePickerFormat.Custom;
            dateTimePicker2.CustomFormat = "yyyy-MM-dd";
            String fechaSel = dateTimePicker2.Text;
            dateTimePicker2.Format = DateTimePickerFormat.Long;
            int numeroReg = 0;
            numeroReg = reportesDAO.numeroConsulta(fechaSel,comboBox1.SelectedIndex);
            existencia = reportesDAO.consultaExistencia(fechaSel, numeroReg, comboBox1.SelectedIndex);
            while (i < numeroReg) {
                DataGridViewRow row = (DataGridViewRow)dataGridView3.Rows[0].Clone();
                row.Cells[0].Value = existencia[i].IdProducto;
                row.Cells[1].Value = reportesDAO.nombreProducto(existencia[i].IdProducto);
                row.Cells[2].Value = existencia[i].Cantidad;
                dataGridView3.Rows.Add(row);
                i++;
            }
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
        private void imprimirExistencia()
        {
            Document document = new Document(PageSize.LETTER);
            PdfWriter writer;
            try { 
                writer = PdfWriter.GetInstance(document, new FileStream(@"D:\Archivos\Eduardo\Desktop\Registro "+comboBox1.SelectedItem.ToString()+" " + dateTimePicker2.Value.ToLongDateString() + ".pdf", FileMode.Create));
            }catch(Exception errorDoc){
                MessageBox.Show("No se puede crear documento, esta siendo utilizado por otro programa");
                return;
            }
            document.AddTitle("Reportes Cocarsa");
            document.AddCreator("Cocarsa Tecamac");
            document.Open();
            iTextSharp.text.Font _standardFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
            // Escribimos el encabezamiento en el documento
            document.Add(new Paragraph("Registro de "+comboBox1.SelectedItem.ToString()+" de " + dateTimePicker2.Value.ToLongDateString()));
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
    }
}
