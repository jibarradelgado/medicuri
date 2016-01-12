using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;

namespace MedDAL.Ensambles
{
    public class DALEnsambles
    {
        DAL.medicuriEntities oMedicuriEntities;

        public DALEnsambles() {
            oMedicuriEntities = new DAL.medicuriEntities();
        }

        /// <summary>
        /// DAL - Insertar nuevo registro
        /// </summary>
        /// <param name="oEnsamble">Registro a guardar</param>
        /// <returns>true registrado, false no registrado</returns>
        public bool NuevoRegistro(DAL.ensamble oEnsamble)
        {
            try
            {
                //Agregar el registro
                oMedicuriEntities.AddToensamble(oEnsamble);
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
        /// <param name="claveBom">claveBom por la cual buscar</param>
        /// <returns></returns>
        public object Buscar(string claveBom)
        {
            string sConsulta = "it.ClaveBom LIKE '%'+@Dato+'%'";
            var oQuery = from q in oMedicuriEntities.ensamble.Where(sConsulta,
                                      new ObjectParameter("Dato", claveBom))
                         select q;
            return oQuery;
        }

        /// <summary> GT
        /// Obtiene los ensambles que coincidan con la búsqueda y el filtro
        /// </summary>
        /// <param name="claveBom">claveBom por la cual buscar</param>
        /// <returns></returns>
        public DAL.ensamble BuscarEnsamble1(string sClaveBom)
        {
            try
            {
            var oQuery = from q in oMedicuriEntities.ensamble
                         where q.ClaveBom == sClaveBom
                         select q;
            return oQuery.First<DAL.ensamble>();
            }
            catch
            {
                return null;
            }
        }

        /// <summary> GT
        /// Obtiene los ensambles que coincidan con la búsqueda 
        /// </summary>
        /// <param name="sNombre">Nombre por la cual buscar</param>
        /// <returns></returns>
        public DAL.ensamble BuscarNombre(string sNombre)
        {
            try
            {
            var oQuery = from q in oMedicuriEntities.ensamble
                         where q.Descripcion.Contains(sNombre)
                         select q;
            return oQuery.First<DAL.ensamble>();
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Obtiene los ensambles que coincidan con la búsqueda y el filtro
        /// </summary>
        /// <param name="idEnsamble">idEnsamble por la cual buscar</param>
        /// <returns></returns>
        public DAL.ensamble Buscar(int idEnsamble)
        {
            try
            {
                var oQuery = from q in oMedicuriEntities.ensamble
                             where q.idEnsamble == idEnsamble
                             select q;
                return oQuery.First<DAL.ensamble>();
            }
            catch {
                return null;
            }
        }

        /// <summary>
        /// Busca todos los ensambles que estén activos
        /// </summary>
        /// <returns>La coleccion de ensambles</returns>
        public IQueryable<DAL.ensamble> BuscarEnum()
        {
            var oQuery = from q in oMedicuriEntities.ensamble
                         select q;

            return oQuery;
        }

        /// <summary>
        /// Eliminar un ensamble
        /// </summary>
        /// <param name="iIdEnsamble">Id ensamble a eliminar</param>
        /// <returns></returns>
        public bool EliminarRegistro(int iIdEnsamble)
        {
            try
            {
                var oQuery = from q in oMedicuriEntities.ensamble
                              where q.idEnsamble.Equals(iIdEnsamble)
                             select q;

                DAL.ensamble oEnsambleOriginal = oQuery.First<DAL.ensamble>();
                oMedicuriEntities.DeleteObject(oEnsambleOriginal);
                oMedicuriEntities.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Validar ensamble
        /// </summary>REVISAR SEVERAMENTE
        /// <param name="claveBom">ClaveBom de ensamble</param>
        /// <returns>Object</returns>
        public int ValidarEnsambleRepetido(string claveBom)
        {
            try
            {

                //Recuperar el objeto a editar
                var oQuery = from q in oMedicuriEntities.ensamble where q.ClaveBom.Equals(claveBom)
                             select q;

                return oQuery.Count();

            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// Acutualiza un ensamble en la DB
        /// </summary>
        /// <param name="oEnsamble"> Ensamble a actualizar</param>
        /// <returns></returns>
        public bool EditarRegistro(DAL.ensamble oEnsamble)
        {
            try
            {

                //Recuperar el objeto a editar
                var oQuery = from q in oMedicuriEntities.ensamble where q.idEnsamble.Equals(oEnsamble.idEnsamble)
                             select q;


                DAL.ensamble oEnsambleOriginal = oQuery.First<DAL.ensamble>();

                //Actualiza ensamble
                oEnsambleOriginal.ClaveBom = oEnsamble.ClaveBom;
                oEnsambleOriginal.Descripcion = oEnsamble.Descripcion;
                oEnsambleOriginal.UnidadMedida = oEnsamble.UnidadMedida;

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
