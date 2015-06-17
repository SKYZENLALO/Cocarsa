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
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
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
                        MessageBox.Show("Ingresa un valor numérico.");
                        dataGridView1.Rows.RemoveAt(dataGridView1.CurrentCell.RowIndex);
                        return;
                    }
                    
                    ProductoDao dao = new ProductoDao();
                    ProductoE prod = dao.obtenerProducto(id);

                    if (prod != null)
                    {
                        dataGridView1.CurrentRow.Cells[1].Value = prod.Nombre;
                        
                    }
                    else {
                        MessageBox.Show("El producto no existe.");
                        dataGridView1.Rows.RemoveAt(dataGridView1.CurrentCell.RowIndex);
                        return;
                    }
                    break;
                case 1:
                    break;
                case 2:
                    break;
            }
        }

    }
}
