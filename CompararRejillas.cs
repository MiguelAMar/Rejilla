/*
 * Miguel Ángel Martínez Jiménez
 * Versión: 2.0.
 * Fecha: Septiembre  2024.
 * Aplicación: Evaluación y Entrenamiento de la Atención.
 * 
 */using DocumentFormat.OpenXml.Packaging;
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
using System.Linq;
using System.Resources;
using System.Runtime.Versioning;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using excel2 = DocumentFormat.OpenXml.Spreadsheet;

namespace demorejilla
{
    [SupportedOSPlatform("windows")]
    public partial class CompararRejillas : Form
    {
        //Datos para exportar
        private string nombreFicheroExcel;
        private int fila = 0;
        private int nFilas = 0, nColumnas = 0;
        private ResourceManager rm;

        private List<DataGridViewRow> conjuntoRejillasComparar;
        private List<DataGridViewRow> listaFilas;

        public CompararRejillas(List<DataGridViewRow> conjunto)
        {
            InitializeComponent();
            rm = new ResourceManager("demorejilla.Recursos", typeof(MetiendoDatos).Assembly);
            this.listaFilas = conjunto;
            exportarExcelDiferencias();
        }

        private void exportarExcelDiferencias()
        {
            bool[] campos = new bool[1];
            try
            {

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

                            //todosFicheros = ed.ficherosExportar;
                            //campos = ed.arrayCampos;


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
            for (int i = 0; i < conjuntoRejillas.Count; i++)
            {
                DataGridViewRow filaActual = conjuntoRejillas[i];
                DataGridViewRow filaSiguiente = (i + 1 < conjuntoRejillas.Count) ? conjuntoRejillas[i + 1] : null;
                exportarHojaCalculoNueva(filaActual, filaSiguiente, h1, h2, filaHoja1, filaHoja2);
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
                for (int i = 1; i < 15 * 15; i++)
                    //lista.Add(i + vectorCampos.Length + 10, rm.GetString("lista29")/*"Boton"*/ + i);
                    lista.Add(i + indice, rm.GetString("lista29")/*"Boton"*/ + i);
            }
        }


        private void rellenarHojaDos(SortedList<int, string> listaHoja2, bool[] f)
        {
            int indice = 0;
            f[0] = true;//exportamos todos los campos sin distinción.
            if (f[0])
            {
                for (int i = 0; i < (15 * 15); i++)
                    listaHoja2.Add(++indice, rm.GetString("lista40") + i);

            }
        }


        private void AddCellToRow(excel2.Row row, string value, uint columnIndex)
        {
            excel2.Cell cell = new excel2.Cell();
            cell.CellValue = new excel2.CellValue(value);
            cell.DataType = excel2.CellValues.String;

            row.Append(cell);
        }


        private void exportarHojaCalculoNueva(DataGridViewRow filaActual, DataGridViewRow filaSiguiente, SheetData hojita1, SheetData hojita2, int filaHoja1, int filaHoja2)
        {
            int POS_Tiempos = 30;
            int POS_TiemposLatencia = 31;
            BaseDatos bd = new BaseDatos();
            bd.conectarBD();

            string tiemposBotonesTablaActual = filaActual.Cells[POS_Tiempos].Value.ToString();
            string tiemposEmparejamientoActual = filaActual.Cells[POS_TiemposLatencia].Value.ToString();
            TimeSpan[] jsonTiemposActual = JsonSerializer.Deserialize<TimeSpan[]>(tiemposBotonesTablaActual);
            TimeSpan[] jsonLatenciasActual = null;
            bool hayTiemposLatenciaActual = !string.IsNullOrEmpty(tiemposEmparejamientoActual);
            bool hayTiemposLatenciaSiguiente = false;

            if (hayTiemposLatenciaActual)
                jsonLatenciasActual = JsonSerializer.Deserialize<TimeSpan[]>(tiemposEmparejamientoActual);

            TimeSpan[] diferenciaTiempos = null;
            TimeSpan[] diferenciasLatencias = null;

            string tiemposBotonesTablaSiguiente;
            string tiemposEmparejamientoSiguiente;
            TimeSpan[] jsonTiemposSiguiente = null;
            TimeSpan[] jsonLatenciasSiguiente = null;

            int indiceHoja1 = 1;
            int indiceHoja2 = 1;


            excel2.Row row = new excel2.Row() { RowIndex = (uint)(filaHoja1 + 1) };
            string valorCelda = "";


            excel2.Row row2 = new excel2.Row() { RowIndex = (uint)(filaHoja2 + 1) };
            string valorCelda2 = "";


            if (filaSiguiente != null)
            {
                tiemposBotonesTablaSiguiente = filaSiguiente.Cells[POS_Tiempos].Value.ToString();

                if (hayTiemposLatenciaActual)
                {
                    tiemposEmparejamientoSiguiente = filaActual.Cells[POS_TiemposLatencia].Value.ToString();
                    jsonLatenciasSiguiente = JsonSerializer.Deserialize<TimeSpan[]>(tiemposEmparejamientoSiguiente);
                    hayTiemposLatenciaSiguiente = !string.IsNullOrEmpty(tiemposEmparejamientoSiguiente);
                }


                jsonTiemposSiguiente = JsonSerializer.Deserialize<TimeSpan[]>(tiemposBotonesTablaSiguiente);


                //calculamos las diferencias

                int menorDelosDos = Math.Min(jsonTiemposActual.Length, jsonTiemposSiguiente.Length);
                int mayorDelosDos = Math.Max(jsonTiemposActual.Length, jsonTiemposSiguiente.Length);
                diferenciaTiempos = new TimeSpan[mayorDelosDos];

                for (int indice = 0; indice < menorDelosDos; indice++)
                {
                    diferenciaTiempos[indice] = jsonTiemposSiguiente[indice] - jsonTiemposActual[indice];
                    valorCelda = diferenciaTiempos[indice].TotalSeconds.ToString();
                    AddCellToRow(row, valorCelda, (uint)indiceHoja1 + 1);
                    indiceHoja1++;
                }

                if (hayTiemposLatenciaActual && hayTiemposLatenciaSiguiente)
                {
                    menorDelosDos = Math.Min(jsonLatenciasActual.Length, jsonLatenciasSiguiente.Length);
                    menorDelosDos = Math.Min(jsonLatenciasActual.Length, jsonLatenciasSiguiente.Length);
                    diferenciasLatencias = new TimeSpan[mayorDelosDos];

                    for (int indice = 0; indice < menorDelosDos; indice++)
                    {
                        diferenciasLatencias[indice] = jsonLatenciasSiguiente[indice] - jsonLatenciasActual[indice];
                        valorCelda2 = diferenciasLatencias[indice].TotalSeconds.ToString();
                        AddCellToRow(row2, valorCelda2, (uint)indiceHoja2 + 1);
                        indiceHoja2++;
                    }
                }
            }
            //Agregamos las filas a la hoja de cálculo.
            hojita1.Append(row);
            hojita2.Append(row2);
            filaHoja1++;
            filaHoja2++;
            bd.cerrarConexiion();
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


    }
}
