using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Cocarsa1.Entidades;
using Cocarsa1.ConexionBD;

namespace Cocarsa1.ControlUsuario
{
    public partial class Fajillas : UserControl
    {
        int idCajera = 1;

        public Fajillas()
        {
            InitializeComponent();
            dateTimePicker1.Value = DateTime.Today;
            dateTimePicker2.Value = DateTime.Today;
            dateTimePicker1.MaxDate = DateTime.Today;
            dateTimePicker2.MaxDate = DateTime.Today;
            cargaCajeras();
            cargaFajillas();
        }
        void cargaCajeras() {
            FajillasDAO fajillasDAO = new FajillasDAO();
            Cajera[] cajera = null;
            int i = 0;
            int numCajeras = fajillasDAO.numeroCajeras();
            cajera = fajillasDAO.cajeras(numCajeras);
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox1.Items.Clear();
            while (i < numCajeras) {
                comboBox1.Items.Add(cajera[i].Nombre+" "+cajera[i].APaterno+" "+cajera[i].AMaterno);
                i++;
            }
            comboBox1.SelectedIndex = 0;
        }
        void cargaFajillas() {
            dataGridView2.Rows.Clear();
            dataGridView3.Rows.Clear();
            FajillasDAO fajillasDAO=new FajillasDAO();
            Fajilla[] fajilla = null;
            int i = 0;
            Double montoTotal = 0;
            DateTime hoy = DateTime.Today;
            String fecha = hoy.Year.ToString()+"-"+hoy.Month.ToString()+"-"+hoy.Day.ToString();
            int numeroFajillas = fajillasDAO.numeroFajillas(fecha,1);
            fajilla = fajillasDAO.fajillas(numeroFajillas,fecha,1);
            while (i < numeroFajillas) {
                DataGridViewRow row = (DataGridViewRow)dataGridView2.Rows[0].Clone();
                row.Cells[0].Value = fajillasDAO.nombreCajera(fajilla[i].IdCajera);
                row.Cells[1].Value = fajilla[i].Monto;
                montoTotal += fajilla[i].Monto;
                dataGridView2.Rows.Add(row);
                i++;
            }
            textBox2.Text = montoTotal.ToString();
            numeroFajillas = 0;
            fajilla = null;
            i = 0;
            montoTotal = 0;
            numeroFajillas = fajillasDAO.numeroFajillas(fecha,2);
            fajilla = fajillasDAO.fajillas(numeroFajillas, fecha, 2);
            while (i < numeroFajillas)
            {
                DataGridViewRow row = (DataGridViewRow)dataGridView3.Rows[0].Clone();
                row.Cells[0].Value = fajillasDAO.nombreCajera(fajilla[i].IdCajera);
                row.Cells[1].Value = fajilla[i].Monto;
                row.Cells[2].Value = fajilla[i].FechaRegistro.ToShortDateString();
                montoTotal += fajilla[i].Monto;
                dataGridView3.Rows.Add(row);
                i++;
            }
            textBox3.Text = montoTotal.ToString();

            //dataGridView2.Rows.Remove(dataGridView2.Rows[dataGridView2.Rows.Count-1]);
        }
        void cargaFajillasFecha(DateTime fechaBuscar, int opc) {
            if (opc == 1)
            {
                dataGridView1.Rows.Clear();
            }else {
                dataGridView4.Rows.Clear();
            }
            FajillasDAO fajillasDAO = new FajillasDAO();
            Fajilla[] fajilla = null;
            int i = 0;
            Double montoTotal = 0;
            String fecha = fechaBuscar.Year.ToString() + "-" + fechaBuscar.Month.ToString() + "-" + fechaBuscar.Day.ToString();
            int numeroFajillas = fajillasDAO.numeroFajillas(fecha, opc);
            if (numeroFajillas > 0)
            {
                fajilla = fajillasDAO.fajillas(numeroFajillas, fecha, opc);
                while (i < numeroFajillas)
                {
                    DataGridViewRow row = null;
                    if (opc == 1)
                    {
                        row = (DataGridViewRow)dataGridView1.Rows[0].Clone();
                        row.Cells[0].Value = fajilla[i].IdCajera;
                        row.Cells[1].Value = fajilla[i].Monto;
                        if (fajilla[i].FechaCorte == Convert.ToDateTime("21-sep-1992"))
                        {
                            row.Cells[2].Value = "";
                        }
                        else
                        {
                            row.Cells[2].Value = fajilla[i].FechaCorte.ToShortDateString();
                        }
                    }
                    else
                    {
                        row = (DataGridViewRow)dataGridView4.Rows[0].Clone();
                        row.Cells[0].Value = fajilla[i].IdCajera;
                        row.Cells[1].Value = fajilla[i].Monto;
                        row.Cells[2].Value = fajilla[i].FechaRegistro.ToShortDateString();
                    } 
                    
                    montoTotal += fajilla[i].Monto;
                    if (opc == 1)
                    {
                        dataGridView1.Rows.Add(row);
                    }else {
                        dataGridView4.Rows.Add(row);
                    }
                    i++;
                }
            }else {
                MessageBox.Show("No existen registros");
            }
            if (opc == 1)
            {
                textBox4.Text = montoTotal.ToString();
            }else {
                textBox5.Text = montoTotal.ToString();
            }
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            idCajera = comboBox1.SelectedIndex + 1;
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F10){
                try
                {
                    Double monto = Convert.ToDouble(textBox1.Text);
                    if (monto <= 0)
                    {
                        MessageBox.Show("Solo cifras validas");
                        textBox1.Clear();
                        return;
                    }
                    else {
                        //Agregar Box de confirmacion 
                        var result = MessageBox.Show("Continuar", "Desea Ingresar la fajilla", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                        if (result == DialogResult.OK)
                        {
                            FajillasDAO FajillasDao = new FajillasDAO();
                            Fajilla nuevaFajilla = new Fajilla();
                            nuevaFajilla.EnCaja = true;
                            nuevaFajilla.IdCajera = idCajera;
                            nuevaFajilla.Monto = monto;
                            Boolean resp = FajillasDao.insertarFajillas(nuevaFajilla);
                            if (resp)
                            {
                                cargaFajillas();
                                MessageBox.Show("se Ingreso: " + monto + " Con la Cajera: " + FajillasDao.nombreCajera(idCajera));
                            }
                        }
                        else
                        {
                            textBox1.Clear();
                        }
                    }
                }
                catch (Exception error) {
                    MessageBox.Show("Solo Cifras validas");
                    textBox1.Clear();
                    return;
                }
            }
            if(e.KeyCode == Keys.F11){
                var result = MessageBox.Show("Continuar", "Desea Realizar el Corte de Caja", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (result == DialogResult.OK)
                {
                    FajillasDAO FajillasDao = new FajillasDAO();
                    Boolean respuesta = FajillasDao.corteCaja();
                    if (respuesta) {
                        cargaFajillas();
                        MessageBox.Show("Se Realizo el Corte de Caja Exitozamente");
                    }
                }
            }
        }

        private void dateTimePicker1_CloseUp(object sender, EventArgs e)
        {
            //Entra evento
            cargaFajillasFecha(dateTimePicker1.Value,1);
        }

        private void dateTimePicker1_KeyDown(object sender, KeyEventArgs e)
        {
            //El otro evento
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                cargaFajillasFecha(dateTimePicker1.Value,1);
            }
        }

        private void dateTimePicker2_CloseUp(object sender, EventArgs e)
        {
            cargaFajillasFecha(dateTimePicker1.Value, 1);
        }

        private void dateTimePicker2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                cargaFajillasFecha(dateTimePicker1.Value, 3);
            }
        }
    }
}
