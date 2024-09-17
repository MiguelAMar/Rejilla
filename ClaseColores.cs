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
using System.Windows.Forms;
using System.Resources;
using System.Runtime.Versioning;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.IO;



namespace demorejilla
{
    [Serializable]
    [SupportedOSPlatform("windows")]
    public class ClaseColores
    {
        private List<Color> listaColores;
        private ResourceManager rm;

        public ClaseColores()
        {
            rm = new ResourceManager("demorejilla.Recursos", typeof(ClaseColores).Assembly);
            listaColores = new List<Color>();
        }

        //2024 - Para cuando se recupera de la BD, se cargue la lista que se ha guardado en la BD.
        public ClaseColores(List<Color> listColors)
        {
            listaColores = new List<Color>();
            listaColores = listColors;
        }

        private void rellenarColores()
        {
            this.listaColores.Clear();

            listaColores.Add(Color.Aqua);
            listaColores.Add(Color.Yellow);
            listaColores.Add(Color.Orange);
            listaColores.Add(Color.Red);
            listaColores.Add(Color.Brown);
            listaColores.Add(Color.White);
            listaColores.Add(Color.Blue);
            listaColores.Add(Color.Green);
            listaColores.Add(Color.Chocolate);
            listaColores.Add(Color.Pink);
            listaColores.Add(Color.Gold);
            listaColores.Add(Color.Gray);
            listaColores.Add(Color.GreenYellow);
            listaColores.Add(Color.Indigo);
            listaColores.Add(Color.Lime);
        }

        public void plantillaInicial()
        {
            this.listaColores.Clear();
            rellenarColores();
        }

        public List<Color> obtenerTodosColores()
        {
            List<Color> lista = new List<Color>();
            foreach (Color c in this.listaColores)
                lista.Add(c);

            return lista;
        }

        public void añadirColor(Color c)
        {
            this.listaColores.Add(c);
        }

        public void borrarColor(Color c)
        {
            this.listaColores.Remove(c);
        }

        public void limpiarColores()
        {
            this.listaColores.Clear();
        }

        public bool esVacia()
        {
            return this.listaColores.Count == 0;
        }

        public bool contieneColor(Color c)
        {
            return this.listaColores.Contains(c);
        }

        public Color obtenerColor(int posicion)
        {
            if (posicion < 0 || posicion >= this.listaColores.Count)
                throw new RejillaException("ERROR OBTENENIENDO COLOR.");
            else
                return this.listaColores.ElementAt(posicion);
        }

        public int cantidadColores()
        {
            return this.listaColores.Count;
        }

        //2024. Para serializar como JSON
        public void GuardarEnArchivo(string filePath)
        {
            var options = new JsonSerializerOptions
            {
                Converters = { new ColorConverter() },
                WriteIndented = true
            };
            var json = JsonSerializer.Serialize(this.listaColores, options);
            File.WriteAllText(filePath, json);
        }

        public void CargarDesdeArchivo(string filePath)
        {
            var options = new JsonSerializerOptions
            {
                Converters = { new ColorConverter() }
            };
            var json = File.ReadAllText(filePath);
            this.listaColores = JsonSerializer.Deserialize<List<Color>>(json, options);
        }

        private class ColorConverter : JsonConverter<Color>
        {
            public override Color Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return ColorTranslator.FromHtml(reader.GetString());
            }

            public override void Write(Utf8JsonWriter writer, Color value, JsonSerializerOptions options)
            {
                writer.WriteStringValue(ColorTranslator.ToHtml(value));
            }
        }
    }
}