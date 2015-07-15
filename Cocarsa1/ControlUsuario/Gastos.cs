using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cocarsa1.ControlUsuario
{
    public partial class Gastos : UserControl
    {
        public Gastos()
        {
            InitializeComponent();
            dateTimePicker1.Value = DateTime.Today;
        }

    }
}
