using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using System.Data.Objects.DataClasses;

namespace MedDAL.AlmacenesContactos
{
    public class DALAlmacenesContactos
    {
        DAL.medicuriEntities oMedicuriEntities;

        public DALAlmacenesContactos()
        {
            oMedicuriEntities = new DAL.medicuriEntities();
        }

        /// <summary>
        /// Registra un nuevo contacto de almacén
        /// </summary>
        /// <param name="oContacto"></param>
        /// <returns></returns>
        public bool NuevoRegistro(MedDAL.DAL.almacenes_contactos oContacto)
        {
            try
            {
                oMedicuriEntities.AddToalmacenes_contactos(oContacto);
                oMedicuriEntities.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Elimina todos los registros que contengan relacion con el almacén
        /// </summary>
        /// <param name="oAlmacen"></param>
        /// <returns>true si se realizo la eliminación, false si no</returns>
        public bool EliminarRegistro(MedDAL.DAL.almacenes oAlmacen)
        {
            try
            {
                var oQuery = from q in oMedicuriEntities.almacenes_contactos
                             where q.idAlmacen == oAlmacen.idAlmacen
                             select q;

                foreach (MedDAL.DAL.almacenes_contactos oContacto in oQuery)
                {
                    oMedicuriEntities.DeleteObject(oContacto);
                }

                oMedicuriEntities.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
