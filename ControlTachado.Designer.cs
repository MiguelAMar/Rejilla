namespace demorejilla
{
    partial class ControlTachado
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ControlTachado));
            this.etOrdenTachado = new System.Windows.Forms.Label();
            this.tbordenTachado = new System.Windows.Forms.TextBox();
            this.btAceptar = new System.Windows.Forms.Button();
            this.etTituloAciertos = new System.Windows.Forms.Label();
            this.etTituloFallos = new System.Windows.Forms.Label();
            this.etAciertos = new System.Windows.Forms.Label();
            this.etFallos = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // etOrdenTachado
            // 
            this.etOrdenTachado.AccessibleDescription = null;
            this.etOrdenTachado.AccessibleName = null;
            resources.ApplyResources(this.etOrdenTachado, "etOrdenTachado");
            this.etOrdenTachado.Name = "etOrdenTachado";
            // 
            // tbordenTachado
            // 
            this.tbordenTachado.AccessibleDescription = null;
            this.tbordenTachado.AccessibleName = null;
            resources.ApplyResources(this.tbordenTachado, "tbordenTachado");
            this.tbordenTachado.BackgroundImage = null;
            this.tbordenTachado.Name = "tbordenTachado";
            this.tbordenTachado.ReadOnly = true;
            // 
            // btAceptar
            // 
            this.btAceptar.AccessibleDescription = null;
            this.btAceptar.AccessibleName = null;
            resources.ApplyResources(this.btAceptar, "btAceptar");
            this.btAceptar.BackgroundImage = null;
            this.btAceptar.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btAceptar.Font = null;
            this.btAceptar.Name = "btAceptar";
            this.btAceptar.UseVisualStyleBackColor = true;
            // 
            // etTituloAciertos
            // 
            this.etTituloAciertos.AccessibleDescription = null;
            this.etTituloAciertos.AccessibleName = null;
            resources.ApplyResources(this.etTituloAciertos, "etTituloAciertos");
            this.etTituloAciertos.Name = "etTituloAciertos";
            // 
            // etTituloFallos
            // 
            this.etTituloFallos.AccessibleDescription = null;
            this.etTituloFallos.AccessibleName = null;
            resources.ApplyResources(this.etTituloFallos, "etTituloFallos");
            this.etTituloFallos.Name = "etTituloFallos";
            // 
            // etAciertos
            // 
            this.etAciertos.AccessibleDescription = null;
            this.etAciertos.AccessibleName = null;
            resources.ApplyResources(this.etAciertos, "etAciertos");
            this.etAciertos.Name = "etAciertos";
            // 
            // etFallos
            // 
            this.etFallos.AccessibleDescription = null;
            this.etFallos.AccessibleName = null;
            resources.ApplyResources(this.etFallos, "etFallos");
            this.etFallos.Name = "etFallos";
            // 
            // ControlTachado
            // 
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = null;
            this.Controls.Add(this.etFallos);
            this.Controls.Add(this.etAciertos);
            this.Controls.Add(this.etTituloFallos);
            this.Controls.Add(this.etTituloAciertos);
            this.Controls.Add(this.btAceptar);
            this.Controls.Add(this.tbordenTachado);
            this.Controls.Add(this.etOrdenTachado);
            this.Font = null;
            this.Icon = null;
            this.Name = "ControlTachado";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label etOrdenTachado;
        private System.Windows.Forms.TextBox tbordenTachado;
        private System.Windows.Forms.Button btAceptar;
        private System.Windows.Forms.Label etTituloAciertos;
        private System.Windows.Forms.Label etTituloFallos;
        private System.Windows.Forms.Label etAciertos;
        private System.Windows.Forms.Label etFallos;





    }
}