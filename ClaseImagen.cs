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
using System.Runtime.Versioning;

[Serializable]
[SupportedOSPlatform("windows")]

public class ClaseImagen
{
    private List<Image> lista;

    public ClaseImagen()
    {
        lista = new List<Image>();
    }


    public ClaseImagen(List<Image> listaPasada)
    {
        this.lista = listaPasada;
    }
    private void rellenarLista()
    {
        this.lista.Add(global::demorejilla.Properties.Resources.Img01);
        this.lista.Add(global::demorejilla.Properties.Resources.Img02);
        this.lista.Add(global::demorejilla.Properties.Resources.Img03);
        this.lista.Add(global::demorejilla.Properties.Resources.Img04);
        this.lista.Add(global::demorejilla.Properties.Resources.Img05);
        this.lista.Add(global::demorejilla.Properties.Resources.Img06);
        this.lista.Add(global::demorejilla.Properties.Resources.Img07);
        this.lista.Add(global::demorejilla.Properties.Resources.Img08);
        this.lista.Add(global::demorejilla.Properties.Resources.Img09);
        this.lista.Add(global::demorejilla.Properties.Resources.Img10);
        this.lista.Add(global::demorejilla.Properties.Resources.Img11);
        this.lista.Add(global::demorejilla.Properties.Resources.Img12);
        this.lista.Add(global::demorejilla.Properties.Resources.Img13);
        this.lista.Add(global::demorejilla.Properties.Resources.Img14);
        this.lista.Add(global::demorejilla.Properties.Resources.Img15);
    }

    public bool estaImagen(Image iman)
    {
        return this.lista.Contains(iman);
    }

    public int cantidadImagenes()
    {
        return this.lista.Count;
    }

    public void añadirImagen(Image iman)
    {
        this.lista.Add(iman);
    }

    public void borrarImagen(Image iman)
    {
        this.lista.Remove(iman);
    }

    public void limpiar()
    {
        this.lista.Clear();
    }

    public void plantillaInicial()
    {
        this.lista.Clear();
        rellenarLista();
    }

    public List<Image> obtenerTodasImagenes()
    {
        List<Image> li = new List<Image>();
        foreach (Image img in this.lista)
            li.Add(img);

        return li;
    }
    // Para poder serializar la clase imagen, no se puede usar BinaryFormatter esta obsoleto. 
    // Método para escribir los datos en un BinaryWriter
    public void Write(BinaryWriter writer)
    {
        writer.Write(this.lista.Count);
        foreach (var imagen in this.lista)
        {
            using (var ms = new MemoryStream())
            {
                imagen.Save(ms, imagen.RawFormat);
                byte[] imageBytes = ms.ToArray();
                writer.Write(imageBytes.Length);
                writer.Write(imageBytes);
            }
        }
    }

    // Método para leer los datos desde un BinaryReader
    public static ClaseImagen Read(BinaryReader reader)
    {
        var claseImagen = new ClaseImagen();
        int count = reader.ReadInt32();
        for (int i = 0; i < count; i++)
        {
            int length = reader.ReadInt32();
            byte[] imageBytes = reader.ReadBytes(length);
            using (var ms = new MemoryStream(imageBytes))
            {
                Image imagen = Image.FromStream(ms);
                claseImagen.añadirImagen(imagen);
            }
        }
        return claseImagen;

    }
}