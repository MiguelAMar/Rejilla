/*
 * Miguel Ángel Martínez Jiménez
 * Versión: 2.0.
 * Fecha: Septiembre  2024.
 * Aplicación: Evaluación y Entrenamiento de la Atención.
 * 
 */

using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using System.Resources;
using System.Runtime.Versioning;

namespace demorejilla
{
    [SupportedOSPlatform("windows")]
    public partial class Porcentajes : Form
    {
        private float aciertos;
        private float fallos;
        private float casillasCorrectas;
        private float pAciertos;
        private float pErrores;
        private float pAciertosNumCasillas;
        private float pAciertosErrores;
        private float pErroresAciertos;
        private float pErroresNumCasillas;
        private float pEficacia;
        private float pEfectividad;
        private float pCasillas;

        private Pen lapiz;
        private Pen lapizCasillas;

        private ResourceManager rm; 

        public Porcentajes(int ok, int error, int casillas)
        {
            InitializeComponent();
            rm = new ResourceManager("demorejilla.Recursos", typeof(MetiendoDatos).Assembly);
            this.lapiz = new Pen(Brushes.Orchid, pictureBoxEficacia.Height * 3);
            this.lapizCasillas = new Pen(Brushes.Orange, pictureBoxAciertos.Height * 3);
            if (ok > 0)
                this.aciertos = ok;
            if (error > 0)
                this.fallos = error;
            if (casillas > 0)
                this.casillasCorrectas = casillas;
            if (casillas == 0)
                throw new RejillaException(rm.GetString("por00"));//("No se pueden obtener los porcentajes (Num. Casillas = 0).");
            //Unable to get the percentages (Num. Casillas = 0)
            
            if (ok == 0 && error == 0)
                throw new RejillaException(rm.GetString("por01"));//("No se pueden obtener los porcentajes (Num. aciertos = num. errores = 0 ).");
            //Unable to get the percentages (Num. hits = num. Errors = 0)
            this.calcularPorcentajes();
            this.mostrarPorcentajes();
        }

        private void calcularPorcentajes()
        {
            this.pAciertos = this.aciertos;
            this.pErrores = this.fallos;
            this.pCasillas = this.casillasCorrectas;

            this.pAciertosNumCasillas = (this.aciertos / this.casillasCorrectas);// *100;
            this.pAciertosErrores = (this.aciertos / (this.aciertos + this.fallos));// *100;
            this.pErroresAciertos = (this.fallos / (this.aciertos + this.fallos));//;*100;
            this.pErroresNumCasillas = (this.fallos / this.casillasCorrectas);// *100;
            this.pEficacia = (this.aciertos - this.fallos) / this.casillasCorrectas;// *100;
            this.pEfectividad = (this.aciertos - this.fallos) / (this.aciertos + this.fallos);// *100;
        }

        private void mostrarPorcentajes()
        {
            this.etValorAciertos.Text = this.aciertos.ToString();
            this.etValorCantidadBotones.Text = this.casillasCorrectas.ToString();
            this.etValorFallos.Text = this.fallos.ToString();

            this.etValorAciertosCasillas.Text = this.pAciertosNumCasillas.ToString("##0.##%", CultureInfo.InvariantCulture);

            this.etValorAciertosErrores.Text = this.pAciertosErrores.ToString("##0.##%", CultureInfo.InvariantCulture);//.ToString() + "%";
            this.etValorErroresAciertos.Text = this.pErroresAciertos.ToString("##0.##%", CultureInfo.InvariantCulture);//ToString() + "%";
            this.etValorErroresCasillas.Text = this.pErroresNumCasillas.ToString("##0.##%", CultureInfo.InvariantCulture);//ToString() + "%";
            this.etVAlorEficacia.Text = this.pEficacia.ToString("##0.##%", CultureInfo.InvariantCulture);//ToString() + "%";
            this.etValorEfectividad.Text = this.pEfectividad.ToString("##0.##%", CultureInfo.InvariantCulture); //ToString() + "%";
        }

        /*
         * Paint para dibujar una barra con el incremento de los porcentajes
         * 
         */ 
        private void pictureBoxAciertos_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(pictureBoxAciertos.BackColor);
            g.DrawLine(lapizCasillas, new Point(0,pictureBoxAciertos.Height), new Point(Convert.ToInt32(this.pAciertos),pictureBoxAciertos.Height));
        }

        private void pictureBoxFallos_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(pictureBoxFallos.BackColor);
            g.DrawLine(lapizCasillas, new Point(0, pictureBoxFallos.Height), new Point(Convert.ToInt32(this.pErrores), pictureBoxFallos.Height));
        }

        private void pictureBoxCantidadbotones_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(pictureBoxCantidadbotones.BackColor);
            g.DrawLine(lapizCasillas, new Point(0, pictureBoxCantidadbotones.Height), new Point(Convert.ToInt32(this.pCasillas), pictureBoxCantidadbotones.Height));
        }

        private void pictureBoxAciertosCasillas_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(pictureBoxAciertosCasillas.BackColor);
            g.DrawLine(lapiz, new Point(0, pictureBoxAciertosCasillas.Height), new Point(Convert.ToInt32(this.pAciertosNumCasillas*100), pictureBoxAciertosCasillas.Height));
        }

        private void pictureBoxAciertosErrores_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(pictureBoxAciertosErrores.BackColor);
            g.DrawLine(lapiz, new Point(0, pictureBoxAciertosErrores.Height), new Point(Convert.ToInt32(this.pAciertosErrores*100), pictureBoxAciertosErrores.Height));
        }

        private void pictureBoxErroresAciertos_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(pictureBoxErroresAciertos.BackColor);
            g.DrawLine(lapiz, new Point(0, pictureBoxErroresAciertos.Height), new Point(Convert.ToInt32(this.pErroresAciertos*100), pictureBoxErroresAciertos.Height));
        }

        private void pictureBoxErroresCasillas_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(pictureBoxErroresCasillas.BackColor);
            g.DrawLine(lapiz, new Point(0, pictureBoxErroresCasillas.Height), new Point(Convert.ToInt32(this.pErroresNumCasillas*100), pictureBoxErroresCasillas.Height));
        }

        private void pictureBoxEficacia_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(pictureBoxEficacia.BackColor);
            g.DrawLine(lapiz, new Point(0, pictureBoxEficacia.Height), new Point(Convert.ToInt32(this.pEficacia*100), pictureBoxEficacia.Height));
        }

        private void pictureBoxEfectividad_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(pictureBoxEfectividad.BackColor);
            g.DrawLine(lapiz, new Point(0, pictureBoxEfectividad.Height), new Point(Convert.ToInt32(this.pEfectividad*100), pictureBoxEfectividad.Height));
        }
    }
}
