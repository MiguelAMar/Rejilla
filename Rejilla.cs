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
using System.Drawing.Printing;
using System.Linq;
using System.Resources;
using System.Runtime.Versioning;
using System.Security.Policy;
using System.Windows.Forms;

namespace demorejilla
{
    [SupportedOSPlatform("windows")]
    public partial class Rejilla : Form
    {
        /*
         * Variables:
         *  - tabla: donde colocamos los botones por posición (i, j)

         *  - indiceMatrizTiempos : indice que recorre el array matrizTiempos                       
         *  - matrizTiempos : array que guarda consecutivamente el reloj del sistema
         *                    cada vez que tachamos un boton de la rejilla
         *                    
         *  - pulsadoBoton : variable booleana que es true si pulso un boton correctamente de la rejilla.
         *                   Para luego proceder a dibujar la grafica.
         *  
         */

        public bool pulsadoBoton = false;
        private TipoRejilla rejilla;

        private System.Windows.Forms.TableLayoutPanel tabla;

        public int indiceMatrizTiempos;
        //tiempos en tachar los botnes
        public DateTime[] matrizTiempos;

        //array de TimeSpan para los tiempos del emparejamiento
        public TimeSpan[] matrizTimeEmparejar;

        //variables para la tarea preliminar
        public DateTime boton1;
        public DateTime boton2;
        public bool pulsadoPrimerBoton;
      
        //Variable pra los colores de fondo de la rejilla
        private ClaseColores coloresBackground;
        private List<Color> coloresFondoElegidos;

        //Variable para programar el intercambio y tachado de los botones
        private bool[] programarColoresTachado;
        public bool rejillaTacharColor;
        public bool rejillaTacharTexto;
        public bool rejillaTacharNada;
        public bool rejillaTacharTodo;

        //variables para el control de tachado en los numeros y las letras y abecedario.
        public SortedList<String, DatosControlTachado> listaControl;
        public List<String> listaOrdenPulsacíon;
        public int aciertos, fallos;
        private int numeroCasillasTachar;

        //variables para las parejas.
        public CBotonEmparejar bParejas;
        private int primero;
        private EmparejarSecuencia emparejarSecuencia;
        private PosicionBotonEmparejar position;
        public bool emparejar;
        private int timeParejas;
        private int ultimo;
        public List<String> listaString;
        public List<Color> listaColores;
        public List<Image> listaImagenes;
        public List<TipoLetraFuente> listaWin;

        public int indiceListas;
        public int indiceNumeros;

        //variables para si el bEmaprejar es aleatori necesiot saber la coordenadaX y en fin de la tabla
        private int coordenadaX_bParejas;
        private int finTabla_bParejas;
        private bool izq_der; //si true izq sino derecha

        //cosntructor de la tareaPreliminar
        public Rejilla(int numeroDeFilas, int numeroDeColumnas, int tamañoDelBoton)
        {
            pulsadoPrimerBoton = false;
            
            //2024
            int anchoPantalla = Screen.PrimaryScreen.WorkingArea.Width;
            int altoPantalla = Screen.PrimaryScreen.WorkingArea.Height;
            this.Width = anchoPantalla;
            this.Height = altoPantalla;
            // Fin 2024

            
            //2010
            Pintar(numeroDeFilas, numeroDeColumnas, tamañoDelBoton);
            
        }

        //Constructor de la rejilla
        public Rejilla(int numFilas, int numColumnas, int comienzoRejilla, int siguienteTachar, int tam,
            bool ale, int tiempoAleatorio, //de aleatorizar los botones
            tipoLinea line, bool conLinea, Color colors, int gorda, double velocidad, //de la lineaMolesta
            int tempo, bool tempoLimite, // tiempo límite para finalizar
            TipoRejilla tipo,
            ClaseColores coloresTotales, List<Color> coloresBotones, TantoPorCiento cantidadMinimaBotones, //tipo de la rejilla y rejilla colores
            List<String> listaRejilla, List<String> listaUsuario, FontFamily familiaFuente, TantoPorCiento minimoLetras,//rejilla de letras 
            List<Image> listaTacharImagenes, List<Image> listaTodasImagenes,TantoPorCiento cantidadMinimaBotonesImg, // listaimagenes
            string textoAbecedario, bool[] opciones, bool orden,//bool mayusculasMinusculas,  //abecedario
            List<TipoLetraFuente> lCompleta , List<TipoLetraFuente> lElegidos, TantoPorCiento tpcW,// Wingdings
            bool colorDetrasFondo, List<Color> lsColor, //Colores de fondo en la rejilla
            bool[] programarBotones, bool gridTacharColor, bool gridTacharTexto, bool gridTacharNada, bool gridTacharTodo, bool ordenDescendente, //vector para intercambiar las cosas
            bool emparejamos, PosicionBotonEmparejar p, EmparejarSecuencia sequence,int thempo )
        {


           


            rm = new ResourceManager("demorejilla.Recursos", typeof(MetiendoDatos).Assembly);
            
            indiceMatrizTiempos = 1; // porque la posicion 0 la usamos para capturar
                                     // el valor del reloj justo antes de empezar a 
                                     // a tachar botones
            
            matrizTiempos = new DateTime[numColumnas * numFilas + 1];//cantidad de botones mas el TIEMPO INICIAL

            this.rejilla = tipo;
             //NEXT se usa tanto para la rejilla numerica como para la rejilla del abecedario

            this.programarColoresTachado = programarBotones;

            this.rejillaTacharColor = gridTacharColor;
            this.rejillaTacharTexto = gridTacharTexto;
            this.rejillaTacharNada = gridTacharNada;
            this.rejillaTacharTodo = gridTacharTodo;

            this.aciertos = this.fallos = 0;

            if (colorDetrasFondo)
            {
                this.coloresBackground = coloresTotales;
                this.coloresFondoElegidos = lsColor;
            }
            if (this.rejilla == TipoRejilla.NUMERICA)
                rejillaNumerica(numFilas, numColumnas, comienzoRejilla, siguienteTachar, ordenDescendente);
            else if (this.rejilla == TipoRejilla.COLORES)
                rejillaColores(numFilas, numColumnas, coloresTotales, coloresBotones, cantidadMinimaBotones);
            else if (this.rejilla == TipoRejilla.LETRAS)
                rejillaLetras(numFilas, numColumnas, listaRejilla, listaUsuario, familiaFuente, minimoLetras);
            else if (this.rejilla == TipoRejilla.IMAGENES)
                rejillaImagenes(numFilas, numColumnas, listaTacharImagenes, listaTodasImagenes, cantidadMinimaBotonesImg);
            else if (this.rejilla == TipoRejilla.ABECEDARIO)
            {
                this.next = 1;
                rejillaAbecedario(numFilas, numColumnas, textoAbecedario, opciones, orden);//, mayusculasMinusculas);
            }
            else if (this.rejilla == TipoRejilla.WINGDINGS)
                rejillaWingdings(numFilas, numColumnas, lCompleta, lElegidos, tpcW);


            if (ale)
                aleatorizarBotones(numFilas, numColumnas, tiempoAleatorio);
            if (conLinea)
            {
                this.hayLinea = conLinea;
                distraccionLinea(line, colors, gorda, velocidad);
            }
             
            if (tempoLimite)
                tiempoLimite(tempo);

            this.emparejarSecuencia = sequence;
            this.emparejar = emparejamos;
            this.primero = comienzoRejilla;
            this.position = p;
            this.timeParejas = thempo;

            if (this.timeParejas < 0)
                throw new RejillaException("Tiempo parejas debe ser mayor que cero.");

            if (this.emparejar)
            {
                timepoEmparejar(this.timeParejas);
                this.listaString = new List<string>();
                this.listaColores = new List<Color>();
                this.listaImagenes = new List<Image>();
                this.listaWin = new List<TipoLetraFuente>();
                this.indiceListas = 0;
                this.indiceNumeros = 0;
                this.coordenadaX_bParejas = 0;
                this.finTabla_bParejas = 0;
            }
            
            this.matrizTimeEmparejar = new TimeSpan[numColumnas * numFilas];
             // Una vez inicializadas todas las variables pintamos la rejilla
            Pintar(numFilas, numColumnas, comienzoRejilla, tam, opciones, colorDetrasFondo);
        }


