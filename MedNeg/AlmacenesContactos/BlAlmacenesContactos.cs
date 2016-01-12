using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using System.Data.Objects.DataClasses;


namespace MedNeg.AlmacenesContactos
{
    public class BlAlmacenesContactos
    {
        MedDAL.AlmacenesContactos.DALAlmacenesContactos odalAlmacenesContactos;

        public BlAlmacenesContactos()
        {
            odalAlmacenesContactos = new MedDAL.AlmacenesContactos.DALAlmacenesContactos();
        }

        public bool NuevoRegistro(MedDAL.DAL.almacenes_contactos oContacto)
        {
            return odalAlmacenesContactos.NuevoRegistro(oContacto);
        }

        public bool EliminarRegistro(MedDAL.DAL.almacenes oAlmacen)
        {
            return odalAlmacenesContactos.EliminarRegistro(oAlmacen);
        }
    }
}
