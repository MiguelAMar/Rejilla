using System;
namespace demorejilla
{
    partial class ExportarDatos
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExportarDatos));
            rbNombre = new System.Windows.Forms.RadioButton();
            gbImportarPorCampo = new System.Windows.Forms.GroupBox();
            rbPais = new System.Windows.Forms.RadioButton();
            rbSexo = new System.Windows.Forms.RadioButton();
            rbEdad = new System.Windows.Forms.RadioButton();
            rbDP = new System.Windows.Forms.RadioButton();
            rbPosicion = new System.Windows.Forms.RadioButton();
            rbDeporte = new System.Windows.Forms.RadioButton();
            rbFecha = new System.Windows.Forms.RadioButton();
            rbTipoRejilla = new System.Windows.Forms.RadioButton();
            rbApellido = new System.Windows.Forms.RadioButton();
            dateTimerPickerInicial = new System.Windows.Forms.DateTimePicker();
            dateTimePickerFinal = new System.Windows.Forms.DateTimePicker();
            etFechaInicio = new System.Windows.Forms.Label();
            etFechaFin = new System.Windows.Forms.Label();
            tbInicio = new System.Windows.Forms.TextBox();
            etInicio = new System.Windows.Forms.Label();
            etFinal = new System.Windows.Forms.Label();
            tbFinal = new System.Windows.Forms.TextBox();
            proveedorDeError = new System.Windows.Forms.ErrorProvider(components);
            gbFecha = new System.Windows.Forms.GroupBox();
            gbAlfabeto = new System.Windows.Forms.GroupBox();
            gbTipoRejilla = new System.Windows.Forms.GroupBox();
            rbWingdingsExportar = new System.Windows.Forms.RadioButton();
            rbAbecedarioExportar = new System.Windows.Forms.RadioButton();
            rbImagenesExportar = new System.Windows.Forms.RadioButton();
            rbLetrasExportar = new System.Windows.Forms.RadioButton();
            rbColoresExportar = new System.Windows.Forms.RadioButton();
            rbNumericaExportar = new System.Windows.Forms.RadioButton();
            btExportar = new System.Windows.Forms.Button();
            btCancelar = new System.Windows.Forms.Button();
            btSelec = new System.Windows.Forms.Button();
            btNoSelec = new System.Windows.Forms.Button();
            gbCampos = new System.Windows.Forms.GroupBox();
            etLatencia = new System.Windows.Forms.Label();
            cbPorcentajes = new System.Windows.Forms.CheckBox();
            cbEmparejar = new System.Windows.Forms.CheckBox();
            cbLateralidad = new System.Windows.Forms.CheckBox();
            cbPais = new System.Windows.Forms.CheckBox();
            cbSexo = new System.Windows.Forms.CheckBox();
            cbEdad = new System.Windows.Forms.CheckBox();
            cbPosicion = new System.Windows.Forms.CheckBox();
            cbDeporte = new System.Windows.Forms.CheckBox();
            cbAciertoFallos = new System.Windows.Forms.CheckBox();
            cbArribaAbajo = new System.Windows.Forms.CheckBox();
            cbMetronomo = new System.Windows.Forms.CheckBox();
            cbTodos = new System.Windows.Forms.CheckBox();
            cbTipoBoton = new System.Windows.Forms.CheckBox();
            cbMusica = new System.Windows.Forms.CheckBox();
            cbObservaciones = new System.Windows.Forms.CheckBox();
            cbTiemposGrafica = new System.Windows.Forms.CheckBox();
            cbGrafica = new System.Windows.Forms.CheckBox();
            cbGrosor = new System.Windows.Forms.CheckBox();
            cbDistraccionLinea = new System.Windows.Forms.CheckBox();
            cbAleatorio = new System.Windows.Forms.CheckBox();
            cbTiempoLimite = new System.Windows.Forms.CheckBox();
            cbTipoRejilla = new System.Windows.Forms.CheckBox();
            cbFecha = new System.Windows.Forms.CheckBox();
            cbApellidos = new System.Windows.Forms.CheckBox();
            cbNombre = new System.Windows.Forms.CheckBox();
            etInformacion = new System.Windows.Forms.Label();
            gbBotones = new System.Windows.Forms.GroupBox();
            gbInformacion = new System.Windows.Forms.GroupBox();
            gbDeporte = new System.Windows.Forms.GroupBox();
            etDeporte = new System.Windows.Forms.Label();
            tbDeporte = new System.Windows.Forms.TextBox();
            etPosicion = new System.Windows.Forms.Label();
            tbPosicion = new System.Windows.Forms.TextBox();
            gbSexo = new System.Windows.Forms.GroupBox();
            rbMujer = new System.Windows.Forms.RadioButton();
            rbHombre = new System.Windows.Forms.RadioButton();
            gbEdad = new System.Windows.Forms.GroupBox();
            label2 = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            tbViejo = new System.Windows.Forms.TextBox();
            tbJoven = new System.Windows.Forms.TextBox();
            gbPais = new System.Windows.Forms.GroupBox();
            tbPais = new System.Windows.Forms.TextBox();
            gbImportarPorCampo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)proveedorDeError).BeginInit();
            gbFecha.SuspendLayout();
            gbAlfabeto.SuspendLayout();
            gbTipoRejilla.SuspendLayout();
            gbCampos.SuspendLayout();
            gbBotones.SuspendLayout();
            gbInformacion.SuspendLayout();
            gbDeporte.SuspendLayout();
            gbSexo.SuspendLayout();
            gbEdad.SuspendLayout();
            gbPais.SuspendLayout();
            SuspendLayout();
            // 
            // rbNombre
            // 
            resources.ApplyResources(rbNombre, "rbNombre");
            rbNombre.Name = "rbNombre";
            rbNombre.UseVisualStyleBackColor = true;
            rbNombre.CheckedChanged += ImportarCAmpo_ChekedChanged;
            rbNombre.Validated += CajaTexto_Validated;
            // 
            // gbImportarPorCampo
            // 
            gbImportarPorCampo.Controls.Add(rbPais);
            gbImportarPorCampo.Controls.Add(rbSexo);
            gbImportarPorCampo.Controls.Add(rbEdad);
            gbImportarPorCampo.Controls.Add(rbDP);
            gbImportarPorCampo.Controls.Add(rbPosicion);
            gbImportarPorCampo.Controls.Add(rbDeporte);
            gbImportarPorCampo.Controls.Add(rbFecha);
            gbImportarPorCampo.Controls.Add(rbTipoRejilla);
            gbImportarPorCampo.Controls.Add(rbApellido);
            gbImportarPorCampo.Controls.Add(rbNombre);
            resources.ApplyResources(gbImportarPorCampo, "gbImportarPorCampo");
            gbImportarPorCampo.Name = "gbImportarPorCampo";
            gbImportarPorCampo.TabStop = false;
            gbImportarPorCampo.Leave += gbImportarPorCampo_Leave;
            // 
            // rbPais
            // 
            resources.ApplyResources(rbPais, "rbPais");
            rbPais.Name = "rbPais";
            rbPais.TabStop = true;
            rbPais.UseVisualStyleBackColor = true;
            rbPais.CheckedChanged += ImportarCAmpo_ChekedChanged;
            // 
            // rbSexo
            // 
            resources.ApplyResources(rbSexo, "rbSexo");
            rbSexo.Name = "rbSexo";
            rbSexo.TabStop = true;
            rbSexo.UseVisualStyleBackColor = true;
            rbSexo.CheckedChanged += ImportarCAmpo_ChekedChanged;
            // 
            // rbEdad
            // 
            resources.ApplyResources(rbEdad, "rbEdad");
            rbEdad.Name = "rbEdad";
            rbEdad.TabStop = true;
            rbEdad.UseVisualStyleBackColor = true;
            rbEdad.CheckedChanged += ImportarCAmpo_ChekedChanged;
            // 
            // rbDP
            // 
            resources.ApplyResources(rbDP, "rbDP");
            rbDP.Name = "rbDP";
            rbDP.TabStop = true;
            rbDP.UseVisualStyleBackColor = true;
            rbDP.CheckedChanged += ImportarCAmpo_ChekedChanged;
            rbDP.Validated += CajaTexto_Validated;
            // 
            // rbPosicion
            // 
            resources.ApplyResources(rbPosicion, "rbPosicion");
            rbPosicion.Name = "rbPosicion";
            rbPosicion.TabStop = true;
            rbPosicion.UseVisualStyleBackColor = true;
            rbPosicion.CheckedChanged += ImportarCAmpo_ChekedChanged;
            rbPosicion.Validated += CajaTexto_Validated;
            // 
            // rbDeporte
            // 
            resources.ApplyResources(rbDeporte, "rbDeporte");
            rbDeporte.Name = "rbDeporte";
            rbDeporte.TabStop = true;
            rbDeporte.UseVisualStyleBackColor = true;
            rbDeporte.CheckedChanged += ImportarCAmpo_ChekedChanged;
            rbDeporte.Validated += CajaTexto_Validated;
            // 
            // rbFecha
            // 
            resources.ApplyResources(rbFecha, "rbFecha");
            rbFecha.Name = "rbFecha";
            rbFecha.TabStop = true;
            rbFecha.UseVisualStyleBackColor = true;
            rbFecha.CheckedChanged += ImportarCAmpo_ChekedChanged;
            rbFecha.Validated += CajaTexto_Validated;
            // 
            // rbTipoRejilla
            // 
            resources.ApplyResources(rbTipoRejilla, "rbTipoRejilla");
            rbTipoRejilla.Name = "rbTipoRejilla";
            rbTipoRejilla.TabStop = true;
            rbTipoRejilla.UseVisualStyleBackColor = true;
            rbTipoRejilla.CheckedChanged += ImportarCAmpo_ChekedChanged;
            rbTipoRejilla.Validated += CajaTexto_Validated;
            // 
            // rbApellido
            // 
            resources.ApplyResources(rbApellido, "rbApellido");
            rbApellido.Name = "rbApellido";
            rbApellido.TabStop = true;
            rbApellido.UseVisualStyleBackColor = true;
            rbApellido.CheckedChanged += ImportarCAmpo_ChekedChanged;
            rbApellido.Validated += CajaTexto_Validated;
            // 
            // dateTimerPickerInicial
            // 
            resources.ApplyResources(dateTimerPickerInicial, "dateTimerPickerInicial");
            dateTimerPickerInicial.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            dateTimerPickerInicial.MaxDate = new DateTime(2009, 11, 26, 0, 0, 0, 0);
            dateTimerPickerInicial.MinDate = new DateTime(2009, 10, 1, 0, 0, 0, 0);
            dateTimerPickerInicial.Name = "dateTimerPickerInicial";
            dateTimerPickerInicial.Value = new DateTime(2009, 11, 1, 0, 0, 0, 0);
            dateTimerPickerInicial.ValueChanged += dateTimerPickerInicial_ValueChanged;
            // 
            // dateTimePickerFinal
            // 
            dateTimePickerFinal.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            resources.ApplyResources(dateTimePickerFinal, "dateTimePickerFinal");
            dateTimePickerFinal.MaxDate = new DateTime(2009, 11, 26, 0, 0, 0, 0);
            dateTimePickerFinal.MinDate = new DateTime(2000, 1, 1, 0, 0, 0, 0);
            dateTimePickerFinal.Name = "dateTimePickerFinal";
            dateTimePickerFinal.Value = new DateTime(2009, 11, 20, 0, 0, 0, 0);
            dateTimePickerFinal.ValueChanged += dateTimePickerFinal_ValueChanged;
            // 
            // etFechaInicio
            // 
            resources.ApplyResources(etFechaInicio, "etFechaInicio");
            etFechaInicio.Name = "etFechaInicio";
            // 
            // etFechaFin
            // 
            resources.ApplyResources(etFechaFin, "etFechaFin");
            etFechaFin.Name = "etFechaFin";
            // 
            // tbInicio
            // 
            tbInicio.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            resources.ApplyResources(tbInicio, "tbInicio");
            tbInicio.Name = "tbInicio";
            tbInicio.KeyPress += tbSoloLetras_KeyPress;
            tbInicio.KeyUp += tbInicio_KeyUp;
            tbInicio.Validating += tbInicio_Validating;
            tbInicio.Validated += CajaTexto_Validated;
            // 
            // etInicio
            // 
            resources.ApplyResources(etInicio, "etInicio");
            etInicio.Name = "etInicio";
            // 
            // etFinal
            // 
            resources.ApplyResources(etFinal, "etFinal");
            etFinal.Name = "etFinal";
            // 
            // tbFinal
            // 
            tbFinal.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            resources.ApplyResources(tbFinal, "tbFinal");
            tbFinal.Name = "tbFinal";
            tbFinal.KeyPress += tbSoloLetras_KeyPress;
            tbFinal.KeyUp += tbFinal_KeyUp;
            tbFinal.Validating += tbFinal_Validating;
            tbFinal.Validated += CajaTexto_Validated;
            // 
            // proveedorDeError
            // 
            proveedorDeError.ContainerControl = this;
            // 
            // gbFecha
            // 
            gbFecha.Controls.Add(etFechaInicio);
            gbFecha.Controls.Add(dateTimerPickerInicial);
            gbFecha.Controls.Add(dateTimePickerFinal);
            gbFecha.Controls.Add(etFechaFin);
            resources.ApplyResources(gbFecha, "gbFecha");
            gbFecha.Name = "gbFecha";
            gbFecha.TabStop = false;
            // 
            // gbAlfabeto
            // 
            gbAlfabeto.Controls.Add(etInicio);
            gbAlfabeto.Controls.Add(tbInicio);
            gbAlfabeto.Controls.Add(etFinal);
            gbAlfabeto.Controls.Add(tbFinal);
            resources.ApplyResources(gbAlfabeto, "gbAlfabeto");
            gbAlfabeto.Name = "gbAlfabeto";
            gbAlfabeto.TabStop = false;
            // 
            // gbTipoRejilla
            // 
            gbTipoRejilla.Controls.Add(rbWingdingsExportar);
            gbTipoRejilla.Controls.Add(rbAbecedarioExportar);
            gbTipoRejilla.Controls.Add(rbImagenesExportar);
            gbTipoRejilla.Controls.Add(rbLetrasExportar);
            gbTipoRejilla.Controls.Add(rbColoresExportar);
            gbTipoRejilla.Controls.Add(rbNumericaExportar);
            resources.ApplyResources(gbTipoRejilla, "gbTipoRejilla");
            gbTipoRejilla.Name = "gbTipoRejilla";
            gbTipoRejilla.TabStop = false;
            // 
            // rbWingdingsExportar
            // 
            resources.ApplyResources(rbWingdingsExportar, "rbWingdingsExportar");
            rbWingdingsExportar.Name = "rbWingdingsExportar";
            rbWingdingsExportar.UseVisualStyleBackColor = true;
            // 
            // rbAbecedarioExportar
            // 
            resources.ApplyResources(rbAbecedarioExportar, "rbAbecedarioExportar");
            rbAbecedarioExportar.Name = "rbAbecedarioExportar";
            rbAbecedarioExportar.UseVisualStyleBackColor = true;
            // 
            // rbImagenesExportar
            // 
            resources.ApplyResources(rbImagenesExportar, "rbImagenesExportar");
            rbImagenesExportar.Name = "rbImagenesExportar";
            rbImagenesExportar.UseVisualStyleBackColor = true;
            // 
            // rbLetrasExportar
            // 
            resources.ApplyResources(rbLetrasExportar, "rbLetrasExportar");
            rbLetrasExportar.Name = "rbLetrasExportar";
            rbLetrasExportar.UseVisualStyleBackColor = true;
            // 
            // rbColoresExportar
            // 
            resources.ApplyResources(rbColoresExportar, "rbColoresExportar");
            rbColoresExportar.Name = "rbColoresExportar";
            rbColoresExportar.UseVisualStyleBackColor = true;
            // 
            // rbNumericaExportar
            // 
            resources.ApplyResources(rbNumericaExportar, "rbNumericaExportar");
            rbNumericaExportar.Checked = true;
            rbNumericaExportar.Name = "rbNumericaExportar";
            rbNumericaExportar.TabStop = true;
            rbNumericaExportar.UseVisualStyleBackColor = true;
            rbNumericaExportar.CheckedChanged += porTipoChekedChanged;
            // 
            // btExportar
            // 
            resources.ApplyResources(btExportar, "btExportar");
            btExportar.Name = "btExportar";
            btExportar.UseVisualStyleBackColor = true;
            btExportar.Click += btExportar2_Click;
            // 
            // btCancelar
            // 
            btCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(btCancelar, "btCancelar");
            btCancelar.Name = "btCancelar";
            btCancelar.UseVisualStyleBackColor = true;
            // 
            // btSelec
            // 
            resources.ApplyResources(btSelec, "btSelec");
            btSelec.Name = "btSelec";
            btSelec.UseVisualStyleBackColor = true;
            btSelec.Click += btSelec_Click;
            btSelec.Enter += btSelec_Enter;
            btSelec.Leave += btSelec_Leave;
            // 
            // btNoSelec
            // 
            resources.ApplyResources(btNoSelec, "btNoSelec");
            btNoSelec.Name = "btNoSelec";
            btNoSelec.UseVisualStyleBackColor = true;
            btNoSelec.Click += btNoSelec_Click;
            // 
            // gbCampos
            // 
            gbCampos.Controls.Add(etLatencia);
            gbCampos.Controls.Add(cbPorcentajes);
            gbCampos.Controls.Add(cbEmparejar);
            gbCampos.Controls.Add(cbLateralidad);
            gbCampos.Controls.Add(cbPais);
            gbCampos.Controls.Add(cbSexo);
            gbCampos.Controls.Add(cbEdad);
            gbCampos.Controls.Add(cbPosicion);
            gbCampos.Controls.Add(cbDeporte);
            gbCampos.Controls.Add(cbAciertoFallos);
            gbCampos.Controls.Add(cbArribaAbajo);
            gbCampos.Controls.Add(cbMetronomo);
            gbCampos.Controls.Add(cbTodos);
            gbCampos.Controls.Add(cbTipoBoton);
            gbCampos.Controls.Add(cbMusica);
            gbCampos.Controls.Add(cbObservaciones);
            gbCampos.Controls.Add(cbTiemposGrafica);
            gbCampos.Controls.Add(cbGrafica);
            gbCampos.Controls.Add(cbGrosor);
            gbCampos.Controls.Add(cbDistraccionLinea);
            gbCampos.Controls.Add(cbAleatorio);
            gbCampos.Controls.Add(cbTiempoLimite);
            gbCampos.Controls.Add(cbTipoRejilla);
            gbCampos.Controls.Add(cbFecha);
            gbCampos.Controls.Add(cbApellidos);
            gbCampos.Controls.Add(cbNombre);
            resources.ApplyResources(gbCampos, "gbCampos");
            gbCampos.Name = "gbCampos";
            gbCampos.TabStop = false;
            gbCampos.Enter += gbCampos_Enter;
            // 
            // etLatencia
            // 
            resources.ApplyResources(etLatencia, "etLatencia");
            etLatencia.ForeColor = System.Drawing.Color.FromArgb(255, 128, 0);
            etLatencia.Name = "etLatencia";
            // 
            // cbPorcentajes
            // 
            resources.ApplyResources(cbPorcentajes, "cbPorcentajes");
            cbPorcentajes.ForeColor = System.Drawing.Color.FromArgb(255, 128, 0);
            cbPorcentajes.Name = "cbPorcentajes";
            cbPorcentajes.UseVisualStyleBackColor = true;
            cbPorcentajes.CheckedChanged += campo_ChekedChanged;
            // 
            // cbEmparejar
            // 
            resources.ApplyResources(cbEmparejar, "cbEmparejar");
            cbEmparejar.ForeColor = System.Drawing.Color.FromArgb(255, 128, 0);
            cbEmparejar.Name = "cbEmparejar";
            cbEmparejar.UseVisualStyleBackColor = true;
            cbEmparejar.CheckedChanged += campo_ChekedChanged;
            // 
            // cbLateralidad
            // 
            resources.ApplyResources(cbLateralidad, "cbLateralidad");
            cbLateralidad.Name = "cbLateralidad";
            cbLateralidad.UseVisualStyleBackColor = true;
            cbLateralidad.CheckedChanged += campo_ChekedChanged;
            cbLateralidad.Validated += Campos_Validated;
            // 
            // cbPais
            // 
            resources.ApplyResources(cbPais, "cbPais");
            cbPais.Name = "cbPais";
            cbPais.UseVisualStyleBackColor = true;
            cbPais.CheckedChanged += campo_ChekedChanged;
            cbPais.Validated += Campos_Validated;
            // 
            // cbSexo
            // 
            resources.ApplyResources(cbSexo, "cbSexo");
            cbSexo.Name = "cbSexo";
            cbSexo.UseVisualStyleBackColor = true;
            cbSexo.CheckedChanged += campo_ChekedChanged;
            cbSexo.Validated += Campos_Validated;
            // 
            // cbEdad
            // 
            resources.ApplyResources(cbEdad, "cbEdad");
            cbEdad.Name = "cbEdad";
            cbEdad.UseVisualStyleBackColor = true;
            cbEdad.CheckedChanged += campo_ChekedChanged;
            cbEdad.Validated += Campos_Validated;
            // 
            // cbPosicion
            // 
            resources.ApplyResources(cbPosicion, "cbPosicion");
            cbPosicion.Name = "cbPosicion";
            cbPosicion.UseVisualStyleBackColor = true;
            cbPosicion.CheckedChanged += campo_ChekedChanged;
            cbPosicion.Validated += Campos_Validated;
            // 
            // cbDeporte
            // 
            resources.ApplyResources(cbDeporte, "cbDeporte");
            cbDeporte.Name = "cbDeporte";
            cbDeporte.UseVisualStyleBackColor = true;
            cbDeporte.CheckedChanged += campo_ChekedChanged;
            cbDeporte.Validated += Campos_Validated;
            // 
            // cbAciertoFallos
            // 
            resources.ApplyResources(cbAciertoFallos, "cbAciertoFallos");
            cbAciertoFallos.Name = "cbAciertoFallos";
            cbAciertoFallos.UseVisualStyleBackColor = true;
            cbAciertoFallos.CheckedChanged += campo_ChekedChanged;
            cbAciertoFallos.Validated += Campos_Validated;
            // 
            // cbArribaAbajo
            // 
            resources.ApplyResources(cbArribaAbajo, "cbArribaAbajo");
            cbArribaAbajo.Name = "cbArribaAbajo";
            cbArribaAbajo.UseVisualStyleBackColor = true;
            cbArribaAbajo.CheckedChanged += campo_ChekedChanged;
            cbArribaAbajo.Validated += Campos_Validated;
            // 
            // cbMetronomo
            // 
            resources.ApplyResources(cbMetronomo, "cbMetronomo");
            cbMetronomo.Name = "cbMetronomo";
            cbMetronomo.UseVisualStyleBackColor = true;
            cbMetronomo.CheckedChanged += campo_ChekedChanged;
            cbMetronomo.Validated += Campos_Validated;
            // 
            // cbTodos
            // 
            resources.ApplyResources(cbTodos, "cbTodos");
            cbTodos.ForeColor = System.Drawing.Color.Red;
            cbTodos.Name = "cbTodos";
            cbTodos.UseVisualStyleBackColor = true;
            cbTodos.CheckedChanged += camposCheckedChanged;
            cbTodos.Validated += Campos_Validated;
            // 
            // cbTipoBoton
            // 
            resources.ApplyResources(cbTipoBoton, "cbTipoBoton");
            cbTipoBoton.Name = "cbTipoBoton";
            cbTipoBoton.UseVisualStyleBackColor = true;
            cbTipoBoton.CheckedChanged += campo_ChekedChanged;
            cbTipoBoton.Validated += Campos_Validated;
            // 
            // cbMusica
            // 
            resources.ApplyResources(cbMusica, "cbMusica");
            cbMusica.Name = "cbMusica";
            cbMusica.UseVisualStyleBackColor = true;
            cbMusica.CheckedChanged += campo_ChekedChanged;
            cbMusica.Validated += Campos_Validated;
            // 
            // cbObservaciones
            // 
            resources.ApplyResources(cbObservaciones, "cbObservaciones");
            cbObservaciones.Name = "cbObservaciones";
            cbObservaciones.UseVisualStyleBackColor = true;
            cbObservaciones.CheckedChanged += campo_ChekedChanged;
            cbObservaciones.Validated += Campos_Validated;
            // 
            // cbTiemposGrafica
            // 
            resources.ApplyResources(cbTiemposGrafica, "cbTiemposGrafica");
            cbTiemposGrafica.Name = "cbTiemposGrafica";
            cbTiemposGrafica.UseVisualStyleBackColor = true;
            cbTiemposGrafica.CheckedChanged += campo_ChekedChanged;
            cbTiemposGrafica.Validated += Campos_Validated;
            // 
            // cbGrafica
            // 
            resources.ApplyResources(cbGrafica, "cbGrafica");
            cbGrafica.Name = "cbGrafica";
            cbGrafica.UseVisualStyleBackColor = true;
            cbGrafica.CheckedChanged += campo_ChekedChanged;
            cbGrafica.Validated += Campos_Validated;
            // 
            // cbGrosor
            // 
            resources.ApplyResources(cbGrosor, "cbGrosor");
            cbGrosor.Name = "cbGrosor";
            cbGrosor.UseVisualStyleBackColor = true;
            cbGrosor.CheckedChanged += campo_ChekedChanged;
            cbGrosor.Validated += Campos_Validated;
            // 
            // cbDistraccionLinea
            // 
            resources.ApplyResources(cbDistraccionLinea, "cbDistraccionLinea");
            cbDistraccionLinea.Name = "cbDistraccionLinea";
            cbDistraccionLinea.UseVisualStyleBackColor = true;
            cbDistraccionLinea.CheckedChanged += campo_ChekedChanged;
            cbDistraccionLinea.Validated += Campos_Validated;
            // 
            // cbAleatorio
            // 
            resources.ApplyResources(cbAleatorio, "cbAleatorio");
            cbAleatorio.Name = "cbAleatorio";
            cbAleatorio.UseVisualStyleBackColor = true;
            cbAleatorio.CheckedChanged += campo_ChekedChanged;
            cbAleatorio.Validated += Campos_Validated;
            // 
            // cbTiempoLimite
            // 
            resources.ApplyResources(cbTiempoLimite, "cbTiempoLimite");
            cbTiempoLimite.Name = "cbTiempoLimite";
            cbTiempoLimite.UseVisualStyleBackColor = true;
            cbTiempoLimite.CheckedChanged += campo_ChekedChanged;
            cbTiempoLimite.Validated += Campos_Validated;
            // 
            // cbTipoRejilla
            // 
            resources.ApplyResources(cbTipoRejilla, "cbTipoRejilla");
            cbTipoRejilla.Name = "cbTipoRejilla";
            cbTipoRejilla.UseVisualStyleBackColor = true;
            cbTipoRejilla.CheckedChanged += campo_ChekedChanged;
            cbTipoRejilla.Validated += Campos_Validated;
            // 
            // cbFecha
            // 
            resources.ApplyResources(cbFecha, "cbFecha");
            cbFecha.Name = "cbFecha";
            cbFecha.UseVisualStyleBackColor = true;
            cbFecha.CheckedChanged += campo_ChekedChanged;
            cbFecha.Validated += Campos_Validated;
            // 
            // cbApellidos
            // 
            resources.ApplyResources(cbApellidos, "cbApellidos");
            cbApellidos.Name = "cbApellidos";
            cbApellidos.UseVisualStyleBackColor = true;
            cbApellidos.CheckedChanged += campo_ChekedChanged;
            cbApellidos.Validated += Campos_Validated;
            // 
            // cbNombre
            // 
            resources.ApplyResources(cbNombre, "cbNombre");
            cbNombre.Name = "cbNombre";
            cbNombre.UseVisualStyleBackColor = true;
            cbNombre.CheckedChanged += campo_ChekedChanged;
            cbNombre.Validated += Campos_Validated;
            // 
            // etInformacion
            // 
            resources.ApplyResources(etInformacion, "etInformacion");
            etInformacion.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            etInformacion.Name = "etInformacion";
            // 
            // gbBotones
            // 
            gbBotones.Controls.Add(btExportar);
            gbBotones.Controls.Add(btCancelar);
            gbBotones.Controls.Add(btNoSelec);
            gbBotones.Controls.Add(btSelec);
            resources.ApplyResources(gbBotones, "gbBotones");
            gbBotones.Name = "gbBotones";
            gbBotones.TabStop = false;
            // 
            // gbInformacion
            // 
            gbInformacion.Controls.Add(etInformacion);
            resources.ApplyResources(gbInformacion, "gbInformacion");
            gbInformacion.Name = "gbInformacion";
            gbInformacion.TabStop = false;
            // 
            // gbDeporte
            // 
            gbDeporte.Controls.Add(etDeporte);
            gbDeporte.Controls.Add(tbDeporte);
            gbDeporte.Controls.Add(etPosicion);
            gbDeporte.Controls.Add(tbPosicion);
            resources.ApplyResources(gbDeporte, "gbDeporte");
            gbDeporte.Name = "gbDeporte";
            gbDeporte.TabStop = false;
            // 
            // etDeporte
            // 
            resources.ApplyResources(etDeporte, "etDeporte");
            etDeporte.Name = "etDeporte";
            // 
            // tbDeporte
            // 
            tbDeporte.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            resources.ApplyResources(tbDeporte, "tbDeporte");
            tbDeporte.Name = "tbDeporte";
            tbDeporte.KeyPress += tbSoloLetras_KeyPress;
            tbDeporte.KeyUp += tbDeporte_KeyUp;
            // 
            // etPosicion
            // 
            resources.ApplyResources(etPosicion, "etPosicion");
            etPosicion.Name = "etPosicion";
            // 
            // tbPosicion
            // 
            tbPosicion.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            resources.ApplyResources(tbPosicion, "tbPosicion");
            tbPosicion.Name = "tbPosicion";
            tbPosicion.KeyPress += tbSoloLetras_KeyPress;
            tbPosicion.KeyUp += tbPosicion_KeyUp;
            // 
            // gbSexo
            // 
            gbSexo.Controls.Add(rbMujer);
            gbSexo.Controls.Add(rbHombre);
            resources.ApplyResources(gbSexo, "gbSexo");
            gbSexo.Name = "gbSexo";
            gbSexo.TabStop = false;
            // 
            // rbMujer
            // 
            resources.ApplyResources(rbMujer, "rbMujer");
            rbMujer.Checked = true;
            rbMujer.Name = "rbMujer";
            rbMujer.TabStop = true;
            rbMujer.UseVisualStyleBackColor = true;
            rbMujer.CheckedChanged += Sexo_CheckedChanged;
            // 
            // rbHombre
            // 
            resources.ApplyResources(rbHombre, "rbHombre");
            rbHombre.Name = "rbHombre";
            rbHombre.UseVisualStyleBackColor = true;
            rbHombre.CheckedChanged += Sexo_CheckedChanged;
            // 
            // gbEdad
            // 
            gbEdad.Controls.Add(label2);
            gbEdad.Controls.Add(label1);
            gbEdad.Controls.Add(tbViejo);
            gbEdad.Controls.Add(tbJoven);
            resources.ApplyResources(gbEdad, "gbEdad");
            gbEdad.Name = "gbEdad";
            gbEdad.TabStop = false;
            // 
            // label2
            // 
            resources.ApplyResources(label2, "label2");
            label2.Name = "label2";
            // 
            // label1
            // 
            resources.ApplyResources(label1, "label1");
            label1.Name = "label1";
            // 
            // tbViejo
            // 
            resources.ApplyResources(tbViejo, "tbViejo");
            tbViejo.Name = "tbViejo";
            tbViejo.KeyPress += tbSoloNumeros_KeyPress;
            tbViejo.KeyUp += tbViejo_KeyUp;
            tbViejo.Validating += tbViejo_Validating;
            tbViejo.Validated += CajaTexto_Validated;
            // 
            // tbJoven
            // 
            resources.ApplyResources(tbJoven, "tbJoven");
            tbJoven.Name = "tbJoven";
            tbJoven.KeyPress += tbSoloNumeros_KeyPress;
            tbJoven.KeyUp += tbJoven_KeyUp;
            tbJoven.Validating += tbJoven_Validating;
            tbJoven.Validated += CajaTexto_Validated;
            // 
            // gbPais
            // 
            gbPais.Controls.Add(tbPais);
            resources.ApplyResources(gbPais, "gbPais");
            gbPais.Name = "gbPais";
            gbPais.TabStop = false;
            // 
            // tbPais
            // 
            tbPais.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            resources.ApplyResources(tbPais, "tbPais");
            tbPais.Name = "tbPais";
            tbPais.KeyPress += tbSoloLetras_KeyPress;
            tbPais.KeyUp += tbPais_KeyUp;
            // 
            // ExportarDatos
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(gbPais);
            Controls.Add(gbEdad);
            Controls.Add(gbSexo);
            Controls.Add(gbDeporte);
            Controls.Add(gbInformacion);
            Controls.Add(gbBotones);
            Controls.Add(gbTipoRejilla);
            Controls.Add(gbAlfabeto);
            Controls.Add(gbFecha);
            Controls.Add(gbImportarPorCampo);
            Controls.Add(gbCampos);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "ExportarDatos";
            gbImportarPorCampo.ResumeLayout(false);
            gbImportarPorCampo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)proveedorDeError).EndInit();
            gbFecha.ResumeLayout(false);
            gbFecha.PerformLayout();
            gbAlfabeto.ResumeLayout(false);
            gbAlfabeto.PerformLayout();
            gbTipoRejilla.ResumeLayout(false);
            gbTipoRejilla.PerformLayout();
            gbCampos.ResumeLayout(false);
            gbCampos.PerformLayout();
            gbBotones.ResumeLayout(false);
            gbInformacion.ResumeLayout(false);
            gbInformacion.PerformLayout();
            gbDeporte.ResumeLayout(false);
            gbDeporte.PerformLayout();
            gbSexo.ResumeLayout(false);
            gbSexo.PerformLayout();
            gbEdad.ResumeLayout(false);
            gbEdad.PerformLayout();
            gbPais.ResumeLayout(false);
            gbPais.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.RadioButton rbNombre;
        private System.Windows.Forms.GroupBox gbImportarPorCampo;
        private System.Windows.Forms.RadioButton rbFecha;
        private System.Windows.Forms.RadioButton rbTipoRejilla;
        private System.Windows.Forms.RadioButton rbApellido;
        private System.Windows.Forms.DateTimePicker dateTimerPickerInicial;
        private System.Windows.Forms.DateTimePicker dateTimePickerFinal;
        private System.Windows.Forms.Label etFechaInicio;
        private System.Windows.Forms.Label etFechaFin;
        private System.Windows.Forms.TextBox tbInicio;
        private System.Windows.Forms.Label etInicio;
        private System.Windows.Forms.Label etFinal;
        private System.Windows.Forms.TextBox tbFinal;
        private System.Windows.Forms.ErrorProvider proveedorDeError;
        private System.Windows.Forms.GroupBox gbTipoRejilla;
        private System.Windows.Forms.GroupBox gbAlfabeto;
        private System.Windows.Forms.GroupBox gbFecha;
        private System.Windows.Forms.RadioButton rbWingdingsExportar;
        private System.Windows.Forms.RadioButton rbAbecedarioExportar;
        private System.Windows.Forms.RadioButton rbImagenesExportar;
        private System.Windows.Forms.RadioButton rbLetrasExportar;
        private System.Windows.Forms.RadioButton rbColoresExportar;
        private System.Windows.Forms.RadioButton rbNumericaExportar;
        private System.Windows.Forms.Button btNoSelec;
        private System.Windows.Forms.Button btSelec;
        private System.Windows.Forms.Button btCancelar;
        private System.Windows.Forms.Button btExportar;
        private System.Windows.Forms.GroupBox gbCampos;
        private System.Windows.Forms.CheckBox cbTodos;
        private System.Windows.Forms.CheckBox cbTipoBoton;
        
        private System.Windows.Forms.CheckBox cbMusica;
        private System.Windows.Forms.CheckBox cbObservaciones;
        private System.Windows.Forms.CheckBox cbTiemposGrafica;
        private System.Windows.Forms.CheckBox cbGrafica;
        private System.Windows.Forms.CheckBox cbGrosor;
        private System.Windows.Forms.CheckBox cbDistraccionLinea;
        private System.Windows.Forms.CheckBox cbAleatorio;
        private System.Windows.Forms.CheckBox cbTiempoLimite;
        private System.Windows.Forms.CheckBox cbTipoRejilla;
        private System.Windows.Forms.CheckBox cbFecha;
        private System.Windows.Forms.CheckBox cbApellidos;
        private System.Windows.Forms.CheckBox cbNombre;
        private System.Windows.Forms.CheckBox cbMetronomo;
        private System.Windows.Forms.GroupBox gbInformacion;
        private System.Windows.Forms.GroupBox gbBotones;
        private System.Windows.Forms.Label etInformacion;
        private System.Windows.Forms.CheckBox cbArribaAbajo;
        private System.Windows.Forms.CheckBox cbAciertoFallos;
        private System.Windows.Forms.CheckBox cbPosicion;
        private System.Windows.Forms.CheckBox cbDeporte;
        private System.Windows.Forms.RadioButton rbPosicion;
        private System.Windows.Forms.RadioButton rbDeporte;
        private System.Windows.Forms.RadioButton rbDP;
        private System.Windows.Forms.GroupBox gbDeporte;
        private System.Windows.Forms.Label etDeporte;
        private System.Windows.Forms.TextBox tbDeporte;
        private System.Windows.Forms.Label etPosicion;
        private System.Windows.Forms.TextBox tbPosicion;
        private System.Windows.Forms.CheckBox cbPais;
        private System.Windows.Forms.CheckBox cbSexo;
        private System.Windows.Forms.CheckBox cbEdad;
        private System.Windows.Forms.GroupBox gbSexo;
        private System.Windows.Forms.GroupBox gbEdad;
        private System.Windows.Forms.GroupBox gbPais;
        private System.Windows.Forms.RadioButton rbPais;
        private System.Windows.Forms.RadioButton rbSexo;
        private System.Windows.Forms.RadioButton rbEdad;
        private System.Windows.Forms.TextBox tbPais;
        private System.Windows.Forms.RadioButton rbMujer;
        private System.Windows.Forms.RadioButton rbHombre;
        private System.Windows.Forms.TextBox tbViejo;
        private System.Windows.Forms.TextBox tbJoven;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox cbLateralidad;
        private System.Windows.Forms.CheckBox cbPorcentajes;
        private System.Windows.Forms.CheckBox cbEmparejar;
        private System.Windows.Forms.Label etLatencia;
    }
}