        //constructor para imprimir la rejilla de botones
        public Rejilla(int numFilas, int numColumnas, int comienzoRejilla, int siguienteTachar, int tam,
            TipoRejilla tipo, ClaseColores cColores, List<Color> coloresBotones, TantoPorCiento cantidadMinimaBotones, //tipo de la rejilla y rejilla colores
            List<String> listaRejilla, List<String> listaUsuario, FontFamily familiaFuente, TantoPorCiento minimoLetrasPlantilla, //rejilla de letras 
            List<Image> listaTacharImagenes, List<Image> listaTodasImagenes, TantoPorCiento cantidadMinimaBotonesImg, // listaimagenes
            string textoAbecedario, bool[] opciones, bool orden, //bool mayusculasMinusculas,  //abecedario
            List<TipoLetraFuente> lCompleta, List<TipoLetraFuente> lElegidos, TantoPorCiento tpcW, // Wingdings
            int esperar, bool colorFondoBoton, List<Color> lColores)
        {
            if (colorFondoBoton)
            {
                this.coloresBackground = cColores;
                this.coloresFondoElegidos = lColores;
            }

            this.soloImprimir = true;
            matrizTiempos = new DateTime[numColumnas * numFilas + 1];
            this.rejilla = tipo;
            //NEXT se usa tanto para la rejilla numerica como para la rejilla del abecedario

            if (this.rejilla == TipoRejilla.NUMERICA)
                rejillaNumerica(numFilas, numColumnas, comienzoRejilla, siguienteTachar);
            else if (this.rejilla == TipoRejilla.COLORES)
                rejillaColores(numFilas, numColumnas, cColores, coloresBotones, cantidadMinimaBotones);
            else if (this.rejilla == TipoRejilla.LETRAS)
                rejillaLetras(numFilas, numColumnas, listaRejilla, listaUsuario, familiaFuente, minimoLetrasPlantilla);
            else if (this.rejilla == TipoRejilla.IMAGENES)
                rejillaImagenes(numFilas, numColumnas, listaTacharImagenes, listaTodasImagenes, cantidadMinimaBotonesImg);
            else if (this.rejilla == TipoRejilla.ABECEDARIO)
            {
                this.next = 1;
                rejillaAbecedario(numFilas, numColumnas, textoAbecedario, opciones, orden); //, mayusculasMinusculas);
            }
            else if (this.rejilla == TipoRejilla.WINGDINGS)
                rejillaWingdings(numFilas, numColumnas, lCompleta, lElegidos, tpcW);

            // Una vez inicializadas todas las variables pintamos la rejilla
            Pintar(numFilas, numColumnas, comienzoRejilla, tam, opciones, colorFondoBoton);
            tiempoLimite(esperar);
        }

        /*
         * Capturar la rejilla e imprimirla
         */
        private bool soloImprimir = false;
        private void capturarEimprimir()
        {
            this.CapturarPantalla();
            if (cajaImprimir.ShowDialog() == DialogResult.OK)
            {
                this.printDocument.DefaultPageSettings.Landscape = true;
                this.printDocument.Print();
            }
        }

        Bitmap imagen;
        private void CapturarPantalla()
        {
            Graphics g = this.CreateGraphics();
            Size s = this.Size;
            imagen = new Bitmap(s.Width, s.Height-35, g);
            Graphics g2 = Graphics.FromImage(imagen);
            g2.CopyFromScreen(this.Location.X, this.Location.Y, 0, 0, s);
        }

