/*
 * Miguel Ángel Martínez Jiménez
 * Versión: 2.0.
 * Fecha: Septiembre  2024.
 * Aplicación: Evaluación y Entrenamiento de la Atención.
 * 
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Versioning;
using System.Windows.Forms;

namespace demorejilla
{
    [SupportedOSPlatform("windows")]
    public partial class DialogoLetras : Form
    {
        public string texto;
        public Font fuenteActual;
        public List<String> listaLetras;
        private List<String> nombreTexboxNoColorear;
        public DialogoLetras()
        {
            InitializeComponent();
            texto = "";
            listaLetras = new List<string>();
            this.fuenteActual = tbA.Font;
        }


        public DialogoLetras(List<string> let, Font fuente)
        {
            InitializeComponent();
            texto = "";
            this.fuenteActual = fuente;
            this.listaLetras = let;
            this.nombreTexboxNoColorear = new List<String>();
            completarNombreTbNoColorear();
            ponerVerdeTextBoxSelecionados();
            cambiarFuente(fuente);
            this.etFuente.Text = fuente.Name + ", "
                + fuente.Size.ToString() + ", "
                + fuente.Style.ToString();
        }


        private void completarNombreTbNoColorear()
        {
            string original = "tb#";
            string cadena = "";
            for (Char c = 'A'; c <= 'Z'; c++)
            {
                cadena = original.Replace('#', c);
                this.nombreTexboxNoColorear.Add(cadena);
            }
        }
    

        private void ponerVerdeTextBoxSelecionados()
        {
            foreach (TextBox tb in this.Controls.OfType<TextBox>())
            {
                if (!nombreTexboxNoColorear.Contains(tb.Name))
                {
                    if (this.listaLetras.Contains(tb.Text))
                    {
                        tb.BackColor = Color.YellowGreen;
                        tb.Font = fuenteActual;
                    }
                }
            }
        }

        private void ponerVerdeTextBoxSelecionados2()
        {
            string original = "tb#F";
            for (Char c = 'A'; c <= 'Z'; c++)
            {
                original.Replace('#', c);
                
            }
            for (char c = 'A'; c <= 'Z'; c++)
            {
                original.Replace('#', c);
                TextBox aux = new TextBox();//this.Controls.Find(original, true) as TextBox;
                aux.BackColor = Color.YellowGreen;

            }
            
            foreach (Control c in this.Controls)
            {
                if (c is TextBox)
                {
                    TextBox txt = (TextBox)c;
                    if (!nombreTexboxNoColorear.Contains(txt.Name))
                    {
                        txt.BackColor = Color.YellowGreen;
                        txt.Font = fuenteActual;
                    }
                }
            }
        }

        private void tB_MouseDown(object sender, MouseEventArgs e)
        {
            TextBox t = (TextBox)sender;
            if (t.BackColor == Color.YellowGreen)
            {
                t.BackColor = Color.Empty;
                listaLetras.Remove(t.Text);
            }
            else
            {
                t.BackColor = Color.YellowGreen;
                listaLetras.Add(t.Text);
            }
        }

        private void DialogoLetras_Load(object sender, EventArgs e)
        {
            /*
            etFuente.Text = this.Font.Name + ", " +
                this.tbA.Font.Size.ToString() + ", " +
                this.Font.Style.ToString();
            */
        }

        private void btFuente_Click(object sender, EventArgs e)
        {
            FontDialog diagFuente = new FontDialog();
            diagFuente.MinSize = 20;
            diagFuente.MaxSize = 20;
            diagFuente.ShowEffects = false;
            diagFuente.FontMustExist = true;
            diagFuente.ScriptsOnly = false;

            if (diagFuente.ShowDialog() == DialogResult.OK)
            {
                etFuente.Text = diagFuente.Font.Name + ", "
                + diagFuente.Font.Size.ToString() + ", "
                + diagFuente.Font.Style.ToString();
                this.fuenteActual = diagFuente.Font;
                cambiarFuente(diagFuente.Font);
            }
        }

        private void cambiarFuente(Font f)
        {
            this.tbAF.Font = f;
            this.tbBF.Font = f;
            this.tbCF.Font = f;
            this.tbDF.Font = f;
            this.tbEF.Font = f;
            this.tbFF.Font = f;
            this.tbGF.Font = f;
            this.tbHF.Font = f;
            this.tbIF.Font = f;
            this.tbJF.Font = f;
            this.tbKF.Font = f;
            this.tbLF.Font = f;
            this.tbMF.Font = f;
            this.tbNF.Font = f;
            this.tbÑF.Font = f;
            this.tbOF.Font = f;
            this.tbPF.Font = f;
            this.tbQF.Font = f;
            this.tbRF.Font = f;
            this.tbSF.Font = f;
            this.tbTF.Font = f;
            this.tbUF.Font = f;
            this.tbVF.Font = f;
            this.tbWF.Font = f;
            this.tbXF.Font = f;
            this.tbYF.Font = f;
            this.tbZF.Font = f;

            this.tbamF.Font = f;
            this.tbbmF.Font = f;
            this.tbcmF.Font = f;
            this.tbdmF.Font = f;
            this.tbemF.Font = f;
            this.tbfmF.Font = f;
            this.tbgmF.Font = f;
            this.tbhmF.Font = f;
            this.tbimF.Font = f;
            this.tbjmF.Font = f;
            this.tbkmF.Font = f;
            this.tblmF.Font = f;
            this.tbmmF.Font = f;
            this.tbnmF.Font = f;
            this.tbñmF.Font = f;
            this.tbomF.Font = f;
            this.tbpmF.Font = f;
            this.tbqmF.Font = f;
            this.tbrmF.Font = f;
            this.tbsmF.Font = f;
            this.tbtmF.Font = f;
            this.tbumF.Font = f;
            this.tbvmF.Font = f;
            this.tbwmF.Font = f;
            this.tbxmF.Font = f;
            this.tbymF.Font = f;
            this.tbzmF.Font = f;
        }

        private void btSeleccionarTodas_Click(object sender, EventArgs e)
        {
            this.tbAF.BackColor = Color.YellowGreen;
            this.tbBF.BackColor = Color.YellowGreen;
            this.tbCF.BackColor = Color.YellowGreen;
            this.tbDF.BackColor = Color.YellowGreen;
            this.tbEF.BackColor = Color.YellowGreen;
            this.tbFF.BackColor = Color.YellowGreen;
            this.tbGF.BackColor = Color.YellowGreen;
            this.tbHF.BackColor = Color.YellowGreen;
            this.tbIF.BackColor = Color.YellowGreen;
            this.tbJF.BackColor = Color.YellowGreen;
            this.tbKF.BackColor = Color.YellowGreen;
            this.tbLF.BackColor = Color.YellowGreen;
            this.tbMF.BackColor = Color.YellowGreen;
            this.tbNF.BackColor = Color.YellowGreen;
            this.tbÑF.BackColor = Color.YellowGreen;
            this.tbOF.BackColor = Color.YellowGreen;
            this.tbPF.BackColor = Color.YellowGreen;
            this.tbQF.BackColor = Color.YellowGreen;
            this.tbRF.BackColor = Color.YellowGreen;
            this.tbSF.BackColor = Color.YellowGreen;
            this.tbTF.BackColor = Color.YellowGreen;
            this.tbUF.BackColor = Color.YellowGreen;
            this.tbVF.BackColor = Color.YellowGreen;
            this.tbWF.BackColor = Color.YellowGreen;
            this.tbXF.BackColor = Color.YellowGreen;
            this.tbYF.BackColor = Color.YellowGreen;
            this.tbZF.BackColor = Color.YellowGreen;

            this.listaLetras.Add(this.tbAF.Text);
            this.listaLetras.Add(this.tbBF.Text);
            this.listaLetras.Add(this.tbCF.Text);
            this.listaLetras.Add(this.tbDF.Text);
            this.listaLetras.Add(this.tbEF.Text);
            this.listaLetras.Add(this.tbFF.Text);
            this.listaLetras.Add(this.tbGF.Text);
            this.listaLetras.Add(this.tbHF.Text);
            this.listaLetras.Add(this.tbIF.Text);
            this.listaLetras.Add(this.tbJF.Text);
            this.listaLetras.Add(this.tbKF.Text);
            this.listaLetras.Add(this.tbLF.Text);
            this.listaLetras.Add(this.tbMF.Text);
            this.listaLetras.Add(this.tbNF.Text);
            this.listaLetras.Add(this.tbÑF.Text);
            this.listaLetras.Add(this.tbOF.Text);
            this.listaLetras.Add(this.tbPF.Text);
            this.listaLetras.Add(this.tbQF.Text);
            this.listaLetras.Add(this.tbRF.Text);
            this.listaLetras.Add(this.tbSF.Text);
            this.listaLetras.Add(this.tbTF.Text);
            this.listaLetras.Add(this.tbUF.Text);
            this.listaLetras.Add(this.tbVF.Text);
            this.listaLetras.Add(this.tbWF.Text);
            this.listaLetras.Add(this.tbXF.Text);
            this.listaLetras.Add(this.tbYF.Text);
            this.listaLetras.Add(this.tbZF.Text);

            this.tbamF.BackColor = Color.YellowGreen;
            this.tbbmF.BackColor = Color.YellowGreen;
            this.tbcmF.BackColor = Color.YellowGreen;
            this.tbdmF.BackColor = Color.YellowGreen;
            this.tbemF.BackColor = Color.YellowGreen;
            this.tbfmF.BackColor = Color.YellowGreen;
            this.tbgmF.BackColor = Color.YellowGreen;
            this.tbhmF.BackColor = Color.YellowGreen;
            this.tbimF.BackColor = Color.YellowGreen;
            this.tbjmF.BackColor = Color.YellowGreen;
            this.tbkmF.BackColor = Color.YellowGreen;
            this.tblmF.BackColor = Color.YellowGreen;
            this.tbmmF.BackColor = Color.YellowGreen;
            this.tbnmF.BackColor = Color.YellowGreen;
            this.tbñmF.BackColor = Color.YellowGreen;
            this.tbomF.BackColor = Color.YellowGreen;
            this.tbpmF.BackColor = Color.YellowGreen;
            this.tbqmF.BackColor = Color.YellowGreen;
            this.tbrmF.BackColor = Color.YellowGreen;
            this.tbsmF.BackColor = Color.YellowGreen;
            this.tbtmF.BackColor = Color.YellowGreen;
            this.tbumF.BackColor = Color.YellowGreen;
            this.tbvmF.BackColor = Color.YellowGreen;
            this.tbwmF.BackColor = Color.YellowGreen;
            this.tbxmF.BackColor = Color.YellowGreen;
            this.tbymF.BackColor = Color.YellowGreen;
            this.tbzmF.BackColor = Color.YellowGreen;

            this.listaLetras.Add(this.tbamF.Text);
            this.listaLetras.Add(this.tbbmF.Text);
            this.listaLetras.Add(this.tbcmF.Text);
            this.listaLetras.Add(this.tbdmF.Text);
            this.listaLetras.Add(this.tbemF.Text);
            this.listaLetras.Add(this.tbfmF.Text);
            this.listaLetras.Add(this.tbgmF.Text);
            this.listaLetras.Add(this.tbhmF.Text);
            this.listaLetras.Add(this.tbimF.Text);
            this.listaLetras.Add(this.tbjmF.Text);
            this.listaLetras.Add(this.tbkmF.Text);
            this.listaLetras.Add(this.tblmF.Text);
            this.listaLetras.Add(this.tbmmF.Text);
            this.listaLetras.Add(this.tbnmF.Text);
            this.listaLetras.Add(this.tbñmF.Text);
            this.listaLetras.Add(this.tbomF.Text);
            this.listaLetras.Add(this.tbpmF.Text);
            this.listaLetras.Add(this.tbqmF.Text);
            this.listaLetras.Add(this.tbrmF.Text);
            this.listaLetras.Add(this.tbsmF.Text);
            this.listaLetras.Add(this.tbtmF.Text);
            this.listaLetras.Add(this.tbumF.Text);
            this.listaLetras.Add(this.tbvmF.Text);
            this.listaLetras.Add(this.tbwmF.Text);
            this.listaLetras.Add(this.tbxmF.Text);
            this.listaLetras.Add(this.tbymF.Text);
            this.listaLetras.Add(this.tbzmF.Text);
        }

        private void btBorrar_Click(object sender, EventArgs e)
        {
            this.tbAF.BackColor = Color.Empty;
            this.tbBF.BackColor = Color.Empty;
            this.tbCF.BackColor = Color.Empty;
            this.tbDF.BackColor = Color.Empty;
            this.tbEF.BackColor = Color.Empty;
            this.tbFF.BackColor = Color.Empty;
            this.tbGF.BackColor = Color.Empty;
            this.tbHF.BackColor = Color.Empty;
            this.tbIF.BackColor = Color.Empty;
            this.tbJF.BackColor = Color.Empty;
            this.tbKF.BackColor = Color.Empty;
            this.tbLF.BackColor = Color.Empty;
            this.tbMF.BackColor = Color.Empty;
            this.tbNF.BackColor = Color.Empty;
            this.tbÑF.BackColor = Color.Empty;
            this.tbOF.BackColor = Color.Empty;
            this.tbPF.BackColor = Color.Empty;
            this.tbQF.BackColor = Color.Empty;
            this.tbRF.BackColor = Color.Empty;
            this.tbSF.BackColor = Color.Empty;
            this.tbTF.BackColor = Color.Empty;
            this.tbUF.BackColor = Color.Empty;
            this.tbVF.BackColor = Color.Empty;
            this.tbWF.BackColor = Color.Empty;
            this.tbXF.BackColor = Color.Empty;
            this.tbYF.BackColor = Color.Empty;
            this.tbZF.BackColor = Color.Empty;

            this.tbamF.BackColor = Color.Empty;
            this.tbbmF.BackColor = Color.Empty;
            this.tbcmF.BackColor = Color.Empty;
            this.tbdmF.BackColor = Color.Empty;
            this.tbemF.BackColor = Color.Empty;
            this.tbfmF.BackColor = Color.Empty;
            this.tbgmF.BackColor = Color.Empty;
            this.tbhmF.BackColor = Color.Empty;
            this.tbimF.BackColor = Color.Empty;
            this.tbjmF.BackColor = Color.Empty;
            this.tbkmF.BackColor = Color.Empty;
            this.tblmF.BackColor = Color.Empty;
            this.tbmmF.BackColor = Color.Empty;
            this.tbnmF.BackColor = Color.Empty;
            this.tbñmF.BackColor = Color.Empty;
            this.tbomF.BackColor = Color.Empty;
            this.tbpmF.BackColor = Color.Empty;
            this.tbqmF.BackColor = Color.Empty;
            this.tbrmF.BackColor = Color.Empty;
            this.tbsmF.BackColor = Color.Empty;
            this.tbtmF.BackColor = Color.Empty;
            this.tbumF.BackColor = Color.Empty;
            this.tbvmF.BackColor = Color.Empty;
            this.tbwmF.BackColor = Color.Empty;
            this.tbxmF.BackColor = Color.Empty;
            this.tbymF.BackColor = Color.Empty;
            this.tbzmF.BackColor = Color.Empty;

            this.listaLetras.Clear();
        }
    }
}
