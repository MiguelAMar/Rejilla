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
        //variables para la rejilla de imagenes
        private List<Image> listaImagenesRejilla;
        public int numImagenes;
        public int numImagenesTotal;
        public List<Image> listaImagenesTachar;
        private List<Image> listaTodasImagenesMenosTachar;
        private struct datosImagenes
        {
            public Image imagen;
            public bool escogido;
        }

        datosImagenes[] arrayImagenes;
        //para las parejas
        public List<Image> listaImagenesTacharParejas;

        //Constructor
        private void rejillaImagenes(int numFilas, int numColumnas, List<Image> listaTacharImagenes, List<Image> listaTodasImagenes, TantoPorCiento cantidadMinimaBotonesImg)
        {
            this.numImagenes = 0;
            this.numImagenesTotal = 0;
            this.listaImagenesTachar = new List<Image>();
            this.listaImagenesTachar = listaTacharImagenes.ToList();
            this.listaTodasImagenesMenosTachar = new List<Image>();
            this.listaTodasImagenesMenosTachar = listaTodasImagenes.ToList();
            eliminarImagenesTachar(this.listaImagenesTachar);
            this.arrayImagenes = new datosImagenes[numFilas * numColumnas];
            Random aleatorioImagenes = new Random((int)DateTime.Now.Ticks);
            this.listaImagenesRejilla = listaTodasImagenes;
            //this.listaImagenesTachar = listaTacharImagenes;
            for (int indiceImagnees = 0; indiceImagnees < arrayImagenes.Length; indiceImagnees++)
            {
                arrayImagenes[indiceImagnees].escogido = false;

                if (cantidadMinimaBotonesImg == TantoPorCiento.CERO)
                {
                    arrayImagenes[indiceImagnees].imagen = this.listaImagenesRejilla.ElementAt(aleatorioImagenes.Next(0, this.listaImagenesRejilla.Count));
                }
                else if (cantidadMinimaBotonesImg == TantoPorCiento.VEINTICINCO)
                {

                    if (indiceImagnees % 4 == 0)
                    {
                        arrayImagenes[indiceImagnees].imagen = this.listaImagenesTachar.ElementAt(aleatorioImagenes.Next(0, this.listaImagenesTachar.Count));
                    }
                    else
                    {
                        arrayImagenes[indiceImagnees].imagen = this.listaTodasImagenesMenosTachar.ElementAt(aleatorioImagenes.Next(0, this.listaTodasImagenesMenosTachar.Count));
                    }
                }
                else if (cantidadMinimaBotonesImg == TantoPorCiento.CINCUENTA)
                {
                    if (indiceImagnees % 2 == 0)
                    {
                        arrayImagenes[indiceImagnees].imagen = this.listaImagenesTachar.ElementAt(aleatorioImagenes.Next(0, this.listaImagenesTachar.Count));
                    }
                    else
                    {
                        arrayImagenes[indiceImagnees].imagen = this.listaTodasImagenesMenosTachar.ElementAt(aleatorioImagenes.Next(0, this.listaTodasImagenesMenosTachar.Count));
                    }
                }
                else if (cantidadMinimaBotonesImg == TantoPorCiento.SETENTAYCINCO)
                {
                    if (indiceImagnees % 4 != 0)
                    {
                        arrayImagenes[indiceImagnees].imagen = this.listaImagenesTachar.ElementAt(aleatorioImagenes.Next(0, this.listaImagenesTachar.Count));
                    }
                    else
                    {
                        arrayImagenes[indiceImagnees].imagen = this.listaTodasImagenesMenosTachar.ElementAt(aleatorioImagenes.Next(0, this.listaTodasImagenesMenosTachar.Count));
                    }
                }
            }
            this.listaImagenesTacharParejas = new List<Image>();
        }
          
        private void eliminarImagenesTachar(List<Image> li)
        {
            for (int indLista = 0; indLista < li.Count; indLista++)
            {
                Image i = li.ElementAt(indLista);
                this.listaTodasImagenesMenosTachar.Remove(i);
            }
        }
    }
}