        private void printDocument_Page(object sender, PrintPageEventArgs e)
        {
                e.Graphics.DrawImage(imagen, 50, 25);     
        }
        /*
         * Fin de captura la rejilla e impresion
         */ 


        // Inicializa el formulario y la tabla de botones
        private void Pintar(int cantidadFilas, int cantidadColumnas, int inicio, int tama, bool[] letraMayusMinus, bool colorF)
        {
            InitializeComponent();
            //2024
            //int anchoPantalla = Screen.PrimaryScreen.WorkingArea.Width;
            //int altoPantalla = Screen.PrimaryScreen.WorkingArea.Height;
            int anchoPantalla = Screen.PrimaryScreen.Bounds.Width;
            //int altoPantalla = Screen.PrimaryScreen.Bounds.Height; //Ponemos el formulario rejilla a todo el tamaño de la pantalla.
            int altoPantalla = Screen.PrimaryScreen.WorkingArea.Height;
            this.Width = anchoPantalla;
            this.Height = altoPantalla;
            // Fin 2024
            InitializaTabla(cantidadFilas, cantidadColumnas, inicio, tama, letraMayusMinus, colorF);
        }

        //Inicializa el formulario y la tabla de la tarea preliminar
        private void Pintar(int muchFilas, int muchColumnas, int sizeBoton)
        {
            InitializeComponent();
            //2024
            //int anchoPantalla = Screen.PrimaryScreen.WorkingArea.Width;
            //int altoPantalla = Screen.PrimaryScreen.WorkingArea.Height;
            int anchoPantalla = Screen.PrimaryScreen.Bounds.Width;
            //int altoPantalla = Screen.PrimaryScreen.Bounds.Height; //Ponemos el formulario rejilla a todo el tamaño de la pantalla.
            int altoPantalla = Screen.PrimaryScreen.WorkingArea.Height;
            this.Width = anchoPantalla;
            this.Height = altoPantalla;
            // Fin 2024
            InicializaTabla(muchFilas, muchColumnas, sizeBoton);
        }
        
