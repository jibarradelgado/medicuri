using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using System.Data.Objects.DataClasses;

namespace MedNeg.ProveedoresContactos
{
    public class BlProveedoresContactos
    {
        MedDAL.ProveedoresContactos.DALProveedoresContactos odalProveedoresContactos;

        public BlProveedoresContactos() 
        {
            odalProveedoresContactos = new MedDAL.ProveedoresContactos.DALProveedoresContactos();
        }

        public bool NuevoRegistro(MedDAL.DAL.proveedores_contactos oContacto)
        {
            return odalProveedoresContactos.NuevoRegistro(oContacto);
        }

        public bool EliminarRegistro(MedDAL.DAL.proveedores_contactos oContacto)
        {
            return odalProveedoresContactos.EliminarRegistro(oContacto);
        }

        public bool EliminarRegistro(MedDAL.DAL.proveedores oProveedor)
        {
            return odalProveedoresContactos.EliminarRegistro(oProveedor);
        }
    }
}
