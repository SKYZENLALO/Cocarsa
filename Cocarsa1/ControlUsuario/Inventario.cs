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
    public partial class Inventario : UserControl
    {
        private int id = 0;
        private int columna = 0;
        private int fila = 0;
        private Boolean flag = false;

        public Inventario()
        {
            InitializeComponent();

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

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            fila = dataGridView1.CurrentCell.RowIndex;
            switch (dataGridView1.SelectedCells[0].ColumnIndex)
            {
                case 0:
                    try
                    {
                        id = Convert.ToInt32(dataGridView1.CurrentCell.Value);
                    }
                    catch (Exception error)
                    {
                        id = 0;
                        MessageBox.Show("Ingresa un valor numérico.","Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        dataGridView1.Rows.RemoveAt(dataGridView1.CurrentCell.RowIndex);
                        return;
                    }
                    
                    ProductoDao dao = new ProductoDao();
                    ProductoE prod = dao.obtenerProducto(id);

                    if (prod != null)
                    {
                        dataGridView1.CurrentRow.Cells[1].Value = prod.Nombre;
                        columna = 2;
                        flag = true;
                    }
                    else {
                        MessageBox.Show("El producto no existe.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        dataGridView1.Rows.RemoveAt(dataGridView1.CurrentCell.RowIndex);
                        columna = 0;
                        flag = true;
                        return;
                    }
                    break;
                case 1:
                    break;
                case 2:
                    double kilogramos = 0;
                    
                    if (dataGridView1.CurrentCell.Value == null)
                    {
                        MessageBox.Show("Ingresa la cantidad de kilos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        columna = 2;
                        flag = true; 
                        return;
                    }
                    else {
                        if (dataGridView1.CurrentRow.Cells[1].Value == null) {
                            MessageBox.Show("Ingresa la clave del producto.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            dataGridView1.CurrentCell.Value = "";
                            columna = 0;
                            dataGridView1.Rows.RemoveAt(dataGridView1.CurrentCell.RowIndex);
                            flag = true;
                            return;   
                        }
                        
                        try { 
                            kilogramos = Convert.ToDouble(dataGridView1.CurrentCell.Value);
                            if (kilogramos <= 0) {
                                MessageBox.Show("Ingresa una cantidad mayor a cero.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                dataGridView1.CurrentCell.Value = "";
                                columna = 2;
                                flag = true;
                                return;
                            }

                        } catch(Exception ex) {
                            MessageBox.Show("Ingresa un valor numérico.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            dataGridView1.CurrentCell.Value = "";
                            columna = 2;
                            flag = true; 
                            return;
                        }

                        fila = fila + 1;
                        columna = 0;
                        flag = true;                        
                    }
                    break;
            }
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
                if (dataGridView1.CurrentRow.Cells[0].Value != null && dataGridView1.CurrentRow.Cells[2].Value == null)
                {
                    MessageBox.Show("Ingresa la cantidad de kilos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    columna = 2;
                    flag = true;
                    return;
                }                
            }
        }

    }
}
