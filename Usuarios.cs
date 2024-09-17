/*
 * Miguel Ángel Martínez Jiménez
 * Versión: 2.0.
 * Fecha: Septiembre  2024.
 * Aplicación: Evaluación y Entrenamiento de la Atención.
 * 
 */
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Resources;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using excel2 = DocumentFormat.OpenXml.Spreadsheet;

namespace demorejilla
{
    [SupportedOSPlatform("windows")]
    public partial class Usuarios : Form

    {
        private BaseDatos basededatos;
        private DataTable tablaDatos;
        private DataSet conjuntoDatos;
        private DataTable tablaDatosRejillaUsuarios;
        List<int> listaRejillasSeleccionadas;
        private bool cargarDatos;
        /*
         * Variable para el idioma
         */
        private ResourceManager rm;

        //Datos de los usuarios
        private string codigo;
        private string nombre;
        private string apellidos;
        private string lateralidad;
        private string genero;
        private string deportes;
        private string edad;
        private string pais;
        private string posicion;

        //Datos de las rejillas

        private List<DataTable> listaFilasTablasRejilla;
        private SortedList<int, DataTable> listaRejillas;
        private List<DataGridViewRow> listaFilas;
        private DataGridViewRow rejillaCargarPrograma;


        //Datos para exportar
        private string nombreFicheroExcel;
        private int fila = 0;
        private int nFilas = 0, nColumnas = 0;

        private CompararRejillasExcel cre;

        private Dictionary<System.Windows.Forms.Control, Rectangle> TamañosOriginal;

        public Usuarios()
        {
            rm = new ResourceManager("demorejilla.Recursos", typeof(MetiendoDatos).Assembly);
            InitializeComponent();
            basededatos = new BaseDatos();
            tablaDatos = new DataTable();
            //conjuntoDatos = new DataSet();
            tablaDatosRejillaUsuarios = new DataTable();
            //conjuntoDatos.Tables.Add(tablaDatos);
            listaRejillasSeleccionadas = new List<int>();
            this.cargarDatos = false;
            cargarDatosDataGrid();
            this.listaRejillas = new SortedList<int, DataTable>();
            this.listaFilasTablasRejilla = new List<DataTable> { };
            this.listaFilas = new List<DataGridViewRow> { };


            //Campos fechas
            this.dateTimePickerFin.MaxDate = DateTime.Now;
            this.dateTimePickerFin.Value = DateTime.Today;
            this.dateTimePickerInicio.MaxDate = DateTime.Today;

            this.btBuscarDosCampos.Visible = false;
            this.rejillaCargarPrograma = new DataGridViewRow();
        }


        private void cargarDatosDataGrid()
        {
            basededatos.conectarBD();
            tablaDatos = basededatos.devolverTodoslosUsuarios();
            dgvUsuarios.DataSource = tablaDatos;
            basededatos.cerrarConexiion();
            TamañosOriginal = new Dictionary<System.Windows.Forms.Control, Rectangle>();
        }


        private void btCancelar_Click(object sender, EventArgs e)
        {
            btCargarUsuario.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void dgvUsuarios_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int indice = e.RowIndex;
            DataGridView rejilla = (DataGridView)sender;

            this.codigo = rejilla.Rows[indice].Cells[0].Value.ToString();
            this.nombre = rejilla.Rows[indice].Cells[1].Value.ToString();
            this.apellidos = rejilla.Rows[indice].Cells[2].Value.ToString();
            this.lateralidad = rejilla.Rows[indice].Cells[3].Value.ToString();
            this.genero = rejilla.Rows[indice].Cells[4].Value.ToString();
            this.deportes = rejilla.Rows[indice].Cells[5].Value.ToString();
            this.edad = rejilla.Rows[indice].Cells[6].Value.ToString();
            this.pais = rejilla.Rows[indice].Cells[7].Value.ToString();
            this.posicion = rejilla.Rows[indice].Cells[8].Value.ToString();

            cargarDatosArriba();
        }


        private void cargarDatosArriba()
        {
            this.tbCodigo.Text = this.codigo;
            this.tbNombre.Text = this.nombre;
            this.tbApellidos.Text = this.apellidos;
            this.tbDeportes.Text = this.deportes;
            this.tbEdad.Text = this.edad;
            this.tbPais.Text = this.pais;
            this.tbPosicion.Text = this.posicion;

            if (this.lateralidad.Equals("IZQUIERDA", StringComparison.CurrentCultureIgnoreCase))
                this.rbIzquierda.Checked = true;
            else if (this.lateralidad.Equals("DERECHA", StringComparison.InvariantCultureIgnoreCase))
                this.rbFemenino.Checked = true;


            if (this.genero.Equals("MASCULINO", StringComparison.CurrentCultureIgnoreCase))
                this.rbHombre.Checked = true;
            else if (this.genero.Equals("FEMENINO", StringComparison.CurrentCultureIgnoreCase))
                this.rbFemenino.Checked = true;

            this.cargarDatos = true;
            btCargarUsuario.DialogResult = DialogResult.OK;

        }

        private void tbBuscarPor_TextChanged(object sender, EventArgs e)
        {
            basededatos.conectarBD();
            if (rbNombre.Checked)
            {
                tablaDatos = basededatos.devolverTodoslosUsuariosNombre(tbBuscarPor.Text);
                dgvUsuarios.DataSource = tablaDatos;
            }
            else if (rbApellidos.Checked)
            {
                tablaDatos = basededatos.devolverTodoslosUsuariosApellidos(tbBuscarPor.Text);
                dgvUsuarios.DataSource = tablaDatos;
            }
            else if (rbLateralidad.Checked)
            {
                tablaDatos = basededatos.devolverTodoslosUsuariosLAteralidad(tbBuscarPor.Text);
                dgvUsuarios.DataSource = tablaDatos;
            }
            else if (rbGenero.Checked)
            {
                tablaDatos = basededatos.devolverTodoslosUsuariosGenero(tbBuscarPor.Text);
                dgvUsuarios.DataSource = tablaDatos;
            }
            else if (rbDeportes.Checked)
            {
                tablaDatos = basededatos.devolverTodoslosUsuariosDeportes(tbBuscarPor.Text);
                dgvUsuarios.DataSource = tablaDatos;
            }
            else if (rbEdad.Checked)
            {
                tablaDatos = basededatos.devolverTodoslosUsuariosEdad(tbBuscarPor.Text);
                dgvUsuarios.DataSource = tablaDatos;
            }
            else if (rbPaís.Checked)
            {
                tablaDatos = basededatos.devolverTodoslosUsuariosPais(tbBuscarPor.Text);
                dgvUsuarios.DataSource = tablaDatos;
            }
            else if (rbPosicion.Checked)
            {
                tablaDatos = basededatos.devolverTodoslosUsuariosPosicion(tbBuscarPor.Text);
                dgvUsuarios.DataSource = tablaDatos;
            }
            else if (rbTodasLasFilas.Checked)
            {
                tablaDatos = basededatos.devolverTodoslosUsuarios();
                dgvUsuarios.DataSource = tablaDatos;
            }
            basededatos.cerrarConexiion();
        }


