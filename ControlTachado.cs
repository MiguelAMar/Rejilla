/*
 * Miguel Ángel Martínez Jiménez
 * Versión: 2.0.
 * Fecha: Septiembre  2024.
 * Aplicación: Evaluación y Entrenamiento de la Atención.
 * 
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Resources;
using System.Runtime.Versioning;

namespace demorejilla
{
    [SupportedOSPlatform("windows")]
    public partial class ControlTachado : Form
    {
        //A pintar le pasamos el diccionario de los valores que queremos en la tabla.
        private SortedList<String, DatosControlTachado> lista;
        private DataGridView dgv;
        private List<String> orden;
        private DateTime[] datoTiempo;
        private bool ascendenteDescendente;
        private TipoRejilla tr;
        private FontFamily tipoFuente;

        private int aciertos, fallos;
        private ResourceManager rm;

        public ControlTachado(FontFamily ff, SortedList<String,DatosControlTachado>l1, List<String> l2, DateTime[] dt, bool ordenTachar, TipoRejilla t, int verdad, int mentira)
        {
            rm = new ResourceManager("demorejilla.Recursos", typeof(MetiendoDatos).Assembly);
            lista = l1;
            orden = l2;
            datoTiempo = new DateTime[dt.Length];
            dt.CopyTo(datoTiempo, 0);
            this.ascendenteDescendente = ordenTachar;
            this.tr = t;
            aciertos = verdad;
            fallos = mentira;
            this.tipoFuente = ff;

            Pintar();
            
            this.tbordenTachado.Select(0,0);
        }

        private void Pintar()
        {
            InitializeComponent();
            InicializaDGV();
            this.etAciertos.Text = aciertos.ToString();
            this.etFallos.Text = fallos.ToString();
        }

        private void rejilla_Letras(DataGridView data, List<DatosControlTachado> ldct)
        {
            string s = "";
            string[] arrayTitulos = new string[lista.Count];
            int indiceTit = 0;
            foreach (KeyValuePair<String, DatosControlTachado> elemento in lista)
            {
                s = elemento.Key;
                arrayTitulos[elemento.Value.indiceItem()] = elemento.Value.obtenerTitulo();
                indiceTit++;
                ldct.Add(elemento.Value);
            }
            string[] arrayPulsaciones = new string[arrayTitulos.Length];
            string[] arrayTiempos = new string[arrayTitulos.Length];
            
            foreach (DatosControlTachado dct in ldct)
            {
                arrayPulsaciones[dct.indiceItem()] = dct.vecesPulsado().ToString();
                arrayTiempos[dct.indiceItem()] = dct.obtenerTiempo(this.datoTiempo, this.ascendenteDescendente);
            }
            int valorColum = 0;
            foreach (string stv in arrayTitulos)
                data.Columns.Add((++valorColum).ToString(), stv);

            data.Rows.Add(arrayTiempos);
            data.Rows.Add(arrayPulsaciones);
        }

        private void rejillaAscendente(DataGridView data, List<DatosControlTachado> ldct)
        {
            string s = "";
            string[] arrayTitulos = new string[lista.Count]; 
            int indiceTit = 0;
            foreach (KeyValuePair<String, DatosControlTachado> elemento in lista)
            {
                s = elemento.Key;
                if (elemento.Value.localizado())
                {
                    arrayTitulos[elemento.Value.indiceItem()] = elemento.Value.obtenerTitulo();
                    indiceTit++;
                    ldct.Add(elemento.Value);
                }                   
            }
            string[] arrayPulsaciones = new string[lista.Count];
            string[] arrayTiempos = new string[lista.Count];

            foreach (DatosControlTachado dct in ldct)
            {              
                arrayPulsaciones[dct.indiceItem()] = dct.vecesPulsado().ToString();
                arrayTiempos[dct.indiceItem()] = dct.obtenerTiempo(this.datoTiempo, this.ascendenteDescendente);
            }
            int valorColum = 0;
            foreach (string stv in arrayTitulos)
                data.Columns.Add((++valorColum).ToString(), stv);

            data.Rows.Add(arrayTiempos);
            data.Rows.Add(arrayPulsaciones);
        }

        private void rejillaDescendente(DataGridView data, List<DatosControlTachado> ldct)
        {
            int indiceInverso = 0;
            String[] normal = new string[lista.Count];
            String[] inversa = new string[normal.Length];
            foreach (KeyValuePair<String, DatosControlTachado> elemento in lista)
            {
                normal[indiceInverso] = elemento.Key;
                indiceInverso++;
            }

            for (int ind = normal.Length - 1; ind >= 0; ind--)
            {
                inversa[normal.Length - 1 - ind] = normal[ind];
            }

            indiceInverso = inversa.Length;
            foreach (KeyValuePair<String, DatosControlTachado> elemento in lista)
            {
                if (elemento.Value.localizado())
                {
                    data.Columns.Add(rm.GetString("olv10")/*"columna"*/+ indiceInverso.ToString(), inversa[inversa.Length - indiceInverso]);
                    ldct.Add(elemento.Value);
                    indiceInverso--;
                }
            }

            string[] arrayPulsaciones = new string[data.ColumnCount];
            string[] arrayTiempos = new string[data.ColumnCount];

            foreach (DatosControlTachado dct in ldct)
            {
                arrayPulsaciones[dct.indiceItem()] = dct.vecesPulsado().ToString();
                arrayTiempos[dct.indiceItem()] = dct.obtenerTiempo(this.datoTiempo, this.ascendenteDescendente);
            }
            data.Rows.Add(arrayTiempos);
            data.Rows.Add(arrayPulsaciones);
        }

        private void InicializaDGV()
        {
            this.dgv = new DataGridView();
            this.SuspendLayout();
            List<DatosControlTachado> ldct = new List<DatosControlTachado>();
            //creo el DGV;

            if (tr == TipoRejilla.LETRAS || tr == TipoRejilla.ABECEDARIO)
            {
                rejilla_Letras(dgv, ldct);
            }
            else if (tr == TipoRejilla.NUMERICA)
            {
                rejillaAscendente(dgv,ldct);
            }

            this.dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv.AllowUserToAddRows = false;
            this.dgv.AllowUserToDeleteRows = false;
            //this.dgv.AutoSize = true;
            this.dgv.BorderStyle = BorderStyle.FixedSingle;
            this.dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Raised;
            this.dgv.ReadOnly = true;
            this.dgv.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Sunken;
            this.dgv.RowHeadersVisible = false;
            this.dgv.BackgroundColor = Color.CornflowerBlue;
            this.dgv.ScrollBars = ScrollBars.Both;
            this.dgv.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.dgv.Size = new Size(this.Width, 100);

            this.Controls.Add(this.dgv);
            this.ResumeLayout(false);
           
            String aux = "";
            foreach (String ss in orden)
                aux = aux +  ss + " -> ";

            aux = aux.Substring(0, aux.Length - 4);       
            this.tbordenTachado.Text = aux;
        }
    }
}
