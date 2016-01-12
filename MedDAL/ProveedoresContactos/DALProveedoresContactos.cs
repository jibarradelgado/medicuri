using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using System.Data.Objects.DataClasses;

namespace MedDAL.ProveedoresContactos
{
    public class DALProveedoresContactos
    {
        DAL.medicuriEntities oMedicuriEntities;
        
        public DALProveedoresContactos()
        {
            oMedicuriEntities = new DAL.medicuriEntities();
        }

        /// <summary>
        /// Registra un nuevo contacto de proveedor
        /// </summary>
        /// <param name="oContacto"></param>
        /// <returns></returns>
        public bool NuevoRegistro(MedDAL.DAL.proveedores_contactos oContacto)
        {
            try
            {
                oMedicuriEntities.AddToproveedores_contactos(oContacto);
                oMedicuriEntities.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool EliminarRegistro(MedDAL.DAL.proveedores_contactos oContacto)
        {
            try
            {
                var oQuery = from q in oMedicuriEntities.proveedores_contactos.
                            Where("it.idContactoProveedor = @idContactoProveedor",
                            new ObjectParameter("idContactoProveedor", oContacto.IdContactoProveedor))
                            select q;

                DAL.proveedores_contactos oProveedorOriginal = oQuery.First<DAL.proveedores_contactos>();

                oMedicuriEntities.DeleteObject(oProveedorOriginal);
                oMedicuriEntities.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Elimina todos los registros que contengan relacion con el proveedor
        /// </summary>
        /// <param name="oProveedor"></param>
        /// <returns>true si se realizo la eliminación, false si no</returns>
        public bool EliminarRegistro(MedDAL.DAL.proveedores oProveedor)
        {
            try
            {
                var oQuery = from q in oMedicuriEntities.proveedores_contactos
                             where q.idProveedor == oProveedor.IdProveedor
                             select q;

                foreach (MedDAL.DAL.proveedores_contactos oContacto in oQuery)
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
