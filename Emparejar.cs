/*
 * Miguel Ángel Martínez Jiménez
 * Versión: 2.0.
 * Fecha: Septiembre  2024.
 * Aplicación: Evaluación y Entrenamiento de la Atención.
 * 
 */

using System;
using System.Drawing;
using System.Resources;
using System.Runtime.Versioning;
using System.Windows.Forms;

namespace demorejilla
{
    [SupportedOSPlatform("windows")]
    public partial class Emparejar : Form
    {
        //variables
        private EmparejarSecuencia secuencia;
        private int tiempo;
        private PosicionBotonEmparejar posicion;
        private ResourceManager rm;

        public Emparejar(int clock, PosicionBotonEmparejar pbe, EmparejarSecuencia es)
        {
            rm = new ResourceManager("demorejilla.Recursos", typeof(MetiendoDatos).Assembly);
            InitializeComponent();
            secuencia = es;//EmparejarSecuencia.SECUENCIAL;
            this.tiempo = clock;
            this.posicion = pbe;//PosicionBotonEmparejar.IZQUIERDA_CENTRO;
            this.tbTiempo.Text = clock.ToString();
            inicializarOrdenBoton();
        }

        private void inicializarOrdenBoton()
        {
            switch (this.secuencia)
            {
                case EmparejarSecuencia.SECUENCIAL:
                    this.rbSecuencial.Checked = true;
                    break;
                case EmparejarSecuencia.INVERSA:
                    this.rbInversa.Checked = true;
                    break;
                case EmparejarSecuencia.ALEATORIA:
                    this.rbAleatoria.Checked = true;
                    break;
            }
            switch (this.posicion)
            {
                case PosicionBotonEmparejar.ALEATORIA:
                    cambiarFondo(this.btA);
                    this.etInformacion.Text = rm.GetString("par05");//"aleatoria";
                    this.etInformacion.ForeColor = Color.Green;
                    break;
                case PosicionBotonEmparejar.DERECHA_CENTRO:
                    cambiarFondo(this.btDerCentro);
                    this.etInformacion.Text = rm.GetString("par01") + "-" + rm.GetString("par03"); //"derecha-centro";
                    this.etInformacion.ForeColor = Color.OrangeRed;
                    break;
                case PosicionBotonEmparejar.DERECHA_INFERIOR:
                    cambiarFondo(this.btDerInf);
                    this.etInformacion.Text = rm.GetString("par01") + "-" + rm.GetString("par04"); //"derecha-inferior";
                    this.etInformacion.ForeColor = Color.OrangeRed;
                    break;
                case PosicionBotonEmparejar.DERECHA_SUPERIOR:
                    cambiarFondo(this.btDerSup);
                    this.etInformacion.Text = rm.GetString("par01") + "-" + rm.GetString("par02");//"derecha-superior";
                    this.etInformacion.ForeColor = Color.OrangeRed;
                    break;
                case PosicionBotonEmparejar.IZQUIERDA_CENTRO:
                    cambiarFondo(this.btIzqCentro);
                    this.etInformacion.Text = rm.GetString("par00") + "-" + rm.GetString("par03"); //"izquierda-centro";
                    this.etInformacion.ForeColor = Color.Blue;
                    break;
                case PosicionBotonEmparejar.IZQUIERDA_INFERIOR:
                    cambiarFondo(this.btIzqInf);
                    this.etInformacion.Text = rm.GetString("par00") + "-" + rm.GetString("par04");//"izquierda-inferior";
                    this.etInformacion.ForeColor = Color.Blue;
                    break;
                case PosicionBotonEmparejar.IZQUIERDA_SUPERIOR:
                    cambiarFondo(this.btIzqSup);
                    this.etInformacion.Text = rm.GetString("par00") + "-" + rm.GetString("par02");//"izquierda-superior";
                    this.etInformacion.ForeColor = Color.Blue;
                    break;
            }
        }

        public PosicionBotonEmparejar posicionBoton()
        {
            return this.posicion;
        }

        public int temporizador()
        {
            return tiempo;
        }

