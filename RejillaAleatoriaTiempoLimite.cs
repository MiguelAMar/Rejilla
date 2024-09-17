/*
 * Miguel Ángel Martínez Jiménez
 * Versión: 2.0.
 * Fecha: Septiembre  2024.
 * Aplicación: Evaluación y Entrenamiento de la Atención.
 * 
 */

using System;
using System.Drawing;
using System.Linq;
using System.Runtime.Versioning;
using System.Threading;
using System.Timers;
using System.Windows.Forms;

namespace demorejilla
{
    [SupportedOSPlatform("windows")]
    public partial class Rejilla : Form
    {
        //Varibles para aleatorizar la rejilla
        private delegate void aleatorioBotonesRellamada(TableLayoutPanel rejilla);
        private System.Timers.Timer temporizadorAleatorio;
        private Thread threadAleatorio = null;
        private bool[,] matrizBotonesEscogidos;
        private ManualResetEvent controladorPararTemporizadorAleatorio;
        private ManualResetEvent controladorTemporizadorAleatorioParado;

        //Variables del tiempo Límite
        private System.Timers.Timer temporizadorTiempoLimite;
        private Thread hebraTemporizadorTiempoLimite;
        private delegate void cerrarRejilla(Form formulario);

        //Variables para las parejas de botones
        private System.Timers.Timer temporizadorBotonParejas;
        private Thread hebraTemporiadorParejas = null;
        private delegate void aleatorioBotonParejas(CBotonEmparejar bEmparejar);
        private ManualResetEvent controladorPararBotonesPareja;
        private ManualResetEvent controladorBotonesParejaParado;

        //Método que intercambia los botones de posición.
        private void aleatorizarBotones(int numFilas, int numColumnas, int tiempoAleatorio)
        {
            matrizBotonesEscogidos = new bool[numFilas, numColumnas];
            temporizadorAleatorio = new System.Timers.Timer();
            temporizadorAleatorio.Elapsed += new System.Timers.ElapsedEventHandler(temporizadorAleatorio_Elapsed);
            temporizadorAleatorio.Interval = tiempoAleatorio * 1000;
            temporizadorAleatorio.Enabled = true;
            controladorPararTemporizadorAleatorio = new ManualResetEvent(false);
            controladorTemporizadorAleatorioParado = new ManualResetEvent(false);
        }

