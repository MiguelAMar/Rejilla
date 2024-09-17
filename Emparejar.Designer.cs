namespace demorejilla
{
    partial class Emparejar
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Emparejar));
            gbEmparejar = new System.Windows.Forms.GroupBox();
            rbInversa = new System.Windows.Forms.RadioButton();
            rbAleatoria = new System.Windows.Forms.RadioButton();
            rbSecuencial = new System.Windows.Forms.RadioButton();
            gbTemporizador = new System.Windows.Forms.GroupBox();
            label1 = new System.Windows.Forms.Label();
            tbTiempo = new System.Windows.Forms.TextBox();
            gbPosicion = new System.Windows.Forms.GroupBox();
            etInformacion = new System.Windows.Forms.Label();
            btDerInf = new System.Windows.Forms.Button();
            panel1 = new System.Windows.Forms.Panel();
            etRejilla = new System.Windows.Forms.Label();
            btA = new System.Windows.Forms.Button();
            btIzqInf = new System.Windows.Forms.Button();
            btDerCentro = new System.Windows.Forms.Button();
            btIzqCentro = new System.Windows.Forms.Button();
            btDerSup = new System.Windows.Forms.Button();
            btIzqSup = new System.Windows.Forms.Button();
            btAceptar = new System.Windows.Forms.Button();
            btCancelar = new System.Windows.Forms.Button();
            gbEmparejar.SuspendLayout();
            gbTemporizador.SuspendLayout();
            gbPosicion.SuspendLayout();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // gbEmparejar
            // 
            gbEmparejar.Controls.Add(rbInversa);
            gbEmparejar.Controls.Add(rbAleatoria);
            gbEmparejar.Controls.Add(rbSecuencial);
            resources.ApplyResources(gbEmparejar, "gbEmparejar");
            gbEmparejar.Name = "gbEmparejar";
            gbEmparejar.TabStop = false;
            // 
            // rbInversa
            // 
            resources.ApplyResources(rbInversa, "rbInversa");
            rbInversa.Name = "rbInversa";
            rbInversa.UseVisualStyleBackColor = true;
            rbInversa.CheckedChanged += rbSecuencial_CheckedChanged;
            // 
            // rbAleatoria
            // 
            resources.ApplyResources(rbAleatoria, "rbAleatoria");
            rbAleatoria.Name = "rbAleatoria";
            rbAleatoria.UseVisualStyleBackColor = true;
            rbAleatoria.CheckedChanged += rbSecuencial_CheckedChanged;
            // 
            // rbSecuencial
            // 
            resources.ApplyResources(rbSecuencial, "rbSecuencial");
            rbSecuencial.Checked = true;
            rbSecuencial.Name = "rbSecuencial";
            rbSecuencial.TabStop = true;
            rbSecuencial.UseVisualStyleBackColor = true;
            rbSecuencial.CheckedChanged += rbSecuencial_CheckedChanged;
            // 
            // gbTemporizador
            // 
            gbTemporizador.Controls.Add(label1);
            gbTemporizador.Controls.Add(tbTiempo);
            resources.ApplyResources(gbTemporizador, "gbTemporizador");
            gbTemporizador.Name = "gbTemporizador";
            gbTemporizador.TabStop = false;
            // 
            // label1
            // 
            resources.ApplyResources(label1, "label1");
            label1.Name = "label1";
            // 
            // tbTiempo
            // 
            resources.ApplyResources(tbTiempo, "tbTiempo");
            tbTiempo.Name = "tbTiempo";
            tbTiempo.KeyPress += tbSoloNumeros_KeyPress;
            // 
            // gbPosicion
            // 
            gbPosicion.Controls.Add(etInformacion);
            gbPosicion.Controls.Add(btDerInf);
            gbPosicion.Controls.Add(panel1);
            gbPosicion.Controls.Add(btIzqInf);
            gbPosicion.Controls.Add(btDerCentro);
            gbPosicion.Controls.Add(btIzqCentro);
            gbPosicion.Controls.Add(btDerSup);
            gbPosicion.Controls.Add(btIzqSup);
            resources.ApplyResources(gbPosicion, "gbPosicion");
            gbPosicion.Name = "gbPosicion";
            gbPosicion.TabStop = false;
            // 
            // etInformacion
            // 
            resources.ApplyResources(etInformacion, "etInformacion");
            etInformacion.Name = "etInformacion";
            // 
            // btDerInf
            // 
            resources.ApplyResources(btDerInf, "btDerInf");
            btDerInf.Name = "btDerInf";
            btDerInf.UseVisualStyleBackColor = true;
            btDerInf.Click += btDerInf_Click;
            // 
            // panel1
            // 
            panel1.BackColor = System.Drawing.SystemColors.InactiveCaption;
            panel1.Controls.Add(etRejilla);
            panel1.Controls.Add(btA);
            resources.ApplyResources(panel1, "panel1");
            panel1.Name = "panel1";
            // 
            // etRejilla
            // 
            resources.ApplyResources(etRejilla, "etRejilla");
            etRejilla.Name = "etRejilla";
            // 
            // btA
            // 
            resources.ApplyResources(btA, "btA");
            btA.Name = "btA";
            btA.UseVisualStyleBackColor = true;
            btA.Click += btA_Click;
            // 
            // btIzqInf
            // 
            resources.ApplyResources(btIzqInf, "btIzqInf");
            btIzqInf.Name = "btIzqInf";
            btIzqInf.UseVisualStyleBackColor = true;
            btIzqInf.Click += btIzqInf_Click;
            // 
            // btDerCentro
            // 
            resources.ApplyResources(btDerCentro, "btDerCentro");
            btDerCentro.Name = "btDerCentro";
            btDerCentro.UseVisualStyleBackColor = true;
            btDerCentro.Click += btDerCentro_Click;
            // 
            // btIzqCentro
            // 
            resources.ApplyResources(btIzqCentro, "btIzqCentro");
            btIzqCentro.Name = "btIzqCentro";
            btIzqCentro.UseVisualStyleBackColor = true;
            btIzqCentro.Click += btIzqCentro_Click;
            // 
            // btDerSup
            // 
            resources.ApplyResources(btDerSup, "btDerSup");
            btDerSup.Name = "btDerSup";
            btDerSup.UseVisualStyleBackColor = true;
            btDerSup.Click += btDerSup_Click;
            // 
            // btIzqSup
            // 
            resources.ApplyResources(btIzqSup, "btIzqSup");
            btIzqSup.Name = "btIzqSup";
            btIzqSup.UseVisualStyleBackColor = true;
            btIzqSup.Click += btIzqSup_Click;
            // 
            // btAceptar
            // 
            btAceptar.DialogResult = System.Windows.Forms.DialogResult.OK;
            resources.ApplyResources(btAceptar, "btAceptar");
            btAceptar.Name = "btAceptar";
            btAceptar.UseVisualStyleBackColor = true;
            btAceptar.Click += btAceptar_Click;
            // 
            // btCancelar
            // 
            btCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(btCancelar, "btCancelar");
            btCancelar.Name = "btCancelar";
            btCancelar.UseVisualStyleBackColor = true;
            // 
            // Emparejar
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(btCancelar);
            Controls.Add(btAceptar);
            Controls.Add(gbPosicion);
            Controls.Add(gbTemporizador);
            Controls.Add(gbEmparejar);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "Emparejar";
            gbEmparejar.ResumeLayout(false);
            gbEmparejar.PerformLayout();
            gbTemporizador.ResumeLayout(false);
            gbTemporizador.PerformLayout();
            gbPosicion.ResumeLayout(false);
            gbPosicion.PerformLayout();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.GroupBox gbEmparejar;
        private System.Windows.Forms.RadioButton rbInversa;
        private System.Windows.Forms.RadioButton rbAleatoria;
        private System.Windows.Forms.RadioButton rbSecuencial;
        private System.Windows.Forms.GroupBox gbTemporizador;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbTiempo;
        private System.Windows.Forms.GroupBox gbPosicion;
        private System.Windows.Forms.Button btAceptar;
        private System.Windows.Forms.Button btCancelar;
        private System.Windows.Forms.Label etInformacion;
        private System.Windows.Forms.Button btDerInf;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label etRejilla;
        private System.Windows.Forms.Button btA;
        private System.Windows.Forms.Button btIzqInf;
        private System.Windows.Forms.Button btDerCentro;
        private System.Windows.Forms.Button btIzqCentro;
        private System.Windows.Forms.Button btDerSup;
        private System.Windows.Forms.Button btIzqSup;
    }
}