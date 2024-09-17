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
using System.Runtime.Versioning;
using System.Text;
using System.Windows.Forms;

namespace demorejilla
{
    [SupportedOSPlatform("windows")]
    public class CBotonEmparejar : Button
    {
        private DateTime tiempo;

        public CBotonEmparejar(): base()
        {
            tiempo = new DateTime();
        }

        public void capturarTiempo()
        {
            tiempo = DateTime.Now;
        }

        public DateTime devolverTiempo()
        {
            return tiempo;
        }
    }
}
