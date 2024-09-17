namespace demorejilla
{
    partial class ProgramarColores
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProgramarColores));
            rbIntercambiarTexto = new System.Windows.Forms.RadioButton();
            rbIntercambiarColor = new System.Windows.Forms.RadioButton();
            rbIntercambiarTodo = new System.Windows.Forms.RadioButton();
            rbTacharTexto = new System.Windows.Forms.RadioButton();
            rbTacharTextoColor = new System.Windows.Forms.RadioButton();
            gbIntercambiar = new System.Windows.Forms.GroupBox();
            gbTachar = new System.Windows.Forms.GroupBox();
            rbTacharColor = new System.Windows.Forms.RadioButton();
            rbTacharNada = new System.Windows.Forms.RadioButton();
            btAceptar = new System.Windows.Forms.Button();
            btCancelar = new System.Windows.Forms.Button();
            gbIntercambiar.SuspendLayout();
            gbTachar.SuspendLayout();
            SuspendLayout();
            // 
            // rbIntercambiarTexto
            // 
            resources.ApplyResources(rbIntercambiarTexto, "rbIntercambiarTexto");
            rbIntercambiarTexto.Name = "rbIntercambiarTexto";
            rbIntercambiarTexto.UseVisualStyleBackColor = true;
            rbIntercambiarTexto.CheckedChanged += rbIntercambiar_CheckedChanged;
            // 
            // rbIntercambiarColor
            // 
            resources.ApplyResources(rbIntercambiarColor, "rbIntercambiarColor");
            rbIntercambiarColor.Name = "rbIntercambiarColor";
            rbIntercambiarColor.UseVisualStyleBackColor = true;
            rbIntercambiarColor.CheckedChanged += rbIntercambiar_CheckedChanged;
            // 
            // rbIntercambiarTodo
            // 
            resources.ApplyResources(rbIntercambiarTodo, "rbIntercambiarTodo");
            rbIntercambiarTodo.Name = "rbIntercambiarTodo";
            rbIntercambiarTodo.UseVisualStyleBackColor = true;
            rbIntercambiarTodo.CheckedChanged += rbIntercambiar_CheckedChanged;
            // 
            // rbTacharTexto
            // 
            resources.ApplyResources(rbTacharTexto, "rbTacharTexto");
            rbTacharTexto.Name = "rbTacharTexto";
            rbTacharTexto.UseVisualStyleBackColor = true;
            rbTacharTexto.CheckedChanged += rbTachar_ChekedChanged;
            // 
            // rbTacharTextoColor
            // 
            resources.ApplyResources(rbTacharTextoColor, "rbTacharTextoColor");
            rbTacharTextoColor.Name = "rbTacharTextoColor";
            rbTacharTextoColor.UseVisualStyleBackColor = true;
            rbTacharTextoColor.CheckedChanged += rbTachar_ChekedChanged;
            // 
            // gbIntercambiar
            // 
            resources.ApplyResources(gbIntercambiar, "gbIntercambiar");
            gbIntercambiar.Controls.Add(rbIntercambiarTodo);
            gbIntercambiar.Controls.Add(rbIntercambiarTexto);
            gbIntercambiar.Controls.Add(rbIntercambiarColor);
            gbIntercambiar.Name = "gbIntercambiar";
            gbIntercambiar.TabStop = false;
            // 
            // gbTachar
            // 
            resources.ApplyResources(gbTachar, "gbTachar");
            gbTachar.Controls.Add(rbTacharColor);
            gbTachar.Controls.Add(rbTacharNada);
            gbTachar.Controls.Add(rbTacharTexto);
            gbTachar.Controls.Add(rbTacharTextoColor);
            gbTachar.Name = "gbTachar";
            gbTachar.TabStop = false;
            // 
            // rbTacharColor
            // 
            resources.ApplyResources(rbTacharColor, "rbTacharColor");
            rbTacharColor.Name = "rbTacharColor";
            rbTacharColor.UseVisualStyleBackColor = true;
            rbTacharColor.CheckedChanged += rbTachar_ChekedChanged;
            // 
            // rbTacharNada
            // 
            resources.ApplyResources(rbTacharNada, "rbTacharNada");
            rbTacharNada.Name = "rbTacharNada";
            rbTacharNada.UseVisualStyleBackColor = true;
            rbTacharNada.CheckedChanged += rbTachar_ChekedChanged;
            // 
            // btAceptar
            // 
            resources.ApplyResources(btAceptar, "btAceptar");
            btAceptar.DialogResult = System.Windows.Forms.DialogResult.OK;
            btAceptar.Name = "btAceptar";
            btAceptar.UseVisualStyleBackColor = true;
            // 
            // btCancelar
            // 
            resources.ApplyResources(btCancelar, "btCancelar");
            btCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            btCancelar.Name = "btCancelar";
            btCancelar.UseVisualStyleBackColor = true;
            // 
            // ProgramarColores
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(btCancelar);
            Controls.Add(btAceptar);
            Controls.Add(gbTachar);
            Controls.Add(gbIntercambiar);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "ProgramarColores";
            gbIntercambiar.ResumeLayout(false);
            gbIntercambiar.PerformLayout();
            gbTachar.ResumeLayout(false);
            gbTachar.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.RadioButton rbIntercambiarTexto;
        private System.Windows.Forms.RadioButton rbIntercambiarColor;
        private System.Windows.Forms.RadioButton rbIntercambiarTodo;
        private System.Windows.Forms.RadioButton rbTacharTexto;
        private System.Windows.Forms.RadioButton rbTacharTextoColor;
        private System.Windows.Forms.GroupBox gbIntercambiar;
        private System.Windows.Forms.GroupBox gbTachar;
        private System.Windows.Forms.Button btAceptar;
        private System.Windows.Forms.Button btCancelar;
        private System.Windows.Forms.RadioButton rbTacharNada;
        private System.Windows.Forms.RadioButton rbTacharColor;
    }
}