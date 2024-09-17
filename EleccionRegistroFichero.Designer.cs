namespace demorejilla
{
    partial class EleccionRegistroFichero
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EleccionRegistroFichero));
            this.lbRegistros = new System.Windows.Forms.ListBox();
            this.btAceptar = new System.Windows.Forms.Button();
            this.btCancelar = new System.Windows.Forms.Button();
            this.etRegistrosTamañoLista = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbRegistros
            // 
            this.lbRegistros.AccessibleDescription = null;
            this.lbRegistros.AccessibleName = null;
            resources.ApplyResources(this.lbRegistros, "lbRegistros");
            this.lbRegistros.BackgroundImage = null;
            this.lbRegistros.Font = null;
            this.lbRegistros.FormattingEnabled = true;
            this.lbRegistros.Name = "lbRegistros";
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
            // btCancelar
            // 
            this.btCancelar.AccessibleDescription = null;
            this.btCancelar.AccessibleName = null;
            resources.ApplyResources(this.btCancelar, "btCancelar");
            this.btCancelar.BackgroundImage = null;
            this.btCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancelar.Font = null;
            this.btCancelar.Name = "btCancelar";
            this.btCancelar.UseVisualStyleBackColor = true;
            // 
            // etRegistrosTamañoLista
            // 
            this.etRegistrosTamañoLista.AccessibleDescription = null;
            this.etRegistrosTamañoLista.AccessibleName = null;
            resources.ApplyResources(this.etRegistrosTamañoLista, "etRegistrosTamañoLista");
            this.etRegistrosTamañoLista.Font = null;
            this.etRegistrosTamañoLista.Name = "etRegistrosTamañoLista";
            // 
            // label1
            // 
            this.label1.AccessibleDescription = null;
            this.label1.AccessibleName = null;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Font = null;
            this.label1.Name = "label1";
            // 
            // label2
            // 
            this.label2.AccessibleDescription = null;
            this.label2.AccessibleName = null;
            resources.ApplyResources(this.label2, "label2");
            this.label2.Font = null;
            this.label2.Name = "label2";
            // 
            // label3
            // 
            this.label3.AccessibleDescription = null;
            this.label3.AccessibleName = null;
            resources.ApplyResources(this.label3, "label3");
            this.label3.Font = null;
            this.label3.Name = "label3";
            // 
            // label4
            // 
            this.label4.AccessibleDescription = null;
            this.label4.AccessibleName = null;
            resources.ApplyResources(this.label4, "label4");
            this.label4.Font = null;
            this.label4.Name = "label4";
            // 
            // EleccionRegistroFichero
            // 
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = null;
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.etRegistrosTamañoLista);
            this.Controls.Add(this.btCancelar);
            this.Controls.Add(this.btAceptar);
            this.Controls.Add(this.lbRegistros);
            this.Font = null;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = null;
            this.MaximizeBox = false;
            this.Name = "EleccionRegistroFichero";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btAceptar;
        private System.Windows.Forms.Button btCancelar;
        public System.Windows.Forms.ListBox lbRegistros;
        public System.Windows.Forms.Label etRegistrosTamañoLista;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
    }
}