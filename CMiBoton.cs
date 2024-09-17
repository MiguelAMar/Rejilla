/*
 * Miguel Ángel Martínez Jiménez
 * Versión: 2.0.
 * Fecha: Septiembre  2024.
 * Aplicación: Evaluación y Entrenamiento de la Atención.
 * 
 */

using System;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Versioning;

namespace demorejilla
{
    [SupportedOSPlatform("windows")]
    class CMiBoton : Button
    {
        /*
         * valor: contiene el valor entero del boton
         * valorUltimoBoton: tiene el valor del ultimo boton o -1 sino es el último
         *                  boton de la rejilla
         */ 
        public int valor;
        public int valorUltimoBoton;

        public TipoLetraFuente textoFuente;

        private bool botonTachado;

        //Constructores
        public CMiBoton(int val)
        {
            valor = val;
            Text = val.ToString();
            valorUltimoBoton = -65536;
            this.Click += this.button_Click;
            this.BackColor = SystemColors.Control;
        }

        public CMiBoton(Color c)
        {
            this.BackColor = c;
            this.Click += this.buttonColor_Click;
        }

        public CMiBoton(string txt)
        {
            this.Text = txt;
            this.Click += this.buttonLetra_Click;
            this.BackColor = SystemColors.Control;
        }

        public CMiBoton(Image img)
        {
            this.BackgroundImage = img;
            this.BackgroundImageLayout = ImageLayout.Stretch;
            this.Click += this.buttonImagen_Click;
        }

        public CMiBoton(int val, string txt, bool[] opcionesMayusMinus, bool intercambiar)
        {
            Text = txt;
            valor = val;
            this.valorUltimoBoton = -1;
            this.Click += this.buttonAbecedario_Click;
            this.BackColor = SystemColors.Control;     
        }

        public CMiBoton(int v1, int v2)
        {
            valor = v1;
            this.Text = v1.ToString();
            this.Click += this.buttonTareaPreliminar;
        }

        public CMiBoton(TipoLetraFuente tlf)
        {
            this.textoFuente = tlf;
            this.Click += this.buttonWingdings_Click;
            this.BackColor = SystemColors.Control;
        }

        //Metodos de control de las variables
        public void inicializarBoton()
        {
            this.botonTachado = false;
        }

        public void tacharBoton()
        {
            this.botonTachado = true;
        }

        public bool estadoBoton()
        {
            return this.botonTachado;
        }

        public void cambiarEstado(bool b)
        {
            this.botonTachado = b;
        }

