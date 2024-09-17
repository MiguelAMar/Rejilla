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
        //variables para la rejilla de colores
        private List<Color> listaColoresDisponibles;
        public List<Color> listaColoresTachar;
        public int numColores; //indica cuantos colores llevo tachados
        public int numColoresTotal; //indica la cantidad total de colores a tachar
        private TantoPorCiento tantoPorCientoBotonesColores;
        private List<Color> listaColoresDisponiblesMenosTachar;
        private struct datosColor
        {
            public Color color;
            public bool escogido;
        }

        datosColor[] arrayColores;
        private ClaseColores todosLosColores;
        //lista para las parejitas
        public List<Color> listaColoresTacharParejas;

        //Constructor
        private void rejillaColores(int numFilas, int numColumnas, ClaseColores cc, List<Color> coloresBotones, TantoPorCiento cantidadMinimaBotones)
        {
            this.todosLosColores = cc;
            this.numColores = 0;
            this.numColoresTotal = 0;
            this.listaColoresTachar = coloresBotones;
            this.tantoPorCientoBotonesColores = cantidadMinimaBotones;
            inicializarListaColoresDisponibles();
            this.listaColoresDisponiblesMenosTachar = new List<Color>();
  
            this.listaColoresDisponiblesMenosTachar = this.listaColoresDisponibles.ToList();
            eliminarColoresTachar(this.listaColoresTachar);
            Random aleatorioColores = new Random((int)DateTime.Now.Ticks);

            //Crear el array de colores y reyenarlo
            this.arrayColores = new datosColor[numColumnas * numFilas];
            for (int indiceArrayColores = 0; indiceArrayColores < arrayColores.Length; indiceArrayColores++)
            {
                arrayColores[indiceArrayColores].escogido = false;
                if (this.listaColoresDisponiblesMenosTachar.Count == 0)
                    this.tantoPorCientoBotonesColores = TantoPorCiento.CERO;

                if (this.tantoPorCientoBotonesColores == TantoPorCiento.CERO)
                {
                    arrayColores[indiceArrayColores].color = this.listaColoresDisponibles.ElementAt(aleatorioColores.Next(0, listaColoresDisponibles.Count));
                }
                else if (this.tantoPorCientoBotonesColores == TantoPorCiento.VEINTICINCO)
                {
                    if (indiceArrayColores % 4 == 0)
                        arrayColores[indiceArrayColores].color = this.listaColoresTachar.ElementAt(aleatorioColores.Next(0, listaColoresTachar.Count));
                    else
                        arrayColores[indiceArrayColores].color = this.listaColoresDisponiblesMenosTachar.ElementAt(aleatorioColores.Next(0, this.listaColoresDisponiblesMenosTachar.Count));
                }
                else if (this.tantoPorCientoBotonesColores == TantoPorCiento.CINCUENTA)
                {
                    if (indiceArrayColores % 2 == 0)
                        arrayColores[indiceArrayColores].color = this.listaColoresTachar.ElementAt(aleatorioColores.Next(0, listaColoresTachar.Count));
                    else
                        arrayColores[indiceArrayColores].color = this.listaColoresDisponiblesMenosTachar.ElementAt(aleatorioColores.Next(0, this.listaColoresDisponiblesMenosTachar.Count));
                }
                else if (this.tantoPorCientoBotonesColores == TantoPorCiento.SETENTAYCINCO)
                {
                    if (indiceArrayColores % 4 != 0)
                        arrayColores[indiceArrayColores].color = this.listaColoresTachar.ElementAt(aleatorioColores.Next(0, listaColoresTachar.Count));
                    else
                        arrayColores[indiceArrayColores].color = this.listaColoresDisponiblesMenosTachar.ElementAt(aleatorioColores.Next(0, this.listaColoresDisponiblesMenosTachar.Count));
                }
            }
            this.listaColoresTacharParejas = new List<Color>();
        }

        private void inicializarListaColoresDisponibles()
        {
            this.listaColoresDisponibles = new List<Color>();
            this.listaColoresDisponibles = this.todosLosColores.obtenerTodosColores();
        }

        private void eliminarColoresTachar(List<Color> lc)
        {
            for (int indLista = 0; indLista < lc.Count; indLista++)
            {
                Color c = lc.ElementAt(indLista);
                this.listaColoresDisponiblesMenosTachar.Remove(c);
            }
        }
    }
}