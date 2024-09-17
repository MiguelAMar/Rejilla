namespace demorejilla
{
    partial class MostrarWindings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MostrarWindings));
            this.labelWin1 = new System.Windows.Forms.Label();
            this.labelWin2 = new System.Windows.Forms.Label();
            this.labelWin3 = new System.Windows.Forms.Label();
            this.btAceptar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labelWin1
            // 
            this.labelWin1.AccessibleDescription = null;
            this.labelWin1.AccessibleName = null;
            resources.ApplyResources(this.labelWin1, "labelWin1");
            this.labelWin1.Name = "labelWin1";
            // 
            // labelWin2
            // 
            this.labelWin2.AccessibleDescription = null;
            this.labelWin2.AccessibleName = null;
            resources.ApplyResources(this.labelWin2, "labelWin2");
            this.labelWin2.Name = "labelWin2";
            // 
            // labelWin3
            // 
            this.labelWin3.AccessibleDescription = null;
            this.labelWin3.AccessibleName = null;
            resources.ApplyResources(this.labelWin3, "labelWin3");
            this.labelWin3.Name = "labelWin3";
            // 
            // btAceptar
            // 
            this.btAceptar.AccessibleDescription = null;
            this.btAceptar.AccessibleName = null;
            resources.ApplyResources(this.btAceptar, "btAceptar");
            this.btAceptar.BackgroundImage = null;
            this.btAceptar.Font = null;
            this.btAceptar.Name = "btAceptar";
            this.btAceptar.UseVisualStyleBackColor = true;
            this.btAceptar.Click += new System.EventHandler(this.button1_Click);
            // 
            // MostrarWindings
            // 
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = null;
            this.Controls.Add(this.btAceptar);
            this.Controls.Add(this.labelWin3);
            this.Controls.Add(this.labelWin2);
            this.Controls.Add(this.labelWin1);
            this.Font = null;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = null;
            this.MaximizeBox = false;
            this.Name = "MostrarWindings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Label labelWin1;
        public System.Windows.Forms.Label labelWin2;
        public System.Windows.Forms.Label labelWin3;
        private System.Windows.Forms.Button btAceptar;

    }
}