        private void aleatorioBotones(TableLayoutPanel tablita)
        {
            if (tabla.InvokeRequired)
            {
                aleatorioBotonesRellamada pilaLlamada = new aleatorioBotonesRellamada(aleatorioBotones);
                tabla.Invoke(pilaLlamada, new object[] { tablita });
            }
            else
            {
                for (int filaI = 0; filaI < tabla.RowCount; filaI++)
                {
                    for (int columnaJ = 0; columnaJ < tabla.ColumnCount; columnaJ++)
                    {
                        matrizBotonesEscogidos[filaI, columnaJ] = false;
                    }
                }

                Random posicion = new Random();
                int x1, x2, y1, y2;
                x1 = 0;
                x2 = 0;
                y1 = 0;
                y2 = 0;
                //un for de filas*columnas / 2 para intercambiar los botones
                for (int numeroVeces = 0; numeroVeces < (tabla.RowCount * tabla.ColumnCount) / 2; numeroVeces++)
                {
                    //cogemos dos botones que no hayan sido intercambiados y los intercambiamos
                    do
                    {
                        //Cojemos una fila/columna de la tabla y escogemos un boton[fila,columna]
                        x1 = posicion.Next(0, tabla.RowCount);
                        y1 = posicion.Next(0, tabla.ColumnCount);

                        //si ya lo he intercambiado eligo otro
                        while (matrizBotonesEscogidos[x1, y1])
                        {
                            x1 = posicion.Next(0, tabla.RowCount);
                            y1 = posicion.Next(0, tabla.ColumnCount);
                        }

                        //elegimos una segunda posicion de otro boton para intercambiarlos
                        x2 = posicion.Next(0, tabla.RowCount);
                        y2 = posicion.Next(0, tabla.ColumnCount);

                        //si la lo hemos intercambiado o es el mismo que el primero cojo otro
                        while (matrizBotonesEscogidos[x2, y2] || (x1 == x2 && y1 == y2))
                        {
                            x2 = posicion.Next(0, tabla.RowCount);
                            y2 = posicion.Next(0, tabla.ColumnCount);
                        }

                        //una vez que ya tenemos dos botones, los marcamos en la matrizBotonesEscogidos

                        matrizBotonesEscogidos[x2, y2] = true;
                        matrizBotonesEscogidos[x1, y1] = true;

                    } while (!matrizBotonesEscogidos[x1, y1] && !matrizBotonesEscogidos[x2, y2]);

                    //Obtenemos los botones de las posiciones elegidas y los intercambiamos
                    // Las x's son las filas y las y's las columnas
                    //GetControl.... se le pasa la Columna y luego la fila
                    CMiBoton bt3 = (CMiBoton)tabla.GetControlFromPosition(y1, x1);
                    CMiBoton bt4 = (CMiBoton)tabla.GetControlFromPosition(y2, x2);

                    if (this.rejilla == TipoRejilla.NUMERICA || this.rejilla == TipoRejilla.ABECEDARIO)
                    {
                        CMiBoton bt5 = new CMiBoton(0);
                       
                        if (this.programarColoresTachado[0] == true)
                        {
                            //intercambio solo el texto
                            bt5.Text = bt3.Text;
                            bt3.Text = bt4.Text;
                            bt4.Text = bt5.Text;

                            bt5.valor = bt3.valor;
                            bt3.valor = bt4.valor;
                            bt4.valor = bt5.valor;

                            bt5.valorUltimoBoton = bt3.valorUltimoBoton;
                            bt3.valorUltimoBoton = bt4.valorUltimoBoton;
                            bt4.valorUltimoBoton = bt5.valorUltimoBoton;

                            bt5.cambiarEstado(bt3.estadoBoton());
                            bt3.cambiarEstado(bt4.estadoBoton());
                            bt4.cambiarEstado(bt5.estadoBoton());
                        }
                        else if (this.programarColoresTachado[1] == true)
                        {
                            //intercambio solo el color de fondo
                            bt5.BackColor = bt3.BackColor;
                            bt3.BackColor = bt4.BackColor;
                            bt4.BackColor = bt5.BackColor;
                        }
                        else if (this.programarColoresTachado[2] == true)
                        {
                            //intercambio todo el texto y el color
                            bt5.Text = bt3.Text;
                            bt3.Text = bt4.Text;
                            bt4.Text = bt5.Text;

                            bt5.valor = bt3.valor;
                            bt3.valor = bt4.valor;
                            bt4.valor = bt5.valor;

                            bt5.valorUltimoBoton = bt3.valorUltimoBoton;
                            bt3.valorUltimoBoton = bt4.valorUltimoBoton;
                            bt4.valorUltimoBoton = bt5.valorUltimoBoton;

                            bt5.BackColor = bt3.BackColor;
                            bt3.BackColor = bt4.BackColor;
                            bt4.BackColor = bt5.BackColor;

                            bt5.cambiarEstado(bt3.estadoBoton());
                            bt3.cambiarEstado(bt4.estadoBoton());
                            bt4.cambiarEstado(bt5.estadoBoton());
                        }

                    }
                    else if (this.rejilla == TipoRejilla.COLORES)
                    {
                        CMiBoton bt5 = new CMiBoton(Color.Empty);
                        bt5.BackColor = bt3.BackColor;
                        bt3.BackColor = bt4.BackColor;
                        bt4.BackColor = bt5.BackColor;

                        bt5.cambiarEstado(bt3.estadoBoton());
                        bt3.cambiarEstado(bt4.estadoBoton());
                        bt4.cambiarEstado(bt5.estadoBoton());
                    }
                    else if (this.rejilla == TipoRejilla.IMAGENES)
                    {
                        CMiBoton bt5 = new CMiBoton("");
                        bt5.BackgroundImage = bt3.BackgroundImage;
                        bt3.BackgroundImage = bt4.BackgroundImage;
                        bt4.BackgroundImage = bt5.BackgroundImage;

                        bt5.cambiarEstado(bt3.estadoBoton());
                        bt3.cambiarEstado(bt4.estadoBoton());
                        bt4.cambiarEstado(bt5.estadoBoton());
                    }
                    else if (this.rejilla == TipoRejilla.LETRAS || this.rejilla == TipoRejilla.ABECEDARIO)
                    {
                        CMiBoton bt5 = new CMiBoton("");

                        if (this.programarColoresTachado[0] == true)
                        {
                            //solo el texto
                            bt5.Text = bt3.Text;
                            bt3.Text = bt4.Text;
                            bt4.Text = bt5.Text;

                            bt5.cambiarEstado(bt3.estadoBoton());
                            bt3.cambiarEstado(bt4.estadoBoton());
                            bt4.cambiarEstado(bt5.estadoBoton());
                        }
                        else if (this.programarColoresTachado[1] == true)
                        {
                            //intercambio solo el color de fondo
                            bt5.BackColor = bt3.BackColor;
                            bt3.BackColor = bt4.BackColor;
                            bt4.BackColor = bt5.BackColor;
                        }
                        else if (this.programarColoresTachado[2] == true)
                        {
                            //intercambio todo el texto y el color
                            bt5.Text = bt3.Text;
                            bt3.Text = bt4.Text;
                            bt4.Text = bt5.Text;

                            bt5.BackColor = bt3.BackColor;
                            bt3.BackColor = bt4.BackColor;
                            bt4.BackColor = bt5.BackColor;

                            bt5.cambiarEstado(bt3.estadoBoton());
                            bt3.cambiarEstado(bt4.estadoBoton());
                            bt4.cambiarEstado(bt5.estadoBoton());
                        }
                    }
                    else if (this.rejilla == TipoRejilla.WINGDINGS)
                    {
                        CMiBoton bt5 = new CMiBoton("");
                        if (this.programarColoresTachado[0])
                        {
                            bt5.Font = bt3.Font;
                            bt5.Text = bt3.Text;
                            bt5.textoFuente = bt3.textoFuente;

                            bt3.Font = bt4.Font;
                            bt3.Text = bt4.Text;
                            bt3.textoFuente = bt4.textoFuente;

                            bt4.Font = bt5.Font;
                            bt4.Text = bt5.Text;
                            bt4.textoFuente = bt5.textoFuente;

                            bt5.cambiarEstado(bt3.estadoBoton());
                            bt3.cambiarEstado(bt4.estadoBoton());
                            bt4.cambiarEstado(bt5.estadoBoton());
                        }
                        else if (this.programarColoresTachado[1] == true)
                        {
                            //intercambio solo el color de fondo
                            bt5.BackColor = bt3.BackColor;
                            bt3.BackColor = bt4.BackColor;
                            bt4.BackColor = bt5.BackColor;
                        }
                        else if (this.programarColoresTachado[2] == true)
                        {
                            bt5.Font = bt3.Font;
                            bt5.Text = bt3.Text;
                            bt5.textoFuente = bt3.textoFuente;

                            bt3.Font = bt4.Font;
                            bt3.Text = bt4.Text;
                            bt3.textoFuente = bt4.textoFuente;

                            bt4.Font = bt5.Font;
                            bt4.Text = bt5.Text;
                            bt4.textoFuente = bt5.textoFuente;

                            bt5.BackColor = bt3.BackColor;
                            bt3.BackColor = bt4.BackColor;
                            bt4.BackColor = bt5.BackColor;

                            bt5.cambiarEstado(bt3.estadoBoton());
                            bt3.cambiarEstado(bt4.estadoBoton());
                            bt4.cambiarEstado(bt5.estadoBoton());
                        }


                    }//fin rejilla wind
                }
            }
        }


