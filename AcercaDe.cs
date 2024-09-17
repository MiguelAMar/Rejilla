/*
 * Miguel Ángel Martínez Jiménez
 * Versión: 2.0.
 * Fecha: Septiembre  2024.
 * Aplicación: Evaluación y Entrenamiento de la Atención.
 * 
 */ 

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Resources;
using System.Runtime.Versioning;

namespace demorejilla
{
    [SupportedOSPlatform("windows")]
    partial class AcercaDe : Form
    {
        public AcercaDe()
        {
            InitializeComponent();
            //this.Text = String.Format("Acerca de {0} {0}", AssemblyTitle);
            //this.labelProductName.Text = AssemblyProduct;
            //this.labelVersion.Text = String.Format("Versión {0} {0}", AssemblyVersion);
            //this.labelCopyright.Text = AssemblyCopyright;
            //this.labelCompanyName.Text = AssemblyCompany;
            //this.textBoxDescription.Text = AssemblyDescription;

            ResourceManager rm = new ResourceManager("demorejilla.Recursos", typeof(MetiendoDatos).Assembly);

            this.Text = String.Format("Acerca de {0}", "Rejilla");
            this.labelProductName.Text = rm.GetString("titulo");//"Evaluación y Entrenamiento de la Atención";
            this.labelVersion.Text = String.Format("Versión {0}", "2.0");
            this.labelCopyright.Text = "Copyright Miguel Angel Martínez Jiménez" ;
            this.labelCompanyName.Text = "";//"Universidad de Málaga";
            this.textBoxDescription.Text = rm.GetString("car16");//"Aplicación para el Entrenamiento y Evaluación de la Atención." + "Dirigido por José Luis Pastrana Brincones y Antonio Hernández Mendo .";
            //this.textBoxDescription.Text = rm.GetString("titulo"); POR AÑADIR AL FICHERO
        }

        #region Descriptores de acceso de atributos de ensamblado

        public string AssemblyTitle
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (attributes.Length > 0)
                {
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    if (titleAttribute.Title != "")
                    {
                        return titleAttribute.Title;
                    }
                }
                return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }

        public string AssemblyVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        public string AssemblyDescription
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyDescriptionAttribute)attributes[0]).Description;
            }
        }

        public string AssemblyProduct
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }

        public string AssemblyCopyright
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        public string AssemblyCompany
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCompanyAttribute)attributes[0]).Company;
            }
        }
        #endregion
    }
}
