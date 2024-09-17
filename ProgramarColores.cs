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
    public partial class ProgramarColores : Form
    {
        public bool interTexto, interColor, interTodo, tacharTexto, tacharTodo, tacharColor, tacharNada;
        
        public ProgramarColores(TipoRejilla tr, bool[] vector, bool uno, bool dos, bool tres, bool cuatro)
        {
            InitializeComponent();
            this.interTexto = vector[0];
            this.interColor = vector[1];
            this.interTodo = vector[2];
            this.tacharTodo = cuatro;
            this.tacharColor = uno;
            this.tacharNada = tres;
            this.tacharTexto = dos;
            
            inicializarRadioButons();
            
            if (tr == TipoRejilla.NUMERICA || tr == TipoRejilla.ABECEDARIO)
            {
                this.rbTacharNada.Enabled = true;
                this.rbTacharColor.Enabled = true;
                this.rbTacharTexto.Enabled = true;
                this.rbTacharTextoColor.Enabled = true;
                
            }
            else if (tr == TipoRejilla.LETRAS  || tr == TipoRejilla.WINGDINGS)
            {
                this.rbTacharColor.Enabled = true;
                this.rbTacharTextoColor.Enabled = true;
                this.rbTacharNada.Enabled = false;
                this.rbTacharTexto.Enabled = true;
                this.rbTacharNada.Checked = false;
            }          
        }

        private void rbIntercambiar_CheckedChanged(object sender, EventArgs e)
        {
            this.interTexto = this.rbIntercambiarTexto.Checked;
            this.interColor = this.rbIntercambiarColor.Checked;
            this.interTodo = this.rbIntercambiarTodo.Checked;
        }

        private void rbTachar_ChekedChanged(object sender, EventArgs e)
        {
            this.tacharTexto = this.rbTacharTexto.Checked;
            this.tacharTodo = this.rbTacharTextoColor.Checked;
            this.tacharColor = this.rbTacharColor.Checked;
            this.tacharNada = this.rbTacharNada.Checked;         
        }

        private void inicializarRadioButons()
        {
            this.rbIntercambiarTexto.Checked = this.interTexto;
            this.rbIntercambiarColor.Checked = this.interColor;
            this.rbIntercambiarTodo.Checked = this.interTodo;
            this.rbTacharTexto.Checked = this.tacharTexto;
            this.rbTacharTextoColor.Checked = this.tacharTodo;
            this.rbTacharNada.Checked = this.tacharNada;
            this.rbTacharColor.Checked = this.tacharColor;
        }
    }
}
