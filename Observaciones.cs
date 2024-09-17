/*
 * Miguel Ángel Martínez Jiménez
 * Versión: 2.0.
 * Fecha: Septiembre  2024.
 * Aplicación: Evaluación y Entrenamiento de la Atención.
 * 
 */

using DocumentFormat.OpenXml.Vml.Spreadsheet;
using System;
using System.IO;
using System.Resources;
using System.Runtime.Versioning;
using System.Windows.Forms;


namespace demorejilla
{
    [SupportedOSPlatform("windows")]
    public partial class Observaciones : Form
    {
        public String textoObservacion = "";
        private string file;
        private string fecha;
        private string nombre;
        private string apellido;
        private string observacionAnterior;
        private ResourceManager rm;
        private bool guardarBD;
        private string obdBaseDatos;
        private int codigoPK;


        public Observaciones(String msg, string rutaFichero, string name, string surname, string date, bool visible)
        {
            InitializeComponent();
            rm = new ResourceManager("demorejilla.Recursos", typeof(MetiendoDatos).Assembly);
            this.btGuardarObservaciones.Enabled = visible;
            this.observacionAnterior = msg;
            String[] lineas = msg.Split(new char[] { '¬' });
            String todoTexto = "";
            foreach (string s in lineas)
                todoTexto += s + "\r\n";

            this.tbObservaciones.Text = todoTexto;
            this.tbObservaciones.Select(0, 0);
            this.file = rutaFichero;
            this.nombre = name;
            this.apellido = surname;
            this.fecha = date;
            guardarBD = false;
        }

        public Observaciones(string observacioon, int codigo)
        {
            InitializeComponent();
            rm = new ResourceManager("demorejilla.Recursos", typeof(MetiendoDatos).Assembly);
            this.btGuardarObservaciones.Enabled = true;
            this.tbObservaciones.Text = observacioon;
            guardarBD = true;
            this.obdBaseDatos = observacioon;
            this.codigoPK = codigo;
            this.btGuardarObservaciones.Enabled = true;
            this.codigoPK = codigo;
        }

        private void btAceptar_Click(object sender, EventArgs e)
        {
            procesarTexto();
        }

        private void btLimpiar_Click(object sender, EventArgs e)
        {
            this.tbObservaciones.Clear();
            this.textoObservacion = "";
        }

