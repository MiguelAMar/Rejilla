/*
 * Miguel Ángel Martínez Jiménez
 * Versión: 2.0.
 * Fecha: Septiembre  2024.
 * Aplicación: Evaluación y Entrenamiento de la Atención.
 * 
 */

using System;
using System.Drawing;
using System.Resources;
using System.Runtime.Versioning;
[SupportedOSPlatform("windows")]
[Serializable]

public class TipoLetraFuente : IComparable
{

    private string fuenteNombre;
    private float fuenteNombreTamaño;
    private Font fuente;
    private String letra;
    //private ResourceManager rm;

    public TipoLetraFuente()
    {
        ResourceManager rm = new ResourceManager("demorejilla.Recursos", typeof(TipoLetraFuente).Assembly);
        fuente = null;
        letra = null;
        fuenteNombre = null;
        fuenteNombreTamaño = 0;
    }

    public TipoLetraFuente(Font f, String c)
    {
        ResourceManager rm = new ResourceManager("demorejilla.Recursos", typeof(TipoLetraFuente).Assembly);
        fuente = f;
        letra = c;
        fuenteNombre = f.Name;
        fuenteNombreTamaño = f.Size;
    }

    public float devolverTamaño()
    {
        return fuenteNombreTamaño;
    }

    public Font devolverFuente()
    {
        return new Font(fuenteNombre, fuenteNombreTamaño);// fuente;
    }

    public String devolverLetra()
    {
        return letra;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override bool Equals(object obj)
    {
        if (obj is TipoLetraFuente)
        {
            TipoLetraFuente tlf = (TipoLetraFuente) obj;
            return (this.fuente.Name == tlf.fuente.Name
                    && this.letra == tlf.letra);
        }
        else
        {
            ResourceManager rm = new ResourceManager("demorejilla.Recursos", typeof(TipoLetraFuente).Assembly);
            //throw new ArgumentException ("Error grave del sitema. Cierre la aplicación.");
            throw new ArgumentException(rm.GetString("olv17"));
        }
    }

    
    public override string ToString()
    {
        return this.fuente.ToString() +  " --->  "  + this.letra.ToString() + "\n";
    }
    


    #region Miembros de IComparable

    public int CompareTo(object obj)
    {
        if (obj is TipoLetraFuente)
        {
            TipoLetraFuente tlf = (TipoLetraFuente)obj;
            if (this.fuente.Name.Equals(tlf.fuente.Name))
                return (this.letra.CompareTo(tlf.letra));
            else
                return this.fuente.Name.CompareTo(tlf.fuente.Name);
        }
        else
        {
            ResourceManager rm = new ResourceManager("demorejilla.Recursos", typeof(TipoLetraFuente).Assembly);
            //throw new ArgumentException("Error grave en Wingdings, fallo del sistema. \n");
            throw new ArgumentException(rm.GetString("olv18"));
        }
    }

    #endregion
}
