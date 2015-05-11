namespace Cocarsa1
{
    partial class Form1
    {
        /// <summary>
        /// Variable del diseñador requerida.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén utilizando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben eliminar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido del método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabPage7 = new System.Windows.Forms.TabPage();
            this.tabPage9 = new System.Windows.Forms.TabPage();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.TabControl = new System.Windows.Forms.TabControl();
            this.ventas1 = new Cocarsa1.ControlUsuario.Ventas();
            this.pagoAbonos1 = new Cocarsa1.ControlUsuario.PagoAbonos();
            this.fajillas1 = new Cocarsa1.ControlUsuario.Fajillas();
            this.producto1 = new Cocarsa1.ControlUsuario.Producto();
            this.cliente1 = new Cocarsa1.ControlUsuario.Cliente();
            this.inventario1 = new Cocarsa1.ControlUsuario.Inventario();
            this.gastos1 = new Cocarsa1.ControlUsuario.Gastos();
            this.reportes1 = new Cocarsa1.ControlUsuario.Reportes();
            this.tabPage7.SuspendLayout();
            this.tabPage9.SuspendLayout();
            this.tabPage6.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.TabControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabPage7
            // 
            this.tabPage7.Controls.Add(this.reportes1);
            this.tabPage7.Font = new System.Drawing.Font("Open Sans", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPage7.Location = new System.Drawing.Point(4, 31);
            this.tabPage7.Name = "tabPage7";
            this.tabPage7.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage7.Size = new System.Drawing.Size(1340, 694);
            this.tabPage7.TabIndex = 9;
            this.tabPage7.Text = " Reportes ";
            this.tabPage7.UseVisualStyleBackColor = true;
            // 
            // tabPage9
            // 
            this.tabPage9.Controls.Add(this.gastos1);
            this.tabPage9.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPage9.Location = new System.Drawing.Point(4, 31);
            this.tabPage9.Name = "tabPage9";
            this.tabPage9.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage9.Size = new System.Drawing.Size(1340, 694);
            this.tabPage9.TabIndex = 8;
            this.tabPage9.Text = " Gastos ";
            this.tabPage9.UseVisualStyleBackColor = true;
            // 
            // tabPage6
            // 
            this.tabPage6.Controls.Add(this.inventario1);
            this.tabPage6.Location = new System.Drawing.Point(4, 31);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage6.Size = new System.Drawing.Size(1340, 694);
            this.tabPage6.TabIndex = 5;
            this.tabPage6.Text = " Inventario ";
            this.tabPage6.UseVisualStyleBackColor = true;
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.cliente1);
            this.tabPage5.Location = new System.Drawing.Point(4, 31);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(1340, 694);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = " Cliente ";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.producto1);
            this.tabPage4.Location = new System.Drawing.Point(4, 31);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(1340, 694);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = " Producto ";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.fajillas1);
            this.tabPage3.Location = new System.Drawing.Point(4, 31);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(1340, 694);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = " Fajillas ";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.pagoAbonos1);
            this.tabPage2.Location = new System.Drawing.Point(4, 31);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1340, 694);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = " Pago Abonos ";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.ventas1);
            this.tabPage1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPage1.Location = new System.Drawing.Point(4, 31);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1340, 694);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = " Ventas ";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // TabControl
            // 
            this.TabControl.Controls.Add(this.tabPage1);
            this.TabControl.Controls.Add(this.tabPage2);
            this.TabControl.Controls.Add(this.tabPage3);
            this.TabControl.Controls.Add(this.tabPage4);
            this.TabControl.Controls.Add(this.tabPage5);
            this.TabControl.Controls.Add(this.tabPage6);
            this.TabControl.Controls.Add(this.tabPage9);
            this.TabControl.Controls.Add(this.tabPage7);
            this.TabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TabControl.Font = new System.Drawing.Font("Open Sans", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TabControl.Location = new System.Drawing.Point(0, 0);
            this.TabControl.Name = "TabControl";
            this.TabControl.SelectedIndex = 0;
            this.TabControl.Size = new System.Drawing.Size(1348, 729);
            this.TabControl.TabIndex = 0;
            // 
            // ventas1
            // 
            this.ventas1.BackColor = System.Drawing.Color.White;
            this.ventas1.Font = new System.Drawing.Font("Open Sans", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ventas1.Location = new System.Drawing.Point(0, 0);
            this.ventas1.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.ventas1.Name = "ventas1";
            this.ventas1.Size = new System.Drawing.Size(1340, 662);
            this.ventas1.TabIndex = 0;
            this.ventas1.Load += new System.EventHandler(this.ventas1_Load);
            // 
            // pagoAbonos1
            // 
            this.pagoAbonos1.BackColor = System.Drawing.Color.White;
            this.pagoAbonos1.Font = new System.Drawing.Font("Open Sans", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pagoAbonos1.Location = new System.Drawing.Point(0, 0);
            this.pagoAbonos1.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.pagoAbonos1.Name = "pagoAbonos1";
            this.pagoAbonos1.Size = new System.Drawing.Size(1340, 662);
            this.pagoAbonos1.TabIndex = 0;
            // 
            // fajillas1
            // 
            this.fajillas1.BackColor = System.Drawing.Color.White;
            this.fajillas1.Font = new System.Drawing.Font("Open Sans", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fajillas1.Location = new System.Drawing.Point(0, 0);
            this.fajillas1.Margin = new System.Windows.Forms.Padding(5);
            this.fajillas1.Name = "fajillas1";
            this.fajillas1.Size = new System.Drawing.Size(1629, 662);
            this.fajillas1.TabIndex = 0;
            // 
            // producto1
            // 
            this.producto1.Font = new System.Drawing.Font("Open Sans", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.producto1.Location = new System.Drawing.Point(0, 0);
            this.producto1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.producto1.Name = "producto1";
            this.producto1.Size = new System.Drawing.Size(1340, 662);
            this.producto1.TabIndex = 0;
            // 
            // cliente1
            // 
            this.cliente1.Font = new System.Drawing.Font("Open Sans", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cliente1.Location = new System.Drawing.Point(0, 0);
            this.cliente1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cliente1.Name = "cliente1";
            this.cliente1.Size = new System.Drawing.Size(1340, 662);
            this.cliente1.TabIndex = 0;
            // 
            // inventario1
            // 
            this.inventario1.Font = new System.Drawing.Font("Open Sans", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.inventario1.Location = new System.Drawing.Point(0, 0);
            this.inventario1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.inventario1.Name = "inventario1";
            this.inventario1.Size = new System.Drawing.Size(1340, 662);
            this.inventario1.TabIndex = 0;
            // 
            // gastos1
            // 
            this.gastos1.BackColor = System.Drawing.Color.White;
            this.gastos1.Font = new System.Drawing.Font("Open Sans", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gastos1.Location = new System.Drawing.Point(0, 0);
            this.gastos1.Margin = new System.Windows.Forms.Padding(5);
            this.gastos1.Name = "gastos1";
            this.gastos1.Size = new System.Drawing.Size(2010, 1120);
            this.gastos1.TabIndex = 0;
            // 
            // reportes1
            // 
            this.reportes1.BackColor = System.Drawing.Color.White;
            this.reportes1.Cursor = System.Windows.Forms.Cursors.Default;
            this.reportes1.Font = new System.Drawing.Font("Open Sans", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.reportes1.Location = new System.Drawing.Point(0, 0);
            this.reportes1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.reportes1.Name = "reportes1";
            this.reportes1.Size = new System.Drawing.Size(1340, 662);
            this.reportes1.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(1348, 729);
            this.Controls.Add(this.TabControl);
            this.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaximumSize = new System.Drawing.Size(1591, 880);
            this.MinimumSize = new System.Drawing.Size(1364, 726);
            this.Name = "Form1";
            this.Text = "Cocarsa";
            this.tabPage7.ResumeLayout(false);
            this.tabPage9.ResumeLayout(false);
            this.tabPage6.ResumeLayout(false);
            this.tabPage5.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.TabControl.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabPage tabPage7;
        private System.Windows.Forms.TabPage tabPage9;
        private System.Windows.Forms.TabPage tabPage6;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabControl TabControl;
        private ControlUsuario.Ventas ventas1;
        private ControlUsuario.PagoAbonos pagoAbonos1;
        private ControlUsuario.Fajillas fajillas1;
        private ControlUsuario.Gastos gastos1;
        private ControlUsuario.Reportes reportes1;
        private ControlUsuario.Inventario inventario1;
        private ControlUsuario.Cliente cliente1;
        private ControlUsuario.Producto producto1;

    }
}