        //Dibuja la rejilla de la tarea preliminar
        private void InicializaTabla(int f, int c, int t)
        {
            int coordenadaX, coordenadaY;

            this.tabla = new System.Windows.Forms.TableLayoutPanel();
            this.SuspendLayout();
            // 
            // tabla
            //             
            //this.tabla.Size = new System.Drawing.Size(this.ClientSize.Width, this.ClientSize.Height);
            //coordenadaX = (this.ClientSize.Width - c * (t + 5)) / 2;
            //coordenadaY = (this.ClientSize.Height - f * (t + 5)) / 2;
            //this.tabla.Location = new System.Drawing.Point(coordenadaX, coordenadaY);
            //Se calcula las coordenadas donde dibujar la rejilla en función del tamaño de la pantalla.
            
            // Se anula la versión anteriror porque el Size lo da en alta resolución Dpi. Y no queda centrada la rejilla en algunos monitores(portátiles sobre todo).
            Screen screen = Screen.PrimaryScreen;
            Rectangle rectanguloTrabajo = screen.WorkingArea;
            int widthInPixels = screen.Bounds.Width;
            int heightInPixels = screen.Bounds.Height;

            this.tabla.Width = this.ClientSize.Width;
            this.tabla.Height = this.ClientSize.Height;

            //Coordenadas en alta resolución
            coordenadaX = (widthInPixels - c * (t + 5)) / 2;
            coordenadaY = (heightInPixels - f * (t + 5)) / 2;


            int subirEjeY = coordenadaY;
            int desplazarEjeX = coordenadaX;

            //Coordenadas del área de trabajo de la pantalla, quitando la barra de Windows, es caso de estar visible. 
            coordenadaX = (rectanguloTrabajo.Width - c * (t + 5)) / 2;
            coordenadaY = (rectanguloTrabajo.Height - f * (t + 5)) / 2;

            int diferenciaY = subirEjeY - coordenadaY;
            coordenadaY -= diferenciaY;
            int diferenciaX = desplazarEjeX - coordenadaX;
            coordenadaX -= diferenciaX;

            this.tabla.Location = new System.Drawing.Point(coordenadaX, coordenadaY);

            this.tabla.Name = "tabla";
            this.tabla.RowCount = f;
            this.tabla.ColumnCount = c;
            this.tabla.TabIndex = 0;
            
            for (int nf = 0; nf < f; nf++)
            {
                for (int nc = 0; nc < c; nc++)
                {
                    if (nf == 0 && nc == 0)
                    {
                        CMiBoton b = new CMiBoton(0,0);
                        b.Font = new Font("Microsoft Sans Serif", (t / 4),
                                          System.Drawing.FontStyle.Regular,
                                          System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                        b.Size = new Size(t, t);
                        tabla.Controls.Add(b, nc, nf);
                    }
                    else if (nf == f-1 && nc == c-1)
                    {
                        CMiBoton b = new CMiBoton(1,1);
                        b.Font = new Font("Microsoft Sans Serif", (t / 4),
                                          System.Drawing.FontStyle.Regular,
                                          System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                        b.Size = new Size(t, t);
                        tabla.Controls.Add(b, nc, nf);
                    }
                    else
                    {
                        CMiBoton b = new CMiBoton(100,100);
                        b.Font = new Font("Microsoft Sans Serif", (t / 4),
                                          System.Drawing.FontStyle.Regular,
                                          System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                        b.Size = new Size(t, t);
                        b.Text = "";
                        b.FlatStyle = FlatStyle.Flat;
                        tabla.Controls.Add(b, nc, nf);
                    }
                }
            }
            this.Controls.Add(this.tabla);
            this.ResumeLayout(false);
        }
        
        /*
         * Dibuja la rejilla en el formulario
         * Inicializa la distracción línea, si la hay.
         */
        private void InitializaTabla(int numeroDeFilas, int numeroDeColumnas, int valorInicial, int size, bool[] letraBotonMayusMinus, bool coloritosFondo)
        {
            //variable para la rejilla abecedario para el valor inicial del
            // boton emparejar
            int valor = 0;
            // Para ir seleccionando botones de la matrizBotones de forma aleatoria
            Random aleatorio = new Random((int) DateTime.Now.Ticks);
            // Coordeandas donde empezar a dibujar la rejilla
            //Se usan para que salga centrada la rejilla en la pantalla
            int coordenadaX ,coordenadaY;
            
            this.tabla = new System.Windows.Forms.TableLayoutPanel();
            this.SuspendLayout();
            // 
            // tabla
            //             
            //Versión anterior 2010.
            this.tabla.Size = new System.Drawing.Size(this.ClientSize.Width, this.ClientSize.Height);
            coordenadaX = (this.ClientSize.Width - numeroDeColumnas * (size + 5)) / 2;
            coordenadaY = (this.ClientSize.Height - numeroDeFilas * (size + 5)) / 2;

            
            //Emparejar
            //Emparejar
            int ultimoValor = valorInicial + ((numeroDeFilas * numeroDeColumnas) - 1) * incrementandoNext;
            int ejeX = 0;
            int ejeY = 0;
            int[] ejes = new int[2];
            int finTabla = coordenadaX + (numeroDeColumnas * (size + 5))+5;
            this.izq_der = true;
            this.coordenadaX_bParejas = coordenadaX;
            this.finTabla_bParejas = finTabla;

            if (emparejar)
            {
                this.ultimo = ultimoValor;
                bParejas = new CBotonEmparejar();
                bParejas.Size = new Size(size, size);


                ejes = posicionEmparejar(size, coordenadaX, finTabla);
                ejeX = ejes[0];
                ejeY = ejes[1];


                if ((this.position == PosicionBotonEmparejar.IZQUIERDA_CENTRO
                    || this.position == PosicionBotonEmparejar.IZQUIERDA_INFERIOR
                    || this.position == PosicionBotonEmparejar.IZQUIERDA_SUPERIOR)
                    //&& this.bParejas.Location.X + this.bParejas.Width > coordenadaX)
                    && ejeX + this.bParejas.Width > coordenadaX)
                {
                    this.temporizadorBotonParejas.Enabled = false;
                    throw new RejillaException(rm.GetString("empa00"));//" No cabe el bototn emparejar, disminuya una columna.");
                }
                if ((this.position == PosicionBotonEmparejar.DERECHA_CENTRO
                    || this.position == PosicionBotonEmparejar.DERECHA_INFERIOR
                    || this.position == PosicionBotonEmparejar.DERECHA_SUPERIOR)
                    && ejeX < finTabla)
                {
                    this.temporizadorBotonParejas.Enabled = false;
                    throw new RejillaException(rm.GetString("empa00"));//"Boton derecha, quitar columnas.");
                }
                if (this.position == PosicionBotonEmparejar.ALEATORIA)
                    if (this.izq_der && ejeX + this.bParejas.Width > coordenadaX)
                    {
                        this.temporizadorBotonParejas.Enabled = false;
                        throw new RejillaException(rm.GetString("empa00"));//" No cabe el bototn emparejar, disminuya una columna.");
                    }
                    else if (!this.izq_der && ejeX < finTabla)
                    {
                        this.temporizadorBotonParejas.Enabled = false;
                        throw new RejillaException(rm.GetString("empa00"));//"Boton derecha, quitar columnas.");
                    }


            }
            //fin emparejar

            this.tabla.Location = new System.Drawing.Point(coordenadaX, coordenadaY);
            this.tabla.Name = "tabla";
            this.tabla.RowCount = numeroDeFilas;
            this.tabla.ColumnCount = numeroDeColumnas;
            this.tabla.TabIndex = 0;

            int posicion;//posicion del boton que cojo de la matrizBotones

            int semilla; //para escoger los aleatorios de los colores de fondo
            semilla = (int)DateTime.Now.Ticks;
            Random rnd = new Random(semilla);

            String textoNumerico = "";

            bool intercambiarMayusMinus = true;
            if (letraBotonMayusMinus[3])
                intercambiarMayusMinus = true;
            else
                intercambiarMayusMinus = false;

            for (int i = 0; i < numeroDeFilas; ++i)
            {
                for (int j = 0; j < numeroDeColumnas; ++j)
                {
                    //Para la matriz numerica
                    if (rejilla == TipoRejilla.NUMERICA)
                    {
                        //Randon.Next coge el rango [a, b)
                        posicion = aleatorio.Next(0, matrizBotones.Length);
                        while (matrizBotones[posicion].escogido)
                        {
                            posicion = aleatorio.Next(0, matrizBotones.Length);
                        }
                        matrizBotones[posicion].escogido = true;

                        CMiBoton b = new CMiBoton(matrizBotones[posicion].valor);

                        //Se divide por 4 para que quepan 3 dígitos usando point, si es pixel se divide entre 3.
                        b.Font = new Font("Microsoft Sans Serif", (size / 4),
                                          System.Drawing.FontStyle.Regular,
                                          System.Drawing.GraphicsUnit.Point, ((byte)(0)));

                        if (emparejar)
                        {
                            this.listaString.Add(b.Text);
                        }

                        //Si es ascendente y el ultimo bototn actualizo su  variable ultimo botton
                        if (!ordenParaTachar)
                        {
                            //Si es el ultimoBoton actualizo su variable ultimoBoton.
                            if (b.Text.Equals(matrizBotones[matrizBotones.Length - 1].valor.ToString()))

                                b.valorUltimoBoton = matrizBotones[matrizBotones.Length - 1].valor;
                        }
                        else
                        {
                            //Si es el primer boton actualizo su variable ultimo boton
                            if (b.Text.Equals(matrizBotones[0].valor.ToString()))
                                b.valorUltimoBoton = matrizBotones[0].valor;
                        }

                        //para que el texto de los numero tenga la misma longitud y no se pueda discriminar.
                        if (ultimoValor > 10 && ultimoValor < 100)
                        {
                            if (matrizBotones[posicion].valor < 10)
                            {
                                textoNumerico = b.Text;
                                b.Text = "0" + textoNumerico;
                            }
                        }
                        else if (ultimoValor >= 100 && ultimoValor < 1000)
                        {
                            if (matrizBotones[posicion].valor < 10)
                            {
                                textoNumerico = b.Text;
                                b.Text = "00" + textoNumerico;
                            }
                            else if (matrizBotones[posicion].valor < 100)
                            {
                                textoNumerico = b.Text;
                                b.Text = "0" + textoNumerico;
                            }
                        }


                        b.inicializarBoton();
                        b.Size = new Size(size, size); //para selecionar el tamaño que yo quiera del boton
                        //Si el boton lleva color de fondo
                        if (coloritosFondo)
                        {
                            b.BackColor = this.coloresFondoElegidos.ElementAt(rnd.Next(0, this.coloresFondoElegidos.Count));
                        }
                        //añado el boton a la tabla
                        tabla.Controls.Add(b, j, i); //añadiendo el boton por posicion en la tabla
                    }
                    else if (rejilla == TipoRejilla.COLORES)
                    {
                        posicion = aleatorio.Next(0, arrayColores.Length);
                        while (arrayColores[posicion].escogido)
                        {
                            posicion = aleatorio.Next(0, arrayColores.Length);
                        }
                        arrayColores[posicion].escogido = true;

                        //rejilla de botones de colores
                        CMiBoton b = new CMiBoton(this.arrayColores[posicion].color);
                        if (listaColoresTachar.Contains(this.arrayColores[posicion].color))
                        {
                            if (emparejar)
                            {
                                this.listaColores.Add(b.BackColor);
                                if (!this.listaColoresTacharParejas.Contains(b.BackColor))
                                    this.listaColoresTacharParejas.Add(b.BackColor);
                            }
                            this.numColoresTotal++;
                        }
                        b.Size = new Size(size, size);
                        b.inicializarBoton();
                        tabla.Controls.Add(b, j, i);
                    }
                    else if (rejilla == TipoRejilla.LETRAS)
                    {
                        //rejilla de letras crearala
                        posicion = aleatorio.Next(0, this.arrayLetras.Length);
                        while (arrayLetras[posicion].escogido)
                        {
                            posicion = aleatorio.Next(0, arrayLetras.Length);
                        }
                        arrayLetras[posicion].escogido = true;

                        CMiBoton b = new CMiBoton(this.arrayLetras[posicion].letra);
                        b.Font = new Font(this.tipoLetra, size / 2);
                        if (this.listaLetrasTachar.Contains(this.arrayLetras[posicion].letra))
                        {
                            if (emparejar)
                            {
                                this.listaString.Add(b.Text);
                                if (!this.listaLetrasTacharParejas.Contains(b.Text))
                                    this.listaLetrasTacharParejas.Add(b.Text);

                            }
                            this.numLetrasTotal++;
                        }
                        b.Size = new Size(size, size);
                        b.inicializarBoton();
                        if (coloritosFondo)
                        {
                            b.BackColor = this.coloresFondoElegidos.ElementAt(rnd.Next(0, this.coloresFondoElegidos.Count));
                        }
                        tabla.Controls.Add(b, j, i);
                    }
                    else if (rejilla == TipoRejilla.IMAGENES)
                    {
                        //rejilla de imagenes
                        posicion = aleatorio.Next(0, this.arrayImagenes.Length);
                        while (arrayImagenes[posicion].escogido)
                        {
                            posicion = aleatorio.Next(0, arrayImagenes.Length);
                        }
                        arrayImagenes[posicion].escogido = true;

                        CMiBoton b = new CMiBoton(this.arrayImagenes[posicion].imagen);
                        //2024 Ajustamos la imagen al botón
                        b.BackgroundImageLayout = ImageLayout.Stretch;
                        if (this.listaImagenesTachar.Contains(this.arrayImagenes[posicion].imagen))
                        {
                            if (emparejar)
                            {
                                this.listaImagenes.Add(b.BackgroundImage);
                                if (!this.listaImagenesTacharParejas.Contains(b.BackgroundImage))
                                    this.listaImagenesTacharParejas.Add(b.BackgroundImage);

                            }
                            this.numImagenesTotal++;
                        }
                        b.Size = new Size(size, size);
                        b.inicializarBoton();
                        tabla.Controls.Add(b, j, i);
                    }
                    else if (rejilla == TipoRejilla.ABECEDARIO)
                    {
                        posicion = aleatorio.Next(0, this.arrayAbecedario.Length);
                        while (arrayAbecedario[posicion].escogido)
                        {
                            posicion = aleatorio.Next(0, arrayAbecedario.Length);
                        }
                        arrayAbecedario[posicion].escogido = true;

                        CMiBoton b = new CMiBoton(arrayAbecedario[posicion].orden, arrayAbecedario[posicion].texto, letraBotonMayusMinus, intercambiarMayusMinus);

                        b.Font = new Font("Microsoft Sans Serif", (size / 6), //se divide entre 6 par que kepan todas las letras en una misma linea del boton
                                         System.Drawing.FontStyle.Regular,
                                         System.Drawing.GraphicsUnit.Point, ((byte)(0)));

                        b.Size = new Size(size, size);
                        //si es el ultimo boton actualizo la variable ultimo boton
                        if (b.valor == arrayAbecedario[arrayAbecedario.Length - 1].orden) b.valorUltimoBoton = arrayAbecedario[arrayAbecedario.Length - 1].orden;

                        if (coloritosFondo)
                        {
                            b.BackColor = this.coloresFondoElegidos.ElementAt(rnd.Next(0, this.coloresFondoElegidos.Count));
                        }
                        b.inicializarBoton();
                        tabla.Controls.Add(b, j, i);
                    }
                    else if (rejilla == TipoRejilla.WINGDINGS)
                    {
                        posicion = aleatorio.Next(0, this.arrayWingdings.Length);
                        while (arrayWingdings[posicion].escogido)
                        {
                            posicion = aleatorio.Next(0, arrayWingdings.Length);
                        }
                        arrayWingdings[posicion].escogido = true;

                        CMiBoton b = new CMiBoton(arrayWingdings[posicion].objeto);
                        FontFamily ff = new FontFamily(arrayWingdings[posicion].objeto.devolverFuente().Name);
                        b.Font = new Font(ff, size / 2);
                        b.Text = arrayWingdings[posicion].objeto.devolverLetra();
                        b.Size = new Size(size, size);

                        if (this.listaElementosBuscar.Contains(arrayWingdings[posicion].objeto))
                        {
                            if (emparejar)
                            {
                                this.listaWin.Add(arrayWingdings[posicion].objeto);
                                if (!this.listaWindingsTacharParejas.Contains(arrayWingdings[posicion].objeto))
                                {
                                    this.listaWindingsTacharParejas.Add(arrayWingdings[posicion].objeto);
                                }
                            }
                            this.numWingstotal++;
                        }

                        if (coloritosFondo)
                        {
                            //b.BackColor = this.coloresBackground.obtenerColor(rnd.Next(0, this.coloresBackground.cantidadColores() - 1));
                            b.BackColor = this.coloresFondoElegidos.ElementAt(rnd.Next(0, this.coloresFondoElegidos.Count));
                        }
                        b.inicializarBoton();
                        tabla.Controls.Add(b, j, i);
                    }
                }//for internno
            }//for externo

            lineaMolestaH = new PictureBox();
            lineaMolestaH.Visible = false;
            if (this.hayLinea)
            {
                lineaMolestaH.Visible = true;
                lineaMolestaH.BackColor = colorLinea;
                switch (tipoDeLinea)
                {
                    case tipoLinea.VERTICAL: lineaMolestaH.Size = new Size(grosorLinea, this.FindForm().Height);
                        this.lineaMolestaH.Location = new System.Drawing.Point(0, 0);
                        break;
                    case tipoLinea.HORIZONTAL: lineaMolestaH.Size = new Size(this.FindForm().Width, grosorLinea);
                        this.lineaMolestaH.Location = new System.Drawing.Point(0, 0);
                        break;
                    case tipoLinea.ALEATORIA:
                        redibujarLinea();
                        break;
                }
            }

            this.Controls.Add(this.lineaMolestaH);
            //para que el boton se vea en la parte derecha lo añadimos antes que la 
            //tabla par que salga encima, ya que la tabla ocupa toda la pantalla.

            //ordenar la lista de tachar las letras segun el orden de eleccion de las
            //letras en los botones
            if (emparejar)
            {
                if (this.rejilla == TipoRejilla.LETRAS)
                {
                    List<String> listaAuxiliar = new List<string>();
                    foreach (String s in listaLetrasTacharParejas)
                        listaAuxiliar.Add(s);

                    this.listaLetrasTacharParejas.Clear();
                    foreach (string s in this.listaLetrasTachar)
                        if (listaAuxiliar.Contains(s))
                            this.listaLetrasTacharParejas.Add(s);
                }
                if (this.rejilla == TipoRejilla.IMAGENES)
                {
                    List<Image> lAux = new List<Image>();
                    foreach (Image i in this.listaImagenesTacharParejas)
                        lAux.Add(i);

                    this.listaImagenesTacharParejas.Clear();

                    foreach (Image i in this.listaImagenesTachar)
                        if (lAux.Contains(i))
                            this.listaImagenesTacharParejas.Add(i);

                }
                if (this.rejilla == TipoRejilla.COLORES)
                {
                    List<Color> lAux = new List<Color>();
                    lAux = this.listaColoresTacharParejas.ToList();

                    this.listaColoresTacharParejas.Clear();

                    foreach (Color c in this.listaColoresTachar)
                        if (lAux.Contains(c))
                            this.listaColoresTacharParejas.Add(c);

                }
                if (this.rejilla == TipoRejilla.WINGDINGS)
                {
                    List<TipoLetraFuente> lAux = new List<TipoLetraFuente>();
                    lAux = this.listaWindingsTacharParejas.ToList();

                    this.listaWindingsTacharParejas.Clear();
                    foreach (TipoLetraFuente t in this.listaElementosBuscar)
                        if (lAux.Contains(t))
                            this.listaWindingsTacharParejas.Add(t);
                }

                //posicion del boton emparejar

                //bParejas.Location = new Point(ejeX, (this.tabla.Height - size) / 2);
                ejes = posicionEmparejar(size, coordenadaX, finTabla);
                bParejas.Location = new Point(ejes[0], ejes[1]);
                switch (rejilla)
                {
                    case TipoRejilla.NUMERICA:

                        bParejas.Font = new Font("Microsoft Sans Serif", (size / 4),
                                                      System.Drawing.FontStyle.Regular,
                                                      System.Drawing.GraphicsUnit.Point, ((byte)(0)));

                        switch (this.emparejarSecuencia)
                        {
                            case EmparejarSecuencia.SECUENCIAL:
                                this.bParejas.Text = this.primero.ToString();
                                break;
                            case EmparejarSecuencia.ALEATORIA:
                                this.bParejas.Text = new Random((int)DateTime.Now.Ticks).Next(primero, ultimo + 1).ToString();
                                break;
                            case EmparejarSecuencia.INVERSA:
                                this.bParejas.Text = ultimoValor.ToString();
                                break;
                        }
                        if (ultimoValor > 10 && ultimoValor < 100)
                        {
                            this.bParejas.Text = this.primero.ToString();
                            this.bParejas.Text = '0' + this.bParejas.Text;
                        }
                        else if (ultimoValor >= 100 && ultimoValor < 1000)
                        {
                            if (this.primero < 10)
                                bParejas.Text = "00" + primero.ToString();
                            else if (this.primero < 100)
                                bParejas.Text = "0" + primero.ToString();
                        }
                        break;
                    case TipoRejilla.COLORES:
                        //lista colores disponibles, es la lista con los colores de
                        //toda la plantilla
                        switch (this.emparejarSecuencia)
                        {
                            case EmparejarSecuencia.SECUENCIAL:
                                this.bParejas.BackColor = this.listaColoresTacharParejas.ElementAt(0);
                                break;
                            case EmparejarSecuencia.ALEATORIA:
                                this.bParejas.BackColor = this.listaColoresTacharParejas.ElementAt(new Random((int)DateTime.Now.Ticks).Next(0, this.listaColoresTacharParejas.Count));
                                break;
                            case EmparejarSecuencia.INVERSA:
                                this.bParejas.BackColor = this.listaColoresTacharParejas.ElementAt(this.listaColoresTacharParejas.Count - 1);
                                break;

                        }
                        break;
                    case TipoRejilla.LETRAS:
                        bParejas.Font = new Font(this.tipoLetra, size / 2);
                        switch (this.emparejarSecuencia)
                        {
                            case EmparejarSecuencia.SECUENCIAL:
                                this.bParejas.Text = this.listaLetrasTacharParejas.ElementAt(0);
                                break;
                            case EmparejarSecuencia.ALEATORIA:
                                this.bParejas.Text = this.listaLetrasTacharParejas.ElementAt(new Random((int)DateTime.Now.Ticks).Next(0, this.listaLetrasTacharParejas.Count));
                                break;
                            case EmparejarSecuencia.INVERSA:
                                this.bParejas.Text = this.listaLetrasTacharParejas.ElementAt(this.listaLetrasTacharParejas.Count - 1);
                                break;
                        }
                        break;
                    case TipoRejilla.IMAGENES:
                        bParejas.BackgroundImageLayout = ImageLayout.Stretch;
                        switch (this.emparejarSecuencia)
                        {
                            case EmparejarSecuencia.SECUENCIAL:
                                this.bParejas.BackgroundImage = this.listaImagenesTacharParejas.ElementAt(0);
                                break;
                            case EmparejarSecuencia.ALEATORIA:
                                this.bParejas.BackgroundImage = this.listaImagenesTacharParejas.ElementAt(new Random((int)DateTime.Now.Ticks).Next(0, this.listaImagenesTacharParejas.Count));
                                break;
                            case EmparejarSecuencia.INVERSA:
                                this.bParejas.BackgroundImage = this.listaImagenesTacharParejas.ElementAt(this.listaImagenesTacharParejas.Count - 1);
                                break;
                        }
                        break;
                    case TipoRejilla.ABECEDARIO:
                        bParejas.Font = new Font("Microsoft Sans Serif", (size / 6), //se divide entre 6 par que kepan todas las letras en una misma linea del boton
                                                 System.Drawing.FontStyle.Regular,
                                                 System.Drawing.GraphicsUnit.Point, ((byte)(0)));

                        switch (this.emparejarSecuencia)
                        {
                            case EmparejarSecuencia.SECUENCIAL:
                                valor = 0;
                                //this.bParejas.Text = this.listaAbecedarioTacharParejas.ElementAt(0);
                                break;
                            case EmparejarSecuencia.ALEATORIA:
                                valor = new Random().Next(0, this.arrayAbecedario.Length - 1);
                                //this.bParejas.Text = this.listaAbecedarioTacharParejas.ElementAt(new Random((int)DateTime.Now.Ticks).Next(0, this.listaAbecedarioTacharParejas.Count));
                                break;
                            case EmparejarSecuencia.INVERSA:
                                valor = this.arrayAbecedario.Length - 1;
                                //this.bParejas.Text = this.listaAbecedarioTacharParejas.ElementAt(this.listaAbecedarioTacharParejas.Count - 1);
                                break;
                        }
                        bParejas.Font = new Font("Microsoft Sans Serif", (size / 6), //se divide entre 6 par que kepan todas las letras en una misma linea del boton
                                          System.Drawing.FontStyle.Regular,
                                          System.Drawing.GraphicsUnit.Point, ((byte)(0)));

                        bParejas.Text = this.listaAbecedarioTacharParejas.ElementAt(valor);

                        break;
                    case TipoRejilla.WINGDINGS:
                        switch (this.emparejarSecuencia)
                        {
                            case EmparejarSecuencia.SECUENCIAL:
                                valor = 0;
                                break;
                            case EmparejarSecuencia.ALEATORIA:
                                valor = new Random((int)DateTime.Now.Ticks).Next(0, this.listaWindingsTacharParejas.Count);
                                break;
                            case EmparejarSecuencia.INVERSA:
                                valor = this.listaWindingsTacharParejas.Count - 1;
                                break;
                        }
                        FontFamily ff = new FontFamily(this.listaWindingsTacharParejas.ElementAt(valor).devolverFuente().Name);
                        bParejas.Font = new Font(ff, size / 2);
                        bParejas.Text = this.listaWindingsTacharParejas.ElementAt(valor).devolverLetra();
                        break;
                }


                this.Controls.Add(this.bParejas);
            }

            this.Controls.Add(this.tabla);

            //añadir aki el boton

            //limpiamos la memoria de objetos no usados.
            System.GC.Collect();
            /*
             * Una vez terminada de construir la tabla o matriz de botones
             * capturamos el reloj del sistema y mostramos la tabla
             */
            matrizTiempos[0] = DateTime.Now;
            if (emparejar)
                this.bParejas.capturarTiempo();

            this.ResumeLayout(false);

            this.actualizarCasillasTachar(numeroDeFilas, numeroDeColumnas);
        }//FIN INICIALIZA_TABLA


        private void Rejilla_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.temporizadorAleatorio != null)
                this.temporizadorAleatorio.Enabled = false;
            if (this.temporizadorLineaMolesta != null)
                this.temporizadorLineaMolesta.Enabled = false;
            if (this.temporizadorTiempoLimite != null)
                this.temporizadorTiempoLimite.Enabled = false;
            if (this.temporizadorBotonParejas != null)
                this.temporizadorBotonParejas.Enabled = false;
            //Parar las hebras adecuadamente.
            PararHiloLineaMolesta();
            PararHiloBotonesAleatorios();
            PararHiloBotonParejas();
            if (soloImprimir)
            {
                this.capturarEimprimir();
            }
        }

        public int casillasTotalesTachar()
        {
            return numeroCasillasTachar;
        }

        private void actualizarCasillasTachar(int f, int c)
        {
            if (this.rejilla == TipoRejilla.NUMERICA || this.rejilla == TipoRejilla.ABECEDARIO)
                this.numeroCasillasTachar = f * c;
            else if (this.rejilla == TipoRejilla.COLORES)
                this.numeroCasillasTachar = this.numColoresTotal;
            else if (this.rejilla == TipoRejilla.IMAGENES)
                this.numeroCasillasTachar = this.numImagenesTotal;
            else if (this.rejilla == TipoRejilla.LETRAS)
                this.numeroCasillasTachar = this.numLetrasTotal;
            else if (this.rejilla == TipoRejilla.WINGDINGS)
                this.numeroCasillasTachar = this.numWingstotal;
        }

        /*
         * Metodo para calcular la posicion del boton emparejar
         */

        private int[] posicionEmparejar(int alturaBotonEmparejar,int coordX, int fin_tabla)
        {
            int ejeX= 0, ejeY = 0;
            int coordenadaX = coordX;
            int finTabla = fin_tabla;
            PosicionBotonEmparejar p = this.position;
            int a = 0;
            if (this.position == PosicionBotonEmparejar.ALEATORIA)
            {
                a = new Random((int)DateTime.Now.Ticks).Next(0, 6);
                switch (a)
                {
                    case 0: p = PosicionBotonEmparejar.IZQUIERDA_CENTRO;
                        break;
                    case 1: p = PosicionBotonEmparejar.DERECHA_CENTRO;
                        break;
                    case 2: p = PosicionBotonEmparejar.IZQUIERDA_INFERIOR;
                        break;
                    case 3: p = PosicionBotonEmparejar.DERECHA_INFERIOR;
                        break;
                    case 4: p = PosicionBotonEmparejar.IZQUIERDA_SUPERIOR;
                        break;
                    case 5: p = PosicionBotonEmparejar.DERECHA_SUPERIOR;
                        break;
                }
                switch (a)
                {
                    case 0:
                    case 2:
                    case 4: this.izq_der = true;
                        break;
                    case 1:
                    case 3:
                    case 5: this.izq_der = false;
                        break;

                }
            }

            int[] localizacion = new int[]{0,0};
            if (p == PosicionBotonEmparejar.IZQUIERDA_SUPERIOR ||
                p == PosicionBotonEmparejar.IZQUIERDA_INFERIOR ||
                p == PosicionBotonEmparejar.IZQUIERDA_CENTRO)
            {
                ejeX = alturaBotonEmparejar + 5;
                if (coordenadaX > (ejeX + bParejas.Size.Width + 5))
                    ejeX = (coordenadaX - alturaBotonEmparejar) /2;
                else
                    ejeX = 0;
            }
            else if (p == PosicionBotonEmparejar.DERECHA_SUPERIOR ||
                     p == PosicionBotonEmparejar.DERECHA_INFERIOR ||
                     p == PosicionBotonEmparejar.DERECHA_CENTRO)
            {
                int auxiliar = (this.ClientSize.Width - finTabla) / 2;
                auxiliar = auxiliar - (alturaBotonEmparejar / 2);

                ejeX = finTabla + auxiliar;
                if (finTabla < ejeX)
                    ejeX = ejeX + 0;
                else
                    ejeX = this.ClientSize.Width - alturaBotonEmparejar - 5;
                //MessageBox.Show("ejeX " + ejeX + " posicion de la width de la tabla " + tabla.Width);
            }

            if (p == PosicionBotonEmparejar.IZQUIERDA_CENTRO ||
                p == PosicionBotonEmparejar.DERECHA_CENTRO)
                ejeY = (this.tabla.Height - alturaBotonEmparejar) / 2;
            else if (p == PosicionBotonEmparejar.IZQUIERDA_SUPERIOR ||
                     p == PosicionBotonEmparejar.DERECHA_SUPERIOR)
                //ejeY = this.tabla.Location.Y;
                ejeY = 5;//mejor el 5 por si se usan rejillas pequeñas de 3x3 con botones muy pequeños.
            else if (p == PosicionBotonEmparejar.IZQUIERDA_INFERIOR ||
                     p == PosicionBotonEmparejar.DERECHA_INFERIOR)
                //Aqui no se la altura de la tabla aun, porque no se ha rellenado
                //todavia con los botones, luego puede que no llegue 
                //a ocupar hasta tabla.height, pero no hay otra solucion.
                ejeY = this.tabla.Height - alturaBotonEmparejar;
            
            localizacion[0] = ejeX;
            localizacion[1] = ejeY;
            return localizacion;
        }
    }
}