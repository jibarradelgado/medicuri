using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;

namespace MedDAL.Causes
{
    public class DALCauses
    {
        DAL.medicuriEntities oMedicuriEntities;

        public DALCauses() {
            oMedicuriEntities = new DAL.medicuriEntities();
        }

        /// <summary>
        /// DAL - Insertar nuevo registro
        /// </summary>
        /// <param name="oCauses">Registro a guardar</param>
        /// <returns>true registrado, false no registrado</returns>
        public bool NuevoRegistro(DAL.causes oCauses)
        {
            try
            {
                //Agregar el registro
                oMedicuriEntities.AddTocauses(oCauses);
                oMedicuriEntities.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Eliminar un cause
        /// </summary>
        /// <param name="iIdCause">Id cause a eliminar</param>
        /// <returns></returns>
        public bool EliminarRegistro(int idCause)
        {
            try
            {
                var oQuery = from q in oMedicuriEntities.causes.
                              Where("it.idCause=@idCause",
                              new ObjectParameter("idCause", idCause))
                             select q;

                DAL.causes oCauseOriginal = oQuery.First<DAL.causes>();
                oMedicuriEntities.DeleteObject(oCauseOriginal);
                oMedicuriEntities.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Validar cause
        /// </summary>REVISAR SEVERAMENTE
        /// <param name="idCause">Nombre de cause</param>
        /// <returns>true si es repetido, false si no</returns>
        public bool ValidarCauseRepetido(string claveCause)
        {           
            //Recuperar el objeto a editar
            var oQuery = from q in oMedicuriEntities.causes.
                            Where("it.Clave = @Clave",
                            new ObjectParameter("Clave", claveCause))
                            select q;

            return oQuery.Count() > 0 ? true : false;
        }

        /// <summary>
        /// Acutualiza un cause en la DB
        /// </summary>
        /// <param name="oCause"> Cause a actualizar</param>
        /// <returns></returns>
        public bool EditarRegistro(DAL.causes oCause)
        {
            try
            {

                //Recuperar el objeto a editar
                var oQuery = from q in oMedicuriEntities.causes.
                             Where("it.idCause=@idCause",
                             new ObjectParameter("idCause", oCause.idCause))
                             select q;


                DAL.causes oCauseOriginal = oQuery.First<DAL.causes>();

                //Datos Cause
                oCauseOriginal.Clave = oCause.Clave;
                oCauseOriginal.Nombre = oCause.Nombre;
                oCauseOriginal.Conglomerado = oCause.Conglomerado;
                oCauseOriginal.Descripcion = oCause.Descripcion;

                oMedicuriEntities.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Obtiene los causes que coincidan con la búsqueda y el filtro
        /// </summary>
        /// <param name="sCadena">La cadena por la cual buscar</param>
        /// <param name="iFiltro">El filtro a aplicar: 1=todo, 2=Clave, 3=Nombre</param>
        /// <returns></returns>
        public IQueryable<MedDAL.DAL.causes> Buscar(string sCadena, int iFiltro)
        {
            //List<MedDAL.DAL.causes> lstCauses = new List<DAL.causes>();

            string sConsulta = "";
            switch (iFiltro)
            {
                case 1:
                    sConsulta = "(it.Nombre LIKE '%'+@Dato+'%' OR it.Clave LIKE '%'+@Dato+'%')";
                    break;
                case 2:
                    sConsulta = "it.Clave LIKE '%'+@Dato+'%'";
                    break;
                case 3:
                    sConsulta = "it.Nombre LIKE '%'+@Dato+'%'";
                    break;
            }

            var oQuery = from q in oMedicuriEntities.causes.
                                      Where(sConsulta,
                                      new ObjectParameter("Dato", sCadena))
                         select q;

            //lstCauses.AddRange(oQuery);

            return oQuery;
        }

        /// <summary>
        /// Buscar un cause por medio de su clave
        /// </summary>
        /// <param name="sClave"></param>
        /// <returns></returns>
        public MedDAL.DAL.causes Buscar(string sClave) 
        {
            var oQuery = from q in oMedicuriEntities.causes
                         where q.Clave.ToUpper() == sClave.ToUpper()
                         select q;

            return oQuery.Count<MedDAL.DAL.causes>() != 0 ? oQuery.First<MedDAL.DAL.causes>() : null;
        }

        public DAL.causes Buscar(int idCause)
        {
            var oQuery = from q in oMedicuriEntities.causes
                         where q.idCause == idCause
                         select q;

            return oQuery.First<DAL.causes>();
        }

        /// <summary>
        /// DAL Metodo que regresa las claves que contengan el parametro
        /// </summary>
        /// <param name="sCadena">Cadena que contenga la clave</param>
        /// <returns></returns>
        public string[] BuscarClaveCausesAsincrono(string sCadena)
        {
            string[] asResultados;

            try
            {
                var oQuery = from q in oMedicuriEntities.causes
                             where q.Clave.Contains(sCadena)
                             select q.Clave + " " + q.Nombre;


                return asResultados = oQuery.ToArray<string>();

            }
            catch
            {
                return asResultados = new string[0];
            }
        }

        /// <summary>
        /// DAL Metodo que regresa las claves que contengan el parametro
        /// </summary>
        /// <param name="sCadena">Cadena que contenga la clave</param>
        /// <returns></returns>
        public string[] BuscarClaveDescripcionCausesCie(string sCadena)
        {
            string[] asResultados;

            try
            {
                var oQuery = from q in oMedicuriEntities.causes_cie
                             where q.Clave.Contains(sCadena)
                             select q.Clave + " " + q.Descripcion;


                return asResultados = oQuery.ToArray<string>();

            }
            catch
            {
                return asResultados = new string[0];
            }
        }
    }
}
