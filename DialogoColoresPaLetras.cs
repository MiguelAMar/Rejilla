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
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Resources;
using System.Runtime.Versioning;

namespace demorejilla
{
    [SupportedOSPlatform("windows")]
    public partial class DialogoColoresPaLetras : Form
    {
        public ClaseColores colores;
        public Color colorcito;
        private List<Color> miListaColores;
        private int soloUno;
        private bool Flas_Colores;
        private String directorioInicial;
        private ResourceManager rm;

        public DialogoColoresPaLetras(ClaseColores cc, bool com, String inicio)
        {
            InitializeComponent();
            rm = new ResourceManager("demorejilla.Recursos", typeof(MetiendoDatos).Assembly);
            this.directorioInicial = inicio;
            colores = new ClaseColores();
            colores = cc;
            this.miListaColores = new List<Color>();

            rellenarBotones();
            this.soloUno = 0;
            this.Flas_Colores = com;
            if (this.Flas_Colores)
            {
                this.btSeleccionarTodosColores.Enabled = true;
                this.btBorrarSeleccionColores.Enabled = true;
                this.btColoresBorrarColor.Enabled = false;
            }
            else
            {
                this.btSeleccionarTodosColores.Enabled = false;
                this.btColoresBorrarColor.Enabled = true;
                this.btBorrarSeleccionColores.Enabled = false;
            }
        }

        private void ButtonColores_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            if (!this.Flas_Colores)
            {
                todosBotonesStandar();
                b.FlatStyle = FlatStyle.Flat;
                this.miListaColores.Add(b.BackColor);
                this.colorcito = b.BackColor;
                soloUno = 1;
            }
            else
            {
                if (b.FlatStyle == FlatStyle.Standard)
                {
                    b.FlatStyle = FlatStyle.Flat;
                    this.miListaColores.Add(b.BackColor);
                    soloUno++;
                    this.colorcito = b.BackColor;
                }
                else if (b.FlatStyle == FlatStyle.Flat)
                {
                    b.FlatStyle = FlatStyle.Standard;
                    this.miListaColores.Remove(b.BackColor);
                    soloUno--;
                    this.colorcito = SystemColors.Control; //Color.Empty;
                }
            }
        }

        private void rellenarBotones()
        {
            btColor1.BackColor = colores.obtenerColor(1);
            btColor2.BackColor = colores.obtenerColor(2);
            btColor3.BackColor = colores.obtenerColor(3);
            btColor4.BackColor = colores.obtenerColor(4);
            btColor5.BackColor = colores.obtenerColor(5);
            btColor6.BackColor = colores.obtenerColor(6);
            btColor7.BackColor = colores.obtenerColor(7);
            btColor8.BackColor = colores.obtenerColor(8);
            btColor9.BackColor = colores.obtenerColor(9);
            btColor10.BackColor = colores.obtenerColor(10);
            btColor11.BackColor = colores.obtenerColor(11);
            btColor12.BackColor = colores.obtenerColor(12);
            btColor13.BackColor = colores.obtenerColor(13);
            btColor14.BackColor = colores.obtenerColor(14);
            btColor15.BackColor = colores.obtenerColor(0);
        }

        private void btColoresCambiarColor_Click(object sender, EventArgs e)
        {

            Button b = (Button)sender;
            if (this.soloUno == 0)
                //MessageBox.Show("No se ha seleccionado ningún color para cambiar.", "Información", MessageBoxButtons.OK);
                MessageBox.Show(rm.GetString("olv01"),rm.GetString("olv02"),MessageBoxButtons.OK);
            else if (comprobarSoloUnoParaCambiarColor())
            {
                this.miListaColores.Clear();
                ColorDialog cd = new ColorDialog();
                if (cd.ShowDialog() == DialogResult.OK)
                {
                    if (colores.contieneColor(cd.Color))
                    {
                        //MessageBox.Show("La plantilla ya contiene ese color. Eliga otro color distinto.", "Información", MessageBoxButtons.OK);
                        MessageBox.Show(rm.GetString("olv03"), rm.GetString("olv02"), MessageBoxButtons.OK);
                    }
                    else
                    {
                        cambiarColor(cd.Color);
                    }
                }
            }
            else
            {
                //MessageBox.Show("Solo se puede cambiar un color a la vez. Seleccione un sólo color por favor.", "Error", MessageBoxButtons.OK);
                MessageBox.Show(rm.GetString("olv04"), "Error", MessageBoxButtons.OK);
            }
        }

        private bool comprobarSoloUnoParaCambiarColor()
        {
            return soloUno == 1; 
        }