        protected void temporizadorAleatorio_Elapsed(object source, ElapsedEventArgs e)
        {
            this.threadAleatorio = new Thread(new ThreadStart(this.llamadaSeguraHebraAleatorio));
            this.threadAleatorio.Start();
        }

        private bool PararHiloBotonesAleatorios()
        {
            //Funcion usada por el hilo principal para detener la ejecucion del hilo secundario
            if (threadAleatorio == null || !threadAleatorio.IsAlive)
                return false;
            //cambiar el estado de este controlador de espera a señalizado para informar
            // al hebraLineaMolesta que debe parar
            controladorPararTemporizadorAleatorio.Set();

            //esperar a que el hilo secundario informe de que ha parado
            while (threadAleatorio.IsAlive)
            {
                if (WaitHandle.WaitAll((new ManualResetEvent[] { controladorTemporizadorAleatorioParado }), 100, false))
                {
                    //MessageBox.Show("Parando el controlador de los boones aleatorios");
                    break;
                }
                Application.DoEvents();
            }
            threadAleatorio = null;
            return true;
        }

        private void llamadaSeguraHebraAleatorio()
        {
            if (controladorPararTemporizadorAleatorio.WaitOne(0, true))
            {
                //tareas de limpieza
                controladorTemporizadorAleatorioParado.Set();
            }
            else
                this.aleatorioBotones(this.tabla);
        }

