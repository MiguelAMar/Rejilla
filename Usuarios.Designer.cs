namespace demorejilla
{
    partial class Usuarios
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Usuarios));
            dgvUsuarios = new System.Windows.Forms.DataGridView();
            etUsuario = new System.Windows.Forms.Label();
            tbNombre = new System.Windows.Forms.TextBox();
            btCargarUsuario = new System.Windows.Forms.Button();
            btCancelar = new System.Windows.Forms.Button();
            etApellidos = new System.Windows.Forms.Label();
            tbApellidos = new System.Windows.Forms.TextBox();
            tbCodigo = new System.Windows.Forms.TextBox();
            etCodigo = new System.Windows.Forms.Label();
            tbDeportes = new System.Windows.Forms.TextBox();
            etDeportes = new System.Windows.Forms.Label();
            etEdad = new System.Windows.Forms.Label();
            tbEdad = new System.Windows.Forms.TextBox();
            tbPosicion = new System.Windows.Forms.TextBox();
            etPosicion = new System.Windows.Forms.Label();
            tbPais = new System.Windows.Forms.TextBox();
            etPais = new System.Windows.Forms.Label();
            btActualizar = new System.Windows.Forms.Button();
            btInsertarUsuario = new System.Windows.Forms.Button();
            gbLateralidad = new System.Windows.Forms.GroupBox();
            rbDerecha = new System.Windows.Forms.RadioButton();
            rbIzquierda = new System.Windows.Forms.RadioButton();
            gbGénero = new System.Windows.Forms.GroupBox();
            rbFemenino = new System.Windows.Forms.RadioButton();
            rbHombre = new System.Windows.Forms.RadioButton();
            gbRejillas = new System.Windows.Forms.GroupBox();
            dgvRejillas = new System.Windows.Forms.DataGridView();
            btCargarRejillasUsuario = new System.Windows.Forms.Button();
            btExportarRejillasExcel = new System.Windows.Forms.Button();
            btComparar = new System.Windows.Forms.Button();
            btAceptar = new System.Windows.Forms.Button();
            gbUsuarios = new System.Windows.Forms.GroupBox();
            tabUserRejillas = new System.Windows.Forms.TabControl();
            tabPageUsuarios = new System.Windows.Forms.TabPage();
            label2 = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            rbTodasLasFilas = new System.Windows.Forms.RadioButton();
            rbPosicion = new System.Windows.Forms.RadioButton();
            rbPaís = new System.Windows.Forms.RadioButton();
            rbEdad = new System.Windows.Forms.RadioButton();
            rbDeportes = new System.Windows.Forms.RadioButton();
            rbGenero = new System.Windows.Forms.RadioButton();
            rbLateralidad = new System.Windows.Forms.RadioButton();
            rbApellidos = new System.Windows.Forms.RadioButton();
            tbBuscarPor = new System.Windows.Forms.TextBox();
            rbNombre = new System.Windows.Forms.RadioButton();
            tabPageRejillas = new System.Windows.Forms.TabPage();
            rbTodasRejillasGenero = new System.Windows.Forms.RadioButton();
            gbTodasRejillasGenero = new System.Windows.Forms.GroupBox();
            rbTodasRejillasMujer = new System.Windows.Forms.RadioButton();
            rbTodasRejillasHombre = new System.Windows.Forms.RadioButton();
            btBuscarDosCampos = new System.Windows.Forms.Button();
            etFechaInicio = new System.Windows.Forms.Label();
            etFechaFin = new System.Windows.Forms.Label();
            dateTimePickerFin = new System.Windows.Forms.DateTimePicker();
            dateTimePickerInicio = new System.Windows.Forms.DateTimePicker();
            etFin = new System.Windows.Forms.Label();
            etInicio = new System.Windows.Forms.Label();
            rbTodasRejillasTodaTabla = new System.Windows.Forms.RadioButton();
            rbTodasRejillasPais = new System.Windows.Forms.RadioButton();
            rbTodasRejillasEdad = new System.Windows.Forms.RadioButton();
            rbTodasRejillasDP = new System.Windows.Forms.RadioButton();
            rbTodasRejillasPosicion = new System.Windows.Forms.RadioButton();
            rbTodasRejillasFecha = new System.Windows.Forms.RadioButton();
            rbTodasRejillasTipoRejilla = new System.Windows.Forms.RadioButton();
            rbTodasRejillasDeporte = new System.Windows.Forms.RadioButton();
            tbFin = new System.Windows.Forms.TextBox();
            tbInicio = new System.Windows.Forms.TextBox();
            gbBotones = new System.Windows.Forms.GroupBox();
            btCargarRejilla = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)dgvUsuarios).BeginInit();
            gbLateralidad.SuspendLayout();
            gbGénero.SuspendLayout();
            gbRejillas.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvRejillas).BeginInit();
            gbUsuarios.SuspendLayout();
            tabUserRejillas.SuspendLayout();
            tabPageUsuarios.SuspendLayout();
            tabPageRejillas.SuspendLayout();
            gbTodasRejillasGenero.SuspendLayout();
            gbBotones.SuspendLayout();
            SuspendLayout();
            // 
            // dgvUsuarios
            // 
            dgvUsuarios.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            resources.ApplyResources(dgvUsuarios, "dgvUsuarios");
            dgvUsuarios.Name = "dgvUsuarios";
            dgvUsuarios.RowHeaderMouseClick += dgvUsuarios_RowHeaderMouseClick;
            dgvUsuarios.RowHeaderMouseDoubleClick += dgvUsuarios_RowHeaderMouseDoubleClick;
            // 
            // etUsuario
            // 
            resources.ApplyResources(etUsuario, "etUsuario");
            etUsuario.Name = "etUsuario";
            // 
            // tbNombre
            // 
            resources.ApplyResources(tbNombre, "tbNombre");
            tbNombre.Name = "tbNombre";
            // 
            // btCargarUsuario
            // 
            resources.ApplyResources(btCargarUsuario, "btCargarUsuario");
            btCargarUsuario.Name = "btCargarUsuario";
            btCargarUsuario.UseVisualStyleBackColor = true;
            btCargarUsuario.Click += btCargarUsuario_Click;
            // 
            // btCancelar
            // 
            resources.ApplyResources(btCancelar, "btCancelar");
            btCancelar.Name = "btCancelar";
            btCancelar.UseVisualStyleBackColor = true;
            btCancelar.Click += btCancelar_Click;
            // 
            // etApellidos
            // 
            resources.ApplyResources(etApellidos, "etApellidos");
            etApellidos.Name = "etApellidos";
            // 
            // tbApellidos
            // 
            resources.ApplyResources(tbApellidos, "tbApellidos");
            tbApellidos.Name = "tbApellidos";
            // 
            // tbCodigo
            // 
            resources.ApplyResources(tbCodigo, "tbCodigo");
            tbCodigo.Name = "tbCodigo";
            tbCodigo.ReadOnly = true;
            // 
            // etCodigo
            // 
            resources.ApplyResources(etCodigo, "etCodigo");
            etCodigo.Name = "etCodigo";
            // 
            // tbDeportes
            // 
            resources.ApplyResources(tbDeportes, "tbDeportes");
            tbDeportes.Name = "tbDeportes";
            // 
            // etDeportes
            // 
            resources.ApplyResources(etDeportes, "etDeportes");
            etDeportes.Name = "etDeportes";
            // 
            // etEdad
            // 
            resources.ApplyResources(etEdad, "etEdad");
            etEdad.Name = "etEdad";
            // 
            // tbEdad
            // 
            resources.ApplyResources(tbEdad, "tbEdad");
            tbEdad.Name = "tbEdad";
            // 
            // tbPosicion
            // 
            resources.ApplyResources(tbPosicion, "tbPosicion");
            tbPosicion.Name = "tbPosicion";
            // 
            // etPosicion
            // 
            resources.ApplyResources(etPosicion, "etPosicion");
            etPosicion.Name = "etPosicion";
            // 
            // tbPais
            // 
            resources.ApplyResources(tbPais, "tbPais");
            tbPais.Name = "tbPais";
            // 
            // etPais
            // 
            resources.ApplyResources(etPais, "etPais");
            etPais.Name = "etPais";
            // 
            // btActualizar
            // 
            resources.ApplyResources(btActualizar, "btActualizar");
            btActualizar.Name = "btActualizar";
            btActualizar.UseVisualStyleBackColor = true;
            btActualizar.Click += btActualizar_Click;
            // 
            // btInsertarUsuario
            // 
            resources.ApplyResources(btInsertarUsuario, "btInsertarUsuario");
            btInsertarUsuario.Name = "btInsertarUsuario";
            btInsertarUsuario.UseVisualStyleBackColor = true;
            btInsertarUsuario.Click += btInsertarUsuario_Click;
            // 
            // gbLateralidad
            // 
            gbLateralidad.Controls.Add(rbDerecha);
            gbLateralidad.Controls.Add(rbIzquierda);
            resources.ApplyResources(gbLateralidad, "gbLateralidad");
            gbLateralidad.Name = "gbLateralidad";
            gbLateralidad.TabStop = false;
            // 
            // rbDerecha
            // 
            resources.ApplyResources(rbDerecha, "rbDerecha");
            rbDerecha.Checked = true;
            rbDerecha.Name = "rbDerecha";
            rbDerecha.TabStop = true;
            rbDerecha.UseVisualStyleBackColor = true;
            // 
            // rbIzquierda
            // 
            resources.ApplyResources(rbIzquierda, "rbIzquierda");
            rbIzquierda.Name = "rbIzquierda";
            rbIzquierda.UseVisualStyleBackColor = true;
            // 
            // gbGénero
            // 
            gbGénero.Controls.Add(rbFemenino);
            gbGénero.Controls.Add(rbHombre);
            resources.ApplyResources(gbGénero, "gbGénero");
            gbGénero.Name = "gbGénero";
            gbGénero.TabStop = false;
            // 
            // rbFemenino
            // 
            resources.ApplyResources(rbFemenino, "rbFemenino");
            rbFemenino.Name = "rbFemenino";
            rbFemenino.UseVisualStyleBackColor = true;
            // 
            // rbHombre
            // 
            resources.ApplyResources(rbHombre, "rbHombre");
            rbHombre.Checked = true;
            rbHombre.Name = "rbHombre";
            rbHombre.TabStop = true;
            rbHombre.UseVisualStyleBackColor = true;
            // 
            // gbRejillas
            // 
            gbRejillas.Controls.Add(dgvRejillas);
            resources.ApplyResources(gbRejillas, "gbRejillas");
            gbRejillas.Name = "gbRejillas";
            gbRejillas.TabStop = false;
            // 
            // dgvRejillas
            // 
            dgvRejillas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            resources.ApplyResources(dgvRejillas, "dgvRejillas");
            dgvRejillas.Name = "dgvRejillas";
            dgvRejillas.RowHeaderMouseClick += dgvRejillas_RowHeaderMouseClick;
            // 
            // btCargarRejillasUsuario
            // 
            resources.ApplyResources(btCargarRejillasUsuario, "btCargarRejillasUsuario");
            btCargarRejillasUsuario.Name = "btCargarRejillasUsuario";
            btCargarRejillasUsuario.UseVisualStyleBackColor = true;
            btCargarRejillasUsuario.Click += btCargarRejillasUsuario_Click;
            // 
            // btExportarRejillasExcel
            // 
            resources.ApplyResources(btExportarRejillasExcel, "btExportarRejillasExcel");
            btExportarRejillasExcel.Name = "btExportarRejillasExcel";
            btExportarRejillasExcel.UseVisualStyleBackColor = true;
            btExportarRejillasExcel.Click += btExportarRejillasExcel_Click;
            // 
            // btComparar
            // 
            resources.ApplyResources(btComparar, "btComparar");
            btComparar.Name = "btComparar";
            btComparar.UseVisualStyleBackColor = true;
            btComparar.Click += btComparar_Click;
            // 
            // btAceptar
            // 
            resources.ApplyResources(btAceptar, "btAceptar");
            btAceptar.Name = "btAceptar";
            btAceptar.UseVisualStyleBackColor = true;
            btAceptar.Click += btAceptar_Click;
            // 
            // gbUsuarios
            // 
            gbUsuarios.Controls.Add(dgvUsuarios);
            resources.ApplyResources(gbUsuarios, "gbUsuarios");
            gbUsuarios.Name = "gbUsuarios";
            gbUsuarios.TabStop = false;
            // 
            // tabUserRejillas
            // 
            tabUserRejillas.Controls.Add(tabPageUsuarios);
            tabUserRejillas.Controls.Add(tabPageRejillas);
            resources.ApplyResources(tabUserRejillas, "tabUserRejillas");
            tabUserRejillas.Name = "tabUserRejillas";
            tabUserRejillas.SelectedIndex = 0;
            tabUserRejillas.MouseClick += tabUserRejillas_MouseClick;
            // 
            // tabPageUsuarios
            // 
            tabPageUsuarios.Controls.Add(label2);
            tabPageUsuarios.Controls.Add(label1);
            tabPageUsuarios.Controls.Add(rbTodasLasFilas);
            tabPageUsuarios.Controls.Add(rbPosicion);
            tabPageUsuarios.Controls.Add(rbPaís);
            tabPageUsuarios.Controls.Add(rbEdad);
            tabPageUsuarios.Controls.Add(rbDeportes);
            tabPageUsuarios.Controls.Add(rbGenero);
            tabPageUsuarios.Controls.Add(rbLateralidad);
            tabPageUsuarios.Controls.Add(rbApellidos);
            tabPageUsuarios.Controls.Add(tbBuscarPor);
            tabPageUsuarios.Controls.Add(rbNombre);
            resources.ApplyResources(tabPageUsuarios, "tabPageUsuarios");
            tabPageUsuarios.Name = "tabPageUsuarios";
            tabPageUsuarios.UseVisualStyleBackColor = true;
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
            // rbTodasLasFilas
            // 
            resources.ApplyResources(rbTodasLasFilas, "rbTodasLasFilas");
            rbTodasLasFilas.Checked = true;
            rbTodasLasFilas.Name = "rbTodasLasFilas";
            rbTodasLasFilas.TabStop = true;
            rbTodasLasFilas.UseVisualStyleBackColor = true;
            rbTodasLasFilas.CheckedChanged += rbTodasLasFilas_CheckedChanged;
            // 
            // rbPosicion
            // 
            resources.ApplyResources(rbPosicion, "rbPosicion");
            rbPosicion.Name = "rbPosicion";
            rbPosicion.UseVisualStyleBackColor = true;
            rbPosicion.CheckedChanged += rbTodasLasFilas_CheckedChanged;
            // 
            // rbPaís
            // 
            resources.ApplyResources(rbPaís, "rbPaís");
            rbPaís.Name = "rbPaís";
            rbPaís.UseVisualStyleBackColor = true;
            rbPaís.CheckedChanged += rbTodasLasFilas_CheckedChanged;
            // 
            // rbEdad
            // 
            resources.ApplyResources(rbEdad, "rbEdad");
            rbEdad.Name = "rbEdad";
            rbEdad.UseVisualStyleBackColor = true;
            rbEdad.CheckedChanged += rbTodasLasFilas_CheckedChanged;
            // 
            // rbDeportes
            // 
            resources.ApplyResources(rbDeportes, "rbDeportes");
            rbDeportes.Name = "rbDeportes";
            rbDeportes.UseVisualStyleBackColor = true;
            rbDeportes.CheckedChanged += rbTodasLasFilas_CheckedChanged;
            // 
            // rbGenero
            // 
            resources.ApplyResources(rbGenero, "rbGenero");
            rbGenero.Name = "rbGenero";
            rbGenero.UseVisualStyleBackColor = true;
            rbGenero.CheckedChanged += rbTodasLasFilas_CheckedChanged;
            // 
            // rbLateralidad
            // 
            resources.ApplyResources(rbLateralidad, "rbLateralidad");
            rbLateralidad.Name = "rbLateralidad";
            rbLateralidad.UseVisualStyleBackColor = true;
            rbLateralidad.CheckedChanged += rbTodasLasFilas_CheckedChanged;
            // 
            // rbApellidos
            // 
            resources.ApplyResources(rbApellidos, "rbApellidos");
            rbApellidos.Name = "rbApellidos";
            rbApellidos.UseVisualStyleBackColor = true;
            rbApellidos.CheckedChanged += rbTodasLasFilas_CheckedChanged;
            // 
            // tbBuscarPor
            // 
            resources.ApplyResources(tbBuscarPor, "tbBuscarPor");
            tbBuscarPor.Name = "tbBuscarPor";
            tbBuscarPor.TextChanged += tbBuscarPor_TextChanged;
            // 
            // rbNombre
            // 
            resources.ApplyResources(rbNombre, "rbNombre");
            rbNombre.Name = "rbNombre";
            rbNombre.UseVisualStyleBackColor = true;
            rbNombre.CheckedChanged += rbTodasLasFilas_CheckedChanged;
            // 
            // tabPageRejillas
            // 
            tabPageRejillas.Controls.Add(rbTodasRejillasGenero);
            tabPageRejillas.Controls.Add(gbTodasRejillasGenero);
            tabPageRejillas.Controls.Add(btBuscarDosCampos);
            tabPageRejillas.Controls.Add(etFechaInicio);
            tabPageRejillas.Controls.Add(etFechaFin);
            tabPageRejillas.Controls.Add(dateTimePickerFin);
            tabPageRejillas.Controls.Add(dateTimePickerInicio);
            tabPageRejillas.Controls.Add(etFin);
            tabPageRejillas.Controls.Add(etInicio);
            tabPageRejillas.Controls.Add(rbTodasRejillasTodaTabla);
            tabPageRejillas.Controls.Add(rbTodasRejillasPais);
            tabPageRejillas.Controls.Add(rbTodasRejillasEdad);
            tabPageRejillas.Controls.Add(rbTodasRejillasDP);
            tabPageRejillas.Controls.Add(rbTodasRejillasPosicion);
            tabPageRejillas.Controls.Add(rbTodasRejillasFecha);
            tabPageRejillas.Controls.Add(rbTodasRejillasTipoRejilla);
            tabPageRejillas.Controls.Add(rbTodasRejillasDeporte);
            tabPageRejillas.Controls.Add(tbFin);
            tabPageRejillas.Controls.Add(tbInicio);
            resources.ApplyResources(tabPageRejillas, "tabPageRejillas");
            tabPageRejillas.Name = "tabPageRejillas";
            tabPageRejillas.UseVisualStyleBackColor = true;
            // 
            // rbTodasRejillasGenero
            // 
            resources.ApplyResources(rbTodasRejillasGenero, "rbTodasRejillasGenero");
            rbTodasRejillasGenero.Name = "rbTodasRejillasGenero";
            rbTodasRejillasGenero.TabStop = true;
            rbTodasRejillasGenero.UseVisualStyleBackColor = true;
            rbTodasRejillasGenero.CheckedChanged += rbTodasRejillasTodaTabla_CheckedChanged;
            // 
            // gbTodasRejillasGenero
            // 
            gbTodasRejillasGenero.Controls.Add(rbTodasRejillasMujer);
            gbTodasRejillasGenero.Controls.Add(rbTodasRejillasHombre);
            resources.ApplyResources(gbTodasRejillasGenero, "gbTodasRejillasGenero");
            gbTodasRejillasGenero.Name = "gbTodasRejillasGenero";
            gbTodasRejillasGenero.TabStop = false;
            // 
            // rbTodasRejillasMujer
            // 
            resources.ApplyResources(rbTodasRejillasMujer, "rbTodasRejillasMujer");
            rbTodasRejillasMujer.Name = "rbTodasRejillasMujer";
            rbTodasRejillasMujer.UseVisualStyleBackColor = true;
            // 
            // rbTodasRejillasHombre
            // 
            resources.ApplyResources(rbTodasRejillasHombre, "rbTodasRejillasHombre");
            rbTodasRejillasHombre.Name = "rbTodasRejillasHombre";
            rbTodasRejillasHombre.UseVisualStyleBackColor = true;
            // 
            // btBuscarDosCampos
            // 
            resources.ApplyResources(btBuscarDosCampos, "btBuscarDosCampos");
            btBuscarDosCampos.Name = "btBuscarDosCampos";
            btBuscarDosCampos.UseVisualStyleBackColor = true;
            btBuscarDosCampos.Click += btBuscarDosCampos_Click;
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
            // dateTimePickerFin
            // 
            dateTimePickerFin.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            resources.ApplyResources(dateTimePickerFin, "dateTimePickerFin");
            dateTimePickerFin.Name = "dateTimePickerFin";
            dateTimePickerFin.Value = new System.DateTime(2024, 8, 27, 0, 0, 0, 0);
            // 
            // dateTimePickerInicio
            // 
            dateTimePickerInicio.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            resources.ApplyResources(dateTimePickerInicio, "dateTimePickerInicio");
            dateTimePickerInicio.Name = "dateTimePickerInicio";
            dateTimePickerInicio.Value = new System.DateTime(2024, 8, 27, 0, 0, 0, 0);
            // 
            // etFin
            // 
            resources.ApplyResources(etFin, "etFin");
            etFin.Name = "etFin";
            // 
            // etInicio
            // 
            resources.ApplyResources(etInicio, "etInicio");
            etInicio.Name = "etInicio";
            // 
            // rbTodasRejillasTodaTabla
            // 
            resources.ApplyResources(rbTodasRejillasTodaTabla, "rbTodasRejillasTodaTabla");
            rbTodasRejillasTodaTabla.Checked = true;
            rbTodasRejillasTodaTabla.Name = "rbTodasRejillasTodaTabla";
            rbTodasRejillasTodaTabla.TabStop = true;
            rbTodasRejillasTodaTabla.UseVisualStyleBackColor = true;
            rbTodasRejillasTodaTabla.CheckedChanged += rbTodasRejillasTodaTabla_CheckedChanged;
            // 
            // rbTodasRejillasPais
            // 
            resources.ApplyResources(rbTodasRejillasPais, "rbTodasRejillasPais");
            rbTodasRejillasPais.Name = "rbTodasRejillasPais";
            rbTodasRejillasPais.TabStop = true;
            rbTodasRejillasPais.UseVisualStyleBackColor = true;
            rbTodasRejillasPais.CheckedChanged += rbTodasRejillasTodaTabla_CheckedChanged;
            // 
            // rbTodasRejillasEdad
            // 
            resources.ApplyResources(rbTodasRejillasEdad, "rbTodasRejillasEdad");
            rbTodasRejillasEdad.Name = "rbTodasRejillasEdad";
            rbTodasRejillasEdad.TabStop = true;
            rbTodasRejillasEdad.UseVisualStyleBackColor = true;
            rbTodasRejillasEdad.CheckedChanged += rbTodasRejillasTodaTabla_CheckedChanged;
            // 
            // rbTodasRejillasDP
            // 
            resources.ApplyResources(rbTodasRejillasDP, "rbTodasRejillasDP");
            rbTodasRejillasDP.Name = "rbTodasRejillasDP";
            rbTodasRejillasDP.TabStop = true;
            rbTodasRejillasDP.UseVisualStyleBackColor = true;
            rbTodasRejillasDP.CheckedChanged += rbTodasRejillasTodaTabla_CheckedChanged;
            // 
            // rbTodasRejillasPosicion
            // 
            resources.ApplyResources(rbTodasRejillasPosicion, "rbTodasRejillasPosicion");
            rbTodasRejillasPosicion.Name = "rbTodasRejillasPosicion";
            rbTodasRejillasPosicion.TabStop = true;
            rbTodasRejillasPosicion.UseVisualStyleBackColor = true;
            rbTodasRejillasPosicion.CheckedChanged += rbTodasRejillasTodaTabla_CheckedChanged;
            // 
            // rbTodasRejillasFecha
            // 
            resources.ApplyResources(rbTodasRejillasFecha, "rbTodasRejillasFecha");
            rbTodasRejillasFecha.Name = "rbTodasRejillasFecha";
            rbTodasRejillasFecha.TabStop = true;
            rbTodasRejillasFecha.UseVisualStyleBackColor = true;
            rbTodasRejillasFecha.CheckedChanged += rbTodasRejillasTodaTabla_CheckedChanged;
            // 
            // rbTodasRejillasTipoRejilla
            // 
            resources.ApplyResources(rbTodasRejillasTipoRejilla, "rbTodasRejillasTipoRejilla");
            rbTodasRejillasTipoRejilla.Name = "rbTodasRejillasTipoRejilla";
            rbTodasRejillasTipoRejilla.TabStop = true;
            rbTodasRejillasTipoRejilla.UseVisualStyleBackColor = true;
            rbTodasRejillasTipoRejilla.CheckedChanged += rbTodasRejillasTodaTabla_CheckedChanged;
            // 
            // rbTodasRejillasDeporte
            // 
            resources.ApplyResources(rbTodasRejillasDeporte, "rbTodasRejillasDeporte");
            rbTodasRejillasDeporte.Name = "rbTodasRejillasDeporte";
            rbTodasRejillasDeporte.TabStop = true;
            rbTodasRejillasDeporte.UseVisualStyleBackColor = true;
            rbTodasRejillasDeporte.CheckedChanged += rbTodasRejillasTodaTabla_CheckedChanged;
            // 
            // tbFin
            // 
            resources.ApplyResources(tbFin, "tbFin");
            tbFin.Name = "tbFin";
            tbFin.TextChanged += tbInicio_TextChanged;
            // 
            // tbInicio
            // 
            resources.ApplyResources(tbInicio, "tbInicio");
            tbInicio.Name = "tbInicio";
            tbInicio.TextChanged += tbInicio_TextChanged;
            // 
            // gbBotones
            // 
            gbBotones.Controls.Add(btCargarRejilla);
            gbBotones.Controls.Add(btCargarRejillasUsuario);
            gbBotones.Controls.Add(btAceptar);
            gbBotones.Controls.Add(btCancelar);
            resources.ApplyResources(gbBotones, "gbBotones");
            gbBotones.Name = "gbBotones";
            gbBotones.TabStop = false;
            // 
            // btCargarRejilla
            // 
            resources.ApplyResources(btCargarRejilla, "btCargarRejilla");
            btCargarRejilla.Name = "btCargarRejilla";
            btCargarRejilla.UseVisualStyleBackColor = true;
            btCargarRejilla.Click += btCargarRejilla_Click;
            // 
            // Usuarios
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(gbBotones);
            Controls.Add(tabUserRejillas);
            Controls.Add(gbUsuarios);
            Controls.Add(btComparar);
            Controls.Add(btExportarRejillasExcel);
            Controls.Add(gbRejillas);
            Controls.Add(gbGénero);
            Controls.Add(gbLateralidad);
            Controls.Add(btInsertarUsuario);
            Controls.Add(btActualizar);
            Controls.Add(etEdad);
            Controls.Add(tbEdad);
            Controls.Add(tbPosicion);
            Controls.Add(etPosicion);
            Controls.Add(tbPais);
            Controls.Add(etPais);
            Controls.Add(tbDeportes);
            Controls.Add(etDeportes);
            Controls.Add(etCodigo);
            Controls.Add(tbCodigo);
            Controls.Add(tbApellidos);
            Controls.Add(etApellidos);
            Controls.Add(btCargarUsuario);
            Controls.Add(tbNombre);
            Controls.Add(etUsuario);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            MaximizeBox = false;
            Name = "Usuarios";
            ((System.ComponentModel.ISupportInitialize)dgvUsuarios).EndInit();
            gbLateralidad.ResumeLayout(false);
            gbLateralidad.PerformLayout();
            gbGénero.ResumeLayout(false);
            gbGénero.PerformLayout();
            gbRejillas.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvRejillas).EndInit();
            gbUsuarios.ResumeLayout(false);
            tabUserRejillas.ResumeLayout(false);
            tabPageUsuarios.ResumeLayout(false);
            tabPageUsuarios.PerformLayout();
            tabPageRejillas.ResumeLayout(false);
            tabPageRejillas.PerformLayout();
            gbTodasRejillasGenero.ResumeLayout(false);
            gbTodasRejillasGenero.PerformLayout();
            gbBotones.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.DataGridView dgvUsuarios;
        private System.Windows.Forms.Label etUsuario;
        private System.Windows.Forms.TextBox tbNombre;
        private System.Windows.Forms.Button btCargarUsuario;
        private System.Windows.Forms.Button btCancelar;
        private System.Windows.Forms.Label etApellidos;
        private System.Windows.Forms.TextBox tbApellidos;
        private System.Windows.Forms.TextBox tbCodigo;
        private System.Windows.Forms.Label etCodigo;
        private System.Windows.Forms.TextBox tbDeportes;
        private System.Windows.Forms.Label etDeportes;
        private System.Windows.Forms.Label etEdad;
        private System.Windows.Forms.TextBox tbEdad;
        private System.Windows.Forms.TextBox tbPosicion;
        private System.Windows.Forms.Label etPosicion;
        private System.Windows.Forms.TextBox tbPais;
        private System.Windows.Forms.Label etPais;
        private System.Windows.Forms.Button btActualizar;
        private System.Windows.Forms.Button btInsertarUsuario;
        private System.Windows.Forms.GroupBox gbLateralidad;
        private System.Windows.Forms.RadioButton rbIzquierda;
        private System.Windows.Forms.RadioButton rbDerecha;
        private System.Windows.Forms.GroupBox gbGénero;
        private System.Windows.Forms.RadioButton rbFemenino;
        private System.Windows.Forms.RadioButton rbHombre;
        private System.Windows.Forms.GroupBox gbRejillas;
        private System.Windows.Forms.DataGridView dgvRejillas;
        private System.Windows.Forms.Button btCargarRejillasUsuario;
        private System.Windows.Forms.Button btExportarRejillasExcel;
        private System.Windows.Forms.Button btComparar;
        private System.Windows.Forms.Button btAceptar;
        private System.Windows.Forms.GroupBox gbUsuarios;
        private System.Windows.Forms.TabControl tabUserRejillas;
        private System.Windows.Forms.TabPage tabPageUsuarios;
        private System.Windows.Forms.TabPage tabPageRejillas;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton rbTodasLasFilas;
        private System.Windows.Forms.RadioButton rbPosicion;
        private System.Windows.Forms.RadioButton rbPaís;
        private System.Windows.Forms.RadioButton rbEdad;
        private System.Windows.Forms.RadioButton rbDeportes;
        private System.Windows.Forms.RadioButton rbGenero;
        private System.Windows.Forms.RadioButton rbLateralidad;
        private System.Windows.Forms.RadioButton rbApellidos;
        private System.Windows.Forms.TextBox tbBuscarPor;
        private System.Windows.Forms.RadioButton rbNombre;
        private System.Windows.Forms.GroupBox gbBotones;
        private System.Windows.Forms.TextBox tbFin;
        private System.Windows.Forms.TextBox tbInicio;
        private System.Windows.Forms.RadioButton rbTodasRejillasTodaTabla;
        private System.Windows.Forms.RadioButton rbTodasRejillasPais;
        private System.Windows.Forms.RadioButton rbTodasRejillasEdad;
        private System.Windows.Forms.RadioButton rbTodasRejillasDP;
        private System.Windows.Forms.RadioButton rbTodasRejillasPosicion;
        private System.Windows.Forms.RadioButton rbTodasRejillasFecha;
        private System.Windows.Forms.RadioButton rbTodasRejillasTipoRejilla;
        private System.Windows.Forms.RadioButton rbTodasRejillasDeporte;
        private System.Windows.Forms.Label etFin;
        private System.Windows.Forms.Label etInicio;
        private System.Windows.Forms.DateTimePicker dateTimePickerInicio;
        private System.Windows.Forms.DateTimePicker dateTimePickerFin;
        private System.Windows.Forms.Label etFechaInicio;
        private System.Windows.Forms.Label etFechaFin;
        private System.Windows.Forms.GroupBox gbTodasRejillasGenero;
        private System.Windows.Forms.RadioButton rbTodasRejillasMujer;
        private System.Windows.Forms.RadioButton rbTodasRejillasHombre;
        private System.Windows.Forms.Button btBuscarDosCampos;
        private System.Windows.Forms.RadioButton rbTodasRejillasGenero;
        private System.Windows.Forms.Button btCargarRejilla;
    }
}