        public EmparejarSecuencia secuenciaItems()
        {
            return secuencia;
        }

        private void rbSecuencial_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rbSecuencial.Checked)
                this.secuencia = EmparejarSecuencia.SECUENCIAL;
            else if (this.rbAleatoria.Checked)
                this.secuencia = EmparejarSecuencia.ALEATORIA;
            else if (this.rbInversa.Checked)
                this.secuencia = EmparejarSecuencia.INVERSA;
        }

        private void tbSoloNumeros_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(8))
                //se pulso la tecla retroceso
                e.Handled = false;
            else if (e.KeyChar < '0' || e.KeyChar > '9')
                //desechar los caracteres que no son digitos
                e.Handled = true;
        }

        private void btAceptar_Click(object sender, EventArgs e)
        {
            if (this.tbTiempo.Text != "")
                this.tiempo = Convert.ToInt32(this.tbTiempo.Text);
        }

        private void btIzqSup_Click(object sender, EventArgs e)
        {
            this.etInformacion.Text = rm.GetString("par00")+"-"+rm.GetString("par02");//"izquierda-superior";
            this.etInformacion.ForeColor = Color.Blue;
            this.posicion = PosicionBotonEmparejar.IZQUIERDA_SUPERIOR;
            cambiarFondo((Button)sender);
        }

        private void btIzqCentro_Click(object sender, EventArgs e)
        {
            this.etInformacion.Text = rm.GetString("par00") + "-" + rm.GetString("par03"); //"izquierda-centro";
            this.etInformacion.ForeColor = Color.Blue;
            this.posicion = PosicionBotonEmparejar.IZQUIERDA_CENTRO;
            cambiarFondo((Button)sender);
        }

        private void btIzqInf_Click(object sender, EventArgs e)
        {
            this.etInformacion.Text = rm.GetString("par00") + "-" + rm.GetString("par04");//"izquierda-inferior";
            this.etInformacion.ForeColor = Color.Blue;
            this.posicion = PosicionBotonEmparejar.IZQUIERDA_INFERIOR;
            cambiarFondo((Button)sender);
        }

        private void btDerSup_Click(object sender, EventArgs e)
        {
            this.etInformacion.Text = rm.GetString("par01") + "-" + rm.GetString("par02");//"derecha-superior";
            this.etInformacion.ForeColor = Color.OrangeRed;
            this.posicion = PosicionBotonEmparejar.DERECHA_SUPERIOR;
            cambiarFondo((Button)sender);
        }

        private void btDerCentro_Click(object sender, EventArgs e)
        {
            this.etInformacion.Text = rm.GetString("par01") + "-" + rm.GetString("par03"); //"derecha-centro";
            this.etInformacion.ForeColor = Color.OrangeRed;
            this.posicion = PosicionBotonEmparejar.DERECHA_CENTRO;
            cambiarFondo((Button)sender);
        }

        private void btDerInf_Click(object sender, EventArgs e)
        {
            this.etInformacion.Text = rm.GetString("par01") + "-" + rm.GetString("par04"); //"derecha-inferior";
            this.etInformacion.ForeColor = Color.OrangeRed;
            this.posicion = PosicionBotonEmparejar.DERECHA_INFERIOR;
            cambiarFondo((Button)sender);
        }

        private void btA_Click(object sender, EventArgs e)
        {
            this.etInformacion.Text = rm.GetString("par05");//"aleatoria";
            this.etInformacion.ForeColor = Color.Green;
            this.posicion = PosicionBotonEmparejar.ALEATORIA;
            cambiarFondo((Button)sender);
        }

        private void cambiarFondo(Button b)
        {
            this.btIzqCentro.BackColor = SystemColors.Control;
            this.btIzqInf.BackColor = SystemColors.Control;
            this.btIzqSup.BackColor = SystemColors.Control;
            this.btDerCentro.BackColor = SystemColors.Control;
            this.btDerInf.BackColor = SystemColors.Control;
            this.btDerSup.BackColor = SystemColors.Control;
            this.btA.BackColor = SystemColors.Control;

            b.BackColor = Color.Yellow;
        }
    }
}
