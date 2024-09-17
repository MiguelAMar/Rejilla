/*
 * Miguel Ángel Martínez Jiménez
 * Versión: 2.0.
 * Fecha: Septiembre  2024.
 * Aplicación: Evaluación y Entrenamiento de la Atención.
 * 
 */

using System;
using System.Collections.Generic;
using System.Resources;
using System.Runtime.Versioning;
using System.Windows.Forms;

namespace demorejilla
{
    [SupportedOSPlatform("windows")]
    public partial class Rejilla : Form
    {
        //variables para la rejilla de abecedario
        private struct datosAbecedario
        {
            public string texto;
            public int orden;
            public bool escogido;
        }
        datosAbecedario[] arrayAbecedario;

        private ResourceManager rm;

        //para las parejas
        public List<String> listaAbecedarioTacharParejas;
        private Random rnd;

        //Constructor
        private void rejillaAbecedario(int numFilas, int numColumnas, string textoAbecedario, bool[] opciones, bool ordenTachar)
        {
            rm = new ResourceManager("demorejilla.Recursos", typeof(MetiendoDatos).Assembly);
            this.arrayAbecedario = new datosAbecedario[numFilas * numColumnas];
            //this.next = 0;

            rnd = new Random((int)DateTime.Now.Ticks);
            this.listaAbecedarioTacharParejas = new List<string>();

            string textoCorrecto = "";
            for (int indiceAbecedario = 0; indiceAbecedario < arrayAbecedario.Length; indiceAbecedario++)
            {
                arrayAbecedario[indiceAbecedario].escogido = false;
                arrayAbecedario[indiceAbecedario].orden = indiceAbecedario + 1;
                if (indiceAbecedario == 0)
                {
                    textoCorrecto = procesarOpciones(textoAbecedario, opciones);
                    
                    arrayAbecedario[indiceAbecedario].texto = textoCorrecto;//textoAbecedario;
                    this.listaAbecedarioTacharParejas.Add(textoCorrecto);
                }
                else
                {
                    if (ordenTachar)
                    {
                        textoCorrecto = this.SiguienteSecuencia(arrayAbecedario[indiceAbecedario - 1].texto.ToUpper());
                        if (siguienteLetra(arrayAbecedario[indiceAbecedario - 1].texto, opciones))
                        {
                            //va en mayusculas
                            textoCorrecto = textoCorrecto.ToUpper();
                        }
                        else
                        {
                            //va en minusculas
                            textoCorrecto = textoCorrecto.ToLower();
                        }
                        arrayAbecedario[indiceAbecedario].texto = textoCorrecto;
                    }
                    else
                    {
                        textoCorrecto = this.anteriorSecuencia(arrayAbecedario[indiceAbecedario - 1].texto.ToUpper());
                        if (siguienteLetra(arrayAbecedario[indiceAbecedario - 1].texto, opciones))
                        {
                            //va en mayusculas
                            textoCorrecto = textoCorrecto.ToUpper();
                        }
                        else
                        {
                            //va en minusculas
                            textoCorrecto = textoCorrecto.ToLower();                            
                        }
                        arrayAbecedario[indiceAbecedario].texto = textoCorrecto;
                    }
                    this.listaAbecedarioTacharParejas.Add(arrayAbecedario[indiceAbecedario].texto);
                }
            }
            this.listaControl = new SortedList<String, DatosControlTachado>();
            this.listaOrdenPulsacíon = new List<string>();
        }


        private bool siguienteLetra(string sAnterior, bool[] vector)
        {          
            int i = rnd.Next(0,1000);
            //MessageBox.Show( semilla +   " -> " + i);
            bool letra = false; //si es false va en minusculas sino en mayusculas
            if (vector[0])
                letra = true;
            else if (vector[1])
                letra = false;
            else if (vector[2])
            {
                //aleatorio
                if (i <= 500)
                    letra = false;
                else
                    letra = true;               
            }
            else if (vector[3] || vector[4])
                if (esMayusculas(sAnterior))
                    letra = false;
                else
                    letra = true;

            return letra;
        }

        private String procesarOpciones(string s, bool [] vector)
        {
            string devolver= s;
            int i = rnd.Next(0, 1000);
            if (vector[0])
            {
                devolver = s.ToUpper();
            }
            else if (vector[1])
            {
                devolver = s.ToLower();
            }
            else if (vector[2])
            {
                //aleatorio
                if ( i < 500)
                    devolver = s.ToLower();
                else
                    devolver = s.ToUpper();
            }
            else if (vector[3])
            {
                devolver = s.ToUpper();
            }
            else if (vector[4])
            {
                devolver = s.ToLower();
            }
            return devolver;
        }

        private bool esMayusculas(string s)
        {
            bool devolver = false;
            foreach (char c in s)
                if (c >= 'A' && c <= 'Z')
                    devolver = true;

            return devolver;
        }

        private string SiguienteSecuencia(string inicio)
        {
            char car = inicio[inicio.Length - 1];
            string res;

            if (car < 'Z')
            {
                ++car;
                res = inicio.Substring(0, inicio.Length - 1) + car;
                return res;
            }

            if (inicio.Length == 1) return "AA";

            car = inicio[inicio.Length - 2];
            if (car < 'Z')
            {
                ++car;
                res = inicio.Substring(0, inicio.Length - 2) + car + 'A';
                return res;
            }

            if (inicio.Length == 2) return "AAA";

            car = inicio[inicio.Length - 3];
            if (car < 'Z')
            {
                ++car;
                res = inicio.Substring(0, inicio.Length - 3) + car + "AA";
                return res;
            }
            //return "AAAA";
            //throw new RejillaException("ERROR ABECEDARIO: Se supera el límite ZZZ. Rango de letras válidos [A...ZZZ].");
            throw new RejillaException(rm.GetString("olv16"));
        }

        private string anteriorSecuencia(String inicio)
        {
            char car = inicio[inicio.Length - 1];
            string res;

            if (car > 'A')
            {
                --car;
                res = inicio.Substring(0, inicio.Length - 1) + car;
                return res;
            }

            if (inicio.Length == 1) throw new RejillaException(rm.GetString("olv16"));

            car = inicio[inicio.Length - 2];
            if (car > 'A')
            {
                --car;
                res = inicio.Substring(0, inicio.Length - 2) + car + 'Z';
                return res;
            }

            if (inicio.Length == 2) return "Z";

            car = inicio[inicio.Length - 3];
            if (car > 'A')
            {
                --car;
                res = inicio.Substring(0, inicio.Length - 3) + car + "ZZ";
                return res;
            }

            if (inicio.Length == 3) return "ZZ";
            //return "AAAA";
            //throw new RejillaException("ERROR ABECEDARIO: Se supera el límite ZZZ. Rango de letras válidos [A...ZZZ].");
            throw new RejillaException(rm.GetString("olv16"));
        }
    }
}