        /*
         * Procedimiento que se ejecuta cada vez que pulso un boton de la rejilla
         * Si ese boton es el que tengo que tachar, se pone su valor text en blanco
         * para indicar que se ha tachado, sino no hace nada.
         * Si se pulsa el ultimo boton y solo queda este, se cierra la rejilla de 
         * botones y se dibuja la grafica en la ventana inicial.
         * Ver btCrearRejilla_Click.
         * 
         */
        public void button_Click(object sender, System.EventArgs e)
        {
            Rejilla formulario = (Rejilla)this.FindForm();
            CMiBoton b = (CMiBoton)sender;
            bool encontrado = false;
            String textito;
            DateTime dt = new DateTime(0);
            textito = b.Text;

            if (formulario.emparejar)
            {
                if (formulario.bParejas.Text == b.Text && !b.estadoBoton())
                {
                    b.tacharBoton();
                    int s = Convert.ToInt32(b.Text);
                    formulario.listaString.Remove(s.ToString());
                    formulario.listaOrdenPulsacíon.Add('[' + b.Text + ']');
                    formulario.aciertos++;

                    if (formulario.rejillaTacharNada || formulario.rejillaTacharColor)
                    {
                        //no tacho el numero
                    }
                    else
                    {
                        b.Text = "";
                    }

                    if (formulario.ordenParaTachar) // Orden Descendente 
                    {
                        formulario.next -= formulario.incrementandoNext;
                    }
                    else //Orden Ascendente
                    {
                        formulario.next += formulario.incrementandoNext;
                    }
                    
                    dt = DateTime.Now;
                    formulario.matrizTiempos[formulario.indiceMatrizTiempos] = dt;

                    formulario.matrizTimeEmparejar[formulario.indiceMatrizTiempos - 1] = dt - formulario.bParejas.devolverTiempo();

                    formulario.indiceMatrizTiempos++;
                    formulario.pulsadoBoton = true;

                    if (formulario.rejillaTacharColor || formulario.rejillaTacharTodo)
                        b.BackColor = SystemColors.Control;//Color.Empty;

                    //Si ya esta en la lista de pulsados y se acaba de tachar, actualizar su tiempo
                    foreach (KeyValuePair<String, DatosControlTachado> elemento in formulario.listaControl)
                    {
                        if (elemento.Key.Equals(textito))
                        {
                            elemento.Value.cambiarTiempo(dt);
                            elemento.Value.cambiarIndice(formulario.indiceMatrizTiempos - 2);
                            elemento.Value.cambiarTitulo(textito);
                            elemento.Value.incrementarPulsacion();
                            elemento.Value.pulsadoBoton(true);
                            encontrado = true;
                            break;
                        }
                    }
                    if (!encontrado)
                    {
                        DatosControlTachado datos = new DatosControlTachado(textito,formulario.indiceMatrizTiempos -2 , 1, dt);
                        datos.pulsadoBoton(true);
                        formulario.listaControl.Add(textito, datos);
                    }
                    formulario.cambiarTiempoBotonParejas();
                }//FIN de: si es el botón correcto para tacharlo.
                else //si no es el boton que tengo que tachar y no es uno en blanco
                {
                    formulario.fallos++;
                    if (b.Text != "")
                    {
                        formulario.listaOrdenPulsacíon.Add(b.Text);
                        //miro a ver si ya lo he pulsado, si ya esta pulsado lo incremento.
                        foreach (KeyValuePair<String, DatosControlTachado> elemento in formulario.listaControl)
                        {
                            if (elemento.Key.Equals(textito))
                            {
                                elemento.Value.incrementarPulsacion();
                                encontrado = true;
                                break;
                            }
                        }
                        //sino esta pulsado lo añado.
                        if (!encontrado)
                        {
                            //formulario.listaControl.Add(textito, new DatosControlTachado(formulario.listaControl.Count - 1, 1, dt));
                            formulario.listaControl.Add(textito, new DatosControlTachado(textito, -1, 1, dt));
                        }
                    }
                }

                if (formulario.listaString.Count == 0)
                    formulario.Close();

            }//fin emparejar
            else
            {
                if (formulario.next == valor && !b.estadoBoton())
                {
                    b.tacharBoton();
                    formulario.listaOrdenPulsacíon.Add('[' + b.Text + ']');
                    formulario.aciertos++;

                    if (formulario.rejillaTacharNada || formulario.rejillaTacharColor)
                    {
                        //no tacho el numero
                    }
                    else
                    {
                        b.Text = "";
                    }

                    if (formulario.ordenParaTachar) // Orden Descendente 
                    {
                        formulario.next -= formulario.incrementandoNext;
                    }
                    else //Orden Ascendente
                    {
                        formulario.next += formulario.incrementandoNext;
                    }
                    dt = DateTime.Now;
                    formulario.matrizTimeEmparejar[formulario.indiceMatrizTiempos - 1] = new TimeSpan(0);
                    formulario.matrizTiempos[formulario.indiceMatrizTiempos] = dt;
                    formulario.indiceMatrizTiempos++;
                    formulario.pulsadoBoton = true;

                    if (formulario.rejillaTacharColor || formulario.rejillaTacharTodo)
                        b.BackColor = SystemColors.Control;//Color.Empty;

                    //Si ya esta en la lista de pulsados y se acaba de tachar, actualizar su tiempo
                    foreach (KeyValuePair<String, DatosControlTachado> elemento in formulario.listaControl)
                    {
                        if (elemento.Key.Equals(textito))
                        {
                            elemento.Value.cambiarTiempo(dt);
                            elemento.Value.cambiarIndice(formulario.indiceMatrizTiempos - 2);
                            elemento.Value.incrementarPulsacion();
                            elemento.Value.pulsadoBoton(true);
                            elemento.Value.cambiarTitulo(textito);
                            encontrado = true;
                            break;
                        }
                    }
                    if (!encontrado)
                    {
                        DatosControlTachado datos = new DatosControlTachado(textito, formulario.indiceMatrizTiempos - 2, 1, dt);
                        datos.pulsadoBoton(true);
                        formulario.listaControl.Add(textito, datos);
                    }

                }//FIN de: si es el botón correcto para tacharlo.
                else //si no es el boton que tengo que tachar y no es uno en blanco
                {
                    formulario.fallos++;
                    if (b.Text != "")
                    {
                        formulario.listaOrdenPulsacíon.Add(b.Text);
                        //miro a ver si ya lo he pulsado, si ya esta pulsado lo incremento.
                        foreach (KeyValuePair<String, DatosControlTachado> elemento in formulario.listaControl)
                        {
                            if (elemento.Key.Equals(textito))
                            {
                                elemento.Value.incrementarPulsacion();
                                encontrado = true;
                                break;
                            }
                        }
                        //sino esta pulsado lo añado.
                        if (!encontrado)
                        {
                            formulario.listaControl.Add(textito, new DatosControlTachado(textito, formulario.listaControl.Count - 1, 1, dt));
                        }
                    }
                }
            }
            if (!formulario.ordenParaTachar) //ascendente
            {
                if (b.valorUltimoBoton == formulario.next - formulario.incrementandoNext)
                    formulario.Close();
            }
            else //descendente
            {
                if (b.valorUltimoBoton == formulario.next + formulario.incrementandoNext)
                    formulario.Close();
            }
        }

