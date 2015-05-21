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

namespace Cocarsa1.ControlUsuario
{
    public partial class Producto : UserControl
    {
        public Producto()
        {
            InitializeComponent();
        }

        private void carga_idProducto(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter)) 
            {
                if (textBox1.Text.Trim().Equals(""))
                {
                    // obtener nuevo id para guardar nuevo producto
                }
                else {
                    int idProducto = 0;
                    
                    try {
                        idProducto = Convert.ToInt32(textBox1.Text);
                    } catch(Exception ex) {
                        ex.ToString();
                        textBox1.Text = "";
                        MessageBox.Show("Ingresa un valor númerico.");
                        return;
                    }

                    // busca si el id existe, si existe puede modificar el producto cargado
                    ProductoDao dao = new ProductoDao();
                    dao.obtenerProducto(idProducto);

                    // si el id no existe guarda un nuevo producto
                    

                }
            }
        }
    }
}