        /*
         * 
         * Metodos para el tiempo límite
         * 
         */ 
        private void tiempoLimite(int tempo)
        {
            temporizadorTiempoLimite = new System.Timers.Timer();
            temporizadorTiempoLimite.Elapsed += new ElapsedEventHandler(temporizadorTiempoLimite_Elapsed);
            temporizadorTiempoLimite.Interval = tempo * 1000;
            temporizadorTiempoLimite.Enabled = true;
        }

        protected void temporizadorTiempoLimite_Elapsed(object source, ElapsedEventArgs e)
        {
            this.hebraTemporizadorTiempoLimite = new Thread(new ThreadStart(this.llamadaSeguraHebraTiempoLimite));
            this.hebraTemporizadorTiempoLimite.Start();
        }

        private void llamadaSeguraHebraTiempoLimite()
        {
            this.cerrarFormularioRejilla(this.FindForm());
        }

        private void cerrarFormularioRejilla(Form f)
        {
            if (f.InvokeRequired)
            {
                cerrarRejilla cr = new cerrarRejilla(cerrarFormularioRejilla);
                this.Invoke(cr, new object[] { f });
            }
            else
                f.Close();
        }

        /*
         * Metodos para intercambiar el fondo del boton emparejar
         */
        private void timepoEmparejar(int tempo)
        {
            this.temporizadorBotonParejas = new System.Timers.Timer();
            this.temporizadorBotonParejas.Elapsed += new ElapsedEventHandler(temporizadorBotonParejas_Elapsed);
            this.temporizadorBotonParejas.Interval = 1000 * tempo;
            this.temporizadorBotonParejas.Enabled = true;

            this.controladorBotonesParejaParado = new ManualResetEvent(false);
            this.controladorPararBotonesPareja = new ManualResetEvent(false);
        }

        protected void temporizadorBotonParejas_Elapsed(object source, ElapsedEventArgs e)
        {
            this.hebraTemporiadorParejas = new Thread(new ThreadStart(this.llamadaSeguraBotonParejas));
            this.hebraTemporiadorParejas.Start();

        }
        private void  llamadaSeguraBotonParejas()
        {
            this.intercambiarFondo(this.bParejas);
        }

