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
using System.Windows.Forms;

namespace demorejilla
{
    [SupportedOSPlatform("windows")]
    public partial class Rejilla : Form
    {
        public List<TipoLetraFuente> listaCompleta;
        public List<TipoLetraFuente> listaElementosBuscar;
        public List<TipoLetraFuente> listaDiferenciaWing;
        public int numWings;
        public int numWingstotal;

        protected struct datosWingdings
        {
            public TipoLetraFuente objeto;
            public bool escogido;
        }

        datosWingdings[] arrayWingdings;
        //para las parejas tachar
        public List<TipoLetraFuente> listaWindingsTacharParejas;

        //Constructor
        private void rejillaWingdings(int numFilas, int numColumnas, List<TipoLetraFuente> lCompleta, List<TipoLetraFuente> lElegidos, TantoPorCiento tpc)
        {
            this.numWings = 0;
            this.numWingstotal = 0;
            this.listaCompleta = lCompleta;
            this.listaElementosBuscar = lElegidos;
            this.listaDiferenciaWing = new List<TipoLetraFuente>();

            this.arrayWingdings = new datosWingdings[numFilas * numColumnas];
            Random rnd = new Random();
            int mesias;
            for (int indCompleta = 0; indCompleta < this.listaCompleta.Count; indCompleta++)
            {
                if (!this.listaElementosBuscar.Contains(this.listaCompleta.ElementAt(indCompleta)))
                {
                    this.listaDiferenciaWing.Add(this.listaCompleta.ElementAt(indCompleta));
                }
            }

            for (int indiceWings = 0; indiceWings < arrayWingdings.Length; indiceWings++)
            {
                arrayWingdings[indiceWings].escogido = false;
                if (this.listaDiferenciaWing.Count == 0)
                    tpc = TantoPorCiento.CERO;
                
                if (tpc == TantoPorCiento.CERO)
                {
                    mesias = rnd.Next(0, this.listaCompleta.Count);
                    arrayWingdings[indiceWings].objeto = this.listaCompleta.ElementAt(mesias);
                }
                else if (tpc == TantoPorCiento.VEINTICINCO)
                {
                    if (indiceWings % 4 == 0)
                    {
                        mesias = rnd.Next(0, this.listaElementosBuscar.Count);
                        arrayWingdings[indiceWings].objeto = this.listaElementosBuscar.ElementAt(mesias);
                    }
                    else
                    {
                        mesias = rnd.Next(0, listaDiferenciaWing.Count);
                        arrayWingdings[indiceWings].objeto = this.listaDiferenciaWing.ElementAt(mesias);
                    }
                }
                else if (tpc == TantoPorCiento.CINCUENTA)
                {
                    if (indiceWings % 2 == 0)
                    {
                        mesias = rnd.Next(0, this.listaElementosBuscar.Count);
                        arrayWingdings[indiceWings].objeto = this.listaElementosBuscar.ElementAt(mesias); 
                    }
                    else
                    {
                        mesias = rnd.Next(0, listaDiferenciaWing.Count);
                        arrayWingdings[indiceWings].objeto = this.listaDiferenciaWing.ElementAt(mesias);
                    }
                }
                else if (tpc == TantoPorCiento.SETENTAYCINCO)
                {
                    if (indiceWings %4 != 0)
                    {
                        mesias = rnd.Next(0, this.listaElementosBuscar.Count);
                        arrayWingdings[indiceWings].objeto = this.listaElementosBuscar.ElementAt(mesias);
                    }
                    else
                    {
                        mesias = rnd.Next(0, listaDiferenciaWing.Count);
                        arrayWingdings[indiceWings].objeto = this.listaDiferenciaWing.ElementAt(mesias);
                    }
                }
            }

            this.listaWindingsTacharParejas = new List<TipoLetraFuente>();
        }
    }
}
