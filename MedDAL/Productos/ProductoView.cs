using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MedDAL.Productos
{
    public class ProductoView
    {
        int iIdProducto, iStockMinimo, iStockMaximo;
        string sClave1, sClave2, sNombre, sPresentacion, sNombreTipo;
        decimal dPrecioMinimo, dPrecioPublico;
        bool bActivo;

        public int idProducto 
        {            
            set 
            {
                this.iIdProducto = value;
            }
            get 
            {
                return iIdProducto;
            }
        }
        public string Clave1 
        {
            set 
            {
                this.sClave1 = value;
            }
            get
            {
                return sClave1;
            }
        }
        public string Clave2
        {
            set
            {
                this.sClave2 = value;
            }
            get
            {
                return sClave2;
            }
        }
        public string Nombre
        {
            set 
            {
                this.sNombre = value;
            }
            get
            {
                return sNombre;
            }
        }
        public string Presentacion
        {
            set
            {
                this.sPresentacion = value;
            }
            get
            {
                return sPresentacion;
            }    
        }
        public int StockMinimo
        {
            set
            {
                this.iStockMinimo = value;
            }
            get
            {
                return iStockMinimo;
            }
        }
        public int StockMaximo
        {
            set
            {
                this.iStockMaximo = value;
            }
            get
            {
                return iStockMaximo;
            }
        }
        public decimal PrecioMinimo
        {
            set
            {
                this.dPrecioMinimo = value;
            }
            get
            {
                return dPrecioMinimo;
            }
        }
        public decimal PrecioPublico
        {
            set
            {
                this.dPrecioPublico = value;
            }
            get
            {
                return dPrecioPublico;
            }
        }
        public string TipoProducto
        {
            set
            {
                this.sNombreTipo = value;
            }
            get
            {
                return sNombreTipo;
            }
        }
        public bool Activo
        {
            set
            {
                this.bActivo = value;
            }
            get
            {
                return bActivo;
            }
        }

        public ProductoView() { }
    }
}
