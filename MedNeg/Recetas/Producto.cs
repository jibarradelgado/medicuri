using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MedNeg.Recetas
{
    public class Producto
    {
        MedDAL.DAL.productos _dbProducto;
        String _clave1;
        String _descripcion;
        decimal _cantidadRecetada;
        decimal _cantidadSurtida;
        String _lote;
        String _serie;
        decimal _precio;
        int _intencion;
        bool _cause;
        int _lineaCredito;
        bool _agregadoPorEdicionDePartida;

        public Producto(MedDAL.DAL.productos dbProducto, String clave1,String descripcion,decimal cantidadRecetada,decimal cantidadSurtida,String lote,String serie, decimal precio,int intencion,bool cause,int idLineaCredito)
        {
            this._dbProducto = dbProducto;
            this._clave1 = clave1;
            this._descripcion = descripcion;
            this._cantidadRecetada = cantidadRecetada;
            this._cantidadSurtida = cantidadSurtida;
            this._lote = lote;
            this._serie=serie;
            this._precio = precio;
            this._intencion = intencion;
            this._cause = cause;
            this._lineaCredito = idLineaCredito;
            this._agregadoPorEdicionDePartida = false;
        }

        public MedDAL.DAL.productos dbProducto
        {
            get { return this._dbProducto; }
        }

        
         public String clave1
        {
            get { return this._clave1 ; }
            set { this._clave1 = value; }
        } 

        public String descripcion
        {
            get { return this._descripcion ; }
            set { this._descripcion = value; }
        }         

        public decimal cantidadRecetada
        {
            get { return this._cantidadRecetada ; }
            set { this._cantidadRecetada = value; }
         }         

        public decimal cantidadSurtida
        {
            get { return this._cantidadSurtida ; }
            set { this._cantidadSurtida = value; }
        }

        public String lote
        {
            get { return this._lote ; }
            set { this._lote = value; }
        }


         public String serie
        {
            get { return this._serie ; }
            set { this._serie = value; }
        }

        public decimal precio
        {
            get { return this._precio ; }
            set { this._precio = value; }
        }

        public int intencion
        {
            get { return this._intencion ; }
            set { this._intencion = value; }
        }

        public bool cause
        {
            get { return this._cause ; }
            set { this._cause = value; }
        }

        public string intencionStr
        {
            get { return intencion == 1 ? "Prim." : "Seg."; }
        }

        public string causeStr
        {
            get { return cause ? "Sí" : "No"; } 
        }

        public decimal totalPrecio
        {
            get { return this.precio * this.cantidadSurtida; }
        }

        public int lineaCredito 
        {
            get { return this._lineaCredito; }
        }

        public bool agregadoPorEdicionDePartida
        {
            get { return this._agregadoPorEdicionDePartida; }
            set { this._agregadoPorEdicionDePartida = value; }
        }
    }
}
