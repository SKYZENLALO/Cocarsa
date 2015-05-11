﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Cocarsa1.ConexionBD;
using Cocarsa1.Entidades;

namespace Cocarsa1
{
    public partial class Popup : Form
    {

        private Cliente clienteSeleccionado = null;
        private List<Cliente> listaClientes = null;

        public Cliente ClienteSeleccionado {
            get { return clienteSeleccionado; }
            set { clienteSeleccionado = value; }
        }

        public Popup()
        {
            InitializeComponent();            
        }

        private void busca_cliente(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter)) {

                dataGridView1.Rows.Clear();

                if (textBox6.Text.Trim() == "")
                {
                    MessageBox.Show("Ingresa un nombre o un digito.");
                }
                else
                {
                    ClienteDao dao = new ClienteDao();
                    listaClientes = dao.busquedaClientes(textBox6.Text);

                    foreach (Cliente cliente in listaClientes)
                    {
                        DataGridViewRow row = (DataGridViewRow)dataGridView1.Rows[0].Clone();
                        row.Cells[0].Value = cliente.Nombre + " " + cliente.APaterno + " " + cliente.AMaterno;
                        row.Cells[1].Value = cliente.Calle + " " + cliente.Colonia;
                        row.Height = 50;
                        dataGridView1.Rows.Add(row);
                    }
                    dataGridView1.Focus();
                }
            }
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;

                int fila = dataGridView1.CurrentCell.RowIndex;
                if (fila < listaClientes.Count)
                {
                    clienteSeleccionado = listaClientes[fila];
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
        }

              
    }
}
