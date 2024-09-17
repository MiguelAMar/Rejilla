namespace demorejilla
{
    partial class Observaciones
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Observaciones));
            tbObservaciones = new System.Windows.Forms.TextBox();
            btAceptar = new System.Windows.Forms.Button();
            btLimpiar = new System.Windows.Forms.Button();
            btGuardarObservaciones = new System.Windows.Forms.Button();
            SuspendLayout();
            // 
            // tbObservaciones
            // 
            tbObservaciones.BackColor = System.Drawing.Color.FromArgb(192, 255, 255);
            resources.ApplyResources(tbObservaciones, "tbObservaciones");
            tbObservaciones.Name = "tbObservaciones";
            tbObservaciones.KeyPress += tbCaracterPermitidos_KeyPress;
            // 
            // btAceptar
            // 
            btAceptar.DialogResult = System.Windows.Forms.DialogResult.OK;
            resources.ApplyResources(btAceptar, "btAceptar");
            btAceptar.Name = "btAceptar";
            btAceptar.UseVisualStyleBackColor = true;
            btAceptar.Click += btAceptar_Click;
            // 
            // btLimpiar
            // 
            resources.ApplyResources(btLimpiar, "btLimpiar");
            btLimpiar.Name = "btLimpiar";
            btLimpiar.UseVisualStyleBackColor = true;
            btLimpiar.Click += btLimpiar_Click;
            // 
            // btGuardarObservaciones
            // 
            btGuardarObservaciones.DialogResult = System.Windows.Forms.DialogResult.OK;
            resources.ApplyResources(btGuardarObservaciones, "btGuardarObservaciones");
            btGuardarObservaciones.Name = "btGuardarObservaciones";
            btGuardarObservaciones.UseVisualStyleBackColor = true;
            btGuardarObservaciones.Click += btGuardarObservaciones_Click;
            // 
            // Observaciones
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(btGuardarObservaciones);
            Controls.Add(btLimpiar);
            Controls.Add(btAceptar);
            Controls.Add(tbObservaciones);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            MaximizeBox = false;
            Name = "Observaciones";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.TextBox tbObservaciones;
        private System.Windows.Forms.Button btAceptar;
        private System.Windows.Forms.Button btLimpiar;
        private System.Windows.Forms.Button btGuardarObservaciones;

    }
}