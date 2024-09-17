/*
 * Miguel Ángel Martínez Jiménez
 * Versión: 2.0.
 * Fecha: Septiembre  2024.
 * Aplicación: Evaluación y Entrenamiento de la Atención.
 * 
 */

using System;
using System.Runtime.Versioning;
[SupportedOSPlatform("windows")]
public class DatosControlTachado
{
    //variables
    private int numVecesPuldado;
    private string tiempo;
    private int indice; 
    private DateTime tiempoPulsado;
    private long dateTiempo;
    private bool botonLocalizado;
    private string titulo;

    //constructor
    public DatosControlTachado(string title, int indiceItem, int conteoPulsado, DateTime daTime)
    {
        numVecesPuldado = conteoPulsado;
        tiempoPulsado = daTime;
        indice = indiceItem;
        dateTiempo = daTime.Ticks;
        this.botonLocalizado = false;
        this.titulo = title;
    }

    public DatosControlTachado(string title, int indiceItem, int conteoPulsado, DateTime daTime, bool localizado)
    {
        numVecesPuldado = conteoPulsado;
        tiempoPulsado = daTime;
        indice = indiceItem;
        dateTiempo = daTime.Ticks;
        this.botonLocalizado = localizado;
        this.titulo = title;
    }


    public string obtenerTiempo(DateTime[] dt, bool boleana)
    {
        TimeSpan resta = new TimeSpan();
        resta = this.tiempoPulsado - dt[this.indice];
        tiempo = (FormatTimeSpan(resta));
        return tiempo;
    }


    public DateTime devolverTiempo()
    {
        return tiempoPulsado;
    }
    public bool devolverBoton()
    {
        return this.botonLocalizado;
    }

    public int devolverItem()
    {
        return this.indice;
    }

    public int devolverPulsaciones()
    { 
        return numVecesPuldado;
    }

    public string obtenerTitulo()
    {
        return titulo;
    }

    public void cambiarTitulo(string t)
    {
        this.titulo = t;
    }

    public int vecesPulsado()
    {
        return numVecesPuldado;
    }

    public int indiceItem()
    {
        return indice;
    }

    public void incrementarPulsacion()
    {
        this.numVecesPuldado++;
    }

    public void cambiarTiempo(DateTime d)
    {
        this.tiempoPulsado = d;
        this.dateTiempo = d.Ticks;
    }

    public void cambiarIndice(int i)
    {
        this.indice = i;
    }

    public void pulsadoBoton(bool pulsacion)
    {
        this.botonLocalizado = pulsacion;
    }

    public bool localizado()
    {
        return this.botonLocalizado;
    }

    /*
     * Procedimiento para pintar los tiempos en las etiquetas de dataGRidView,
     * no se emplea el toString() directamente porque saca las fraccciones de misilesgundos con 
     * 8 decimales
     */
    private static string FormatTimeSpan(TimeSpan span)
    {
        string sign = String.Empty;
        return sign + //span.Days.ToString("00") + "." +
            //span.Hours.ToString("00") + ":" +
               span.Minutes.ToString("00") + ":" +
               span.Seconds.ToString("00") + "," +
               span.Milliseconds.ToString("000");
    }

    public override string ToString()
    {
        string cadena = "";
        cadena += this.titulo;
        cadena += Convert.ToChar(3);
        cadena += this.indice;
        cadena += Convert.ToChar(3);
        cadena += this.numVecesPuldado;
        cadena += Convert.ToChar(4);
        cadena += this.dateTiempo;
        cadena += Convert.ToChar(4);
        cadena += this.botonLocalizado;
        //return this.indice + '#' + this.numVecesPuldado + '#' + this.dateTiempo.ToString();
        return cadena;
    }
}