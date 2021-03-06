﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Cocarsa1.ConexionBD;
using Cocarsa1.Entidades;

namespace Cocarsa1.ControlUsuario
{
    public partial class Producto : UserControl
    {
        public Producto()
        {
            InitializeComponent();
            cargaHistorial(DateTime.Now.Month, DateTime.Now.Year);
        }

        public void cargaHistorial(int mes, int annio) {

            dataGridView1.Rows.Clear();
            ProductoDao dao = new ProductoDao();
            List<HistorialPrecio> historial = dao.historialCambioPrecios(mes, annio);
            
            foreach (HistorialPrecio registro in historial)
            {
                dataGridView1.Rows.Add(registro.IdProducto,
                                        registro.Producto,
                                        registro.Fecha.ToString("dd-MMMM-yyyy"),
                                        Math.Round(registro.PrecioAnterior, 2).ToString("N2"),
                                        Math.Round(registro.PrecioActual, 2).ToString("N2"));
            }
        }

        private void carga_idProducto(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter)) 
            {
                ProductoDao dao = new ProductoDao();
                int idProducto = 0;

                if (textBox1.Text.Trim().Equals(""))
                {
                    // obtener nuevo id para guardar nuevo producto
                    if ((idProducto = dao.obtenerIdProducto()) != -1)
                    {
                        textBox1.Text = idProducto.ToString();
                        textBox4.Text = "NUEVO";
                        textBox1.Enabled = false;
                        textBox2.Enabled = true;
                        textBox3.Enabled = true;
                        textBox3.Focus();
                        textBox3.Select(0, textBox3.Text.Length);
                    }
                    else {
                        MessageBox.Show("Error de conexion con la base de datos.");
                        textBox1.Text = "";
                        textBox1.Enabled = true;
                        textBox2.Enabled = false;
                        textBox3.Enabled = false;
                    }
                }
                else {                    
                    
                    try {
                        idProducto = Convert.ToInt32(textBox1.Text);
                    } catch(Exception ex) {
                        ex.ToString();
                        textBox1.Text = "";
                        MessageBox.Show("Ingresa un valor númerico.");
                        return;
                    }

                    // busca si el id existe, si existe puede modificar el producto cargado
                    ProductoE producto = null;
                    if ((producto = dao.obtenerProducto(idProducto)) != null)
                    {
                        textBox1.Enabled = false;
                        textBox2.Enabled = true;
                        textBox3.Enabled = true;
                        textBox2.Text = producto.PrecioVenta.ToString("N2");
                        textBox3.Text = producto.Nombre.ToString();
                        textBox4.Text = "EDITABLE";
                        textBox3.Focus();
                        textBox3.Select(0, textBox3.Text.Length);
                    }
                    else {
                        textBox1.Enabled = false;
                        textBox2.Enabled = true;
                        textBox3.Enabled = true;
                        textBox4.Text = "NUEVO";
                        textBox3.Focus();
                        textBox3.Select(0, textBox3.Text.Length);
                    }
                }
            }
        }

        private void cancelar(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Escape))
            {
                limpiarCampos();
            }
        }

        private void limpiarCampos()
        {
            textBox1.Text = "";
            textBox2.Text = "0.0";
            textBox3.Text = "";
            textBox4.Text = "NUEVO";
            textBox1.Enabled = true;
            textBox1.Focus();
            textBox1.Select(0, 0);
            textBox2.Enabled = false;
            textBox3.Enabled = false;
        }

        private void guardar_cambios(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F10)
            {
                if (textBox3.Text.Trim().Equals("")) {
                    MessageBox.Show("Falta nombre de producto");
                    return;
                }

                double precioVenta = 0;

                try
                {
                    precioVenta = Convert.ToDouble(textBox2.Text);
                    
                    if (precioVenta <= 0)
                    {
                        MessageBox.Show("El precio de venta debe ser mayor a cero.");
                        textBox2.Text = "";
                        return;
                    }
                } catch(Exception ex) {
                    MessageBox.Show("El precio debe ser un valor numérico");
                    textBox2.Text = "";
                    return;
                }
                
                ProductoE prod = new ProductoE();
                prod.IdProducto = Convert.ToInt32(textBox1.Text);
                prod.Nombre = textBox3.Text;
                prod.PrecioVenta = precioVenta;

                ProductoDao dao = new ProductoDao();

                if (textBox4.Text.Equals("NUEVO"))
                {
                    if (dao.registrarProducto(prod) != -1)
                    {
                        MessageBox.Show(prod.Nombre + " guardado correctamente.");
                        limpiarCampos();
                    }
                    else {
                        MessageBox.Show("Error al registrar el producto.");
                    }
                }
                else
                {
                    if (dao.guardarCambios(prod) != -1)
                    {
                        MessageBox.Show(prod.Nombre + " guardado correctamente.");
                        limpiarCampos();
                        cargaHistorial(DateTime.Now.Month, DateTime.Now.Year);
                    }
                    else {
                        MessageBox.Show("Error al guardar los cambios.");
                    }
                }
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            cargaHistorial(dateTimePicker1.Value.Month, dateTimePicker1.Value.Year);
        }

        
    }
}
