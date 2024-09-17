/*
 * Miguel Ángel Martínez Jiménez
 * Versión: 2.0.
 * Fecha: Septiembre  2024.
 * Aplicación: Evaluación y Entrenamiento de la Atención.
 * 
 */

using System;
using System.Runtime.Versioning;
using System.Windows.Forms;

namespace demorejilla
{
    [SupportedOSPlatform("windows")]
    public partial class Informacion : Form
    {
        public Informacion()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