        public void buttonColor_Click(object sender, EventArgs e)
        {
            Rejilla formulario = (Rejilla)this.FindForm();
            CMiBoton b = (CMiBoton)sender;
            DateTime dt = new DateTime(0);
            if (formulario.emparejar)
            {
                if (formulario.bParejas.BackColor == b.BackColor && !b.estadoBoton())
                {
                    formulario.aciertos++;
                    b.tacharBoton();
                    formulario.indiceListas = formulario.listaColoresTacharParejas.IndexOf(b.BackColor);
                    formulario.listaColores.Remove(b.BackColor);
                    if (!formulario.listaColores.Contains(b.BackColor))
                        formulario.listaColoresTacharParejas.Remove(b.BackColor);

                    b.BackColor = SystemColors.Control;// Color.Empty; // Color.Transparent;
                    b.Text = " ";
                    dt = DateTime.Now;
                    formulario.matrizTiempos[formulario.indiceMatrizTiempos] = dt;
                    formulario.matrizTimeEmparejar[formulario.indiceMatrizTiempos - 1] = dt - formulario.bParejas.devolverTiempo();
                    formulario.indiceMatrizTiempos++;
                    formulario.numColores++;
                    formulario.pulsadoBoton = true;
                    formulario.cambiarTiempoBotonParejas();
                }
                else
                {
                    formulario.fallos++;
                }
            }//fin emparejar
            else
            {
                if (formulario.listaColoresTachar.Contains(b.BackColor) && !b.estadoBoton())
                {
                    formulario.aciertos++;
                    b.tacharBoton();
                    b.BackColor = SystemColors.Control;// Color.Empty; // Color.Transparent;
                    b.Text = " ";
                    formulario.matrizTimeEmparejar[formulario.indiceMatrizTiempos - 1] = new TimeSpan(0);
                    dt = DateTime.Now;
                    formulario.matrizTiempos[formulario.indiceMatrizTiempos] = dt;
                    formulario.indiceMatrizTiempos++;
                    formulario.numColores++;
                    formulario.pulsadoBoton = true;
                }
                else
                {
                    formulario.fallos++;
                }
                if (formulario.numColores == formulario.numColoresTotal)
                    formulario.Close();
            }
            if (formulario.numColores == formulario.numColoresTotal)
                formulario.Close();
        }

