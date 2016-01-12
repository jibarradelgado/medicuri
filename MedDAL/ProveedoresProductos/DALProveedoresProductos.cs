using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MedDAL.ProveedoresProductos
{
    public class DALProveedoresProductos
    {
   
        DAL.medicuriEntities oMedicuriEntities;

        public DALProveedoresProductos() {
            oMedicuriEntities = new DAL.medicuriEntities();
        }

        /// <summary>
        /// Agrega un nuevo registro
        /// </summary>
        /// <param name="oProveedorProducto"></param>
        /// <returns></returns>
        public bool NuevoRegistro(MedDAL.DAL.proveedores_productos oProveedorProducto)
        {
            try
            {
                oMedicuriEntities.AddToproveedores_productos(oProveedorProducto);
                oMedicuriEntities.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        /// Elimina todos los registros de proveedor producto de acuerdo al producto
        /// </summary>
        /// <param name="idProducto">id del producto</param>
        /// <returns></returns>
        public bool EliminarRegistro(int idProducto)
        {
            try
            {
                var oQuery = from q in oMedicuriEntities.proveedores_productos
                             where q.idProducto == idProducto
                             select q;

                foreach (MedDAL.DAL.proveedores_productos oProveedorProducto in oQuery)
                {
                    oMedicuriEntities.DeleteObject(oProveedorProducto);
                    oMedicuriEntities.SaveChanges();
                }
                
                return true;
            }
            catch
            {
                return false;
            }
        }

        public object Buscar(int idProducto) {
            try {
                var oQuery = from q in oMedicuriEntities.proveedores_productos
                             where q.idProducto == idProducto
                             select q;
                return oQuery.First<DAL.proveedores_productos>();
            }
            catch {
                return null;
            }
        }
    }
}
