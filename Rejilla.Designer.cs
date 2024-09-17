/*
 * Miguel Angel Martínez Jiménez
 * Evaluacion y Entrenamiento de la atención
 * PFC
 * 
 * 
 * Fecha: 16 - 03 - 2008
 * Versión: 1.0.8
 * CAmbiada la grafica a la primera ventana, elñiminar la ventana nueva
 * 
 */

using System.Windows.Forms;

namespace demorejilla
{
    partial class Rejilla
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            cajaImprimir = new PrintDialog();
            printDocument = new System.Drawing.Printing.PrintDocument();
            SuspendLayout();
            // 
            // cajaImprimir
            // 
            cajaImprimir.AllowPrintToFile = false;
            cajaImprimir.AllowSomePages = true;
            cajaImprimir.Document = printDocument;
            cajaImprimir.UseEXDialog = true;
            // 
            // printDocument
            // 
            printDocument.DocumentName = "impresionrejilla";
            printDocument.PrintPage += printDocument_Page;
            // 
            // Rejilla
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(784, 561);
            DoubleBuffered = true;
            Margin = new Padding(4, 3, 4, 3);
            MaximizeBox = false;
            Name = "Rejilla";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "La Rejilla";
            WindowState = FormWindowState.Maximized;
            FormClosing += Rejilla_FormClosing;
            ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.PrintDialog cajaImprimir;
        private System.Drawing.Printing.PrintDocument printDocument;
    }
}

