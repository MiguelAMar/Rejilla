/*
 * Miguel Ángel Martínez Jiménez
 * Versión: 2.0.
 * Fecha: Septiembre  2024.
 * Aplicación: Evaluación y Entrenamiento de la Atención.
 * 
 */

using System;
using System.Drawing;
using System.Runtime.Versioning;
using System.Threading;
using System.Timers;
using System.Windows.Forms;

namespace demorejilla
{
    [SupportedOSPlatform("windows")]
    public partial class Rejilla : Form
    {
        //Variables para la distracción línea.
        delegate void lineaMolestaRellamada(PictureBox panelDibujo);
        private System.Timers.Timer temporizadorLineaMolesta;
        private Thread hebraLineaMolesta = null;
        private PictureBox lineaMolestaH;
        private tipoLinea tipoDeLinea;
        private bool hayLinea;
        private Color colorLinea;
        private int grosorLinea;
        private int valorX, valorY;
        private int movPosX, movPosY;
        private ManualResetEvent controladorPararLineaMolesta;
        private ManualResetEvent controladorLineaMolestaParada;
        private MovimientoLinea movimientoLineaAleatoria;

        //Método que mueve la lína por pantalla.
        private void lineaMolesta(PictureBox p)
        {
            if (this.lineaMolestaH != null)
            {
                if (lineaMolestaH.InvokeRequired)
                {
                    lineaMolestaRellamada lmr = new lineaMolestaRellamada(lineaMolesta);
                    this.lineaMolestaH.Invoke(lmr, new object[] { p });
                }
                else
                {
                    if (this.tipoDeLinea == tipoLinea.VERTICAL)
                    {
                        valorX += movPosX;
                        if (valorX + lineaMolestaH.Width >= this.FindForm().Width)
                        {
                            movPosX = -movPosX;
                        }
                        else if (valorX == 0)
                        {
                            movPosX = -movPosX;
                        }
                        this.lineaMolestaH.Location = new System.Drawing.Point(valorX, valorY);
                    }
                    if (this.tipoDeLinea == tipoLinea.HORIZONTAL)
                    {
                        valorY += movPosY;
                        if (valorY + lineaMolestaH.Height >= this.FindForm().Height)
                        {
                            movPosY = -movPosY;
                        }
                        else if (valorY == 0)
                        {
                            movPosY = -movPosY;
                        }
                        this.lineaMolestaH.Location = new System.Drawing.Point(valorX, valorY);
                    }
                    if (this.tipoDeLinea == tipoLinea.ALEATORIA)
                    {
                        if (this.movimientoLineaAleatoria == MovimientoLinea.ABAJO)
                        {
                            valorY += movPosY;
                            if (valorY + this.lineaMolestaH.Height >= this.FindForm().Height)
                            {
                                redibujarLinea();
                            }
                        }
                        else if (this.movimientoLineaAleatoria == MovimientoLinea.ARRIBA)
                        {
                            valorY += movPosY;
                            if (valorY <= 0)
                            {
                                redibujarLinea();
                            }
                        }
                        else if (this.movimientoLineaAleatoria == MovimientoLinea.DERECHA)
                        {
                            valorX += movPosX;
                            if (this.lineaMolestaH.Width + valorX >= this.FindForm().Width)
                            {
                                redibujarLinea();
                            }
                        }
                        else if (this.movimientoLineaAleatoria == MovimientoLinea.IZQUIERDA)
                        {
                            valorX += movPosX;
                            if (valorX <= 0)
                            {
                                redibujarLinea();
                            }
                        }
                        //desplazamos la linea a su nueva posicion
                        this.lineaMolestaH.Location = new System.Drawing.Point(valorX, valorY);
                    }
                }
            }
        }

        //Constructor
        private void distraccionLinea(tipoLinea line, Color colors, int gorda, double velocidad)
        {
            tipoDeLinea = line;
            if (colors != Color.Empty)
            {
                tipoDeLinea = line;
                this.colorLinea = colors;
                this.grosorLinea = gorda;
                temporizadorLineaMolesta = new System.Timers.Timer();
                temporizadorLineaMolesta.Elapsed += new ElapsedEventHandler(temporizadorLineaMolesta_Elapsed);
                temporizadorLineaMolesta.Interval = velocidad;
                temporizadorLineaMolesta.Enabled = true; ;
                valorX = 0;
                valorY = 0;
               
                controladorLineaMolestaParada = new ManualResetEvent(false);
                controladorPararLineaMolesta = new ManualResetEvent(false);
               
                movPosX = 10;
                movPosY = 10;
            }
        }

