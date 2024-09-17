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
[SupportedOSPlatform("windows")]
public class CarpetaMisDocumentos
{
    private String path;
    private  static string rutaCarpeta;
    private ResourceManager rm;

    public CarpetaMisDocumentos()
    {
        rm = new ResourceManager("demorejilla.Recursos", typeof(CarpetaMisDocumentos).Assembly);
        path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
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

    public static string ruta()
    {
        return rutaCarpeta;
    }
}