        public void buttonLetra_Click(object sender, EventArgs e)
        {
            Rejilla formulario = (Rejilla)this.FindForm();
            CMiBoton b = (CMiBoton)sender;
            String textito;
            DateTime dt = new DateTime(0);
            textito = b.Text;

            if (formulario.emparejar)
            {
                if (formulario.bParejas.Text == b.Text && !b.estadoBoton())
                {
                    b.tacharBoton();
                    formulario.indiceListas = formulario.listaLetrasTacharParejas.IndexOf(b.Text);
                    formulario.listaString.Remove(b.Text);
                    if (!formulario.listaString.Contains(b.Text))
                        formulario.listaLetrasTacharParejas.Remove(b.Text);
                    
                    formulario.listaOrdenPulsacíon.Add('[' + b.Text + ']');
                    formulario.aciertos++;

                    if (formulario.rejillaTacharNada || formulario.rejillaTacharColor)
                    {
                        //no tachar nada, o solo el color.
                    }
                    else
                    {
                        b.Text = "";
                    }
                    
                    dt = DateTime.Now;
                    formulario.matrizTimeEmparejar[formulario.indiceMatrizTiempos - 1] = dt - formulario.bParejas.devolverTiempo();
                    formulario.matrizTiempos[formulario.indiceMatrizTiempos] = dt;
                    formulario.indiceMatrizTiempos++;
                    formulario.numLetras++;
                    formulario.pulsadoBoton = true;

                    if (formulario.rejillaTacharColor || formulario.rejillaTacharTodo)
                        this.BackColor = SystemColors.Control; //Color.Empty;

                    String s = (formulario.indiceMatrizTiempos - 2).ToString();
                    int longitud = formulario.matrizTiempos.Length;

                    if (longitud > 100)
                    {
                        if (s.Length == 1)
                            s = "00" + s;
                        else if (s.Length == 2)
                            s = "0" + s;
                    }
                    else if (longitud > 10)
                    {
                        if (s.Length == 1)
                            s = "0" + s;
                    }
                    //formulario.listaControl.Add("Orden 00" + (formulario.indiceMatrizTiempos -2).ToString()+" :" +textito, new DatosControlTachado(formulario.indiceMatrizTiempos - 2, 1, dt));
                    formulario.listaControl.Add("Orden " + s + " :" + textito, new DatosControlTachado(textito, formulario.indiceMatrizTiempos - 2, 1, dt));
                    formulario.cambiarTiempoBotonParejas();
                }
                else //si no es el boton que tengo que tachar y no es uno en blanco
                {
                    formulario.fallos++;
                    if (b.Text != "")
                    {
                        formulario.listaOrdenPulsacíon.Add(b.Text);
                    }
                }
            }//fin emparejar
            else
            {
                if (formulario.listaLetrasTachar.Contains(b.Text) && !b.estadoBoton())
                {
                    b.tacharBoton();
                    formulario.listaOrdenPulsacíon.Add('[' + b.Text + ']');
                    formulario.aciertos++;

                    if (formulario.rejillaTacharNada || formulario.rejillaTacharColor)
                    {
                        //no tachar nada, o solo el color.
                    }
                    else
                    {
                        b.Text = "";
                    }
                    formulario.matrizTimeEmparejar[formulario.indiceMatrizTiempos - 1] = new TimeSpan(0);
                    dt = DateTime.Now;
                    formulario.matrizTiempos[formulario.indiceMatrizTiempos] = dt;
                    formulario.indiceMatrizTiempos++;
                    formulario.numLetras++;
                    formulario.pulsadoBoton = true;

                    if (formulario.rejillaTacharColor || formulario.rejillaTacharTodo)
                        this.BackColor = SystemColors.Control; //Color.Empty;

                    String s = (formulario.indiceMatrizTiempos - 2).ToString();
                    int longitud = formulario.matrizTiempos.Length;

                    if (longitud > 100)
                    {
                        if (s.Length == 1)
                            s = "00" + s;
                        else if (s.Length == 2)
                            s = "0" + s;
                    }
                    else if (longitud > 10)
                    {
                        if (s.Length == 1)
                            s = "0" + s;
                    }

                    //formulario.listaControl.Add("Orden 00" + (formulario.indiceMatrizTiempos -2).ToString()+" :" +textito, new DatosControlTachado(formulario.indiceMatrizTiempos - 2, 1, dt));
                    formulario.listaControl.Add("Orden " + s + " :" + textito, new DatosControlTachado(textito, formulario.indiceMatrizTiempos - 2, 1, dt));
                }
                else //si no es el boton que tengo que tachar y no es uno en blanco
                {
                    formulario.fallos++;
                    if (b.Text != "")
                    {
                        formulario.listaOrdenPulsacíon.Add(b.Text);
                    }
                }
            }
            if (formulario.numLetras == formulario.numLetrasTotal)
                formulario.Close();
        }

