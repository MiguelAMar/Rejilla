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
    public partial class Rejilla : Form
    {
        //variables para la rejilla de letras 
        private List<String> listaLetrasRejilla;
        private FontFamily tipoLetra;
        public List<String> listaLetrasTachar;
        public int numLetras;
        public int numLetrasTotal;
        private struct datosLetras
        {
            public String letra;
            public bool escogido;
        }

        datosLetras[] arrayLetras;
        //para las parejas
        public List<String> listaLetrasTacharParejas;

        //constructor
        private void rejillaLetras(int numFilas, int numColumnas, List<String> listaRejilla, List<String> listaUsuario, FontFamily familiaFuente, TantoPorCiento tpc)
        {
            List<String> listaLetrasNoTachar = new List<string>();
            for (int ind = 0; ind < listaRejilla.Count; ind++)
            {
                if (!listaUsuario.Contains(listaRejilla.ElementAt(ind)))
                    listaLetrasNoTachar.Add(listaRejilla.ElementAt(ind));
            }
            this.numLetras = 0;
            this.numLetrasTotal = 0;
            this.listaLetrasTachar = listaUsuario;
            this.listaLetrasRejilla = listaRejilla;
            this.tipoLetra = familiaFuente;
            //crear el array de letras y reyenarlo
            this.arrayLetras = new datosLetras[numFilas * numColumnas];
            Random aleatorioLetras = new Random((int)DateTime.Now.Ticks);

            for (int indiceLetras = 0; indiceLetras < arrayLetras.Length; indiceLetras++)
            {
                arrayLetras[indiceLetras].escogido = false;

                if (listaLetrasNoTachar.Count == 0)
                    tpc = TantoPorCiento.CERO;

                if (tpc == TantoPorCiento.CERO)
                    arrayLetras[indiceLetras].letra = this.listaLetrasRejilla.ElementAt(aleatorioLetras.Next(0, this.listaLetrasRejilla.Count));
                else if (tpc == TantoPorCiento.VEINTICINCO)
                {
                    if (indiceLetras % 4 == 0)
                        arrayLetras[indiceLetras].letra = listaLetrasTachar.ElementAt(aleatorioLetras.Next(0,listaLetrasTachar.Count));
                    else
                        arrayLetras[indiceLetras].letra = listaLetrasNoTachar.ElementAt(aleatorioLetras.Next(0, listaLetrasNoTachar.Count));
                }
                else if (tpc == TantoPorCiento.CINCUENTA)
                {
                    if (indiceLetras % 2 == 0)
                        arrayLetras[indiceLetras].letra = listaLetrasTachar.ElementAt(aleatorioLetras.Next(0,listaLetrasTachar.Count));
                    else
                       arrayLetras[indiceLetras].letra = listaLetrasNoTachar.ElementAt(aleatorioLetras.Next(0, listaLetrasNoTachar.Count)); 
                }
                else if (tpc == TantoPorCiento.SETENTAYCINCO)
                {
                    if (indiceLetras % 4 != 0)
                        arrayLetras[indiceLetras].letra = listaLetrasTachar.ElementAt(aleatorioLetras.Next(0,listaLetrasTachar.Count));
                    else
                        arrayLetras[indiceLetras].letra = listaLetrasNoTachar.ElementAt(aleatorioLetras.Next(0, listaLetrasNoTachar.Count)); 
                }
            }

            this.listaControl = new SortedList<String, DatosControlTachado>();
            this.listaOrdenPulsacíon = new List<string>();

            this.listaLetrasTacharParejas = new List<string>();
        }
    }
}