        private void llamadaSeguraHebraLineaMolesta()
        {
            /*
               * Ver si se me pide que pare la hebra, con 
               * if controladorPararHiloSEcundario.WaitOne(0,true)
               * operaciones de limpieza y un break;
               */
            if (controladorPararLineaMolesta.WaitOne(0, true))
            {
                //tareas de limpieza
                controladorLineaMolestaParada.Set();
            }
            else
                this.lineaMolesta(this.lineaMolestaH);
        }
        
        protected void temporizadorLineaMolesta_Elapsed(object source, ElapsedEventArgs e)
        {
            this.hebraLineaMolesta = new Thread(new ThreadStart(this.llamadaSeguraHebraLineaMolesta));
            this.hebraLineaMolesta.Start();
        }

        private bool PararHiloLineaMolesta()
        {
            //Funcion usada por el hilo principal para detener la ejecucion del hilo secundario
            if (hebraLineaMolesta == null || !hebraLineaMolesta.IsAlive)
                return false;
            //cambiar el estado de este controlador de espera a señalizado para informar
            // al hebraLineaMolesta que debe parar
            controladorPararLineaMolesta.Set();

            //esperar a que el hilo secundario informe de que ha parado
            while (hebraLineaMolesta.IsAlive)
            {

                if (WaitHandle.WaitAll((new ManualResetEvent[] { controladorLineaMolestaParada }), 100, false))
                {
                    //MessageBox.Show("Parando la hebra de la linea");
                    break;
                }
                Application.DoEvents();
            }
            hebraLineaMolesta = null;
            return true;
        }

        /*
         * Procedimiento que dibuja la linea en la pantalla de forma aleatoria
         */
        private void redibujarLinea()
        {
            Random rdn = new Random();
            switch (rdn.Next(0, 4))
            {
                //vertical movimiento a la derecha
                case 0: lineaMolestaH.Size = new Size(grosorLinea, this.FindForm().Height);
                    this.lineaMolestaH.Location = new System.Drawing.Point(0, 0);
                    this.movimientoLineaAleatoria = MovimientoLinea.DERECHA;
                    this.valorX = 0;
                    this.valorY = 0;
                    this.movPosX = Math.Abs(movPosX);
                    break;
                //horizontal movimiento hacia abajo
                case 1: lineaMolestaH.Size = new Size(this.FindForm().Width, grosorLinea);
                    this.lineaMolestaH.Location = new System.Drawing.Point(0, 0);
                    this.movimientoLineaAleatoria = MovimientoLinea.ABAJO;
                    this.valorY = 0;
                    this.valorX = 0;
                    this.movPosY = Math.Abs(movPosY);
                    break;
                //vertical movimiento hacia la izquierda
                case 2: lineaMolestaH.Size = new Size(grosorLinea, this.FindForm().Height);
                    this.lineaMolestaH.Location = new System.Drawing.Point(this.FindForm().Width - this.lineaMolestaH.Width, 0);
                    this.movimientoLineaAleatoria = MovimientoLinea.IZQUIERDA;
                    this.valorX = this.FindForm().Width;
                    this.valorY = 0;
                    this.movPosX = -Math.Abs(movPosX);
                    break;
                //horizontal movimiento hacia arriba
                case 3: lineaMolestaH.Size = new Size(this.FindForm().Width, grosorLinea);
                    this.lineaMolestaH.Location = new System.Drawing.Point(0, this.FindForm().Height - this.lineaMolestaH.Height);
                    this.movimientoLineaAleatoria = MovimientoLinea.ARRIBA;
                    this.valorX = 0;
                    this.valorY = this.FindForm().Height;
                    this.movPosY = -Math.Abs(movPosY);
                    break;
            }
        }
    }
}