        public void buttonImagen_Click(object sender, EventArgs e)
        {
            Rejilla formulario = (Rejilla)this.FindForm();
            CMiBoton b = (CMiBoton)sender;
            DateTime dt = new DateTime(0);

            if (formulario.emparejar)
            {
                if (formulario.bParejas.BackgroundImage.Equals(b.BackgroundImage) && !b.estadoBoton())
                {
                    formulario.aciertos++;
                    b.tacharBoton();
                    formulario.indiceListas = formulario.listaImagenesTacharParejas.IndexOf(b.BackgroundImage);
                    formulario.listaImagenes.Remove(b.BackgroundImage);
                    if (!formulario.listaImagenes.Contains(b.BackgroundImage))
                        formulario.listaImagenesTacharParejas.Remove(b.BackgroundImage);

                    b.BackgroundImage = null;
                    dt = DateTime.Now;
                    formulario.matrizTiempos[formulario.indiceMatrizTiempos] = dt;
                    formulario.matrizTimeEmparejar[formulario.indiceMatrizTiempos - 1] = dt - formulario.bParejas.devolverTiempo();
                    formulario.indiceMatrizTiempos++;
                    formulario.numImagenes++;
                    formulario.pulsadoBoton = true;
                    formulario.cambiarTiempoBotonParejas();
                }
                else
                {
                    formulario.fallos++;
                }
            }//fin emparejar
            else
            {
                if (formulario.listaImagenesTachar.Contains(b.BackgroundImage) && !b.estadoBoton())
                {
                    formulario.aciertos++;
                    b.tacharBoton();
                    b.BackgroundImage = null;
                    formulario.matrizTimeEmparejar[formulario.indiceMatrizTiempos - 1] = new TimeSpan(0);
                    formulario.matrizTiempos[formulario.indiceMatrizTiempos] = DateTime.Now;
                    formulario.indiceMatrizTiempos++;
                    formulario.numImagenes++;
                    formulario.pulsadoBoton = true;
                }
                else
                {
                    formulario.fallos++;
                }

            }
            if (formulario.numImagenes == formulario.numImagenesTotal)
                formulario.Close();
        }

