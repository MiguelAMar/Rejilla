/*
 * Miguel Ángel Martínez Jiménez
 * Versión: 2.0.
 * Fecha: Septiembre  2024.
 * Aplicación: Evaluación y Entrenamiento de la Atención.
 * 
 */

using DocumentFormat.OpenXml.Drawing;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Resources;
using System.Runtime.Versioning;
using System.Windows.Forms;

namespace demorejilla
{
    [SupportedOSPlatform("windows")]
    public partial class ExportarDatos : Form
    {
        public String[] ficherosExportar;
        public String comienzo, fin;
        public String deporte, posicion;

        public ExportarPor exportarPor;
        public DateTime fechaDesde, fechaHasta;
        public TipoRejilla tipoRejilla;
        public bool[] arrayCampos;

        private bool campoOk, tipoOk, archivoOk;
        private bool boolInicio, boolFin, boolDeporte, boolPosicion;

        private ResourceManager rm;

        public Genero sexoSujeto;
        public string pais;
        public string joven, viejo;

        private bool chico;
        private bool chica;

        private bool paisOk;
        private bool sexoOk;
        private bool boolJoven, boolviejo;

        const int TOTALCAMPOS = 23 + 2 + 2;//+2 ultimo para los datos de emparejar y los %

        public ExportarDatos()
        {
            InitializeComponent();

            rm = new ResourceManager("demorejilla.Recursos", typeof(MetiendoDatos).Assembly);

            this.comienzo = "";
            this.fin = "";
            this.deporte = "";
            this.posicion = "";

            this.tipoRejilla = TipoRejilla.NUMERICA;
            this.arrayCampos = new bool[TOTALCAMPOS];
            this.arrayCamposFalse();
            this.exportarPor = ExportarPor.VACIO;

            this.campoOk = false;
            this.tipoOk = false;
            this.archivoOk = false;
            this.boolInicio = false;
            this.boolDeporte = false;
            this.boolFin = false;
            this.boolPosicion = false;

            this.dateTimePickerFinal.MaxDate = DateTime.Today;
            this.dateTimePickerFinal.Value = DateTime.Today;

            this.dateTimerPickerInicial.MaxDate = DateTime.Today;

            this.paisOk = false;
            this.sexoOk = false;
            this.chico = false;
            this.chica = true;
            this.sexoSujeto = Genero.FEMENINO;
            this.pais = "";
            this.joven = "";
            this.viejo = "";
            this.boolJoven = false;
            this.boolviejo = false;
            this.proveedorDeError = new ErrorProvider();
        }

