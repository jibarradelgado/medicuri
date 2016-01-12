using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;

namespace MedDAL.EnsambleProductos
{
    public class DALEnsambleProductos
    {
        DAL.medicuriEntities oMedicuriEntities;

        public DALEnsambleProductos() { 
            oMedicuriEntities = new DAL.medicuriEntities();
        }

        /// <summary>
        /// DAL - Insertar nuevo registro
        /// </summary>
        /// <param name="oEnsambleProductos">Registro a guardar</param>
        /// <returns>true registrado, false no registrado</returns>
        public bool NuevoRegistro(DAL.ensamble_productos oEnsambleProductos)
        {
            try
            {
                //Agregar el registro
                oMedicuriEntities.AddToensamble_productos(oEnsambleProductos);
                oMedicuriEntities.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Obtiene los ensambles que coincidan con la búsqueda y el filtro
        /// </summary>
        /// <param name="idEnsamble">idEnsamble por la cual buscar</param>
        /// <returns></returns>
        public List<DAL.ensamble_productos> Buscar(int idEnsamble)
        {
            var oQuery = from q in oMedicuriEntities.ensamble_productos
                         where q.idEnsamble.Equals(idEnsamble)
                         select q;
            return oQuery.ToList<DAL.ensamble_productos>();
        }

        /// <summary>
        /// Busca todos los ensambleProductos que estén activos
        /// </summary>
        /// <returns>La coleccion de ensamblesProductos</returns>
        public IQueryable<DAL.ensamble_productos> BuscarEnum()
        {
            var oQuery = from q in oMedicuriEntities.ensamble_productos
                         select q;

            return oQuery;
        }

        /// <summary>
        /// Eliminar un ensambleProducto
        /// </summary>
        /// <param name="iIdEnsamble">Id ensamble a eliminar</param>
        /// <returns></returns>
        public bool EliminarRegistro(int iIdEnsamble)
        {
            try
            {
                var oQuery = from q in oMedicuriEntities.ensamble_productos
                             where q.idEnsamble.Equals(iIdEnsamble)
                             select q;

                //DAL.ensamble_productos oEnsambleProductoOriginal = oQuery.First<DAL.ensamble_productos>();
                //oMedicuriEntities.DeleteObject(oEnsambleProductoOriginal);
                //oMedicuriEntities.SaveChanges();
                foreach (DAL.ensamble_productos oEnsambleProductoOriginal in oQuery)
                {
                    oMedicuriEntities.DeleteObject(oEnsambleProductoOriginal);
                    
                }
                oMedicuriEntities.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool EliminarRegistro(DAL.ensamble_productos oEnsambleProductos)
        {
            try
            {
                var oQuery = from q in oMedicuriEntities.ensamble_productos.
                            Where("it.idEnsamble = @idEnsamble",
                            new ObjectParameter("idEnsamble", oEnsambleProductos.idEnsamble))
                             select q;

                DAL.ensamble_productos oEnsambleProducto = oQuery.First<DAL.ensamble_productos>();

                oMedicuriEntities.DeleteObject(oEnsambleProducto);
                oMedicuriEntities.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public IQueryable<DAL.ensamble_productos> RecuperarProductos(string sClaveBOM)
        {
            try
            {
                var oQuery = from q in oMedicuriEntities.ensamble_productos
                             where q.ensamble.ClaveBom == sClaveBOM
                             select q;
                return oQuery;
            }
            catch
            {
                return null;
            }

        }

        public DAL.ensamble_productos RecuperarProducto(string sClaveBOM)
        {
            try
            {
                var oQuery = from q in oMedicuriEntities.ensamble_productos
                             where q.ensamble.ClaveBom == sClaveBOM
                             select q;
                return oQuery.First<DAL.ensamble_productos>();
            }
            catch
            {
                return null;
            }

        }
    }
}
