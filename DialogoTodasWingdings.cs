/*
 * Miguel Ángel Martínez Jiménez
 * Versión: 2.0.
 * Fecha: Septiembre  2024.
 * Aplicación: Evaluación y Entrenamiento de la Atención.
 * 
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Versioning;
using System.Windows.Forms;

namespace demorejilla
{
    [SupportedOSPlatform("windows")]
    public partial class DialogoTodasWingdings : Form
    {
        public List<TipoLetraFuente> listaCompleta;
        public List<TipoLetraFuente> listaEscogidos;

        public DialogoTodasWingdings()
        {
            InitializeComponent();
            listaCompleta = new List<TipoLetraFuente>();
            listaEscogidos = new List<TipoLetraFuente>();
        }



        public DialogoTodasWingdings(List<TipoLetraFuente> listaTodasLetras, List<TipoLetraFuente> listaTachar)
        {
            InitializeComponent();
            listaCompleta = new List<TipoLetraFuente>();
            listaCompleta = listaTodasLetras;
            listaEscogidos = new List<TipoLetraFuente>();
            listaEscogidos = listaTachar;
            colorearBotones();
        }



        private void colorearBotones()
        {
            TipoLetraFuente tlf = null;
            foreach (Button bt in this.Controls.OfType<Button>())
            {
                if (bt.Name.Equals("btAceptar") || bt.Name.Equals("BtCancelar") || bt.Name.Equals("btLimpiar"))
                {
                    //No se hace nada con ellos
                }
                else
                {
                    tlf = new TipoLetraFuente(bt.Font, bt.Text);
                    if (this.listaCompleta.Contains(tlf))
                    {
                        bt.BackColor = Color.YellowGreen;
                    }
                    else
                    {
                        bt.BackColor = Color.White;
                    }

                    if (this.listaEscogidos.Contains(tlf))
                    {
                        bt.BackColor = Color.Coral;
                    }
                }
            }
        }

        private void button_Click(object sender, EventArgs e)
        {
            Button aux = (Button)sender;
            TipoLetraFuente tlf = new TipoLetraFuente(aux.Font, aux.Text);

            if (this.cbTodasWidings.Checked)
            {
                if (aux.BackColor == Color.White)
                {
                    aux.BackColor = Color.Coral;
                    this.listaCompleta.Add(tlf);
                    this.listaEscogidos.Add(tlf);
                }
                else if (aux.BackColor == Color.YellowGreen)
                {
                    aux.BackColor = Color.Coral;
                    this.listaEscogidos.Add(tlf);
                }
                else if (aux.BackColor == Color.Coral)
                {
                    this.listaCompleta.Remove(tlf);
                    this.listaEscogidos.Remove(tlf);
                    aux.BackColor = Color.White;
                }
            }
            else
            {
                if (aux.BackColor == Color.YellowGreen)
                {
                    aux.BackColor = Color.White;
                    listaCompleta.Remove(tlf);
                }
                else if (aux.BackColor == Color.White)
                {
                    aux.BackColor = Color.YellowGreen;
                    listaCompleta.Add(tlf);
                }
            }
        }

        private void btLimpiar_Click(object sender, EventArgs e)
        {
            foreach (Button bt in this.Controls.OfType<Button>())
            {
                if (bt.Name.Equals("btAceptar") || bt.Name.Equals("BtCancelar") || bt.Name.Equals("btLimpiar"))
                {
                    //No se hace nada con ellos
                }
                else
                {
                        bt.BackColor = Color.White;
                }
            }
            this.listaCompleta.Clear();
            this.listaEscogidos.Clear();
        }
    }
}
