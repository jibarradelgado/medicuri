using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;

namespace MedDAL.ClientesContactos
{
    public class DALClientesContactos
    {
        DAL.medicuriEntities oMedicuriEntities;

        public DALClientesContactos(){
            oMedicuriEntities = new DAL.medicuriEntities();
        }

        /// <summary>
        /// Registra un nuevo contacto para un cliente
        /// </summary>
        /// <param name="oClienteContacto"></param>
        /// <returns></returns>
        public bool NuevoRegistro(MedDAL.DAL.clientes_contacto oClienteContacto)
        {
            try
            {
                oMedicuriEntities.AddToclientes_contacto(oClienteContacto);
                oMedicuriEntities.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Obtiene un todos cliente 
        /// </summary>
        /// <param name="iId">El id del cliente</param>
        /// <returns></returns>
        public List<MedDAL.DAL.clientes_contacto> Buscar(int iId)
        {
            var oQuery = from q in oMedicuriEntities.clientes_contacto.
                            Where("it.idCliente = @idCliente",
                            new ObjectParameter("idCliente", iId))
                         select q;
            
            return oQuery.ToList<MedDAL.DAL.clientes_contacto>();
        }

        /// <summary>
        /// Intenta eliminar un registro de la base de datos.
        /// </summary>
        /// <param name="oClienteContacto"></param>
        /// <returns></returns>
        public bool EliminarRegistro(DAL.clientes_contacto oClienteContacto)
        {
            try
            {
                var oQuery = from q in oMedicuriEntities.clientes_contacto.
                            Where("it.idContacto = @idContacto",
                            new ObjectParameter("idContacto", oClienteContacto.idContacto))
                             select q;

                DAL.clientes_contacto oClienteOriginal = oQuery.First<DAL.clientes_contacto>();

                oMedicuriEntities.DeleteObject(oClienteOriginal);
                oMedicuriEntities.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Intenta eliminar un registro de la base de datos.
        /// </summary>
        /// <param name="oClienteContacto"></param>
        /// <returns></returns>
        public bool EliminarSimultaneos(int idCliente)
        {
            try
            {
                var oQuery = from q in oMedicuriEntities.clientes_contacto.
                            Where("it.idCliente = @idCliente",
                            new ObjectParameter("idCliente", idCliente))
                             select q;

                foreach(DAL.clientes_contacto contacto in oQuery.ToList<DAL.clientes_contacto>())
                    oMedicuriEntities.DeleteObject(contacto);
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
