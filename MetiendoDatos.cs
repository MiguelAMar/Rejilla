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
using System.IO;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows.Forms;
using System.Media;
using System.Drawing.Printing;
using System.Drawing.Drawing2D;
using Word = Microsoft.Office.Interop.Word; //No se usan 
using Excel = Microsoft.Office.Interop.Excel; // No se usan
using DocumentFormat.OpenXml.Packaging;
using word2 = DocumentFormat.OpenXml.Wordprocessing;
using excel2 = DocumentFormat.OpenXml.Spreadsheet;
using System.Threading;
using System.Resources;
using System.Runtime.InteropServices;
using System.Globalization;
using System.Runtime.Versioning;
using System.Diagnostics;
using System.ComponentModel.DataAnnotations;
using static demorejilla.Tachar;
using System.Text.Json;
using MessagePack;
using Microsoft.Data.Sqlite;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.IO.Compression;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;



namespace demorejilla
{
    [SupportedOSPlatform("windows")]
    public partial class MetiendoDatos : Form
    {

        //Comienzo de la rejilla numerica
        private String comienzoRejilla;
        //Tipo de rejilla que estamos usando
        private TipoRejilla rejillaTipo;
        //incremento que toma los botones de la rejilla, de 1 en 1, 2 en 2.... 
        public int incrementoNext;
        //tamaño de los botones de la rejilla
        private int tamañoBoton;
        /*
         * Tiempos en la rejilla y dibujar la grafica.
         */

        /*
         * Array que contiene el tiempo (en TimeSpan) que tarda el usuario en tachar
         * cada botón. Este array se pasa a float y nos da el array tiempoSegundos
         * que es el que se dibuja en la parte gráfica.
         */
        private TimeSpan[] tiempos;
        private float[] tiempoSegundos;
        /*
         * Variable que indica si se ha tachado algún botón para dibujar la gráfica
         * y habilitar el botón guardar
         */
        public bool lienzoinicial;
        /*
         * Variables para guardar en el fichero los tiempos de la gráfica
         * y los tiempos de latencia (emparejamiento)
         */
        private DateTime[] guardarTiemposGrafica;
        private TimeSpan[] guardarTiemposEmparejamiento;
        /*
         * Variables para los tiempos de la parte gráfica, tarea preliminar,
         * tiempo corregido, y la correción
         */
        private TimeSpan tiempoTareaInicial;//tiempo de la tarea preliminar
        private TimeSpan tiempoCorregido;//tiempo total menos la corrección
        private TimeSpan tiempoCorreccionEfectuada;//corrección efectuada
        /*
         * Variables Distraccion Linea
         */
        private bool radioButonSeleccionado;//indica si hay que dibujar la linea
        private tipoLinea linea;//si es horizontal, vertical, aleatoria, sin linea
        private Color color;//color de la linea
        private int grosor;//grosor
        private int[] tipoGrosores;//array de grosores de la linea
        private double velocidadLinea;//velocida a la que se mueve la linea
        /*
         * Tiempos Límite y Aleatorio 
         */
        private int tiempoLimite;
        private int timeButton;
        /*
         * Distracción música
         */
        private System.Timers.Timer temporizadorMetronomo;
        private SoundPlayer winamp = new SoundPlayer(); //para el metronomo
        private string /*rutaFicheroTmp,*/ rutaFicheroM3u;
        public bool loopCanciones;
        public string rutaCarpeta;

        //2024
        private string rutaCarpetaMusica;
        private string rutaCarpetaDocuementos;
        private string rutaCarpetaDB;
        private string rutaCarpetaBackUp;
        private string rutaCarpetaSonidos;
        // FIN 2024
        /*
         * Rejilla de Colores
         * Lista de Colores a tachar en la rejilla.
         * Cantidad mínima de botones a tachar (0%, 25%, 50%, 75%)
         */
        private List<Color> listaBotonesColores;
        private TantoPorCiento minimoCantidadBotonesColores;
        /*
         * Plantilla de colores, ya sea para los colores de fondo de los botones
         * o para la rejilla de colores.
         */
        private ClaseColores plantillaColoresRejilla;
        private List<Color> listaColoresElegidosParaElFondo;
        /*
         * Rejilla de letras
         */
        private List<String> listaLetras;
        private List<String> listaLetrasTachar;
        private FontFamily fuenteFamilia;
        private TantoPorCiento minimoLetras;
        /*
         * Rejilla de imagenes
         */
        private List<Image> listaImagenes;
        private List<Image> listaImagenesEscogidas;
        private TantoPorCiento minimoCantidadBotonesImagenes;
        private ClaseImagen imagenes;
        /*
         * Rejilla Abecedario
         * Opciones guarda si en mayusculas, minusculas, alternando...
         * ordenTachado si es FALSE => descendnete, Si TRUE => ascendente
         */
        private bool[] opcionesAbecedario;
        private bool ordenTachadoAbc;
        /*
         * Rejilla de Wingdings
         */
        private List<TipoLetraFuente> listaCompletaObjetos;
        private List<TipoLetraFuente> listaSeleccionados;
        private TantoPorCiento minimoWin;
        /*
         * Imprimir
         */
        private bool pantalla = false;
        private bool informe = false;
        private Bitmap imagen, imagenGrafica;
        private int totalLineasImpresas;
        private Font fuenteParaImprimir;
        private String[] cadenaTroceada;//Cadena con el informe
        private bool noGrafica = false;
        private bool informeMasGrafica = true;
        /*
         * Variables para la linea de la grafica con el raton
         */
        bool botonPulsado = false;
        float x1, x2, y1, y2;
        /*
         * Variables para las observaciones
         */
        private String observaciones;
        /*
         * Variables para programar los colores y las letras
         */
        private bool[] vectorProgramarColores;//aleatoridad 
        private bool tacharColor;
        private bool tacharTexto;
        private bool tacharNada;
        private bool tacharTodo;
        /*
         * Variables para el control de tachado
         */
        private SortedList<String, DatosControlTachado> listaDatosControlTachado;
        private List<String> ordenPulsacion;
        private String verBotonRbNombre;
        private int aciertos, errores;
        private int cantidadBotones;//los botones totales que hay q tachar en la rejilla
        /*
         * Variables para la expotacion de datos
         */
        private Word.Application aplicacionWord;
        private Word.Document doc;
        private object opc = Type.Missing;
        private Excel.Application apExcel;
        private Excel.Workbook libro;
        private int fila;//fila de excel por la que vamos reyenando datos
        private int contadorProgresBar;
        /*
        * Variable para guardar el nombre del fichero Excel, cuando se exportan los datos, ya que hay que abrirlo y cerrarlo varias veces al
        * haber cambiado de COM a Open XML SDK.
        */
        private string nombreFicheroExcel;
        /*
         * Variable para el idioma
         */
        private ResourceManager rm;
        /*
         * Variable para modificar las observaciones en el fichero de datos
         */
        private string fichero;
        /*
         * Variables género y lateralidad
         */
        private Genero sexo;
        private Lateralidad later;
        /*
         * Variables para el emparejamiento, orden emparejamiento (secuencial, inverso, aleatorio)
         * y posicion del boton (izquierda o derecha) y tiempo interacmbio
         */
        private int tiempoEmparejar;
        private EmparejarSecuencia secuencia;
        private PosicionBotonEmparejar posicion;
        private bool hayParejas;
        /*
         * Variables para la BD
         * 
         */
        private int codigoUsuarios;
        private int codigoAbecedario;
        private int codigoColores;
        private int codigoColoresFondo;
        private int codigoEmparejar;
        private int codigoImagenes;
        private int codigoLetras;
        private int codigoLineaDistractora;
        private int codigoNumerica;
        private int codigoRejilla;
        private int codigoSnidos;
        private int codigoWindings;
        private int codigoControlTachado;
        private int codigoObservaciones;
        private BaseDatos BaseDatosPrograma;
        private DataGridViewRow filaRejillaEscogida;
        private bool datosCargadosBD;
        private Font fuenteBD;
        private bool datosWinCargadosBD;
        private bool observacionesBD;

        /*
         * ***************************************************************
         * CONSTRUCTORES
         * ***************************************************************
         */

        /*
         * Procedimiento que crea la ventana inicial para introducir los datos
         * y crear la rejilla de botones. Inicializa las variables.
         */
        public MetiendoDatos()
        {

            rm = new ResourceManager("demorejilla.Recursos", typeof(MetiendoDatos).Assembly);
            rutaCarpeta = new CarpetaMisDocumentos().crearCarpeta("rejilla");
            this.rutaCarpetaDocuementos = new CarpetaLocal().carpetaDocumentos();
            this.rutaCarpetaDB = new CarpetaLocal().carpetaBD();
            this.rutaCarpetaMusica = new CarpetaLocal().carpetaMusica();
            this.rutaCarpetaBackUp = new CarpetaLocal().carpetaBackUp();
            this.rutaCarpetaSonidos = new CarpetaLocal().carpetaSonidos();
            crearBaseDatos();

            //por si no se puede crear el directorio en la carpeta mis documentos.
            if (rutaCarpeta == null || rutaCarpeta == "")
            {
                String directorioTrabajo = Directory.GetCurrentDirectory();
                this.rutaFicheroM3u = directorioTrabajo + Path.DirectorySeparatorChar + "music" + Path.DirectorySeparatorChar + "musikalizate.m3u";
            }
            else //SE van a guardar los ficheros de musica en la carpeta musica dentro del ejecutable
            {
                //this.rutaFicheroM3u = this.rutaCarpeta + Path.DirectorySeparatorChar + "musikalizate.m3u";
                this.rutaFicheroM3u = this.rutaCarpetaMusica + "musikalizate_" + DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss") + ".m3u";
            }
            lienzoinicial = false;
            InitializeComponent();
            this.etTiempoTranscurridoEmparejar.Visible = false;
            this.etValorMaximoEmparejar.Visible = false;
            this.etValorMediaEmparejar.Visible = false;
            this.etValorMinimoEmparejar.Visible = false;
            this.etVarianzaEmparejar.Visible = false;

            incrementoNext = 1;  //por defecto los numeros iran de 1 en 1
            tamañoBoton = 40;    //por defecto el tamaño del boton es el pequeño;
            linea = tipoLinea.SINLINEA; //por defecto sin linea
            radioButonSeleccionado = false;
            color = Color.Empty;
            tipoGrosores = new int[15] { 20, 30, 40, 45, 50, 55, 60, 65, 70, 75, 80, 85, 95, 150, 155 };
            grosor = tipoGrosores[0]; //el grosor por defecto es el minimo de todos 
            this.velocidadLinea = 125;
            rejillaTipo = TipoRejilla.NUMERICA;
            listaBotonesColores = new List<Color>();
            this.minimoCantidadBotonesColores = TantoPorCiento.CERO;
            listaLetras = new List<string>();
            listaLetrasTachar = new List<string>();
            this.fuenteFamilia = this.Font.FontFamily;
            this.minimoLetras = TantoPorCiento.CERO;
            this.rbLetrasCero.Checked = true;

            this.listaImagenesEscogidas = new List<Image>();

            this.imagenes = new ClaseImagen();
            this.imagenes.plantillaInicial();
            this.minimoCantidadBotonesImagenes = TantoPorCiento.CERO;
            this.listaImagenes = imagenes.obtenerTodasImagenes();

            this.opcionesAbecedario = new bool[5];
            for (int i = 0; i < this.opcionesAbecedario.Length; i++)
                opcionesAbecedario[i] = false;
            this.opcionesAbecedario[0] = true;

            this.ordenTachadoAbc = true;

            this.tbFecha.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

            this.tiempoCorreccionEfectuada = new TimeSpan();
            this.tiempoCorregido = new TimeSpan();
            this.tiempoTareaInicial = new TimeSpan();

            this.listaCompletaObjetos = new List<TipoLetraFuente>();
            this.listaSeleccionados = new List<TipoLetraFuente>();
            this.minimoWin = TantoPorCiento.CERO;

            this.plantillaColoresRejilla = new ClaseColores();
            this.plantillaColoresRejilla.plantillaInicial();

            this.listaColoresElegidosParaElFondo = new List<Color>();

            this.observaciones = "";

            this.vectorProgramarColores = new bool[] { true, false, false };
            this.tacharColor = false;
            this.tacharTexto = true;
            this.tacharNada = false;
            this.tacharTodo = false;
            this.verBotonRbNombre = this.rbAscendente.Name;

            this.loopCanciones = false;
            this.aciertos = this.errores = 0;
            this.cantidadBotones = 0;

            this.comienzoRejilla = "";
            this.fila = 0;

            this.sexo = Genero.MASCULINO;
            this.later = Lateralidad.DERECHA;

            this.tiempoEmparejar = 1;
            this.contadorProgresBar = 1;

            this.nombreFicheroExcel = "";

            inicializarCodigosTablas();
            inicialiarUsuarioRejilla();
            this.filaRejillaEscogida = null;
            this.datosCargadosBD = false;
            this.datosWinCargadosBD = false;
            this.observacionesBD = false;
            if (cbNoGuardarBD.Checked)
                this.guardarToolStripMenuItem.Enabled = true;
            else
                this.guardarToolStripMenuItem.Enabled = true;
        }

        private void actualizarUsuarioDefecto()
        {
            BaseDatos bd = new BaseDatos();
            bd.conectarBD();
            bd.actualizarIndice("CodigoUltimoUsuario", Convert.ToInt32(this.tbCodigoUsuario.Text));
            bd.cerrarConexiion();

        }
        private void inicialiarUsuarioRejilla()
        {
            BaseDatos bd = new BaseDatos();
            bd.conectarBD();
            int codigo = Convert.ToInt32(bd.obtenerIndice("CodigoUltimoUsuario").ToString());
            SqliteDataReader readerUsuario = bd.obtenerDatosUsuarioPorPK(codigo);

            while (readerUsuario.Read())
            {
                tbCodigoUsuario.Text = readerUsuario["Codigo"].ToString();
                tbNombre.Text = readerUsuario["Nombre"].ToString();
                tbApellidos.Text = readerUsuario["Apellidos"].ToString();
                tbEdad.Text = readerUsuario["Edad"].ToString();
                tbPais.Text = readerUsuario["Pais"].ToString();
                this.sexo = devolverGeneroFichero(readerUsuario["Genero"].ToString());
                marcarCampoSexo();
                this.later = devolverLateralidadFichero(readerUsuario["Lateralidad"].ToString());
                marcarCampoLAT();
                this.tbDeporte.Text = readerUsuario["Deportes"].ToString();
                this.tbPosicion.Text = readerUsuario["Posicion"].ToString();
            }
            bd.cerrarConexiion();
            readerUsuario.Close();
        }

        private void inicializarCodigosTablas()
        {
            this.codigoAbecedario = 0;
            this.codigoColores = 0;
            this.codigoColoresFondo = 0;
            this.codigoEmparejar = 0;
            this.codigoImagenes = 0;
            this.codigoLetras = 0;
            this.codigoLineaDistractora = 0;
            this.codigoNumerica = 0;
            this.codigoRejilla = 0;
            this.codigoSnidos = 0;
            this.codigoUsuarios = 0;
            this.codigoWindings = 0;
            this.codigoControlTachado = 0;
            this.codigoObservaciones = 0;
        }

        /*
         * Procedimiento que se ejecuta al hacer click en el boton btCrearRejilla
         * Lo que hace es generar un formulario nuevo con la rejilla de botones, leyendo
         * los datos de la ventana actual.
         * 
         */
        private void btcrearRejilla_Click(object sender, EventArgs e)
        {
            int a, b, c, d, ee;
            //ResourceManager rm = new ResourceManager("demorejilla.Recursos", typeof(MetiendoDatos).Assembly);
            try
            {
                comprobarQueesUnNumero(textBoxNumFila.Text);
                a = System.Convert.ToInt32(textBoxNumFila.Text);

                comprobarQueesUnNumero(textBoxNumColumnas.Text);
                b = System.Convert.ToInt32(textBoxNumColumnas.Text);

                //c es el valor inicial del campo de la rejilla numerica
                c = System.Convert.ToInt32(textBoxValorIncial.Text);
                d = incrementoNext; //por defecto 1
                ee = tamañoBoton;  //por defecto el tamaño pequeño 40x40;

                //Control de errores que compruebea si caben todos los botones en la pantalla de dibujo
                if (b * (ee + 5) > SystemInformation.PrimaryMonitorSize.Width)//this.ClientSize.Width)
                    //throw new RejillaException("Error Tamaño: No caben todas las columnas en pantalla, indique menos columnas o eliga un botón más pequeño. ");          
                    throw new RejillaException(rm.GetString("error01"));

                if (a * (ee + 5) > SystemInformation.PrimaryMonitorSize.Height - 100)//this.ClientSize.Height)
                    //throw new RejillaException("Error Tamaño: No caben todas las filas en pantalla, indique menos filas o eliga un botón más pequeño.");
                    throw new RejillaException(rm.GetString("error02"));

                if (checkBoxAleatorio.Checked && textBoxTiempoSegundosAleatorio.Text == "")
                    //throw new RejillaException("Error Tiempo: Falta el valor del tiempo Aleatorio");
                    throw new RejillaException(rm.GetString("error03"));

                if (checkBoxAleatorio.Checked)
                {
                    this.timeButton = System.Convert.ToInt32(textBoxTiempoSegundosAleatorio.Text);
                    if (this.timeButton == 0)
                        //throw new RejillaException("ERROR ALEATORIO: El valor de Aleatorio tiene que ser mayor MAYOR CERO.");
                        throw new RejillaException(rm.GetString("error30"));
                }

                if (cbTiempoLimite.Checked && tbTiempoLimite.Text == "")
                    //throw new RejillaException("Error Tiempo: Falta el valor del tiempo Límite.");
                    throw new RejillaException(rm.GetString("error20"));

                if (cbTiempoLimite.Checked)
                {
                    this.tiempoLimite = Convert.ToInt32(tbTiempoLimite.Text);
                    if (this.tiempoLimite == 0)
                        //throw new RejillaException("ERROR TIEMPO LÍMITE: El valor del tiempo límite tiene que ser MAYOR que CERO");
                        throw new RejillaException(rm.GetString("error31"));
                }

                if (this.rejillaTipo == TipoRejilla.COLORES && this.listaBotonesColores.Count == 0)
                    //throw new RejillaException("Error Colores: No se han escogido los colores que hay que tachar.");
                    throw new RejillaException(rm.GetString("error04"));

                if (this.rejillaTipo == TipoRejilla.LETRAS && this.listaLetras.Count == 0)
                    //throw new RejillaException("Error Letras: La lista de Items no puede ser vacia.");
                    throw new RejillaException(rm.GetString("error05"));

                if (this.rejillaTipo == TipoRejilla.LETRAS && this.listaLetrasTachar.Count == 0)
                    //throw new RejillaException("Error Letras: No se ha seleccionado letras para tachar, seleccione al menos una.");
                    throw new RejillaException(rm.GetString("error06"));

                if (this.rejillaTipo == TipoRejilla.ABECEDARIO && this.tbAbecedario.Text == "")
                    //throw new RejillaException("Error Abecedario: No se ha indicado un valor inicial para el Abecedario.");
                    throw new RejillaException(rm.GetString("error07"));

                if (this.rejillaTipo == TipoRejilla.IMAGENES && this.listaImagenesEscogidas.Count == 0)
                    //throw new RejillaException("Error Imágenes: No hay seleccionada ninguna imagen para tachar en la rejilla.");
                    throw new RejillaException(rm.GetString("error08"));

                if (this.rejillaTipo == TipoRejilla.WINGDINGS && this.listaCompletaObjetos.Count == 0)
                    //throw new RejillaException("Error Wingdings: No se ha seleccionado ningun item para crear la rejilla.");
                    throw new RejillaException(rm.GetString("error09"));

                if (this.rejillaTipo == TipoRejilla.WINGDINGS && this.listaSeleccionados.Count == 0)
                    //throw new RejillaException("Error Wingdings: No se ha seleccionado ningun item para tachar en la rejilla.");
                    throw new RejillaException(rm.GetString("error10"));

                if ((this.rejillaTipo == TipoRejilla.NUMERICA) && (this.rbAscendente.Checked) && (1000 - Convert.ToInt32(this.textBoxValorIncial.Text) <= ((a * b) - 1) * this.incrementoNext))
                {
                    this.gbErrorNumerico.Enabled = true;
                    this.gbErrorNumerico.Text = rm.GetString("cad02");// "Valor Máximo";
                    this.etErrorNumerico.Enabled = true;
                    this.etErrorNumerico.Text = (999 - ((a * b) - 1) * this.incrementoNext).ToString();
                    //throw new RejillaException("Error fuera de rango: El rango de valores de los botones de la rejilla es de 0 a 999. "
                    //    + "Decremente el valor inicial y/o disminuya el valor del incremento. \n"
                    //    + "SUGERENCIA: Manteniendo el incremento usar como valor inicial: " + this.etErrorNumerico.Text);
                    throw new RejillaException(rm.GetString("error11") + this.etErrorNumerico.Text);
                }

                if (this.rejillaTipo == TipoRejilla.NUMERICA && this.rbDescendente.Checked && (0 > (c - (((a * b) - 1) * this.incrementoNext))))
                {
                    this.gbErrorNumerico.Enabled = true;
                    this.gbErrorNumerico.Text = rm.GetString("cad01");//"Valor Mínimo";
                    this.etErrorNumerico.Enabled = true;
                    this.etErrorNumerico.Text = (((a * b) - 1) * this.incrementoNext).ToString();
                    //throw new RejillaException("Error fuera de rango: El rango de valores de los botones de la rejilla es de 0 a 999. "
                    //    + "Incremente el valor inicial y/o disminuya el valor del decremento. \n" 
                    //    + "SUGERENCIA: Manteniendo el decremento usar como valor inicial: " + this.etErrorNumerico.Text);
                    throw new RejillaException(rm.GetString("error12") + this.etErrorNumerico.Text);
                }

                this.gbErrorNumerico.Enabled = false;
                this.gbErrorNumerico.Text = "";
                this.etErrorNumerico.Enabled = false;
                this.etErrorNumerico.Text = "";

                if (this.rejillaTipo == TipoRejilla.COLORES && minimoCantidadBotonesColores == TantoPorCiento.CERO)
                {
                    foreach (Color clr in this.listaBotonesColores)
                    {
                        if (!this.plantillaColoresRejilla.contieneColor(clr))
                        {
                            //if (MessageBox.Show("Hay colores para tachar que no pertenecen a la plantilla de colores. Estos colores no saldrán en la rejilla final.", "ADVERTENCIA", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                            //    throw new RejillaException("Elimine los colores que no pertenecen a la plantilla o eliga una cantidad de botones (%) a tachar.");
                            if (MessageBox.Show(rm.GetString("error13"), rm.GetString("war00"), MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                                throw new RejillaException(rm.GetString("error14"));

                            break;
                        }
                    }
                }

                if (this.rejillaTipo != TipoRejilla.IMAGENES && this.rejillaTipo != TipoRejilla.COLORES)
                {
                    if (this.checkBoxAleatorio.Checked && (this.vectorProgramarColores[1] || this.vectorProgramarColores[2]))
                    {
                        if (this.listaColoresElegidosParaElFondo.Count == 0)
                            //throw new RejillaException("Esta programado que se intercambie el color y/o texto y no hay colores de fondo seleccionado para los botones.");
                            throw new RejillaException(rm.GetString("error27"));
                        else if (!this.cbColoresFondo.Checked)
                            //throw new RejillaException("Esta programado que se intercambie el color y/o texto y no esta seleccionada la casilla para los colores de fondo. Selecciona esta casilla o modifique las opciones de programación de la rejilla.");
                            throw new RejillaException(rm.GetString("error29"));
                    }
                }

                if (this.cbColoresFondo.Checked)
                {
                    if (this.rejillaTipo == TipoRejilla.IMAGENES || this.rejillaTipo == TipoRejilla.COLORES)
                    {
                        //throw new RejillaException("Error Colores de Fondo: No se puede usar con la rejilla de imágenes. Eliga otro tipo de rejilla o desactive los colores de fondo.");
                        throw new RejillaException(rm.GetString("error15"));
                    }
                    else
                    {
                        if (this.listaColoresElegidosParaElFondo.Count == 0)
                            //throw new RejillaException("Error Colores de Fondo: No hay ningun color seleccionado para el fondo de la rejilla.");
                            throw new RejillaException(rm.GetString("error16"));
                    }
                }
                this.reproductorWindowsMedia.Ctlcontrols.stop();

                /*  
                 * si la rejilla es descendente calculo el valor inicial
                 * desde el que construir la rejilla a partir del tamaño de la rejilla
                 */
                if (this.rbDescendente.Checked)
                {
                    int producto = ((a * b - 1) * d);
                    int vf = c;
                    c = vf - producto;
                }
                /*
                 * Si tengo que imprimir la rejilla la relleno y la capturo e imprimo
                 * como si fuera un formulario inicial.
                 */
                if (this.cbImprimir.Checked)
                {

                    Rejilla rejillaImprimir = new Rejilla(a, b, c, d, ee,  //variables para crear la rejilla
                        this.rejillaTipo,  //tipo de rejilla: numerica,colores,imagenes,letras
                        this.plantillaColoresRejilla, this.listaBotonesColores, this.minimoCantidadBotonesColores, //colores a tachar de la lista
                        this.listaLetras, this.listaLetrasTachar, this.fuenteFamilia, this.minimoLetras,//letras de la rejilla
                        this.listaImagenesEscogidas, this.listaImagenes, this.minimoCantidadBotonesImagenes, // imagenes de la rejilla
                        this.tbAbecedario.Text, this.opcionesAbecedario, this.ordenTachadoAbc,//this.minusculas, //para el abecedario de la rejilla
                        this.listaCompletaObjetos, this.listaSeleccionados, this.minimoWin, // rejilla de Wingdings
                        2,// tiempo para cerrar la rejilla
                        this.cbColoresFondo.Checked, this.listaColoresElegidosParaElFondo); //para añadir colores de fondo, lista colores elegidos

                    rejillaImprimir.ShowDialog();
                }
                else //Sino imprimimos la rejilla
                {
                    if (this.rbAscendente.Checked)
                        this.verBotonRbNombre = this.rbAscendente.Name;
                    else
                        this.verBotonRbNombre = this.rbDescendente.Name;

                    this.etValorTiempoTareaPreliminar.Text = "00:00.000";
                    //Si hay tarea preliminar la ejecutamos
                    if (cbTareaPreliminar.Checked)
                    {
                        Rejilla red = new Rejilla(a, b, ee);
                        red.ShowDialog();
                        this.tiempoTareaInicial = red.boton2 - red.boton1;
                        this.etValorTiempoTareaPreliminar.Text = FormatTimeSpan(this.tiempoTareaInicial);
                    }
                    //Si hay metronomo
                    if (this.rbMetronomo.Checked)
                    {
                        if (tbMetronomo.Text.Length == 0)
                            //throw new RejillaException("Error Metronomo: No se ha introducido el valor del metrónomo");
                            throw new RejillaException(rm.GetString("error17"));
                        else
                        {
                            temporizadorMetronomo.Interval = System.Convert.ToInt32(tbMetronomo.Text) * 100;
                            temporizadorMetronomo.Enabled = true;
                            winamp.Stream = global::demorejilla.Properties.Resources.Beep;
                            //winamp.SoundLocation = "..\\..\\Resources\\Beep.wav";
                            winamp.Play();
                        }
                    }
                    //Si hay música
                    if (this.rbMusica.Checked)
                    {
                        if (this.listaMusica.Items.Count == 0)
                            //throw new RejillaException("Error Música: La lista de música no puede estar vacía");
                            throw new RejillaException(rm.GetString("error18"));
                        else
                        {
                            crearFicheroReproduccion();
                            this.reproductorWindowsMedia.URL = this.rutaFicheroM3u;
                            this.loopCanciones = true;
                            this.reproductorWindowsMedia.Ctlcontrols.play();
                            this.reproductorWindowsMedia.settings.setMode("loop", true);
                        }
                    }

                    //Capturamos la fecha y hora y lo ponemos en fecha
                    this.tbFecha.Text = DateTime.Now.ToString();

                    //Valor inicial de la rejilla numérica
                    this.comienzoRejilla = this.textBoxValorIncial.Text;



                    //si tengo que emparejar ordeno la lista de imagenes,
                    // y la lista de colores para el orden secuencial.
                    if (cbEmparejar.Checked)
                    {
                        if (this.rejillaTipo == TipoRejilla.IMAGENES)
                        {
                            this.listaImagenesEscogidas.Clear();
                            if (btImagen1.BackgroundImage != null)
                                this.listaImagenesEscogidas.Add(btImagen1.BackgroundImage);
                            if (btImagen2.BackgroundImage != null)
                                this.listaImagenesEscogidas.Add(btImagen2.BackgroundImage);
                            if (btImagen3.BackgroundImage != null)
                                this.listaImagenesEscogidas.Add(btImagen3.BackgroundImage);
                            if (btImagen4.BackgroundImage != null)
                                this.listaImagenesEscogidas.Add(btImagen4.BackgroundImage);
                            if (btImagen5.BackgroundImage != null)
                                this.listaImagenesEscogidas.Add(btImagen5.BackgroundImage);
                            if (btImagen6.BackgroundImage != null)
                                this.listaImagenesEscogidas.Add(btImagen6.BackgroundImage);
                            if (btImagen7.BackgroundImage != null)
                                this.listaImagenesEscogidas.Add(btImagen7.BackgroundImage);
                            if (btImagen8.BackgroundImage != null)
                                this.listaImagenesEscogidas.Add(btImagen8.BackgroundImage);
                            if (btImagen9.BackgroundImage != null)
                                this.listaImagenesEscogidas.Add(btImagen9.BackgroundImage);
                        }
                        else if (this.rejillaTipo == TipoRejilla.COLORES)
                        {
                            this.listaBotonesColores.Clear();
                            if (btColor1.BackColor != SystemColors.Control)
                                this.listaBotonesColores.Add(btColor1.BackColor);
                            if (btColor2.BackColor != SystemColors.Control)
                                this.listaBotonesColores.Add(btColor2.BackColor);
                            if (btColor3.BackColor != SystemColors.Control)
                                this.listaBotonesColores.Add(btColor3.BackColor);
                            if (btColor4.BackColor != SystemColors.Control)
                                this.listaBotonesColores.Add(btColor4.BackColor);
                            if (btColor5.BackColor != SystemColors.Control)
                                this.listaBotonesColores.Add(btColor5.BackColor);
                            if (btColor6.BackColor != SystemColors.Control)
                                this.listaBotonesColores.Add(btColor6.BackColor);
                            if (btColor7.BackColor != SystemColors.Control)
                                this.listaBotonesColores.Add(btColor7.BackColor);
                            if (btColor8.BackColor != SystemColors.Control)
                                this.listaBotonesColores.Add(btColor8.BackColor);
                            if (btColor9.BackColor != SystemColors.Control)
                                this.listaBotonesColores.Add(btColor9.BackColor);
                            if (btColor10.BackColor != SystemColors.Control)
                                this.listaBotonesColores.Add(btColor10.BackColor);
                            if (btColor11.BackColor != SystemColors.Control)
                                this.listaBotonesColores.Add(btColor11.BackColor);
                            if (btColor12.BackColor != SystemColors.Control)
                                this.listaBotonesColores.Add(btColor12.BackColor);
                        }
                        else if (this.rejillaTipo == TipoRejilla.LETRAS)
                        {
                            this.listaLetrasTachar.Clear();
                            if (btLetras1.Text != "")
                                this.listaLetrasTachar.Add(btLetras1.Text);
                            if (btLetras2.Text != "")
                                this.listaLetrasTachar.Add(btLetras2.Text);
                            if (btLetras3.Text != "")
                                this.listaLetrasTachar.Add(btLetras3.Text);
                            if (btLetras4.Text != "")
                                this.listaLetrasTachar.Add(btLetras4.Text);
                            if (btLetras5.Text != "")
                                this.listaLetrasTachar.Add(btLetras5.Text);
                            if (btLetras6.Text != "")
                                this.listaLetrasTachar.Add(btLetras6.Text);
                            if (btLetras7.Text != "")
                                this.listaLetrasTachar.Add(btLetras7.Text);
                            if (btLetras8.Text != "")
                                this.listaLetrasTachar.Add(btLetras8.Text);
                            if (btLetras9.Text != "")
                                this.listaLetrasTachar.Add(btLetras9.Text);
                        }
                    }
                    //Creamos la rejilla
                    Rejilla f = new Rejilla(a, b, c, d, ee,  //variables para crear la rejilla
                        checkBoxAleatorio.Checked, this.timeButton, //variables para aleatorizar los botones
                        this.linea, this.radioButonSeleccionado, this.color, this.grosor, this.velocidadLinea, //variables para la distraccion linea
                        this.tiempoLimite, cbTiempoLimite.Checked, //tiempoLimite para realizar la rejilla
                        this.rejillaTipo,  //tipo de rejilla: numerica,colores,imagenes,letras
                        this.plantillaColoresRejilla, this.listaBotonesColores, this.minimoCantidadBotonesColores, //colores a tachar de la lista
                        this.listaLetras, this.listaLetrasTachar, this.fuenteFamilia, this.minimoLetras, //letras de la rejilla
                        this.listaImagenesEscogidas, this.listaImagenes, this.minimoCantidadBotonesImagenes, // imagenes de la rejilla
                        this.tbAbecedario.Text, this.opcionesAbecedario, this.ordenTachadoAbc, // this.minusculas, //para el abecedario de la rejilla
                        this.listaCompletaObjetos, this.listaSeleccionados, this.minimoWin, // rejilla de Wingdings
                        this.cbColoresFondo.Checked, this.listaColoresElegidosParaElFondo, //para añadir colores de fondo
                        this.vectorProgramarColores, this.tacharColor, this.tacharTexto, this.tacharNada, this.tacharTodo, this.rbDescendente.Checked, //para programar el aleatorio y le tachado de los botones. Y la ultima para tachar en orden inverso
                        this.cbEmparejar.Checked, this.posicion, this.secuencia, this.tiempoEmparejar);
                    f.ShowDialog();
                    /*
                     * Cuando se pulsa el ultimo boton y se cierra el formulario
                     * el programa sigue la ejecución por este punto, asi que justo 
                     * cuando pulsamos el ultimo boton, o si se cierra el formulario
                     * de la rejilla, habiendo pulsado un boton, por lo menos, 
                     * se crea la gráfica de los tiempos tardados en tachar los botones
                     * con la media, y los tiempos medidos totales. 
                     */

                    this.loopCanciones = false;
                    this.reproductorWindowsMedia.Ctlcontrols.stop();
                    if (this.temporizadorMetronomo != null)
                        this.temporizadorMetronomo.Enabled = false;

                    lienzoinicial = f.pulsadoBoton;
                    this.guardarToolStripMenuItem.Enabled = f.pulsadoBoton;
                    dibujarGrafica(lienzoinicial, f.matrizTiempos, this.rbGrafica.Checked, f.matrizTimeEmparejar);

                    this.guardarTiemposGrafica = new DateTime[f.matrizTiempos.Length];
                    f.matrizTiempos.CopyTo(this.guardarTiemposGrafica, 0);

                    this.guardarTiemposEmparejamiento = new TimeSpan[f.matrizTimeEmparejar.Length];
                    f.matrizTimeEmparejar.CopyTo(this.guardarTiemposEmparejamiento, 0);

                    this.listaDatosControlTachado = f.listaControl;
                    this.ordenPulsacion = f.listaOrdenPulsacíon;

                    this.btControlTachado.Enabled = f.pulsadoBoton;

                    this.aciertos = f.aciertos;
                    this.errores = f.fallos;
                    this.cantidadBotones = f.casillasTotalesTachar();
                }


                //2024 Guardar en la base de datos automaticamente
                if (!cbNoGuardarBD.Checked)
                {
                    guardarEnBaseDeDatos();
                }

            }
            catch (RejillaException re)
            {
                MessageBox.Show(re.Message, rm.GetString("war04") /*"Mensaje"*/, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            catch (FormatException fe)
            {
                MessageBox.Show(fe.Message + rm.GetString("error19"));//"Error Inesperado.");
            }
        }//FIN btcrearRejilla_Click


        /*
         * 2024 - Método para guardar los datos en la base de datos. 
         * 
         */

        private void guardarUsuario()
        {
            BaseDatos bd = new BaseDatos();
            int codigoUsuario = -1;
            bd.conectarBD();
            Lateralidad later = Lateralidad.DERECHA;
            Genero sex = Genero.FEMENINO;

            if (this.rbIzq.Checked)
                later = Lateralidad.IZQUIERDA;
            if (this.rbHombre.Checked)
                sex = Genero.MASCULINO;


            codigoUsuario = bd.consultarUsuario(this.tbNombre.Text, this.tbApellidos.Text, later.ToString(), sex.ToString(), this.tbDeporte.Text, this.tbEdad.Text,
                this.tbPais.Text, this.tbPosicion.Text);

            if (codigoUsuario == -1)
            {
                codigoUsuario = Convert.ToInt32(bd.obtenerIndice("Usuarios"));
                codigoUsuario++; //Empezamos en el 1 o en el siguiente.

                bd.insertarUsuario(codigoUsuario, this.tbNombre.Text, this.tbApellidos.Text, later.ToString(), sex.ToString(), this.tbDeporte.Text, this.tbEdad.Text,
                    this.tbPais.Text, this.tbPosicion.Text);
                bd.actualizarIndice("Usuarios", codigoUsuario);
                this.codigoUsuarios = codigoUsuario;
            }
            else
            {
                this.codigoUsuarios = codigoUsuario;
            }
            bd.cerrarConexiion();
        }
        private void actualizarIndiceUsuario()
        {
            BaseDatos bd = new BaseDatos();
            bd.conectarBD();
            bd.actualizarIndice("Usuarios", this.codigoUsuarios);//CodigoUsuario tiene el valor del usuario por el que vamos.
            bd.cerrarConexiion();
        }
        private void guardarEnBaseDeDatos()
        {
            //Vamos a comprobar que se a pulsado al menos un botón para guardar en la base de datos, en caso contrario no  se guardan datos porque no tiene sentido
            if (this.tiempos != null)
            {
                BaseDatos bd = new BaseDatos();
                if (!String.IsNullOrEmpty(this.tbCodigoUsuario.Text))//Es un usuario nuevo 
                {
                    guardarUsuario();
                    actualizarIndiceUsuario();
                    guardarTipoRejilla();
                    if (this.cbColoresFondo.Checked)
                        guardarColoresFondo();
                    if (this.cbEmparejar.Checked)
                        guardarEmparejar();
                    if (rbHorizontal.Checked || rbVertical.Checked || rbAleatorio.Checked)
                        guardarLineaDistractora();
                    if (rbMetronomo.Checked || rbMusica.Checked)
                        guardarSonidos();

                    guardarRejilla();
                }
            }
        }
        private void guardarTipoRejilla()
        {
            switch (this.rejillaTipo)
            {
                case TipoRejilla.NUMERICA:
                    guardarNumerica();
                    guardarControlTachado();
                    break;
                case TipoRejilla.COLORES:
                    guardarColores();
                    break;
                case TipoRejilla.ABECEDARIO:
                    guardarAbecedario();
                    guardarControlTachado();
                    break;
                case TipoRejilla.LETRAS:
                    guardarLetras();
                    guardarControlTachado();
                    break;
                case TipoRejilla.IMAGENES:
                    guardarImagenes();
                    break;
                case TipoRejilla.WINGDINGS:
                    guardarWindings();
                    break;
            }
        }
        private void guardarNumerica()
        {
            int indice = -1;
            Tachar.Orden orden = Tachar.Orden.ASCENDENTE; //Por defecto ascendente
            BaseDatos bd = new BaseDatos();
            bd.conectarBD();
            if (this.rbAscendente.Checked)
                orden = Tachar.Orden.ASCENDENTE;
            else if (this.rbDescendente.Checked)
                orden = Tachar.Orden.DESCENDENTE;

            indice = bd.consultarNumerica(Convert.ToInt32(this.comienzoRejilla), this.incrementoNext, orden.ToString());
            if (indice == -1) //inserto en la tabla
            {
                indice = Convert.ToInt32(bd.obtenerIndice("Numerica"));
                indice++; //Empezamos en el 1 o en el siguiente.
                bd.insertarNumerica(indice, Convert.ToInt32(this.comienzoRejilla), this.incrementoNext, orden.ToString());
                bd.actualizarIndice("NUMERICA", indice);
                this.codigoNumerica = indice; //si el indice de la tabla INDICE era 3, ahora el nuevo debe ser 4.
            }
            else //existe la rejilla y 
            {
                this.codigoNumerica = indice; //tiene el codigo actual de la rejilla numerica
            }
            bd.cerrarConexiion();
        }
        private void guardarAbecedario()
        {
            int indice = -1;
            Tachar.Abecedario abc = Tachar.Abecedario.MINUSCULAS;
            Tachar.Orden asc = Tachar.Orden.ASCENDENTE;
            BaseDatos bd = new BaseDatos();
            bd.conectarBD();
            if (this.rbAbcMayusculas.Checked)
                abc = Tachar.Abecedario.MAYUSCULAS;
            else if (this.rbAbcMinusculas.Checked)
                abc = Tachar.Abecedario.MINUSCULAS;
            else if (this.rbAbcAleatorio.Checked)
                abc = Tachar.Abecedario.ALEATORIO;
            else if (this.rbAbcminusMAYUS.Checked)
                abc = Tachar.Abecedario.MINUSMAYUS;
            else if (this.rbAbcMAYUSminus.Checked)
                abc = Tachar.Abecedario.MAYUSMINUS;

            if (this.rbAscAbecedario.Checked)
                asc = Tachar.Orden.ASCENDENTE;
            else if (this.rbDescAbecedario.Checked)
                asc = Tachar.Orden.DESCENDENTE;

            indice = bd.consultarAbecedario(this.tbAbecedario.Text, abc.ToString(), asc.ToString());
            if (indice == -1)//No existe esta fila en la rejilla Abecedario
            {
                indice = Convert.ToInt32(bd.obtenerIndice("Abecedario"));
                indice++; //Empezamos en el 1 o en el siguiente.
                bd.insertarAbecedario(indice, this.tbAbecedario.Text, abc.ToString(), asc.ToString());
                bd.actualizarIndice("ABECEDARIO", indice);
                this.codigoAbecedario = indice; //si el indice de la tabla INDICE era 3, ahora el nuevo debe ser 4.
            }
            else
            {
                this.codigoAbecedario = indice;
            }
            bd.cerrarConexiion();
        }

        private void guardarControlTachado()
        {
            int indice = -1;
            BaseDatos bd = new BaseDatos();
            bd.conectarBD();

            List<string> listaClaves = new List<string>();
            List<string> listaTitulos = new List<string>();
            List<int> listaIndiceItem = new List<int>();
            List<int> listaConteoPulsaciones = new List<int>();
            List<DateTime> listaTiempos = new List<DateTime>();
            List<bool> listaBotonLocalizado = new List<bool>();
            DatosControlTachado dct;
            foreach (KeyValuePair<String, DatosControlTachado> elemento in this.listaDatosControlTachado)
            {
                listaClaves.Add((elemento.Key));
                dct = elemento.Value;
                listaTitulos.Add(elemento.Value.obtenerTitulo());
                listaConteoPulsaciones.Add(elemento.Value.devolverPulsaciones());
                listaIndiceItem.Add(elemento.Value.devolverItem());
                listaTiempos.Add(elemento.Value.devolverTiempo());
                listaBotonLocalizado.Add(elemento.Value.devolverBoton());
            }

            //Serializamos las listas y las guardamos en la BD
            string jsonListaclave = JsonSerializer.Serialize(listaClaves);
            string jsonListaTitulos = JsonSerializer.Serialize(listaTitulos);
            string jsonlistaIndicesItem = JsonSerializer.Serialize(listaIndiceItem);
            string jsonListaConteoPulsaciones = JsonSerializer.Serialize(listaConteoPulsaciones);
            string jsonListaBotonLocalizado = JsonSerializer.Serialize(listaBotonLocalizado);
            string jsonListaTiempos = JsonSerializer.Serialize(listaTiempos);

            string jSonOrdenListaPulsacion = JsonSerializer.Serialize<List<string>>(this.ordenPulsacion);
            indice = Convert.ToInt32(bd.obtenerIndice("OrdenTachado"));
            indice++;
            this.codigoControlTachado = indice;

            bd.insertarControlTachado(indice, this.fuenteFamilia.Name, jsonListaclave, jsonListaTitulos, jsonlistaIndicesItem, jsonListaConteoPulsaciones, jsonListaBotonLocalizado, jsonListaTiempos, jSonOrdenListaPulsacion);
            bd.actualizarIndice("OrdenTachado", indice);
            bd.cerrarConexiion();
        }

        private void guardarLetras()
        {
            int indice = -1;
            BaseDatos bd = new BaseDatos();
            bd.conectarBD();

            //Ordenamos las listas antes de guardarla, para que luego la comparación de cadenas sea inmediata en la BD
            this.listaLetras.Sort();
            this.listaLetrasTachar.Sort();
            string listillaLetras = string.Join("~", this.listaLetras);
            string listillaLetrasTachar = string.Join("~", this.listaLetrasTachar);
            //this.minimoLetras //tiene el % de letras que aparecen en la Rejilla De Letras

            indice = bd.consultarLetras(this.minimoLetras.ToString(), this.fuenteFamilia.ToString(), listillaLetras, listillaLetrasTachar);
            if (indice == -1)
            {
                indice = Convert.ToInt32(bd.obtenerIndice("Letras"));
                indice++;
                bd.insertarLetras(indice, this.minimoLetras.ToString(), this.fuenteFamilia.Name.ToString(), listillaLetras, listillaLetrasTachar);
                bd.actualizarIndice("Letras", indice);
                this.codigoLetras = indice;
            }
            else
            {
                this.codigoLetras = indice;
            }
            bd.cerrarConexiion();
        }
        private void guardarWindings()
        {
            int indice = -1;
            BaseDatos bd = new BaseDatos();
            bd.conectarBD();
            string listillaLetrasRejilla = string.Join("~", this.listaCompletaObjetos);
            string listillaLetrasTachar = string.Join("~", this.listaSeleccionados);

            List<ClaseTipoLetraFuenteBD> listaBD = new List<ClaseTipoLetraFuenteBD>();
            foreach (TipoLetraFuente tlf in this.listaCompletaObjetos)
            {
                ClaseTipoLetraFuenteBD ctlf = new ClaseTipoLetraFuenteBD();
                ctlf.letra = tlf.devolverLetra();
                ctlf.nombreFuente = tlf.devolverFuente().Name;
                ctlf.tamaño = tlf.devolverTamaño();
                listaBD.Add(ctlf);
            }

            List<ClaseTipoLetraFuenteBD> listaBDTachar = new List<ClaseTipoLetraFuenteBD>();
            foreach (TipoLetraFuente tlf in this.listaSeleccionados)
            {
                ClaseTipoLetraFuenteBD ctlf = new ClaseTipoLetraFuenteBD();
                ctlf.letra = tlf.devolverLetra();
                ctlf.tamaño = tlf.devolverTamaño();
                ctlf.nombreFuente = tlf.devolverFuente().Name;
                listaBDTachar.Add(ctlf);
            }

            //Ordeno la lista antes de serializar y meterla en la BD
            listaBD.Sort((x, y) => string.Compare(x.letra, y.letra));
            listaBDTachar.Sort((x, y) => string.Compare(x.letra, y.letra));

            string listaBDSerializada = JsonSerializer.Serialize(listaBD);
            string listaBDSerializadaTachar = JsonSerializer.Serialize(listaBDTachar);


            indice = bd.consultarWindings(listaBDSerializada, listaBDSerializadaTachar, this.minimoWin.ToString());
            if (indice == -1)//No existe, luego inserto una nueva fila.
            {
                indice = Convert.ToInt32(bd.obtenerIndice("Windings"));
                indice++;
                bd.insertarWindings(indice, listaBDSerializada, listaBDSerializadaTachar, this.minimoWin.ToString());
                bd.actualizarIndice("Windings", indice);
                this.codigoWindings = indice;
            }
            else
            {
                this.codigoWindings = indice;
            }
            bd.cerrarConexiion();
        }
        private void guardarColores()
        {
            int indice = -1;
            BaseDatos bd = new BaseDatos();
            bd.conectarBD();

            List<ClaseColoresRGBA> coloresRGBARejilla = new List<ClaseColoresRGBA>();
            foreach (Color c in this.plantillaColoresRejilla.obtenerTodosColores())
            {
                ClaseColoresRGBA ccrgba = new ClaseColoresRGBA
                {
                    R = c.R,
                    G = c.G,
                    B = c.B,
                    A = c.A
                };
                coloresRGBARejilla.Add(ccrgba);
            }

            List<ClaseColoresRGBA> coloresRGBATachar = new List<ClaseColoresRGBA>();
            foreach (Color c in this.listaBotonesColores)
            {
                ClaseColoresRGBA ccrgba = new ClaseColoresRGBA
                {
                    R = c.R,
                    G = c.G,
                    B = c.B,
                    A = c.A
                };
                coloresRGBATachar.Add(ccrgba);
            }

            List<ClaseColoresRGBA> listaColoresRGBARejillaOrdenada = coloresRGBARejilla.OrderBy(color => color.ToArgb()).ToList();
            List<ClaseColoresRGBA> listaColoresRGBATacharOrdenada = coloresRGBATachar.OrderBy(color => color.ToArgb()).ToList();

            //string listillaColores = JsonSerializer.Serialize(coloresRGBARejilla);
            //string listillaColoresTachar = JsonSerializer.Serialize(coloresRGBATachar);
            string listillaColores = JsonSerializer.Serialize(listaColoresRGBARejillaOrdenada);
            string listillaColoresTachar = JsonSerializer.Serialize(listaColoresRGBATacharOrdenada);

            indice = bd.consultarColores(this.minimoCantidadBotonesColores.ToString(), listillaColores, listillaColoresTachar);
            if (indice == -1)
            {
                indice = Convert.ToInt32(bd.obtenerIndice("Colores"));
                indice++;
                bd.insertarColores(indice, this.minimoCantidadBotonesColores.ToString(), listillaColores, listillaColoresTachar);
                bd.actualizarIndice("Colores", indice);
                this.codigoColores = indice;
            }
            else
            {
                this.codigoColores = indice;
            }
            bd.cerrarConexiion();
        }



        //Para serializar las imagenes, las convertimos a array de byte



        // Convierte una imagen a un array de bytes
        public static byte[] imagenToArrayByte(Image image)
        {
            using (var ms = new MemoryStream())
            {
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Png); // Se pueden usar otros formatos como JPEG
                return ms.ToArray();
            }
        }

        // Convierte un array de bytes a una imagen
        public static Image arrayByteToImagen(byte[] byteArray)
        {
            using (var ms = new MemoryStream(byteArray))
            {
                return Image.FromStream(ms);
            }
        }

        private void guardarImagenes()
        {
            int indice = -1;
            BaseDatos bd = new BaseDatos();
            bd.conectarBD();

            // Convertir imágenes a arrays de bytes
            List<byte[]> imagenesEnBytes = new List<byte[]>();
            foreach (var img in listaImagenes)
            {
                imagenesEnBytes.Add(imagenToArrayByte(img));
            }

            //Ordenamos la lista de imagenes
            imagenesEnBytes.Sort((img1, img2) => CompareImageHashes(img1, img2));            // Serializar la lista de arrays de bytes
            byte[] binario = MessagePackSerializer.Serialize(imagenesEnBytes);

            //Convertir la lista de imagenes seleccionadas en bytes
            List<byte[]> imagenesSeleccionadasEnBytes = new List<byte[]>();
            foreach (var img in listaImagenesEscogidas)
            {
                imagenesSeleccionadasEnBytes.Add(imagenToArrayByte(img));
            }
            //ordenar la lista de imagenes tachar
            imagenesSeleccionadasEnBytes.Sort((img1, img2) => CompareImageHashes(img1, img2));
            byte[] binarioTachar = MessagePackSerializer.Serialize(imagenesSeleccionadasEnBytes);

            indice = bd.consultarImagenes(this.minimoCantidadBotonesImagenes.ToString(), binario, binarioTachar);
            if (indice == -1)
            {
                indice = Convert.ToInt32(bd.obtenerIndice("Imagenes"));
                indice++;
                bd.insertarImagenes(indice, this.minimoCantidadBotonesImagenes.ToString(), binario, binarioTachar);
                bd.actualizarIndice("Imagenes", indice);
                this.codigoColores = indice;
            }
            else
            {
                this.codigoImagenes = indice;
            }
            bd.cerrarConexiion();
        }

        static int CompareImageHashes(byte[] img1, byte[] img2)
        {
            string hash1 = ComputeMd5Hash(img1);
            string hash2 = ComputeMd5Hash(img2);

            return string.Compare(hash1, hash2);
        }

        static string ComputeMd5Hash(byte[] data)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] hashBytes = md5.ComputeHash(data);
                StringBuilder sb = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    sb.Append(b.ToString("x2"));
                }
                return sb.ToString();
            }
        }

        private Tachar.Intercambiar intercambiar()
        {
            Tachar.Intercambiar devolver = Tachar.Intercambiar.NADA;
            if (vectorProgramarColores[0] == true)
                return Tachar.Intercambiar.TEXTO;
            else if (vectorProgramarColores[1] == true)
                return Tachar.Intercambiar.COLOR;
            else if (vectorProgramarColores[2] == true)
                return Intercambiar.TEXTOYCOLOR;


            return devolver;
        }
        private Tachar.TacharBoton tacharParaBD()
        {
            Tachar.TacharBoton devolver = Tachar.TacharBoton.NADA;
            if (this.tacharNada)
            {
                devolver = Tachar.TacharBoton.NADA;
            }
            else if (this.tacharColor)
            {
                devolver = Tachar.TacharBoton.COLOR;
            }
            else if (this.tacharTexto)
            {
                devolver = Tachar.TacharBoton.TEXTO;
            }
            else if (this.tacharTodo)
            {
                devolver = Tachar.TacharBoton.TEXTOYCOLOR;
            }
            return devolver;
        }
        private void guardarColoresFondo()
        {
            int indice = -1;
            BaseDatos bd = new BaseDatos();
            bd.conectarBD();

            List<ClaseColoresRGBA> listaRGBAColoresFondo = new List<ClaseColoresRGBA>();
            foreach (Color c in this.listaColoresElegidosParaElFondo)
            {
                ClaseColoresRGBA ccrgba = new ClaseColoresRGBA
                {
                    R = c.R,
                    G = c.G,
                    B = c.B,
                    A = c.A
                };
                listaRGBAColoresFondo.Add(ccrgba);
            }


            List<ClaseColoresRGBA> coloresRGBARejilla = new List<ClaseColoresRGBA>();
            foreach (Color c in this.plantillaColoresRejilla.obtenerTodosColores())
            {
                ClaseColoresRGBA ccrgba = new ClaseColoresRGBA
                {
                    R = c.R,
                    G = c.G,
                    B = c.B,
                    A = c.A
                };
                coloresRGBARejilla.Add(ccrgba);
            }

            List<ClaseColoresRGBA> listaColoresRGBARejillaOrdenada = coloresRGBARejilla.OrderBy(color => color.ToArgb()).ToList();

            string listillaColores = JsonSerializer.Serialize(listaColoresRGBARejillaOrdenada);
            string listillaColoresElegidosFondo = JsonSerializer.Serialize(listaRGBAColoresFondo);

            indice = bd.consultarColoresFondo(listillaColores, listillaColoresElegidosFondo);
            if (indice == -1)
            {
                indice = Convert.ToInt32(bd.obtenerIndice("ColoresFondo"));
                indice++;
                bd.insertarColoresFondo(indice, listillaColores, listillaColoresElegidosFondo);
                bd.actualizarIndice("ColoresFondo", indice);
                this.codigoColoresFondo = indice;
            }
            else
            {
                this.codigoColoresFondo = indice;
            }
            bd.cerrarConexiion();
        }
        private void guardarEmparejar()
        {
            int indice = -1;
            BaseDatos bd = new BaseDatos();
            bd.conectarBD();
            indice = bd.consultarEmparejar(this.tiempoEmparejar.ToString(), this.secuencia.ToString(), this.posicion.ToString());
            if (indice == -1)
            {
                indice = Convert.ToInt32(bd.obtenerIndice("Emparejar"));
                indice++;
                bd.insertarEmparejar(indice, this.tiempoEmparejar.ToString(), this.secuencia.ToString(), this.posicion.ToString());
                bd.actualizarIndice("Emparejar", indice);
                this.codigoEmparejar = indice;
            }
            else
            {
                this.codigoEmparejar = indice;
            }
            bd.cerrarConexiion();
        }
        private void guardarLineaDistractora()
        {
            int indice = -1;
            BaseDatos bd = new BaseDatos();
            bd.conectarBD();
            indice = bd.consultarLinea(this.color.ToString(), this.grosor.ToString(), this.velocidadLinea.ToString(), this.linea.ToString());
            if (indice == -1)
            {
                indice = Convert.ToInt32(bd.obtenerIndice("LineaDistractora"));
                indice++;
                ClaseColoresRGBA c = new ClaseColoresRGBA();
                c.R = this.color.R;
                c.A = this.color.A;
                c.B = this.color.B;
                c.G = this.color.G;

                List<ClaseColoresRGBA> listaColor = new List<ClaseColoresRGBA> { c };

                string colorSerializa = JsonSerializer.Serialize(listaColor);

                bd.insertarLinea(indice, colorSerializa, this.cBGrosor.SelectedIndex.ToString(), this.velocidadLinea.ToString(), this.linea.ToString());
                bd.actualizarIndice("LineaDistractora", indice);
                this.codigoLineaDistractora = indice;
            }
            else
            {
                this.codigoLineaDistractora = indice;
            }
            bd.cerrarConexiion();
        }
        private void guardarSonidos()
        {
            int indice = -1;
            BaseDatos bd = new BaseDatos();
            bd.conectarBD();
            string metro = null, music = null;
            if (rbMetronomo.Checked)
            {
                metro = this.tbMetronomo.Text;//hay metronomo
            }
            else if (rbMusica.Checked)
            {
                //hay musica
                music = this.rutaFicheroM3u;
            }
            else if (rbSinSonido.Checked)
            {
                //no hay sonido
            }

            //indice = bd.consultarSonidos(this.tbMetronomo.Text, this.rutaFicheroM3u);
            indice = bd.consultarSonidos(metro, music);
            if (indice == -1)
            {
                indice = Convert.ToInt32(bd.obtenerIndice("Sonidos"));
                indice++;
                //bd.insertarSonidos(indice, this.tbMetronomo.Text, this.rutaFicheroM3u);
                bd.insertarSonidos(indice, metro, music);
                bd.actualizarIndice("Sonidos", indice);
                this.codigoSnidos = indice;
            }
            else
            {
                this.codigoSnidos = indice;
            }
            bd.cerrarConexiion();
        }

        private int cantidadBotonesPorRejilla()
        {
            int devolver = -1;
            devolver = Convert.ToInt32(this.textBoxNumColumnas.Text) * Convert.ToInt32(textBoxNumFila.Text);
            int cero = 0;
            int veinticinco = (int)(devolver * 0.25);
            int cincuenta = (int)(devolver * 0.50);
            int setentaycinco = (int)(devolver * 0.5);
            switch (this.rejillaTipo)
            {
                case TipoRejilla.NUMERICA:
                case TipoRejilla.ABECEDARIO:
                    break;
                case TipoRejilla.COLORES:
                    switch (this.minimoCantidadBotonesColores)
                    {
                        case TantoPorCiento.CERO:
                            devolver = this.cantidadBotones;
                            break;
                        case TantoPorCiento.VEINTICINCO:
                            devolver = veinticinco;
                            break;
                        case TantoPorCiento.CINCUENTA:
                            devolver = cincuenta;
                            break;
                        case TantoPorCiento.SETENTAYCINCO:
                            devolver = setentaycinco;
                            break;
                    }
                    break;
                case TipoRejilla.LETRAS:
                    switch (this.minimoLetras)
                    {
                        case TantoPorCiento.CERO:
                            devolver = this.cantidadBotones;
                            break;
                        case TantoPorCiento.VEINTICINCO:
                            devolver = veinticinco;
                            break;
                        case TantoPorCiento.CINCUENTA:
                            devolver = cincuenta;
                            break;
                        case TantoPorCiento.SETENTAYCINCO:
                            devolver = setentaycinco;
                            break;
                    }
                    break;
                case TipoRejilla.IMAGENES:
                    switch (this.minimoCantidadBotonesImagenes)
                    {
                        case TantoPorCiento.CERO:
                            devolver = this.cantidadBotones;
                            break;
                        case TantoPorCiento.VEINTICINCO:
                            devolver = veinticinco;
                            break;
                        case TantoPorCiento.CINCUENTA:
                            devolver = cincuenta;
                            break;
                        case TantoPorCiento.SETENTAYCINCO:
                            devolver = setentaycinco;
                            break;
                    }
                    break;
                case TipoRejilla.WINGDINGS:
                    switch (this.minimoWin)
                    {
                        case TantoPorCiento.CERO:
                            devolver = this.cantidadBotones;
                            break;
                        case TantoPorCiento.VEINTICINCO:
                            devolver = veinticinco;
                            break;
                        case TantoPorCiento.CINCUENTA:
                            devolver = cincuenta;
                            break;
                        case TantoPorCiento.SETENTAYCINCO:
                            devolver = setentaycinco;
                            break;
                    }
                    break;
            }
            return devolver;
        }

        //Este método me devuelve el código que tengo que asociar al tipo de rejilla en la tabla rejilla-
        private int codigoBDTipoRejilla()
        {
            int devolver = -1;
            switch (rejillaTipo)
            {
                case TipoRejilla.NUMERICA:
                    devolver = this.codigoNumerica;
                    break;
                case TipoRejilla.COLORES:
                    devolver = this.codigoColores;
                    break;
                case TipoRejilla.LETRAS:
                    devolver = this.codigoLetras;
                    break;
                case TipoRejilla.ABECEDARIO:
                    devolver = this.codigoAbecedario;
                    break;
                case TipoRejilla.IMAGENES:
                    devolver = this.codigoImagenes;
                    break;
                case TipoRejilla.WINGDINGS:
                    devolver = this.codigoWindings;
                    break;
            }
            return devolver;
        }

        private void guardarRejilla()
        {
            int indice = -1;
            BaseDatos bd = new BaseDatos();
            bd.conectarBD();
            int totalBotones = Convert.ToInt32(textBoxNumColumnas.Text) * Convert.ToInt32(textBoxNumFila.Text);
            indice = Convert.ToInt32(bd.obtenerIndice("Rejilla"));
            indice++;
            string jsonTiempos = JsonSerializer.Serialize(tiempos);
            string jsonLatencias = String.Empty;
            if (cbEmparejar.Checked)
                jsonLatencias = JsonSerializer.Serialize(guardarTiemposEmparejamiento);
            string fechaFormateada = "";
            string ff = this.tbFecha.Text;
            string fechaOriginal = "";
            string formato = "dd/MM/yyyy H:mm:ss";
            string formato2 = "dd/MM/yyyy HH:mm:ss";

            if (ff.Length == 18)
            {
                //muestra la hora sin el cero delante
                DateTime fecha1 = DateTime.ParseExact(ff, formato, null);
                fechaOriginal = fecha1.ToString(formato2);
            }
            else
            {
                fechaOriginal = this.tbFecha.Text;
            }
            DateTime fechaAuxiliar;
            DateTime.TryParseExact(fechaOriginal, "dd/MM/yyyy HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out fechaAuxiliar);
            fechaFormateada = fechaAuxiliar.ToString("yyyy-MM-dd");

            //Vamos a guardar los ticks en la BD para luego poder recuperarlos y procesarlos mejor
            List<double> tiskTiempoGrafica = new List<double>();
            List<double> tiskTiempoLatencia = new List<double>();
            string JsonTickGrafica = null, JsonTickTiempos = null;
            if (this.guardarTiemposGrafica != null)
            {
                foreach (DateTime dt in this.guardarTiemposGrafica)
                {
                    tiskTiempoGrafica.Add(dt.Ticks);
                }
                JsonTickGrafica = JsonSerializer.Serialize<List<double>>(tiskTiempoGrafica);
            }
            if (this.tiempos != null)
            {
                foreach (TimeSpan ts in this.tiempos)
                {
                    tiskTiempoLatencia.Add(ts.Ticks);
                }
                JsonTickTiempos = JsonSerializer.Serialize<List<Double>>(tiskTiempoLatencia);
            }
            bd.actualizarIndice("Rejilla", indice);
            bd.insertarRejilla(indice, this.textBoxNumFila.Text, this.textBoxNumColumnas.Text, errores, aciertos, tbFecha.Text, totalBotones, this.tamañoBoton.ToString(),
                tacharParaBD().ToString(), intercambiar().ToString(), cantidadBotonesPorRejilla().ToString(), this.observaciones,
                rejillaTipo.ToString(), codigoBDTipoRejilla(), this.codigoUsuarios, this.codigoEmparejar, this.codigoLineaDistractora, this.codigoSnidos,
                this.etTiempoTranscurrido.Text, this.etValorMedia.Text, this.etVarianza.Text, this.etValorTiempoCorregido.Text, this.etValorTiempoCorreccionEfectuada.Text,
                this.etValorTiempoTareaPreliminar.Text, this.etValorMediaCorreccion.Text, this.etValorVarianzaCorreccion.Text,
                this.etValorMaximo.Text, this.etValorMinimo.Text, this.tbTiempoLimite.Text, this.textBoxTiempoSegundosAleatorio.Text, jsonTiempos, jsonLatencias,
                this.codigoColoresFondo, this.codigoControlTachado, fechaFormateada, JsonTickGrafica, JsonTickTiempos);

            bd.cerrarConexiion();
            this.codigoRejilla = indice;
        }
        /*
         * ***************************************************************
         * METODOS VERIFICACION DE DATOS E INICIALIZACIÓN, CONTROL GENERAL DEL FORMULARIO
         * ***************************************************************
         */

        /*
         * Metodo para verificar que las filas/columnas van de 1 a 15.
         */
        private void comprobarQueesUnNumero(String msg)
        {
            List<String> listaNumeros = new List<string>();
            listaNumeros.Add("1");
            listaNumeros.Add("2");
            listaNumeros.Add("3");
            listaNumeros.Add("4");
            listaNumeros.Add("5");
            listaNumeros.Add("6");
            listaNumeros.Add("7");
            listaNumeros.Add("8");
            listaNumeros.Add("9");
            listaNumeros.Add("10");
            listaNumeros.Add("11");
            listaNumeros.Add("12");
            listaNumeros.Add("13");
            listaNumeros.Add("14");
            listaNumeros.Add("15");
            if (!listaNumeros.Contains(msg))
                throw new RejillaException(rm.GetString("cad03"));//"Valor de FILA/COLUMNA entre 1 --> 15");
        }

        /*
         * Método que solo permite que se escriban números en los textBox
         */
        private void tbSoloNumeros_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(8))
                //se pulso la tecla retroceso
                e.Handled = false;
            else if (e.KeyChar < '0' || e.KeyChar > '9')
                //desechar los caracteres que no son digitos
                e.Handled = true;
        }

        /*
         * Método que sólo permite que se escriban letras en los textBox
         */
        private void tbSoloLetras_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(8))
                //se pulso la tecla retroceso
                e.Handled = false;
            else if (e.KeyChar < 'a' || e.KeyChar > 'z')
                //desechar los caracteres que no sean letras
                e.Handled = true;
        }

        /*
         * Inicializamos todos los comboBox, para que apunten al primer 
         * elemento de la lista.
         */
        private void MetiendoDatos_Load(object sender, EventArgs e)
        {
            if (comboBoxTamañoBoton.SelectedIndex == -1)
                comboBoxTamañoBoton.SelectedIndex = 0;
            if (comboIncrementoBotones.SelectedIndex == -1)
                comboIncrementoBotones.SelectedIndex = 0;
            if (cBGrosor.SelectedIndex == -1)
                cBGrosor.SelectedIndex = 0;
        }

        /*
         * Método que controla la pulsación del tabPanel.
         */
        private void tabControlRejilla_Selected(object sender, TabControlEventArgs e)
        {
            if (e.TabPageIndex == 0)
                this.rbNumerica.Checked = true;
            else if (e.TabPageIndex == 1)
                this.rbColores.Checked = true;
            else if (e.TabPageIndex == 2)
                this.rbLetras.Checked = true;
            else if (e.TabPageIndex == 3)
                this.rbImagenes.Checked = true;
            else if (e.TabPageIndex == 4)
                this.rbAbecedario.Checked = true;
            else if (e.TabPageIndex == 5)
                this.rbWingdings.Checked = true;
        }

        /*
         * Método que controla los radioButton del tipo de rejilla.
         */
        private void radioButtonTipoRejilla_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            if (this.rbNumerica.Checked)
            {
                this.rejillaTipo = TipoRejilla.NUMERICA;
            }
            else if (rbColores.Checked)
            {
                this.rejillaTipo = TipoRejilla.COLORES;
            }
            else if (this.rbLetras.Checked)
            {
                this.rejillaTipo = TipoRejilla.LETRAS;
            }
            else if (rbImagenes.Checked)
            {
                this.rejillaTipo = TipoRejilla.IMAGENES;
            }
            else if (rbAbecedario.Checked)
            {
                this.rejillaTipo = TipoRejilla.ABECEDARIO;
            }
            else if (rbWingdings.Checked)
            {
                this.rejillaTipo = TipoRejilla.WINGDINGS;
            }
            cargarTipoRejilla(this.rejillaTipo);
            botonesPorTipo();
        }

        /*
         * Método que habilita /no habilita los botones dependiendo del
         * tipo de rejilla pulsado.
         */
        private void botonesPorTipo()
        {
            switch (this.rejillaTipo)
            {
                case TipoRejilla.NUMERICA:
                case TipoRejilla.ABECEDARIO:
                    this.etOrdenGrafica.Visible = true;
                    this.btColoresFondo.Enabled = true;
                    this.btProgramarColores.Enabled = true;
                    this.cbColoresFondo.Enabled = true;
                    this.btControlTachado.Visible = true;
                    //this.btControlTachado.Enabled = true;
                    break;
                case TipoRejilla.IMAGENES:
                case TipoRejilla.COLORES:
                    this.etOrdenGrafica.Visible = false;
                    this.btColoresFondo.Enabled = false;
                    this.btProgramarColores.Enabled = false;
                    this.cbColoresFondo.Checked = false;
                    this.cbColoresFondo.Enabled = false;
                    this.btControlTachado.Visible = false;
                    this.btControlTachado.Enabled = false;
                    break;
                //case TipoRejilla.ABECEDARIO:
                case TipoRejilla.LETRAS:
                    this.etOrdenGrafica.Visible = false;
                    this.btColoresFondo.Enabled = true;
                    this.btProgramarColores.Enabled = true;
                    this.cbColoresFondo.Enabled = true;
                    this.btControlTachado.Visible = true;
                    //this.btControlTachado.Enabled = true;
                    break;
                case TipoRejilla.WINGDINGS:
                    this.etOrdenGrafica.Visible = false;
                    this.btColoresFondo.Enabled = true;
                    this.btProgramarColores.Enabled = true;
                    this.cbColoresFondo.Enabled = true;
                    this.btControlTachado.Enabled = false;
                    this.btControlTachado.Visible = false;
                    break;
            }
        }

        /*
         * Procedimiento que actualiza el botonTamaño.
         */
        private void cargarTipoRejilla(TipoRejilla tr)
        {
            if (tr == TipoRejilla.NUMERICA)
            {
                this.rbNumerica.Checked = true;
                this.rejillaTipo = TipoRejilla.NUMERICA;
                this.tabControlRejilla.SelectTab(0);
                botonTamaño.BackgroundImage = null;
                botonTamaño.BackColor = SystemColors.Control;//Color.Transparent;
                botonTamaño.Font = new Font("Microsoft Sans Serif", (this.tamañoBoton / 4),
                                         System.Drawing.FontStyle.Regular,
                                         System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                botonTamaño.Text = new Random((int)DateTime.Now.Ticks).Next(1, 1000).ToString();
                if (this.rbAscendente.Checked)
                    this.etOrdenGrafica.Text = rm.GetString("txt01");
                else if (this.rbDescendente.Checked)
                    this.etOrdenGrafica.Text = rm.GetString("txt02");
            }
            else if (tr == TipoRejilla.COLORES)
            {
                this.rbColores.Checked = true;
                this.rejillaTipo = TipoRejilla.COLORES;
                this.tabControlRejilla.SelectTab(1);
                this.botonTamaño.Text = "";
                this.botonTamaño.BackgroundImage = null;
                if (this.listaBotonesColores.Count != 0)
                    botonTamaño.BackColor = listaBotonesColores.ElementAt(new Random().Next(0, this.listaBotonesColores.Count));
            }
            else if (tr == TipoRejilla.LETRAS)
            {
                this.rbLetras.Checked = true;
                this.rejillaTipo = TipoRejilla.LETRAS;
                this.tabControlRejilla.SelectTab(2);
                botonTamaño.Text = "";
                botonTamaño.BackColor = SystemColors.Control;//Color.Transparent;
                botonTamaño.BackgroundImage = null;
                botonTamaño.Font = new Font(btLetras1.Font.Name, (tamañoBoton / 2),
                                            btLetras1.Font.Style,
                                            System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                if (this.listaLetras.Count != 0)
                    this.botonTamaño.Text = listaLetras.ElementAt(new Random().Next(0, this.listaLetras.Count));
            }
            else if (tr == TipoRejilla.IMAGENES)
            {
                this.rbImagenes.Checked = true;
                this.rejillaTipo = TipoRejilla.IMAGENES;
                this.tabControlRejilla.SelectTab(3);
                this.botonTamaño.Text = "";
                this.botonTamaño.BackColor = SystemColors.Control;//Color.Transparent;
                this.botonTamaño.BackgroundImageLayout = ImageLayout.Stretch;
                if (this.listaImagenesEscogidas.Count != 0)
                    this.botonTamaño.BackgroundImage = listaImagenesEscogidas.ElementAt(new Random().Next(0, this.listaImagenesEscogidas.Count));

            }
            else if (tr == TipoRejilla.ABECEDARIO)
            {
                this.rbAbecedario.Checked = true;
                this.rejillaTipo = TipoRejilla.ABECEDARIO;
                this.tabControlRejilla.SelectTab(4);
                this.botonTamaño.Text = "";
                this.botonTamaño.BackColor = SystemColors.Control;//Color.Transparent;
                this.botonTamaño.BackgroundImage = null;
                this.botonTamaño.BackColor = Color.Transparent;
                this.botonTamaño.Font = new Font("Microsoft Sans Serif", (this.tamañoBoton / 6),
                                        System.Drawing.FontStyle.Regular,
                                        System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                this.botonTamaño.Text = this.tbAbecedario.Text;
                if (this.rbAscAbecedario.Checked)
                    this.etOrdenGrafica.Text = rm.GetString("txt01");
                else if (this.rbDescAbecedario.Checked)
                    this.etOrdenGrafica.Text = rm.GetString("txt02");
            }
            else if (tr == TipoRejilla.WINGDINGS)
            {
                this.rbWingdings.Checked = true;
                this.botonTamaño.BackColor = SystemColors.Control;//Color.Transparent;
                this.botonTamaño.BackgroundImage = null;
                this.rejillaTipo = TipoRejilla.WINGDINGS;
                this.tabControlRejilla.SelectTab(5);
                if (this.listaCompletaObjetos.Count != 0)
                {
                    int rd = new Random().Next(0, this.listaCompletaObjetos.Count);
                    FontFamily ff = new FontFamily(this.listaCompletaObjetos.ElementAt(rd).devolverFuente().Name);
                    this.botonTamaño.Font = new Font(ff, this.tamañoBoton / 2);
                    this.botonTamaño.Text = this.listaCompletaObjetos.ElementAt(rd).devolverLetra();
                }
            }
        }

        /*
         * Procedimiento que me devuelve el tamañoBoton seleccionado,
         * al mismo tiempo, muestra en la pantalla como queda el boton
         * con el tamaño seleccionado
         */
        private void comboBoxTamañoBoton_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBoxTamañoBoton.SelectedIndex != -1)
            {
                String texto = this.comboBoxTamañoBoton.SelectedItem.ToString();
                int valor = System.Convert.ToInt32(texto);

                switch (valor)
                {
                    case 1:
                        botonTamaño.Size = new Size(40, 40);
                        tamañoBoton = 40;
                        break;
                    case 2:
                        botonTamaño.Size = new Size(50, 50);
                        tamañoBoton = 50;
                        break;
                    case 3:
                        botonTamaño.Size = new Size(60, 60);
                        tamañoBoton = 60;
                        break;
                    case 4:
                        botonTamaño.Size = new Size(70, 70);
                        tamañoBoton = 70;
                        break;
                    case 5:
                        botonTamaño.Size = new Size(80, 80);
                        tamañoBoton = 80;
                        break;
                    case 6:
                        botonTamaño.Size = new Size(90, 90);
                        tamañoBoton = 90;
                        break;
                    case 7:
                        botonTamaño.Size = new Size(150, 150);
                        tamañoBoton = 150;
                        break;
                    default:
                        botonTamaño.Size = new Size(40, 40);
                        break;
                }
                cargarTipoRejilla(rejillaTipo);
            }

        }//FIN comboBoxTamañoBoton_SelectedIndexChanged

        /*
         * ***************************************************************
         * DISTRACCION LINEA
         * ***************************************************************
         */

        /*
         * Metodo para escoger el color de la distracción línea.
         */
        private void btColorLinea_Click(object sender, EventArgs e)
        {
            ColorDialog cajaColor = new ColorDialog();
            if (cajaColor.ShowDialog() == DialogResult.OK)
            {
                pBColor.BackColor = cajaColor.Color;
                Graphics lin = pBGrosor.CreateGraphics();
                String s = this.cBGrosor.SelectedItem.ToString();
                int tam = System.Convert.ToInt32(s);
                lin.DrawLine(new Pen(cajaColor.Color, this.tipoGrosores[tam - 1]), new Point(pBGrosor.Width / 2, pBGrosor.Height), new Point(pBGrosor.Width / 2, 0));
                this.color = cajaColor.Color;
            }
        }

        /*
         * Escoge un grosor para la linea Molesta y dibuja como queda
         * en su pictureBox asociado
         */
        private void cBGrosor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cBGrosor.SelectedIndex != -1)
            {
                String s = this.cBGrosor.SelectedItem.ToString();
                int ii = System.Convert.ToInt32(s);
                int i = tipoGrosores[ii - 1];
                Graphics lin = pBGrosor.CreateGraphics();
                lin.Clear(pBGrosor.BackColor);
                Pen lapiz;
                if (pBColor.BackColor.Equals(Color.White))
                {
                    lapiz = new Pen(Color.Chocolate, i);
                }
                else
                {
                    lapiz = new Pen(pBColor.BackColor, i);
                }
                lin.DrawLine(lapiz, new Point(pBGrosor.Width / 2, pBGrosor.Height), new Point(pBGrosor.Width / 2, 0));
                this.grosor = i;
            }
        }

        /*
         * Método Paint del pictureBoxGrosor para que si se maximiza 
         * o cambia el tamaño... no se pierda el dibujo de la linea.
         */
        private void pBGrosor_Paint(object source, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(pBGrosor.BackColor);
            String s = this.cBGrosor.SelectedItem.ToString();
            int ii = System.Convert.ToInt32(s);
            int i = tipoGrosores[ii - 1];
            Pen lapiz = new Pen(pBColor.BackColor, i);
            g.DrawLine(lapiz, new Point(pBGrosor.Width / 2, pBGrosor.Height), new Point(pBGrosor.Width / 2, 0));
        }

        /*
         * Procedimiento que me indica el tipo de linea
         */
        private void tipoDeLinea()
        {
            if (this.rbAleatorio.Checked)
            {
                linea = tipoLinea.ALEATORIA;
                radioButonSeleccionado = true;
            }
            else if (this.rbHorizontal.Checked)
            {
                linea = tipoLinea.HORIZONTAL;
                radioButonSeleccionado = true;
            }
            else if (this.rbVertical.Checked)
            {
                linea = tipoLinea.VERTICAL;
                radioButonSeleccionado = true;
            }
            else if (this.rbSinlinea.Checked)
            {
                linea = tipoLinea.SINLINEA;
                radioButonSeleccionado = false;
            }
        }

        private void radioButton_Click(object sender, EventArgs e)
        {
            tipoDeLinea();
        }

        /*
         * Procedimiento para establecer la velocidad de la linea
         * TrackBar de 1 a 151
         */
        private void deslizadorVelocidad_ValueChanged(object sender, EventArgs e)
        {
            //el tiempo de 1 a 250 milisegundos
            this.velocidadLinea = 3.60869565217391 * deslizadorVelocidad.Value - 2.60869565217383;
        }

        /*
         * ***************************************************************
         * DISTRACCION MÚSICA
         * ***************************************************************
         */

        private void btAbrirMusica_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialogoAbrir = new OpenFileDialog();
            dialogoAbrir.InitialDirectory = this.rutaCarpetaSonidos;
            String[] rutas;

            dialogoAbrir.Filter = rm.GetString("cad04");//"Ficheros de sonido (*.wav; *.mp3) |*.wav;*.mp3";
            dialogoAbrir.RestoreDirectory = true;
            dialogoAbrir.Multiselect = true;

            if (dialogoAbrir.ShowDialog() == DialogResult.OK)
            {
                rutas = dialogoAbrir.FileNames;
                foreach (String file in dialogoAbrir.FileNames)
                {
                    this.listaMusica.Items.Add(file);
                    //valido este de abajo, sin extensión
                    //this.listaMostrar.Items.Add(file.Substring(file.LastIndexOf(Path.DirectorySeparatorChar) + 1, (file.LastIndexOf('.') - file.LastIndexOf(Path.DirectorySeparatorChar) - 1)));
                    //con extension del tipo de archivo
                    this.listaMostrar.Items.Add(file.Substring(file.LastIndexOf(Path.DirectorySeparatorChar) + 1));
                }
            }
        }

        private void rbMusica_CheckedChanged(object sender, EventArgs e)
        {
            this.winamp.Stop();
            this.reproductorWindowsMedia.Ctlcontrols.stop();

            if (rbMetronomo.Checked)
            {
                if (temporizadorMetronomo == null)
                {
                    temporizadorMetronomo = new System.Timers.Timer();
                    temporizadorMetronomo.Enabled = false;
                    temporizadorMetronomo.Elapsed += new System.Timers.ElapsedEventHandler(temporizadorMetronomo_Elapsed);
                }
            }
            if (this.temporizadorMetronomo != null)
                this.temporizadorMetronomo.Enabled = false;

            visibleControlesMusica();
        }

        /*
         * Método que controla el temporizador del metrónomo
         */
        protected void temporizadorMetronomo_Elapsed(object sender, ElapsedEventArgs e)
        {
            winamp.Play();
        }

        /*
         * Habilita / deshabilita los botones del grupo música dependiendo
         * de la opción elegida.
         */
        private void visibleControlesMusica()
        {
            if (this.rbSinSonido.Checked)
            {
                this.tbMetronomo.Enabled = false;
                this.btAñadirListaMusica.Enabled = false;
                this.btPlay.Enabled = false;
                this.btStop.Enabled = false;
                this.listaMostrar.Enabled = false;
                this.btAbrirMusica.Enabled = false;
                this.btBorrarCancion.Enabled = false;
                this.btPlaySong.Enabled = false;
                this.btPlay.Text = rm.GetString("cad05");//"Reproducir Lista/Metrónomo";

            }
            else if (this.rbMetronomo.Checked)
            {
                this.tbMetronomo.Enabled = true;
                this.btPlay.Enabled = true;
                this.btPlay.Text = rm.GetString("cad06");//"Reproducir Metrónomo";
                this.btStop.Enabled = true;

                this.listaMostrar.Enabled = false;
                this.btAbrirMusica.Enabled = false;
                this.btBorrarCancion.Enabled = false;
                this.btPlaySong.Enabled = false;
                this.btAñadirListaMusica.Enabled = false;
            }
            else if (rbMusica.Checked)
            {
                this.tbMetronomo.Enabled = false;
                this.btPlay.Enabled = true;
                this.btPlay.Text = rm.GetString("cad07");//"Reproducir Lista";
                this.btStop.Enabled = true;

                this.listaMostrar.Enabled = true;
                this.btAbrirMusica.Enabled = true;
                this.btBorrarCancion.Enabled = true;
                this.btPlaySong.Enabled = true;
                this.btAñadirListaMusica.Enabled = true;
            }
        }

        /*
         * Añadir canciones a la lista de música
         */
        private void btAñadirListaMusica_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialogoAbrir = new OpenFileDialog();
            dialogoAbrir.InitialDirectory = this.rutaCarpetaMusica;
            string lista;
            dialogoAbrir.Filter = rm.GetString("cad08");//"Lista de Música (*.m3u; *.wpl)|*.m3u;*.wpl";
            if (dialogoAbrir.ShowDialog() == DialogResult.OK)
            {
                lista = dialogoAbrir.FileName;

                /*Versión anterior
                if (this.listaMostrar.Items.Count != 0)
                {
                    this.listaMostrar.Items.Clear();
                    this.listaMusica.Items.Clear();
                }

                this.listaMusica.Items.Add(dialogoAbrir.FileName);

                //this.listaMostrar.Items.Add(lista.Substring(lista.LastIndexOf('\\') + 1, (lista.LastIndexOf('.') - lista.LastIndexOf('\\') - 1)));
                //this.listaMostrar.Items.Add(lista);
                //valido la linea de abajo de este comentario
                //this.listaMostrar.Items.Add(lista.Substring(lista.LastIndexOf(Path.DirectorySeparatorChar) + 1, (lista.LastIndexOf('.') - lista.LastIndexOf(Path.DirectorySeparatorChar) - 1)));
                this.listaMostrar.Items.Add(lista.Substring(lista.LastIndexOf(Path.DirectorySeparatorChar) + 1));
                */
                this.listaMusica.Items.Clear();
                this.listaMostrar.Items.Clear();
                procesarFicheroM3u(lista);

                this.reproductorWindowsMedia.URL = lista;
            }
            this.reproductorWindowsMedia.Ctlcontrols.stop();
        }

        /*
         * Método para reproducir una canción de la lista
         */
        private void btPlaySong_Click(object sender, EventArgs e)
        {
            try
            {
                if (rbMusica.Checked)
                {
                    if (this.listaMostrar.Items.Count == 0)
                        //throw new RejillaException("La lista de música esta vacía");
                        throw new RejillaException(rm.GetString("war01"));
                    else if (this.listaMostrar.SelectedIndex == -1)
                        //throw new RejillaException("No se ha seleccionado ninguna canción para reproducir.");
                        throw new RejillaException(rm.GetString("war02"));
                    else
                    {
                        this.listaMusica.SelectedIndex = this.listaMostrar.SelectedIndex;
                        this.reproductorWindowsMedia.URL = this.listaMusica.SelectedItem.ToString();
                    }
                }
            }
            catch (RejillaException re)
            {
                MessageBox.Show(re.Message, rm.GetString("war00")/*"Advertencia"*/, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        /*
         * Método que reproduce una lista de música, o el metronomo.
         */
        private void btPlay_Click(object sender, EventArgs e)
        {
            try
            {
                if (rbMetronomo.Checked)
                {
                    if (tbMetronomo.Text.Length == 0)
                        //throw new RejillaException("No se ha introducido el valor del metrónomo");
                        throw new RejillaException(rm.GetString("war03"));
                    else
                    {
                        temporizadorMetronomo.Interval = System.Convert.ToInt32(tbMetronomo.Text) * 100;
                        temporizadorMetronomo.Enabled = true;
                        winamp.Stream = global::demorejilla.Properties.Resources.Beep;
                        winamp.Play();
                    }
                }
                else if (rbMusica.Checked)
                {
                    if (this.listaMostrar.Items.Count == 0)
                        //throw new RejillaException("La lista de música esta vacia.");
                        throw new RejillaException(rm.GetString("war01"));
                    else
                    {
                        try
                        {
                            if (this.listaMostrar.SelectedItem == null)
                                this.listaMostrar.SelectedIndex = 0;

                            crearFicheroReproduccion();
                            this.reproductorWindowsMedia.URL = this.rutaFicheroM3u;
                            this.reproductorWindowsMedia.Ctlcontrols.play();
                        }
                        catch (IOException iexcp)
                        {
                            MessageBox.Show(iexcp.Message);
                        }
                    }
                }
            }
            catch (RejillaException re)
            {
                MessageBox.Show(re.Message, rm.GetString("war00")/*"Advertencia"*/, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            catch (InvalidOperationException ioe)
            {
                //MessageBox.Show("El archivo de sonido esta dañado ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MessageBox.Show(rm.GetString("error21"), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ioe.Message.ToString();
            }
            catch (ArgumentException arexc)
            {
                //MessageBox.Show("El valor del metronomo tiene que ser MAYOR que CERO.","Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MessageBox.Show(rm.GetString("error32"), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /*
         * Método que crea en un fichero la lista de reproducción
         * que tenemos en la lista de canciones, las pasa a fichero y
         * la reproduce
         */
        private void crearFicheroReproduccion()
        {
            string cadena;
            using (StreamWriter sw = new StreamWriter(File.Open(this.rutaFicheroM3u, FileMode.Create), Encoding.Default))
            {
                sw.WriteLine("#EXTM3U");
                foreach (string s in this.listaMusica.Items)
                {
                    cadena = s.Substring(s.LastIndexOf('\\') + 1);
                    //sw.WriteLine("#EXTINF:0," + s.Substring(s.LastIndexOf('\\') + 1));
                    sw.WriteLine("#EXTINF:-1," + s.Substring(s.LastIndexOf(Path.DirectorySeparatorChar) + 1));
                    sw.WriteLine(s);
                    sw.WriteLine();
                }
                sw.Close();
                //2024 Se indica en #EXTINF un -1 para indicar que no se sabe el tiempo de duración de la canción.
            }
        }

        /*
         * Método que borra una canción de la lista
         */
        private void btBorrarCancion_Click(object sender, EventArgs e)
        {
            if (this.listaMostrar.SelectedIndex < 0) return;
            List<int> listaBorrar = new List<int>();
            foreach (int indice in this.listaMostrar.SelectedIndices)
                listaBorrar.Add(indice);

            listaBorrar.Reverse();

            foreach (int ind in listaBorrar)
            {
                this.listaMostrar.Items.RemoveAt(ind);
                this.listaMusica.Items.RemoveAt(ind);
            }
            this.reproductorWindowsMedia.Ctlcontrols.stop();
        }

        private void btStop_Click(object sender, EventArgs e)
        {
            this.winamp.Stop();
            this.reproductorWindowsMedia.Ctlcontrols.stop();
            if (this.temporizadorMetronomo != null)
                temporizadorMetronomo.Enabled = false;
        }

        /*
         * Si se para el reproductor que siga sonando hast que se termine 
         * de realizar la rejilla entera.
         */
        private void reproductorWindowsMedia_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        {
            if (this.loopCanciones)
            {
                if (e.newState != 3 || e.newState != 2)
                    this.reproductorWindowsMedia.Ctlcontrols.play();
            }
        }

        /*
         * ***************************************************************
         * REJILLA NUMERICA
         * ***************************************************************
         */

        /*
         * Procedimiento que me devuelve el valor de incrementoBotones
         * sino se selecciona ninguno, por defecto es 1 
         */
        private void comboIncrementoBotones_SelectedIndexChanged(object sender, EventArgs e)
        {
            String valor = (string)this.comboIncrementoBotones.SelectedItem;
            incrementoNext = System.Convert.ToInt32(valor);
        }

        private void rbAscendente_CheckedChanged(object sender, EventArgs e)
        {
            this.etIncrementoRejilla.Text = rm.GetString("cad09");//"Incremento de los valores";
            this.etOrdenGrafica.Text = rm.GetString("txt01");
        }

        private void rbDescendente_Chekedchanged(object sender, EventArgs e)
        {
            this.etIncrementoRejilla.Text = rm.GetString("cad10"); //"Decremento de los valores";
            this.etOrdenGrafica.Text = rm.GetString("txt02");
        }

        /*
         * ***************************************************************
         * REJILLA DE COLORES
         * ***************************************************************
         */

        //Escoger los colores a tachar. 
        private void buttonColor_Click(object sender, EventArgs e)
        {
            DialogoColoresPaLetras dcpl = new DialogoColoresPaLetras(this.plantillaColoresRejilla, false, this.rutaCarpeta);
            Button b = (Button)sender;
            DialogResult respuesta = dcpl.ShowDialog();
            int semilla = (int)DateTime.Now.Ticks;
            Random rnd = new Random(semilla);
            if (respuesta == DialogResult.OK)
            {
                if (dcpl.colorcito != Color.Empty)
                {
                    this.listaBotonesColores.Remove(b.BackColor);
                    b.BackColor = dcpl.colorcito;
                    this.listaBotonesColores.Add(b.BackColor);
                    this.botonTamaño.BackColor = dcpl.colorcito;
                    this.botonTamaño.Text = "";
                }
                if (!dcpl.colores.contieneColor(b.BackColor))
                {
                    this.listaBotonesColores.Remove(b.BackColor);
                    b.BackColor = SystemColors.Control;// Color.Transparent;
                    if (this.listaBotonesColores.Count == 0)
                        this.botonTamaño.BackColor = SystemColors.Control; //Color.Transparent;//Color.Empty;
                    else
                        this.botonTamaño.BackColor = this.listaBotonesColores.ElementAt(/*new Random()*/rnd.Next(0, this.listaBotonesColores.Count));
                }
            }
            else if (respuesta == DialogResult.Ignore)
            {
                this.listaBotonesColores.Remove(b.BackColor);
                b.BackColor = SystemColors.Control; //Color.Transparent;//Color.Empty;
                if (this.listaBotonesColores.Count == 0)
                    this.botonTamaño.BackColor = SystemColors.Control; //Color.Transparent;//Color.Empty;
                else
                    this.botonTamaño.BackColor = this.listaBotonesColores.ElementAt(/*new Random()*/rnd.Next(0, this.listaBotonesColores.Count));
            }
            this.plantillaColoresRejilla = dcpl.colores;
        }

        /*
         * Método que me indica la cantidad de botones que apareceran en la rejilla
         * de colores.
         */
        private void tbCantidadBotones_ValueChanged(object sender, EventArgs e)
        {
            /*
             * Devuelve un valor del 0 al 3
             * El 0 --> 0%, 1 --> 25%, 2 --> 50%, 3 --> 75%
             */
            switch (this.tbCantidadBotones.Value)
            {
                case 0:
                    this.minimoCantidadBotonesColores = TantoPorCiento.CERO;
                    break;
                case 1:
                    this.minimoCantidadBotonesColores = TantoPorCiento.VEINTICINCO;
                    break;
                case 2:
                    this.minimoCantidadBotonesColores = TantoPorCiento.CINCUENTA;
                    break;
                case 3:
                    this.minimoCantidadBotonesColores = TantoPorCiento.SETENTAYCINCO;
                    break;
            }
        }

        /*
         * ***************************************************************
         * REJILLA DE LETRAS 
         * ***************************************************************
         */

        /*
         * Metodo para seleccionar las letras a identificar de la rejilla
         */
        private void cbLetras_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cb = (ComboBox)sender;
            String s = cb.SelectedItem.ToString();
            if (this.btLetras1.Text == "")
            {
                this.btLetras1.Text = s;
            }
            else if (this.btLetras2.Text == "")
            {
                this.btLetras2.Text = s;
            }
            else if (this.btLetras3.Text == "")
            {
                this.btLetras3.Text = s;
            }
            else if (this.btLetras4.Text == "")
            {
                this.btLetras4.Text = s;
            }
            else if (this.btLetras5.Text == "")
            {
                this.btLetras5.Text = s;
            }
            else if (this.btLetras6.Text == "")
            {
                this.btLetras6.Text = s;
            }
            else if (this.btLetras7.Text == "")
            {
                this.btLetras7.Text = s;
            }
            else if (this.btLetras8.Text == "")
            {
                this.btLetras8.Text = s;
            }
            else if (this.btLetras9.Text == "")
            {
                this.btLetras9.Text = s;
            }
            botonTamaño.Text = listaLetras.ElementAt(new Random((int)DateTime.Now.Ticks).Next(0, this.listaLetras.Count));
            this.listaLetrasTachar.Add(s);
        }

        /*
         * Añadir letras a la lista de items.
         */
        private void btAñadirLetras_Click(object sender, EventArgs e)
        {
            DialogoLetras diagLetras;
            if (this.datosCargadosBD)
            {
                diagLetras = new DialogoLetras(this.listaLetras, this.fuenteBD);
            }
            else
            {
                diagLetras = new DialogoLetras();
            }

            //DialogoLetras diagLetras = new DialogoLetras();
            DialogResult dr = diagLetras.ShowDialog();
            int semilla = (int)DateTime.Now.Ticks;
            Random rnd = new Random(semilla);

            if (dr == DialogResult.OK)
            {
                if (!this.datosCargadosBD)
                {
                    this.listaLetras.Clear();
                    cbLetras.Items.Clear();
                }
                else
                {
                    //al caragar los datos de la base de datos, se rellena el comboBox de las letras, por lo que lo borramos.
                    this.cbLetras.Items.Clear();
                }
                borrarBotonesLetras();

                this.listaLetras = diagLetras.listaLetras;
                this.listaLetras.Sort(); //ordenamos la lista de letras 
                cbLetras.Font = diagLetras.fuenteActual;
                this.fuenteFamilia = diagLetras.fuenteActual.FontFamily;
                Font ff = new Font(diagLetras.fuenteActual.FontFamily, btLetras1.Width / 2);
                cambiarFuenteBotones(ff);
                cbLetras.Items.AddRange(this.listaLetras.ToArray());
                botonTamaño.Font = new Font(diagLetras.fuenteActual.FontFamily, botonTamaño.Width / 2);
                if (this.listaLetras.Count != 0)
                    botonTamaño.Text = listaLetras.ElementAt(rnd.Next(0, this.listaLetras.Count));
            }
        }

        /*
         * cambiar la fuente de las letras a identificar.
         */
        private void cambiarFuenteBotones(Font f)
        {
            btLetras1.Font = f;
            btLetras2.Font = f;
            btLetras3.Font = f;
            btLetras4.Font = f;
            btLetras5.Font = f;
            btLetras6.Font = f;
            btLetras7.Font = f;
            btLetras8.Font = f;
            btLetras9.Font = f;
        }

        /*
         * Borrar todos los botones de letras
         */
        private void borrarBotonesLetras()
        {
            this.btLetras1.Text = "";
            this.btLetras2.Text = "";
            this.btLetras3.Text = "";
            this.btLetras4.Text = "";
            this.btLetras5.Text = "";
            this.btLetras6.Text = "";
            this.btLetras7.Text = "";
            this.btLetras8.Text = "";
            this.btLetras9.Text = "";
        }

        /*
         * Borrar la lista de items
         */
        private void btBorrarLista_Click(object sender, EventArgs e)
        {
            cbLetras.Items.Clear();
            this.listaLetras.Clear();
            borrarBotonesLetras();
            this.botonTamaño.Text = "";
        }

        /*
         * Borra la letra del boton pulsado
         */
        private void btLetrasBorrar_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            this.listaLetrasTachar.Remove(b.Text);
            b.Text = "";
            if (this.listaLetras.Count != 0)
                this.botonTamaño.Text = this.listaLetras.ElementAt(new Random((int)DateTime.Now.Ticks).Next(0, this.listaLetras.Count));
        }

        /*
         * Cantidad minima de letras que apareceran en la rejilla
         */
        private void radioButtonLetras_CheckedChanged(object sender, EventArgs e)
        {
            if (rbLetrasCero.Checked)
                minimoLetras = TantoPorCiento.CERO;
            else if (rbLetras25.Checked)
                minimoLetras = TantoPorCiento.VEINTICINCO;
            else if (rbLetras50.Checked)
                minimoLetras = TantoPorCiento.CINCUENTA;
            else if (rbLetras75.Checked)
                minimoLetras = TantoPorCiento.SETENTAYCINCO;
        }

        /*
         * ***************************************************************
         * REJILLA IMÁGENES
         * ***************************************************************
         */

        private void tbCantidadBotonesImagenes_ValueChanged(object sender, EventArgs e)
        {
            /*
             * Devuelve un valor del 0 al 3
             * El 0 --> 0%, 1 --> 25%, 2 --> 50%, 3 --> 75%
             */
            switch (tbCantidadBotonesImagenes.Value)
            {
                case 0:
                    this.minimoCantidadBotonesImagenes = TantoPorCiento.CERO;
                    break;
                case 1:
                    this.minimoCantidadBotonesImagenes = TantoPorCiento.VEINTICINCO;
                    break;
                case 2:
                    this.minimoCantidadBotonesImagenes = TantoPorCiento.CINCUENTA;
                    break;
                case 3:
                    this.minimoCantidadBotonesImagenes = TantoPorCiento.SETENTAYCINCO;
                    break;
            }
        }

        /*
         * Seleccionar las imagenes a identificar en la rejilla
         */
        private void buttonImagenes_Click(object sender, EventArgs e)
        {
            DialogoImagenes di = new DialogoImagenes(this.imagenes, this.rutaCarpeta);
            DialogResult dr = di.ShowDialog();
            int semilla = (int)DateTime.Now.Ticks;
            Random rnd = new Random(semilla);
            if (di.DialogResult == DialogResult.OK)
            {
                Button b = (Button)sender;
                this.listaImagenesEscogidas.Remove(b.BackgroundImage);

                b.BackgroundImageLayout = ImageLayout.Stretch;
                b.BackgroundImage = di.imagen;
                this.botonTamaño.BackgroundImageLayout = ImageLayout.Stretch;
                this.botonTamaño.BackgroundImage = di.imagen;
                this.listaImagenesEscogidas.Add(di.imagen);
            }
            else if (di.DialogResult == DialogResult.Ignore)
            {
                //borra la imagen del boton
                Button b = (Button)sender;
                b.BackgroundImageLayout = ImageLayout.Stretch;
                this.listaImagenesEscogidas.Remove(b.BackgroundImage);
                b.BackgroundImage = di.imagen;
                if (listaImagenesEscogidas.Count == 0)
                    this.botonTamaño.BackgroundImage = null;
                else
                    this.botonTamaño.BackgroundImage = this.listaImagenesEscogidas.ElementAt(rnd.Next(0, this.listaImagenesEscogidas.Count));
            }
            this.listaImagenes = di.claseIman.obtenerTodasImagenes();
            this.imagenes = di.claseIman;
        }

        /*
         * ***************************************************************
         * REJILLA ABECEDARIO
         * ***************************************************************
         */

        /*
         * Mayúsculas o minusculas
         */
        private void tbAbecedario_TextChanged(object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (rbAbcMinusculas.Checked)
                this.botonTamaño.Text = tb.Text.ToLower();
            else
                this.botonTamaño.Text = tb.Text;
        }

        /*
         * Opciones de la rejilla abecedario
         * MAYUSCULAS
         * minusculas
         * ReJiLlA
         * rEjIlLa
         * Aleatorio
         */
        private void rellenarOpcionesAbecedario(int posicion)
        {
            for (int i = 0; i < this.opcionesAbecedario.Length; i++)// in this.opcionesAbecedario)
                this.opcionesAbecedario[i] = false;

            this.opcionesAbecedario[posicion] = true;
        }

        /*
         * Orden de tachar las letras del abecedario,
         * ascendente [A -> Z]o descendente [Z -> A]
         */
        private void rbAbcOrdenTachado_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rbAscAbecedario.Checked)
            {
                this.ordenTachadoAbc = true;
                this.etOrdenGrafica.Text = rm.GetString("txt01");
            }
            if (this.rbDescAbecedario.Checked)
            {
                this.ordenTachadoAbc = false;
                this.etOrdenGrafica.Text = rm.GetString("txt02");
            }
        }

        private void rbMinusculasMayusculas_CheckedChanged(object sender, EventArgs e)
        {
            int posicionValor = 0;
            if (this.rbAbcMayusculas.Checked)
                //minusculas = false;
                posicionValor = 0;
            if (this.rbAbcMinusculas.Checked)
                //minusculas = true;
                posicionValor = 1;
            if (this.rbAbcAleatorio.Checked)
                posicionValor = 2;
            if (this.rbAbcMAYUSminus.Checked)
                posicionValor = 3;
            if (this.rbAbcminusMAYUS.Checked)
                posicionValor = 4;

            rellenarOpcionesAbecedario(posicionValor);
            int semilla = (int)DateTime.Now.Ticks;
            Random rnd = new Random(semilla);

            if (tbAbecedario.Text.Length != 0)
            {
                if (this.opcionesAbecedario[0] || this.opcionesAbecedario[3])
                    botonTamaño.Text = this.tbAbecedario.Text.ToUpper();
                else if (this.opcionesAbecedario[1] || this.opcionesAbecedario[4])
                    botonTamaño.Text = this.tbAbecedario.Text.ToLower();
                else if (this.opcionesAbecedario[2])
                {
                    //rnd = new Random().Next(0, 2);
                    if (rnd.Next(0, 2) == 0)
                        botonTamaño.Text = this.tbAbecedario.Text.ToUpper();
                    else
                        botonTamaño.Text = this.tbAbecedario.Text.ToLower();
                }
            }
        }

        private void activarSegunOpcionAbecedario(int p)
        {
            if (p == 0)
                this.rbAbcMayusculas.Checked = true;
            if (p == 1)
                this.rbAbcMinusculas.Checked = true;
            if (p == 2)
                this.rbAbcAleatorio.Checked = true;
            if (p == 3)
                this.rbAbcMAYUSminus.Checked = true;
            if (p == 4)
                this.rbAbcminusMAYUS.Checked = true;

            rellenarOpcionesAbecedario(p);
        }

        /*
         * ***************************************************************
         * REJILLA WINGDINGS
         * ***************************************************************
         */

        /*
         * Metodo para seleccionar los items que formarán parte de la rejilla
         * y también los que el usuario deberá identificar y tachar en la rejilla
         */
        private void btWi_Click(object sender, EventArgs e)
        {
            DialogoTodasWingdings di;

            if (this.datosWinCargadosBD)
            {
                di = new DialogoTodasWingdings(this.listaCompletaObjetos, this.listaSeleccionados);
            }
            else
            {
                di = new DialogoTodasWingdings();
                this.listaSeleccionados.Clear();
                this.listaCompletaObjetos.Clear();
            }

            //DialogoTodasWingdings di = new DialogoTodasWingdings();
            DialogResult dr = di.ShowDialog();
            Random rnd = new Random();

            if (dr == DialogResult.OK)
            {

                //this.listaSeleccionados.Clear();
                //this.listaCompletaObjetos.Clear();

                this.listaSeleccionados = di.listaEscogidos;
                this.listaSeleccionados.OrderBy(TipoLetraFuente => TipoLetraFuente.devolverLetra());

                this.listaCompletaObjetos = di.listaCompleta;
                int elegido = rnd.Next(0, this.listaCompletaObjetos.Count);
                if (this.listaCompletaObjetos.Count != 0)
                {
                    this.botonTamaño.Font = this.listaCompletaObjetos.ElementAt(elegido).devolverFuente();
                    FontFamily ff = new FontFamily(this.botonTamaño.Font.Name);
                    this.botonTamaño.Font = new Font(ff, this.botonTamaño.Size.Width / 2);
                    this.botonTamaño.Text = this.listaCompletaObjetos.ElementAt(elegido).devolverLetra();
                }
            }
        }

        /*
         * Cantidad mínima de items que aparecerán
         */
        private void radioButtonWingdings_ChekedChanged(object sender, EventArgs e)
        {
            if (this.rbWinAleatorio.Checked)
                minimoWin = TantoPorCiento.CERO;
            else if (rbWin25.Checked)
                minimoWin = TantoPorCiento.VEINTICINCO;
            else if (rbWin50.Checked)
                minimoWin = TantoPorCiento.CINCUENTA;
            else if (rbWin75.Checked)
                minimoWin = TantoPorCiento.SETENTAYCINCO;
        }

        /*
         * Método que visualiza por pantalla los items que el sujeto
         * debe identificar en la rejilla. Se ejecuta al hacer click
         * en el botón corespondienbte y se muestra en una 
         * ventana aparte.
         */
        private void btVisualizar_Click(object sender, EventArgs e)
        {
            if (this.listaSeleccionados.Count == 0)
                //MessageBox.Show("WINDINGS: No hay ningún item para identificar por el usuario.", "INFORMACIÓN", MessageBoxButtons.OK, MessageBoxIcon.Information);
                MessageBox.Show(rm.GetString("error22"), rm.GetString("cad11"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
            {
                MostrarWindings mw = new MostrarWindings();
                mw.labelWin1.Text = " ";
                mw.labelWin2.Text = " ";
                mw.labelWin3.Text = " ";
                //mw.Text = "Lista de Items a tachar en la rejilla";
                mw.Text = rm.GetString("cad12");
                foreach (TipoLetraFuente tl in this.listaSeleccionados)
                {
                    if (mw.labelWin1.Font.Name.Equals(tl.devolverFuente().Name))
                    {
                        mw.labelWin1.Text += tl.devolverLetra() + " ";
                    }
                    if (mw.labelWin2.Font.Name.Equals(tl.devolverFuente().Name))
                    {
                        mw.labelWin2.Text += tl.devolverLetra() + " ";
                    }
                    if (mw.labelWin3.Font.Name.Equals(tl.devolverFuente().Name))
                        mw.labelWin3.Text += tl.devolverLetra() + " ";

                }
                mw.ShowDialog();
            }
        }

        /*
         * ***************************************************************
         * GRÁFICA DE LA REJILLA Y TIEMPOS DEL USUARIO
         * ***************************************************************
         */

        private void pictureBoxLienzoDibujo_SizeChanged(object sender, EventArgs e)
        {
            pintarGrafica();//invoca a pictureBoxLienzoDibujo_Paint
        }

        private void pintarGrafica()
        {
            this.pBLienzoDibujo.Invalidate();
        }
        /*
        * Procedimiento para pintar los tiempos en las etiquetas de la gráfica,
        * no se emplea el toString() directamente porque saca las fraccciones de misilesgundos con 
        * 8 decimales
        */
        private static string FormatTimeSpan(TimeSpan span)
        {
            string sign = String.Empty;
            return sign + //span.Days.ToString("00") + "." +
                          //span.Hours.ToString("00") + ":" +
                   span.Minutes.ToString("00") + ":" +
                   span.Seconds.ToString("00") + "." +
                   span.Milliseconds.ToString("000");
        }

        /*
         * Añadido para dibujar la grafica en el formulario inicial.
         */
        private void pictureBoxLienzoDibujo_Paint(object sender, PaintEventArgs e)
        {
            if (lienzoinicial)
            {
                //Objeto grafico del lienzo dibujo.
                Graphics grafico = e.Graphics;
                grafico.Clear(pBLienzoDibujo.BackColor);

                //varibales para escalar la funcion, valores maximos y minimos de cada eje.
                float xMin = 0, xMax = tiempoSegundos.Length - 1;
                float yMin, yMax;

                //valores del ejeX, 0, 1, 2, 3...
                float[] arrayX = new float[tiempoSegundos.Length];

                for (int indiceX = 0; indiceX < arrayX.Length; indiceX++)
                {
                    arrayX[indiceX] = indiceX;
                }

                yMax = tiempoSegundos.Max();
                /* 
                 * yMin = tiempoSegundos.Min(); Si en el eje de las y hubiesen valores
                 * negativos, entonces habria que quitar este comentario
                 * pero la y minima que vamos a tener es 0.
                 */
                yMin = 0;
                /* 
                 * Usamos un objeto de tipo Matrix para escalar y trasladar
                 * el dibujo en la grafica
                 */

                Matrix matrix = new Matrix();
                /* ESCALADO
                 * el -3 del Width es para que no pinte sobre el borde el pictureBox
                 * y el -10 es para dibujar luego los valores del eje x, el 0, 1, 2...
                 * el signo - de la parte y es para que los lvalores de y crezcan hacia arriba.
                 */
                matrix.Scale(((pBLienzoDibujo.Width - 3) / (xMax - xMin)), -(pBLienzoDibujo.Height - 10) / (yMax - yMin));
                /* TRANSLACION
                 * El origen de coordenadas esta en la esquina superior izquierda y crece hacia la derecha las x
                 * y hacia abajo las y, asi que lo trasladamos a la esquina inferior izquierda
                 * que es donde no interesa empezar a dibujar la gráfica, como si fuera un eje normal de 
                 * coordenadas
                 */

                matrix.Translate(-xMin, -yMax);

                /*
                 * rutaDibujo contendra el dibujo final de la función
                 */
                GraphicsPath rutaDibujo = new GraphicsPath();
                Pen colorGrafica = new Pen(Color.Blue, 1);

                GraphicsPath ejeX = new GraphicsPath();
                ejeX.AddLine(new PointF(xMin, 0), new PointF(xMax, 0));

                GraphicsPath ejeY = new GraphicsPath();
                ejeY.AddLine(new PointF(0, yMax), new PointF(0, yMin));

                /*
                 * Creamos el dibujo de la grafica (funcion de los tiempos medidos)
                 */
                float xAnterior = 0.0F, yAnterior = 0.0F, xActual = 0.0F, yActual = 0.0F;
                for (int recorrer = 0; recorrer < arrayX.Length; recorrer++)
                {
                    xActual = arrayX[recorrer];
                    yActual = tiempoSegundos[recorrer];
                    rutaDibujo.AddLine(xAnterior, yAnterior, xActual, yActual);
                    xAnterior = xActual;
                    yAnterior = yActual;
                }

                ejeX.Transform(matrix);
                ejeY.Transform(matrix);
                rutaDibujo.Transform(matrix);

                /*
                 * Añadimos al ejeX los valores de los numeros, 0, 1, 2, 
                 * dependiendo del numnero de valores en el ejeX
                 * dibujaremos unos puntos u otros, ya que si dibuujamos
                 * todos los puntos se solapan entre si y no se ve nada.
                 * NOTA: Hay que hacer un switch porque si son 100 numeros se solapan,
                 * solo pintar unos pocos
                 * asi que dependiendo del tamaño del ejeX, pintaremos mas numeros o menos
                 */
                int longitudEjeX = arrayX.Length;
                int incLetras = 1;
                int incIndice = 1;
                if (longitudEjeX <= 50)
                {
                    incLetras = 1;
                    incIndice = 1;
                }
                else if ((longitudEjeX > 50) && (longitudEjeX <= 100))
                {
                    incLetras = 10;
                    incIndice = 2;
                }
                else if ((longitudEjeX > 100) && (longitudEjeX <= 150))
                {
                    incLetras = 15;
                    incIndice = 3;
                }
                else if ((longitudEjeX > 150) && (longitudEjeX <= 200))
                {
                    incLetras = 20;
                    incIndice = 4;
                }
                else
                {
                    incLetras = 25;
                    incIndice = 5;
                }

                FontFamily fuente = new FontFamily("Arial");
                float rango = (pBLienzoDibujo.Width - 5) / (xMax - xMin);
                int letras = 0;

                //int valorEjeX = letras;
                while (letras <= longitudEjeX)
                {
                    ejeX.AddString(letras.ToString(), fuente, 2, 8F, new PointF((letras * rango) - 5, pBLienzoDibujo.Height - 10), StringFormat.GenericDefault);
                    //ejeX.AddString((Convert.ToInt32(this.comienzoRejilla)+valorEjeX).ToString(), fuente, 2, 8F, new PointF((letras * rango) - 5, pBLienzoDibujo.Height - 10), StringFormat.GenericDefault);
                    letras += incLetras;
                    //valorEjeX += incLetras + this.incrementoNext;
                }
                /*
                 * Creamos unos ejes paralelos al ejeY para mejorar la gráfica,
                 * en algunos puntos del ejeX.
                 */
                GraphicsPath[] ejesY = new GraphicsPath[arrayX.Length + 1];
                int indice = 0;
                int posicionDibujar = 0;
                while (indice <= longitudEjeX)
                {
                    ejesY[indice] = new GraphicsPath();
                    ejesY[indice].AddLine(new PointF(posicionDibujar, yMax), new PointF(posicionDibujar, yMin));
                    ejesY[indice].Transform(matrix);
                    indice++;
                    posicionDibujar += incIndice;
                }
                /*
                 * dibujamos los ejes
                 * y la funcion, en alta calidad.
                 */
                grafico.SmoothingMode = SmoothingMode.HighQuality;
                grafico.DrawPath(colorGrafica, rutaDibujo);
                grafico.DrawPath(new Pen(Color.Red, 1), ejeX);
                grafico.DrawPath(new Pen(Color.Pink, 1), ejeY);
                /*
                 * dibujamos los ejes paralelos al ejeY
                 */
                for (int ind = 0; ind < ejesY.Length; ind++)
                {
                    grafico.DrawPath(new Pen(Color.Black, 1), ejesY[ind]);
                }

                if (this.botonPulsado)
                {
                    grafico.DrawLine(Pens.Green, x1, y1, x2, y2);
                    //Convertir pixeles a coordenadas logicas
                    float x1log = xMin + x1 * (xMax - xMin) / (pBLienzoDibujo.Width - 3);
                    tbCoordX.Text = Math.Truncate(x1log).ToString();
                    //if (this.rejillaTipo == TipoRejilla.NUMERICA)
                    //    tbCoordX.Text = (Math.Truncate(x1log) + Convert.ToInt32(this.comienzoRejilla) + this.incrementoNext).ToString();

                    int valorX = Convert.ToInt32(Math.Truncate(x1log));
                    if (valorX < tiempoSegundos.Length)
                    {
                        TimeSpan ts = new TimeSpan(((long)(tiempoSegundos[valorX] * 1e9 / 100)));
                        tbCoordY.Text = FormatTimeSpan(ts);
                    }
                }
            }//Fin del si se ha pulsado algun boton para dibujar
        }

        /*
         * Procedimiento que nos reajusta la grafica en el caso de que no termines
         * la rejilla entera o que cerremos el formulario de la rejilla, lo que hace es 
         * ajustar la grafica a la cantidad de los botones que llevamos pulsados.
         * Y si la hemos terminado entera la dibuja entera.
         */
        private void dibujarGrafica(bool dibujar, DateTime[] matrizDT, bool dateOSpan, TimeSpan[] vectorTiempos)
        {
            DateTime[] mat = new DateTime[1];
            if (matrizDT != null)
                mat = new DateTime[matrizDT.Length];

            if (dibujar)
            {
                if (dateOSpan) //si es true dibujar como siempre, los tiempos de los botones
                {
                    this.etTiempoTranscurridoEmparejar.Visible = false;
                    this.etValorMediaEmparejar.Visible = false;
                    this.etValorMinimoEmparejar.Visible = false;
                    this.etValorMaximoEmparejar.Visible = false;
                    this.etVarianzaEmparejar.Visible = false;

                    this.etValorEmparejarCero1.Visible = false;
                    this.etValorEmparejarCero2.Visible = false;
                    this.etValorEmparejarCero3.Visible = false;
                    this.etValorEmparejarCero4.Visible = false;
                    this.etValorEmparejarCero5.Visible = false;

                    this.etTiempoTranscurrido.Visible = true;
                    this.etValorMedia.Visible = true;
                    this.etValorMaximo.Visible = true;
                    this.etValorMinimo.Visible = true;
                    this.etVarianza.Visible = true;

                    this.etValorTiempoCorregido.Visible = true;
                    this.etValorTiempoCorreccionEfectuada.Visible = true;
                    this.etValorTiempoTareaPreliminar.Visible = true;
                    this.etValorMediaCorreccion.Visible = true;
                    this.etValorVarianzaCorreccion.Visible = true;


                    mat = matrizDT;
                    tiempos = new TimeSpan[mat.Length - 1];

                    for (int iTiempo = 0; iTiempo < tiempos.Length; iTiempo++)
                    {
                        tiempos[iTiempo] = new TimeSpan(0L);
                    }

                    tiempoSegundos = new float[mat.Length - 1];
                    for (int i = 0; i < mat.Length - 1; i++)
                    {
                        tiempos[i] = (mat[i + 1] - mat[i]);
                        tiempoSegundos[i] = (float)tiempos[i].TotalSeconds;
                        //Si salgo antes de terminar la rejilla meto un cero
                        if (tiempoSegundos[i] < 0) tiempoSegundos[i] = 0;
                    }

                    //Buscamos si hay tiempoCero en el array, si lo hay es que no hemos terminado la rejilla entera
                    int posicionDelCero = 0;
                    while (posicionDelCero < tiempoSegundos.Length && tiempoSegundos[posicionDelCero] != 0)
                    {
                        posicionDelCero++;
                    }
                    /*
                     * Si posicionDelCero es menor que la longitud del array --> significa que hay un cero en medio
                     *      por lo que no hemos terminado la rejilla entera, asi que cogemos 
                     *      y nos quedamos con los datos hasta la posicionDelCero, que son los datos
                     *      buenos, ya que los otros pueden contener basura, y solo nos interesan esos primeros.
                     *      ASi que cogemos y cribamos los arrays y nos quedamos con las n-primeras posiciones.
                     */
                    if (posicionDelCero < tiempoSegundos.Length)
                    {
                        float[] tiempoSegundos2 = new float[posicionDelCero];
                        TimeSpan[] tiempos2 = new TimeSpan[posicionDelCero];
                        DateTime[] mat2 = new DateTime[posicionDelCero + 1];
                        for (int copiando = 0; copiando < posicionDelCero; copiando++)
                        {
                            tiempos2[copiando] = tiempos[copiando];
                            tiempoSegundos2[copiando] = tiempoSegundos[copiando];
                            mat2[copiando] = mat[copiando];
                        }
                        mat2[posicionDelCero] = mat[posicionDelCero];
                        tiempoSegundos = new float[tiempoSegundos2.Length];
                        tiempoSegundos2.CopyTo(tiempoSegundos, 0);
                        tiempos = new TimeSpan[tiempos2.Length];
                        tiempos2.CopyTo(tiempos, 0);
                        mat = new DateTime[mat2.Length];
                        mat2.CopyTo(mat, 0);
                    }

                    pintarGrafica();//invoca a paint de pictureBox

                    this.etValorMaximo.Text = FormatTimeSpan(tiempos.Max());
                    this.etValorMinimo.Text = FormatTimeSpan(tiempos.Min());
                    this.etTiempoTranscurrido.Text = FormatTimeSpan(mat[mat.Length - 1] - mat[0]);

                    float media = tiempoSegundos.Average();
                    TimeSpan ts = new TimeSpan((long)(media * 1e9 / 100));
                    this.etValorMedia.Text = FormatTimeSpan(ts);

                    double suma = 0;
                    for (int indiceVarianza = 0; indiceVarianza < tiempoSegundos.Length; indiceVarianza++)
                    {
                        //suma += Math.Pow((tiempoSegundos[indiceVarianza] - media), 2);
                        suma += Math.Pow(tiempoSegundos[indiceVarianza], 2);
                    }
                    double varianza = (suma / tiempoSegundos.Length) - (media * media);
                    double desviacionTipica = Math.Sqrt(varianza);

                    TimeSpan tv = new TimeSpan((long)(varianza * 1e9 / 100));
                    this.etVarianza.Text = FormatTimeSpan(tv);

                    this.etValorTiempoCorreccionEfectuada.Text = FormatTimeSpan(this.tiempoCorreccionEfectuada);

                    this.etValorTiempoCorregido.Text = FormatTimeSpan(this.tiempoCorregido);
                }
                else if (!dateOSpan && vectorTiempos != null) //si es false dibujo el span, los tiempos de latencia
                {
                    this.etTiempoTranscurridoEmparejar.Visible = true;
                    this.etValorMediaEmparejar.Visible = true;
                    this.etValorMinimoEmparejar.Visible = true;
                    this.etValorMaximoEmparejar.Visible = true;
                    this.etVarianzaEmparejar.Visible = true;

                    this.etValorEmparejarCero1.Visible = true;
                    this.etValorEmparejarCero2.Visible = true;
                    this.etValorEmparejarCero3.Visible = true;
                    this.etValorEmparejarCero4.Visible = true;
                    this.etValorEmparejarCero5.Visible = true;

                    this.etTiempoTranscurrido.Visible = false;
                    this.etValorMedia.Visible = false;
                    this.etValorMaximo.Visible = false;
                    this.etValorMinimo.Visible = false;
                    this.etVarianza.Visible = false;


                    this.etValorTiempoCorregido.Visible = false;
                    this.etValorTiempoCorreccionEfectuada.Visible = false;
                    this.etValorTiempoTareaPreliminar.Visible = false;
                    this.etValorMediaCorreccion.Visible = false;
                    this.etValorVarianzaCorreccion.Visible = false;



                    tiempoSegundos = new float[vectorTiempos.Length];
                    for (int i = 0; i < vectorTiempos.Length; i++)
                    {
                        tiempoSegundos[i] = (float)vectorTiempos[i].TotalSeconds;
                        if (tiempoSegundos[i] < 0) tiempoSegundos[i] = 0;
                    }

                    int posicionDelCero = 0;
                    while (posicionDelCero < tiempoSegundos.Length && tiempoSegundos[posicionDelCero] != 0)
                    {
                        posicionDelCero++;
                    }

                    if (posicionDelCero < tiempoSegundos.Length)
                    {
                        float[] tiempoSegundos2 = new float[posicionDelCero];
                        TimeSpan[] tiempos2 = new TimeSpan[posicionDelCero];

                        for (int copiando = 0; copiando < posicionDelCero; copiando++)
                        {
                            tiempos2[copiando] = vectorTiempos[copiando];
                            tiempoSegundos2[copiando] = tiempoSegundos[copiando];

                        }
                        tiempoSegundos = new float[tiempoSegundos2.Length];
                        tiempoSegundos2.CopyTo(tiempoSegundos, 0);
                        tiempos = new TimeSpan[tiempos2.Length];
                        tiempos2.CopyTo(tiempos, 0);
                    }
                    pintarGrafica();
                    if (tiempos.Length > 0)
                    {

                        this.etValorMaximoEmparejar.Text = FormatTimeSpan(tiempos.Max());
                        this.etValorMinimoEmparejar.Text = FormatTimeSpan(tiempos.Min());
                        double sumaTot = 0;
                        sumaTot = tiempoSegundos.Sum();
                        TimeSpan tt = new TimeSpan((long)(sumaTot * 1e9 / 100));
                        this.etTiempoTranscurridoEmparejar.Text = FormatTimeSpan(tt);

                        float media = tiempoSegundos.Average();
                        TimeSpan ts = new TimeSpan((long)(media * 1e9 / 100));
                        this.etValorMediaEmparejar.Text = FormatTimeSpan(ts);

                        double suma = 0;
                        for (int indiceVarianza = 0; indiceVarianza < tiempoSegundos.Length; indiceVarianza++)
                        {
                            suma += Math.Pow(tiempoSegundos[indiceVarianza], 2);
                        }
                        double varianza = (suma / tiempoSegundos.Length) - (media * media);
                        double desviacionTipica = Math.Sqrt(varianza);

                        TimeSpan tv = new TimeSpan((long)(varianza * 1e9 / 100));
                        this.etVarianzaEmparejar.Text = FormatTimeSpan(tv);
                    }
                    else
                    {
                        //creamos un array de tiempo segundos de dos items
                        //para que luego el picturebox_paint no 
                        //de fallo al calular el maximo de tiempoSegundos.
                        tiempoSegundos = new float[2];
                        tiempoSegundos[0] = 0;
                        tiempoSegundos[1] = 0;
                    }
                }
            }
            //Si hay tarea preliminar dibujar sus tiempos
            if (cbTareaPreliminar.Checked && dateOSpan)
            {
                double auxiliar;
                double mediaCorreccion;
                double sumaCorreccion = 0;
                double varianzaCorreccion = 0;
                auxiliar = this.tiempoTareaInicial.TotalSeconds;
                auxiliar = auxiliar * (mat.Length - 2);
                TimeSpan tAuxiliar = new TimeSpan((long)(auxiliar * 1e9 / 100));

                TimeSpan tiempoTrascurrido = new TimeSpan();
                tiempoTrascurrido = (mat[mat.Length - 1] - mat[0]) - tAuxiliar;

                //Si la corrección es mayor a cero, que salga en la grafica, sino nada.
                if (tiempoTrascurrido > new TimeSpan(0))
                {
                    this.etValorTiempoCorreccionEfectuada.Text = FormatTimeSpan(tAuxiliar);
                    this.etValorTiempoCorregido.Text = FormatTimeSpan(tiempoTrascurrido);

                    mediaCorreccion = (this.tiempoSegundos.Sum() - (this.tiempoTareaInicial.TotalSeconds * this.tiempoSegundos.Length)) / this.tiempoSegundos.Length;
                    TimeSpan tiempoValorMediaCorreecion = new TimeSpan((long)(mediaCorreccion * 1e9 / 100));
                    this.etValorMediaCorreccion.Text = FormatTimeSpan(tiempoValorMediaCorreecion);

                    for (int indiceCorreccionvarianza = 0; indiceCorreccionvarianza < tiempoSegundos.Length; indiceCorreccionvarianza++)
                    {
                        sumaCorreccion += Math.Pow((tiempoSegundos[indiceCorreccionvarianza] - tiempoTareaInicial.TotalSeconds), 2);
                    }
                    varianzaCorreccion = (sumaCorreccion / tiempoSegundos.Length) - (mediaCorreccion * mediaCorreccion);
                    TimeSpan tvc = new TimeSpan((long)(varianzaCorreccion * 1e9 / 100));

                    this.etValorVarianzaCorreccion.Text = FormatTimeSpan(tvc);
                }
                else
                    this.etValorTiempoTareaPreliminar.Text = "00:00.000";
            }
            else
            {
                //this.etValorTiempoCorreccionEfectuada.Text = FormatTimeSpan(this.tiempoCorreccionEfectuada);
                //this.etValorTiempoCorregido.Text = FormatTimeSpan(this.tiempoCorregido);
            }
        }

        /*
         * ***************************************************************
         * DIBUJAR UNA LINEA EN LA FUNCION CON EL RATON
         * ***************************************************************
         */
        private void pBLienzoDibujo_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                botonPulsado = true;
                if (e.X < 0)
                    x1 = 0;
                else
                    x1 = e.X;

                y1 = 0;
                x2 = e.X;
                y2 = pBLienzoDibujo.Height;
                pBLienzoDibujo.Invalidate();
            }
        }

        private void pBLienzoDibujo_MouseUp(object sender, MouseEventArgs e)
        {
            botonPulsado = false;
            pBLienzoDibujo.Invalidate(); //borrar el cursor
            tbCoordX.Text = "";
            tbCoordY.Text = "";
        }

        /*
         * ***************************************************************
         * BARRA DE MENU. SALIR.
         * ***************************************************************
         */

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
            //2024 - Guardar el codigo del usuario actual en la BD.
            actualizarUsuarioDefecto();
        }

        /*
         * ***************************************************************
         * BARRA DE MENU ARCHIVOS. ABRIR Y GUARDAR.
         * ***************************************************************
         */

        /*
         * Metodo que guarda los datos en un fichero. Se elige el fichero donde se va a guardar,
         * si le fichero exixste se añaden al final del mismo y sino existe se crea uno nuevo
         * con el nombre dado por el usuario.
         * Una vez que tenemos el fichero se llama a guardardatosFichero para guardar los datos.
         */
        private void guardarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileStream fs;
            String rutaFichero;
            SaveFileDialog cajaGuardarFichero = new SaveFileDialog();
            cajaGuardarFichero.InitialDirectory = this.rutaCarpeta;
            //cajaGuardarFichero.Filter = "Ficheros de datos tbl (*.tbl)|*.tbl";
            cajaGuardarFichero.Filter = rm.GetString("cad13");
            cajaGuardarFichero.RestoreDirectory = true;
            cajaGuardarFichero.OverwritePrompt = false;
            cajaGuardarFichero.AddExtension = true;
            cajaGuardarFichero.DefaultExt = "tbl";

            DialogResult dr = cajaGuardarFichero.ShowDialog();
            if (dr == DialogResult.OK)
            {
                rutaFichero = cajaGuardarFichero.FileName;
                fs = new FileStream(rutaFichero, FileMode.Append, FileAccess.Write);
                //si el fichero existe añado datos al final y sino existe se crea
                guardarDatosFichero(fs);
                fs.Close();
            }
        }

        /*
         * Guarda fisicamente los datos en el fichero que se le pasa como argumento.
         */
        private void guardarDatosFichero(FileStream fs)
        {
            StreamWriter sw = new StreamWriter(fs);
            //sw.Write('ç');
            sw.Write(Convert.ToChar(7));
            //fecha hora
            sw.Write(this.tbFecha.Text);//línea 0
            //sw.Write('#');
            sw.Write(Convert.ToChar(8));
            sw.WriteLine('\n');
            //nombre apellidos
            if (this.tbNombre.Text != "")//Línea 1
                sw.WriteLine(this.tbNombre.Text);
            else
                sw.WriteLine("Prueba");
            if (this.tbApellidos.Text != "") //línea 2
                sw.WriteLine(this.tbApellidos.Text);
            else
                sw.WriteLine("Prueba");

            //deporte y posicion
            if (this.tbDeporte.Text != "")
                sw.WriteLine(this.tbDeporte.Text); //linea 3
            else
                sw.WriteLine("null");

            if (this.tbPosicion.Text != "")
                sw.WriteLine(this.tbPosicion.Text); //linea 4
            else
                sw.WriteLine("null");

            //edad, sexo y pais
            if (this.tbEdad.Text != "")
                sw.WriteLine(this.tbEdad.Text); //linea 5
            else
                sw.WriteLine(0);
            sw.WriteLine(this.sexo); //linea 6
            if (this.tbPais.Text != "")
                sw.WriteLine(this.tbPais.Text);//linea 7
            else
                sw.WriteLine("null");

            //tipo rejilla
            sw.WriteLine(this.rejillaTipo);//linea 8
            //lateralidad
            sw.WriteLine(this.later); //nueva linea 8.1

            //aciertos, errores y numero casillas
            sw.WriteLine(this.aciertos);
            sw.WriteLine(this.errores);
            sw.WriteLine(this.cantidadBotones);

            //meter aqui lo del emparejamiento
            sw.WriteLine(this.cbEmparejar.Checked);
            sw.WriteLine(this.tiempoEmparejar);
            sw.WriteLine(this.posicion);
            sw.WriteLine(this.secuencia);

            //meto aqui si esta activada la casilla de colores de fondo
            sw.WriteLine(this.cbColoresFondo.Checked);


            //el valor inicial del textbox numerico
            //el valor inicial del textbox abecedario
            //la posicionValor del tipo de letra ABECEDARio
            if (textBoxValorIncial.Text == "")
                sw.WriteLine("0");
            else
                sw.WriteLine(this.textBoxValorIncial.Text);

            if (tbAbecedario.Text == "")
                sw.WriteLine("A");
            else
                sw.WriteLine(this.tbAbecedario.Text);

            sw.WriteLine(this.opcionesAbecedario.ToList().IndexOf(true));

            //tiempo aleatorio y tiempo limite
            //sino estan seleccionados escribir un valor nulo para a la hora de cargar el fichero que 
            //no hay lineas en blanco que las ignoramos y es peor.
            sw.WriteLine(this.checkBoxAleatorio.CheckState);//linea 9
            if (!this.checkBoxAleatorio.Checked) //linea 10
                sw.WriteLine("valor nulo");
            else
                sw.WriteLine(this.textBoxTiempoSegundosAleatorio.Text);

            sw.WriteLine(this.cbTiempoLimite.CheckState);//linea 11
            if (this.cbTiempoLimite.Checked)        //linea 12
                sw.WriteLine(this.tbTiempoLimite.Text);
            else
                sw.WriteLine("valor nulo");
            //distraccion linea, color y grosor
            sw.WriteLine(this.rbHorizontal.Checked);//linea 13
            sw.WriteLine(this.rbVertical.Checked);//linea 14
            sw.WriteLine(this.rbAleatorio.Checked);//linea 15
            sw.WriteLine(this.rbSinlinea.Checked);//linea 16
            if (!this.rbSinlinea.Checked)
            {
                sw.WriteLine(this.cBGrosor.SelectedItem); //linea 17
                sw.WriteLine(this.pBColor.BackColor.A); //linea 18
                sw.WriteLine(this.pBColor.BackColor.R); //linea 19
                sw.WriteLine(this.pBColor.BackColor.G); //linea 20
                sw.WriteLine(this.pBColor.BackColor.B); //linea 21
            }
            else
            {
                sw.WriteLine("Sin grosor");
                sw.WriteLine("Sin A");
                sw.WriteLine("Sin R");
                sw.WriteLine("Sin G");
                sw.WriteLine("Sin B");
            }

            //tamaño boton
            sw.WriteLine(this.comboBoxTamañoBoton.SelectedIndex); //linea 22
            //tarea preliminar
            sw.WriteLine(this.cbTareaPreliminar.CheckState); //linea 23
            if (this.cbTareaPreliminar.Checked) //linea 24
                sw.WriteLine(this.tiempoTareaInicial.Ticks);
            else
                sw.WriteLine("valor nulo");

            //datos de la grafica
            sw.WriteLine(this.textBoxNumFila.Text); //linea 25
            sw.WriteLine(this.textBoxNumColumnas.Text); //linea 26
            // -- valores del array de tiempos y del array de los tiempos de latencia
            for (int dato = 0; dato < this.guardarTiemposGrafica.Length; dato++)
            {
                sw.Write(this.guardarTiemposGrafica[dato].Ticks);
                sw.Write("\t");
                if (dato < this.guardarTiemposEmparejamiento.Length)
                    sw.WriteLine(this.guardarTiemposEmparejamiento[dato].Ticks);
                else
                    sw.WriteLine(0);
            }
            //    foreach (DateTime dt in this.guardarTiemposGrafica)
            //        sw.WriteLine(dt.Ticks);  //linea 27 hasta dt.longitud

            /*
             * Guardamos tb los datos de la música o metronomo
             */
            sw.WriteLine(rbSinSonido.Checked);
            sw.WriteLine(rbMetronomo.Checked);
            if (rbMetronomo.Checked)
                sw.WriteLine(tbMetronomo.Text);
            else
                sw.WriteLine("Valor Nulo.");

            sw.WriteLine(rbMusica.Checked);
            if (rbMusica.Checked)
            {
                sw.WriteLine(this.listaMusica.Items.Count);
                foreach (string ss in listaMusica.Items)
                    sw.WriteLine(ss);
            }
            sw.WriteLine(this.deslizadorVelocidad.Value);
            if (this.observaciones.Length == 0)
                sw.WriteLine("SinObservaciones");
            else
                sw.WriteLine(this.observaciones);

            sw.WriteLine(this.btControlTachado.Visible);
            //sw.WriteLine(this.btControlTachado.Enabled);
            //if (this.btControlTachado.Visible)
            if (this.btControlTachado.Enabled &&
                 (rejillaTipo != TipoRejilla.IMAGENES
                  && rejillaTipo != TipoRejilla.COLORES
                  && rejillaTipo != TipoRejilla.WINGDINGS)
                )
            {
                sw.WriteLine(this.listaDatosControlTachado.Count);
                foreach (KeyValuePair<String, DatosControlTachado> elemento in this.listaDatosControlTachado)
                {
                    sw.Write(elemento.Key);
                    sw.Write(Convert.ToChar(2));
                    sw.WriteLine(elemento.Value.ToString());
                }
                sw.WriteLine(this.aciertos);
                if (rejillaTipo != TipoRejilla.ABECEDARIO)
                    sw.WriteLine(this.rbDescendente.Checked);
                else
                    sw.WriteLine(this.rbDescAbecedario.Checked);

                sw.WriteLine(this.errores);

                sw.WriteLine(this.ordenPulsacion.Count);
                foreach (string s in this.ordenPulsacion)
                {
                    sw.Write(s);
                    sw.Write(Convert.ToChar(5));
                }
            }
            //guardar todos los tiempos para la exportacion
            //en el fichero ya que al cargarlos los calcula el pintar grafica pero
            //si quiero exportar los datos no los tengo en el fichero
            //sw.WriteLine();
            sw.WriteLine(Convert.ToChar(9));
            sw.Write(this.etTiempoTranscurrido.Text); //tiempo total
            sw.Write(Convert.ToChar(10));
            sw.Write(this.etValorMedia.Text); //la media
            sw.Write(Convert.ToChar(10));
            sw.Write(this.etVarianza.Text); //varianza
            sw.Write(Convert.ToChar(10));
            sw.Write(this.etValorTiempoCorregido.Text);//tiempo corregido
            sw.Write(Convert.ToChar(10));
            sw.Write(this.etValorTiempoCorreccionEfectuada.Text); //Corrección
            sw.Write(Convert.ToChar(10));
            sw.Write(this.etValorTiempoTareaPreliminar.Text); //Tarea Preliminar
            sw.Write(Convert.ToChar(10));
            sw.Write(this.etValorMediaCorreccion.Text); // Media corrección
            sw.Write(Convert.ToChar(10));
            sw.Write(this.etValorVarianzaCorreccion.Text); // varianza corrección
            sw.Write(Convert.ToChar(10));
            sw.Write(this.etValorMaximo.Text); //maximo
            sw.Write(Convert.ToChar(10));
            sw.Write(this.etValorMinimo.Text); //minimo
            sw.Write(Convert.ToChar(10));

            //sw.Write('^');
            sw.Write(Convert.ToChar(6));
            //Cerrar el flujo
            sw.Close();
        }
        /*
         * Métido para inicializar las variables temporales que aparecen en la gráfica
         */
        private void inicializarVariablesTemporalesGrafica()
        {
            this.etTiempoTranscurridoEmparejar.Text = "00:00.000";
            this.etValorMaximoEmparejar.Text = "00:00.000";
            this.etValorMinimoEmparejar.Text = "00:00.000";
            this.etValorMediaEmparejar.Text = "00:00.000";
            this.etVarianzaEmparejar.Text = "00:00.000";

            this.etValorEmparejarCero1.Text = "00:00.000";
            this.etValorEmparejarCero2.Text = "00:00.000";
            this.etValorEmparejarCero3.Text = "00:00.000";
            this.etValorEmparejarCero4.Text = "00:00.000";
            this.etValorEmparejarCero5.Text = "00:00.000";

            this.etTiempoTranscurrido.Text = "00:00.000";
            this.etValorMaximo.Text = "00:00.000";
            this.etValorMinimo.Text = "00:00.000";
            this.etValorMediaCorreccion.Text = "00:00.000";
            this.etValorMedia.Text = "00:00.000";
            this.etVarianza.Text = "00:00.000";
            this.etValorVarianzaCorreccion.Text = "00:00.000";
            this.etValorTiempoCorreccionEfectuada.Text = "00:00.000";
            this.etValorTiempoTareaPreliminar.Text = "00:00.000";
            this.etValorTiempoCorregido.Text = "00:00.000";
        }



        /*
         * Metodo que escoge el fichero de datos que se va a cargar
         * y llama a cargarFichero.
         */
        private void abrirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = this.rutaCarpeta;
            //ofd.Filter = "Ficheros de datos (*.tbl)|*.tbl";
            ofd.Filter = rm.GetString("cad13");
            DialogResult dr = ofd.ShowDialog();
            try
            {
                if (dr == DialogResult.OK)
                {
                    inicializarVariablesTemporalesGrafica();

                    try
                    {
                        cargarFichero(new FileStream(ofd.FileName, FileMode.Open, FileAccess.Read));
                        //this.guardarToolStripMenuItem.Enabled = true;
                        this.fichero = ofd.FileName;


                        //Los datos son cargados desde fichero, luego pongo las variables a fichero.<
                        datosCargadosBD = false;
                        datosWinCargadosBD = false;
                        observacionesBD = false;
                    }
                    catch (FileNotFoundException fne)
                    {
                        //throw new RejillaException("ABRIR FICHERO: No se encuentra el fichero \n");
                        throw new RejillaException(rm.GetString("sop01"));
                    }
                    catch (IOException ie)
                    {
                        //throw new RejillaException("ABRIR FICHERO: Error en la lectura del fichero \n.");
                        throw new RejillaException(rm.GetString("sop02"));
                    }
                    catch (UnauthorizedAccessException uae)
                    {
                        //throw new RejillaException("ABRIR FICHERO: No tiene permisos para abrir este fichero. \n");
                        throw new RejillaException(rm.GetString("sop03"));
                    }
                }
                else
                {
                    if (dr == DialogResult.Cancel)
                    {
                        this.guardarToolStripMenuItem.Enabled = false;
                    }
                }
            }
            catch (RejillaException re)
            {
                //MessageBox.Show(re.Message, "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                MessageBox.Show(re.Message, rm.GetString("war00"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /*
         * Metodo que carga los datos del fichero en un formulario 
         */
        private void cargarFichero(FileStream fs)
        {
            int POS_FILAS = 33 + 1 + 3;
            int POS_COLUMNAS = 34 + 1 + 3;
            int POS_TIPOREJILLA = 8;
            int POS_NOMBRE = 1;
            int POS_APELLIDO = 2;
            int POS_DEPORTE = 3;
            int POS_POSICION = 4;
            int POS_FECHA = 0;

            String todoElContenido;
            String[] contenidoPorEtapas;
            StreamReader sr = new StreamReader(fs);
            int longitudDatosGrafica = 0;

            this.MetiendoDatos_Load(this, EventArgs.Empty);

            this.Cursor = Cursors.WaitCursor;
            todoElContenido = sr.ReadToEnd();
            sr.Close();
            sr.Dispose();

            contenidoPorEtapas = todoElContenido.Split(new char[] { Convert.ToChar(6) }, StringSplitOptions.RemoveEmptyEntries);

            EleccionRegistroFichero erf = new EleccionRegistroFichero();
            string concatenarCadena = "";
            String[] acc;
            int blancos = 1;
            int espBlancos = 1;
            int largo = 25;
            char[] separChar = new char[4];
            separChar[0] = Convert.ToChar(7);
            separChar[1] = Convert.ToChar(8);
            separChar[2] = '\n';
            separChar[3] = '\r';
            try
            {
                for (int indiceTodo = 0; indiceTodo < contenidoPorEtapas.Length; indiceTodo++)
                {
                    acc = contenidoPorEtapas[indiceTodo].Split(separChar /*new char[] { 'ç', '#', '\n', '\r' }*/, StringSplitOptions.RemoveEmptyEntries);
                    if (acc[25].Length == 1)
                        blancos = 8;
                    else
                        blancos = 7;

                    switch (acc[5].Length)
                    {
                        case 6:
                            espBlancos = largo - 5;
                            break;
                        case 7:
                            espBlancos = largo - 8;
                            break;
                        case 8:
                            espBlancos = largo - 8;
                            break;
                        case 9:
                            espBlancos = largo - 10;
                            break;
                        case 10:
                            espBlancos = largo - 10;
                            break;
                        default:
                            espBlancos = 14;
                            break;
                    }

                    //concatenarCadena =  acc[0] + acc[22].PadLeft(blancos,' ') + "X" + acc[23].PadRight(8,' ') + this.returnTipoRejilla(acc[5]).ToString().PadRight(espBlancos,' ')  + "  " +(acc[1] + " " + acc[2]) ;
                    //concatenarCadena = acc[0] + acc[25].PadLeft(blancos, ' ') + "X" + acc[26].PadRight(8, ' ') + this.returnTipoRejilla(acc[8]).ToString().PadRight(espBlancos, ' ') + "  " + (acc[1] + " " + acc[2] + " -> "  + acc[3] + " -> " + acc[4]);
                    concatenarCadena = acc[POS_FECHA] + acc[POS_FILAS].PadLeft(blancos, ' ') + "X" + acc[POS_COLUMNAS].PadRight(8, ' ') + this.returnTipoRejilla(acc[POS_TIPOREJILLA]).ToString().PadRight(espBlancos, ' ') + "  " + (acc[POS_NOMBRE] + " " + acc[POS_APELLIDO] + " -> " + acc[POS_DEPORTE] + " -> " + acc[POS_POSICION]);
                    erf.lbRegistros.Items.Add(concatenarCadena);
                }
                //erf.etRegistrosTamañoLista.Text = "Cantidad de Registros en el Fichero " + erf.lbRegistros.Items.Count;
                erf.etRegistrosTamañoLista.Text = rm.GetString("txt05") + erf.lbRegistros.Items.Count;
                /*
                 * Me quedo con la cadena que necesite del listbox que haya elegio el usuario.
                 */
                String cadenaElegida = "";
                String[] procesarCadena;
                DialogResult dr = erf.ShowDialog();
                this.Cursor = Cursors.Default;
                if (dr == DialogResult.OK)
                {
                    this.winamp.Stop();
                    this.reproductorWindowsMedia.Ctlcontrols.stop();

                    cadenaElegida = contenidoPorEtapas[erf.lbRegistros.SelectedIndex];
                    erf.Dispose();
                    int LineaFicheroTocaLeer = 0;
                    procesarCadena = cadenaElegida.Split(separChar/*new char[] { 'ç', '#', '\n', '\r' }*/, StringSplitOptions.RemoveEmptyEntries);

                    //pasamos los datos al formulario
                    this.tbFecha.Text = procesarCadena[LineaFicheroTocaLeer];//linea 0
                    LineaFicheroTocaLeer++;
                    this.tbNombre.Text = procesarCadena[LineaFicheroTocaLeer]; //linea 1
                    LineaFicheroTocaLeer++;
                    this.tbApellidos.Text = procesarCadena[LineaFicheroTocaLeer]; //linea 2
                    LineaFicheroTocaLeer++;
                    this.tbDeporte.Text = procesarCadena[LineaFicheroTocaLeer];//linea 3
                    LineaFicheroTocaLeer++;
                    this.tbPosicion.Text = procesarCadena[LineaFicheroTocaLeer];//linea 4
                    LineaFicheroTocaLeer++;
                    //lo nuevo
                    this.tbEdad.Text = procesarCadena[LineaFicheroTocaLeer]; //linea 5
                    LineaFicheroTocaLeer++;
                    //this.rbHombre.Checked = Convert.ToBoolean(procesarCadena[LineaFicheroTocaLeer]);// linea 6
                    this.sexo = devolverGeneroFichero(procesarCadena[LineaFicheroTocaLeer]);
                    LineaFicheroTocaLeer++;
                    marcarCampoSexo();
                    this.tbPais.Text = procesarCadena[LineaFicheroTocaLeer];//linea 7
                    LineaFicheroTocaLeer++;
                    //fin de lo nuevo

                    this.rejillaTipo = returnTipoRejilla(procesarCadena[LineaFicheroTocaLeer]);//linea 8
                                                                                               //                    this.cargarTipoRejilla(this.rejillaTipo);
                    LineaFicheroTocaLeer++;

                    this.later = devolverLateralidadFichero(procesarCadena[LineaFicheroTocaLeer]);
                    LineaFicheroTocaLeer++;
                    marcarCampoLAT();

                    //Aqui esta los aciertos, errores y numeroCAsillas
                    this.aciertos = Convert.ToInt32(procesarCadena[LineaFicheroTocaLeer]);
                    LineaFicheroTocaLeer++;
                    this.errores = Convert.ToInt32(procesarCadena[LineaFicheroTocaLeer]);
                    LineaFicheroTocaLeer++;
                    this.cantidadBotones = Convert.ToInt32(procesarCadena[LineaFicheroTocaLeer]);
                    LineaFicheroTocaLeer++;

                    //aqui lo de emparejar
                    //estado, tiempo, ubicacion boton, secuencia
                    this.cbEmparejar.Checked = Convert.ToBoolean(procesarCadena[LineaFicheroTocaLeer]);
                    LineaFicheroTocaLeer++;
                    this.tiempoEmparejar = Convert.ToInt32(procesarCadena[LineaFicheroTocaLeer]);
                    LineaFicheroTocaLeer++;
                    this.posicion = devolverPosicionBotonEmparejar(procesarCadena[LineaFicheroTocaLeer]);
                    LineaFicheroTocaLeer++;
                    this.secuencia = devolverSecuenciaEmparejar(procesarCadena[LineaFicheroTocaLeer]);
                    LineaFicheroTocaLeer++;


                    //la casilla colores de fondo
                    this.cbColoresFondo.Checked = Convert.ToBoolean(procesarCadena[LineaFicheroTocaLeer]);
                    LineaFicheroTocaLeer++;

                    this.textBoxValorIncial.Text = procesarCadena[LineaFicheroTocaLeer];
                    LineaFicheroTocaLeer++;
                    this.tbAbecedario.Text = procesarCadena[LineaFicheroTocaLeer];
                    LineaFicheroTocaLeer++;
                    rellenarOpcionesAbecedario(Convert.ToInt32(procesarCadena[LineaFicheroTocaLeer]));
                    LineaFicheroTocaLeer++;



                    this.checkBoxAleatorio.Checked = devolverCheked(procesarCadena[LineaFicheroTocaLeer]);//linea 9
                    LineaFicheroTocaLeer++;
                    if (this.checkBoxAleatorio.Checked)
                        this.textBoxTiempoSegundosAleatorio.Text = procesarCadena[LineaFicheroTocaLeer];//linea 10
                    else
                        this.textBoxTiempoSegundosAleatorio.Text = "";
                    LineaFicheroTocaLeer++;

                    this.cbTiempoLimite.Checked = devolverCheked(procesarCadena[LineaFicheroTocaLeer]); //linea 11
                    LineaFicheroTocaLeer++;
                    if (this.cbTiempoLimite.Checked)
                        this.tbTiempoLimite.Text = procesarCadena[LineaFicheroTocaLeer]; //linea 12
                    else
                        this.tbTiempoLimite.Text = "";


                    LineaFicheroTocaLeer++;
                    //Distraccion linea, tipo, grosor y color
                    this.rbHorizontal.Checked = System.Convert.ToBoolean(procesarCadena[LineaFicheroTocaLeer]);//linea 13
                    LineaFicheroTocaLeer++;
                    this.rbVertical.Checked = System.Convert.ToBoolean(procesarCadena[LineaFicheroTocaLeer]);//linea 14
                    LineaFicheroTocaLeer++;
                    this.rbAleatorio.Checked = System.Convert.ToBoolean(procesarCadena[LineaFicheroTocaLeer]);//linea 15
                    LineaFicheroTocaLeer++;
                    this.rbSinlinea.Checked = System.Convert.ToBoolean(procesarCadena[LineaFicheroTocaLeer]);//linea 16
                    LineaFicheroTocaLeer++;

                    if (!this.rbSinlinea.Checked)
                    {
                        this.cBGrosor.SelectedItem = procesarCadena[LineaFicheroTocaLeer];//linea 17
                        LineaFicheroTocaLeer++;
                        int alpha, rojo, verde, azul;
                        alpha = Convert.ToInt32(procesarCadena[LineaFicheroTocaLeer]); //linea 18
                        LineaFicheroTocaLeer++;
                        rojo = Convert.ToInt32(procesarCadena[LineaFicheroTocaLeer]);  //linea 19
                        LineaFicheroTocaLeer++;
                        verde = Convert.ToInt32(procesarCadena[LineaFicheroTocaLeer]); //linea 20
                        LineaFicheroTocaLeer++;
                        azul = Convert.ToInt32(procesarCadena[LineaFicheroTocaLeer]);  //linea 21
                        LineaFicheroTocaLeer++;
                        this.pBColor.BackColor = Color.FromArgb(alpha, rojo, verde, azul);

                    }
                    else
                    {
                        //sino hay linea saltar las 5 del grosor, color.
                        LineaFicheroTocaLeer += 5;
                    }
                    tipoDeLinea(); //para activar la distraccion linea.
                    //Tamaño del boton.
                    this.comboBoxTamañoBoton.SelectedIndex = System.Convert.ToInt32(procesarCadena[LineaFicheroTocaLeer]);//linea 22
                    LineaFicheroTocaLeer++;
                    /*
                     * CArgar la tarea prelminar si la hay
                     */
                    this.cbTareaPreliminar.Checked = devolverCheked(procesarCadena[LineaFicheroTocaLeer]); //linea 23
                    LineaFicheroTocaLeer++;
                    if (this.cbTareaPreliminar.Checked)
                    {
                        this.tiempoTareaInicial = new TimeSpan(System.Convert.ToInt64(procesarCadena[LineaFicheroTocaLeer]));//linea 24
                        this.etValorTiempoTareaPreliminar.Text = FormatTimeSpan(this.tiempoTareaInicial);
                    }
                    LineaFicheroTocaLeer++;
                    this.textBoxNumFila.Text = procesarCadena[LineaFicheroTocaLeer];
                    LineaFicheroTocaLeer++;
                    this.textBoxNumColumnas.Text = procesarCadena[LineaFicheroTocaLeer];
                    LineaFicheroTocaLeer++;

                    long ticks = 0;

                    this.guardarTiemposGrafica = new DateTime[1 + System.Convert.ToInt32(this.textBoxNumFila.Text) * System.Convert.ToInt32(textBoxNumColumnas.Text)];
                    longitudDatosGrafica = LineaFicheroTocaLeer + System.Convert.ToInt32(this.textBoxNumFila.Text) * System.Convert.ToInt32(textBoxNumColumnas.Text);

                    this.guardarTiemposEmparejamiento = new TimeSpan[this.guardarTiemposGrafica.Length];

                    String[] s;
                    for (int i = LineaFicheroTocaLeer; i <= longitudDatosGrafica; i++)
                    {
                        s = procesarCadena[i].Split(new char[] { '\t' });
                        ticks = Convert.ToInt64(s[0]);
                        //ticks = System.Convert.ToInt64(procesarCadena[i]);
                        if (ticks != 0)
                            this.guardarTiemposGrafica[i - LineaFicheroTocaLeer] = new DateTime(ticks);
                        else
                            this.guardarTiemposGrafica[i - LineaFicheroTocaLeer] = new DateTime();

                        ticks = Convert.ToInt64(s[1]);
                        if (ticks != 0)
                            this.guardarTiemposEmparejamiento[i - LineaFicheroTocaLeer] = new TimeSpan(ticks);
                        else
                            this.guardarTiemposEmparejamiento[i - LineaFicheroTocaLeer] = new TimeSpan();
                    }

                    LineaFicheroTocaLeer = longitudDatosGrafica;

                    /*
                     * Procesar la música o el metronomo lo que haya
                     */
                    LineaFicheroTocaLeer++;
                    //this.rbSinSonido.Checked = System.Convert.ToBoolean(procesarCadena[LineaFicheroTocaLeer + 1]);//lineaFicheroTocaLeer+1
                    this.rbSinSonido.Checked = System.Convert.ToBoolean(procesarCadena[LineaFicheroTocaLeer]);
                    LineaFicheroTocaLeer++;
                    //this.rbMetronomo.Checked = System.Convert.ToBoolean(procesarCadena[LineaFicheroTocaLeer + 2]);
                    this.rbMetronomo.Checked = System.Convert.ToBoolean(procesarCadena[LineaFicheroTocaLeer]);//lineaFicheroTocaLeer+2
                    LineaFicheroTocaLeer++;

                    if (this.rbMetronomo.Checked)
                        this.tbMetronomo.Text = procesarCadena[LineaFicheroTocaLeer]; //lineaFicheroTocaLeer+3
                    else
                        this.tbMetronomo.Text = "";

                    LineaFicheroTocaLeer++;
                    this.rbMusica.Checked = System.Convert.ToBoolean(procesarCadena[LineaFicheroTocaLeer]);//lineaFicheroTocaLeer+4

                    //LineaFicheroTocaLeer += 5;
                    LineaFicheroTocaLeer++;
                    this.listaMostrar.Items.Clear();
                    this.listaMusica.Items.Clear();
                    if (this.rbMusica.Checked)
                    {
                        int cantidadCanciones = Convert.ToInt32(procesarCadena[LineaFicheroTocaLeer]);//lineaFicheroTocaLeer+5
                        LineaFicheroTocaLeer++;
                        String cadenaActual = ""; ;
                        for (int canciones = LineaFicheroTocaLeer; canciones < cantidadCanciones + LineaFicheroTocaLeer; canciones++)
                        {
                            cadenaActual = procesarCadena[canciones];
                            this.listaMusica.Items.Add(cadenaActual);
                            //this.listaMostrar.Items.Add(cadenaActual.Substring(cadenaActual.LastIndexOf('\\') + 1, (cadenaActual.LastIndexOf('.') - cadenaActual.LastIndexOf('\\') - 1)));
                            this.listaMostrar.Items.Add(cadenaActual.Substring(cadenaActual.LastIndexOf(Path.DirectorySeparatorChar) + 1, (cadenaActual.LastIndexOf('.') - cadenaActual.LastIndexOf(Path.DirectorySeparatorChar) - 1)));
                        }
                        LineaFicheroTocaLeer += cantidadCanciones;
                    }
                    else
                    {
                        this.listaMusica.Items.Clear();
                        this.listaMostrar.Items.Clear();
                    }
                    this.deslizadorVelocidad.Value = Convert.ToInt32(procesarCadena[LineaFicheroTocaLeer]);
                    LineaFicheroTocaLeer++;
                    this.observaciones = procesarCadena[LineaFicheroTocaLeer];//lineaFicheroTocaLeer+1
                    LineaFicheroTocaLeer++;
                    this.btControlTachado.Enabled = Convert.ToBoolean(procesarCadena[LineaFicheroTocaLeer]);//lineaFicheroTocaLeer+2;

                    if (this.btControlTachado.Enabled)
                    {
                        LineaFicheroTocaLeer++;
                        this.btControlTachado.Visible = true;
                        //int tamañoLista = Convert.ToInt32(procesarCadena[LineaFicheroTocaLeer + 3]);
                        int tamañoLista = Convert.ToInt32(procesarCadena[LineaFicheroTocaLeer]);
                        //LineaFicheroTocaLeer += 4;
                        LineaFicheroTocaLeer++;
                        string[] trocearLineas;
                        char[] aChar = new char[3];
                        aChar[0] = Convert.ToChar(2);
                        aChar[1] = Convert.ToChar(3);
                        aChar[2] = Convert.ToChar(4);
                        this.listaDatosControlTachado = new SortedList<string, DatosControlTachado>();
                        for (int i = 0; i < tamañoLista; i++)
                        {
                            //EL VALOR 0 es para ORDEN...X
                            trocearLineas = procesarCadena[LineaFicheroTocaLeer + i].Split(aChar);
                            DatosControlTachado dc = new DatosControlTachado(
                                trocearLineas[1],
                                Convert.ToInt32(trocearLineas[2]),
                                Convert.ToInt32(trocearLineas[3]),
                                new DateTime(Convert.ToInt64(trocearLineas[4])));
                            dc.pulsadoBoton(Convert.ToBoolean(trocearLineas[5]));

                            this.listaDatosControlTachado.Add(trocearLineas[0], dc);
                        }
                        LineaFicheroTocaLeer += tamañoLista;


                        this.aciertos = Convert.ToInt32(procesarCadena[LineaFicheroTocaLeer]);
                        LineaFicheroTocaLeer++;
                        if (this.rejillaTipo != TipoRejilla.ABECEDARIO)
                            this.rbDescendente.Checked = Convert.ToBoolean(procesarCadena[LineaFicheroTocaLeer]);//lineaficheroTocaLeer+1
                        else
                            this.rbDescAbecedario.Checked = Convert.ToBoolean(procesarCadena[LineaFicheroTocaLeer]);


                        if (this.rbDescendente.Checked)
                            this.verBotonRbNombre = this.rbDescendente.Name;
                        else
                            this.verBotonRbNombre = this.rbAscendente.Name;

                        this.rbAscendente.Checked = !this.rbDescendente.Checked;

                        LineaFicheroTocaLeer++;
                        this.errores = Convert.ToInt32(procesarCadena[LineaFicheroTocaLeer]);//lineaFicheroTocaLeer+2;
                        this.ordenPulsacion = new List<string>();
                        LineaFicheroTocaLeer++;
                        int longOrden = Convert.ToInt32(procesarCadena[LineaFicheroTocaLeer]);//lineaFicheroTocaLeer+3
                        char[] oChar = new char[1];
                        oChar[0] = Convert.ToChar(5);
                        LineaFicheroTocaLeer++; //mas 4 de la linea de fichero
                        string[] sOrden = procesarCadena[LineaFicheroTocaLeer/*LineaFicheroTocaLeer + 4*/].Split(oChar, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string st in sOrden)
                            this.ordenPulsacion.Add(st);
                    }
                    this.guardarToolStripMenuItem.Enabled = true;

                    /*
                     * Dibujamos la grafica al final por si salta alguna excepción
                     * antes para que no se dibuje.
                     */
                    this.cargarTipoRejilla(this.rejillaTipo);
                    this.lienzoinicial = true;
                    dibujarGrafica(true, this.guardarTiemposGrafica, this.rbGrafica.Checked, this.guardarTiemposEmparejamiento);
                }
                else if (dr == DialogResult.Cancel)
                {
                    this.guardarToolStripMenuItem.Enabled = false;
                    erf.Close();
                }
            }
            catch (IndexOutOfRangeException io)
            {
                //throw new RejillaException("No se ha seleccionado ningún registro para cargar.");
                throw new RejillaException(rm.GetString("error23"));
            }
            catch (FormatException fe)
            {
                this.tbFecha.Text = DateTime.Now.ToString();
                this.tbNombre.Text = "";
                this.tbApellidos.Text = "";
                this.rejillaTipo = TipoRejilla.NUMERICA;
                this.rbNumerica.Checked = true;
                this.checkBoxAleatorio.Checked = false;
                this.cbTiempoLimite.Checked = false;
                this.rbSinlinea.Checked = true;
                this.textBoxTiempoSegundosAleatorio.Text = "";
                this.tbTiempoLimite.Text = "";
                this.pBColor.BackColor = Color.White;
                this.comboBoxTamañoBoton.SelectedIndex = 0;
                this.cbTareaPreliminar.Checked = false;
                this.textBoxNumFila.Text = "10";
                this.textBoxNumColumnas.Text = "10";
                this.rbSinSonido.Checked = true;
                this.listaMusica.Items.Clear();
                this.listaMostrar.Items.Clear();
                this.tbMetronomo.Text = "";
                this.btControlTachado.Enabled = false;

                //throw new RejillaException("Fichero dañado..........");
                throw new RejillaException(rm.GetString("error24"));
            }
            catch (RejillaException re)
            {
                //MessageBox.Show(re.Message, "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                MessageBox.Show(re.Message, rm.GetString("war00"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void marcarCampoLAT()
        {
            if (this.later == Lateralidad.IZQUIERDA)
                this.rbIzq.Checked = true;
            if (this.later == Lateralidad.DERECHA)
                this.rbDer.Checked = true;
        }

        private void marcarCampoSexo()
        {
            if (this.sexo == Genero.MASCULINO)
                this.rbHombre.Checked = true;
            if (this.sexo == Genero.FEMENINO)
                this.rbMujer.Checked = true;
        }

        private bool devolverCheked(String s)
        {
            bool devolver = false;
            if (s.Equals("Unchecked"))
                devolver = false;
            else if (s.Equals("Checked"))
                devolver = true;
            else
                //throw new RejillaException("Error CHECKED: formato erroneo o fichero dañado. \n");
                //Cambiado a fichero dañado.
                throw new RejillaException(rm.GetString("error25"));
            return devolver;
        }

        private Genero devolverGeneroFichero(string s)
        {
            Genero g;
            switch (s)
            {
                case "MASCULINO":
                    g = Genero.MASCULINO;
                    break;
                case "FEMENINO":
                    g = Genero.FEMENINO;
                    break;
                default:
                    throw new RejillaException(rm.GetString("error24"));//"FORMATO FICHERO DAÑADO");
            }
            return g;
        }

        private Lateralidad devolverLateralidadFichero(string s)
        {
            Lateralidad l;
            switch (s)
            {
                case "IZQUIERDA":
                    l = Lateralidad.IZQUIERDA;
                    break;
                case "DERECHA":
                    l = Lateralidad.DERECHA;
                    break;
                default:
                    throw new RejillaException(rm.GetString("error24"));//"FICHERO DAÑADO");
            }
            return l;
        }

        private TipoRejilla returnTipoRejilla(String s)
        {
            TipoRejilla tr;
            switch (s)
            {
                case "NUMERICA":
                    tr = TipoRejilla.NUMERICA;
                    break;
                case "LETRAS":
                    tr = TipoRejilla.LETRAS;
                    break;
                case "COLORES":
                    tr = TipoRejilla.COLORES;
                    break;
                case "ABECEDARIO":
                    tr = TipoRejilla.ABECEDARIO;
                    break;
                case "WINGDINGS":
                    tr = TipoRejilla.WINGDINGS;
                    break;
                case "IMAGENES":
                    tr = TipoRejilla.IMAGENES;
                    break;
                default: //throw new RejillaException("Error en el contenido del fichero, el tipoREjilla del fichero no existe o está mal escrito. ");
                    //throw new RejillaException("Error TIPOREJILLA: formato del fichero erroneo o fichero dañado. ");
                    throw new RejillaException(rm.GetString("error26"));
            }
            return tr;
        }

        private PosicionBotonEmparejar devolverPosicionBotonEmparejar(String s)
        {
            PosicionBotonEmparejar p;
            s = s.ToUpper();
            switch (s)
            {
                case "IZQUIERDA_CENTRO":
                    p = PosicionBotonEmparejar.IZQUIERDA_CENTRO;
                    break;
                case "IZQUIERDA_SUPERIOR":
                    p = PosicionBotonEmparejar.IZQUIERDA_SUPERIOR;
                    break;
                case "IZQUIERDA_INFERIOR":
                    p = PosicionBotonEmparejar.IZQUIERDA_INFERIOR;
                    break;
                case "DERECHA_CENTRO":
                    p = PosicionBotonEmparejar.DERECHA_CENTRO;
                    break;
                case "DERECHA_INFERIOR":
                    p = PosicionBotonEmparejar.DERECHA_INFERIOR;
                    break;
                case "DERECHA_SUPERIOR":
                    p = PosicionBotonEmparejar.DERECHA_SUPERIOR;
                    break;
                case "ALEATORIA":
                    p = PosicionBotonEmparejar.ALEATORIA;
                    break;
                default: //throw new RejillaException("Error en el contenido del fichero, el tipoREjilla del fichero no existe o está mal escrito. ");
                    //throw new RejillaException("Error TIPOREJILLA: formato del fichero erroneo o fichero dañado. ");
                    throw new RejillaException(rm.GetString("error24"));
            }
            return p;

        }

        private EmparejarSecuencia devolverSecuenciaEmparejar(String s)
        {
            EmparejarSecuencia es;
            s = s.ToUpper();
            switch (s)
            {
                case "SECUENCIAL":
                    es = EmparejarSecuencia.SECUENCIAL;
                    break;
                case "ALEATORIA":
                    es = EmparejarSecuencia.ALEATORIA;
                    break;
                case "INVERSA":
                    es = EmparejarSecuencia.INVERSA;
                    break;
                default: //throw new RejillaException("Error en el contenido del fichero, el tipoREjilla del fichero no existe o está mal escrito. ");
                    //throw new RejillaException("Error TIPOREJILLA: formato del fichero erroneo o fichero dañado. ");
                    throw new RejillaException(rm.GetString("error24"));
            }
            return es;
        }

        /*
         * ***************************************************************
         * BARRA DE MENU. IMPRIMIR.
         * ***************************************************************
         */

        /*
         * Metodo que me imprime un informe del formulario principal.
         * Elige una fuente para la impresión y luego llama a Print que es el que se
         * encarga ya de imprimir todo.
         * Como solo queremos el informe ponemos informeMasGrafica = false;
         */
        private void imprimirInformeToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (diagImprimir.ShowDialog() == DialogResult.OK)
            {
                this.informeMasGrafica = false;
                fuenteParaImprimir = this.Font;
                FontDialog fd = new FontDialog();
                if (fd.ShowDialog() == DialogResult.OK)
                {
                    this.fuenteParaImprimir = fd.Font;
                }
                this.totalLineasImpresas = 0;
                this.informe = true;
                this.printDocument.DefaultPageSettings.Landscape = false;
                this.printDocument.Print();
            }
        }

        /*
         * Imprimimos el informe mas la grafica al final, se imprime con la hoja en horizontal
         * porque la grafica no cabe en una hoja vertical.
         * Se elige la fuente con la que se va a imprimir el informe.
         */
        private void imprimirInformeMasGraficaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            capturarGrafica();
            if (diagImprimir.ShowDialog() == DialogResult.OK)
            {
                this.informeMasGrafica = true;
                fuenteParaImprimir = this.Font;
                FontDialog fd = new FontDialog();
                if (fd.ShowDialog() == DialogResult.OK)
                {
                    this.fuenteParaImprimir = fd.Font;
                }
                this.totalLineasImpresas = 0;
                this.informe = true;
                //PARA PONER LA PAGINA EN HORIZONTAL
                this.printDocument.DefaultPageSettings.Landscape = true;
                this.printDocument.Print();
                this.noGrafica = false;
            }
        }

        /*
         * Imprime el formulario tal y como esta en pantalla en una hoja apaisada.
         */
        private void imprimirPantallaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.CenterToScreen();
            this.archivoToolStripMenuItem.HideDropDown();

            if (this.archivoToolStripMenuItem.Visible == false)
            {
                CapturarPantalla();
                if (diagImprimir.ShowDialog() == DialogResult.OK)
                {
                    this.pantalla = true;
                    this.printDocument.DefaultPageSettings.Landscape = true;
                    this.printDocument.Print();
                }
            }
            else
            {
                /*
                 * Usamos un temporizador para que me capture bien la pantalla y no me salga
                 * el menu archivo abierto, que me salga cerrado.
                 */
                this.esperaImprimir.Enabled = true;
            }
        }

        private void esperaImprimir_Tick(object sender, EventArgs e)
        {
            this.esperaImprimir.Enabled = false;
            CapturarPantalla();
            if (diagImprimir.ShowDialog() == DialogResult.OK)
            {
                this.pantalla = true;
                this.printDocument.DefaultPageSettings.Landscape = true;
                this.printDocument.Print();
            }
        }

        private void CapturarPantalla()
        {
            Graphics g = this.CreateGraphics();
            Size s = this.Size;
            imagen = new Bitmap(s.Width, s.Height, g);
            Graphics g2 = Graphics.FromImage(imagen);
            g2.CopyFromScreen(this.Location.X, this.Location.Y, 0, 0, s);
        }

        private void centrarFormulario_Tick(object sender, EventArgs e)
        {
            this.centrarFormulario.Enabled = false;
            capturarGraficaCentrada();
        }

        private void capturarGraficaCentrada()
        {
            Graphics g = this.CreateGraphics();
            Size s = this.Size;
            Size tamañoGrafica = this.gbGrafica.Size;
            imagenGrafica = new Bitmap(tamañoGrafica.Width - 2, tamañoGrafica.Height);
            Graphics grafica = Graphics.FromImage(imagenGrafica);
            int desde, hasta;
            desde = this.Bounds.Location.X + 3;
            hasta = 2 + this.gbGrafica.Location.Y + this.Location.Y + this.etTiempoCorreccionEfectuada.Height * 2;
            grafica.CopyFromScreen(desde, hasta, 0, 0, tamañoGrafica);
        }

        private void capturarGrafica()
        {
            this.CenterToScreen();
            this.centrarFormulario.Enabled = true;
        }

        /*
         * Método que imprime por la impresora.
         */
        private void printDocument_Page(object sender, PrintPageEventArgs e)
        {
            if (this.pantalla)
            {
                e.Graphics.DrawImage(imagen, 75, 50);
                pantalla = false;
            }
            else if (informe)
            {
                float pos_Y = 0;
                float lineasPorPaginas;
                float margenSup = e.MarginBounds.Top;
                float margenIzq = e.MarginBounds.Left;
                Font fuente = this.fuenteParaImprimir;

                float altofuente = fuente.GetHeight(e.Graphics);
                lineasPorPaginas = e.MarginBounds.Height / altofuente;

                //Contador de las lineas impresas por página
                int lineasImpresasPorPaginas = 0;

                string cadenaTexto = generarCadenaInforme();
                informe = false;
                cadenaTroceada = cadenaTexto.Split(new char[] { '\t' });
                while (this.totalLineasImpresas < cadenaTroceada.Length && lineasImpresasPorPaginas < lineasPorPaginas)
                {
                    pos_Y = margenSup + (lineasImpresasPorPaginas * altofuente);
                    e.Graphics.DrawString(cadenaTroceada[totalLineasImpresas], fuente, Brushes.Black, margenIzq, pos_Y, new StringFormat());
                    lineasImpresasPorPaginas += 1;
                    totalLineasImpresas += 1;
                }
                if (totalLineasImpresas < cadenaTroceada.Length)
                {
                    e.HasMorePages = true;
                    informe = true;
                }

                if (this.informeMasGrafica)
                {
                    if (noGrafica && this.informeMasGrafica)
                    {
                        //grafica en una nueva página
                        pos_Y = margenSup;
                        e.Graphics.DrawImage(this.imagenGrafica, margenIzq, pos_Y + 25);
                    }
                    else if (totalLineasImpresas < cadenaTroceada.Length)
                    {
                        e.HasMorePages = true;
                        informe = true;
                    }
                    else if (this.imagenGrafica != null && !e.HasMorePages && (pos_Y + this.imagenGrafica.Height > e.MarginBounds.Height))
                    {
                        e.HasMorePages = true;
                        noGrafica = true;//si la grafica no cabe en  esta pagina la imprimo en la siguiente
                        informe = true;
                    }
                    else
                    {
                        //gráfica en la misma página
                        e.HasMorePages = false;
                        if (this.imagenGrafica != null)
                            e.Graphics.DrawImage(this.imagenGrafica, margenIzq, pos_Y + 25);
                    }
                }
            }
        }

        private string informe_sexo()
        {
            string cadena = null;
            if (this.rbHombre.Checked)
                cadena = rm.GetString("inf41");
            if (this.rbMujer.Checked)
                cadena = rm.GetString("inf42");
            return cadena;
        }

        /*
         * Método que genera una cadena a partir del formulario inicial,
         * separando los datos por tabuladores y luego lo que se hace es
         * imprimir esa cadena en el informe de impresión.
         */

        private string generarCadenaInforme()
        {
            //String cadena = "Informe generado: " + DateTime.Now.ToString() + '\t';
            String cadena = rm.GetString("inf01") + DateTime.Now.ToString() + '\t';
            //cadena += "Fecha Realización Ejercicio:  " + this.tbFecha.Text + '\t';
            cadena += rm.GetString("inf02") + this.tbFecha.Text + '\t';
            //cadena += "Nombre: " + this.tbNombre.Text + "  ";
            cadena += rm.GetString("inf24") + this.tbNombre.Text + "  ";
            cadena += this.tbApellidos.Text + '\t';
            //cadena += "Deporte: " + this.tbDeporte.Text + '\t';
            cadena += rm.GetString("inf25") + this.tbDeporte.Text + '\t';
            //cadena += "Posición: " + this.tbPosicion.Text + '\t';
            cadena += rm.GetString("inf26") + this.tbPosicion.Text + '\t';
            // genero, edad,  pais y lateralidad.
            cadena += rm.GetString("lista33") + ":   " + this.later.ToString() + '\t'; ;
            cadena += rm.GetString("inf38") + this.tbEdad.Text + '\t';
            cadena += rm.GetString("inf39") + informe_sexo() + '\t';
            cadena += rm.GetString("inf40") + this.tbPais.Text + '\t';
            //aciertos, errores, cantidad botones, y porcentajes
            cadena += rm.GetString("lista17") + ":   " + this.aciertos + '\t';
            cadena += rm.GetString("lista18") + ":   " + this.errores + '\t';
            cadena += rm.GetString("lista41") + ":   " + this.cantidadBotones + '\t';

            float[] aux = obtenerPorcentaje(this.aciertos, this.errores, this.cantidadBotones);

            cadena += rm.GetString("lista34") + ":   " + aux[3] + "%" + '\t';
            cadena += rm.GetString("lista35") + ":   " + aux[4] + "%" + '\t';
            cadena += rm.GetString("lista36") + ":   " + aux[5] + "%" + '\t';
            cadena += rm.GetString("lista37") + ":   " + aux[6] + "%" + '\t';
            cadena += rm.GetString("lista38") + ":   " + aux[7] + "%" + '\t';
            cadena += rm.GetString("lista39") + ":   " + aux[8] + "%" + '\t';

            //cadena += "Tamaño de la rejilla: " + this.textBoxNumFila.Text + " x " + this.textBoxNumColumnas.Text + '\t';
            cadena += rm.GetString("inf03") + this.textBoxNumFila.Text + " x " + this.textBoxNumColumnas.Text + '\t';
            //cadena += "Tipo de Rejilla: " + this.rejillaTipo.ToString() + '\t';
            cadena += rm.GetString("inf04") + this.rejillaTipo.ToString() + '\t';
            //cadena += "Tiempo Aleatorio en segundos de intercambio de los botones: " + this.textBoxTiempoSegundosAleatorio.Text.ToString() + "\t";
            cadena += rm.GetString("inf05") + this.textBoxTiempoSegundosAleatorio.Text.ToString() + "\t";
            //cadena += "Tiempo Límite en segundos para finalizar la rejilla: " + this.tbTiempoLimite.Text.ToString() + "\t";
            cadena += rm.GetString("inf06") + this.tbTiempoLimite.Text.ToString() + "\t";
            //cadena += "Tamaño: Botón Tipo " + this.comboBoxTamañoBoton.SelectedItem + '\t';
            cadena += rm.GetString("inf07") + this.comboBoxTamañoBoton.SelectedItem + '\t';
            //cadena += "Distracciones: " + '\t' + "      - ";
            cadena += rm.GetString("inf08") + '\t' + "      - ";
            if (rbSinlinea.Checked)
            {
                //cadena += "Sin Línea.  " + '\t';
                cadena += rm.GetString("inf09") + '\t';
            }
            else if (rbHorizontal.Checked)
            {
                //cadena += "Línea Horizontal." + '\t';
                cadena += rm.GetString("inf10") + '\t';
            }
            else if (rbVertical.Checked)
            {
                //cadena += "Línea Vertical." + '\t';
                cadena += rm.GetString("inf11") + '\t';
            }
            else if (rbAleatorio.Checked)
            {
                //cadena += "Línea Aleatoria." + '\t';
                cadena += rm.GetString("inf12") + '\t';
            }

            //velocidad de la Línea va de 1 a 250 milisegundos.
            if (!rbSinlinea.Checked)
            {
                //cadena += "           Velocidad desplazamiento: ";
                cadena += rm.GetString("inf13");
                if (this.velocidadLinea <= 83)
                {
                    //cadena += "Rápido.";
                    cadena += rm.GetString("inf14");
                }
                else if (this.velocidadLinea > 83 && this.velocidadLinea <= 167)
                {
                    //cadena += "Normal.";
                    cadena += rm.GetString("inf15");
                }
                else
                {
                    //cadena += "Lento.";
                    cadena += rm.GetString("inf16");
                }
                cadena += '\t';
                cadena += rm.GetString("inf27") + pBColor.BackColor.Name.ToString() + '.';
                //cadena += "           Color: " + pBColor.BackColor.Name.ToString() + '.';
                cadena += '\t';
                cadena += rm.GetString("inf28") + cBGrosor.SelectedItem.ToString() + '.';
                //cadena += "           Grosor: " + cBGrosor.SelectedItem.ToString() + '.';
                cadena += '\t';
            }
            cadena += "      - ";
            if (rbSinSonido.Checked)
            {
                //cadena += "Sin sonido. \t";
                cadena += rm.GetString("inf17");
            }
            else if (rbMetronomo.Checked)
            {
                cadena += rm.GetString("inf29") + '\t';
                //cadena += "Metrónomo. \t";
                //cadena += "           Tiempo en milisegundos: " + tbMetronomo.Text.ToString() + ". \t";
                cadena += rm.GetString("inf18") + tbMetronomo.Text.ToString() + ". \t";
            }
            else if (rbMusica.Checked)
            {
                cadena += rm.GetString("inf30") + '\t';
                //cadena += "Música. \t";
                //cadena += "           Lista de canciones: \t";
                cadena += rm.GetString("inf19") + "\t";

                foreach (string s in listaMostrar.Items)
                    cadena += "          > " + s.ToString() + '\t';
            }
            //cadena += " \t TIEMPOS DE LA REJILLA \t";
            cadena += "\t" + rm.GetString("inf20") + "\t";
            //cadena += "Tiempo Total:   " + this.etTiempoTranscurrido.Text + '\t';
            cadena += rm.GetString("gra01") + ":   " + this.etTiempoTranscurrido.Text + '\t';
            //cadena += "Máximo:   " + this.etValorMaximo.Text + '\t';
            cadena += rm.GetString("inf31") + this.etValorMaximo.Text + '\t';
            //cadena += "Mínimo:   " + this.etValorMinimo.Text + '\t';
            cadena += rm.GetString("inf32") + this.etValorMaximo.Text + '\t';
            //cadena += "Media:   " + this.etValorMedia.Text + '\t';
            cadena += rm.GetString("gra02") + ":   " + this.etValorMedia.Text + '\t';
            //cadena += "Varianza:   " + this.etVarianza.Text + '\t';
            cadena += rm.GetString("gra03") + ":   " + this.etVarianza.Text + '\t';
            //cadena += "Tarea preliminar:   " + this.etValorTiempoTareaPreliminar.Text + '\t';
            cadena += rm.GetString("inf33") + this.etValorTiempoTareaPreliminar.Text + '\t';
            //cadena += "Tiempo Corregido:   " + this.etValorTiempoCorregido.Text + '\t';
            cadena += rm.GetString("gra04") + ":   " + this.etValorTiempoCorregido.Text + '\t';
            //cadena += "Tiempo Corregido:   " + this.etValorTiempoCorregido.Text + '\t';
            cadena += rm.GetString("gra05") + ":   " + this.etValorTiempoCorregido.Text + '\t';
            //cadena += "Media Corrección:   " + this.etValorMediaCorreccion.Text + '\t';
            cadena += rm.GetString("inf34") + this.etValorMediaCorreccion.Text + '\t';
            //cadena += "Varianza Corrección:   " + this.etValorVarianzaCorreccion.Text + '\t';
            cadena += rm.GetString("inf35") + this.etValorVarianzaCorreccion.Text + '\t';

            //cadena += " \t OBSERVACIONES \n";
            cadena += '\t' + rm.GetString("inf36"); //+ '\n';
            if (this.observaciones == "")
                //cadena +=  '\t' + "No hay observaciones.";
                cadena += '\t' + rm.GetString("inf37");
            else
            {
                String[] frases = this.observaciones.Split(new char[] { '¬' });
                foreach (String s in frases)
                    cadena += '\t' + s;
            }
            cadena += '\n';
            return cadena;
        }//Fin generar cadena

        /*
         * ***************************************************************
         * BARRA DE MENU. EXCEL Y WORD.
         * ***************************************************************
         */

        /*
         * Inicia Word, solo si no estuviera ya iniciada.
         * Crea un nuevo documento, si ya hubiese un documento creado
         * borra su contenido.
         * Añade el informe al documento.
         */
        private void exportarWordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (doc == null)
                {
                    if (this.aplicacionWord == null)
                    {
                        this.aplicacionWord = new Word.Application();
                        Word.ApplicationEvents4_Event eventosApWord = aplicacionWord;
                        eventosApWord.Quit += new Word.ApplicationEvents4_QuitEventHandler(aplicacionWord_Quit);
                    }
                    Word.ApplicationEvents4_Event eventosDoc = aplicacionWord;
                    eventosDoc.DocumentBeforeClose += new Word.ApplicationEvents4_DocumentBeforeCloseEventHandler(documento_Cerrar);
                    doc = aplicacionWord.Documents.Add(ref opc, ref opc, ref opc, ref opc);
                }
                else
                {
                    this.doc.Range(ref opc, ref opc).Delete(ref opc, ref opc);
                }
                //rellenamos el documento
                string cadenaTexto = generarCadenaInforme();
                String[] cadenaWord = cadenaTexto.Split(new char[] { '\t' });

                for (int i = 0; i < cadenaWord.Length; i++)
                {
                    doc.Content.InsertAfter(cadenaWord[i]);
                    doc.Content.InsertAfter("\n");
                }
                //doc.Content.InsertAfter("Tiempos en tachar cada botón --> Emparejamiento: \n");
                doc.Content.InsertAfter(rm.GetString("inf23") + " \n");
                if (this.tiempos != null)
                {
                    for (int i = 0; i < tiempos.Length; i++)
                    {
                        doc.Content.InsertAfter(rm.GetString("lista42") /* "Botón " */+ " " +
                            i + " : " +
                            FormatTimeSpan(tiempos[i]) +
                            "   --> " + FormatTimeSpan(this.guardarTiemposEmparejamiento[i]) + "\n");
                    }
                }
                aplicacionWord.Visible = true;
            }
            catch (COMException com)
            {
                MessageBox.Show("Error Word..." + com.Message);
                if (this.aplicacionWord != null)
                {
                    aplicacionWord.Quit(ref opc, ref opc, ref opc);
                    this.aplicacionWord_Quit();
                }
            }
        }

        private void aplicacionWord_Quit()
        {
            aplicacionWord = null;
            doc = null;
        }

        private void documento_Cerrar(Microsoft.Office.Interop.Word.Document d, ref bool vacio)
        {
            //doc.Close(ref opc, ref opc, ref opc);
            doc = null;
        }

        /*
         * Si cerramos nuestra aplicacion y esta word abierto,
         * lo cerramos sin guardar ningun cambio ni nada.
         * 2024- Al actualizar las librerías, no es necesario ya cerrar Word o Excel. 
         * Si pulsamos la X, guardamos el usuario que haya en la rejilla como usuario por defecto. 
         */
        private void MetiendoDatos_FormClosing(object sender, FormClosingEventArgs e)
        {
            actualizarUsuarioDefecto();
            /*if (this.aplicacionWord != null)
            {
                aplicacionWord.Quit(ref opc, ref opc, ref opc);
                aplicacionWord = null;
            }
            if (this.apExcel != null)
            {
                apExcel.Quit();
                apExcel = null;
            }
            File.Delete(this.rutaFicheroM3u); Como se va a guardar en la Base de datos la ruta del fichero m3u, ya no se borra al salir de la aplicación.
            */
        }


        //2024 Usando la librería OPEN XML SDK para trabajar con Word y Excel, ya que COM sino está instalado da muchos errores. 
        private void exportarWordToolStripMenuItem2_Click(Object sender, EventArgs e)
        {
            DateTime ahora = DateTime.Now;
            string fechaActual = ahora.ToString("dd_MM_yyyy_HH_mm_ss");

            string rutaAlWord = CarpetaMisDocumentos.ruta() + Path.DirectorySeparatorChar + "Documento_" + fechaActual + ".docx";
            WordprocessingDocument word = WordprocessingDocument.Create(rutaAlWord, DocumentFormat.OpenXml.WordprocessingDocumentType.Document);
            MainDocumentPart mainDocumento = word.AddMainDocumentPart();
            mainDocumento.Document = new word2.Document(new word2.Body());

            // Agregar líneas al documento
            word2.Body body = mainDocumento.Document.Body;
            word2.Text texto = new word2.Text();

            //rellenamos el documento
            string cadenaTexto = generarCadenaInforme();
            String[] cadenaWord = cadenaTexto.Split(new char[] { '\t' });

            foreach (string cadena in cadenaWord)
            {
                word2.Paragraph paragraph = new word2.Paragraph();
                word2.Run run = new word2.Run();
                run.Append(new word2.Text(cadena));
                paragraph.Append(run);
                body.Append(paragraph);
            }

            //("Tiempos en tachar cada botón --> Emparejamiento: \n");
            word2.Paragraph parrafo = new word2.Paragraph();
            word2.Run linea = new word2.Run();
            linea.Append(new word2.Text(rm.GetString("inf23")));
            parrafo.Append(linea);
            body.Append(parrafo);

            if (this.tiempos != null)
            {
                for (int i = 0; i < tiempos.Length; i++)
                {
                    word2.Paragraph parrafo2 = new word2.Paragraph();
                    word2.Run linea2 = new word2.Run();

                    string cadena = rm.GetString("lista42") + " " + i + " : " + FormatTimeSpan(tiempos[i]) + " --> " + FormatTimeSpan(this.guardarTiemposEmparejamiento[i]);
                    linea2.Append(new word2.Text(cadena));
                    parrafo2.Append(linea2);
                    body.Append(parrafo2);
                }


            }
            // Guardar el documento en la carpeta Rejilla de MisDocumentos
            mainDocumento.Document.Save();

            //Cierro el archivo para luego poder abrirlo
            if (word != null)
                word.Dispose();


            // Abrir el documento con la aplicación predeterminada
            Process.Start(new ProcessStartInfo
            {
                FileName = rutaAlWord,
                UseShellExecute = true
            });

        }



        /*
         * Exportar datos a excel.
         */
        private void exportarExcelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExportarDatos ed = new ExportarDatos();
            String[] todosFicheros;
            int pbIndice = 0;
            bool[] campos;
            DialogResult dr = ed.ShowDialog();
            try
            {
                if (dr == DialogResult.OK)

                //abro el libro de excel y lo hago todo
                {
                    WaitExportarExcel wait = new WaitExportarExcel();
                    try
                    {
                        wait.StartPosition = FormStartPosition.CenterScreen;
                        wait.Show();
                        wait.pbTotal.Value = 0;

                        this.fila = 1;
                        if (apExcel == null)
                        {
                            apExcel = new Microsoft.Office.Interop.Excel.Application();
                            apExcel.WorkbookBeforeClose += new Microsoft.Office.Interop.Excel.AppEvents_WorkbookBeforeCloseEventHandler(cerrarLibro);
                            libro = apExcel.Workbooks.Add(opc);
                        }

                        todosFicheros = ed.ficherosExportar;
                        campos = ed.arrayCampos;

                        Excel.Worksheet hoja1 = (Excel.Worksheet)apExcel.ActiveWorkbook.Sheets[1];
                        Excel.Worksheet hoja2 = (Excel.Worksheet)apExcel.ActiveWorkbook.Sheets[2];
                        //rellenar los titulos de las columnas
                        SortedList<int, String> sl = new SortedList<int, string>();
                        rellenarListaBooleana(sl, campos);

                        for (int i = 1; i <= sl.Count; i++)
                        {
                            hoja1.Cells[fila, i] = sl.ElementAt(i - 1).Value;
                            //MessageBox.Show("Longitud sl" + sl.Count +  " --- " + i );
                        }

                        SortedList<int, String> listaHoja2 = new SortedList<int, string>();
                        rellenarHojaDos(listaHoja2, campos);
                        for (int j = 1; j <= listaHoja2.Count; j++)
                        {
                            hoja2.Cells[fila, j] = listaHoja2.ElementAt(j - 1).Value;
                        }

                        this.fila++;

                        for (int i = 0; i < todosFicheros.Length; i++)
                        {
                            //abrir el primer fichero y procesarlo
                            wait.etFichero.Text = "..." + todosFicheros[i].Substring(todosFicheros[i].LastIndexOf(Path.DirectorySeparatorChar));
                            pbIndice = (int)((float)(i + 1) / todosFicheros.Length * 100);
                            if (pbIndice > wait.pbTotal.Value) wait.pbTotal.Value = pbIndice;

                            cargarFicheroExportar(wait, hoja1, hoja2, campos, ed.exportarPor, ed.comienzo, ed.fin,
                                ed.fechaDesde, ed.fechaHasta, ed.deporte, ed.posicion, ed.tipoRejilla,
                                new FileStream(todosFicheros[i], FileMode.Open, FileAccess.Read),
                                ed.pais, ed.joven, ed.viejo, ed.sexoSujeto);

                            this.contadorProgresBar = 1;
                        }
                        wait.Close();
                    }
                    catch (FileNotFoundException fne)
                    {
                        //throw new RejillaException("EXPORTAR FICHERO: No se encuentra el fichero \n");
                        throw new RejillaException(rm.GetString("exp01"));
                    }
                    catch (IOException ie)
                    {
                        //throw new RejillaException("EXPORTAR FICHERO: Error en la lectura del fichero \n.");
                        throw new RejillaException(rm.GetString("exp02"));
                    }
                    catch (UnauthorizedAccessException uae)
                    {
                        //throw new RejillaException("EXPORTAR FICHERO: No tiene permisos para abrir este fichero. \n");
                        throw new RejillaException(rm.GetString("exp03"));
                    }
                    catch (NullReferenceException nre)
                    {
                        //throw new RejillaException("Error de la aplicacion office.");
                        throw new RejillaException(rm.GetString("error33"));
                    }
                    catch (RejillaException re)
                    {
                        //MessageBox.Show(re.Message, "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        wait.Close();
                        MessageBox.Show(re.Message, rm.GetString("war00"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    catch (COMException com)
                    {
                        MessageBox.Show("Error Excel..." + com.Message);
                        if (this.apExcel != null)
                            this.apExcel.Quit();
                    }
                    catch (InvalidCastException coom)
                    {
                        MessageBox.Show("Error Excel..." + coom.Message);
                        if (this.apExcel != null)
                            this.apExcel.Quit();
                    }
                    finally
                    {
                        wait.Close();
                        if (apExcel != null)
                            apExcel.Visible = true;
                    }
                }
            }
            catch (RejillaException rejillaError)
            {
                MessageBox.Show(rejillaError.Message, rm.GetString("war00"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }


        //2024 Exportamos a Excel usando la libreria Open XML Sdk
        private void exportarExcelToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            ExportarDatos ed = new ExportarDatos();
            String[] todosFicheros;
            int pbIndice = 0;
            bool[] campos;
            DialogResult dr = ed.ShowDialog();
            try
            {
                if (dr == DialogResult.OK)

                //abro el libro de excel y lo hago todo
                {
                    WaitExportarExcel wait = new WaitExportarExcel();
                    try
                    {
                        wait.StartPosition = FormStartPosition.CenterScreen;
                        wait.Show();
                        wait.pbTotal.Value = 0;


                        // Ruta donde se guarda el ficerho excel
                        //string rutalAlexcel = CarpetaMisDocumentos.ruta() + Path.DirectorySeparatorChar + "Documento_" + DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss") + ".xlsx";
                        string rutalAlexcel = new CarpetaLocal().carpetaDocumentos() + DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss") + ".xlsx";
                        this.nombreFicheroExcel = rutalAlexcel;
                        using (SpreadsheetDocument libroCalculo = SpreadsheetDocument.Create(rutalAlexcel, DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook))
                        {
                            WorkbookPart libroParte = libroCalculo.AddWorkbookPart();
                            libroParte.Workbook = new excel2.Workbook();
                            excel2.Sheets hojas = libroParte.Workbook.AppendChild(new excel2.Sheets());


                            //Creamos la primera hoja
                            WorksheetPart libroHoja1 = libroParte.AddNewPart<WorksheetPart>();
                            excel2.Worksheet hojita1 = new excel2.Worksheet(new excel2.SheetData());
                            libroHoja1.Worksheet = hojita1;
                            excel2.Sheet algunasHojas = new excel2.Sheet() { Id = libroParte.GetIdOfPart(libroHoja1), SheetId = 1, Name = "Hoja1" };
                            hojas.Append(algunasHojas);

                            //Creamos la segunda hoja
                            WorksheetPart libroHoja2 = libroParte.AddNewPart<WorksheetPart>();
                            excel2.Worksheet hojita2 = new excel2.Worksheet(new excel2.SheetData());
                            libroHoja2.Worksheet = hojita2;
                            excel2.Sheet algunasHojas2 = new excel2.Sheet() { Id = libroParte.GetIdOfPart(libroHoja2), SheetId = 2, Name = "Hoja2" };
                            hojas.Append(algunasHojas2);

                            this.fila = 1;

                            todosFicheros = ed.ficherosExportar;
                            campos = ed.arrayCampos;


                            //Rellenar la hoja1
                            excel2.SheetData sheetData1 = hojita1.GetFirstChild<excel2.SheetData>();
                            excel2.Row row = new excel2.Row();
                            sheetData1.AppendChild(row);
                            SortedList<int, string> listaOrdenada = new SortedList<int, string>();
                            rellenarListaBooleana(listaOrdenada, ed.arrayCampos);
                            for (int i = 1; i <= listaOrdenada.Count; i++)
                            {
                                excel2.Cell celda = new excel2.Cell();
                                celda.CellValue = new excel2.CellValue(listaOrdenada.ElementAt(i - 1).Value);
                                celda.DataType = excel2.CellValues.String;
                                row.AppendChild(celda);
                            }

                            // Rellenar Hoja2
                            excel2.SheetData sheetData2 = hojita2.GetFirstChild<excel2.SheetData>();
                            excel2.Row row2 = new excel2.Row();
                            sheetData2.AppendChild(row2);
                            SortedList<int, String> listaHoja2 = new SortedList<int, string>();
                            rellenarHojaDos(listaHoja2, campos);
                            for (int i = 1; i <= listaHoja2.Count; i++)
                            {
                                excel2.Cell celda = new excel2.Cell();
                                celda.CellValue = new excel2.CellValue(listaHoja2.ElementAt(i - 1).Value);
                                celda.DataType = excel2.CellValues.String;
                                row2.AppendChild(celda);
                            }

                            int filaHoja1 = 1;
                            int filaHoja2 = 1;
                            for (int i = 0; i < todosFicheros.Length; i++)
                            {
                                int[] actualizarIndices = null;
                                //abrir el primer fichero y procesarlo
                                wait.etFichero.Text = "..." + todosFicheros[i].Substring(todosFicheros[i].LastIndexOf(Path.DirectorySeparatorChar));
                                pbIndice = (int)((float)(i + 1) / todosFicheros.Length * 100);
                                if (pbIndice > wait.pbTotal.Value) wait.pbTotal.Value = pbIndice;
                                actualizarIndices = cargarFicheroExportarNuevo(wait, sheetData1, sheetData2, campos, ed.exportarPor, ed.comienzo, ed.fin,
                                ed.fechaDesde, ed.fechaHasta, ed.deporte, ed.posicion, ed.tipoRejilla,
                                new FileStream(todosFicheros[i], FileMode.Open, FileAccess.Read),
                                ed.pais, ed.joven, ed.viejo, ed.sexoSujeto, filaHoja1, filaHoja2);
                                filaHoja1 = actualizarIndices[0];
                                filaHoja2 = actualizarIndices[1];

                                this.contadorProgresBar = 1;
                            }
                            libroHoja1.Worksheet.Save();
                            libroHoja2.Worksheet.Save();
                            libroParte.Workbook.Save();
                            wait.Close();
                        }

                        //El using me cierra el fichero por defecto cuando acaba.

                        // Abrir el documento con la aplicación predeterminada
                        Process.Start(new ProcessStartInfo
                        {
                            FileName = rutalAlexcel,
                            UseShellExecute = true
                        });

                    }
                    catch (FileNotFoundException fne)
                    {
                        //throw new RejillaException("EXPORTAR FICHERO: No se encuentra el fichero \n");
                        throw new RejillaException(rm.GetString("exp01"));
                    }
                    catch (IOException ie)
                    {
                        //throw new RejillaException("EXPORTAR FICHERO: Error en la lectura del fichero \n.");
                        throw new RejillaException(rm.GetString("exp02"));
                    }
                    catch (UnauthorizedAccessException uae)
                    {
                        //throw new RejillaException("EXPORTAR FICHERO: No tiene permisos para abrir este fichero. \n");
                        throw new RejillaException(rm.GetString("exp03"));
                    }
                    catch (NullReferenceException nre)
                    {
                        //throw new RejillaException("Error de la aplicacion office.");
                        throw new RejillaException(rm.GetString("error33"));
                    }
                    catch (RejillaException re)
                    {
                        //MessageBox.Show(re.Message, "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        wait.Close();
                        MessageBox.Show(re.Message, rm.GetString("war00"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    catch (COMException com)
                    {
                        MessageBox.Show("Error Excel..." + com.Message);
                        if (this.apExcel != null)
                            this.apExcel.Quit();
                    }
                    catch (InvalidCastException coom)
                    {
                        MessageBox.Show("Error Excel..." + coom.Message);
                        if (this.apExcel != null)
                            this.apExcel.Quit();
                    }
                    finally
                    {
                        wait.Close();
                        if (apExcel != null)
                            apExcel.Visible = true;
                    }
                }
            }
            catch (RejillaException rejillaError)
            {
                MessageBox.Show(rejillaError.Message, rm.GetString("war00"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private int[] cargarFicheroExportarNuevo(WaitExportarExcel wee, excel2.SheetData hoj1, excel2.SheetData hoj2, bool[] aC, ExportarPor ep,
            string s1, string s2, DateTime f1, DateTime f2, string d, string p, TipoRejilla tr, FileStream fileStream, string paisse,
            string años1, string años2, Genero sexxo, int filaHoja1, int filaHoja2)
        {
            int[] filasConsumidas = new int[2];
            string todoElFichero;
            string[] registrosFichero;

            using (StreamReader flujoLectura = new StreamReader(fileStream))
            {
                todoElFichero = flujoLectura.ReadToEnd();
            }

            // Parto el fichero en registros
            registrosFichero = todoElFichero.Split(new char[] { Convert.ToChar(6) }, StringSplitOptions.RemoveEmptyEntries);
            int carga = registrosFichero.Length;
            wee.pbFichero.Value = 0;

            //int filaHoja1 = 1;
            //int filaHoja2 = 1;

            for (int indRegistros = 0; indRegistros < registrosFichero.Length; indRegistros++)
            {
                if (cumpleCondicion(ep, s1, s2, f1, f2, d, p, tr, registrosFichero[indRegistros], paisse, años1, años2, sexxo))
                {
                    //exportarHojaCalculoNuevo2(aC, registrosFichero[indRegistros], hoj1, hoj2, ref filaHoja1, ref filaHoja2, rowVoyHoja1, rowVoyHoja2);
                    exportarHojaCalculoNuevo(aC, registrosFichero[indRegistros], hoj1, hoj2, filaHoja1, filaHoja2);
                    int tpHecho = (int)((float)(this.contadorProgresBar) / carga * 100);
                    this.fila++;
                    this.contadorProgresBar++;
                    filaHoja1++;
                    filaHoja2++;
                    filasConsumidas[0] = filaHoja1;
                    filasConsumidas[1] = filaHoja2;
                    if (tpHecho > wee.pbFichero.Value) wee.pbFichero.Value = tpHecho;
                }
            }
            return filasConsumidas;
        }


        // Versión 2010 usando COM
        private void cargarFicheroExportar(WaitExportarExcel wee,
            Excel.Worksheet parper, Excel.Worksheet parper2, bool[] aC, ExportarPor ep,
            string s1, string s2, DateTime f1, DateTime f2, string d, string p,
            TipoRejilla tr, FileStream fileStream, string paisse,
            string años1, string años2,
            Genero sexxo)
        {
            String todoElFichero;
            String[] registrosFichero;
            StreamReader flujoLectura = new StreamReader(fileStream);
            int tpHecho = 0;
            int carga;

            todoElFichero = flujoLectura.ReadToEnd();
            //libero el flujo de lectura y los recursos asociados.
            flujoLectura.Close();
            flujoLectura.Dispose();
            GC.Collect();

            //parto el fichero en registros
            registrosFichero = todoElFichero.Split(new char[] { Convert.ToChar(6) }, StringSplitOptions.RemoveEmptyEntries);
            carga = registrosFichero.Length;
            wee.pbFichero.Value = 0;
            //recorro todos los registros y añado los que cumnplen la condicion
            for (int indRegistros = 0; indRegistros < registrosFichero.Length; indRegistros++)
            {
                if (cumpleCondicion(ep, s1, s2, f1, f2, d, p, tr, registrosFichero[indRegistros], paisse, años1, años2, sexxo))
                {
                    //exporto
                    exportarHojaCalculo(aC, registrosFichero[indRegistros], parper, parper2);
                    tpHecho = (int)((float)(this.contadorProgresBar) / carga * 100);
                    this.fila++;
                    this.contadorProgresBar++;
                }
                if (tpHecho > wee.pbFichero.Value) wee.pbFichero.Value = tpHecho;
            }
        }

        /*
         * 2010
         * Método que escribe en el Excell.
         * En la HOJA 1 escribe todo menos lo de la hoja 2.
         * En la hoja dos escribe lo referente a los emparejamientos, los 
         * porcentajes y los tiempos de latencia si los hay.
         */
        private void exportarHojaCalculo(bool[] aC, string p, Excel.Worksheet hoja, Excel.Worksheet hoja2)
        {
            int POS_FECHA = 0;
            int POS_NOMBRE = 1;
            int POS_APELLIDO = 2;
            int POS_DEPORTE = 3;
            int POS_POSICION = 4;
            int POS_EDAD = 5;
            int POS_SEXO = 7;
            int POS_PAIS = 6;
            int POS_TIPOREJILLA = 8;
            int POS_LATERALIDAD = 9; //añado la linea de la lateralidad

            //tres nuevos campos, aciertos, errores y cantidad Botones
            int POS_ACERTOS = 10;
            int POS_FALLOS = 11;
            int POS_CASILLAS = 12;

            //cuatro nuevos campos, emparejamiento, tiempo, posicion y secuencia
            int POS_EMPAREJAMIENTO = 13;
            int POS_TIEMPO_EMPAREJAR = 14;
            int POS_HUBICACION = 15;
            int POS_SECUENCIA = 16;

            //int POS_CASILLA_COLORES_FONDO = 17; //linea nueva
            //mas tres por añadir los campos
            //valor inicial del abecedario y del numerica,
            //valor de las opciones del abecedario

            int POS_ALEATORIO = 11 + 1 + 3 + 4 + 1 + 3;
            int POS_TIEMPO_ALEATORIO = 12 + 1 + 3 + 4 + 1 + 3;
            int POS_LIMITE = 9 + 1 + 3 + 4 + 1 + 3;
            int POS_TIEMPO_LIMITE = 10 + 1 + 3 + 4 + 1 + 3;
            int POS_LINEA_HORIZONTAL = 13 + 1 + 3 + 4 + 1 + 3;
            int POS_LINEA_VERTICAL = 14 + 1 + 3 + 4 + 1 + 3;
            int POS_LINEA_ALEATORIA = 15 + 1 + 3 + 4 + 1 + 3;
            int POS_LINEA_SIN = 16 + 1 + 3 + 4 + 1 + 3;
            int POS_GROSOR = 17 + 1 + 3 + 4 + 1 + 3;
            //int POS_COLOR_A = 18+1;
            //int POS_COLOR_R = 19+1;
            //int POS_COLOR_G = 20+1;
            //int POS_COLOR_B = 21+1;
            int POS_TAMAÑO_BOTON = 22 + 1 + 3 + 4 + 1 + 3;
            //int POS_TAREA_PRELIMINAR = 23;
            //int POS_VALOR_T_P = 24;
            int POS_FILAS = 25 + 1 + 3 + 4 + 1 + 3;
            int POS_COLUMNAS = 26 + 1 + 3 + 4 + 1 + 3;
            // Datos de la gráfica
            int POS_INICIAL_DATOS_GRAFICA = POS_COLUMNAS + 1;

            char[] separChar = new char[5];
            separChar[0] = Convert.ToChar(7);
            separChar[1] = Convert.ToChar(8);
            separChar[2] = '\r';
            separChar[3] = '\n';
            //separChar[4] = '\t';

            bool hayLinea = false;
            String[] cadenaPartida = p.Split(separChar, StringSplitOptions.RemoveEmptyEntries);
            int nF, nC;
            int longitudListaMusic = 0;
            try
            {
                nF = Convert.ToInt32(cadenaPartida[POS_FILAS]);
                nC = Convert.ToInt32(cadenaPartida[POS_COLUMNAS]);
                int longitudDatosGrafica = POS_COLUMNAS + 1 + (nF * nC);

                int POS_FINAL_DATOS_GRAFICA = nF * nC + POS_INICIAL_DATOS_GRAFICA;
                int POS_SINSONIDO = POS_FINAL_DATOS_GRAFICA + 1;
                int POS_METRONOMO = POS_SINSONIDO + 1;
                int POS_VALOR_METRO = POS_METRONOMO + 1;
                int POS_MUSICA = POS_VALOR_METRO + 1;
                int POS_LONGITUD_CANCIONES = 0;
                //si hay canciones se guarda la longitud, sino hay no se guarda nada
                if (Convert.ToBoolean(cadenaPartida[POS_MUSICA]))
                {
                    POS_LONGITUD_CANCIONES = POS_MUSICA + 1;
                }
                else
                {
                    POS_LONGITUD_CANCIONES = POS_MUSICA;
                }
                if (Convert.ToBoolean(cadenaPartida[POS_MUSICA/*longitudDatosGrafica + 4*/]))
                {
                    longitudListaMusic = Convert.ToInt32(cadenaPartida[POS_LONGITUD_CANCIONES/*nF * nC + 32*/]);
                }
                int POS_VELOCIDAD_LINEA = POS_LONGITUD_CANCIONES + longitudListaMusic + 1;
                int POS_OBSERVACIONES = POS_LONGITUD_CANCIONES + 2;
                int POS_CONTROL_TACHADO = POS_LONGITUD_CANCIONES + 3;
                int POS_LONGITUD_CONTROL_TACHADO = POS_LONGITUD_CANCIONES + 4;
                if (longitudListaMusic != 0)
                {
                    POS_CONTROL_TACHADO = POS_LONGITUD_CANCIONES + 3 + longitudListaMusic;
                    POS_OBSERVACIONES = longitudListaMusic + POS_LONGITUD_CANCIONES + 1;
                    POS_LONGITUD_CONTROL_TACHADO = POS_LONGITUD_CANCIONES + 4 + longitudListaMusic;
                }

                int longitud_control_tachado = 0;
                if (Convert.ToBoolean(cadenaPartida[POS_CONTROL_TACHADO]))
                {
                    if (longitudListaMusic != 0)
                    {
                        longitud_control_tachado = Convert.ToInt32(cadenaPartida[POS_LONGITUD_CONTROL_TACHADO]);
                    }
                    else
                    {
                        //necesitamos la longitud del control de tachado
                        longitud_control_tachado = Convert.ToInt32(cadenaPartida[POS_LONGITUD_CONTROL_TACHADO]);
                    }
                }

                int POS_ACIERTOS = POS_LONGITUD_CANCIONES + longitud_control_tachado + 5;
                int POS_ASC_DESC = POS_ACIERTOS + 1;
                int POS_ERRORES = POS_ASC_DESC + 1;
                if (longitudListaMusic != 0)
                {
                    POS_ASC_DESC += longitudListaMusic;
                    POS_ACIERTOS += longitudListaMusic;
                    POS_ERRORES += longitudListaMusic;
                }

                int POS_TAMAÑO_LISTA_ORDEN_PULSACION = POS_ERRORES + 1;
                int POS_LISTA_ORDEN_PULSACION = POS_TAMAÑO_LISTA_ORDEN_PULSACION + 1;
                int POS_TIEMPOS_GRAFICA = POS_LISTA_ORDEN_PULSACION + 1;

                int longitudDatosgrafica = POS_FINAL_DATOS_GRAFICA;
                int indiceCadena = 1;
                int indiceHoja2 = 1;

                if (aC[1])
                {
                    hoja.Cells[fila, indiceCadena] = cadenaPartida[POS_NOMBRE];
                    indiceCadena++;
                }
                if (aC[2])
                {
                    hoja.Cells[fila, indiceCadena] = cadenaPartida[POS_APELLIDO];
                    indiceCadena++;
                }
                if (aC[20])//edad
                {
                    hoja.Cells[fila, indiceCadena] = cadenaPartida[POS_EDAD];//5
                    indiceCadena++;
                }
                if (aC[21])//sexo
                {
                    hoja.Cells[fila, indiceCadena] = cadenaPartida[POS_SEXO];//6
                    indiceCadena++;
                }
                //pais
                if (aC[22])
                {
                    hoja.Cells[fila, indiceCadena] = cadenaPartida[POS_PAIS];//7
                    indiceCadena++;
                }
                //lateralidad
                if (aC[10])//cambio la posicion del color por la lateralidad (color = 10, lateralidad = 23)
                {
                    hoja.Cells[fila, indiceCadena] = cadenaPartida[POS_LATERALIDAD];//9
                    indiceCadena++;
                }
                //deporte
                if (aC[18])
                {
                    hoja.Cells[fila, indiceCadena] = cadenaPartida[POS_DEPORTE];//3
                    indiceCadena++;
                }
                //posicion
                if (aC[19])
                {
                    hoja.Cells[fila, indiceCadena] = cadenaPartida[POS_POSICION];//4
                    indiceCadena++;
                }
                //tipo fecha
                if (aC[3])
                {
                    hoja.Cells[fila, indiceCadena] = cadenaPartida[POS_FECHA];//fecha 0
                    indiceCadena++;
                }
                //observaciones
                if (aC[4])
                {
                    //dejar para cd llegue a ese punto
                    int caballo = POS_OBSERVACIONES;//27 + (nF * nC) + 4 + lMusic + 2;
                    if (longitudListaMusic != 0) caballo++;
                    String cadena = cadenaPartida[caballo];
                    cadena = cadena.Replace('¬', ' ');
                    hoja.Cells[fila, indiceCadena] = cadena;
                    indiceCadena++;
                }
                //tipo rejilla
                if (aC[5])
                {
                    hoja.Cells[fila, indiceCadena] = cadenaPartida[POS_TIPOREJILLA];//5
                    indiceCadena++;
                }
                //aleatorio
                if (aC[6])
                {
                    if (devolverCheked(cadenaPartida[POS_ALEATORIO]))//9
                        hoja.Cells[fila, indiceCadena] = cadenaPartida[POS_TIEMPO_ALEATORIO];//10
                    else
                        hoja.Cells[fila, indiceCadena] = 0;

                    indiceCadena++;
                }
                // tiempo limite
                if (aC[7])
                {
                    if (devolverCheked(cadenaPartida[POS_LIMITE]))//11
                        hoja.Cells[fila, indiceCadena] = cadenaPartida[POS_TIEMPO_LIMITE];//12
                    else
                        hoja.Cells[fila, indiceCadena] = 0;

                    indiceCadena++;
                }
                //tamaño bototn
                if (aC[8])
                {
                    hoja.Cells[fila, indiceCadena] = cadenaPartida[POS_TAMAÑO_BOTON];//22
                    indiceCadena++;
                }
                // tipo de linea
                if (aC[9])
                {
                    if (Convert.ToBoolean(cadenaPartida[POS_LINEA_HORIZONTAL]))//13
                        hoja.Cells[fila, indiceCadena] = rm.GetString("txt06");//"Horizontal";
                    if (Convert.ToBoolean(cadenaPartida[POS_LINEA_VERTICAL]))//14
                        hoja.Cells[fila, indiceCadena] = rm.GetString("txt07"); //"Vertical";
                    if (Convert.ToBoolean(cadenaPartida[POS_LINEA_ALEATORIA]))//15
                        hoja.Cells[fila, indiceCadena] = rm.GetString("txt08"); //"Aleatoria";
                    if (Convert.ToBoolean(cadenaPartida[POS_LINEA_SIN]))//16
                    {
                        hoja.Cells[fila, indiceCadena] = rm.GetString("txt09"); //"Sin linea";
                        hayLinea = false;
                    }
                    indiceCadena++;
                }
                //grosor
                if (aC[11])
                {
                    hoja.Cells[fila, indiceCadena] = cadenaPartida[POS_GROSOR];//17
                    indiceCadena++;
                }
                //ascendente - descendente
                if (aC[12])
                {
                    if (Convert.ToBoolean(cadenaPartida[POS_CONTROL_TACHADO/*pos*/]) &&
                        ((returnTipoRejilla(cadenaPartida[POS_TIPOREJILLA]) == TipoRejilla.NUMERICA) ||
                        (returnTipoRejilla(cadenaPartida[POS_TIPOREJILLA]) == TipoRejilla.ABECEDARIO))
                       )
                    {
                        if (Convert.ToBoolean(cadenaPartida[POS_ASC_DESC/*pos + 1 + l + 2*/]))
                            hoja.Cells[fila, indiceCadena] = rm.GetString("txt02");//"Descendente";
                        else
                            hoja.Cells[fila, indiceCadena] = rm.GetString("txt01"); //"Ascendente";
                    }
                    else
                    {
                        hoja.Cells[fila, indiceCadena] = "null";
                    }
                    indiceCadena++;
                }
                //si hay musica
                if (aC[13])
                {
                    if (longitudListaMusic != 0)
                        hoja.Cells[fila, indiceCadena] = rm.GetString("txt10");//"SI";
                    else
                        hoja.Cells[fila, indiceCadena] = "NO";

                    indiceCadena++;
                }
                //metronomo
                if (aC[14])
                {
                    int pp = (nF * nC) + 24 + 2;
                    if (Convert.ToBoolean(cadenaPartida[POS_METRONOMO/*pp*/]))
                        hoja.Cells[fila, indiceCadena] = cadenaPartida[POS_VALOR_METRO/*pp + 1*/];
                    else
                        hoja.Cells[fila, indiceCadena] = 0;

                    indiceCadena++;
                }
                //aciertos fallos
                if (aC[17])
                {
                    hoja.Cells[fila, indiceCadena] = Convert.ToInt32(cadenaPartida[POS_ACERTOS]);
                    indiceCadena++;
                    hoja.Cells[fila, indiceCadena] = Convert.ToInt32(cadenaPartida[POS_FALLOS]);
                    indiceCadena++;
                }

                //tiempo de la grafica
                if (aC[15])
                {
                    //tiempos grafica
                    hoja.Cells[fila, indiceCadena] = cadenaPartida[cadenaPartida.Length - 10];
                    hoja.Cells[fila, ++indiceCadena] = cadenaPartida[cadenaPartida.Length - 9];
                    hoja.Cells[fila, ++indiceCadena] = cadenaPartida[cadenaPartida.Length - 8];
                    hoja.Cells[fila, ++indiceCadena] = cadenaPartida[cadenaPartida.Length - 7];
                    hoja.Cells[fila, ++indiceCadena] = cadenaPartida[cadenaPartida.Length - 6];
                    hoja.Cells[fila, ++indiceCadena] = cadenaPartida[cadenaPartida.Length - 5];
                    hoja.Cells[fila, ++indiceCadena] = cadenaPartida[cadenaPartida.Length - 4];
                    hoja.Cells[fila, ++indiceCadena] = cadenaPartida[cadenaPartida.Length - 3];
                    hoja.Cells[fila, ++indiceCadena] = cadenaPartida[cadenaPartida.Length - 2];
                    hoja.Cells[fila, ++indiceCadena] = cadenaPartida[cadenaPartida.Length - 1];
                    indiceCadena++;

                }
                //opciones de los emparejamientos
                if (aC[23])
                {
                    hoja2.Cells[fila, indiceHoja2] = cadenaPartida[POS_EMPAREJAMIENTO];
                    hoja2.Cells[fila, ++indiceHoja2] = cadenaPartida[POS_TIEMPO_EMPAREJAR];
                    hoja2.Cells[fila, ++indiceHoja2] = cadenaPartida[POS_HUBICACION];
                    hoja2.Cells[fila, ++indiceHoja2] = cadenaPartida[POS_SECUENCIA];
                }
                //Porcentajes
                if (aC[24])
                {
                    int v1, v2, v3;
                    float[] porcentajes = new float[9];
                    v1 = Convert.ToInt32(cadenaPartida[POS_ACERTOS]);
                    v2 = Convert.ToInt32(cadenaPartida[POS_FALLOS]);
                    v3 = Convert.ToInt32(cadenaPartida[POS_CASILLAS]);
                    porcentajes = obtenerPorcentaje(v1, v2, v3);
                    hoja2.Cells[fila, ++indiceHoja2] = porcentajes[0];//.ToString("##0.##%", CultureInfo.InvariantCulture);
                    hoja2.Cells[fila, ++indiceHoja2] = porcentajes[1];//.ToString("##0.##%", CultureInfo.InvariantCulture);
                    hoja2.Cells[fila, ++indiceHoja2] = porcentajes[2];//.ToString("##0.##%", CultureInfo.InvariantCulture);
                    hoja2.Cells[fila, ++indiceHoja2] = porcentajes[3].ToString("##0.##%", CultureInfo.InvariantCulture);
                    hoja2.Cells[fila, ++indiceHoja2] = porcentajes[4].ToString("##0.##%", CultureInfo.InvariantCulture);
                    hoja2.Cells[fila, ++indiceHoja2] = porcentajes[5].ToString("##0.##%", CultureInfo.InvariantCulture);
                    hoja2.Cells[fila, ++indiceHoja2] = porcentajes[6].ToString("##0.##%", CultureInfo.InvariantCulture);
                    hoja2.Cells[fila, ++indiceHoja2] = porcentajes[7].ToString("##0.##%", CultureInfo.InvariantCulture);
                    hoja2.Cells[fila, ++indiceHoja2] = porcentajes[8].ToString("##0.##%", CultureInfo.InvariantCulture);
                }
                //valores de la grafica
                if (aC[16])
                {
                    //valores grafica
                    DateTime[] valoresTiempos = new DateTime[nF * nC + 1];
                    TimeSpan[] valoresEmparejamiento = new TimeSpan[valoresTiempos.Length];
                    long ticks = 0;
                    TimeSpan ts = new TimeSpan();
                    String[] nuevo;

                    for (int i = POS_INICIAL_DATOS_GRAFICA; i < valoresTiempos.Length + POS_INICIAL_DATOS_GRAFICA; i++)
                    {
                        nuevo = cadenaPartida[i].Split(new char[] { '\t' });
                        ticks = Convert.ToInt64(nuevo[0]);
                        //ticks = System.Convert.ToInt64(cadenaPartida[i]);
                        if (ticks != 0)
                            valoresTiempos[i - POS_INICIAL_DATOS_GRAFICA] = new DateTime(ticks);
                        else
                            valoresTiempos[i - POS_INICIAL_DATOS_GRAFICA] = new DateTime();

                        //meter aki los tiempos de emparejamiento en su hoja respectiva
                        ticks = Convert.ToInt64(nuevo[1]);
                        if (ticks != 0)
                            valoresEmparejamiento[i - POS_INICIAL_DATOS_GRAFICA] = new TimeSpan(ticks);
                        else
                            valoresEmparejamiento[i - POS_INICIAL_DATOS_GRAFICA] = new TimeSpan();


                    }

                    int ind = 0;
                    indiceHoja2++;
                    while ((ind < valoresTiempos.Length - 1) && valoresTiempos[ind + 1] != new DateTime())
                    {
                        ts = valoresTiempos[ind + 1] - valoresTiempos[ind];
                        hoja.Cells[fila, indiceCadena] = ts.TotalSeconds;
                        hoja2.Cells[fila, indiceHoja2] = valoresEmparejamiento[ind].TotalSeconds;
                        indiceCadena++;
                        indiceHoja2++;
                        ind++;
                    }
                }
            }
            catch (IndexOutOfRangeException io)
            {
                //throw new RejillaException("Fichero dañado..........");
                throw new RejillaException(rm.GetString("error24") + io.Message);
            }
            catch (FormatException fe)
            {
                //throw new RejillaException("Fichero dañado..........");
                throw new RejillaException(rm.GetString("error24") + fe.Message);
            }
        }


        /*
         * 2024-Nueva versión usando Open XML SDK
         * Método que escribe en el Excell.
         * En la HOJA 1 escribe todo menos lo de la hoja 2.
         * En la hoja dos escribe lo referente a los emparejamientos, los 
         * porcentajes y los tiempos de latencia si los hay.
         */



        private void AddCellToRow(excel2.Row row, string value, uint columnIndex)
        {
            excel2.Cell cell = new excel2.Cell();
            cell.CellValue = new excel2.CellValue(value);
            cell.DataType = excel2.CellValues.String;

            row.Append(cell);
        }

        private void exportarHojaCalculoNuevo(bool[] aC, string p, excel2.SheetData sheetData1, excel2.SheetData sheetData2, int filaHoja1, int filaHoja2)
        {
            int POS_FECHA = 0;
            int POS_NOMBRE = 1;
            int POS_APELLIDO = 2;
            int POS_DEPORTE = 3;
            int POS_POSICION = 4;
            int POS_EDAD = 5;
            int POS_SEXO = 7;
            int POS_PAIS = 6;
            int POS_TIPOREJILLA = 8;
            int POS_LATERALIDAD = 9; //añado la linea de la lateralidad

            //tres nuevos campos, aciertos, errores y cantidad Botones
            int POS_ACERTOS = 10;
            int POS_FALLOS = 11;
            int POS_CASILLAS = 12;

            //cuatro nuevos campos, emparejamiento, tiempo, posicion y secuencia
            int POS_EMPAREJAMIENTO = 13;
            int POS_TIEMPO_EMPAREJAR = 14;
            int POS_HUBICACION = 15;
            int POS_SECUENCIA = 16;

            //int POS_CASILLA_COLORES_FONDO = 17; //linea nueva
            //mas tres por añadir los campos
            //valor inicial del abecedario y del numerica,
            //valor de las opciones del abecedario

            int POS_ALEATORIO = 11 + 1 + 3 + 4 + 1 + 3;
            int POS_TIEMPO_ALEATORIO = 12 + 1 + 3 + 4 + 1 + 3;
            int POS_LIMITE = 9 + 1 + 3 + 4 + 1 + 3;
            int POS_TIEMPO_LIMITE = 10 + 1 + 3 + 4 + 1 + 3;
            int POS_LINEA_HORIZONTAL = 13 + 1 + 3 + 4 + 1 + 3;
            int POS_LINEA_VERTICAL = 14 + 1 + 3 + 4 + 1 + 3;
            int POS_LINEA_ALEATORIA = 15 + 1 + 3 + 4 + 1 + 3;
            int POS_LINEA_SIN = 16 + 1 + 3 + 4 + 1 + 3;
            int POS_GROSOR = 17 + 1 + 3 + 4 + 1 + 3;
            //int POS_COLOR_A = 18+1;
            //int POS_COLOR_R = 19+1;
            //int POS_COLOR_G = 20+1;
            //int POS_COLOR_B = 21+1;
            int POS_TAMAÑO_BOTON = 22 + 1 + 3 + 4 + 1 + 3;
            //int POS_TAREA_PRELIMINAR = 23;
            //int POS_VALOR_T_P = 24;
            int POS_FILAS = 25 + 1 + 3 + 4 + 1 + 3;
            int POS_COLUMNAS = 26 + 1 + 3 + 4 + 1 + 3;
            // Datos de la gráfica
            int POS_INICIAL_DATOS_GRAFICA = POS_COLUMNAS + 1;

            char[] separChar = new char[5];
            separChar[0] = Convert.ToChar(7);
            separChar[1] = Convert.ToChar(8);
            separChar[2] = '\r';
            separChar[3] = '\n';
            //separChar[4] = '\t';

            bool hayLinea = false;
            String[] cadenaPartida = p.Split(separChar, StringSplitOptions.RemoveEmptyEntries);
            int nF, nC;
            int longitudListaMusic = 0;
            try
            {
                nF = Convert.ToInt32(cadenaPartida[POS_FILAS]);
                nC = Convert.ToInt32(cadenaPartida[POS_COLUMNAS]);
                int longitudDatosGrafica = POS_COLUMNAS + 1 + (nF * nC);

                int POS_FINAL_DATOS_GRAFICA = nF * nC + POS_INICIAL_DATOS_GRAFICA;
                int POS_SINSONIDO = POS_FINAL_DATOS_GRAFICA + 1;
                int POS_METRONOMO = POS_SINSONIDO + 1;
                int POS_VALOR_METRO = POS_METRONOMO + 1;
                int POS_MUSICA = POS_VALOR_METRO + 1;
                int POS_LONGITUD_CANCIONES = 0;
                //si hay canciones se guarda la longitud, sino hay no se guarda nada
                if (Convert.ToBoolean(cadenaPartida[POS_MUSICA]))
                {
                    POS_LONGITUD_CANCIONES = POS_MUSICA + 1;
                }
                else
                {
                    POS_LONGITUD_CANCIONES = POS_MUSICA;
                }
                if (Convert.ToBoolean(cadenaPartida[POS_MUSICA/*longitudDatosGrafica + 4*/]))
                {
                    longitudListaMusic = Convert.ToInt32(cadenaPartida[POS_LONGITUD_CANCIONES/*nF * nC + 32*/]);
                }
                int POS_VELOCIDAD_LINEA = POS_LONGITUD_CANCIONES + longitudListaMusic + 1;
                int POS_OBSERVACIONES = POS_LONGITUD_CANCIONES + 2;
                int POS_CONTROL_TACHADO = POS_LONGITUD_CANCIONES + 3;
                int POS_LONGITUD_CONTROL_TACHADO = POS_LONGITUD_CANCIONES + 4;
                if (longitudListaMusic != 0)
                {
                    POS_CONTROL_TACHADO = POS_LONGITUD_CANCIONES + 3 + longitudListaMusic;
                    POS_OBSERVACIONES = longitudListaMusic + POS_LONGITUD_CANCIONES + 1;
                    POS_LONGITUD_CONTROL_TACHADO = POS_LONGITUD_CANCIONES + 4 + longitudListaMusic;
                }

                int longitud_control_tachado = 0;
                if (Convert.ToBoolean(cadenaPartida[POS_CONTROL_TACHADO]))
                {
                    if (longitudListaMusic != 0)
                    {
                        longitud_control_tachado = Convert.ToInt32(cadenaPartida[POS_LONGITUD_CONTROL_TACHADO]);
                    }
                    else
                    {
                        //necesitamos la longitud del control de tachado
                        longitud_control_tachado = Convert.ToInt32(cadenaPartida[POS_LONGITUD_CONTROL_TACHADO]);
                    }
                }

                int POS_ACIERTOS = POS_LONGITUD_CANCIONES + longitud_control_tachado + 5;
                int POS_ASC_DESC = POS_ACIERTOS + 1;
                int POS_ERRORES = POS_ASC_DESC + 1;
                if (longitudListaMusic != 0)
                {
                    POS_ASC_DESC += longitudListaMusic;
                    POS_ACIERTOS += longitudListaMusic;
                    POS_ERRORES += longitudListaMusic;
                }

                int POS_TAMAÑO_LISTA_ORDEN_PULSACION = POS_ERRORES + 1;
                int POS_LISTA_ORDEN_PULSACION = POS_TAMAÑO_LISTA_ORDEN_PULSACION + 1;
                int POS_TIEMPOS_GRAFICA = POS_LISTA_ORDEN_PULSACION + 1;

                int longitudDatosgrafica = POS_FINAL_DATOS_GRAFICA;
                int indiceCadena = 1;
                int indiceHoja2 = 1;


                excel2.Row row = new excel2.Row() { RowIndex = (uint)(filaHoja1 + 1) };
                string valorCelda = "";


                excel2.Row row2 = new excel2.Row() { RowIndex = (uint)(filaHoja2 + 1) };
                string valorCelda2 = "";


                if (aC[1])
                {
                    valorCelda = cadenaPartida[POS_NOMBRE];
                    AddCellToRow(row, valorCelda, (uint)indiceCadena + 1);
                    indiceCadena++;
                }
                //sheetData1.Append(row);
                if (aC[2])
                {
                    valorCelda = cadenaPartida[POS_APELLIDO];
                    AddCellToRow(row, valorCelda, (uint)indiceCadena + 1);
                    indiceCadena++;
                }


                if (aC[20])//edad
                {
                    valorCelda = cadenaPartida[POS_EDAD];
                    AddCellToRow(row, valorCelda, (uint)indiceCadena + 1);//5
                    indiceCadena++;
                }
                if (aC[21])//sexo
                {
                    valorCelda = cadenaPartida[POS_SEXO];//6
                    AddCellToRow(row, valorCelda, (uint)indiceCadena + 1);
                    indiceCadena++;
                }
                //pais
                if (aC[22])
                {
                    valorCelda = cadenaPartida[POS_PAIS];//7
                    AddCellToRow(row, valorCelda, (uint)indiceCadena + 1);
                    indiceCadena++;
                }
                //lateralidad
                if (aC[10])//cambio la posicion del color por la lateralidad (color = 10, lateralidad = 23)
                {
                    valorCelda = cadenaPartida[POS_LATERALIDAD];//9
                    AddCellToRow(row, valorCelda, (uint)indiceCadena + 1);
                    indiceCadena++;
                }
                //deporte
                if (aC[18])
                {
                    valorCelda = cadenaPartida[POS_DEPORTE];//3
                    AddCellToRow(row, valorCelda, (uint)indiceCadena + 1);
                    indiceCadena++;
                }
                //posicion
                if (aC[19])
                {
                    valorCelda = cadenaPartida[POS_POSICION];//4
                    AddCellToRow(row, valorCelda, (uint)indiceCadena + 1);
                    indiceCadena++;
                }
                //tipo fecha
                if (aC[3])
                {
                    valorCelda = cadenaPartida[POS_FECHA];//0
                    AddCellToRow(row, valorCelda, (uint)indiceCadena + 1);
                    indiceCadena++;
                }
                //observaciones
                if (aC[4])
                {
                    //dejar para cd llegue a ese punto
                    int caballo = POS_OBSERVACIONES;//27 + (nF * nC) + 4 + lMusic + 2;
                    if (longitudListaMusic != 0) caballo++;
                    String cadena = cadenaPartida[caballo];
                    cadena = cadena.Replace('¬', ' ');
                    valorCelda = cadena;
                    AddCellToRow(row, valorCelda, (uint)indiceCadena + 1);
                    indiceCadena++;
                }
                //tipo rejilla
                if (aC[5])
                {
                    valorCelda = cadenaPartida[POS_TIPOREJILLA];//5
                    AddCellToRow(row, valorCelda, (uint)indiceCadena + 1);
                    indiceCadena++;
                }
                //aleatorio
                if (aC[6])
                {
                    if (devolverCheked(cadenaPartida[POS_ALEATORIO]))//9
                    {
                        valorCelda = cadenaPartida[POS_TIEMPO_ALEATORIO];//10
                        AddCellToRow(row, valorCelda, (uint)indiceCadena + 1);
                    }

                    else
                    {
                        AddCellToRow(row, "0", (uint)indiceCadena + 1);
                    }
                    indiceCadena++;
                }
                // tiempo limite
                if (aC[7])
                {
                    if (devolverCheked(cadenaPartida[POS_LIMITE]))//11
                    {
                        valorCelda = cadenaPartida[POS_TIEMPO_LIMITE];//12
                        AddCellToRow(row, valorCelda, (uint)indiceCadena + 1);
                    }

                    else
                    {
                        AddCellToRow(row, "0", (uint)indiceCadena + 1);
                    }
                    indiceCadena++;
                }
                //tamaño botÓn
                if (aC[8])
                {
                    valorCelda = cadenaPartida[POS_TAMAÑO_BOTON];//22
                    AddCellToRow(row, valorCelda, (uint)indiceCadena + 1);
                    indiceCadena++;
                }
                // tipo de linea
                if (aC[9])
                {
                    if (Convert.ToBoolean(cadenaPartida[POS_LINEA_HORIZONTAL]))//13
                    {
                        valorCelda = rm.GetString("txt06");//"Horizontal";
                        AddCellToRow(row, valorCelda, (uint)indiceCadena + 1);
                    }
                    if (Convert.ToBoolean(cadenaPartida[POS_LINEA_VERTICAL]))//14
                    {
                        valorCelda = rm.GetString("txt07");//"Vertical";
                        AddCellToRow(row, valorCelda, (uint)indiceCadena + 1);
                    }
                    if (Convert.ToBoolean(cadenaPartida[POS_LINEA_ALEATORIA]))//15
                    {
                        valorCelda = rm.GetString("txt08");//"Aleatoria";
                        AddCellToRow(row, valorCelda, (uint)indiceCadena + 1);
                    }
                    if (Convert.ToBoolean(cadenaPartida[POS_LINEA_SIN]))//16
                    {
                        valorCelda = rm.GetString("txt09");//"Sin linea";
                        AddCellToRow(row, valorCelda, (uint)indiceCadena + 1);
                        hayLinea = false;
                    }
                    indiceCadena++;
                }
                //grosor
                if (aC[11])
                {
                    valorCelda = cadenaPartida[POS_GROSOR];//17
                    AddCellToRow(row, valorCelda, (uint)indiceCadena + 1);
                    indiceCadena++;
                }
                //ascendente - descendente
                if (aC[12])
                {
                    if (Convert.ToBoolean(cadenaPartida[POS_CONTROL_TACHADO/*pos*/]) &&
                        ((returnTipoRejilla(cadenaPartida[POS_TIPOREJILLA]) == TipoRejilla.NUMERICA) ||
                        (returnTipoRejilla(cadenaPartida[POS_TIPOREJILLA]) == TipoRejilla.ABECEDARIO))
                       )
                    {
                        if (Convert.ToBoolean(cadenaPartida[POS_ASC_DESC/*pos + 1 + l + 2*/]))
                        {
                            valorCelda = rm.GetString("txt02");//"Descendente";
                            AddCellToRow(row, valorCelda, (uint)indiceCadena + 1);
                        }
                        else
                        {
                            valorCelda = rm.GetString("txt01");//"Ascendente";
                            AddCellToRow(row, valorCelda, (uint)indiceCadena + 1);
                        }
                    }
                    else
                    {
                        valorCelda = "null";
                        AddCellToRow(row, valorCelda, (uint)indiceCadena + 1);
                    }
                    indiceCadena++;
                }
                //si hay musica
                if (aC[13])
                {
                    if (longitudListaMusic != 0)
                    {
                        valorCelda = rm.GetString("txt10");//"SI";
                        AddCellToRow(row, valorCelda, (uint)indiceCadena + 1);
                    }
                    else
                    {
                        valorCelda = "NO";//"NO";
                        AddCellToRow(row, valorCelda, (uint)indiceCadena + 1);
                    }
                    indiceCadena++;
                }
                //metronomo
                if (aC[14])
                {
                    int pp = (nF * nC) + 24 + 2;
                    if (Convert.ToBoolean(cadenaPartida[POS_METRONOMO/*pp*/]))
                    {
                        valorCelda = cadenaPartida[POS_VALOR_METRO];// pp + 1
                        AddCellToRow(row, valorCelda, (uint)indiceCadena + 1);
                    }
                    else
                    {
                        valorCelda = "0";
                        AddCellToRow(row, valorCelda, (uint)indiceCadena + 1);
                    }
                    indiceCadena++;
                }
                //aciertos fallos
                if (aC[17])
                {
                    valorCelda = cadenaPartida[POS_ACERTOS];
                    AddCellToRow(row, valorCelda, (uint)indiceCadena + 1);
                    indiceCadena++;
                    valorCelda = cadenaPartida[POS_FALLOS];
                    AddCellToRow(row, valorCelda, (uint)indiceCadena + 1);
                    indiceCadena++;
                }

                //tiempo de la grafica
                if (aC[15])
                {
                    //tiempos grafica
                    valorCelda = cadenaPartida[cadenaPartida.Length - 10];
                    AddCellToRow(row, valorCelda, (uint)indiceCadena + 1);
                    indiceCadena++;
                    valorCelda = cadenaPartida[cadenaPartida.Length - 9];
                    AddCellToRow(row, valorCelda, (uint)indiceCadena + 1);
                    indiceCadena++;
                    valorCelda = cadenaPartida[cadenaPartida.Length - 8];
                    AddCellToRow(row, valorCelda, (uint)indiceCadena + 1);
                    indiceCadena++;
                    valorCelda = cadenaPartida[cadenaPartida.Length - 7];
                    AddCellToRow(row, valorCelda, (uint)indiceCadena + 1);
                    indiceCadena++;
                    valorCelda = cadenaPartida[cadenaPartida.Length - 6];
                    AddCellToRow(row, valorCelda, (uint)indiceCadena + 1);
                    indiceCadena++;
                    valorCelda = cadenaPartida[cadenaPartida.Length - 5];
                    AddCellToRow(row, valorCelda, (uint)indiceCadena + 1);
                    indiceCadena++;
                    valorCelda = cadenaPartida[cadenaPartida.Length - 4];
                    AddCellToRow(row, valorCelda, (uint)indiceCadena + 1);
                    indiceCadena++;
                    valorCelda = cadenaPartida[cadenaPartida.Length - 3];
                    AddCellToRow(row, valorCelda, (uint)indiceCadena + 1);
                    indiceCadena++;
                    valorCelda = cadenaPartida[cadenaPartida.Length - 2];
                    AddCellToRow(row, valorCelda, (uint)indiceCadena + 1);
                    indiceCadena++;
                    valorCelda = cadenaPartida[cadenaPartida.Length - 1];
                    AddCellToRow(row, valorCelda, (uint)indiceCadena + 1);
                    indiceCadena++;
                }
                //opciones de los emparejamientos
                if (aC[23])
                {
                    valorCelda = cadenaPartida[POS_EMPAREJAMIENTO];
                    AddCellToRow(row, valorCelda, (uint)indiceCadena + 1);
                    indiceCadena++;
                    valorCelda = cadenaPartida[POS_TIEMPO_EMPAREJAR];
                    AddCellToRow(row, valorCelda, (uint)indiceCadena + 1);
                    indiceCadena++;
                    valorCelda = cadenaPartida[POS_HUBICACION];
                    AddCellToRow(row, valorCelda, (uint)indiceCadena + 1);
                    indiceCadena++;
                    valorCelda = cadenaPartida[POS_SECUENCIA];
                    AddCellToRow(row, valorCelda, (uint)indiceCadena + 1);
                    indiceCadena++;

                    valorCelda2 = cadenaPartida[POS_EMPAREJAMIENTO];
                    AddCellToRow(row2, valorCelda2, (uint)indiceHoja2 + 1);
                    indiceHoja2++;
                    valorCelda2 = cadenaPartida[POS_TIEMPO_EMPAREJAR];
                    AddCellToRow(row2, valorCelda2, (uint)indiceHoja2 + 1);
                    indiceHoja2++;
                    valorCelda2 = cadenaPartida[POS_HUBICACION];
                    AddCellToRow(row2, valorCelda2, (uint)indiceHoja2 + 1);
                    indiceHoja2++;
                    valorCelda2 = cadenaPartida[POS_SECUENCIA];
                    AddCellToRow(row2, valorCelda2, (uint)indiceHoja2 + 1);
                    //filaHoja2++;
                    indiceHoja2++;
                }
                //Porcentajes
                if (aC[24])
                {
                    int v1, v2, v3;
                    float[] porcentajes = new float[9];
                    v1 = Convert.ToInt32(cadenaPartida[POS_ACERTOS]);
                    v2 = Convert.ToInt32(cadenaPartida[POS_FALLOS]);
                    v3 = Convert.ToInt32(cadenaPartida[POS_CASILLAS]);
                    porcentajes = obtenerPorcentaje(v1, v2, v3);
                    valorCelda = porcentajes[0].ToString();
                    AddCellToRow(row, valorCelda, (uint)indiceCadena + 1);
                    indiceCadena++;
                    valorCelda = porcentajes[1].ToString();
                    AddCellToRow(row, valorCelda, (uint)indiceCadena + 1);
                    indiceCadena++;
                    valorCelda = porcentajes[2].ToString();
                    AddCellToRow(row, valorCelda, (uint)indiceCadena + 1);
                    indiceCadena++;
                    valorCelda = porcentajes[3].ToString("##0.##%", CultureInfo.InvariantCulture);
                    AddCellToRow(row, valorCelda, (uint)indiceCadena + 1);
                    indiceCadena++;
                    valorCelda = porcentajes[4].ToString("##0.##%", CultureInfo.InvariantCulture);
                    AddCellToRow(row, valorCelda, (uint)indiceCadena + 1);
                    indiceCadena++;
                    valorCelda = porcentajes[5].ToString("##0.##%", CultureInfo.InvariantCulture);
                    AddCellToRow(row, valorCelda, (uint)indiceCadena + 1);
                    indiceCadena++;
                    valorCelda = porcentajes[6].ToString("##0.##%", CultureInfo.InvariantCulture);
                    AddCellToRow(row, valorCelda, (uint)indiceCadena + 1);
                    indiceCadena++;
                    valorCelda = porcentajes[7].ToString("##0.##%", CultureInfo.InvariantCulture);
                    AddCellToRow(row, valorCelda, (uint)indiceCadena + 1);
                    indiceCadena++;
                    valorCelda = porcentajes[8].ToString("##0.##%", CultureInfo.InvariantCulture);
                    AddCellToRow(row, valorCelda, (uint)indiceCadena + 1);
                    indiceCadena++;


                    valorCelda2 = porcentajes[0].ToString();
                    AddCellToRow(row2, valorCelda2, (uint)indiceHoja2 + 1);
                    indiceHoja2++;
                    valorCelda2 = porcentajes[1].ToString();
                    AddCellToRow(row2, valorCelda2, (uint)indiceHoja2 + 1);
                    indiceHoja2++;
                    valorCelda2 = porcentajes[2].ToString();
                    AddCellToRow(row2, valorCelda2, (uint)indiceHoja2 + 1);
                    indiceHoja2++;
                    valorCelda2 = porcentajes[3].ToString("##0.##%", CultureInfo.InvariantCulture);
                    AddCellToRow(row2, valorCelda2, (uint)indiceHoja2 + 1);
                    indiceHoja2++;
                    valorCelda2 = porcentajes[4].ToString("##0.##%", CultureInfo.InvariantCulture);
                    AddCellToRow(row2, valorCelda2, (uint)indiceHoja2 + 1);
                    indiceHoja2++;
                    valorCelda2 = porcentajes[5].ToString("##0.##%", CultureInfo.InvariantCulture);
                    AddCellToRow(row2, valorCelda2, (uint)indiceHoja2 + 1);
                    indiceHoja2++;
                    valorCelda2 = porcentajes[6].ToString("##0.##%", CultureInfo.InvariantCulture);
                    AddCellToRow(row2, valorCelda2, (uint)indiceHoja2 + 1);
                    indiceHoja2++;
                    valorCelda2 = porcentajes[7].ToString("##0.##%", CultureInfo.InvariantCulture);
                    AddCellToRow(row2, valorCelda2, (uint)indiceHoja2 + 1);
                    indiceHoja2++;
                    valorCelda2 = porcentajes[8].ToString("##0.##%", CultureInfo.InvariantCulture);
                    AddCellToRow(row2, valorCelda2, (uint)indiceHoja2 + 1);
                    indiceHoja2++;
                }
                //valores de la grafica
                if (aC[16])
                {
                    //valores grafica
                    DateTime[] valoresTiempos = new DateTime[nF * nC + 1];
                    TimeSpan[] valoresEmparejamiento = new TimeSpan[valoresTiempos.Length];
                    long ticks = 0;
                    TimeSpan ts = new TimeSpan();
                    String[] nuevo;

                    for (int i = POS_INICIAL_DATOS_GRAFICA; i < valoresTiempos.Length + POS_INICIAL_DATOS_GRAFICA; i++)
                    {
                        nuevo = cadenaPartida[i].Split(new char[] { '\t' });
                        ticks = Convert.ToInt64(nuevo[0]);
                        //ticks = System.Convert.ToInt64(cadenaPartida[i]);
                        if (ticks != 0)
                            valoresTiempos[i - POS_INICIAL_DATOS_GRAFICA] = new DateTime(ticks);
                        else
                            valoresTiempos[i - POS_INICIAL_DATOS_GRAFICA] = new DateTime();

                        //meter aki los tiempos de emparejamiento en su hoja respectiva
                        ticks = Convert.ToInt64(nuevo[1]);
                        if (ticks != 0)
                            valoresEmparejamiento[i - POS_INICIAL_DATOS_GRAFICA] = new TimeSpan(ticks);
                        else
                            valoresEmparejamiento[i - POS_INICIAL_DATOS_GRAFICA] = new TimeSpan();


                    }

                    int ind = 0;
                    indiceHoja2++;
                    while ((ind < valoresTiempos.Length - 1) && valoresTiempos[ind + 1] != new DateTime())
                    {
                        ts = valoresTiempos[ind + 1] - valoresTiempos[ind];
                        valorCelda = ts.TotalSeconds.ToString();
                        AddCellToRow(row, valorCelda, (uint)indiceCadena + 1);

                        valorCelda2 = valoresEmparejamiento[ind].TotalSeconds.ToString();
                        AddCellToRow(row2, valorCelda2, (uint)indiceHoja2 + 1);
                        indiceCadena++;
                        indiceHoja2++;
                        ind++;
                    }
                }
                //Agregamos las filas a la hoja de cálculo.
                sheetData1.Append(row);
                sheetData2.Append(row2);
                filaHoja1++;
                filaHoja2++;

            }
            catch (IndexOutOfRangeException io)
            {
                //throw new RejillaException("Fichero dañado..........");
                throw new RejillaException(rm.GetString("error24") + io.Message);
            }
            catch (FormatException fe)
            {
                //throw new RejillaException("Fichero dañado..........");
                throw new RejillaException(rm.GetString("error24") + fe.Message);
            }
        }

        //private void exportarHojaCalculoNuevo2(bool[] aC, string p, excel2.SheetData sheetData1, excel2.SheetData sheetData2, ref int filaHoja1, ref int filaHoja2, uint rh1, uint rh2)
        //{

        //    int POS_NOMBRE = 1;
        //    int POS_APELLIDO = 2;


        //    int POS_FILAS = 25 + 1 + 3 + 4 + 1 + 3;
        //    int POS_COLUMNAS = 26 + 1 + 3 + 4 + 1 + 3;
        //    // Datos de la gráfica
        //    int POS_INICIAL_DATOS_GRAFICA = POS_COLUMNAS + 1;

        //    char[] separChar = new char[5];
        //    separChar[0] = Convert.ToChar(7);
        //    separChar[1] = Convert.ToChar(8);
        //    separChar[2] = '\r';
        //    separChar[3] = '\n';
        //    //separChar[4] = '\t';

        //    bool hayLinea = false;
        //    String[] cadenaPartida = p.Split(separChar, StringSplitOptions.RemoveEmptyEntries);
        //    int nF, nC;
        //    int longitudListaMusic = 0;
        //    try
        //    {
        //        nF = Convert.ToInt32(cadenaPartida[POS_FILAS]);
        //        nC = Convert.ToInt32(cadenaPartida[POS_COLUMNAS]);
        //        int longitudDatosGrafica = POS_COLUMNAS + 1 + (nF * nC);

        //        int POS_FINAL_DATOS_GRAFICA = nF * nC + POS_INICIAL_DATOS_GRAFICA;
        //        int POS_SINSONIDO = POS_FINAL_DATOS_GRAFICA + 1;
        //        int POS_METRONOMO = POS_SINSONIDO + 1;
        //        int POS_VALOR_METRO = POS_METRONOMO + 1;
        //        int POS_MUSICA = POS_VALOR_METRO + 1;
        //        int POS_LONGITUD_CANCIONES = 0;
        //        //si hay canciones se guarda la longitud, sino hay no se guarda nada
        //        if (Convert.ToBoolean(cadenaPartida[POS_MUSICA]))
        //        {
        //            POS_LONGITUD_CANCIONES = POS_MUSICA + 1;
        //        }
        //        else
        //        {
        //            POS_LONGITUD_CANCIONES = POS_MUSICA;
        //        }
        //        if (Convert.ToBoolean(cadenaPartida[POS_MUSICA/*longitudDatosGrafica + 4*/]))
        //        {
        //            longitudListaMusic = Convert.ToInt32(cadenaPartida[POS_LONGITUD_CANCIONES/*nF * nC + 32*/]);
        //        }
        //        int POS_VELOCIDAD_LINEA = POS_LONGITUD_CANCIONES + longitudListaMusic + 1;
        //        int POS_OBSERVACIONES = POS_LONGITUD_CANCIONES + 2;
        //        int POS_CONTROL_TACHADO = POS_LONGITUD_CANCIONES + 3;
        //        int POS_LONGITUD_CONTROL_TACHADO = POS_LONGITUD_CANCIONES + 4;
        //        if (longitudListaMusic != 0)
        //        {
        //            POS_CONTROL_TACHADO = POS_LONGITUD_CANCIONES + 3 + longitudListaMusic;
        //            POS_OBSERVACIONES = longitudListaMusic + POS_LONGITUD_CANCIONES + 1;
        //            POS_LONGITUD_CONTROL_TACHADO = POS_LONGITUD_CANCIONES + 4 + longitudListaMusic;
        //        }

        //        int longitud_control_tachado = 0;
        //        if (Convert.ToBoolean(cadenaPartida[POS_CONTROL_TACHADO]))
        //        {
        //            if (longitudListaMusic != 0)
        //            {
        //                longitud_control_tachado = Convert.ToInt32(cadenaPartida[POS_LONGITUD_CONTROL_TACHADO]);
        //            }
        //            else
        //            {
        //                //necesitamos la longitud del control de tachado
        //                longitud_control_tachado = Convert.ToInt32(cadenaPartida[POS_LONGITUD_CONTROL_TACHADO]);
        //            }
        //        }

        //        int POS_ACIERTOS = POS_LONGITUD_CANCIONES + longitud_control_tachado + 5;
        //        int POS_ASC_DESC = POS_ACIERTOS + 1;
        //        int POS_ERRORES = POS_ASC_DESC + 1;
        //        if (longitudListaMusic != 0)
        //        {
        //            POS_ASC_DESC += longitudListaMusic;
        //            POS_ACIERTOS += longitudListaMusic;
        //            POS_ERRORES += longitudListaMusic;
        //        }

        //        int POS_TAMAÑO_LISTA_ORDEN_PULSACION = POS_ERRORES + 1;
        //        int POS_LISTA_ORDEN_PULSACION = POS_TAMAÑO_LISTA_ORDEN_PULSACION + 1;
        //        int POS_TIEMPOS_GRAFICA = POS_LISTA_ORDEN_PULSACION + 1;

        //        int longitudDatosgrafica = POS_FINAL_DATOS_GRAFICA;
        //        int indiceCadena = 1;


        //        excel2.Row row = new excel2.Row() { RowIndex = (uint)(indiceCadena + 1) };

        //        if (aC[1])
        //        {
        //            excel2.Cell celda = new excel2.Cell();
        //            //valorCelda = new excel2.CellValue(cadenaPartida[POS_NOMBRE]);
        //            //celda.CellValue = valorCelda;                    
        //            string valorCelda22 = cadenaPartida[POS_NOMBRE];
        //            AddCellToRow(row, valorCelda22, (uint)indiceCadena + 1);
        //            indiceCadena++;
        //        }
        //        if (aC[2])
        //        {
        //            excel2.Cell celda = new excel2.Cell();
        //            //valorCelda = new excel2.CellValue(cadenaPartida[POS_NOMBRE]);
        //            //celda.CellValue = valorCelda;                    
        //            string valorCelda22 = cadenaPartida[POS_APELLIDO];
        //            AddCellToRow(row, valorCelda22, (uint)indiceCadena + 1);
        //            indiceCadena++;
        //        }

        //        sheetData1.Append(row);
        //    }
        //    catch (IndexOutOfRangeException io)
        //    {
        //        //throw new RejillaException("Fichero dañado..........");
        //        throw new RejillaException(rm.GetString("error24") + io.Message);
        //    }
        //    catch (FormatException fe)
        //    {
        //        //throw new RejillaException("Fichero dañado..........");
        //        throw new RejillaException(rm.GetString("error24") + fe.Message);
        //    }
        //}


        /*
         * Metodo que obtienes los porcentajes a partir de los aciertos,
         * errores y la cantidad de casillas correctas a tachar
         */
        private float[] obtenerPorcentaje(float uno, float dos, float tres)
        {
            // uno ==> aciertos
            // dos ==> fallos
            // tres ==> cantidad Casillas
            float[] vF = new float[9];
            for (int i = 0; i < vF.Length; i++)
                vF[i] = 0.0f;

            vF[0] = uno;
            vF[1] = dos;
            vF[2] = tres;
            if (tres != 0 && (uno + dos != 0))
            {
                vF[3] = (uno / tres);
                vF[4] = (uno / (uno + dos));
                vF[5] = (dos / (uno + dos));
                vF[6] = (dos / tres);
                vF[7] = ((uno - dos) / tres);
                vF[8] = (uno - dos) / (uno + dos);
            }
            return vF;
        }

        private bool cumpleCondicion(ExportarPor ep, string s1, string s2,
            DateTime f1, DateTime f2, string d, string position, TipoRejilla tr,
            string p, string paise, string edad1, string edad2, Genero sexo)
        {
            bool cumple = false;
            DateTime fechita;
            String aux1;
            String[] partirCadena;
            String[] fecha;

            char[] separChar = new char[4];
            separChar[0] = Convert.ToChar(7);
            separChar[1] = Convert.ToChar(8);
            separChar[2] = '\r';
            separChar[3] = '\n';

            int POSICION_FECHA = 0;
            int POSICION_NOMBRE = 1;
            int POSICION_APELLIDO = 2;
            int POSICION_DEPORTE = 3;
            int POSICION_POSICION = 4;
            int POSICION_EDAD = 5;
            int POSICION_SEXO = 6;
            int POSICION_PAIS = 7;
            int POSICON_TIPOREJILLA = 8;

            try
            {
                partirCadena = p.Split(separChar, StringSplitOptions.RemoveEmptyEntries);
                switch (ep)
                {
                    case ExportarPor.NOMBRE:
                        aux1 = partirCadena[POSICION_NOMBRE];
                        aux1 = aux1.ToUpper();
                        if (aux1.CompareTo(s1) >= 0)
                        {
                            cumple = true;
                        }
                        if (cumple && (aux1.CompareTo(s2) <= 0))
                            cumple = true;
                        else
                            cumple = false;
                        break;
                    case ExportarPor.APELLIDO:
                        aux1 = partirCadena[POSICION_APELLIDO];
                        aux1 = aux1.ToUpper();
                        if (aux1.CompareTo(s1) >= 0)
                        {
                            cumple = true;
                        }
                        if (cumple && (aux1.CompareTo(s2) <= 0))
                            cumple = true;
                        else
                            cumple = false;
                        break;
                    case ExportarPor.TIPO:
                        if (returnTipoRejilla(partirCadena[POSICON_TIPOREJILLA]) == tr)
                            cumple = true;
                        break;
                    case ExportarPor.FECHA:
                        aux1 = partirCadena[POSICION_FECHA];
                        fecha = aux1.Split(new char[] { '/', ':', ' ' });
                        fechita = new DateTime(Convert.ToInt32(fecha[2]), Convert.ToInt32(fecha[1]), Convert.ToInt32(fecha[0]));
                        if (fechita >= f1 && fechita <= f2)
                            cumple = true;
                        break;
                    case ExportarPor.DEPORTE_POSICION:
                        aux1 = partirCadena[POSICION_DEPORTE];
                        aux1 = aux1.ToUpper();
                        if (aux1.CompareTo(d) == 0)
                        {
                            cumple = true;
                        }
                        aux1 = partirCadena[POSICION_POSICION];
                        aux1 = aux1.ToUpper();
                        if (cumple && (aux1.CompareTo(position) == 0))
                            cumple = true;
                        else
                            cumple = false;
                        break;
                    case ExportarPor.DEPORTE:
                        aux1 = partirCadena[POSICION_DEPORTE];
                        aux1 = aux1.ToUpper();
                        if (aux1.CompareTo(d) == 0)
                            cumple = true;
                        break;
                    case ExportarPor.POSICION:
                        aux1 = partirCadena[POSICION_POSICION];
                        aux1 = aux1.ToUpper();
                        if (aux1.CompareTo(position) == 0)
                            cumple = true;
                        break;
                    case ExportarPor.PAIS:
                        aux1 = partirCadena[POSICION_PAIS];
                        aux1 = aux1.ToUpper();
                        if (aux1.CompareTo(paise) == 0)
                            cumple = true;
                        break;
                    case ExportarPor.EDAD:
                        aux1 = partirCadena[POSICION_EDAD];
                        if (aux1.CompareTo(edad1) >= 0)
                        {
                            cumple = true;
                        }
                        if (cumple && (aux1.CompareTo(edad2) <= 0))
                            cumple = true;
                        else
                            cumple = false;
                        break;
                    case ExportarPor.SEXO:
                        aux1 = partirCadena[POSICION_SEXO];
                        aux1 = aux1.ToUpper();
                        if (aux1.CompareTo(sexo) == 0)
                            cumple = true;
                        break;
                }
                return cumple;
            }
            catch (FormatException fe)
            {
                //MessageBox.Show("Fichero dañado.....");
                MessageBox.Show(rm.GetString("error24"));
            }
            catch (Exception)
            {
                //MessageBox.Show("Fichero dañado....");
                MessageBox.Show(rm.GetString("error24"));
            }
            return cumple;
        }

        /*
         * Método para rellenar la lista de encabezamiento de la hoja 1 de Excell
         */
        private void rellenarListaBooleana(SortedList<int, string> lista, bool[] vectorCampos)
        {
            lista.Clear();
            bool f = vectorCampos[0];
            int indice = 0;

            if (f)
            {
                lista.Add(++indice, rm.GetString("lista1"));
                lista.Add(++indice, rm.GetString("lista2"));
                lista.Add(++indice, rm.GetString("lista30"));
                lista.Add(++indice, rm.GetString("lista32"));
                lista.Add(++indice, rm.GetString("lista31"));
                lista.Add(++indice, rm.GetString("lista33"));//lateralidad
                lista.Add(++indice, rm.GetString("lista3"));
                lista.Add(++indice, rm.GetString("lista4"));
                lista.Add(++indice, rm.GetString("lista5"));
                lista.Add(++indice, rm.GetString("lista6"));
                lista.Add(++indice, rm.GetString("lista7"));
                lista.Add(++indice, rm.GetString("lista8"));
                lista.Add(++indice, rm.GetString("lista9"));
                lista.Add(++indice, rm.GetString("lista10"));
                lista.Add(++indice, rm.GetString("lista11"));
                //lista.Add(++indice, rm.GetString("lista12"));era el color
                lista.Add(++indice, rm.GetString("lista13"));
                lista.Add(++indice, rm.GetString("lista14"));
                lista.Add(++indice, rm.GetString("lista15"));
                lista.Add(++indice, rm.GetString("lista16"));
                lista.Add(++indice, rm.GetString("lista17"));
                lista.Add(++indice, rm.GetString("lista18"));
                lista.Add(++indice, rm.GetString("lista19"));
                lista.Add(++indice, rm.GetString("lista20"));
                lista.Add(++indice, rm.GetString("lista21"));
                lista.Add(++indice, rm.GetString("lista22"));
                lista.Add(++indice, rm.GetString("lista23"));
                lista.Add(++indice, rm.GetString("lista24"));
                lista.Add(++indice, rm.GetString("lista25"));
                lista.Add(++indice, rm.GetString("lista26"));
                lista.Add(++indice, rm.GetString("lista27"));
                lista.Add(++indice, rm.GetString("lista28"));
                lista.Add(++indice, rm.GetString("lista43"));//Los datos de emparejar
                lista.Add(++indice, rm.GetString("lista44"));
                lista.Add(++indice, rm.GetString("lista45"));
                lista.Add(++indice, rm.GetString("lista46"));
                lista.Add(++indice, rm.GetString("lista17"));//Aciertos para el botón %
                lista.Add(++indice, rm.GetString("lista18"));//Errores para el botón %
                lista.Add(++indice, rm.GetString("lista41"));//Cantidad de botones para %
                lista.Add(++indice, rm.GetString("lista34"));//Los percentajes
                lista.Add(++indice, rm.GetString("lista35"));
                lista.Add(++indice, rm.GetString("lista36"));
                lista.Add(++indice, rm.GetString("lista37"));
                lista.Add(++indice, rm.GetString("lista38"));//Eficacia
                lista.Add(++indice, rm.GetString("lista39"));//Efectividad
                for (int i = 1; i < 15 * 15; i++)
                    //lista.Add(i + vectorCampos.Length + 10, rm.GetString("lista29")/*"Boton"*/ + i);
                    lista.Add(i + indice, rm.GetString("lista29")/*"Boton"*/ + i);

            }
            else
            {
                if (vectorCampos[1]) //Nombre
                    lista.Add(++indice, rm.GetString("lista1"));
                if (vectorCampos[2]) //Apellidos
                    lista.Add(++indice, rm.GetString("lista2"));
                if (vectorCampos[3])//Fecha
                    lista.Add(++indice, rm.GetString("lista5"));//Fecha
                if (vectorCampos[4]) //Posicion
                    lista.Add(++indice, rm.GetString("lista6"));
                if (vectorCampos[5])//Fecha
                    lista.Add(++indice, rm.GetString("lista7"));
                if (vectorCampos[6])//Observaciones
                    lista.Add(++indice, rm.GetString("lista8"));
                if (vectorCampos[7])//TipoRejilla
                    lista.Add(++indice, rm.GetString("lista9"));
                if (vectorCampos[8])//Tipo Botón
                    lista.Add(++indice, rm.GetString("lista10"));
                if (vectorCampos[9])//Distracción Línea
                    lista.Add(++indice, rm.GetString("lista11"));
                //if (vectorCampos[10])//Color
                //    lista.Add(++indice, rm.GetString("lista12"));


                //edad, sexo y pais
                if (vectorCampos[20])
                    lista.Add(++indice, rm.GetString("lista30"));//Edad
                if (vectorCampos[21])
                    lista.Add(++indice, rm.GetString("lista32"));//Sexo
                if (vectorCampos[22])
                    lista.Add(++indice, rm.GetString("lista31"));//Pais
                //lateralidad
                if (vectorCampos[10])
                    lista.Add(++indice, rm.GetString("lista33"));//lateralidad

                if (vectorCampos[11])//Grosor
                    lista.Add(++indice, rm.GetString("lista13"));
                if (vectorCampos[12])//Ascendente-descendente
                    lista.Add(++indice, rm.GetString("lista14"));
                if (vectorCampos[13])//Música
                    lista.Add(++indice, rm.GetString("lista15"));
                if (vectorCampos[14])//Metrónomo
                    lista.Add(++indice, rm.GetString("lista16"));

                if (vectorCampos[15]) //Tiempos de la gráfica
                {
                    lista.Add(++indice, rm.GetString("lista19"));
                    lista.Add(++indice, rm.GetString("lista20"));
                    lista.Add(++indice, rm.GetString("lista21"));
                    lista.Add(++indice, rm.GetString("lista22"));
                    lista.Add(++indice, rm.GetString("lista23"));
                    lista.Add(++indice, rm.GetString("lista24"));
                    lista.Add(++indice, rm.GetString("lista25"));
                    lista.Add(++indice, rm.GetString("lista26"));
                    lista.Add(++indice, rm.GetString("lista27"));
                    lista.Add(++indice, rm.GetString("lista28"));
                }


                if (vectorCampos[17])
                {
                    lista.Add(++indice, rm.GetString("lista17"));//Aciertos
                    lista.Add(++indice, rm.GetString("lista18"));//Errores
                }

                if (vectorCampos[18])
                    lista.Add(++indice, rm.GetString("lista3"));//Deporte

                if (vectorCampos[19])
                    lista.Add(++indice, rm.GetString("lista4"));//Tiempo Total, no hay casilla para seleccionar solo el tiempo TOTAL

                if (vectorCampos[25])//Los datos de emparejar
                {
                    lista.Add(++indice, rm.GetString("lista43"));//Los datos de emparejar
                    lista.Add(++indice, rm.GetString("lista44"));
                    lista.Add(++indice, rm.GetString("lista45"));
                    lista.Add(++indice, rm.GetString("lista46"));
                }

                if (vectorCampos[26])//Los datos de los %
                {
                    lista.Add(++indice, rm.GetString("lista17"));//Aciertos para el botón %
                    lista.Add(++indice, rm.GetString("lista18"));//Errores para el botón %
                    lista.Add(++indice, rm.GetString("lista41"));//Cantidad de botones para %
                    lista.Add(++indice, rm.GetString("lista34"));//Los percentajes
                    lista.Add(++indice, rm.GetString("lista35"));
                    lista.Add(++indice, rm.GetString("lista36"));
                    lista.Add(++indice, rm.GetString("lista37"));
                    lista.Add(++indice, rm.GetString("lista38"));//Eficacia
                    lista.Add(++indice, rm.GetString("lista39"));//Efectividad
                }
                if (vectorCampos[16])
                {
                    for (int i = 0; i < 15 * 15; i++)
                        //lista.Add(i + vectorCampos.Length+10, rm.GetString("lista29")/*"Boton"*/ + i);
                        lista.Add(++indice, rm.GetString("lista29")/*"Boton"*/ + i);
                }
            }
        }


        private void rellenarHojaDos(SortedList<int, string> listaHoja2, bool[] f)
        {
            int indice = 0;
            if (f[0])
            {
                listaHoja2.Add(++indice, rm.GetString("lista43"));//"EMPAREJIAMIENTO");
                listaHoja2.Add(++indice, rm.GetString("lista44"));//"TIEMPO");
                listaHoja2.Add(++indice, rm.GetString("lista45"));//"POSICION");
                listaHoja2.Add(++indice, rm.GetString("lista46"));//"SECUENcia");
                listaHoja2.Add(++indice, rm.GetString("lista17"));//"ACIERTOS");
                listaHoja2.Add(++indice, rm.GetString("lista18"));//errores
                listaHoja2.Add(++indice, rm.GetString("lista41"));//"CANTIDAD BOTONES");
                listaHoja2.Add(++indice, rm.GetString("lista34"));//"ACIERTOS (CANTIDAD BOTONES)");
                listaHoja2.Add(++indice, rm.GetString("lista35"));//"ACIERTOS (ERRORES)");
                listaHoja2.Add(++indice, rm.GetString("lista36"));//"ERRORES (ACIERTOS)");
                listaHoja2.Add(++indice, rm.GetString("lista37"));//"ERRORES (CANTIDAD BOTONES)");
                listaHoja2.Add(++indice, rm.GetString("lista38"));//"EFICACIA");
                listaHoja2.Add(++indice, rm.GetString("lista39"));//"EFECTIVIDAD");
                for (int i = 0; i < (15 * 15); i++)
                    listaHoja2.Add(++indice, rm.GetString("lista40") + i);

            }
            else
            {
                if (f[23])//emparejamiento
                {
                    listaHoja2.Add(++indice, rm.GetString("lista43"));//"EMPAREJIAMIENTO");
                    listaHoja2.Add(++indice, rm.GetString("lista44"));//"TIEMPO");
                    listaHoja2.Add(++indice, rm.GetString("lista45"));//"POSICION");
                    listaHoja2.Add(++indice, rm.GetString("lista46"));//"SECUENcia");
                }
                if (f[24])//los porcentajes
                {
                    listaHoja2.Add(++indice, rm.GetString("lista17"));//"ACIERTOS");
                    listaHoja2.Add(++indice, rm.GetString("lista18"));//errores
                    listaHoja2.Add(++indice, rm.GetString("lista41"));//"CANTIDAD BOTONES");
                    listaHoja2.Add(++indice, rm.GetString("lista34"));//"ACIERTOS (CANTIDAD BOTONES)");
                    listaHoja2.Add(++indice, rm.GetString("lista35"));//"ACIERTOS (ERRORES)");
                    listaHoja2.Add(++indice, rm.GetString("lista36"));//"ERRORES (ACIERTOS)");
                    listaHoja2.Add(++indice, rm.GetString("lista37"));//"ERRORES (CANTIDAD BOTONES)");
                    listaHoja2.Add(++indice, rm.GetString("lista38"));//"EFICACIA");
                    listaHoja2.Add(++indice, rm.GetString("lista39"));//"EFECTIVIDAD");

                }
                if (f[16])//los tiempos de latencia o reaccion
                {
                    for (int i = 0; i < (15 * 15); i++)
                        listaHoja2.Add(++indice, rm.GetString("lista40") + i);
                }
            }
        }


        private void cerrarLibro(Excel.Workbook wb, ref bool p2)
        {
            libro = null;
            apExcel.Quit();
            apExcel = null;
        }

        /*
         * ***************************************************************
         * BARRA DE MENU. ACERCA DE.
         * ***************************************************************
         */
        private void acercaDeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AcercaDe ad = new AcercaDe();
            ad.ShowDialog();
        }

        /*
         * ***************************************************************
         * BOTON COLORES DE FONDO.
         * ***************************************************************
         */
        private void btColoresFondo_Click(object sender, EventArgs e)
        {
            DialogoColoresPaLetras dcpa = new DialogoColoresPaLetras(this.plantillaColoresRejilla, true, this.rutaCarpeta);
            DialogResult dr;
            dr = dcpa.ShowDialog();
            if (dr == DialogResult.OK)
            {
                this.plantillaColoresRejilla = dcpa.colores;
                if (dcpa.obtenerColoresEscogidos().Count != 0)
                    this.listaColoresElegidosParaElFondo = dcpa.obtenerColoresEscogidos();
            }
        }

        /*
         * ***************************************************************
         * BOTÓN OBSERVACIONES.
         * ***************************************************************
         */
        private void btObservaciones_Click(object sender, EventArgs e)
        {
            Observaciones obs;
            if (this.observacionesBD == false)
            {
                obs = new Observaciones(observaciones, this.fichero, this.tbNombre.Text, this.tbApellidos.Text, this.tbFecha.Text, this.guardarToolStripMenuItem.Enabled);
                if (obs.ShowDialog() == DialogResult.OK)
                    observaciones = obs.textoObservacion;
            }
            else if (this.observacionesBD == true)
            {
                obs = new Observaciones(this.observaciones, this.codigoObservaciones);
                if (obs.ShowDialog() == DialogResult.OK)
                    observaciones = obs.textoObservacion;
            }

        }

        /*
         * ***************************************************************
         * BOTON PROGRAMAR.
         * ***************************************************************
         */

        private void btProgramarColores_Click(object sender, EventArgs e)
        {
            ProgramarColores pc = new ProgramarColores(this.rejillaTipo, this.vectorProgramarColores,
                this.tacharColor, this.tacharTexto, this.tacharNada, this.tacharTodo);
            if (pc.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    this.vectorProgramarColores[0] = pc.interTexto;
                    this.vectorProgramarColores[1] = pc.interColor;
                    this.vectorProgramarColores[2] = pc.interTodo;

                    this.tacharColor = pc.tacharColor;
                    this.tacharTexto = pc.tacharTexto;
                    this.tacharNada = pc.tacharNada;
                    this.tacharTodo = pc.tacharTodo;

                }
                catch (IndexOutOfRangeException eo)
                {
                    //MessageBox.Show("Error inesperado. Se cerrará la aplicación.");
                    MessageBox.Show(rm.GetString("error00"));
                    this.Close();
                }
            }
        }

        /*
         * ***************************************************************
         * BOTON CONTROL TACHADO.
         * ***************************************************************
         */

        private void btControlTachado_Click(object sender, EventArgs e)
        {
            ControlTachado ct = new ControlTachado(this.fuenteFamilia, this.listaDatosControlTachado, this.ordenPulsacion, this.guardarTiemposGrafica, this.rbDescendente.Checked, this.rejillaTipo, this.aciertos, this.errores);
            ct.ShowDialog();
        }

        /*
         * ***************************************************************
         * BOTON DE IDIOMAS
         * ***************************************************************
         */
        private void btUk_Click(object sender, EventArgs e)
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            this.Controls.Clear();
            this.InitializeComponent();
            comboBoxTamañoBoton.SelectedIndex = 0;
            comboIncrementoBotones.SelectedIndex = 0;
            cBGrosor.SelectedIndex = 0;
            this.inicializarFormulario();
        }

        private void btSp_Click(object sender, EventArgs e)
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("es-ES");
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("es-ES");
            this.Controls.Clear();
            this.InitializeComponent();
            comboBoxTamañoBoton.SelectedIndex = 0;
            comboIncrementoBotones.SelectedIndex = 0;
            cBGrosor.SelectedIndex = 0;
            this.inicializarFormulario();
        }

        private void inicializarFormulario()
        {
            lienzoinicial = false;
            incrementoNext = 1;  //por defecto los numeros iran de 1 en 1
            tamañoBoton = 40;    //por defecto el tamaño del boton es el pequeño;
            linea = tipoLinea.SINLINEA; //por defecto sin linea
            radioButonSeleccionado = false;
            color = SystemColors.Control;//Color.Empty;
            //tipoGrosores = new int[15] {6, 8, 10, 12, 14, 16, 18, 20, 22, 24, 26, 28, 30, 32, 35};
            tipoGrosores = new int[15] { 20, 30, 40, 45, 50, 55, 60, 65, 70, 75, 80, 85, 95, 150, 155 };
            grosor = tipoGrosores[0]; //el grosor por defecto es el minimo de todos 
            //velocidadLinea = 2.50005;//por defecto el valor donde esta fijao el TrackBar
            this.velocidadLinea = 125;
            //rejillaTipo = TipoRejilla.NUMERICA;
            listaBotonesColores = new List<Color>();
            this.minimoCantidadBotonesColores = TantoPorCiento.CERO;
            listaLetras = new List<string>();
            listaLetrasTachar = new List<string>();
            this.fuenteFamilia = this.Font.FontFamily;
            this.minimoLetras = TantoPorCiento.CERO;
            this.rbLetrasCero.Checked = true;
            this.listaImagenesEscogidas.Clear();

            this.listaImagenesEscogidas.Clear();

            this.imagenes = new ClaseImagen();
            this.imagenes.plantillaInicial();
            this.minimoCantidadBotonesImagenes = TantoPorCiento.CERO;
            this.listaImagenes = imagenes.obtenerTodasImagenes();

            this.tbFecha.Text = DateTime.Now.ToString();

            this.ordenTachadoAbc = true;
            this.rellenarOpcionesAbecedario(0);

            this.tiempoCorreccionEfectuada = new TimeSpan();
            this.tiempoCorregido = new TimeSpan();
            this.tiempoTareaInicial = new TimeSpan();

            this.listaCompletaObjetos.Clear();
            this.listaSeleccionados.Clear();
            this.minimoWin = TantoPorCiento.CERO;

            this.plantillaColoresRejilla = new ClaseColores();
            this.plantillaColoresRejilla.plantillaInicial();

            this.listaColoresElegidosParaElFondo.Clear();

            this.observaciones = "";

            this.vectorProgramarColores = new bool[] { true, false, false };
            this.tacharColor = false;
            this.tacharTexto = false;
            this.tacharColor = false;
            this.verBotonRbNombre = this.rbAscendente.Name;

            this.loopCanciones = false;
            this.aciertos = this.errores = 0;

            this.comienzoRejilla = "";
            this.sexo = Genero.MASCULINO;
            this.cantidadBotones = 0;
            System.GC.Collect();
        }

        /*
         * ***************************************************************
         * PREFERENCIAS. Guardar y cargar preferencias.
         * ***************************************************************
         */
        private void guardarPreferenciasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog guardarFichero = new SaveFileDialog();
            guardarFichero.InitialDirectory = this.rutaCarpeta;
            guardarFichero.Filter = rm.GetString("txt11"); //"Ficheros de preferencias *.mmj|*.mmj";
            guardarFichero.DefaultExt = "mmj";
            guardarFichero.RestoreDirectory = true;
            guardarFichero.Title = rm.GetString("txt12"); //"Guardar Ficheros de preferencias";

            if (guardarFichero.ShowDialog() == DialogResult.OK)
            {
                FileStream file = new FileStream(guardarFichero.FileName, FileMode.Create);
                StreamWriter flujoEscritura = new StreamWriter(file);
                flujoEscritura.WriteLine(this.textBoxNumColumnas.Text);
                flujoEscritura.WriteLine(this.textBoxNumFila.Text);
                flujoEscritura.WriteLine(this.rejillaTipo);
                //valor inicial, incrementos, ascendente o  descendente
                flujoEscritura.WriteLine(this.textBoxValorIncial.Text);
                flujoEscritura.WriteLine(this.rbAscendente.Checked);
                flujoEscritura.WriteLine(this.rbDescendente.Checked);
                flujoEscritura.WriteLine(this.comboIncrementoBotones.SelectedIndex);
                flujoEscritura.WriteLine(this.tbAbecedario.Text);
                flujoEscritura.WriteLine(this.rbAbcMayusculas.Checked);
                flujoEscritura.WriteLine(this.rbAbcMinusculas.Checked);

                flujoEscritura.WriteLine(this.rbAbcMAYUSminus.Checked);
                flujoEscritura.WriteLine(this.rbAbcminusMAYUS.Checked);
                flujoEscritura.WriteLine(this.rbAbcAleatorio.Checked);
                flujoEscritura.WriteLine(this.rbAscAbecedario.Checked);
                flujoEscritura.WriteLine(this.rbDescAbecedario.Checked);

                flujoEscritura.WriteLine(this.cbColoresFondo.Checked);
                //fin de guardar lo que quiere mendo
                //guardar lo de emparejar
                flujoEscritura.WriteLine(this.posicion);
                flujoEscritura.WriteLine(this.tiempoEmparejar);
                flujoEscritura.WriteLine(this.secuencia);
                flujoEscritura.WriteLine(this.cbEmparejar.Checked);
                //fin de guardar lo nuevo de emparejar
                flujoEscritura.WriteLine(this.checkBoxAleatorio.Checked);
                flujoEscritura.WriteLine(this.textBoxTiempoSegundosAleatorio.Text);
                flujoEscritura.WriteLine(this.cbTiempoLimite.Checked);
                flujoEscritura.WriteLine(this.tbTiempoLimite.Text);
                flujoEscritura.WriteLine(this.rbHorizontal.Checked);
                flujoEscritura.WriteLine(this.rbVertical.Checked);
                flujoEscritura.WriteLine(this.rbAleatorio.Checked);
                flujoEscritura.WriteLine(this.rbSinlinea.Checked);
                flujoEscritura.WriteLine(this.comboBoxTamañoBoton.SelectedIndex);
                flujoEscritura.WriteLine(this.cbTareaPreliminar.Checked);
                flujoEscritura.WriteLine(this.rbSinSonido.Checked);
                flujoEscritura.WriteLine(this.rbMetronomo.Checked);
                flujoEscritura.WriteLine(this.tbMetronomo.Text);
                flujoEscritura.WriteLine(this.deslizadorVelocidad.Value);
                flujoEscritura.WriteLine(this.vectorProgramarColores[0].ToString());
                flujoEscritura.WriteLine(this.vectorProgramarColores[1].ToString());
                flujoEscritura.WriteLine(this.vectorProgramarColores[2].ToString());
                flujoEscritura.WriteLine(this.tacharColor.ToString());
                flujoEscritura.WriteLine(this.tacharTexto.ToString());
                flujoEscritura.WriteLine(this.tacharNada.ToString());
                flujoEscritura.WriteLine(this.tacharTodo.ToString());
                if (!this.rbSinlinea.Checked)
                {
                    flujoEscritura.WriteLine(this.cBGrosor.SelectedItem);
                    flujoEscritura.WriteLine(this.pBColor.BackColor.A);
                    flujoEscritura.WriteLine(this.pBColor.BackColor.R);
                    flujoEscritura.WriteLine(this.pBColor.BackColor.G);
                    flujoEscritura.WriteLine(this.pBColor.BackColor.B);
                }
                else
                {
                    flujoEscritura.WriteLine("null");
                    flujoEscritura.WriteLine("null");
                    flujoEscritura.WriteLine("null");
                    flujoEscritura.WriteLine("null");
                    flujoEscritura.WriteLine("null");
                }
                flujoEscritura.WriteLine(rbMusica.Checked);
                if (rbMusica.Checked)
                {
                    flujoEscritura.WriteLine(this.listaMusica.Items.Count);
                    foreach (string ss in listaMusica.Items)
                        flujoEscritura.WriteLine(ss);
                }

                flujoEscritura.Close();
                file.Close();
            }
        }

        private void cargarPreferenciasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileStream file;
            StreamReader flujoLectura;
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = this.rutaCarpeta;
            ofd.Filter = rm.GetString("txt11"); //"Ficheros de preferencias *.mmj|*.mmj";
            ofd.Title = rm.GetString("txt13"); //"Abrir Ficheros de preferencias";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    file = new FileStream(ofd.FileName, FileMode.Open, FileAccess.Read);
                    flujoLectura = new StreamReader(file);
                    this.textBoxNumColumnas.Text = flujoLectura.ReadLine();
                    this.textBoxNumFila.Text = flujoLectura.ReadLine();
                    this.rejillaTipo = this.returnTipoRejilla(flujoLectura.ReadLine());
                    this.cargarTipoRejilla(this.rejillaTipo);
                    // ascendente o descendente, valor inicial e incremento
                    this.textBoxValorIncial.Text = flujoLectura.ReadLine();
                    this.rbAscendente.Checked = Convert.ToBoolean(flujoLectura.ReadLine());
                    this.rbDescendente.Checked = Convert.ToBoolean(flujoLectura.ReadLine());
                    this.comboIncrementoBotones.SelectedIndex = Convert.ToInt32(flujoLectura.ReadLine());
                    this.tbAbecedario.Text = flujoLectura.ReadLine();
                    this.rbAbcMayusculas.Checked = Convert.ToBoolean(flujoLectura.ReadLine());
                    this.rbAbcMinusculas.Checked = Convert.ToBoolean(flujoLectura.ReadLine());

                    this.rbAbcMAYUSminus.Checked = Convert.ToBoolean(flujoLectura.ReadLine());
                    this.rbAbcminusMAYUS.Checked = Convert.ToBoolean(flujoLectura.ReadLine());
                    this.rbAbcAleatorio.Checked = Convert.ToBoolean(flujoLectura.ReadLine());
                    this.rbAscAbecedario.Checked = Convert.ToBoolean(flujoLectura.ReadLine());
                    this.rbDescAbecedario.Checked = Convert.ToBoolean(flujoLectura.ReadLine());

                    this.cbColoresFondo.Checked = Convert.ToBoolean(flujoLectura.ReadLine());

                    //los tres campos, posicion, tiempo y secuencia
                    this.posicion = convertirPosicion(flujoLectura.ReadLine());
                    this.tiempoEmparejar = Convert.ToInt32(flujoLectura.ReadLine());
                    this.secuencia = convertirSecuencia(flujoLectura.ReadLine());
                    this.cbEmparejar.Checked = Convert.ToBoolean(flujoLectura.ReadLine());


                    this.checkBoxAleatorio.Checked = Convert.ToBoolean(flujoLectura.ReadLine());
                    this.textBoxTiempoSegundosAleatorio.Text = flujoLectura.ReadLine();
                    this.cbTiempoLimite.Checked = Convert.ToBoolean(flujoLectura.ReadLine());
                    this.tbTiempoLimite.Text = flujoLectura.ReadLine(); ;
                    this.rbHorizontal.Checked = Convert.ToBoolean(flujoLectura.ReadLine());
                    this.rbVertical.Checked = Convert.ToBoolean(flujoLectura.ReadLine());
                    this.rbAleatorio.Checked = Convert.ToBoolean(flujoLectura.ReadLine());
                    this.rbSinlinea.Checked = Convert.ToBoolean(flujoLectura.ReadLine());
                    this.comboBoxTamañoBoton.SelectedIndex = Convert.ToInt32(flujoLectura.ReadLine());
                    this.cbTareaPreliminar.Checked = Convert.ToBoolean(flujoLectura.ReadLine());
                    this.rbSinSonido.Checked = Convert.ToBoolean(flujoLectura.ReadLine());
                    this.rbMetronomo.Checked = Convert.ToBoolean(flujoLectura.ReadLine());
                    this.tbMetronomo.Text = flujoLectura.ReadLine();
                    this.deslizadorVelocidad.Value = Convert.ToInt16(flujoLectura.ReadLine());
                    this.vectorProgramarColores[0] = Convert.ToBoolean(flujoLectura.ReadLine());
                    this.vectorProgramarColores[1] = Convert.ToBoolean(flujoLectura.ReadLine());
                    this.vectorProgramarColores[2] = Convert.ToBoolean(flujoLectura.ReadLine());
                    this.tacharColor = Convert.ToBoolean(flujoLectura.ReadLine());
                    this.tacharTexto = Convert.ToBoolean(flujoLectura.ReadLine());
                    this.tacharNada = Convert.ToBoolean(flujoLectura.ReadLine());
                    this.tacharTodo = Convert.ToBoolean(flujoLectura.ReadLine());
                    if (!this.rbSinlinea.Checked)
                    {
                        object o = flujoLectura.ReadLine();
                        //this.cBGrosor.SelectedItem = flujoLectura.ReadLine();
                        int alpha, rojo, verde, azul;
                        alpha = Convert.ToInt32(flujoLectura.ReadLine());
                        rojo = Convert.ToInt32(flujoLectura.ReadLine());
                        verde = Convert.ToInt32(flujoLectura.ReadLine());
                        azul = Convert.ToInt32(flujoLectura.ReadLine());
                        Color c = Color.FromArgb(alpha, rojo, verde, azul);
                        //this.pBColor.BackColor = Color.FromArgb(alpha, rojo, verde, azul);
                        this.pBColor.BackColor = c;
                        this.color = c;
                        this.cBGrosor.SelectedItem = o;
                    }
                    else
                    {
                        flujoLectura.ReadLine();
                        flujoLectura.ReadLine();
                        flujoLectura.ReadLine();
                        flujoLectura.ReadLine();
                        flujoLectura.ReadLine();
                    }
                    this.tipoDeLinea(); //para activar la distracción línea
                    this.rbMusica.Checked = Convert.ToBoolean(flujoLectura.ReadLine());
                    if (this.rbMusica.Checked)
                    {
                        int cantidadCanciones = Convert.ToInt32(flujoLectura.ReadLine());

                        String cadenaActual = ""; ;
                        for (int canciones = 0; canciones < cantidadCanciones; canciones++)
                        {
                            cadenaActual = flujoLectura.ReadLine(); ;
                            this.listaMusica.Items.Add(cadenaActual);
                            this.listaMostrar.Items.Add(cadenaActual.Substring(cadenaActual.LastIndexOf(Path.DirectorySeparatorChar) + 1, (cadenaActual.LastIndexOf('.') - cadenaActual.LastIndexOf(Path.DirectorySeparatorChar) - 1)));
                        }
                    }
                    else
                    {
                        this.listaMusica.Items.Clear();
                        this.listaMostrar.Items.Clear();
                    }
                    flujoLectura.Close();
                    file.Close();
                }
                catch (FileNotFoundException fne)
                {
                    //throw new RejillaException("ABRIR PREFERENCIAS: No se encuentra el fichero \n");
                    throw new RejillaException(rm.GetString("sop04"));
                }
                catch (IOException ie)
                {
                    //throw new RejillaException("ABRIR PREFERENCIAS: Error en la lectura del fichero \n.");
                    throw new RejillaException(rm.GetString("sop05"));
                }
                catch (UnauthorizedAccessException uae)
                {
                    //throw new RejillaException("ABRIR PREFERENCIAS: No tiene permisos para abrir este fichero. \n");
                    throw new RejillaException(rm.GetString("sop06"));
                }
                catch (FormatException)
                {
                    this.inicializarFormulario();
                    MessageBox.Show(rm.GetString("error24"));
                }
                catch (ArgumentException)
                {
                    this.inicializarFormulario();
                    MessageBox.Show(rm.GetString("error24"));
                }
            }
        }

        private PosicionBotonEmparejar convertirPosicion(string s)
        {
            PosicionBotonEmparejar posi;
            if (s.ToUpper().Equals("ALEATORIA"))
                posi = PosicionBotonEmparejar.ALEATORIA;
            else if (s.ToUpper().Equals("IZQUIERDA_INFERIOR"))
            {
                posi = PosicionBotonEmparejar.IZQUIERDA_INFERIOR;
            }
            else if (s.ToUpper().Equals("IZQUIERDA_CENTRO"))
            {
                posi = PosicionBotonEmparejar.IZQUIERDA_CENTRO;
            }
            else if (s.ToUpper().Equals("IZQUIERDA_SUPERIOR"))
            {
                posi = PosicionBotonEmparejar.IZQUIERDA_SUPERIOR;
            }
            else if (s.ToUpper().Equals("DERECHA_INFERIOR"))
            {
                posi = PosicionBotonEmparejar.DERECHA_INFERIOR;
            }
            else if (s.ToUpper().Equals("DERECHA_CENTRO"))
            {
                posi = PosicionBotonEmparejar.DERECHA_CENTRO;
            }
            else if (s.ToUpper().Equals("DERECHA_SUPERIOR"))
            {
                posi = PosicionBotonEmparejar.DERECHA_SUPERIOR;
            }
            else
            {
                throw new RejillaException(rm.GetString("error24"));//"FICHJERO mal ");
            }
            return posi;
        }

        private EmparejarSecuencia convertirSecuencia(string s)
        {
            EmparejarSecuencia secu;
            if (s.ToUpper().Equals("ALEATORIA"))
                secu = EmparejarSecuencia.ALEATORIA;
            else if (s.ToUpper().Equals("SECUENCIAL"))
                secu = EmparejarSecuencia.SECUENCIAL;
            else if (s.ToUpper().Equals("INVERSA"))
                secu = EmparejarSecuencia.INVERSA;
            else
                throw new RejillaException(rm.GetString("error24"));//"fallo en secuencia emparejar");
            return secu;
        }

        /*
         * Para controlar el valor del sexo y que no se salga de rango
         */
        private void sexo_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rbHombre.Checked)
                this.sexo = Genero.MASCULINO;
            if (this.rbMujer.Checked)
                this.sexo = Genero.FEMENINO;
        }

        private void pFCRealizadoPorMiguelAngelMartinezJimenesYDirigidoPorAntonioHernandezMendoYJoseLiusPrastanaBrinconesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Informacion().ShowDialog();
        }

        /*
         * ******************************************************************
         * EMPAREJAMIENTO
         * ******************************************************************
         */

        private void cbEmparejar_CheckedChanged(object sender, EventArgs e)
        {
            this.hayParejas = cbEmparejar.Checked;
        }

        private void cbEmparejar_Click(object sender, EventArgs e)
        {
            Emparejar empa = new Emparejar(this.tiempoEmparejar, this.posicion, this.secuencia);
            if (empa.ShowDialog() == DialogResult.OK)
            {
                this.posicion = empa.posicionBoton();
                this.tiempoEmparejar = empa.temporizador();
                this.secuencia = empa.secuenciaItems();
            }
        }

        /*
         * Método que llama a pintar Gráfica cada vez que queremos que se cambie el gráfico del dibujo
         * según sea tiempo total o tiempos de latencia
         * 
         */
        private void rbGrafica_CheckedChanged(object sender, EventArgs e)
        {
            dibujarGrafica(this.lienzoinicial, this.guardarTiemposGrafica, this.rbGrafica.Checked, this.guardarTiemposEmparejamiento);
        }

        /*
         * Boton porcentajes
         */
        private void btTantoPorCiento_Click(object sender, EventArgs e)
        {
            try
            {
                Porcentajes p = new Porcentajes(this.aciertos, this.errores, this.cantidadBotones);
                p.ShowDialog();
            }
            catch (RejillaException r)
            {
                MessageBox.Show(r.Message, rm.GetString("war00"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        /*
         *  2024: Metodos para buscar usuarios y cargar rejillas al programa.
         *  Nuevo.
         */
        private void baseDatosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BaseDatos basita = new BaseDatos();
        }


        private void btBuscarUsuario_Click(object sender, EventArgs e)
        {
            Usuarios usuarios = new Usuarios();
            DialogResult dr = usuarios.ShowDialog();

            if (dr == DialogResult.OK)
            {

                this.tbNombre.Text = usuarios.obtenerNombre();
                this.tbApellidos.Text = usuarios.obtenerApellidos();
                this.tbDeporte.Text = usuarios.obtenerDeportes();
                this.tbEdad.Text = usuarios.obtenerEdad();
                this.tbPais.Text = usuarios.obtenerPais();
                this.tbPosicion.Text = usuarios.obtenerPosicion();
                this.tbCodigoUsuario.Text = usuarios.obtenerCodigo();

                if (usuarios.obtenerGenero().Equals(Genero.FEMENINO))
                    this.rbMujer.Checked = true;
                else if (usuarios.obtenerGenero().Equals(Genero.MASCULINO))
                    this.rbHombre.Checked = true;


                if (usuarios.obtenerLateralidad().Equals(Lateralidad.DERECHA))
                    this.rbDer.Checked = true;
                else if (usuarios.obtenerLateralidad().Equals(Lateralidad.IZQUIERDA))
                    this.rbIzq.Checked = true;
            }
            if (dr == DialogResult.Continue)
            {
                //aqui le digo que devuelvo todos los datos al programa y los cargo.
                //Pongo que el botón me de un .Retry.
                //MessageBox.Show("Datos importados", "INFO", MessageBoxButtons.OK);

                this.filaRejillaEscogida = usuarios.obtenerFilaRejilla();

                BaseDatos bd = new BaseDatos();
                bd.conectarBD();
                int FKUsuario = -1;
                int indiceCadena = 1;
                int FKSonido = -1;
                int FKEmparejar = -1;
                int FKLineaDistractora = -1;
                SqliteDataReader readerUsuario = null, readerSonido = null, readerEmparejar = null, readerLineaDistractora = null, readerColoresFondo = null, readerControlTachado = null;
                SqliteDataReader readerTipoRejilla = null;

                //Posiciones de la tabla Rejilla (fila)
                int POS_Codigo = 0;
                int POS_Filas = 1;
                int POS_Columnas = 2;
                int POS_Errores = 3;
                int POS_Aciertos = 4;
                int POS_Fecha = 5;
                int POS_BotonesTotal = 6;
                int POS_TamañoBoton = 7;
                int POS_Tachar = 8;
                int POS_Intercambiar = 9;
                int POS_CantidadBotones = 10;
                int POS_Observaciones = 11;
                int POS_TipoRejilla = 12;
                int POS_FKTipoRejilla = 13;   //Indice de la tabla del tipo de rejilla correspondiente.
                int POS_FKUsuarios = 14;
                int POS_FKEmparejar = 15;
                int POS_FKLinea = 16;
                int POS_FKSonido = 17;
                int POS_TiempoTranscurrido = 18;
                int POS_ValorMedia = 19;
                int POS_ValorVarianza = 20;
                int POS_TiempoCorregido = 21;
                int POS_TiempoCorrecionEfectuada = 22;
                int POS_TiempoTareaPreliminar = 23;
                int POS_TiempoValorMediaCorrecion = 24;
                int POS_TiempoValorVarianzaCorrecion = 25;
                int POS_TiempoMaximo = 26;
                int POS_TiempoMinimo = 27;
                int POS_TLimite = 28;
                int POS_TAleatorio = 29;
                int POS_Tiempos = 30;
                int POS_TiemposLatencia = 31;
                int POS_FKColorFondo = 32;
                int POS_FKControTachado = 33;
                int POS_FechaFormateada = 34;
                int POS_TickGrafica = 35;
                int POS_TickLatencia = 36;

                //Posiciones usuarios
                int POS_Usuarios_Codigo = 0;
                int POS_Usuarios_Nombre = 1;
                int POS_Usuarios_Apellidos = 2;
                int POS_Usuarios_Lateralidad = 3;
                int POS_Usuarios_Genero = 4;
                int POS_Usuarios_Deportes = 5;
                int POS_Usuarios_Edad = 6;
                int POS_Usuarios_Pais = 7;
                int POS_Usuarios_Posicion = 8;


                //Posiciones distraccion linea
                int POS_Linea_Codigo = 0;
                int POS_Linea_Color = 1;
                int POS_Linea_Grosor = 2;
                int POS_Linea_Velocidad = 3;
                int POS_Linea_TipoMovimiento = 4;


                //Posciones emparejar
                int POS_Emparejar_Codigo = 0;
                int POS_Emparejar_Tiempo = 1;
                int POS_Emparejar_Orden = 2;
                int POS_Emparejar_Posicion = 3;


                //Posiciones sonido
                int POS_Sonido_Codigo = 0;
                int POS_Sonido_Metronomo = 1;
                int POS_Sonido_Canciones = 2;

                //Posiciones Numerica
                int POS_NUM_Codigo = 0;
                int POS_NUM_ValorNicial = 1;
                int POS_NUM_Incremento = 2;
                int POS_NUM_TipoMovimiento = 3;

                //Posiciones Abecedario
                int POS_ABC_Codigo = 0;
                int POS_ABC_ValorNicial = 1;
                int POS_ABC_Tipo = 2;
                int POS_ABC_Orden = 3;


                //Posiciones Colores
                int POS_COL_Codigo = 0;
                int POS_COL_Cantidadbotones = 1;
                int POS_COL_ColoresFondo = 2;
                int POS_COL_ColoresTachar = 3;


                //Posiciones Colores Fondo
                int POS_COLFON_Codigo = 0;
                int POS_COLFON_Plantilla = 1;
                int POS_COLFON_Colores = 2;


                //Posiciones imágenes
                int POS_IMG_Codigo = 0;
                int POS_IMG_CantidadBotones = 1;
                int POS_IMG_ImagenesRejilla = 2;
                int POS_IMG_ImagenesTachar = 3;

                //Posiciones de la tabla letras
                int POS_LET_Codigo = 0;
                int POS_LET_CantidadBotones = 1;
                int POS_LET_Fuente = 2;
                int POS_LET_LetrasRejilla = 3;
                int POS_LET_LetrasTachar = 4;

                //Posiciones Windings
                int POS_WIN_Codigo = 0;
                int POS_WIN_LetrasRejilla = 1;
                int POS_WIN_LetrasTachar = 2;
                int POS_WIN_CantidadBotones = 3;


                //Posiciones Control Tachado
                int POS_Control_Codigo = 0;
                int POS_Control_Fuente = 1;
                int POS_Control_Key = 2;
                int POS_Control_Titulos = 3;
                int POS_Control_Indices = 4;
                int POS_Control_Conteo = 5;
                int POS_Control_Localizado = 6;
                int POS_Control_Tiempos = 7;
                int POS_Control_OrdenPulsado = 8;

                inicializarVariablesTemporalesGrafica();
                this.MetiendoDatos_Load(this, EventArgs.Empty);

                this.rejillaTipo = returnTipoRejilla(this.filaRejillaEscogida.Cells[POS_TipoRejilla].Value.ToString());

                //Cargamos los datos de ese usuario en el progrma.
                int codigosTablas = Convert.ToInt32(this.filaRejillaEscogida.Cells[POS_FKUsuarios].Value.ToString());
                readerUsuario = bd.obtenerDatosUsuarioPorPK(codigosTablas);


                //Datos del tamaño 
                this.textBoxNumFila.Text = this.filaRejillaEscogida.Cells[POS_Filas].Value.ToString();
                this.textBoxNumColumnas.Text = this.filaRejillaEscogida.Cells[POS_Columnas].Value.ToString();
                while (readerUsuario.Read())
                {
                    tbNombre.Text = readerUsuario["Nombre"].ToString();
                    tbApellidos.Text = readerUsuario["Apellidos"].ToString();
                    tbEdad.Text = readerUsuario["Edad"].ToString();
                    tbPais.Text = readerUsuario["Pais"].ToString();
                    this.sexo = devolverGeneroFichero(readerUsuario["Genero"].ToString());
                    marcarCampoSexo();
                    this.later = devolverLateralidadFichero(readerUsuario["Lateralidad"].ToString());
                    marcarCampoLAT();
                    this.tbDeporte.Text = readerUsuario["Deportes"].ToString();
                    this.tbPosicion.Text = readerUsuario["Posicion"].ToString();
                }
                readerUsuario.Close();
                codigosTablas = Convert.ToInt32(this.filaRejillaEscogida.Cells[POS_FKTipoRejilla].Value.ToString());
                switch (rejillaTipo)
                {
                    case TipoRejilla.NUMERICA:
                        //codigosTablas = Convert.ToInt32(this.filaRejillaEscogida.Cells[POS_FKTipoRejilla].Value.ToString());
                        if (codigosTablas == -1)
                        {
                            //No hay este tipo de rejilla
                        }
                        else
                        {
                            readerTipoRejilla = bd.obtenerNumericaFKTablaRejilla(codigosTablas);
                            while (readerTipoRejilla.Read())
                            {
                                this.textBoxValorIncial.Text = readerTipoRejilla[POS_NUM_ValorNicial].ToString();
                                this.comboIncrementoBotones.SelectedValue = Convert.ToInt32(readerTipoRejilla[POS_NUM_Incremento].ToString());
                                marcarOrdenNumerica(readerTipoRejilla[POS_NUM_TipoMovimiento].ToString());
                            }
                        }
                        this.rbNumerica.Checked = true;
                        break;
                    case TipoRejilla.IMAGENES:
                        //codigosTablas = Convert.ToInt32(this.filaRejillaEscogida.Cells[POS_FKTipoRejilla].Value.ToString());
                        if (codigosTablas == -1)
                        {

                        }
                        else
                        {
                            List<byte[]> imagenesRecuperadasBytes = null;
                            byte[] arrayBytes = null;
                            readerTipoRejilla = bd.obtenerImagenesFKTablaRejilla(codigosTablas);
                            while (readerTipoRejilla.Read())
                            {
                                //readerTipoRejilla[POS_IMG_CantidadBotones].ToString();

                                /* Valores del trackball CantidadImagenes
                                 * Devuelve un valor del 0 al 3
                                 * El 0 --> 0%, 1 --> 25%, 2 --> 50%, 3 --> 75%
                                 */

                                this.tbCantidadBotonesImagenes.Value = marcarValorTrackBallCantidadMinimaBotones(readerTipoRejilla[POS_IMG_CantidadBotones].ToString());

                                this.listaImagenesEscogidas = new List<Image>(); //Lista de las imagenes a tachar
                                this.listaImagenes = new List<Image>(); //lista con las imagenes que van a formar toda la rejilla. 
                                arrayBytes = (byte[])readerTipoRejilla[POS_IMG_ImagenesRejilla];
                                imagenesRecuperadasBytes = MessagePackSerializer.Deserialize<List<byte[]>>(arrayBytes);

                                foreach (var imgBytes in imagenesRecuperadasBytes)
                                {
                                    this.listaImagenes.Add(arrayByteToImagen(imgBytes));
                                }

                                arrayBytes = null;
                                imagenesRecuperadasBytes.Clear();
                                arrayBytes = (byte[])readerTipoRejilla[POS_IMG_ImagenesTachar];

                                imagenesRecuperadasBytes = MessagePackSerializer.Deserialize<List<byte[]>>(arrayBytes);
                                foreach (var imgBytes in imagenesRecuperadasBytes)
                                {
                                    this.listaImagenesEscogidas.Add(arrayByteToImagen(imgBytes));
                                }

                                //tabPage4
                                int indiceListaTope = 0;
                                foreach (Control ctr in this.tabPage4.Controls)
                                {
                                    if (ctr is Button)
                                    {
                                        Button bt = (Button)ctr;
                                        bt.BackgroundImage = this.listaImagenesEscogidas.ElementAt(indiceListaTope);
                                        bt.BackgroundImageLayout = ImageLayout.Stretch;
                                        indiceListaTope++;
                                        if (indiceListaTope > this.listaImagenesEscogidas.Count - 1)
                                        {
                                            break;
                                        }
                                    }
                                }
                                this.imagenes = new ClaseImagen(this.listaImagenes); //Es de la ClaseImagen y su usa para cargar las imagenes
                                this.rbImagenes.Checked = true;
                            }
                        }
                        break;
                    case TipoRejilla.COLORES:
                        //codigosTablas = Convert.ToInt32(this.filaRejillaEscogida.Cells[POS_FKTipoRejilla].Value.ToString());
                        readerTipoRejilla = bd.obtenerColoresFKTablaRejilla(codigosTablas);
                        List<Color> listaTodosColores = new List<Color>();
                        List<Color> listaParaPlantillaColores = new List<Color>();
                        while (readerTipoRejilla.Read())
                        {
                            /* Valores del trackball CantidadImagenes
                             * Devuelve un valor del 0 al 3
                             * El 0 --> 0%, 1 --> 25%, 2 --> 50%, 3 --> 75%
                             */

                            this.tbCantidadBotones.Value = marcarValorTrackBallCantidadMinimaBotones(readerTipoRejilla[POS_COL_Cantidadbotones].ToString());

                            //Procedemos con la plantilla de colores                            

                            this.plantillaColoresRejilla = null;

                            string jsonColoresRejilla = readerTipoRejilla[POS_COL_ColoresFondo].ToString();
                            string jsonColoresRejillaTachar = readerTipoRejilla[POS_COL_ColoresTachar].ToString();

                            List<ClaseColoresRGBA> listaTodosRGBA = JsonSerializer.Deserialize<List<ClaseColoresRGBA>>(jsonColoresRejilla);

                            List<ClaseColoresRGBA> ListaTodosRGBATachar = JsonSerializer.Deserialize<List<ClaseColoresRGBA>>(jsonColoresRejillaTachar);
                            //Añadimos los colores a la lista de plantilla de colores
                            foreach (var colorRGBA in listaTodosRGBA)
                            {
                                listaParaPlantillaColores.Add(Color.FromArgb(colorRGBA.A, colorRGBA.R, colorRGBA.G, colorRGBA.B));
                            }

                            foreach (var cTachar in ListaTodosRGBATachar)
                            {
                                this.listaBotonesColores.Add(Color.FromArgb(cTachar.A, cTachar.R, cTachar.G, cTachar.B));
                            }

                            this.plantillaColoresRejilla = new ClaseColores(listaParaPlantillaColores);

                            //tabPage2
                            int indiceLista = 0;
                            foreach (Control ctr in this.tabPage2.Controls)
                            {
                                if (ctr is Button)
                                {
                                    Button bt = (Button)ctr;
                                    bt.BackColor = this.listaBotonesColores.ElementAt(indiceLista);
                                    indiceLista++;
                                    if (indiceLista > this.listaBotonesColores.Count - 1)
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                        //this.plantillaColoresRejilla = new ClaseColores(listaParaPlantillaColores);
                        this.rbColores.Checked = true;
                        break;
                    case TipoRejilla.LETRAS:
                        readerTipoRejilla = bd.obtenerLetrasFKTablaRejilla(codigosTablas);
                        while (readerTipoRejilla.Read())
                        {
                            marcarValorPorcentajeLetras(readerTipoRejilla[POS_LET_CantidadBotones].ToString());
                            string letrasTodas = readerTipoRejilla[POS_LET_LetrasRejilla].ToString();
                            string letrasTachar = readerTipoRejilla[POS_LET_LetrasTachar].ToString();
                            FontFamily f = new FontFamily(readerTipoRejilla[POS_LET_Fuente].ToString());
                            this.fuenteFamilia = f;
                            this.listaLetras = null;
                            string cadenaLimpia = letrasTodas.Replace("~", "");
                            this.listaLetras = cadenaLimpia.ToCharArray().Select(c => c.ToString()).ToList();
                            cadenaLimpia = letrasTachar.Replace("~", "");
                            this.listaLetrasTachar = cadenaLimpia.ToCharArray().Select(c => c.ToString()).ToList();
                            Font ff = new Font(this.fuenteFamilia, btLetras1.Width / 2);
                            cbLetras.Font = ff;
                            cbLetras.Items.AddRange(listaLetras.ToArray());
                            int indiceLista = 0;
                            this.fuenteBD = ff;
                            foreach (Control c in tabPage3.Controls)
                            {
                                if (c is Button)
                                {
                                    Button bt = (Button)c;
                                    bt.Font = ff;
                                    bt.Text = listaLetrasTachar.ElementAt(indiceLista);
                                    indiceLista++;
                                    if (indiceLista > this.listaLetrasTachar.Count - 1)
                                    {
                                        break;
                                    }
                                }
                            }
                            //cambiarFuenteBotones(ff);
                            this.rbLetras.Checked = true;
                            this.datosCargadosBD = true;
                        }
                        rbLetras.Checked = true;
                        break;
                    case TipoRejilla.ABECEDARIO:
                        readerTipoRejilla = bd.obtenerAbecedarioFKTablaRejilla(codigosTablas);
                        while (readerTipoRejilla.Read())
                        {
                            this.tbAbecedario.Text = readerTipoRejilla[POS_ABC_ValorNicial].ToString();
                            marcarTipoLetrasAbecedario(readerTipoRejilla[POS_ABC_Tipo].ToString());
                            marcarOrdenAbecedario(readerTipoRejilla[POS_ABC_Orden].ToString());
                        }
                        this.rbAbecedario.Checked = true;
                        break;
                    case TipoRejilla.WINGDINGS:
                        readerTipoRejilla = bd.obtenerWindingsFKTablaRejilla(codigosTablas);
                        while (readerTipoRejilla.Read())
                        {
                            marcarValorPorcentajeWin(readerTipoRejilla[POS_WIN_CantidadBotones].ToString());

                            string jsonWinRejilla = readerTipoRejilla[POS_WIN_LetrasRejilla].ToString();
                            string jsonWinTachar = readerTipoRejilla[POS_WIN_LetrasTachar].ToString();

                            List<ClaseTipoLetraFuenteBD> listTodas = JsonSerializer.Deserialize<List<ClaseTipoLetraFuenteBD>>(jsonWinRejilla);
                            List<ClaseTipoLetraFuenteBD> listTachar = JsonSerializer.Deserialize<List<ClaseTipoLetraFuenteBD>>(jsonWinTachar);

                            foreach (var contenido in listTodas)
                            {
                                TipoLetraFuente tlf = new TipoLetraFuente(new Font(contenido.nombreFuente, contenido.tamaño), contenido.letra);
                                this.listaCompletaObjetos.Add(tlf);
                            }


                            foreach (var contenido in listTachar)
                            {
                                TipoLetraFuente tlf = new TipoLetraFuente(new Font(contenido.nombreFuente, contenido.tamaño), contenido.letra);
                                this.listaSeleccionados.Add(tlf);
                            }
                            this.datosWinCargadosBD = true;
                            this.rbWingdings.Checked = true;
                        }
                        break;
                    default:
                        readerTipoRejilla.Close();
                        break;
                }

                //Si tenemos marcado los colores de fondo en el programa principal
                codigosTablas = 0;
                string hayColores = this.filaRejillaEscogida.Cells[POS_FKColorFondo].Value.ToString();
                if (string.IsNullOrEmpty(hayColores))
                    codigosTablas = 0;
                else
                    codigosTablas = Convert.ToInt32(hayColores);

                if (codigosTablas != 0)//Se almacena un CERO para indicar que no hay datos extra. 
                {
                    readerColoresFondo = bd.obtenerColoresFondoFKTablaRejilla(codigosTablas);
                    List<ClaseColoresRGBA> listaColoresFondo = new List<ClaseColoresRGBA>();
                    while (readerColoresFondo.Read())
                    {
                        List<ClaseColoresRGBA> listaFondo = JsonSerializer.Deserialize<List<ClaseColoresRGBA>>(readerColoresFondo[POS_COLFON_Colores].ToString());
                        List<ClaseColoresRGBA> listaPlantilla = JsonSerializer.Deserialize<List<ClaseColoresRGBA>>(readerColoresFondo[POS_COLFON_Plantilla].ToString());
                        foreach (var c in listaFondo)
                        {
                            this.listaColoresElegidosParaElFondo.Add(Color.FromArgb(c.A, c.R, c.G, c.B));
                        }

                        foreach (var c in listaPlantilla)
                        {
                            listaBotonesColores.Add(Color.FromArgb(c.A, c.R, c.G, c.B));
                        }
                    }
                    this.plantillaColoresRejilla = new ClaseColores(this.listaBotonesColores);
                    this.cbColoresFondo.Checked = true;
                    readerColoresFondo.Close();
                }

                //TiempoLimite, tiempoAleatorio, TamañoBotón
                //filaRejillaEscogida
                this.tbTiempoLimite.Text = this.filaRejillaEscogida.Cells[POS_TLimite].Value.ToString();
                if (!string.IsNullOrEmpty(this.tbTiempoLimite.Text))
                    cbTiempoLimite.Checked = true;
                this.textBoxTiempoSegundosAleatorio.Text = this.filaRejillaEscogida.Cells[POS_TAleatorio].Value.ToString();
                if (!string.IsNullOrEmpty(this.textBoxTiempoSegundosAleatorio.Text))
                    cbTiempoLimite.Checked = true;


                this.tamañoBoton = Convert.ToInt32(this.filaRejillaEscogida.Cells[POS_TamañoBoton].Value.ToString());

                int entero = convertirIndiceTamaño(this.tamañoBoton);
                //comboBoxTamañoBoton.SelectedIndex = comboBoxTamañoBoton.Items.IndexOf(this.tamañoBoton);      //marcarValorCombobox(this.tamañoBoton)-1;
                comboBoxTamañoBoton.SelectedIndex = entero;
                //Distracción linea
                //Si rbSinLine = true, no hay linea distractora
                codigosTablas = Convert.ToInt32(this.filaRejillaEscogida.Cells[POS_FKLinea].Value.ToString());
                if (0 != codigosTablas)
                {
                    //hay linea distractora
                    readerLineaDistractora = bd.obtenerLineaDistractoraFKTablaRejilla(codigosTablas);
                    while (readerLineaDistractora.Read())
                    {
                        List<ClaseColoresRGBA> c = JsonSerializer.Deserialize<List<ClaseColoresRGBA>>(readerLineaDistractora[POS_Linea_Color].ToString());
                        this.color = Color.FromArgb(c[0].A, c[0].R, c[0].G, c[0].B);
                        this.pBColor.BackColor = this.color;
                        this.velocidadLinea = Convert.ToDouble(readerLineaDistractora[POS_Linea_Velocidad].ToString());
                        this.linea = marcarTipoLinea(readerLineaDistractora[POS_Linea_TipoMovimiento].ToString());
                        double valorTrackBar = (velocidadLinea + 2.60869565217383) / 3.60869565217391;
                        int trackBarValue = (int)Math.Round(valorTrackBar);
                        this.deslizadorVelocidad.Value = trackBarValue;
                        this.cBGrosor.SelectedIndex = Convert.ToInt32(readerLineaDistractora[POS_Linea_Grosor].ToString());
                    }
                    readerLineaDistractora.Close();
                }

                //Distraccion sonido
                codigosTablas = Convert.ToInt32(this.filaRejillaEscogida.Cells[POS_FKSonido].Value.ToString());
                if (codigosTablas != 0)
                {
                    //hay sonidos
                    readerSonido = bd.obtenerSonidosFKTablaRejilla(codigosTablas);
                    while (readerSonido.Read())
                    {
                        if (!String.IsNullOrEmpty(readerSonido[POS_Sonido_Metronomo].ToString()))
                        {
                            //hay metronomo
                            this.rbMetronomo.Checked = true;
                            this.tbMetronomo.Text = readerSonido[POS_Sonido_Metronomo].ToString();
                        }
                        else if (!String.IsNullOrEmpty(readerSonido[POS_Sonido_Canciones].ToString()))
                        {
                            //hay lista de reproducción.
                            string ficheroMusica = readerSonido[POS_Sonido_Canciones].ToString();

                            procesarFicheroM3u(ficheroMusica);
                            this.reproductorWindowsMedia.URL = ficheroMusica;
                        }
                        else
                        {
                            this.rbSinSonido.Checked = true;
                        }
                    }
                    readerSonido.Close();
                    this.reproductorWindowsMedia.Ctlcontrols.stop();
                    readerSonido.Close();
                }
                //Control Tachado
                codigosTablas = Convert.ToInt32(this.filaRejillaEscogida.Cells[POS_FKControTachado].Value.ToString());
                if (codigosTablas != 0)
                {
                    List<string> desJsonClaves = null, desJsonTitulos = null;
                    List<int> desJsonIndiceItem = null, desJsonConteoPulsaciones = null;
                    List<DateTime> desJsonListaTiempos = null;
                    List<bool> desJsonLocalizado = null;
                    readerControlTachado = bd.obtenerControlTachadoFKRejilla(codigosTablas);
                    while (readerControlTachado.Read())
                    {
                        this.ordenPulsacion = JsonSerializer.Deserialize<List<string>>(readerControlTachado[POS_Control_OrdenPulsado].ToString());
                        /*List<string>*/
                        desJsonClaves = JsonSerializer.Deserialize<List<string>>(readerControlTachado[POS_Control_Key].ToString());
                        /*List<string>*/
                        desJsonTitulos = JsonSerializer.Deserialize<List<string>>(readerControlTachado[POS_Control_Titulos].ToString());
                        /*List<int>*/
                        desJsonIndiceItem = JsonSerializer.Deserialize<List<int>>(readerControlTachado[POS_Control_Indices].ToString());
                        /*List<int>*/
                        desJsonConteoPulsaciones = JsonSerializer.Deserialize<List<int>>(readerControlTachado[POS_Control_Conteo].ToString());
                        /*List<DateTime>*/
                        desJsonListaTiempos = JsonSerializer.Deserialize<List<DateTime>>(readerControlTachado[POS_Control_Tiempos].ToString());
                        /*List<bool>*/
                        desJsonLocalizado = JsonSerializer.Deserialize<List<bool>>(readerControlTachado[POS_Control_Localizado].ToString());
                    }
                    if (this.listaDatosControlTachado != null)
                    {
                        this.listaDatosControlTachado.Clear();
                    }
                    else
                    {
                        this.listaDatosControlTachado = new SortedList<string, DatosControlTachado>();
                    }
                    for (int indice = 0; indice < desJsonClaves.Count; indice++)
                    {
                        DatosControlTachado dct = new DatosControlTachado(desJsonTitulos[indice], Convert.ToInt32(desJsonIndiceItem[indice]),
                            Convert.ToInt32(desJsonConteoPulsaciones[indice]), desJsonListaTiempos[indice], desJsonLocalizado[indice]);

                        this.listaDatosControlTachado.Add(desJsonClaves[indice], dct);
                    }
                }
                List<double> tickGraficaRejilla = null;
                List<double> tickGraficaLatencia = null;
                try
                {
                    tickGraficaRejilla = JsonSerializer.Deserialize<List<double>>(this.filaRejillaEscogida.Cells[POS_TickGrafica].Value.ToString());
                    tickGraficaLatencia = JsonSerializer.Deserialize<List<double>>(this.filaRejillaEscogida.Cells[POS_TickLatencia].Value.ToString());

                }
                catch (System.Text.Json.JsonException ejson)
                {
                    throw new RejillaException(ejson.Message);
                }

                //Recuperamos los tiempos y las latencias
                this.tiempos = JsonSerializer.Deserialize<TimeSpan[]>(this.filaRejillaEscogida.Cells[POS_Tiempos].Value.ToString());
                string cad = this.filaRejillaEscogida.Cells[POS_TiemposLatencia].Value.ToString();
                if (!string.IsNullOrEmpty(this.filaRejillaEscogida.Cells[POS_TiemposLatencia].Value.ToString()))
                {
                    this.guardarTiemposEmparejamiento = JsonSerializer.Deserialize<TimeSpan[]>(this.filaRejillaEscogida.Cells[POS_TiemposLatencia].Value.ToString());
                }
                int index = 0;
                DateTime dt = new DateTime();
                this.guardarTiemposGrafica = new DateTime[tickGraficaRejilla.Count + 1];//   this.tiempos.Length + 1];
                foreach (double de in tickGraficaRejilla)
                {
                    if (de != 0)
                    {
                        dt = new DateTime(Convert.ToInt64(de));
                        this.guardarTiemposGrafica[index] = dt;
                    }
                    else
                        this.guardarTiemposGrafica[index] = new DateTime();

                    index++;
                }

                inicializarVariablesTemporalesGrafica();
                this.btControlTachado.Enabled = true;
                this.btControlTachado.Visible = true;
                //this.MetiendoDatos_Load(this, EventArgs.Empty);
                //Los porcentajes
                //Los tiempos de la tabla
                this.aciertos = Convert.ToInt32(this.filaRejillaEscogida.Cells[POS_Aciertos].Value.ToString());
                this.errores = Convert.ToInt32(this.filaRejillaEscogida.Cells[POS_Errores].Value.ToString());
                this.cantidadBotones = Convert.ToInt32(this.filaRejillaEscogida.Cells[POS_CantidadBotones].Value.ToString());
                this.etTiempoTranscurrido.Text = this.filaRejillaEscogida.Cells[POS_TiempoTranscurrido].Value.ToString();
                this.etValorMedia.Text = this.filaRejillaEscogida.Cells[POS_ValorMedia].Value.ToString();
                this.etVarianza.Text = this.filaRejillaEscogida.Cells[POS_ValorVarianza].Value.ToString();
                this.etValorTiempoCorregido.Text = this.filaRejillaEscogida.Cells[POS_TiempoCorregido].Value.ToString();
                this.etValorTiempoCorreccionEfectuada.Text = this.filaRejillaEscogida.Cells[POS_TiempoCorrecionEfectuada].Value.ToString();
                this.etValorTiempoTareaPreliminar.Text = this.filaRejillaEscogida.Cells[POS_TiempoTareaPreliminar].Value.ToString();
                this.etValorMediaCorreccion.Text = this.filaRejillaEscogida.Cells[POS_TiempoValorMediaCorrecion].Value.ToString();
                this.etValorVarianzaCorreccion.Text = this.filaRejillaEscogida.Cells[POS_TiempoValorVarianzaCorrecion].Value.ToString();
                this.etValorMaximo.Text = this.filaRejillaEscogida.Cells[POS_TiempoMaximo].Value.ToString();
                this.etValorMinimo.Text = this.filaRejillaEscogida.Cells[POS_TiempoMinimo].Value.ToString();

                //Tachar e intercambiar y observaciones
                this.vectorProgramarColores = completarVector(devolverIntercambio(this.filaRejillaEscogida.Cells[POS_Intercambiar].Value.ToString()));
                marcarBoton(devolverTachado(this.filaRejillaEscogida.Cells[POS_Tachar].Value.ToString()));
                this.observacionesBD = true;
                this.codigoObservaciones = Convert.ToInt32(this.filaRejillaEscogida.Cells[POS_Codigo].Value.ToString());
                this.lienzoinicial = true;
                dibujarGrafica(true, this.guardarTiemposGrafica, this.rbGrafica.Checked, this.guardarTiemposEmparejamiento);
            }
        }


        private int convertirIndiceTamaño(int talla)
        {
            int tallaMarcada = 0;

            switch (talla)
            {
                case 40:
                    tallaMarcada = 1;
                    break;

                case 50:
                    tallaMarcada = 2;
                    break;

                case 60:
                    tallaMarcada = 3;
                    break;
                case 70:
                    tallaMarcada = 4;
                    break;

                case 80:
                    tallaMarcada = 5;
                    break;
                case 90:
                    tallaMarcada = 6;
                    break;
                case 150:
                    tallaMarcada = 7;
                    break;
                default:
                    tallaMarcada = 1;
                    break;
            }
            return tallaMarcada;
        }
        private void marcarBoton(TacharBoton tb)
        {
            switch (tb)
            {
                case TacharBoton.NADA:
                    this.tacharNada = true;
                    this.tacharColor = false;
                    this.tacharTexto = false;
                    this.tacharTodo = false;
                    break;
                case TacharBoton.TEXTO:
                    this.tacharNada = false;
                    this.tacharColor = false;
                    this.tacharTexto = true;
                    this.tacharTodo = false;
                    break;
                case TacharBoton.COLOR:
                    this.tacharNada = false;
                    this.tacharColor = true;
                    this.tacharTexto = false;
                    this.tacharTodo = false;
                    break;
                case TacharBoton.TEXTOYCOLOR:
                    this.tacharNada = false;
                    this.tacharColor = false;
                    this.tacharTexto = false;
                    this.tacharTodo = true;
                    break;
            }
        }

        private Tachar.TacharBoton devolverTachado(string tacho)
        {
            Tachar.TacharBoton t = TacharBoton.TEXTO;
            switch (tacho)
            {
                case "NADA":
                    t = TacharBoton.NADA;
                    break;
                case "TEXTO":
                    t = TacharBoton.TEXTO;
                    break;
                case "COLOR":
                    t = TacharBoton.COLOR;
                    break;
                case "TEXTOYCOLOR":
                    t = TacharBoton.TEXTOYCOLOR;
                    break;
            }
            return t;
        }

        private Tachar.Intercambiar devolverIntercambio(string inter)
        {
            Tachar.Intercambiar modo = Intercambiar.NADA;
            switch (inter)
            {
                case "NADA":
                case "TEXTO":
                    modo = Intercambiar.TEXTO;
                    break;
                case "COLOR":
                    modo = Intercambiar.COLOR;
                    break;
                case "TEXTOYCOLOR":
                    modo = Intercambiar.TEXTOYCOLOR;
                    break;
            }
            return modo;
        }

        private bool[] completarVector(Tachar.Intercambiar inter)
        {
            bool[] vector = null;
            switch (inter)
            {
                case Tachar.Intercambiar.NADA:
                    vector = new bool[] { true, false, false };
                    break;
                case Intercambiar.TEXTO:
                    vector = new bool[] { true, false, false };
                    break;
                case Tachar.Intercambiar.COLOR:
                    vector = new bool[] { false, true, false };
                    break;
                case Intercambiar.TEXTOYCOLOR:
                    vector = new bool[] { false, false, true };
                    break;
            }

            return vector;
        }

        private void procesarFicheroM3u(string fichero)
        {
            using (StreamReader sr = new StreamReader(fichero))
            {
                string linea;
                while ((linea = sr.ReadLine()) != null)
                {
                    if (!string.IsNullOrWhiteSpace(linea) && !linea.StartsWith("#"))
                    {
                        this.listaMusica.Items.Add(linea);
                        //valido este de abajo, sin extensión
                        //this.listaMostrar.Items.Add(file.Substring(file.LastIndexOf(Path.DirectorySeparatorChar) + 1, (file.LastIndexOf('.') - file.LastIndexOf(Path.DirectorySeparatorChar) - 1)));
                        //con extension del tipo de archivo
                        this.listaMostrar.Items.Add(linea.Substring(linea.LastIndexOf(Path.DirectorySeparatorChar) + 1));
                    }
                }
            }
        }
        private int marcarValorComboGrosorLinea(int gordo)
        {
            int valor = 0;

            switch (gordo)
            {
                case 0:
                    valor = 1;
                    break;
            }

            return valor;
        }

        private tipoLinea marcarTipoLinea(string str)
        {
            tipoLinea tipo;
            Enum.TryParse(str, out tipo);
            switch (tipo)
            {
                case tipoLinea.HORIZONTAL:
                    this.rbHorizontal.Checked = true; break;
                case tipoLinea.VERTICAL:
                    this.rbVertical.Checked = true; break;
                case tipoLinea.ALEATORIA:
                    this.rbAleatorio.Checked = true; break;
                case tipoLinea.SINLINEA:
                    this.rbSinlinea.Checked = true; break;
            }
            return tipo;
        }
        private int marcarValorCombobox(int tamaño)
        {
            int valor = Array.IndexOf(tipoGrosores, tamaño);
            return valor;
            //tipoGrosores.ElementAt(tamaño);
            //tipoGrosores[tamaño];
        }

        private int marcarValorTrackBallCantidadMinimaBotones(string str)
        {
            int devolver = -1;
            TantoPorCiento cantidad;
            Enum.TryParse(str, out cantidad);


            switch (cantidad)
            {
                case TantoPorCiento.CERO:
                    devolver = 0;
                    break;
                case TantoPorCiento.VEINTICINCO:
                    devolver = 1;
                    break;
                case TantoPorCiento.CINCUENTA:
                    devolver = 2;
                    break;
                case TantoPorCiento.SETENTAYCINCO:
                    devolver = 3;
                    break;
            }
            return devolver;
        }

        private void marcarValorPorcentajeLetras(string str)
        {
            TantoPorCiento cantidad;
            Enum.TryParse(str, out cantidad);
            switch (cantidad)
            {
                case TantoPorCiento.CERO:
                    this.rbLetrasCero.Checked = true;
                    break;
                case TantoPorCiento.VEINTICINCO:
                    this.rbLetras25.Checked = true;
                    break;
                case TantoPorCiento.CINCUENTA:
                    this.rbLetras50.Checked = true;
                    break;
                case TantoPorCiento.SETENTAYCINCO:
                    this.rbLetras75.Checked = true;
                    break;
            }
        }

        private void marcarValorPorcentajeWin(string str)
        {
            TantoPorCiento cantidad;
            Enum.TryParse(str, out cantidad);
            switch (cantidad)
            {
                case TantoPorCiento.CERO:
                    this.rbWinAleatorio.Checked = true;
                    break;
                case TantoPorCiento.VEINTICINCO:
                    this.rbWin25.Checked = true;
                    break;
                case TantoPorCiento.CINCUENTA:
                    this.rbWin50.Checked = true;
                    break;
                case TantoPorCiento.SETENTAYCINCO:
                    this.rbWin75.Checked = true;
                    break;
            }
        }

        private void marcarOrdenNumerica(string str)
        {
            if (str.Equals("ASCENDENTE"))
                this.rbAscendente.Checked = true;
            else if (str.Equals("DESCENDENTE"))
                this.rbDescendente.Checked = true;
        }

        private void marcarOrdenAbecedario(string str)
        {
            if (str.Equals("ASCENDENTE"))
                this.rbAscAbecedario.Checked = true;
            else if (str.Equals("DESCENDENTE"))
                this.rbDescAbecedario.Checked = true;
        }

        private void marcarTipoLetrasAbecedario(string str)
        {
            Tachar.Abecedario abecedario;

            Enum.TryParse(str, out abecedario);

            switch (abecedario)
            {
                case Abecedario.MINUSCULAS:
                    this.rbAbcMinusculas.Checked = true; break;
                case Abecedario.MAYUSCULAS:
                    this.rbAbcMayusculas.Checked = true; break;
                case Abecedario.ALEATORIO:
                    this.rbAbcAleatorio.Checked = true; break;
                case Abecedario.MAYUSMINUS:
                    this.rbAbcMAYUSminus.Checked = true; break;
                case Abecedario.MINUSMAYUS:
                    this.rbAbcminusMAYUS.Checked = true; break;
            }
        }


        private void crearBaseDatos()
        {
            string ruta = this.rutaCarpetaDB + "rejilla.db";
            try
            {
                if (!File.Exists(ruta))
                {
                    FileStream fs = File.Create(ruta);
                    if (fs != null)
                        fs.Close();

                    BaseDatos bd = new BaseDatos(ruta);
                    bd.conectarBD();
                    bd.crearTablas();
                    bd.inicialiarTablasIndicesUsuarios();
                    bd.cerrarConexiion();
                }
            }
            catch (Exception ex)
            {
                throw new RejillaException(rm.GetString("car23" + " ===> " + ex.Message));  // "Error creando la base de datos."
            }
        }
        private void crearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            crearBaseDatos();
            MessageBox.Show(rm.GetString("car22"), rm.GetString("car2"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            //MessageBox.Show("Base de Datos creada correctamente." , "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void borrarLosDatosDeLasTablasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BaseDatos bd = new BaseDatos();
            bd.conectarBD();
            bd.borrarTablas();
            bd.cerrarConexiion();
        }

        private void borrarFicheroBDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BaseDatos bd = new BaseDatos();
            File.Delete(bd.rutaFicheroBD() + "rejilla.db");
        }

        private void reiniciarBDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BaseDatos bd = new BaseDatos();
            bd.conectarBD();
            bd.borrarDatosTablas();
            bd.inicialiarTablasIndicesUsuarios();
        }

        private void backupBDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CarpetaLocal cl = new CarpetaLocal();
            string ficheroOrigen = new BaseDatos().rutaFicheroBD();
            string ficheroDestino = cl.nombreFicheroBackUp();
            try
            {
                using (FileStream originalFileStream = new FileStream(ficheroOrigen, FileMode.Open, FileAccess.Read))
                {
                    using (FileStream compressedFileStream = new FileStream(ficheroDestino, FileMode.Create))
                    {
                        using (GZipStream compressionStream = new GZipStream(compressedFileStream, CompressionMode.Compress))
                        {
                            originalFileStream.CopyTo(compressionStream);
                        }
                    }
                }
                MessageBox.Show(rm.GetString("car17"), rm.GetString("car2"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                //MessageBox.Show("BackUp Realizado Correctamente", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (IOException ex)
            {
                throw new RejillaException(rm.GetString("car19") + " ==> " + ex.Message); //"Error realizando la copia de seguridad de la base de datos.");
            }
        }

        private void restaurarBackUpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string ficheroElegido = "";
            string ficheroDestino = "";
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = this.rutaCarpetaBackUp;
            DialogResult dr = ofd.ShowDialog();
            if (dr == DialogResult.OK)
            {
                ficheroElegido = ofd.FileName;
                ficheroDestino = new BaseDatos().rutaFicheroBD();
                try
                {
                    using (FileStream compressedFileStream = new FileStream(ficheroElegido, FileMode.Open, FileAccess.Read))
                    {
                        using (FileStream decompressedFileStream = new FileStream(ficheroDestino, FileMode.Create))
                        {
                            using (GZipStream decompressionStream = new GZipStream(compressedFileStream, CompressionMode.Decompress))
                            {
                                decompressionStream.CopyTo(decompressedFileStream);
                            }
                        }
                    }
                    MessageBox.Show(rm.GetString("car20"), rm.GetString("car2"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //MessageBox.Show("Base de Datos Restaurada Correctamente.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (IOException io)
                {
                    throw new RejillaException(rm.GetString("car21") + " ==> " + io.Message); //"Error restaurando el fichero de BD. ");
                }
            }

        }

        private void reiniciarBDMenosUsuariosToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            BaseDatos bd = new BaseDatos();
            bd.conectarBD();
            bd.borrarDatosTablasMenosUsuarios();
            bd.inicialiarTablasIndicesUsuarios();
            MessageBox.Show(rm.GetString("car2"), rm.GetString("car25"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            //MessageBox.Show("Todos los datos de las tablas han sido borrados, excepto los usuarios.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void reiniciarBDToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            BaseDatos bd = new BaseDatos();
            bd.conectarBD();
            bd.borrarDatosTablas();
            bd.inicialiarTablasIndicesUsuarios();
            //MessageBox.Show("Todos los datos de las tablas han sido borrados.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
            MessageBox.Show(rm.GetString("car24"), rm.GetString("car2"), MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void cbNoGuardarBD_CheckedChanged(object sender, EventArgs e)
        {
            if (cbNoGuardarBD.Checked == true)
                this.guardarToolStripMenuItem.Enabled = true;
            else
                this.guardarToolStripMenuItem.Enabled = false;
        }
    }
}
