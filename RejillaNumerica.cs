/*
 * Miguel Ángel Martínez Jiménez
 * Versión: 2.0.
 * Fecha: Septiembre  2024.
 * Aplicación: Evaluación y Entrenamiento de la Atención.
 * 
 */

using System;
using System.Collections.Generic;
using System.Runtime.Versioning;
using System.Windows.Forms;

namespace demorejilla
{
    [SupportedOSPlatform("windows")]
    public partial class Rejilla : Form
    {
        /*  - next: guarda el valor del boton que toca tachar
         *  - incrementandoNext: guarda el valor con el que actualizamos next
         *                       cada vez que tachamos un boton, por si los botones no van de uno en uno
         *                       , si van de 5 en 5 guardara un 5.
         *  - struct datos : estructura con dos campos, int y boolean.
         *  - matrizBotones: matriz de struct datos que contiene todos los
         *                   botones que conforman la rejilla, la usamos 
         *                   para crear la rejilla aleatoriamente la primera
         *                   vez, y que no salga siempre la misma.
         */

        public int next;
        public int incrementandoNext;

        private struct datos
        {
            public int valor;
            public bool escogido;
        }

        datos[] matrizBotones;
        public bool ordenParaTachar;
        
        //Constructor para imprimir la rejilla
        private void rejillaNumerica(int numFilas, int numColumnas, int comienzoRejilla, int siguienteTachar)
        {
            matrizBotones = new datos[numColumnas * numFilas];
            this.next = comienzoRejilla;
            incrementandoNext = siguienteTachar;
            int siguienteValor = comienzoRejilla;
            for (int indice = 0; indice < numColumnas * numFilas; indice++)
            {
                matrizTiempos[indice] = new DateTime(0);

                matrizBotones[indice].escogido = false;
                matrizBotones[indice].valor = siguienteValor;
                siguienteValor += incrementandoNext;
            }
        }

        //Constructor
        private void rejillaNumerica(int numFilas, int numColumnas, int comienzoRejilla, int siguienteTachar, bool descendente)
        {
            matrizBotones = new datos[numColumnas * numFilas];
            ordenParaTachar = descendente;

            if (!descendente) //Ascendente
                this.next = comienzoRejilla;
            else//Orden de tachado descendente
            {
                int uB = comienzoRejilla + (numFilas * numColumnas - 1) * siguienteTachar;
                this.next = uB;
            }

            incrementandoNext = siguienteTachar;
            int siguienteValor = comienzoRejilla;
            for (int indice = 0; indice < numColumnas * numFilas; indice++)
            {
                matrizTiempos[indice] = new DateTime(0);

                matrizBotones[indice].escogido = false;
                matrizBotones[indice].valor = siguienteValor;
                siguienteValor += incrementandoNext;
            }

            this.listaControl = new SortedList<String, DatosControlTachado>();
            this.listaOrdenPulsacíon = new List<string>();
        }
    }
}