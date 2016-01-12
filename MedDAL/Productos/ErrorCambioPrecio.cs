using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MedDAL.Inventarios
{
    public class ErrorCambioPrecio
    {
        String _codigoProducto, _Mensaje;

        public ErrorCambioPrecio(String codigoProducto, String mensaje)
        {
            this._codigoProducto = codigoProducto;
            this._Mensaje = mensaje;
        }

        public String CodigoProducto 
        {
            get { return this._codigoProducto; }
        }

        public String MensajeError
        {
            get { return this._Mensaje; }
        }

    }
}
