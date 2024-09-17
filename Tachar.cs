/*
 * Miguel Ángel Martínez Jiménez
 * Versión: 2.0.
 * Fecha: Septiembre  2024.
 * Aplicación: Evaluación y Entrenamiento de la Atención.
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace demorejilla
{
    internal class Tachar
    {
        public enum Intercambiar
        {
            NADA,
            TEXTO,
            COLOR,
            TEXTOYCOLOR
        }

        public enum TacharBoton
        {
            NADA,
            TEXTO,
            TEXTOYCOLOR,
            COLOR
        }


        public enum Abecedario
        {
            MINUSCULAS,
            MAYUSCULAS,
            ALEATORIO,
            MAYUSMINUS,
            MINUSMAYUS
        }

        public enum Orden
        {
            ASCENDENTE,
            DESCENDENTE
        }
    }
}