        private void tbCaracterPermitidos_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(172))
                //pulso ¬
                e.Handled = true;
        }

        private void procesarTexto()
        {
            bool algunaletra = false;
            String cadena = "";
            String[] lineas = this.tbObservaciones.Text.Split(new char[] { '\n', '\r' }, StringSplitOptions.None);
            foreach (string s in lineas)
            {
                if (s.Length == 0)
                    cadena += '¬'; //son los intros de la observacion
                else
                {
                    cadena += s + " ";
                    algunaletra = true;
                }
            }
            if (algunaletra)
                this.textoObservacion = cadena;
            else
                this.textoObservacion = "";
        }

        private void guardarObservacionesFichero()
        {
            bool creado = false;
            int indiceTrozoCambio = -1;
            string[] trozoCambio = null;
            int POS_OBSERV = 39;//35
            try
            {
                if (this.file != null)
                {
                    char[] separChar = new char[4];
                    separChar[0] = Convert.ToChar(7);
                    separChar[1] = Convert.ToChar(8);
                    separChar[2] = '\n';
                    separChar[3] = '\r';
                    String cadena;
                    String[] ficheroPorTrozos;
                    string[] trocitosDeRegistros;

                    int posicionObservaciones;
                    FileStream fe = new FileStream(file, FileMode.Open, FileAccess.ReadWrite);
                    TextReader tr = new StreamReader(fe);
                    cadena = tr.ReadToEnd();
                    ficheroPorTrozos = cadena.Split(Convert.ToChar(6));
                    tr.Close();
                    fe.Close();
                    for (int i = 0; i < ficheroPorTrozos.Length; i++)
                    {
                        trocitosDeRegistros = ficheroPorTrozos[i].Split(separChar, StringSplitOptions.RemoveEmptyEntries);
                        if (trocitosDeRegistros[0] == this.fecha && trocitosDeRegistros[1].Equals(this.nombre) && trocitosDeRegistros[2].Equals(this.apellido))
                        {
                            posicionObservaciones = POS_OBSERV + /*27+4+3+1/*24*/ +System.Convert.ToInt32(trocitosDeRegistros[25 + 4 + 3 + 1 + 4/*22*/]) * System.Convert.ToInt32(trocitosDeRegistros[26 + 4 + 3 + 1 + 4/*23*/]);
                            posicionObservaciones += 4;// ver si hay musica
                            if (System.Convert.ToBoolean(trocitosDeRegistros[posicionObservaciones]))
                            {
                                int cantidadCanciones = Convert.ToInt32(trocitosDeRegistros[posicionObservaciones + 1]);
                                posicionObservaciones += cantidadCanciones + 1;
                            }
                            posicionObservaciones += 2; //esta el la posicion de la observaciones
                            procesarTexto();
                            if (this.textoObservacion == "")
                                this.textoObservacion = rm.GetString("inf37");
                            trocitosDeRegistros[posicionObservaciones] = this.textoObservacion;
                            //guardarlo todo en fichero;
                            trozoCambio = new string[trocitosDeRegistros.Length];
                            trocitosDeRegistros.CopyTo(trozoCambio, 0);
                            indiceTrozoCambio = i;
                            i = ficheroPorTrozos.Length;
                        }
                    }
                    /*
                     * La idea es reemplazarlo en la cadena y luego volcar esa cadena a fichero
                     */
                    File.Copy(this.file, this.file + ".bak");
                    creado = true;

                    FileStream fe2 = new FileStream(file, FileMode.Create, FileAccess.Write);
                    StreamWriter sw = new StreamWriter(fe2);
                    for (int i = 0; i < ficheroPorTrozos.Length - 1; i++)
                    {
                        if (i == indiceTrozoCambio)
                        {

                            sw.Write(Convert.ToChar(7));
                            sw.Write(trozoCambio[0]);
                            sw.Write(Convert.ToChar(8));
                            sw.WriteLine('\n');
                            for (int linea = 1; linea < trozoCambio.Length; linea++)
                            {
                                sw.WriteLine(trozoCambio[linea]);
                            }

                            sw.Write(Convert.ToChar(6));
                        }
                        else
                        {
                            sw.Write(ficheroPorTrozos[i]);
                            sw.Write(Convert.ToChar(6));
                        }
                    }
                    sw.Close();
                    fe2.Close();
                }
                //this.textoObservacion = "";
            }
            catch (Exception exc)
            {
                if (creado)
                {
                    File.Delete(this.file);
                    File.Copy(this.file + ".bak", this.file);
                }
                //MessageBox.Show("Error inesperado cambiando las observaciones. Restaurando el fichero a su estado original" + exc);
                this.textoObservacion = this.observacionAnterior;
                MessageBox.Show(rm.GetString("olv15"));
            }
            finally
            {
                File.Delete(this.file + ".bak");
                this.Close();
            }
        }
        private void btGuardarObservaciones_Click(object sender, EventArgs e)
        {
            if (guardarBD == false)//guardo en fichero
            {
                guardarObservacionesFichero();
            }else if (guardarBD == true)
            {
                guardarObservacionesBaseDatos(this.codigoPK, this.tbObservaciones.Text);
            }
        }

        private void guardarObservacionesBaseDatos(int codigo, string obs)
        {
            BaseDatos bd = new BaseDatos();
            bd.conectarBD();
            bd.actualizarObservacionesRejilla(codigo, obs);
            bd.cerrarConexiion();
        }
    }
}
