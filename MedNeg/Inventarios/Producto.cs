using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace MedNeg.Inventarios
{
   public class Producto
    {
         MedDAL.DAL.productos _dbProducto;
         String _codigo;
         String _descripcion;
         decimal _cantidad;
         String _lote;
         String _serie;
         decimal _costo;
         decimal _existenciaTeórica;
         decimal _existenciaReal;
         String _sExistenciaReal=string.Empty;
         DateTime _fechaCaducidad;

        public Producto(MedDAL.DAL.productos dbProducto,String codigo, String descripcion, decimal cantidad,String lote,String serie,decimal costo,DateTime fechaCaducidad)
        {
            this._dbProducto = dbProducto;
            this._codigo = codigo;
            this._descripcion = descripcion;
            this._cantidad= cantidad;
            this._lote = lote;
            this._serie = serie;
            this._costo = costo;
            this._fechaCaducidad = fechaCaducidad;
        }

        public Producto(MedDAL.DAL.productos dbProducto, string codigo, string descripcion, string lote, string serie,decimal existenciaTeorica)
        {
            this._dbProducto = dbProducto;
            this._codigo = codigo;
            this._descripcion = descripcion;
            this._cantidad = cantidad;
            this._lote = lote;
            this._serie = serie;
            this._costo = costo;
            this._existenciaTeórica = existenciaTeorica;
        }

        public MedDAL.DAL.productos dbProducto
        {
            get { return this._dbProducto; } 
        }
      
       public String codigo
        {
            get {return this._codigo;}
        }

        public String descripcion
        {
            get{return this._descripcion;}
        }

        public decimal cantidad
        {
            get { return this._cantidad; }
        }

        public String lote
        {
            get { return this._lote; }
        }
        
       public String serie
        {
            get { return this._serie; }
        }

       public decimal costo
       {
           get { return this._costo; }
       }

       public decimal existenciaTeorica 
       {
           get { return this._existenciaTeórica; }
       }

       public decimal existenciaReal
       {
           get { return this._existenciaReal; }
           set { this._existenciaReal = value; }
       }

       public string strExistenciaReal
       {
           get { return this._sExistenciaReal; }
           set { this._sExistenciaReal = value; }
       }

       public DateTime fechaCaducidad 
       {
           get { return this._fechaCaducidad; }
           set { this._fechaCaducidad = value; }
       }

       public string strFechaCaducidadShort
       {
           get { return fechaCaducidad.ToShortDateString(); }
       }

   }
}
