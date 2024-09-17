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
using System.IO;
using System.Linq;
using System.Resources;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Versioning;
using System.Windows.Forms;

namespace demorejilla
{
    [SupportedOSPlatform("windows")]
    public partial class DialogoImagenes : Form
    {
        public Image imagen;
        private string rutaImagen;
        private List<Image> listaImagenesDialogo;
        public ClaseImagen claseIman;
        private String directorioInicio;
        private ResourceManager rm;

               
        public DialogoImagenes(ClaseImagen clsi, String inicio)
        {
            InitializeComponent();
            rm = new ResourceManager("demorejilla.Recursos", typeof(MetiendoDatos).Assembly);
            claseIman = new ClaseImagen();

            this.directorioInicio = inicio;
   
            claseIman = clsi;
   
            listaImagenesDialogo = new List<Image>();
            listaImagenesDialogo = clsi.obtenerTodasImagenes();
            
            ponerImagenesBotones();
        }
        
        public DialogoImagenes(List<Image> listita)
        {
            InitializeComponent();
            listaImagenesDialogo = new List<Image>();

            listaImagenesDialogo = listita;
            ponerImagenesBotones();
        }

        private void ponerImagenesBotones()
        {
            this.button1.BackgroundImage = listaImagenesDialogo.ElementAt(0);
            this.button2.BackgroundImage = listaImagenesDialogo.ElementAt(1);
            this.button3.BackgroundImage = listaImagenesDialogo.ElementAt(2);
            this.button4.BackgroundImage = listaImagenesDialogo.ElementAt(3);
            this.button5.BackgroundImage = listaImagenesDialogo.ElementAt(4);
            this.button6.BackgroundImage = listaImagenesDialogo.ElementAt(5);
            this.button7.BackgroundImage = listaImagenesDialogo.ElementAt(6);
            this.button8.BackgroundImage = listaImagenesDialogo.ElementAt(7);
            this.button9.BackgroundImage = listaImagenesDialogo.ElementAt(8);
            this.button10.BackgroundImage = listaImagenesDialogo.ElementAt(9);
            this.button11.BackgroundImage = listaImagenesDialogo.ElementAt(10);
            this.button12.BackgroundImage = listaImagenesDialogo.ElementAt(11);
            this.button13.BackgroundImage = listaImagenesDialogo.ElementAt(12);
            this.button14.BackgroundImage = listaImagenesDialogo.ElementAt(13);
            this.button15.BackgroundImage = listaImagenesDialogo.ElementAt(14);
        }

        private void button_MouseDown(object sender, MouseEventArgs e)
        {
            this.button1.FlatStyle = FlatStyle.Standard;
            this.button2.FlatStyle = FlatStyle.Standard;
            this.button3.FlatStyle = FlatStyle.Standard;
            this.button4.FlatStyle = FlatStyle.Standard;
            this.button5.FlatStyle = FlatStyle.Standard;
            this.button6.FlatStyle = FlatStyle.Standard;
            this.button7.FlatStyle = FlatStyle.Standard;
            this.button8.FlatStyle = FlatStyle.Standard;
            this.button9.FlatStyle = FlatStyle.Standard;
            this.button10.FlatStyle = FlatStyle.Standard;
            this.button11.FlatStyle = FlatStyle.Standard;
            this.button12.FlatStyle = FlatStyle.Standard;
            this.button13.FlatStyle = FlatStyle.Standard;
            this.button14.FlatStyle = FlatStyle.Standard;
            this.button15.FlatStyle = FlatStyle.Standard;
                        
            Button b = (Button)sender;
            b.FlatStyle = FlatStyle.Flat;
            this.imagen = b.BackgroundImage;   
        }

        private void btBorrarImagen_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            this.imagen = b.Image;
        }

        private void btCambiarImagen_Click(object sender, EventArgs e)
        {
            this.rutaImagen = "";
            this.imagen = null;
            Image imagenCambiar;
            OpenFileDialog dialogoAbrir = new OpenFileDialog();

            //dialogoAbrir.Title = "Elegir Imagen";
            dialogoAbrir.Title = rm.GetString("olv11");
            //dialogoAbrir.Filter = "Ficheros imagenes (*.JPG;*.BMP;*.GIF;*.PNG)|*.JPG;*.BMP;*.GIF;*.PNG|Todos (*.*)|*.*";
            dialogoAbrir.Filter = rm.GetString("olv12");
            dialogoAbrir.FilterIndex = 2;
            dialogoAbrir.RestoreDirectory = true;
            if (dialogoAbrir.ShowDialog() == DialogResult.OK)
            {
                rutaImagen = dialogoAbrir.FileName;
                imagenCambiar = Image.FromFile(rutaImagen);
                cambiarImagen(imagenCambiar);
                this.imagen = imagenCambiar;
            }
            
        }

