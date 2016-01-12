using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MedDAL.Inventarios
{
    public class InventariosView
    {
        int iIdProAlmStocks, iStockMin, iStockMax;
        string sAlmacen, sClave, sProducto;
        decimal dCantidad;

        public int idProAlmStocks {
            get { return iIdProAlmStocks; }
            set { this.iIdProAlmStocks = value; }
        }
        public int StockMin
        {
            get { return iStockMin; }
            set { this.iStockMin = value; }
        }
        public int StockMax
        {
            get { return iStockMax; }
            set { this.iStockMax = value; }
        }
        public string Almacen {
            get { return sAlmacen; }
            set { this.sAlmacen = value; }
        }
        public string Clave
        {
            get { return sClave; }
            set { this.sClave = value; }
        }
        public string Producto
        {
            get { return sProducto; }
            set { this.sProducto = value; }
        }
        public decimal Cantidad
        {
            get { return dCantidad; }
            set { this.dCantidad = value; }
        }

        public InventariosView() { }
    }
}