        private void todaslasFilasDgv()
        {
            basededatos.conectarBD();
            tablaDatos = basededatos.devolverTodoslosUsuarios();
            dgvUsuarios.DataSource = tablaDatos;
            basededatos.cerrarConexiion();
        }

        private void rbTodasLasFilas_CheckedChanged(object sender, EventArgs e)
        {
            if (rbTodasLasFilas.Checked)
            {
                basededatos.conectarBD();
                tablaDatos = basededatos.devolverTodoslosUsuarios();
                dgvUsuarios.DataSource = tablaDatos;
                basededatos.cerrarConexiion();
            }
            this.tbBuscarPor.Clear();
        }


        private void btActualizar_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(tbCodigo.Text))
                //rm.GetString("error01")
                //MessageBox.Show("Para actualizar un usuario primero seleccionarlo haciendo doble click al principio de la tabla, se cargaran los datos del usuario arriba.", "información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                MessageBox.Show(rm.GetString("car1"), rm.GetString("car2"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
            {
                basededatos.conectarBD();
                int code = Convert.ToInt32(this.tbCodigo.Text);

                lateralidadYgenero();

                basededatos.actualizarUsuario(code, this.tbNombre.Text, this.tbApellidos.Text, this.lateralidad,
                    this.genero, this.tbDeportes.Text, this.tbEdad.Text, this.tbPais.Text, this.tbPosicion.Text);

                basededatos.cerrarConexiion();
                todaslasFilasDgv();
            }
        }

        private void lateralidadYgenero()
        {
            if (this.rbDerecha.Checked)
                lateralidad = Lateralidad.DERECHA.ToString();
            else if (this.rbIzquierda.Checked)
                lateralidad = Lateralidad.IZQUIERDA.ToString();


            if (this.rbHombre.Checked)
                genero = Genero.MASCULINO.ToString();
            else if (this.rbFemenino.Checked)
                genero = Genero.FEMENINO.ToString();
        }

        private void btInsertarUsuario_Click(object sender, EventArgs e)
        {
            basededatos.conectarBD();
            string codigo = basededatos.obtenerIndice("Usuarios");
            int codigoNumerico = Convert.ToInt32(codigo) + 1;

            lateralidadYgenero();

            basededatos.insertarUsuario(codigoNumerico, tbNombre.Text, tbApellidos.Text, this.lateralidad, this.genero, tbDeportes.Text,
                tbEdad.Text, tbPais.Text, tbPosicion.Text);

            basededatos.actualizarIndice("Usuarios", codigoNumerico);
            basededatos.cerrarConexiion();

            todaslasFilasDgv();
        }

        private void dgvUsuarios_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int fila = e.RowIndex;
            DataGridView rejilla = (DataGridView)sender;

            this.codigo = rejilla.Rows[fila].Cells[0].Value.ToString();
        }


        private void cargarRejillasUsuario(int indice)
        {
            basededatos.conectarBD();
            tablaDatosRejillaUsuarios = basededatos.consultarRejillaPorUsuario(indice);
            dgvRejillas.DataSource = tablaDatosRejillaUsuarios;
            DataGridViewColumn ultColumna = dgvRejillas.Columns[dgvRejillas.Columns.Count - 1];
            ultColumna.Visible = false;
            basededatos.cerrarConexiion();
        }

        private void btCargarRejillasUsuario_Click(object sender, EventArgs e)
        {
            if (this.codigo != null)
                cargarRejillasUsuario(Convert.ToInt32(this.codigo));
            else
                //MessageBox.Show("Debe seleccionar un usuario de la tabla superior", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                MessageBox.Show(rm.GetString("car1"), rm.GetString("car2"), MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void dgvRejillas_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int n = this.dgvRejillas.CurrentRow.Index;
            dgvRejillas.Rows[n].Selected = false;
            dgvRejillas.ClearSelection();

            if (listaRejillasSeleccionadas.Contains(n))
            {
                this.listaRejillasSeleccionadas.Remove(n);
            }
            else
            {
                this.listaRejillasSeleccionadas.Add(n);
            }

            this.listaFilas.Clear();
            foreach (int indice in listaRejillasSeleccionadas)
            {
                this.dgvRejillas.Rows[indice].Selected = true;
                this.listaFilas.Add(this.dgvRejillas.Rows[indice]);
            }
        }

        private void btComparar_Click(object sender, EventArgs e)
        {
            //CompararRejillas cr = new CompararRejillas(this.listaFilas);
            //cr.ShowDialog();
            cre = new CompararRejillasExcel();
            cre.compararRejillasExcel(this.listaFilas);
        }


        public string obtenerNombre()
        {
            return this.tbNombre.Text;
        }

        public string obtenerApellidos()
        {
            return this.tbApellidos.Text;
        }

        public Lateralidad obtenerLateralidad()
        {
            if (this.rbDerecha.Checked)
                return Lateralidad.DERECHA;
            else
                return Lateralidad.IZQUIERDA;
        }

        public Genero obtenerGenero()
        {
            if (this.rbHombre.Checked)
                return Genero.MASCULINO;
            else
                return Genero.FEMENINO;
        }


        public string obtenerDeportes()
        {
            return this.tbDeportes.Text;
        }


        public string obtenerEdad()
        {
            return this.tbEdad.Text;
        }

        public string obtenerPais()
        {
            return this.tbPais.Text;
        }

        public string obtenerPosicion()
        {
            return this.tbPosicion.Text;
        }

        public string obtenerCodigo()
        {
            return this.codigo;
        }

        private void btCargarUsuario_Click(object sender, EventArgs e)
        {
            if (this.cargarDatos)
            {
                btCargarUsuario.DialogResult = DialogResult.OK;
            }

            else
                MessageBox.Show("Al pulsar el botón cargar usuario, se cargan los datos del usuario en la pantalla principal, " +
                    "por lo que debe seleccionar un usuario haciendo doble click al principio de la fila de la tabla del usuario  " +
                    "y comprobar que se completan los campos superiores.", "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void btAceptar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btExportarRejillasExcel_Click(object sender, EventArgs e)
        {
            bool[] campos = new bool[1];
            //DialogResult dr = ed.ShowDialog();
            DialogResult dr = DialogResult.OK;
            try
            {
                if (dr == DialogResult.OK)

                //abro el libro de excel y lo hago todo
                {
                    try
                    {
                        // Ruta donde se guarda el ficerho excel
                        string rutalAlexcel = CarpetaMisDocumentos.ruta() + Path.DirectorySeparatorChar + "Documento_" + DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss") + ".xlsx";
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

                            //Rellenar la hoja1
                            excel2.SheetData sheetData1 = hojita1.GetFirstChild<excel2.SheetData>();
                            excel2.Row row = new excel2.Row();
                            sheetData1.AppendChild(row);
                            SortedList<int, string> listaOrdenada = new SortedList<int, string>();
                            rellenarListaBooleana(listaOrdenada, campos);//ed.arrayCampos); //ListaBooleana contiene todas las Cabeceras de la hoja 1.
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


                            int[] actualizarIndices = null;
                            actualizarIndices = cargarFilasDataTable(sheetData1, sheetData2, this.listaFilas);

                            libroHoja1.Worksheet.Save();
                            libroHoja2.Worksheet.Save();
                            libroParte.Workbook.Save();
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
                        MessageBox.Show(re.Message, rm.GetString("war00"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    //catch (COMException com)
                    //{
                    //    MessageBox.Show("Error Excel..." + com.Message);
                    //    if (this.apExcel != null)
                    //        this.apExcel.Quit();
                    //}
                    //catch (InvalidCastException coom)
                    //{
                    //    MessageBox.Show("Error Excel..." + coom.Message);
                    //    if (this.apExcel != null)
                    //        this.apExcel.Quit();
                    //}
                    //finally
                    //{
                    //    wait.Close();
                    //    if (apExcel != null)
                    //        apExcel.Visible = true;
                    //}
                }
            }
            catch (RejillaException rejillaError)
            {
                MessageBox.Show(rejillaError.Message, rm.GetString("war00"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        }


        private int[] cargarFilasDataTable(SheetData h1, SheetData h2, List<DataGridViewRow> conjuntoRejillas)
        {
            int[] filasConsumidas = new int[2];
            int filaHoja1 = 1;
            int filaHoja2 = 1;
            foreach (DataGridViewRow dt in conjuntoRejillas)
            {
                exportarHojaCalculoNueva(dt, h1, h2, filaHoja1, filaHoja2);
                filaHoja1++;
                filaHoja2++;
                filasConsumidas[0] = filaHoja1;
                filasConsumidas[1] = filaHoja2;
            }

            return filasConsumidas;
        }

        private void rellenarListaBooleana(SortedList<int, string> lista, bool[] vectorCampos)
        {
            lista.Clear();
            bool f = vectorCampos[0];
            f = true;
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
            /*
              
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
                        //lista.Add(i + vectorCampos.Length+10, rm.GetString("lista29")/*"Boton"
            +i);
            lista.Add(++indice, rm.GetString("lista29")/*"Boton" + i);
        }
    }
            */
        }


        private void rellenarHojaDos(SortedList<int, string> listaHoja2, bool[] f)
        {
            int indice = 0;
            f[0] = true;//exportamos todos los campos sin distinción.
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
            //else
            //{
            //    if (f[23])//emparejamiento
            //    {
            //        listaHoja2.Add(++indice, rm.GetString("lista43"));//"EMPAREJIAMIENTO");
            //        listaHoja2.Add(++indice, rm.GetString("lista44"));//"TIEMPO");
            //        listaHoja2.Add(++indice, rm.GetString("lista45"));//"POSICION");
            //        listaHoja2.Add(++indice, rm.GetString("lista46"));//"SECUENcia");
            //    }
            //    if (f[24])//los porcentajes
            //    {
            //        listaHoja2.Add(++indice, rm.GetString("lista17"));//"ACIERTOS");
            //        listaHoja2.Add(++indice, rm.GetString("lista18"));//errores
            //        listaHoja2.Add(++indice, rm.GetString("lista41"));//"CANTIDAD BOTONES");
            //        listaHoja2.Add(++indice, rm.GetString("lista34"));//"ACIERTOS (CANTIDAD BOTONES)");
            //        listaHoja2.Add(++indice, rm.GetString("lista35"));//"ACIERTOS (ERRORES)");
            //        listaHoja2.Add(++indice, rm.GetString("lista36"));//"ERRORES (ACIERTOS)");
            //        listaHoja2.Add(++indice, rm.GetString("lista37"));//"ERRORES (CANTIDAD BOTONES)");
            //        listaHoja2.Add(++indice, rm.GetString("lista38"));//"EFICACIA");
            //        listaHoja2.Add(++indice, rm.GetString("lista39"));//"EFECTIVIDAD");

            //    }
            //    if (f[16])//los tiempos de latencia o reaccion
            //    {
            //        for (int i = 0; i < (15 * 15); i++)
            //            listaHoja2.Add(++indice, rm.GetString("lista40") + i);
            //    }
            //}
        }


        private void AddCellToRow(excel2.Row row, string value, uint columnIndex)
        {
            excel2.Cell cell = new excel2.Cell();
            cell.CellValue = new excel2.CellValue(value);
            cell.DataType = excel2.CellValues.String;

            row.Append(cell);
        }


        private void exportarHojaCalculoNueva(DataGridViewRow filaDataTable, SheetData hojita1, SheetData hojita2, int filaHoja1, int filaHoja2)
        {
            BaseDatos bd = new BaseDatos();
            bd.conectarBD();
            int FKUsuario = -1;
            int indiceCadena = 1;
            int FKSonido = -1;
            int FKEmparejar = -1;
            int FKLineaDistractora = -1;
            SqliteDataReader readerUsuario = null, readerSonido = null, readerEmparejar = null, readerLineaDistractora = null;
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
            int POS_FKTipoRejilla = 13;
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


            DataGridViewRow unicaFilaDataTable = filaDataTable;//Fila de la tabla Rejilla a Exportar

            FKUsuario = Convert.ToInt32(unicaFilaDataTable.Cells[POS_FKUsuarios].Value);
            //FKUsuario = Convert.ToInt32(unicaFilaDataTable.Cells[13].Value);
            readerUsuario = bd.obtenerDatosUsuarioPorPK(FKUsuario);

            FKSonido = Convert.ToInt32(unicaFilaDataTable.Cells[POS_FKSonido].Value.ToString());
            FKEmparejar = Convert.ToInt32(unicaFilaDataTable.Cells[POS_FKEmparejar].Value.ToString());
            FKLineaDistractora = Convert.ToInt32(unicaFilaDataTable.Cells[POS_FKLinea].Value.ToString());
            bool sinLinea = false, sinSonido = false, sinEmparejar = false;


            
            if (FKSonido > 0)
            {
                readerSonido = bd.obtenerSonidosFKTablaRejilla(FKSonido);
            }
            else
                sinSonido = true;

            if (FKEmparejar > 0)
                readerEmparejar = bd.obtenerEmparejarFKTablaRejilla(FKEmparejar);
            else
                sinEmparejar = true;

            if (FKLineaDistractora > 0)
                readerLineaDistractora = bd.obtenerLineaDistractoraFKTablaRejilla(FKLineaDistractora);
            else
                sinLinea = true;

            int indiceHoja1 = 1;
            int indiceHoja2 = 1;



            excel2.Row row = new excel2.Row() { RowIndex = (uint)(filaHoja1 + 1) };
            string valorCelda = "";

            excel2.Row row2 = new excel2.Row() { RowIndex = (uint)(filaHoja2 + 1) };
            string valorCelda2 = "";

            //hoja1
            //valorCelda = readerUsuario["Nombre"].ToString();
            while (readerUsuario.Read())
            {
                valorCelda = readerUsuario["Nombre"].ToString();
                AddCellToRow(row, valorCelda, (uint)indiceHoja1 + 1);
                indiceHoja1++;

                valorCelda = readerUsuario["Apellidos"].ToString();
                AddCellToRow(row, valorCelda, (uint)indiceHoja1 + 1);
                indiceHoja1++;

                valorCelda = readerUsuario["Edad"].ToString();
                AddCellToRow(row, valorCelda, (uint)indiceHoja1 + 1);
                indiceHoja1++;

                valorCelda = readerUsuario["Pais"].ToString();
                AddCellToRow(row, valorCelda, (uint)indiceHoja1 + 1);
                indiceHoja1++;

                valorCelda = readerUsuario["Genero"].ToString();
                AddCellToRow(row, valorCelda, (uint)indiceHoja1 + 1);
                indiceHoja1++;

                valorCelda = readerUsuario["Lateralidad"].ToString();
                AddCellToRow(row, valorCelda, (uint)indiceHoja1 + 1);
                indiceHoja1++;

                valorCelda = readerUsuario["Deportes"].ToString();
                AddCellToRow(row, valorCelda, (uint)indiceHoja1 + 1);
                indiceHoja1++;

                valorCelda = readerUsuario["Posicion"].ToString();
                AddCellToRow(row, valorCelda, (uint)indiceHoja1 + 1);
                indiceHoja1++;
            }

            //falta la fecha que va aquí.
            valorCelda = unicaFilaDataTable.Cells[POS_Fecha].Value.ToString();
            AddCellToRow(row, valorCelda, (uint)indiceHoja1 + 1);
            indiceHoja1++;


            valorCelda = unicaFilaDataTable.Cells[POS_Observaciones].Value.ToString();
            AddCellToRow(row, valorCelda, (uint)indiceHoja1 + 1);
            indiceHoja1++;

            valorCelda = unicaFilaDataTable.Cells[POS_TipoRejilla].Value.ToString();
            AddCellToRow(row, valorCelda, (uint)indiceHoja1 + 1);
            indiceHoja1++;

            valorCelda = unicaFilaDataTable.Cells[POS_TLimite].Value.ToString();
            AddCellToRow(row, valorCelda, (uint)indiceHoja1 + 1);
            indiceHoja1++;

            valorCelda = unicaFilaDataTable.Cells[POS_TAleatorio].Value.ToString();
            AddCellToRow(row, valorCelda, (uint)indiceHoja1 + 1);
            indiceHoja1++;

            valorCelda = unicaFilaDataTable.Cells[POS_TamañoBoton].Value.ToString();
            AddCellToRow(row, valorCelda, (uint)indiceHoja1 + 1);
            indiceHoja1++;

            if (sinLinea)
            {
                valorCelda = "Sin Linea";
                AddCellToRow(row, valorCelda, (uint)indiceHoja1 + 1);
                indiceHoja1++;

                valorCelda = "Sin Grosor";
                AddCellToRow(row, valorCelda, (uint)indiceHoja1 + 1);
                indiceHoja1++;

                valorCelda = " ";
                AddCellToRow(row, valorCelda, (uint)indiceHoja1 + 1);
                indiceHoja1++;
            }
            else
            {
                valorCelda = "Con Linea";
                AddCellToRow(row, valorCelda, (uint)indiceHoja1 + 1);
                indiceHoja1++;

                while (readerLineaDistractora.Read())
                {
                    if (readerLineaDistractora.GetName(POS_Linea_Grosor) == "Grosor")
                    {
                        valorCelda = readerLineaDistractora["Grosor"].ToString();
                        AddCellToRow(row, valorCelda, (uint)indiceHoja1 + 1);
                        indiceHoja1++;
                    }
                    if (readerLineaDistractora.GetName(POS_Linea_TipoMovimiento) == "TipoMovimiento")
                    {
                        valorCelda = readerLineaDistractora["TipoMovimiento"].ToString();
                        AddCellToRow(row, valorCelda, (uint)indiceHoja1 + 1);
                        indiceHoja1++;
                    }
                }
            }

            if (sinSonido)//Solo aparece el metronomo en la exportación.
            {
                valorCelda = "";
                AddCellToRow(row, valorCelda, (uint)indiceHoja1 + 1);
                indiceHoja1++;
            }
            else
            {
                while (readerSonido.Read())
                {
                    valorCelda = readerSonido["Canciones"].ToString();
                    AddCellToRow(row, valorCelda, (uint)indiceHoja1 + 1);
                    indiceHoja1++;

                    valorCelda = readerSonido["Metronomo"].ToString();
                        AddCellToRow(row, valorCelda, (uint)indiceHoja1 + 1);
                        indiceHoja1++;
                }
            }

            valorCelda = unicaFilaDataTable.Cells[POS_Aciertos].Value.ToString();
            AddCellToRow(row, valorCelda, (uint)indiceHoja1 + 1);
            indiceHoja1++;

            valorCelda = unicaFilaDataTable.Cells[POS_Errores].Value.ToString();
            AddCellToRow(row, valorCelda, (uint)indiceHoja1 + 1);
            indiceHoja1++;

            valorCelda = unicaFilaDataTable.Cells[POS_TiempoTranscurrido].Value.ToString();
            AddCellToRow(row, valorCelda, (uint)indiceHoja1 + 1);
            indiceHoja1++;

            valorCelda = unicaFilaDataTable.Cells[POS_ValorMedia].Value.ToString();
            AddCellToRow(row, valorCelda, (uint)indiceHoja1 + 1);
            indiceHoja1++;

            valorCelda = unicaFilaDataTable.Cells[POS_ValorVarianza].Value.ToString();
            AddCellToRow(row, valorCelda, (uint)indiceHoja1 + 1);
            indiceHoja1++;

            valorCelda = unicaFilaDataTable.Cells[POS_TiempoCorregido].Value.ToString();
            AddCellToRow(row, valorCelda, (uint)indiceHoja1 + 1);
            indiceHoja1++;

            valorCelda = unicaFilaDataTable.Cells[POS_TiempoCorrecionEfectuada].Value.ToString();
            AddCellToRow(row, valorCelda, (uint)indiceHoja1 + 1);
            indiceHoja1++;

            valorCelda = unicaFilaDataTable.Cells[POS_TiempoTareaPreliminar].Value.ToString();
            AddCellToRow(row, valorCelda, (uint)indiceHoja1 + 1);
            indiceHoja1++;

            valorCelda = unicaFilaDataTable.Cells[POS_TiempoValorMediaCorrecion].Value.ToString();
            AddCellToRow(row, valorCelda, (uint)indiceHoja1 + 1);
            indiceHoja1++;

            valorCelda = unicaFilaDataTable.Cells[POS_TiempoValorVarianzaCorrecion].Value.ToString();
            AddCellToRow(row, valorCelda, (uint)indiceHoja1 + 1);
            indiceHoja1++;

            valorCelda = unicaFilaDataTable.Cells[POS_TiempoMaximo].Value.ToString();
            AddCellToRow(row, valorCelda, (uint)indiceHoja1 + 1);
            indiceHoja1++;

            valorCelda = unicaFilaDataTable.Cells[POS_TiempoMinimo].Value.ToString();
            AddCellToRow(row, valorCelda, (uint)indiceHoja1 + 1);
            indiceHoja1++;

            //Ahora vienen las columnas de emparejar Emparejar SI/NO, Tiempo, Posicion, Orden

            if (sinEmparejar)
            {
                valorCelda = "False";
                AddCellToRow(row, valorCelda, (uint)indiceHoja1 + 1);
                indiceHoja1++;

                valorCelda = "";
                AddCellToRow(row, valorCelda, (uint)indiceHoja1 + 1);
                indiceHoja1++;

                valorCelda = "";
                AddCellToRow(row, valorCelda, (uint)indiceHoja1 + 1);
                indiceHoja1++;

                valorCelda = "";
                AddCellToRow(row, valorCelda, (uint)indiceHoja1 + 1);
                indiceHoja1++;


                valorCelda2 = "False";
                AddCellToRow(row2, valorCelda2, (uint)indiceHoja2 + 1);
                indiceHoja2++;

                valorCelda2 = "";
                AddCellToRow(row, valorCelda2, (uint)indiceHoja2 + 1);
                indiceHoja2++;

                valorCelda2 = "";
                AddCellToRow(row, valorCelda2, (uint)indiceHoja2 + 1);
                indiceHoja2++;

                valorCelda2 = "";
                AddCellToRow(row, valorCelda2, (uint)indiceHoja2 + 1);
                indiceHoja2++;

            }
            else
            {
                valorCelda = "True";
                AddCellToRow(row, valorCelda, (uint)indiceHoja1 + 1);
                indiceHoja1++;

                while (readerEmparejar.Read())
                {

                    valorCelda = readerEmparejar["Tiempo"].ToString();
                    AddCellToRow(row, valorCelda, (uint)indiceHoja1 + 1);
                    indiceHoja1++;

                    valorCelda = readerEmparejar["Posicion"].ToString();
                    AddCellToRow(row, valorCelda, (uint)indiceHoja1 + 1);
                    indiceHoja1++;

                    valorCelda = readerEmparejar["Orden"].ToString();
                    AddCellToRow(row, valorCelda, (uint)indiceHoja1 + 1);
                    indiceHoja1++;

                    valorCelda2 = "True";
                    AddCellToRow(row2, valorCelda2, (uint)indiceHoja2 + 1);
                    indiceHoja2++;

                    valorCelda2 = readerEmparejar["Tiempo"].ToString();
                    AddCellToRow(row2, valorCelda2, (uint)indiceHoja2 + 1);
                    indiceHoja2++;

                    valorCelda2 = readerEmparejar["Posicion"].ToString();
                    AddCellToRow(row2, valorCelda2, (uint)indiceHoja2 + 1);
                    indiceHoja2++;

                    valorCelda2 = readerEmparejar["Orden"].ToString();
                    AddCellToRow(row2, valorCelda, (uint)indiceHoja2 + 1);
                    indiceHoja2++;
                }

            }

            valorCelda = unicaFilaDataTable.Cells[POS_Aciertos].Value.ToString();
            AddCellToRow(row, valorCelda, (uint)indiceHoja1 + 1);
            indiceHoja1++;

            valorCelda = unicaFilaDataTable.Cells[POS_Errores].Value.ToString();
            AddCellToRow(row, valorCelda, (uint)indiceHoja1 + 1);
            indiceHoja1++;

            valorCelda = unicaFilaDataTable.Cells[POS_BotonesTotal].Value.ToString();
            AddCellToRow(row, valorCelda, (uint)indiceHoja1 + 1);
            indiceHoja1++;


            //hoja2
            valorCelda2 = unicaFilaDataTable.Cells[POS_Aciertos].Value.ToString();
            AddCellToRow(row2, valorCelda2, (uint)indiceHoja2 + 1);
            indiceHoja2++;

            valorCelda2 = unicaFilaDataTable.Cells[POS_Errores].Value.ToString();
            AddCellToRow(row2, valorCelda2, (uint)indiceHoja2 + 1);
            indiceHoja2++;

            valorCelda2 = unicaFilaDataTable.Cells[POS_BotonesTotal].Value.ToString();
            AddCellToRow(row2, valorCelda2, (uint)indiceHoja2 + 1);
            indiceHoja2++;


            //Los porcetnajes
            int v1, v2, v3;
            float[] porcentajes = new float[9];
            v1 = Convert.ToInt32(unicaFilaDataTable.Cells[POS_Aciertos].Value.ToString());
            v2 = Convert.ToInt32(unicaFilaDataTable.Cells[POS_Errores].Value.ToString());
            v3 = Convert.ToInt32(unicaFilaDataTable.Cells[POS_BotonesTotal].Value.ToString());
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

            string tiemposBotonesTabla = unicaFilaDataTable.Cells[POS_Tiempos].Value.ToString();
            string guardarTiemposEmparejamiento = unicaFilaDataTable.Cells[POS_TiemposLatencia].Value.ToString();
            TimeSpan[] jsonTiempos = JsonSerializer.Deserialize<TimeSpan[]>(tiemposBotonesTabla);
            TimeSpan[] jsonLatencias = null;

            if (!string.IsNullOrEmpty(guardarTiemposEmparejamiento))
                jsonLatencias = JsonSerializer.Deserialize<TimeSpan[]>(guardarTiemposEmparejamiento);



            foreach (TimeSpan tsTiempos in jsonTiempos)
            {

                valorCelda = tsTiempos.TotalSeconds.ToString();
                AddCellToRow(row, valorCelda, (uint)indiceHoja1 + 1);
                indiceHoja1++;
            }

            if (!string.IsNullOrEmpty(guardarTiemposEmparejamiento))
            {
                foreach (TimeSpan tsLatencia in jsonLatencias)
                {
                    valorCelda2 = tsLatencia.TotalSeconds.ToString();
                    AddCellToRow(row2, valorCelda2, (uint)indiceHoja2 + 1);
                }
            }
            //Agregamos las filas a la hoja de cálculo.
            hojita1.Append(row);
            hojita2.Append(row2);
            filaHoja1++;
            filaHoja2++;

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

        private void rbTodasRejillasTodaTabla_CheckedChanged(object sender, EventArgs e)
        {
            habilitarCamposBusqueda();
            if (this.rbTodasRejillasTodaTabla.Checked)
            {
                //No se hace nada, se resetea a que aparezcan todas las rejillas de la base de datos.
            }
            else if (this.rbTodasRejillasDeporte.Checked)//filtramos las rejillas de datos por deportes
            {
                if (!String.IsNullOrEmpty(this.tbInicio.Text))
                {
                    basededatos.conectarBD();
                    tablaDatos = basededatos.devolverRejillaPorDeporte(tbInicio.Text);
                    dgvRejillas.DataSource = tablaDatos;
                    DataGridViewColumn ultColumna = dgvRejillas.Columns[dgvRejillas.Columns.Count - 1];
                    ultColumna.Visible = false;
                    basededatos.cerrarConexiion();
                }
            }
            else if (this.rbTodasRejillasFecha.Checked)
            {
                /*basededatos.conectarBD();
                tablaDatos = basededatos.devolverRejillaPorFecha(dateTimePickerInicio.Value.ToLongDateString(), dateTimePickerFin.Value.ToLongDateString());
                dgvRejillas.DataSource=tablaDatos;
                basededatos.cerrarConexiion();*/
                btBuscarDosCampos.Visible = true;
            }

        }


        private void tabUserRejillas_MouseClick(object sender, MouseEventArgs e)
        {
            if (this.tabUserRejillas.SelectedIndex == 0)//Tab usuarios
            {
                gbRejillas.Text = "Rejillas del usuario";
                this.dgvRejillas.Columns.Clear();
                this.basededatos.conectarBD();
                this.dgvUsuarios.DataSource = basededatos.devolverTodoslosUsuarios();
                basededatos.cerrarConexiion();
            }
            else if (this.tabUserRejillas.SelectedIndex == 1)//Tab Rejillas
            {
                this.dgvUsuarios.Columns.Clear();
                if (this.rbTodasRejillasTodaTabla.Checked)
                {
                    if (rbTodasLasFilas.Checked)
                    {
                        basededatos.conectarBD();
                        tablaDatos = basededatos.devolverTodasLasRejillas();
                        dgvRejillas.DataSource = tablaDatos;
                        DataGridViewColumn ultColumna = dgvRejillas.Columns[dgvRejillas.Columns.Count - 1];
                        ultColumna.Visible = false;
                        ultColumna = dgvRejillas.Columns[dgvRejillas.Columns.Count - 2];
                        ultColumna.Visible = false;
                        ultColumna = dgvRejillas.Columns[dgvRejillas.Columns.Count - 3];
                        ultColumna.Visible = false;
                        basededatos.cerrarConexiion();
                        //gbRejillas.Text = "Todas las rejillas";
                        gbRejillas.Text = rm.GetString("car5");
                    }
                    this.tbBuscarPor.Clear();
                }
            }
        }


        private void habilitarCamposBusqueda()
        {
            if (rbTodasRejillasTodaTabla.Checked)
            {
                dgvUsuarios.Columns.Clear();
                this.tbInicio.Enabled = true;
                this.tbFin.Enabled = true;
                this.dateTimePickerFin.Enabled = true;
                this.dateTimePickerInicio.Enabled = true;
                this.btBuscarDosCampos.Visible = false;
                this.tbInicio.Clear();
                this.gbTodasRejillasGenero.Enabled = false;
                this.rbTodasRejillasHombre.Checked = false;
                this.rbTodasRejillasMujer.Checked = false;
                etInicio.Text = rm.GetString("car6");  //"Inicio";
                etFin.Text = rm.GetString("car7");   //"Fin";
                tbFin.Left = etFin.Right + 10;
                tbInicio.Left = etInicio.Right + 10;
            }
            else if (rbTodasRejillasDeporte.Checked)
            {
                dgvUsuarios.Columns.Clear();
                tbInicio.Enabled = true;

                this.etInicio.Text = rm.GetString("car8"); //"Deporte";
                this.tbFin.Enabled = false;
                this.dateTimePickerFin.Enabled = false;
                this.dateTimePickerInicio.Enabled = false;
                this.btBuscarDosCampos.Visible = false;
                this.tbFin.Clear();
                etFin.Text = rm.GetString("car7");  //"Fin";
                tbFin.Left = etFin.Right + 10;
                tbInicio.Left = etInicio.Right + 10;
                this.gbTodasRejillasGenero.Enabled = false;
                this.rbTodasRejillasHombre.Checked = false;
                this.rbTodasRejillasMujer.Checked = false;
            }
            else if (rbTodasRejillasPais.Checked)
            {
                dgvUsuarios.Columns.Clear();
                this.tbInicio.Enabled = true;
                this.tbFin.Enabled = false;
                this.dateTimePickerFin.Enabled = false;
                this.dateTimePickerInicio.Enabled = false;
                this.btBuscarDosCampos.Visible = false;
                this.tbInicio.Clear();
                etInicio.Text = rm.GetString("car9"); //"País";
                etFin.Text = rm.GetString("car7"); //"Fin";
                this.tbFin.Clear();
                tbFin.Left = etFin.Right + 10;
                tbInicio.Left = etInicio.Right + 10;
                this.gbTodasRejillasGenero.Enabled = false;
                this.rbTodasRejillasHombre.Checked = false;
                this.rbTodasRejillasMujer.Checked = false;
            }
            else if (rbTodasRejillasPosicion.Checked)
            {
                dgvUsuarios.Columns.Clear();
                this.tbInicio.Enabled = true;
                this.tbFin.Enabled = false;
                this.dateTimePickerFin.Enabled = false;
                this.dateTimePickerInicio.Enabled = false;
                this.btBuscarDosCampos.Visible = false;
                this.tbInicio.Clear();
                etInicio.Text = rm.GetString("car10"); //"Posición";
                this.tbFin.Clear();
                etFin.Text = "Fin";
                tbFin.Left = etFin.Right + 10;
                tbInicio.Left = etInicio.Right + 10;
                this.gbTodasRejillasGenero.Enabled = false;
                this.rbTodasRejillasHombre.Checked = false;
                this.rbTodasRejillasMujer.Checked = false;
            }
            else if (rbTodasRejillasTipoRejilla.Checked)
            {
                dgvUsuarios.Columns.Clear();
                this.tbInicio.Enabled = true;
                this.tbFin.Enabled = false;
                this.dateTimePickerFin.Enabled = false;
                this.dateTimePickerInicio.Enabled = false;
                this.btBuscarDosCampos.Visible = false;
                this.tbInicio.Clear();
                etInicio.Text = rm.GetString("car7"); //"Tipo Rejilla";
                this.tbFin.Clear();
                etFin.Text = rm.GetString("car7"); //"Fin";
                tbFin.Left = etFin.Right + 10;
                tbInicio.Left = etInicio.Right + 10;
                this.gbTodasRejillasGenero.Enabled = false;
                this.rbTodasRejillasHombre.Checked = false;
                this.rbTodasRejillasMujer.Checked = false;
            }
            else if (rbTodasRejillasFecha.Checked)
            {
                dgvUsuarios.Columns.Clear();
                this.tbInicio.Enabled = false;
                this.tbFin.Enabled = false;
                this.dateTimePickerFin.Enabled = true;
                this.dateTimePickerInicio.Enabled = true;
                this.btBuscarDosCampos.Visible = false;
                this.tbInicio.Clear();
                this.tbFin.Clear();
                etInicio.Text = rm.GetString("car6"); //"Inicio";
                etFin.Text = rm.GetString("car7"); //"Fin";
                tbFin.Left = etFin.Right + 10;
                tbInicio.Left = etInicio.Right + 10;
                this.gbTodasRejillasGenero.Enabled = false;
                this.rbTodasRejillasHombre.Checked = false;
                this.rbTodasRejillasMujer.Checked = false;
            }
            else if (rbTodasRejillasEdad.Checked)
            {
                dgvUsuarios.Columns.Clear();
                this.tbInicio.Enabled = true;
                this.tbFin.Enabled = true;
                this.dateTimePickerFin.Enabled = false;
                this.dateTimePickerInicio.Enabled = false;
                this.btBuscarDosCampos.Visible = false;
                this.tbInicio.Clear();
                this.tbFin.Clear();
                etInicio.Text = rm.GetString("car12"); //"Desde";
                etFin.Text = rm.GetString("car13"); //"Hasta";
                tbFin.Left = etFin.Right + 10;
                tbInicio.Left = etInicio.Right + 10;
                this.gbTodasRejillasGenero.Enabled = false;
                this.rbTodasRejillasHombre.Checked = false;
                this.rbTodasRejillasMujer.Checked = false;
            }
            else if (rbTodasRejillasDP.Checked)
            {
                dgvUsuarios.Columns.Clear();
                this.tbInicio.Enabled = true;
                this.tbFin.Enabled = true;
                this.dateTimePickerFin.Enabled = false;
                this.dateTimePickerInicio.Enabled = false;
                this.btBuscarDosCampos.Visible = false;
                this.tbInicio.Clear();
                this.tbFin.Clear();
                etInicio.Text = rm.GetString("car8"); //"Deporte";
                etFin.Text = rm.GetString("car10"); //"Posición";
                this.gbTodasRejillasGenero.Enabled = false;
                tbFin.Left = etFin.Right + 10;
                tbInicio.Left = etInicio.Right + 10;
                this.gbTodasRejillasGenero.Enabled = false;
            }
            else if (rbTodasRejillasGenero.Checked)
            {
                dgvUsuarios.Columns.Clear();
                this.tbInicio.Enabled = false;
                this.tbFin.Enabled = false;
                this.dateTimePickerFin.Enabled = false;
                this.dateTimePickerInicio.Enabled = false;
                this.btBuscarDosCampos.Visible = false;
                this.tbInicio.Clear();
                this.tbFin.Clear();
                etInicio.Text = rm.GetString("car6"); //"Inicio";
                etFin.Text = rm.GetString("car7"); //"Fin";
                tbFin.Left = etFin.Right + 10;
                tbInicio.Left = etInicio.Right + 10;
                this.gbTodasRejillasGenero.Enabled = true;
                this.rbTodasRejillasHombre.Checked = false;
                this.rbTodasRejillasMujer.Checked = false;
            }
            this.basededatos.conectarBD();
            dgvRejillas.DataSource = basededatos.devolverTodasLasRejillas();
            DataGridViewColumn ultColumna = dgvRejillas.Columns[dgvRejillas.Columns.Count - 1];
            ultColumna.Visible = false;
            this.basededatos.cerrarConexiion();
        }

        private void tbInicio_TextChanged(object sender, EventArgs e)
        {
            basededatos.conectarBD();

            if (rbTodasRejillasEdad.Checked)
            {
                if (!String.IsNullOrEmpty(tbFin.Text) && !String.IsNullOrEmpty(tbInicio.Text))
                {
                    this.btBuscarDosCampos.Visible = true;
                }
            }
            else if (rbTodasRejillasDeporte.Checked)
            {
                tablaDatos = basededatos.devolverRejillaPorDeporte(this.tbInicio.Text);
                dgvRejillas.DataSource = tablaDatos;
                DataGridViewColumn ultColumna = dgvRejillas.Columns[dgvRejillas.Columns.Count - 1];
                ultColumna.Visible = false;
            }
            else if (rbTodasRejillasPosicion.Checked)
            {
                tablaDatos = basededatos.devolverRejillaPorPosicion(this.tbInicio.Text);
                dgvRejillas.DataSource = tablaDatos;
                DataGridViewColumn ultColumna = dgvRejillas.Columns[dgvRejillas.Columns.Count - 1];
                ultColumna.Visible = false;
            }
            else if (rbTodasRejillasPais.Checked)
            {
                tablaDatos = basededatos.devolverRejillaPorPais(this.tbInicio.Text);
                dgvRejillas.DataSource = tablaDatos;
                DataGridViewColumn ultColumna = dgvRejillas.Columns[dgvRejillas.Columns.Count - 1];
                ultColumna.Visible = false;
            }
            else if (rbTodasRejillasTipoRejilla.Checked)
            {
                tablaDatos = basededatos.devolverRejillaPorTipoRejilla(this.tbInicio.Text);
                dgvRejillas.DataSource = tablaDatos;
                DataGridViewColumn ultColumna = dgvRejillas.Columns[dgvRejillas.Columns.Count - 1];
                ultColumna.Visible = false;
            }
            else if (rbTodasRejillasDP.Checked)
            {
                tablaDatos = basededatos.devolverRejillaPorDeporteyPosicion(tbInicio.Text, tbFin.Text);
                dgvRejillas.DataSource = tablaDatos;
                DataGridViewColumn ultColumna = dgvRejillas.Columns[dgvRejillas.Columns.Count - 1];
                ultColumna.Visible = false;
            }
            else if (rbTodasRejillasFecha.Checked)
            {
                /*tablaDatos = basededatos.devolverRejillaPorFecha(dateTimePickerInicio.Text, dateTimePickerFin.Text);
                dgvRejillas.DataSource = tablaDatos;*/
                this.btBuscarDosCampos.Visible = true;
            }
            else if (rbTodasRejillasTodaTabla.Checked)
            {
                tablaDatos = basededatos.devolverTodasLasRejillas();
                dgvRejillas.DataSource = tablaDatos;
                DataGridViewColumn ultColumna = dgvRejillas.Columns[dgvRejillas.Columns.Count - 1];
                ultColumna.Visible = false;
            }
            basededatos.cerrarConexiion();
        }

        private void btBuscarDosCampos_Click(object sender, EventArgs e)
        {
            basededatos.conectarBD();
            if (rbTodasRejillasEdad.Checked)
            {
                tablaDatos = basededatos.devolverRejillaPorEdad(this.tbInicio.Text, this.tbFin.Text);
                dgvRejillas.DataSource = tablaDatos;
                DataGridViewColumn ultColumna = dgvRejillas.Columns[dgvRejillas.Columns.Count - 1];
                ultColumna.Visible = false;
            }
            else if (rbTodasRejillasFecha.Checked)
            {
                if (dateTimePickerInicio.Value <= dateTimePickerFin.Value)
                {

                    tablaDatos = basededatos.devolverRejillaPorFecha(dateTimePickerInicio.Value.ToString("yyyy-MM-dd"), dateTimePickerFin.Value.ToString("yyyy-MM-dd"));
                    dgvRejillas.DataSource = tablaDatos;
                    DataGridViewColumn ultColumna = dgvRejillas.Columns[dgvRejillas.Columns.Count - 1];
                    ultColumna.Visible = false;
                }
                else
                {
                    //MessageBox.Show("La fecha de inicio debe ser menor o igual a la fecha de fin", "Información", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MessageBox.Show(rm.GetString("car14"), rm.GetString("car2"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            basededatos.cerrarConexiion();
        }

        private void btCargarRejilla_Click(object sender, EventArgs e)
        {
            if (this.listaFilas.Count > 1 || this.listaFilas.Count == 0)
            {
                //MessageBox.Show("Solo se puede seleccionar una rejilla para cargar en el programa.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                MessageBox.Show(rm.GetString("car15"), rm.GetString("car2"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                this.rejillaCargarPrograma = this.listaFilas[0];
                this.DialogResult = DialogResult.Continue;
            }
        }
        public DataGridViewRow obtenerFilaRejilla()
        {
            return this.rejillaCargarPrograma;
        }
    }
}