        private void cambiarImagen(Image iman)
        {
            if (button1.FlatStyle == FlatStyle.Flat)
            {
                this.claseIman.borrarImagen(button1.BackgroundImage);
                this.claseIman.añadirImagen(iman);
                
                this.listaImagenesDialogo.Remove(button1.BackgroundImage);
                this.listaImagenesDialogo.Add(iman);
                this.button1.BackgroundImage = iman;
                //this.button1.FlatStyle = FlatStyle.Standard;
            }
            else if (button2.FlatStyle == FlatStyle.Flat)
            {
                this.claseIman.borrarImagen(button2.BackgroundImage);
                this.claseIman.añadirImagen(iman);
                
                this.listaImagenesDialogo.Remove(button2.BackgroundImage);
                this.listaImagenesDialogo.Add(iman);
                this.button2.BackgroundImage = iman;
                //this.button2.FlatStyle = FlatStyle.Standard;
            }
            else if (button3.FlatStyle == FlatStyle.Flat)
            {
                this.claseIman.borrarImagen(button3.BackgroundImage);
                this.claseIman.añadirImagen(iman);
                
                this.listaImagenesDialogo.Remove(button3.BackgroundImage);
                this.listaImagenesDialogo.Add(iman);
                this.button3.BackgroundImage = iman;
                //this.button3.FlatStyle = FlatStyle.Standard;
            }
            else if (button4.FlatStyle == FlatStyle.Flat)
            {
                this.claseIman.borrarImagen(button4.BackgroundImage);
                this.claseIman.añadirImagen(iman);
                
                this.listaImagenesDialogo.Remove(button4.BackgroundImage);
                this.listaImagenesDialogo.Add(iman);
                this.button4.BackgroundImage = iman;
                //this.button4.FlatStyle = FlatStyle.Standard;
            }
            else if (button5.FlatStyle == FlatStyle.Flat)
            {
                this.claseIman.borrarImagen(button5.BackgroundImage);
                this.claseIman.añadirImagen(iman);
                
                this.listaImagenesDialogo.Remove(button5.BackgroundImage);
                this.listaImagenesDialogo.Add(iman);
                this.button5.BackgroundImage = iman;
                //this.button5.FlatStyle = FlatStyle.Standard;
            }
            else if (button6.FlatStyle == FlatStyle.Flat)
            {
                this.claseIman.borrarImagen(button6.BackgroundImage);
                this.claseIman.añadirImagen(iman);
                
                this.listaImagenesDialogo.Remove(button6.BackgroundImage);
                this.listaImagenesDialogo.Add(iman);
                this.button6.BackgroundImage = iman;
                //this.button6.FlatStyle = FlatStyle.Standard;
            }
            else if (button7.FlatStyle == FlatStyle.Flat)
            {
                this.claseIman.borrarImagen(button7.BackgroundImage);
                this.claseIman.añadirImagen(iman);
                
                this.listaImagenesDialogo.Remove(button7.BackgroundImage);
                this.listaImagenesDialogo.Add(iman);
                this.button7.BackgroundImage = iman;
                //this.button7.FlatStyle = FlatStyle.Standard;
            }
            else if (button8.FlatStyle == FlatStyle.Flat)
            {
                this.claseIman.borrarImagen(button8.BackgroundImage);
                this.claseIman.añadirImagen(iman);
                
                this.listaImagenesDialogo.Remove(button8.BackgroundImage);
                this.listaImagenesDialogo.Add(iman);
                this.button8.BackgroundImage = iman;
                //this.button8.FlatStyle = FlatStyle.Standard;
            }
            else if (button9.FlatStyle == FlatStyle.Flat)
            {
                this.claseIman.borrarImagen(button9.BackgroundImage);
                this.claseIman.añadirImagen(iman);
                
                this.listaImagenesDialogo.Remove(button9.BackgroundImage);
                this.listaImagenesDialogo.Add(iman);
                this.button9.BackgroundImage = iman;
                //this.button9.FlatStyle = FlatStyle.Standard;
            }
            else if (button10.FlatStyle == FlatStyle.Flat)
            {
                this.claseIman.borrarImagen(button10.BackgroundImage);
                this.claseIman.añadirImagen(iman);
                
                this.listaImagenesDialogo.Remove(button10.BackgroundImage);
                this.listaImagenesDialogo.Add(iman);
                this.button10.BackgroundImage = iman;
                //this.button10.FlatStyle = FlatStyle.Standard;
            }
            else if (button11.FlatStyle == FlatStyle.Flat)
            {
                this.claseIman.borrarImagen(button11.BackgroundImage);
                this.claseIman.añadirImagen(iman);
                
                this.listaImagenesDialogo.Remove(button11.BackgroundImage);
                this.listaImagenesDialogo.Add(iman);
                this.button11.BackgroundImage = iman;
                //this.button11.FlatStyle = FlatStyle.Standard;
            }
            else if (button12.FlatStyle == FlatStyle.Flat)
            {
                this.claseIman.borrarImagen(button12.BackgroundImage);
                this.claseIman.añadirImagen(iman);
                
                this.listaImagenesDialogo.Remove(button12.BackgroundImage);
                this.listaImagenesDialogo.Add(iman);
                this.button12.BackgroundImage = iman;
                //this.button12.FlatStyle = FlatStyle.Standard;
            }
            else if (button13.FlatStyle == FlatStyle.Flat)
            {
                this.claseIman.borrarImagen(button13.BackgroundImage);
                this.claseIman.añadirImagen(iman);
                
                this.listaImagenesDialogo.Remove(button13.BackgroundImage);
                this.listaImagenesDialogo.Add(iman);
                this.button13.BackgroundImage = iman;
                //this.button13.FlatStyle = FlatStyle.Standard;
            }
            else if (button14.FlatStyle == FlatStyle.Flat)
            {
                this.claseIman.borrarImagen(button14.BackgroundImage);
                this.claseIman.añadirImagen(iman);
                
                this.listaImagenesDialogo.Remove(button14.BackgroundImage);
                this.listaImagenesDialogo.Add(iman);
                this.button14.BackgroundImage = iman;
                //this.button14.FlatStyle = FlatStyle.Standard;
            }
            else if (button15.FlatStyle == FlatStyle.Flat)
            {
                this.claseIman.borrarImagen(button15.BackgroundImage);
                this.claseIman.añadirImagen(iman);
                
                this.listaImagenesDialogo.Remove(button15.BackgroundImage);
                this.listaImagenesDialogo.Add(iman);
                this.button15.BackgroundImage = iman;
                //this.button15.FlatStyle = FlatStyle.Standard;
            }
        }

