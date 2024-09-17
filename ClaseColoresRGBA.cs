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
    internal class ClaseColoresRGBA
    {
        public int R { get; set; }
        public int G { get; set; }
        public int B { get; set; }
        public int A { get; set; }


        public int ToArgb()
        {
            // Combina los componentes A, R, G, B en un solo entero de 32 bits.
            int argb = (A << 24) | (R << 16) | (G << 8) | B;
            return argb;
        }
    }
}