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
                imprimirPDF();
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
            dataGridView2.Rows.Clear();
            if (idIni != 0) {
                i = 0;
                idIni--;
                idFin++;
                int numProductos = reportesDAO.numeroProductos(idIni,idFin);
                OrdenNota[] ordenNota = null;
                ordenNota = reportesDAO.consultaOrden(idIni,idFin,numProductos);
                while (i  < numProductos)
                {
                    DataGridViewRow row = (DataGridViewRow)dataGridView2.Rows[0].Clone();
                    row.Cells[0].Value = ordenNota[i].IdProducto;
                    Console.Write(ordenNota[i].IdProducto + "\n");
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
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(@"D:\Archivos\Eduardo\Desktop\prueba.pdf", FileMode.Create));
            document.AddTitle("Mi primer PDF");
            document.AddCreator("Eduardo Ruiz");
            document.Open();
            iTextSharp.text.Font _standardFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
            // Escribimos el encabezamiento en el documento
            document.Add(new Paragraph("Mi primer documento PDF"));
            document.Add(Chunk.NEWLINE);

            // Creamos una tabla que contendrá el nombre, apellido y país
            // de nuestros visitante.
            PdfPTable tblPrueba = new PdfPTable(3);
            tblPrueba.WidthPercentage = 100;

            // Configuramos el título de las columnas de la tabla
            PdfPCell clNombre = new PdfPCell(new Phrase("Nombre", _standardFont));
            clNombre.BorderWidth = 0;
            clNombre.BorderWidthBottom = 0.75f;

            PdfPCell clApellido = new PdfPCell(new Phrase("Apellido", _standardFont));
            clApellido.BorderWidth = 0;
            clApellido.BorderWidthBottom = 0.75f;

            PdfPCell clPais = new PdfPCell(new Phrase("País", _standardFont));
            clPais.BorderWidth = 0;
            clPais.BorderWidthBottom = 0.75f;

            // Añadimos las celdas a la tabla
            tblPrueba.AddCell(clNombre);
            tblPrueba.AddCell(clApellido);
            tblPrueba.AddCell(clPais);

            // Llenamos la tabla con información
            clNombre = new PdfPCell(new Phrase("Eduardo", _standardFont));
            clNombre.BorderWidth = 0;

            clApellido = new PdfPCell(new Phrase("Ruiz", _standardFont));
            clApellido.BorderWidth = 0;

            clPais = new PdfPCell(new Phrase("México", _standardFont));
            clPais.BorderWidth = 0;

            // Añadimos las celdas a la tabla
            tblPrueba.AddCell(clNombre);
            tblPrueba.AddCell(clApellido);
            tblPrueba.AddCell(clPais);
            // Finalmente, añadimos la tabla al documento PDF y cerramos el documento
            document.Add(tblPrueba);

            document.Close();
            writer.Close();
        }
    }
}