        private void btGuardar_Click(object sender, EventArgs e)
        {
            String ruta = "";
            SaveFileDialog sfd = new SaveFileDialog();
            //sfd.Filter = "Archivos de plantillas *.fsi|*.fsi";
            sfd.Filter = rm.GetString("olv13");
            sfd.AddExtension = true;
            sfd.DefaultExt = "fsi";
            sfd.InitialDirectory = this.directorioInicio;

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                ruta = sfd.FileName;
                if (this.claseIman != null)
                {
                     Stream fs = File.Create(ruta);
                    //Pasamos claseIman que es una lista de imágnenes a un BinaryWriter para poder serializar
                    using (BinaryWriter writer = new BinaryWriter(fs))
                    {
                        claseIman.Write(writer);
                    }
                    //BinaryFormatter seriador = new BinaryFormatter();
                    //seriador.Serialize(fs, claseIman);
                    fs.Close();
                }
            }
        }

        private void btCargar_Click(object sender, EventArgs e)
        {
            String ruta = "";
            OpenFileDialog ofd = new OpenFileDialog();

            //ofd.Filter = "Archivos de plantillas *.fsi|*.fsi";
            ofd.Filter = rm.GetString("olv13");
            ofd.InitialDirectory = this.directorioInicio;

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                ruta = ofd.FileName;
                if (File.Exists(ruta))
                {
                    Stream fs = File.OpenRead(ruta);
                    /*
                    BinaryFormatter deseriador = new BinaryFormatter();
                    //ClaseImagen nuevoObjeto;
                    claseIman = (ClaseImagen)deseriador.Deserialize(fs);
                    */
                    using (BinaryReader reader = new BinaryReader(fs))
                    {
                        claseIman = ClaseImagen.Read(reader);
                    }
                    fs.Close();


                    listaImagenesDialogo = claseIman.obtenerTodasImagenes();
                    ponerImagenesBotones();
                }
                else
                {
                    //MessageBox.Show("El fichero no existe.");
                    MessageBox.Show(rm.GetString("olv6"));
                }
            }
        }
    }
}