        private void intercambiarFondo(CBotonEmparejar buton)
        {
            if (this.bParejas.InvokeRequired)
            {
                aleatorioBotonParejas ap = new aleatorioBotonParejas(intercambiarFondo);
                this.bParejas.Invoke(ap, new object[] { buton });
            }
            else
            { //escribir todo el codigo para intercambiar el valor del boton emparejar

                if (this.position == PosicionBotonEmparejar.ALEATORIA)
                {
                    int[] oo = new int[]{0,0};
                    oo = posicionEmparejar(this.bParejas.Size.Height, coordenadaX_bParejas, finTabla_bParejas);
                    this.bParejas.Location = new Point(oo[0], oo[1]);

                }
                
                if (this.rejilla == TipoRejilla.NUMERICA)
                {
                    int valorBoton = Convert.ToInt32(bParejas.Text);
                    switch (this.emparejarSecuencia)
                    {
                        case EmparejarSecuencia.SECUENCIAL:

                            bParejas.Text = (Convert.ToInt32(bParejas.Text) + incrementandoNext).ToString();
                            if (valorBoton + incrementandoNext > ultimo)
                                bParejas.Text = primero.ToString();

                            if (this.listaString.Count != 0)
                            {
                                while (!listaString.Contains(bParejas.Text))
                                {
                                    valorBoton = Convert.ToInt32(bParejas.Text);
                                    valorBoton += incrementandoNext;
                                    if (valorBoton > ultimo)
                                        valorBoton = primero;
                                    bParejas.Text = (valorBoton).ToString();
                                }
                            }
                            break;
                        case EmparejarSecuencia.ALEATORIA:
                            bParejas.Text = new Random((int)DateTime.Now.Ticks).Next(primero, ultimo + 1).ToString();
                            if (this.listaString.Count != 0)
                            {
                                while (!listaString.Contains(bParejas.Text))
                                    bParejas.Text = new Random((int)DateTime.Now.Ticks).Next(primero, ultimo + 1).ToString();
                            }
                            break;
                        case EmparejarSecuencia.INVERSA:
                            bParejas.Text = (Convert.ToInt32(bParejas.Text) - incrementandoNext).ToString();
                            if (valorBoton - incrementandoNext < primero)
                                bParejas.Text = ultimo.ToString();

                            if (this.listaString.Count != 0)
                            {
                                while (!listaString.Contains(bParejas.Text))
                                    bParejas.Text = (Convert.ToInt32(bParejas.Text) - incrementandoNext).ToString();
                            }
                            break;
                    }

                    if (ultimo > 10 && ultimo < 100)
                    {
                        if (Convert.ToInt32(bParejas.Text) < 10)
                        {
                            bParejas.Text = "0" + bParejas.Text;
                        }
                    }
                    else if (ultimo >= 100 && ultimo < 1000)
                    {
                        if (Convert.ToInt32(bParejas.Text) < 10)
                        {
                            bParejas.Text = "00" + bParejas.Text;
                        }
                        else if (Convert.ToInt32(bParejas.Text) < 100)
                        {
                            bParejas.Text = "0" + bParejas.Text;
                        }
                    }
                }
                else if (this.rejilla == TipoRejilla.COLORES)
                {
                    int indiceColor = this.listaColoresTacharParejas.IndexOf(bParejas.BackColor);
                    if (indiceColor == -1)
                        indiceColor = indiceListas - 1;
                     
                    switch (this.emparejarSecuencia)
                    {
                        case EmparejarSecuencia.SECUENCIAL:
                            if (indiceColor == this.listaColoresTacharParejas.Count - 1)
                                indiceColor = 0;
                            else
                                indiceColor++;
                            //this.bParejas.BackColor = this.listaColoresTacharParejas.ElementAt(indiceColor);
                            break;
                        case EmparejarSecuencia.ALEATORIA:
                            indiceColor = new Random((int)DateTime.Now.Ticks).Next(0, this.listaColoresTacharParejas.Count);
                            //this.bParejas.BackColor = this.listaColoresTacharParejas.ElementAt(new Random((int)DateTime.Now.Ticks).Next(0, this.listaColoresTacharParejas.Count));
                            break;
                        case EmparejarSecuencia.INVERSA:
                            if (indiceColor <= 0)
                                indiceColor = this.listaColoresTacharParejas.Count - 1;
                            else
                                indiceColor--;
                            //this.bParejas.BackColor = this.listaColoresTacharParejas.ElementAt(indiceColor);
                            break;
                    }
                    if (this.listaColoresTacharParejas.Count > 0)
                        this.bParejas.BackColor = this.listaColoresTacharParejas.ElementAt(indiceColor);
                }
                else if (this.rejilla == TipoRejilla.IMAGENES)
                {
                    int indiceImagen = this.listaImagenesTacharParejas.IndexOf(bParejas.BackgroundImage);
                    if (indiceImagen == -1)
                        indiceImagen = indiceListas - 1;
                    switch (this.emparejarSecuencia)
                    {
                        case EmparejarSecuencia.SECUENCIAL:
                            if (indiceImagen == this.listaImagenesTacharParejas.Count - 1)
                                indiceImagen = 0;
                            else
                                indiceImagen++;
                            break;
                        case EmparejarSecuencia.ALEATORIA:
                            indiceImagen = new Random((int)DateTime.Now.Ticks).Next(0, this.listaImagenesTacharParejas.Count);
                            break;
                        case EmparejarSecuencia.INVERSA:
                            if (indiceImagen <= 0)
                                indiceImagen = this.listaImagenesTacharParejas.Count - 1;
                            else
                                indiceImagen--;
                            break;
                    }
                    if (this.listaImagenesTacharParejas.Count > 0)
                    {
                        this.bParejas.BackgroundImage = this.listaImagenesTacharParejas.ElementAt(indiceImagen);
                        this.bParejas.BackgroundImageLayout = ImageLayout.Stretch;
                    }
                }
                else if (this.rejilla == TipoRejilla.LETRAS)
                {
                    int indiceLetras = this.listaLetrasTacharParejas.IndexOf(this.bParejas.Text);
                    //MessageBox.Show("indiceLetras : "+ indiceLetras + " indiceListas " + indiceListas );
                    if (indiceLetras == -1)
                        indiceLetras = indiceListas-1;
                    switch (this.emparejarSecuencia)
                    {
                        case EmparejarSecuencia.SECUENCIAL:
                            if (indiceLetras == this.listaLetrasTacharParejas.Count - 1)
                                indiceLetras = 0;
                            else
                                indiceLetras++;
                            break;
                        case EmparejarSecuencia.ALEATORIA:
                            indiceLetras = new Random((int)DateTime.Now.Ticks).Next(0, this.listaLetrasTacharParejas.Count);
                            break;
                        case EmparejarSecuencia.INVERSA:
                            if (indiceLetras <= 0)
                                indiceLetras = this.listaLetrasTacharParejas.Count - 1;
                            else
                                indiceLetras--;
                            break;
                    }
                    if (this.listaLetrasTacharParejas.Count > 0)
                        this.bParejas.Text = this.listaLetrasTacharParejas.ElementAt(indiceLetras);
                }
                else if (this.rejilla == TipoRejilla.ABECEDARIO)
                {
                    int indiceAbecedario = this.listaAbecedarioTacharParejas.IndexOf(this.bParejas.Text);
                    if (indiceAbecedario == -1)
                        indiceAbecedario = indiceListas - 1;
                    switch (this.emparejarSecuencia)
                    {
                        case EmparejarSecuencia.SECUENCIAL:
                            if (indiceAbecedario == this.listaAbecedarioTacharParejas.Count - 1)
                                indiceAbecedario = 0;
                            else
                                indiceAbecedario++;
                            break;
                        case EmparejarSecuencia.ALEATORIA:
                            indiceAbecedario = new Random((int)DateTime.Now.Ticks).Next(0, this.listaAbecedarioTacharParejas.Count);
                            break;
                        case EmparejarSecuencia.INVERSA:
                            if (indiceAbecedario <= 0)
                                indiceAbecedario = this.listaAbecedarioTacharParejas.Count - 1;
                            else
                                indiceAbecedario--;
                            break;
                    }
                    if (this.listaAbecedarioTacharParejas.Count > 0)
                        this.bParejas.Text = this.listaAbecedarioTacharParejas.ElementAt(indiceAbecedario);
                }
                else if (this.rejilla == TipoRejilla.WINGDINGS)
                {
                    int indiceWin = this.listaWindingsTacharParejas.IndexOf(new TipoLetraFuente(this.bParejas.Font,this.bParejas.Text));
                    if (indiceWin == -1)
                        indiceWin = indiceListas - 1;
                    switch (this.emparejarSecuencia)
                    {
                        case EmparejarSecuencia.SECUENCIAL:
                            if (indiceWin == this.listaWindingsTacharParejas.Count - 1)
                                indiceWin = 0;
                            else
                                indiceWin++;
                            break;
                        case EmparejarSecuencia.ALEATORIA:
                            indiceWin = new Random((int)DateTime.Now.Ticks).Next(0, this.listaWindingsTacharParejas.Count);
                            break;
                        case EmparejarSecuencia.INVERSA:
                            if (indiceWin <= 0)
                                indiceWin = this.listaWindingsTacharParejas.Count - 1;
                            else
                                indiceWin--;
                            break;
                    }
                    if (this.listaWindingsTacharParejas.Count > 0)
                    {
                        this.bParejas.Font = this.listaWindingsTacharParejas.ElementAt(indiceWin).devolverFuente();
                        this.bParejas.Text = this.listaWindingsTacharParejas.ElementAt(indiceWin).devolverLetra();
                    }
                }
                this.bParejas.capturarTiempo();
            }
        }

        private bool PararHiloBotonParejas()
        {
            //Funcion usada por el hilo principal para detener la ejecucion del hilo secundario
            if (hebraTemporiadorParejas == null || !hebraTemporiadorParejas.IsAlive)
                return false;
            //cambiar el estado de este controlador de espera a señalizado para informar
            // al hebraLineaMolesta que debe parar
            controladorPararBotonesPareja.Set();
            //esperar a que el hilo secundario informe de que ha parado
            while (hebraTemporiadorParejas.IsAlive)
            {
                if (WaitHandle.WaitAll((new ManualResetEvent[] { controladorBotonesParejaParado }), 100, false))
                {
                    //MessageBox.Show("Parando la hebra de la linea");
                    break;
                }
                Application.DoEvents();
            }
            hebraTemporiadorParejas = null;
            return true;
        }

        public void cambiarTiempoBotonParejas()
        {
            this.temporizadorBotonParejas.Enabled = false;
            this.temporizadorBotonParejas.Interval = this.timeParejas * 1000;
            this.temporizadorBotonParejas.Enabled = true;
            this.llamadaSeguraBotonParejas();
        }
    }
}