        //Controlas que en el alfabeto solo se puedan introducir letras
        private void tbSoloLetras_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(8))
                //se pulso la tecla retroceso o el espacio en blanco
                e.Handled = false;
            else if (e.KeyChar == Convert.ToChar(32))
                //si pulso la barra espaciadora
                e.Handled = false;
            else if (e.KeyChar == 'ñ' || e.KeyChar == 'Ñ')
                //si pulso la ñ
                e.Handled = false;
            else if ((e.KeyChar >= 'a' && e.KeyChar <= 'z') || (e.KeyChar >= 'A' && e.KeyChar <= 'Z'))
                //si pulso [a-z] o [A-Z]
                e.Handled = false;
            else
                //desechar el resto de caracteres
                e.Handled = true;
        }

        /*
         * Control de error del inicio y fin del alfabeto. Inicio <= Fin
         */
        private void tbInicio_Validating(object sender, CancelEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if ((this.tbFinal.Text != "") && (this.tbInicio.Text.CompareTo(this.tbFinal.Text) > 0))
            {
                e.Cancel = true;
                //this.proveedorDeError.SetError(tb, "Tiene que ser menor o igual que DESDE");
                this.proveedorDeError.SetError(tb, "exp04");
                this.etInformacion.Enabled = true;
                this.etInformacion.ForeColor = Color.Red;
                //this.etInformacion.Text = "Error: Desde es mayor que Hasta";
                this.etInformacion.Text = rm.GetString("exp05");
            }
        }

        private void tbFinal_Validating(object sender, CancelEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if ((this.tbInicio.Text != "") && (this.tbInicio.Text.CompareTo(this.tbFinal.Text) > 0))
            {
                e.Cancel = true;
                //this.proveedorDeError.SetError(tb, "Tiene que ser mayor o igual que HASTA");
                this.proveedorDeError.SetError(tb, rm.GetString("exp06"));
                this.etInformacion.Enabled = true;
                //this.etInformacion.Text = "ERROR: Hasta es menor que Desde";
                this.etInformacion.Text = rm.GetString("exp07");
                this.etInformacion.ForeColor = Color.Red;
            }
        }

        /*
         * Control de errores de las fechas. INICIO <= FIN
         */
        private void dateTimerPickerInicial_Validating(object sender, CancelEventArgs e)
        {
            DateTimePicker dtp = (DateTimePicker)sender;
            if (this.dateTimerPickerInicial.Value > this.dateTimePickerFinal.Value)
            {
                e.Cancel = true;
                //this.proveedorDeError.SetError(dtp,"La fecha de inicio debe ser menor o igual a la fecha de fin");
                this.proveedorDeError.SetError(dtp, rm.GetString("exp08"));
                this.etInformacion.Enabled = true;
                //this.etInformacion.Text = "Fecha INICIO debe ser MENOR o IGUAl a la Fecha FIN";
                this.etInformacion.Text = rm.GetString("exp09");
                this.etInformacion.ForeColor = Color.Red;
            }
        }

        private void dateTimePickerFinal_Validating(object sender, CancelEventArgs e)
        {
            DateTimePicker dtp = (DateTimePicker)sender;
            if (this.dateTimerPickerInicial.Value > this.dateTimePickerFinal.Value)
            {
                e.Cancel = true;
                //this.proveedorDeError.SetError(dtp, "La fecha de fin debe ser mayor o igual a la fecha de inicio");
                this.proveedorDeError.SetError(dtp, rm.GetString("exp10"));
                this.etInformacion.Enabled = true;
                this.etInformacion.ForeColor = Color.Red;
                //this.etInformacion.Text = "Fecha FINAL debe ser MAYOR o IGUAL a Fecha INICIO";
                this.etInformacion.Text = rm.GetString("exp11");
            }
        }

        /*
         * Manejo de los radioButton para exportar.
         */
        private void ImportarCAmpo_ChekedChanged(object sender, EventArgs e)
        {
            if (this.rbNombre.Checked || this.rbApellido.Checked)
            {
                this.gbAlfabeto.Enabled = true;
                this.gbDeporte.Enabled = false;
                this.gbFecha.Enabled = false;
                this.gbTipoRejilla.Enabled = false;
                this.etInformacion.Enabled = true;
                this.etInformacion.ForeColor = Color.Blue;
                //this.etInformacion.Text = "Introducir rango de Exportación.  DESDE <= HASTA";
                this.etInformacion.Text = rm.GetString("exp12");
                this.gbSexo.Enabled = false;
                this.gbEdad.Enabled = false;
                this.gbPais.Enabled = false;
            }
            else if (this.rbPosicion.Checked || this.rbDeporte.Checked || this.rbDP.Checked)
            {
                this.gbDeporte.Enabled = true;
                this.tbDeporte.Enabled = true;
                this.tbPosicion.Enabled = true;
                this.etPosicion.Enabled = true;
                this.etDeporte.Enabled = true;
                //this.gbDeporte.Text = "Deporte y Posición";
                this.gbDeporte.Text = rm.GetString("exp13");

                this.gbAlfabeto.Enabled = false;
                this.gbFecha.Enabled = false;
                this.gbTipoRejilla.Enabled = false;
                this.etInformacion.Enabled = true;
                this.etInformacion.ForeColor = Color.Blue;
                if (this.rbDeporte.Checked)
                {
                    this.tbPosicion.Enabled = false;
                    //this.etInformacion.Text = "Introducir deporte";
                    this.etInformacion.Text = rm.GetString("exp14");
                    this.etPosicion.Enabled = false;
                    //this.gbDeporte.Text = "Deporte";
                    this.gbDeporte.Text = rm.GetString("exp35");
                }
                if (this.rbPosicion.Checked)
                {
                    //this.etInformacion.Text = "Introducir posición";
                    this.etInformacion.Text = rm.GetString("exp15");
                    this.tbDeporte.Enabled = false;
                    this.etDeporte.Enabled = false;
                    //this.gbDeporte.Text = "Posición";
                    this.gbDeporte.Text = rm.GetString("exp36");
                }
                if (this.rbDP.Checked) //this.etInformacion.Text = "Introducir deporte y posición";
                    this.etInformacion.Text = rm.GetString("exp16");

                this.gbSexo.Enabled = false;
                this.gbEdad.Enabled = false;
                this.gbPais.Enabled = false;
            }
            else if (rbTipoRejilla.Checked)
            {
                this.gbAlfabeto.Enabled = false;
                this.gbFecha.Enabled = false;
                this.gbDeporte.Enabled = false;
                this.gbTipoRejilla.Enabled = true;
                this.etInformacion.Enabled = true;
                this.etInformacion.ForeColor = Color.Blue;
                //this.etInformacion.Text = "Seleccionar el TIPO de la REJILLA";
                this.etInformacion.Text = rm.GetString("exp17");
                this.gbSexo.Enabled = false;
                this.gbEdad.Enabled = false;
                this.gbPais.Enabled = false;
            }
            else if (rbFecha.Checked)
            {
                this.gbAlfabeto.Enabled = false;
                this.gbFecha.Enabled = true;
                this.gbDeporte.Enabled = false;
                this.gbTipoRejilla.Enabled = false; this.etInformacion.Enabled = true;
                this.etInformacion.ForeColor = Color.Blue;
                //this.etInformacion.Text = "Seleccionar el intervalo de fechas. INICIO <= FINAL";
                this.etInformacion.Text = rm.GetString("exp18");
                this.gbSexo.Enabled = false;
                this.gbEdad.Enabled = false;
                this.gbPais.Enabled = false;
            }
            else if (rbEdad.Checked)
            {
                this.gbAlfabeto.Enabled = false;
                this.gbFecha.Enabled = false;
                this.gbPais.Enabled = false;
                this.gbSexo.Enabled = false;
                this.gbDeporte.Enabled = false;
                this.gbTipoRejilla.Enabled = false;
                this.gbEdad.Enabled = true;
                this.etInformacion.ForeColor = Color.Blue;
                //this.etInformacion.Text = "Introducir el rango de edad";
                this.etInformacion.Text = rm.GetString("exp40");
            }
            else if (rbPais.Checked)
            {
                this.gbAlfabeto.Enabled = false;
                this.gbFecha.Enabled = false;
                this.gbPais.Enabled = true;
                this.gbSexo.Enabled = false;
                this.gbDeporte.Enabled = false;
                this.gbTipoRejilla.Enabled = false;
                this.gbEdad.Enabled = false;
                this.etInformacion.ForeColor = Color.Blue;
                //this.etInformacion.Text = "Introducir el país";
                this.etInformacion.Text = rm.GetString("exp41");
            }
            else if (rbSexo.Checked)
            {
                this.gbAlfabeto.Enabled = false;
                this.gbFecha.Enabled = false;
                this.gbTipoRejilla.Enabled = false;
                this.gbSexo.Enabled = true;
                this.gbDeporte.Enabled = false;
                this.gbPais.Enabled = false;
                this.gbEdad.Enabled = false;
                this.etInformacion.ForeColor = Color.Blue;
                //this.etInformacion.Text = "Introducir el género";
                this.etInformacion.Text = rm.GetString("exp42");
            }
            porTipos();
        }

        private void porTipos()
        {
            if (rbNombre.Checked)
                this.exportarPor = ExportarPor.NOMBRE;
            else if (rbApellido.Checked)
                this.exportarPor = ExportarPor.APELLIDO;
            else if (rbFecha.Checked)
                this.exportarPor = ExportarPor.FECHA;
            else if (rbTipoRejilla.Checked)
                this.exportarPor = ExportarPor.TIPO;
            else if (rbDeporte.Checked)
                this.exportarPor = ExportarPor.DEPORTE;
            else if (rbPosicion.Checked)
                this.exportarPor = ExportarPor.POSICION;
            else if (rbDP.Checked)
                this.exportarPor = ExportarPor.DEPORTE_POSICION;
            else if (rbSexo.Checked)
                this.exportarPor = ExportarPor.SEXO;
            else if (rbPais.Checked)
                this.exportarPor = ExportarPor.PAIS;
            else if (rbEdad.Checked)
                this.exportarPor = ExportarPor.EDAD;

            this.tipoOk = true;
        }

        /*
         * Pulsación del botón abrir ficheros para exportar.
         */
        private void btSelec_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            //ofd.Filter = "Archivos de datos *.tbl |*.tbl";
            ofd.Filter = rm.GetString("exp19");
            ofd.Multiselect = true;
            ofd.RestoreDirectory = true;
            ofd.InitialDirectory = CarpetaMisDocumentos.ruta();
            //ofd.Title = "Seleccionar archivos para exportar a Excel";
            ofd.Title = rm.GetString("exp20");

            this.etInformacion.Enabled = true;
            this.etInformacion.ForeColor = Color.Blue;
            //this.etInformacion.Text = "Seleccionar archivos para exportar los datos";
            this.etInformacion.Text = rm.GetString("exp21");
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                etInformacion.Enabled = true;
                etInformacion.ForeColor = Color.Green;
                //etInformacion.Text = "Archivos cargados con éxito";
                etInformacion.Text = rm.GetString("exp22");
                this.ficherosExportar = ofd.FileNames;
                this.proveedorDeError.Clear();
                this.archivoOk = true;
                
                if (this.exportarPor == ExportarPor.NOMBRE || this.exportarPor == ExportarPor.APELLIDO)
                {
                    if (boolInicio && boolFin && this.campoOk && this.tipoOk)
                        if (todoCorrectoParaExportar())
                            this.btExportar.DialogResult = DialogResult.OK;
                }
                else if (this.exportarPor == ExportarPor.DEPORTE_POSICION)
                {
                    if (this.campoOk && this.tipoOk && this.boolPosicion && this.boolDeporte)
                        if (todoCorrectoParaExportar())
                            this.btExportar.DialogResult = DialogResult.OK;
                }
                else if (this.exportarPor == ExportarPor.DEPORTE)
                {
                    if (this.campoOk && this.tipoOk && this.boolDeporte)
                        if (todoCorrectoParaExportar())
                            this.btExportar.DialogResult = DialogResult.OK;
                }
                else if (this.exportarPor == ExportarPor.POSICION)
                {
                    if (this.campoOk && this.tipoOk && this.boolPosicion)
                        if (todoCorrectoParaExportar())
                            this.btExportar.DialogResult = DialogResult.OK;
                }
                else if (exportarPor == ExportarPor.PAIS)
                {
                    if (this.campoOk && this.tipoOk && this.paisOk)
                        if (todoCorrectoParaExportar())
                            this.btExportar.DialogResult = DialogResult.OK;
                }
                else if (this.exportarPor == ExportarPor.EDAD)
                {
                    if (this.campoOk && this.tipoOk && this.boolJoven && this.boolviejo)
                        if (todoCorrectoParaExportar())
                            this.btExportar.DialogResult = DialogResult.OK;
                }
                else
                {
                    if (this.campoOk && this.tipoOk)
                        if (todoCorrectoParaExportar())
                            this.btExportar.DialogResult = DialogResult.OK;
                }
            }
            else
            {
                this.etInformacion.ForeColor = Color.Blue;
                this.etInformacion.Text = "";
                this.archivoOk = false;
                this.proveedorDeError.SetError(this.btSelec, rm.GetString("exp30"));
            }
        }

        /*
         * Control para los checkbox de campos
         */
        private void seleccionarTodos()
        {
            this.cbNombre.Checked = true;
            this.cbApellidos.Checked = true;
            this.cbFecha.Checked = true;
            this.cbObservaciones.Checked = true;

            this.cbTipoRejilla.Checked = true;
            this.cbAleatorio.Checked = true;
            this.cbTiempoLimite.Checked = true;
            this.cbTipoBoton.Checked = true;

            this.cbDistraccionLinea.Checked = true;
            //this.cbColor.Checked = true;
            this.cbGrosor.Checked = true;

            this.cbGrafica.Checked = true;
            this.cbMusica.Checked = true;
            this.cbTiemposGrafica.Checked = true;
            this.cbMetronomo.Checked = true;

            this.cbArribaAbajo.Checked = true;
            this.cbAciertoFallos.Checked = true;

            //this.etInformacion.Text = "Seleccionados todos los campos.";
            this.etInformacion.Text = rm.GetString("exp23");
            this.etInformacion.Enabled = true;
            this.etInformacion.ForeColor = Color.Green;

            this.cbDeporte.Checked = true;
            this.cbPosicion.Checked = true;

            this.cbEdad.Checked = true;
            this.cbSexo.Checked = true;
            this.cbPais.Checked = true;

            this.cbLateralidad.Checked = true;
            this.cbPorcentajes.Checked = true;
            this.cbEmparejar.Checked = true;

            this.campoOk = true;
        }

        private void btNoSelec_Click(object sender, EventArgs e)
        {
            this.cbNombre.Checked = false;
            this.cbApellidos.Checked = false;
            this.cbFecha.Checked = false;
            this.cbObservaciones.Checked = false;

            this.cbTipoRejilla.Checked = false;
            this.cbAleatorio.Checked = false;
            this.cbTiempoLimite.Checked = false;
            this.cbTipoBoton.Checked = false;

            this.cbDistraccionLinea.Checked = false;
            //this.cbColor.Checked = false;
            this.cbGrosor.Checked = false;

            this.cbGrafica.Checked = false;
            this.cbMusica.Checked = false;
            this.cbTiemposGrafica.Checked = false;
            this.cbMetronomo.Checked = false;

            this.cbArribaAbajo.Checked = false;
            this.cbAciertoFallos.Checked = false;

            this.etInformacion.ForeColor = Color.Green;
            this.etInformacion.Enabled = true;
            //this.etInformacion.Text = "Deseleccionados todos los campos.";
            this.etInformacion.Text = rm.GetString("exp24");
            this.cbTodos.Checked = false;
            this.arrayCamposFalse();

            this.cbDeporte.Checked = false;
            this.cbPosicion.Checked = false;

            this.cbEdad.Checked = false;
            this.cbPais.Checked = false;
            this.cbSexo.Checked = false;

            this.cbLateralidad.Checked = false;
            this.cbEmparejar.Checked = false;
            this.cbPorcentajes.Checked = false;

            this.campoOk = false;
        }

        private void cbTodos_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            if (cb.Checked == true)
            {
                seleccionarTodos();
                this.campoOk = true;
            }
        }

        /*
         * Si se produce algún error, aparece el proveedor de error, cuando
         * este solucionado el error, quitar el proveedor DeError.
         */
        private void CajaTexto_Validated(object sender, EventArgs e)
        {
            this.proveedorDeError.Clear();
            this.etInformacion.Text = "";
        }

        /*
         * Comprobar que todo es correcto para exportar y exportar. Si
         * hay algun error salta le proveedor de error.
         */
        private void btExportar_Click(object sender, EventArgs e)
        {
          
            if (this.exportarPor == ExportarPor.VACIO)
            {
                //this.proveedorDeError.SetError(this.gbImportarPorCampo, "Se debe seleeccionar al menos un campo.");
                this.proveedorDeError.SetError(this.gbImportarPorCampo, rm.GetString("exp25"));
            }

            if (this.exportarPor == ExportarPor.APELLIDO || this.exportarPor == ExportarPor.NOMBRE)
            {
                this.comienzo = this.tbInicio.Text;
                this.fin = this.tbFinal.Text;
                if (string.IsNullOrEmpty(this.comienzo) || string.IsNullOrEmpty(this.fin))
                {
                    //this.etInformacion.Text = "Introducir rango de Exportación.  DESDE <= HASTA";
                    this.etInformacion.Text = rm.GetString("exp26");
                    //this.proveedorDeError.SetError(this.gbAlfabeto, "ERROR: Nombre o Apellido: Los campos DESDE y/o HASTA no pueden ser vacios.");
                    this.proveedorDeError.SetError(this.gbAlfabeto, rm.GetString("exp27"));
                }


                /*
                
                if (this.comienzo == "" || this.fin == "")
                {
                    //this.etInformacion.Text = "Introducir rango de Exportación.  DESDE <= HASTA";
                    this.etInformacion.Text = rm.GetString("exp26");
                    //this.proveedorDeError.SetError(this.gbAlfabeto, "ERROR: Nombre o Apellido: Los campos DESDE y/o HASTA no pueden ser vacios.");
                    this.proveedorDeError.SetError(this.gbAlfabeto, rm.GetString("exp27"));
                }
                */
            }


            if (!this.arrayCampos.Contains(true))
            {
                //this.proveedorDeError.SetError(this.gbCampos, "Se debe seleccionar al menos un campo");
                this.proveedorDeError.SetError(gbCampos, rm.GetString("exp25"));
                this.etInformacion.ForeColor = Color.Red;
                //this.etInformacion.Text = "Se debe seleccionar al menos un campo para exportar.";
                this.etInformacion.Text = rm.GetString("exp28");
            }

            if (!this.archivoOk)//(this.ficherosExportar == null)
            {
                //this.proveedorDeError.SetError(btSelec, "Seleccionar Ficheros para exportar datos.");
                this.proveedorDeError.SetError(btSelec, rm.GetString("exp29"));
                this.etInformacion.ForeColor = Color.Red;
                //this.etInformacion.Text = "No se ha seleccionado ningún fichero para exportar los datos.";
                this.etInformacion.Text = rm.GetString("exp30");
            }

            if (this.exportarPor == ExportarPor.FECHA)
            {
                this.fechaDesde = this.dateTimerPickerInicial.Value;
                this.fechaHasta = this.dateTimePickerFinal.Value;
            }

            if (this.exportarPor == ExportarPor.POSICION)
            {
                this.posicion = this.tbPosicion.Text;
                if (this.posicion == "")
                {
                    //this.proveedorDeError.SetError(this.tbPosicion, "El campo posición no puede ser vacio");
                    this.proveedorDeError.SetError(this.tbPosicion, rm.GetString("exp32"));
                    this.etInformacion.ForeColor = Color.Red;
                    //this.etInformacion.Text = "El campo posición no puede ser vacio";
                    this.etInformacion.Text = rm.GetString("exp31");
                }
            }
            if (this.exportarPor == ExportarPor.DEPORTE)
            {
                this.deporte = this.tbDeporte.Text;
                if (this.deporte == "")
                {
                    //this.proveedorDeError.SetError(this.tbDeporte, "Este campo no puede ser vacio");
                    this.proveedorDeError.SetError(this.tbDeporte, rm.GetString("exp32"));
                    this.etInformacion.ForeColor = Color.Red;
                    //this.etInformacion.Text = "El campo deporte no puede ser vacio";
                    this.etInformacion.Text = rm.GetString("exp33");
                }
            }
            if (this.exportarPor == ExportarPor.DEPORTE_POSICION)
            {
                this.posicion = this.tbPosicion.Text;
                this.deporte = this.tbDeporte.Text;
                if (this.deporte == "" || this.posicion == "")
                {
                    //this.proveedorDeError.SetError(this.gbDeporte, "Los campos no pueden ser vacio");
                    this.proveedorDeError.SetError(this.gbDeporte, rm.GetString("exp34"));
                    this.etInformacion.ForeColor = Color.Red;
                    //this.etInformacion.Text = "Los campos no puede ser vacios.";
                    this.etInformacion.Text = rm.GetString("exp34");
                }
            }
            if (this.exportarPor == ExportarPor.PAIS)
            {
                this.pais = this.tbPais.Text;
                if (this.pais == "")
                {
                    //this.proveedorDeError.SetError(this.gbPais, "ELCAMPO PAIS NO PUEDE SER VACIO.");
                    this.proveedorDeError.SetError(this.gbPais, rm.GetString("exp43"));
                    this.etInformacion.ForeColor = Color.Red;
                    this.etInformacion.Text = rm.GetString("exp43");//"El campo pais no puede ser vacio";
                }
            }
            if (this.exportarPor == ExportarPor.EDAD)
            {
                this.joven = this.tbJoven.Text;
                this.viejo = this.tbViejo.Text;
                if (this.joven == "" || this.viejo == "")
                {
                    //this.proveedorDeError.SetError(this.gbEdad, "Los  campos edad no pueden estar vacion");
                    this.proveedorDeError.SetError(this.gbEdad, rm.GetString("exp44"));
                    this.etInformacion.ForeColor = Color.Red;
                    this.etInformacion.Text = rm.GetString("exp44");//"CAmpos edad deben estar reyenos";
                }
            }
        }

        private bool todoCorrectoParaExportar()
        {
            bool todook = false;
            if (this.exportarPor == ExportarPor.VACIO)
            {
                //this.proveedorDeError.SetError(this.gbImportarPorCampo, "Se debe seleeccionar al me
                //nos un campo.");
                this.proveedorDeError.SetError(this.gbImportarPorCampo, rm.GetString("exp25"));
                todook = false;
            }

            if (this.exportarPor == ExportarPor.APELLIDO || this.exportarPor == ExportarPor.NOMBRE)
            {
                this.comienzo = this.tbInicio.Text;
                this.fin = this.tbFinal.Text;
                if (string.IsNullOrEmpty(this.comienzo) || string.IsNullOrEmpty(this.fin))
                {
                    //this.etInformacion.Text = "Introducir rango de Exportación.  DESDE <= HASTA";
                    this.etInformacion.Text = rm.GetString("exp26");
                    //this.proveedorDeError.SetError(this.gbAlfabeto, "ERROR: Nombre o Apellido: Los campos DESDE y/o HASTA no pueden ser vacios.");
                    this.proveedorDeError.SetError(this.gbAlfabeto, rm.GetString("exp27"));
                    todook = false;
                }

            }


            if (!this.arrayCampos.Contains(true))
            {
                //this.proveedorDeError.SetError(this.gbCampos, "Se debe seleccionar al menos un campo");
                this.proveedorDeError.SetError(gbCampos, rm.GetString("exp25"));
                this.etInformacion.ForeColor = Color.Red;
                //this.etInformacion.Text = "Se debe seleccionar al menos un campo para exportar.";
                this.etInformacion.Text = rm.GetString("exp28");
                todook = false;
            }

            if (!this.archivoOk)//(this.ficherosExportar == null)
            {
                //this.proveedorDeError.SetError(btSelec, "Seleccionar Ficheros para exportar datos.");
                this.proveedorDeError.SetError(btSelec, rm.GetString("exp29"));
                this.etInformacion.ForeColor = Color.Red;
                //this.etInformacion.Text = "No se ha seleccionado ningún fichero para exportar los datos.";
                this.etInformacion.Text = rm.GetString("exp30");
                todook = false;
            }

            if (this.exportarPor == ExportarPor.FECHA)
            {
                this.fechaDesde = this.dateTimerPickerInicial.Value;
                this.fechaHasta = this.dateTimePickerFinal.Value;
                todook = true;
            }

            if (this.exportarPor == ExportarPor.POSICION)
            {
                this.posicion = this.tbPosicion.Text;
                if (this.posicion == "")
                {
                    //this.proveedorDeError.SetError(this.tbPosicion, "El campo posición no puede ser vacio");
                    this.proveedorDeError.SetError(this.tbPosicion, rm.GetString("exp32"));
                    this.etInformacion.ForeColor = Color.Red;
                    //this.etInformacion.Text = "El campo posición no puede ser vacio";
                    this.etInformacion.Text = rm.GetString("exp31");
                    todook = false;
                }
            }
            if (this.exportarPor == ExportarPor.DEPORTE)
            {
                this.deporte = this.tbDeporte.Text;
                if (this.deporte == "")
                {
                    //this.proveedorDeError.SetError(this.tbDeporte, "Este campo no puede ser vacio");
                    this.proveedorDeError.SetError(this.tbDeporte, rm.GetString("exp32"));
                    this.etInformacion.ForeColor = Color.Red;
                    //this.etInformacion.Text = "El campo deporte no puede ser vacio";
                    this.etInformacion.Text = rm.GetString("exp33");
                    todook = false;
                }
            }
            if (this.exportarPor == ExportarPor.DEPORTE_POSICION)
            {
                this.posicion = this.tbPosicion.Text;
                this.deporte = this.tbDeporte.Text;
                if (this.deporte == "" || this.posicion == "")
                {
                    //this.proveedorDeError.SetError(this.gbDeporte, "Los campos no pueden ser vacio");
                    this.proveedorDeError.SetError(this.gbDeporte, rm.GetString("exp34"));
                    this.etInformacion.ForeColor = Color.Red;
                    //this.etInformacion.Text = "Los campos no puede ser vacios.";
                    this.etInformacion.Text = rm.GetString("exp34");
                    todook = false;
                }
            }
            if (this.exportarPor == ExportarPor.PAIS)
            {
                this.pais = this.tbPais.Text;
                if (this.pais == "")
                {
                    //this.proveedorDeError.SetError(this.gbPais, "ELCAMPO PAIS NO PUEDE SER VACIO.");
                    this.proveedorDeError.SetError(this.gbPais, rm.GetString("exp43"));
                    this.etInformacion.ForeColor = Color.Red;
                    this.etInformacion.Text = rm.GetString("exp43");//"El campo pais no puede ser vacio";
                    todook = false;
                }
            }
            if (this.exportarPor == ExportarPor.EDAD)
            {
                this.joven = this.tbJoven.Text;
                this.viejo = this.tbViejo.Text;
                if (this.joven == "" || this.viejo == "")
                {
                    //this.proveedorDeError.SetError(this.gbEdad, "Los  campos edad no pueden estar vacion");
                    this.proveedorDeError.SetError(this.gbEdad, rm.GetString("exp44"));
                    this.etInformacion.ForeColor = Color.Red;
                    this.etInformacion.Text = rm.GetString("exp44");//"CAmpos edad deben estar reyenos";
                    todook = false;
                }
            }
            if (this.exportarPor == ExportarPor.TIPO)
            {
                todook = true;
            }
            return todook;
        }

        private void btExportar2_Click(object sender, EventArgs e)
        {
            if (this.exportarPor == ExportarPor.TIPO)
            {
                if (todoCorrectoParaExportar())
                {
                    btExportar.DialogResult = DialogResult.OK;
                }
                //else
                    //btExportar.DialogResult = DialogResult.Retry;
            }
            else
            {
                if (todoCorrectoParaExportar())
                    btExportar.DialogResult = DialogResult.OK;
                //else
                    //btExportar.DialogResult = DialogResult.Retry;
            }
         
        }

        /*
         * Para exportar por el tipo de rejilla
         */
        private void porTipoChekedChanged(object sender, EventArgs e)
        {
            if (rbNumericaExportar.Checked)
                this.tipoRejilla = TipoRejilla.NUMERICA;
            else if (rbAbecedarioExportar.Checked)
                this.tipoRejilla = TipoRejilla.ABECEDARIO;
            else if (rbColoresExportar.Checked)
                this.tipoRejilla = TipoRejilla.COLORES;
            else if (rbImagenesExportar.Checked)
                this.tipoRejilla = TipoRejilla.IMAGENES;
            else if (rbWingdingsExportar.Checked)
                this.tipoRejilla = TipoRejilla.WINGDINGS;
            else if (rbLetrasExportar.Checked)
                this.tipoRejilla = TipoRejilla.LETRAS;
        }

        /*
         * Todo el array de los camposa true
         */
        private void arrayCamposTrue()
        {
            for (int ind = 0; ind < arrayCampos.Length; ind++)
                arrayCampos[ind] = true;
        }

        /*
         * Todo el array de campos a false
         */
        private void arrayCamposFalse()
        {
            for (int ind = 0; ind < arrayCampos.Length; ind++)
                arrayCampos[ind] = false;
        }

        /*
         * Si seleccionamos todos los campos con la casilla todos los campos
         */
        private void camposCheckedChanged(object sender, EventArgs e)
        {
            if (cbTodos.Checked)
            {
                arrayCamposTrue();
                this.seleccionarTodos();
            }
            if (!cbTodos.Checked)
            {
                arrayCampos[0] = false;
                this.campoOk = false;
            }
        }

        private void Campos_Validated(object sender, EventArgs e)
        {
            if (arrayCampos.Contains(true))
            {
                this.proveedorDeError.Clear();
                this.etInformacion.Text = "";
                this.campoOk = true;
            }
        }

        /*
         * Cambio en los checkbox de los campos
         */
        private void campo_ChekedChanged(object sender, EventArgs e)
        {
            if (cbNombre.Checked)
                arrayCampos[1] = true;
            if (!cbNombre.Checked)
                arrayCampos[1] = false;
            if (cbApellidos.Checked)
                arrayCampos[2] = true;
            if (!cbApellidos.Checked)
                arrayCampos[2] = false;
            if (cbFecha.Checked)
                arrayCampos[3] = true;
            if (!cbFecha.Checked)
                arrayCampos[3] = false;
            if (cbObservaciones.Checked)
                arrayCampos[4] = true;
            if (!cbObservaciones.Checked)
                arrayCampos[4] = false;
            if (cbTipoRejilla.Checked)
                arrayCampos[5] = true;
            if (!cbTipoRejilla.Checked)
                arrayCampos[5] = false;
            if (cbTiempoLimite.Checked)
                arrayCampos[6] = true;
            if (!cbTiempoLimite.Checked)
                arrayCampos[6] = false;
            if (cbAleatorio.Checked)
                arrayCampos[7] = true;
            if (!cbAleatorio.Checked)
                arrayCampos[7] = false;
            if (cbTipoBoton.Checked)
                arrayCampos[8] = true;
            if (!cbTipoBoton.Checked)
                arrayCampos[8] = false;
            if (cbDistraccionLinea.Checked)
                arrayCampos[9] = true;
            if (!cbDistraccionLinea.Checked)
                arrayCampos[9] = false;
            //if (cbColor.Checked)
            //    arrayCampos[10] = true;
            //if (!cbColor.Checked)
            //    arrayCampos[10] = false;
            if (cbGrosor.Checked)
                arrayCampos[11] = true;
            if (!cbGrosor.Checked)
                arrayCampos[11] = false;
            if (cbArribaAbajo.Checked)
                arrayCampos[12] = true;
            if (!cbArribaAbajo.Checked)
                arrayCampos[12] = false;
            if (cbMusica.Checked)
                arrayCampos[13] = true;
            if (!cbMusica.Checked)
                arrayCampos[13] = false;
            if (cbMetronomo.Checked)
                arrayCampos[14] = true;
            if (!cbMetronomo.Checked)
                arrayCampos[14] = false;
            if (cbTiemposGrafica.Checked)
                arrayCampos[15] = true;
            if (!cbTiemposGrafica.Checked)
                arrayCampos[15] = false;
            if (cbGrafica.Checked)
                arrayCampos[16] = true;
            if (!cbGrafica.Checked)
                arrayCampos[16] = false;
            if (cbAciertoFallos.Checked)
                arrayCampos[17] = true;
            if (!cbAciertoFallos.Checked)
                arrayCampos[17] = false;
            if (cbDeporte.Checked)
                arrayCampos[18] = true;
            if (!cbDeporte.Checked)
                arrayCampos[18] = false;
            if (cbPosicion.Checked)
                arrayCampos[19] = true;
            if (!cbPosicion.Checked)
                arrayCampos[19] = false;
            if (cbEdad.Checked)
                arrayCampos[20] = true;
            if (!cbEdad.Checked)
                arrayCampos[20] = false;
            if (cbSexo.Checked)
                arrayCampos[21] = true;
            if (!cbSexo.Checked)
                arrayCampos[21] = false;
            if (cbPais.Checked)
                arrayCampos[22] = true;
            if (!cbPais.Checked)
                arrayCampos[22] = false;
            if (cbLateralidad.Checked)
                arrayCampos[10] = true;
            if (!cbLateralidad.Checked)
                arrayCampos[10] = false;
            if (cbEmparejar.Checked)
                arrayCampos[23] = true;
            if (!cbEmparejar.Checked)
                arrayCampos[23] = false;
            if (cbPorcentajes.Checked)
                arrayCampos[24] = true;
            if (!cbPorcentajes.Checked)
                arrayCampos[24] = false;
            if (cbEmparejar.Checked)
                arrayCampos[25] = true;
            if (!cbEmparejar.Checked)
                arrayCampos[25] = false;
            if (cbPorcentajes.Checked)
                arrayCampos[26] = true;
            if (!cbPorcentajes.Checked)
                arrayCampos[26] = false;

        }

        /*
         * cuando se deja el gb importar por campos
         */
        private void gbImportarPorCampo_Leave(object sender, EventArgs e)
        {
            if (this.campoOk && this.archivoOk)
            {
                //this.btExportar.DialogResult = DialogResult.OK;
                dialogoOK();
            }
        }

        /*
         * Cuando se activa el gbCampos
         */
        private void gbCampos_Enter(object sender, EventArgs e)
        {
            if (this.tipoOk && this.archivoOk)
            {
                dialogoOK();
            }
        }

        /*
         * cuando se activa el boton seleccionar archivo
         */
        private void btSelec_Enter(object sender, EventArgs e)
        {
            if (this.tipoOk && this.campoOk)
            {
                dialogoOK();
            }
        }

        /*
         * Control de las fechas dentro de rango fecha_inicio <= fecha_fin
         */
        private void dateTimerPickerInicial_ValueChanged(object sender, EventArgs e)
        {
            if (this.dateTimePickerFinal.Value < this.dateTimerPickerInicial.Value)
                this.dateTimePickerFinal.Value = this.dateTimerPickerInicial.Value;
        }

        private void dateTimePickerFinal_ValueChanged(object sender, EventArgs e)
        {
            if (this.dateTimerPickerInicial.Value > this.dateTimePickerFinal.Value)
                this.dateTimerPickerInicial.Value = this.dateTimePickerFinal.Value;
        }

        /*
         * Para controlar si lo ultimo que hacemos en escribir texto en las cajas de 
         * texto. Ya he seleccionado el archivo, el tipo por el que exportar
         * y los campos.
         */
        private void ultimoTexto()
        {
                    if (this.archivoOk && this.tipoOk && this.campoOk)
            {
                if (this.tbInicio.Text != "")
                {
                    if (this.tbFinal.Text != "")
                        this.btExportar.DialogResult = DialogResult.OK;
                }
                else if (this.tbFinal.Text != "")
                {
                    if (this.tbInicio.Text != "")
                        this.btExportar.DialogResult = DialogResult.OK;
                }
                else if (this.tbDeporte.Text != "")
                {
                    if (this.tbPosicion.Enabled && this.tbPosicion.Text != "")
                        this.btExportar.DialogResult = DialogResult.OK;
                    else
                        this.btExportar.DialogResult = DialogResult.OK;
                }
                else if (this.tbPosicion.Text != "")
                {
                    if (this.tbDeporte.Enabled && this.tbDeporte.Text != "")
                        this.btExportar.DialogResult = DialogResult.OK;
                    else
                        this.btExportar.DialogResult = DialogResult.OK;
                }
                else if (this.tbPais.Text != "")
                {
                    this.btExportar.DialogResult = DialogResult.OK;
                }
                else if (this.tbJoven.Text != "")
                {
                    if (this.tbViejo.Text != "")
                        this.btExportar.DialogResult = DialogResult.OK;
                }
                else if (this.tbViejo.Text != "")
                {
                    if (this.tbJoven.Text != "")
                        this.btExportar.DialogResult = DialogResult.OK;
                }
            }
        }

        /*
         * Si ya esta seleccionado el exportar por, relleno los campos correspondientes
         * 
         */
        private void dialogoOK()
        {
            if (this.exportarPor == ExportarPor.NOMBRE || this.exportarPor == ExportarPor.APELLIDO)
            {
                if (this.boolInicio && this.boolFin)
                    this.btExportar.DialogResult = DialogResult.OK;
            }
            else if (this.exportarPor == ExportarPor.DEPORTE_POSICION)
            {
                if (this.boolDeporte && this.boolPosicion)
                    this.btExportar.DialogResult = DialogResult.OK;
            }
            else if (this.exportarPor == ExportarPor.DEPORTE)
            {
                if (this.boolDeporte)
                    this.btExportar.DialogResult = DialogResult.OK;
            }
            else if (this.exportarPor == ExportarPor.POSICION)
            {
                if (this.boolPosicion)
                    this.btExportar.DialogResult = DialogResult.OK;
            }
            else if (this.exportarPor == ExportarPor.EDAD)
            {
                if (this.boolviejo && this.boolJoven)
                    this.btExportar.DialogResult = DialogResult.OK;
            }
            else if (this.exportarPor == ExportarPor.PAIS)
            {
                if (this.paisOk)
                    this.btExportar.DialogResult = DialogResult.OK;
            }
            else if (this.exportarPor == ExportarPor.SEXO)
            {
                if (this.sexoOk)
                    this.btExportar.DialogResult = DialogResult.OK;
            }
        }

        /*
         * TextBox de inico y fin del alfabeto
         */
        private void tbInicio_KeyUp(object sender, KeyEventArgs e)
        {
            if (this.tbInicio.Text != "")
            {
                this.boolInicio = true;
                ultimoTexto();
            }
            else
                this.boolInicio = false;
        }

        private void tbFinal_KeyUp(object sender, KeyEventArgs e)
        {
            if (this.tbFinal.Text != "")
            {
                this.boolFin = true;
                ultimoTexto();
            }
            else
                this.boolFin = false;
        }

        /*
         * TextBox Deporte y posición
         */
        private void tbDeporte_KeyUp(object sender, KeyEventArgs e)
        {
            if (this.tbDeporte.Text != "")
            {
                this.boolDeporte = true;
                ultimoTexto();
            }
            else
                this.boolDeporte = false;
        }

        private void tbPosicion_KeyUp(object sender, KeyEventArgs e)
        {
            if (this.tbPosicion.Text != "")
            {
                this.boolPosicion = true;
                ultimoTexto();
            }
            else
                this.boolPosicion = false;
        }

        /*
         * Control para la edad, solo números
         */
        private void tbSoloNumeros_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(8))
                //se pulso la tecla retroceso o el espacio en blanco
                e.Handled = false;
            else if (e.KeyChar <= '9' && e.KeyChar >= '0')
                e.Handled = false;
            else
                //desechar el resto de caracteres
                e.Handled = true;
        }

        /*
         * Control tbPais
         */
        private void tbPais_KeyUp(object sender, KeyEventArgs e)
        {
            if (tbPais.Text != "")
            {
                this.paisOk = true;
                ultimoTexto();
            }
            else
                this.paisOk = false;
        }

        /*
         * Control cuando usamos teclas en los campos edad
         */
        private void tbJoven_KeyUp(object sender, KeyEventArgs e)
        {
            if (tbJoven.Text != "")
            {
                this.boolJoven = true;
                ultimoTexto();
            }
            else
                this.boolJoven = false;
        }

        private void tbViejo_KeyUp(object sender, KeyEventArgs e)
        {
            if (tbViejo.Text != "")
            {
                this.boolviejo = true;
                ultimoTexto();
            }
            else
                this.boolviejo = false;
        }

        /*
         * Control de errores en la edad. INICIO MENOR O IGUAL FIN
         */
        private void tbJoven_Validating(object sender, CancelEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if ((this.tbViejo.Text != "") &&  (Convert.ToInt32(this.tbJoven.Text) < Convert.ToInt32(this.tbViejo.Text)))    // (this.tbJoven.Text.CompareTo(this.tbViejo.Text) > 0))
            {
                e.Cancel = true;
                //this.proveedorDeError.SetError(tb, "Tiene que ser menor o igual que DESDE");
                this.proveedorDeError.SetError(tb, "exp04");
                this.etInformacion.Enabled = true;
                this.etInformacion.ForeColor = Color.Red;
                //this.etInformacion.Text = "Error: Desde es mayor que Hasta";
                this.etInformacion.Text = rm.GetString("exp05");
            }
        }

        private void tbViejo_Validating(object sender, CancelEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if ((this.tbJoven.Text != "") && (Convert.ToInt32(this.tbJoven.Text) > Convert.ToInt32(this.tbViejo.Text)))               //(this.tbJoven.Text.CompareTo(this.tbViejo.Text) > 0))
            {
                e.Cancel = true;
                //this.proveedorDeError.SetError(tb, "Tiene que ser mayor o igual que HASTA");
                this.proveedorDeError.SetError(tb, rm.GetString("exp06"));
                this.etInformacion.Enabled = true;
                //this.etInformacion.Text = "ERROR: Hasta es menor que Desde";
                this.etInformacion.Text = rm.GetString("exp07");
                this.etInformacion.ForeColor = Color.Red;
            }
        }

        private void Sexo_CheckedChanged(object sender, EventArgs e)
        {
            this.chica = this.rbMujer.Checked;
            this.chico = this.rbHombre.Checked;
            if (this.chica)
                this.sexoSujeto = Genero.FEMENINO;
            if (this.chico)
                this.sexoSujeto = Genero.MASCULINO;
        }

        private void btSelec_Leave(object sender, EventArgs e)
        {
            if (this.ficherosExportar != null)
            {
                if (this.exportarPor != ExportarPor.VACIO)
                    if (todoCorrectoParaExportar())
                        this.btExportar.DialogResult = DialogResult.OK;
            }
        }
    }
}