        public void buttonAbecedario_Click(object sender, EventArgs e)
        {
            Rejilla formulario = (Rejilla)this.FindForm();
            CMiBoton b = (CMiBoton)sender;
            bool encontrado = false;
            String textito;
            DateTime dt = new DateTime(0);
            textito = b.Text;

            if (formulario.emparejar)
            {
                if (formulario.bParejas.Text == b.Text && !b.estadoBoton())
                {
                    b.tacharBoton();
                    formulario.indiceListas = formulario.listaAbecedarioTacharParejas.IndexOf(b.Text);
                    formulario.listaAbecedarioTacharParejas.Remove(b.Text);
                    if (formulario.rejillaTacharColor || formulario.rejillaTacharNada)
                    {
                        //no hago nada no tacho lo dejo
                    }
                    else
                    {
                        b.Text = "";
                    }

                    formulario.listaOrdenPulsacíon.Add('[' + textito + ']');
                    formulario.aciertos++;
                    dt = DateTime.Now;

                    formulario.next++;
                    formulario.matrizTiempos[formulario.indiceMatrizTiempos] = dt;
                    formulario.matrizTimeEmparejar[formulario.indiceMatrizTiempos - 1] = dt - formulario.bParejas.devolverTiempo();
                    formulario.indiceMatrizTiempos++;
                    formulario.pulsadoBoton = true;

                    if (formulario.rejillaTacharColor || formulario.rejillaTacharTodo)
                        this.BackColor = SystemColors.Control; //Color.Empty;

                    //Si ya esta en la lista de pulsados y se acaba de tachar, actualizar su tiempo
                    foreach (KeyValuePair<String, DatosControlTachado> elemento in formulario.listaControl)
                    {
                        if (elemento.Key.Equals(textito))
                        {
                            elemento.Value.cambiarTiempo(dt);
                            elemento.Value.cambiarIndice(formulario.indiceMatrizTiempos - 2);
                            elemento.Value.cambiarTitulo(textito);
                            elemento.Value.incrementarPulsacion();
                            encontrado = true;
                            break;
                        }
                    }
                    if (!encontrado)
                        formulario.listaControl.Add(textito, new DatosControlTachado(textito,formulario.indiceMatrizTiempos - 2, 1, dt));

                    formulario.cambiarTiempoBotonParejas();
                }
                else
                {
                    formulario.fallos++;
                    if (b.Text != "")
                    {
                        formulario.listaOrdenPulsacíon.Add(b.Text);
                        //miro a ver si ya lo he pulsado, si ya esta pulsado lo incremento.
                        foreach (KeyValuePair<String, DatosControlTachado> elemento in formulario.listaControl)
                        {
                            if (elemento.Key.Equals(textito))
                            {
                                elemento.Value.incrementarPulsacion();
                                encontrado = true;
                                break;
                            }
                        }
                        //sino esta pulsado lo añado.
                        if (!encontrado)
                        {
                            formulario.listaControl.Add(textito, new DatosControlTachado(textito, formulario.listaControl.Count - 1, 1, dt));
                        }
                    }
                }
                if (formulario.listaAbecedarioTacharParejas.Count == 0)
                    formulario.Close();
                
            }//fin emparejar
            else
            {
                if (formulario.next == valor && !b.estadoBoton())
                {
                    b.tacharBoton();
                    if (formulario.rejillaTacharColor || formulario.rejillaTacharNada)
                    {
                        //no hago nada no tacho lo dejo
                    }
                    else
                    {
                        b.Text = "";
                    }

                    formulario.listaOrdenPulsacíon.Add('[' + textito + ']');
                    formulario.aciertos++;

                    formulario.matrizTimeEmparejar[formulario.indiceMatrizTiempos - 1] = new TimeSpan(0);
                    dt = DateTime.Now;
                    formulario.next++;
                    formulario.matrizTiempos[formulario.indiceMatrizTiempos] = dt;
                    formulario.indiceMatrizTiempos++;
                    formulario.pulsadoBoton = true;

                    if (formulario.rejillaTacharColor || formulario.rejillaTacharTodo)
                        this.BackColor = SystemColors.Control; //Color.Empty;

                    //Si ya esta en la lista de pulsados y se acaba de tachar, actualizar su tiempo
                    foreach (KeyValuePair<String, DatosControlTachado> elemento in formulario.listaControl)
                    {
                        if (elemento.Key.Equals(textito))
                        {
                            elemento.Value.cambiarTiempo(dt);
                            elemento.Value.cambiarIndice(formulario.indiceMatrizTiempos - 2);
                            elemento.Value.incrementarPulsacion();
                            elemento.Value.cambiarTitulo(textito);
                            encontrado = true;
                            break;
                        }
                    }
                    if (!encontrado)
                        formulario.listaControl.Add(textito, new DatosControlTachado(textito, formulario.indiceMatrizTiempos - 2, 1, dt));
                }
                else
                {
                    formulario.fallos++;
                    if (b.Text != "")
                    {
                        formulario.listaOrdenPulsacíon.Add(b.Text);
                        //miro a ver si ya lo he pulsado, si ya esta pulsado lo incremento.
                        foreach (KeyValuePair<String, DatosControlTachado> elemento in formulario.listaControl)
                        {
                            if (elemento.Key.Equals(textito))
                            {
                                elemento.Value.incrementarPulsacion();
                                encontrado = true;
                                break;
                            }
                        }
                        //sino esta pulsado lo añado.
                        if (!encontrado)
                        {
                            formulario.listaControl.Add(textito, new DatosControlTachado(textito, formulario.listaControl.Count - 1, 1, dt));
                        }
                    }
                }
                if (this.valorUltimoBoton == formulario.next - 1)
                    formulario.Close();
            }
        }
      