        private void cambiarColor(Color c)
        {            
            if (btColor1.FlatStyle == FlatStyle.Flat)
            {
                colores.borrarColor(btColor1.BackColor);
                btColor1.BackColor = c;
                colores.añadirColor(btColor1.BackColor);
                btColor1.FlatStyle = FlatStyle.Standard;
            }
            else if (btColor2.FlatStyle == FlatStyle.Flat)
            {
                colores.borrarColor(btColor2.BackColor);
                btColor2.BackColor = c;
                btColor2.FlatStyle = FlatStyle.Standard;
                colores.añadirColor(btColor2.BackColor);
            }
            else if (btColor3.FlatStyle == FlatStyle.Flat)
            {
                colores.borrarColor(btColor3.BackColor);
                btColor3.BackColor = c;
                btColor3.FlatStyle = FlatStyle.Standard;
                colores.añadirColor(btColor3.BackColor);
            }
            else if (btColor4.FlatStyle == FlatStyle.Flat)
            {
                colores.borrarColor(btColor4.BackColor);
                btColor4.BackColor = c;
                btColor4.FlatStyle = FlatStyle.Standard;
                colores.añadirColor(btColor4.BackColor);
            }
            else if (btColor5.FlatStyle == FlatStyle.Flat)
            {
                colores.borrarColor(btColor5.BackColor);
                btColor5.BackColor = c;
                btColor5.FlatStyle = FlatStyle.Standard;
                colores.añadirColor(btColor5.BackColor);
            }
            else if (btColor6.FlatStyle == FlatStyle.Flat)
            {
                colores.borrarColor(btColor6.BackColor);
                btColor6.BackColor = c;
                btColor6.FlatStyle = FlatStyle.Standard;
                colores.añadirColor(btColor6.BackColor);
            }
            else if (btColor7.FlatStyle == FlatStyle.Flat)
            {
                colores.borrarColor(btColor7.BackColor);
                btColor7.BackColor = c;
                btColor7.FlatStyle = FlatStyle.Standard;
                colores.añadirColor(btColor7.BackColor);
            }
            else if (btColor8.FlatStyle == FlatStyle.Flat)
            {
                colores.borrarColor(btColor8.BackColor);
                btColor8.BackColor = c;
                btColor8.FlatStyle = FlatStyle.Standard;
                colores.añadirColor(btColor8.BackColor);
            }
            else if (btColor9.FlatStyle == FlatStyle.Flat)
            {
                colores.borrarColor(btColor9.BackColor);
                btColor9.BackColor = c;
                btColor9.FlatStyle = FlatStyle.Standard;
                colores.añadirColor(btColor9.BackColor);
            }
            else if (btColor10.FlatStyle == FlatStyle.Flat)
            {
                colores.borrarColor(btColor10.BackColor);
                btColor10.BackColor = c;
                btColor10.FlatStyle = FlatStyle.Standard;
                colores.añadirColor(btColor10.BackColor);
            }
            else if (btColor11.FlatStyle == FlatStyle.Flat)
            {
                colores.borrarColor(btColor11.BackColor);
                btColor11.BackColor = c;
                btColor11.FlatStyle = FlatStyle.Standard;
                colores.añadirColor(btColor11.BackColor);
            }
            else if (btColor12.FlatStyle == FlatStyle.Flat)
            {
                colores.borrarColor(btColor12.BackColor);
                btColor12.BackColor = c;
                btColor12.FlatStyle = FlatStyle.Standard;
                colores.añadirColor(btColor12.BackColor);
            }
            else if (btColor13.FlatStyle == FlatStyle.Flat)
            {
                colores.borrarColor(btColor13.BackColor);
                btColor13.BackColor = c;
                btColor13.FlatStyle = FlatStyle.Standard;
                colores.añadirColor(btColor13.BackColor);
            }
            else if (btColor14.FlatStyle == FlatStyle.Flat)
            {
                colores.borrarColor(btColor14.BackColor);
                btColor14.BackColor = c;
                btColor14.FlatStyle = FlatStyle.Standard;
                colores.añadirColor(btColor14.BackColor);
            }
            else if (btColor15.FlatStyle == FlatStyle.Flat)
            {
                colores.borrarColor(btColor15.BackColor);
                btColor15.BackColor = c;
                btColor15.FlatStyle = FlatStyle.Standard;
                colores.añadirColor(btColor15.BackColor);
            }
            this.soloUno--;
        }

