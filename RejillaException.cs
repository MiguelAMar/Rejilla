/*
 * Miguel Ángel Martínez Jiménez
 * Versión: 2.0.
 * Fecha: Septiembre  2024.
 * Aplicación: Evaluación y Entrenamiento de la Atención.
 * 
 */

using System;

namespace demorejilla
{
    public class RejillaException : Exception
    {
        public RejillaException()
        {

        }

        public RejillaException(string msg)
            : base(msg)
        {

        }

        public RejillaException(String msg, Exception inner)
            : base(msg, inner)
        {

        }
    }
}