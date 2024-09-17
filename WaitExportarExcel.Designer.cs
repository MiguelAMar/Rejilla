namespace demorejilla
{
    partial class WaitExportarExcel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WaitExportarExcel));
            this.label1 = new System.Windows.Forms.Label();
            this.pbFichero = new System.Windows.Forms.ProgressBar();
            this.pbTotal = new System.Windows.Forms.ProgressBar();
            this.etFichero = new System.Windows.Forms.Label();
            this.etTitulo = new System.Windows.Forms.Label();
            this.etTitulo2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.label1.Name = "label1";
            // 
            // pbFichero
            // 
            resources.ApplyResources(this.pbFichero, "pbFichero");
            this.pbFichero.Name = "pbFichero";
            // 
            // pbTotal
            // 
            resources.ApplyResources(this.pbTotal, "pbTotal");
            this.pbTotal.Name = "pbTotal";
            // 
            // etFichero
            // 
            resources.ApplyResources(this.etFichero, "etFichero");
            this.etFichero.Name = "etFichero";
            // 
            // etTitulo
            // 
            resources.ApplyResources(this.etTitulo, "etTitulo");
            this.etTitulo.Name = "etTitulo";
            // 
            // etTitulo2
            // 
            resources.ApplyResources(this.etTitulo2, "etTitulo2");
            this.etTitulo2.Name = "etTitulo2";
            // 
            // WaitExportarExcel
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.etTitulo2);
            this.Controls.Add(this.etTitulo);
            this.Controls.Add(this.etFichero);
            this.Controls.Add(this.pbTotal);
            this.Controls.Add(this.pbFichero);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "WaitExportarExcel";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.ProgressBar pbTotal;
        public System.Windows.Forms.ProgressBar pbFichero;
        public System.Windows.Forms.Label etFichero;
        private System.Windows.Forms.Label etTitulo;
        private System.Windows.Forms.Label etTitulo2;
    }
}