        private void todosBotonesFlat()
        {
            btColor1.FlatStyle = FlatStyle.Flat;
            btColor2.FlatStyle = FlatStyle.Flat;
            btColor3.FlatStyle = FlatStyle.Flat;
            btColor4.FlatStyle = FlatStyle.Flat;
            btColor5.FlatStyle = FlatStyle.Flat;
            btColor6.FlatStyle = FlatStyle.Flat;
            btColor7.FlatStyle = FlatStyle.Flat;
            btColor8.FlatStyle = FlatStyle.Flat;
            btColor9.FlatStyle = FlatStyle.Flat;
            btColor10.FlatStyle = FlatStyle.Flat;
            btColor11.FlatStyle = FlatStyle.Flat;
            btColor12.FlatStyle = FlatStyle.Flat;
            btColor13.FlatStyle = FlatStyle.Flat;
            btColor14.FlatStyle = FlatStyle.Flat;
            btColor15.FlatStyle = FlatStyle.Flat;
        }

        private void btSeleccionarTodosColores_Click(object sender, EventArgs e)
        {
            todosBotonesFlat();
            miListaColores.Clear();
            miListaColores = colores.obtenerTodosColores();
            this.soloUno = miListaColores.Count;
        }

        private void todosBotonesStandar()
        {
            btColor1.FlatStyle = FlatStyle.Standard;
            btColor2.FlatStyle = FlatStyle.Standard;
            btColor3.FlatStyle = FlatStyle.Standard;
            btColor4.FlatStyle = FlatStyle.Standard;
            btColor5.FlatStyle = FlatStyle.Standard;
            btColor6.FlatStyle = FlatStyle.Standard;
            btColor7.FlatStyle = FlatStyle.Standard;
            btColor8.FlatStyle = FlatStyle.Standard;
            btColor9.FlatStyle = FlatStyle.Standard;
            btColor10.FlatStyle = FlatStyle.Standard;
            btColor11.FlatStyle = FlatStyle.Standard;
            btColor12.FlatStyle = FlatStyle.Standard;
            btColor13.FlatStyle = FlatStyle.Standard;
            btColor14.FlatStyle = FlatStyle.Standard;
            btColor15.FlatStyle = FlatStyle.Standard;
        }

        private void btBorrarSeleccionColores_Click(object sender, EventArgs e)
        {
            todosBotonesStandar();
            this.miListaColores.Clear();
            this.soloUno = 0;
        }

        private void btGuardarColores_Click(object sender, EventArgs e)
        {
            String ruta = "";
            SaveFileDialog sfd = new SaveFileDialog();
            //sfd.Filter = "Archivos de plantillas de colores *.clr|*.clr";
            sfd.Filter = rm.GetString("olv05");
            sfd.DefaultExt = "clr";
            sfd.AddExtension = true;
            sfd.InitialDirectory = this.directorioInicial;

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                
                /*
                                     Stream fs = File.Create(ruta);
                                     BinaryFormatter seriador = new BinaryFormatter();
                                     seriador.Serialize(fs, colores);
                                     fs.Close();
                                     ------
                                     Stream fs = File.Create(ruta);
                                     JsonSerializerOptions opciones = new JsonSerializerOptions(); ;
                                     JsonSerializer.Serialize(fs, colores, opciones);
                                     fs.Close();
                                     */

                try
                {
                    ruta = sfd.FileName;
                    this.colores.GuardarEnArchivo(ruta);
                    //MessageBox.Show("Archivo guardado exitosamente.");
                }
                catch (Exception ex)
                {
                    throw new RejillaException(ex.Message);
                }
            }
        }

        private void btCargarColores_Click(object sender, EventArgs e)
        {
            String ruta = "";
            OpenFileDialog ofd = new OpenFileDialog();

            //ofd.Filter = "Archivos de plantillas de colores *.clr|*.clr";
            ofd.Filter = rm.GetString("olv05");
            ofd.InitialDirectory = this.directorioInicial;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                ruta = ofd.FileName;
                if (File.Exists(ruta))
                {
                    /*
                    Stream fs = File.OpenRead(ruta);
                    BinaryFormatter deseriador = new BinaryFormatter();
                    //ClaseColores nuevoObjeto;
                    
                    this.colores = (ClaseColores)deseriador.Deserialize(fs);
                    fs.Close();
                    rellenarBotones();
                    */
                    try
                    {
                        this.colores = new ClaseColores();
                        this.colores.CargarDesdeArchivo(ruta);
                        rellenarBotones();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al cargar el archivo: {ex.Message}");
                    }
                }
                else
                {
                    //MessageBox.Show("El fichero no existe.");
                    MessageBox.Show(rm.GetString("olv06"));
                }
            }
        }

        private void btColoresBorrarColor_Click(object sender, EventArgs e)
        {
            this.colorcito = SystemColors.Control; //Color.Empty;
        }

        public ClaseColores obtenerColores()
        {
            return this.colores;
        }

        public List<Color> obtenerColoresEscogidos()
        {
            return this.miListaColores;
        }
    }
}