        public void buttonWingdings_Click(object sender, EventArgs e)
        {
            Rejilla formulario = (Rejilla)this.FindForm();
            CMiBoton b = (CMiBoton)sender;
            TipoLetraFuente tlf = new TipoLetraFuente(b.Font, b.Text);
            DateTime dt = new DateTime(0);
            if (formulario.emparejar)
            {
                if (b.textoFuente != null)
                {
                    if (formulario.bParejas.Text == b.textoFuente.devolverLetra()
                        && formulario.bParejas.Font.Name == b.textoFuente.devolverFuente().Name
                        && !b.estadoBoton())
                    {
                        formulario.indiceListas = formulario.listaWindingsTacharParejas.IndexOf(tlf);
                        b.tacharBoton();
                        formulario.listaWin.Remove(tlf);
                        formulario.aciertos++;

                        if (!formulario.listaWin.Contains(tlf))
                        {
                            formulario.listaWindingsTacharParejas.Remove(tlf);
                        }
                        dt = DateTime.Now;
                        formulario.matrizTiempos[formulario.indiceMatrizTiempos] = dt;
                        formulario.matrizTimeEmparejar[formulario.indiceMatrizTiempos - 1] = dt - formulario.bParejas.devolverTiempo();
                        formulario.indiceMatrizTiempos++;
                        formulario.pulsadoBoton = true;
                        formulario.numWings++;
                        if (formulario.rejillaTacharTexto || formulario.rejillaTacharTodo)
                        {
                            b.Text = "";
                        }
                        b.textoFuente = null;

                        if (formulario.rejillaTacharColor || formulario.rejillaTacharTodo)
                            this.BackColor = SystemColors.Control;

                        formulario.cambiarTiempoBotonParejas();
                    }
                    else
                    {
                        formulario.fallos++;
                    }

                    if (formulario.numWings == formulario.numWingstotal)
                        formulario.Close();
                }
            }//fin emparejar
            else
            {
                if (formulario.listaElementosBuscar.Contains(b.textoFuente) && !b.estadoBoton())
                {
                    b.tacharBoton();
                    formulario.matrizTimeEmparejar[formulario.indiceMatrizTiempos - 1] = new TimeSpan(0);
                    formulario.matrizTiempos[formulario.indiceMatrizTiempos] = DateTime.Now;
                    formulario.indiceMatrizTiempos++;
                    formulario.pulsadoBoton = true;
                    formulario.numWings++;
                    formulario.aciertos++;
                    if (formulario.rejillaTacharTexto || formulario.rejillaTacharTodo)
                    {
                        b.Text = "";
                    }
                    b.textoFuente = null;

                    if (formulario.rejillaTacharColor || formulario.rejillaTacharTodo)
                        this.BackColor = SystemColors.Control;
                }
                else
                {
                    formulario.fallos++;
                }
                if (formulario.numWings == formulario.numWingstotal)
                    formulario.Close();
            }
        }

        public void buttonTareaPreliminar(object sender, EventArgs e)
        {
            Rejilla formularioPreliminar = (Rejilla)this.FindForm();
            Button b = (Button)sender;
            if (!formularioPreliminar.pulsadoPrimerBoton)
            {
                if (b.Text == "0")
                {
                    formularioPreliminar.boton1 = DateTime.Now;
                    formularioPreliminar.pulsadoPrimerBoton = true;
                    b.Text = "";
                }
            }
            if (formularioPreliminar.pulsadoPrimerBoton)
            {
                if (b.Text == "1")
                {
                    formularioPreliminar.boton2 = DateTime.Now;
                    b.Text = "";
                    formularioPreliminar.Close();
                }
            }
        }
    }
}
