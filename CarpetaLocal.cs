/*
 * Miguel Ángel Martínez Jiménez
 * Versión: 2.0.
 * Fecha: Septiembre  2024.
 * Aplicación: Evaluación y Entrenamiento de la Atención.
 * 
 */

using System.IO;
using System;
using System.Windows.Forms;
using System.Resources;
using System.Runtime.Versioning;



namespace demorejilla
{
    [SupportedOSPlatform("windows")]
    internal class CarpetaLocal
    {
        private String path;
        private static string rutaCarpeta;
        private ResourceManager rm;


        public CarpetaLocal()
        {
            rm = new ResourceManager("demorejilla.Recursos", typeof(CarpetaMisDocumentos).Assembly);
            //path = AppDomain.CurrentDomain.BaseDirectory;
            path = Application.StartupPath;
        }
        public string carpetaBD()
        {
            rm = new ResourceManager("demorejilla.Recursos", typeof(CarpetaMisDocumentos).Assembly);
            path = path + "BD" + Path.DirectorySeparatorChar;
            return path;
        }

        public string carpetaDocumentos()
        {
            rm = new ResourceManager("demorejilla.Recursos", typeof(CarpetaMisDocumentos).Assembly);
            path = path + "Documentos" + Path.DirectorySeparatorChar; 
            return path;
        }

        public string carpetaSonidos()
        {
            rm = new ResourceManager("demorejilla.Recursos", typeof(CarpetaMisDocumentos).Assembly);
            path = path + "Sonidos" + Path.DirectorySeparatorChar;
            return path;
        }


        public string carpetaMusica()
        {
            rm = new ResourceManager("demorejilla.Recursos", typeof(CarpetaMisDocumentos).Assembly);
            path = path + "Musica" + Path.DirectorySeparatorChar;
            return path;
        }


        public string carpetaBackUp()
        {
            rm = new ResourceManager("demorejilla.Recursos", typeof(CarpetaMisDocumentos).Assembly);
            path = path + "BackUp" + Path.DirectorySeparatorChar;
            return path;
        }

        public string crearCarpeta(String nombre)
        {
            try
            {
                new DirectoryInfo(path.ToString() + Path.DirectorySeparatorChar + nombre).Create();
                DirectoryInfo d = new DirectoryInfo(path.ToString() + Path.DirectorySeparatorChar + nombre.ToString());
                rutaCarpeta = d.FullName;
                return d.FullName;
            }
            catch (IOException ie)
            {
                //MessageBox.Show("Error No se puede crear una carpeta en mis documentos.");
                MessageBox.Show(rm.GetString("olv07"));
            }
            return "";
        }

        public static string rutaDirectorio()
        {
            return rutaCarpeta;
        }

        public string nombreFicheroBackUp()
        {
            return this.carpetaBackUp() + "rejilla_" + DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss") + "_backup";
        }

    }
}
