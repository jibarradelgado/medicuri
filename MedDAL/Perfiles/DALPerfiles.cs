using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using System.Data.Objects.DataClasses;

namespace MedDAL.Perfiles
{
    public class DALPerfiles
    {
        DAL.medicuriEntities oMedicuriEntities;

        /// <summary>
        /// Constructor
        /// </summary>
        public DALPerfiles()
        {
            oMedicuriEntities = new DAL.medicuriEntities();
        }

        /// <summary>
        /// DAL - Insertar nuevo registro
        /// </summary>
        /// <param name="oPerfil">Perfil a guardar</param>
        /// <returns>true registrado, false no registrado</returns>
        public bool NuevoRegistro(DAL.perfiles oPerfil)
        {
            try
            {
                //Agregar el registro
                oMedicuriEntities.AddToperfiles(oPerfil);
                oMedicuriEntities.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        /// Buscar mediante filtros parametros
        /// </summary>
        /// <param name="sCadena">Parametro a buscar</param>
        /// <param name="iFiltro">Filtro</param>
        /// <returns>var oQuery</returns>
        public object Buscar(string sCadena, int iFiltro)
        {
            string sConsulta = "";
            switch (iFiltro)
            {
                //Filtro 1
                case 1:
                    sConsulta = "it.Nombre LIKE '%'+@Dato+'%'";
                    break;
                //Filtro 2
                case 2:
                    sConsulta = "it.Descrpcion LIKE '%'+@Dato+'%'";
                    break;
                //Filtro 3
                /*case 3:
                    sConsulta = "it.CorreoElectronico LIKE '%'+@Dato+'%'";
                    break;*/
            }

            var oQuery = from q in oMedicuriEntities.perfiles.Where(sConsulta, new ObjectParameter("Dato", sCadena))
                         select q;

            return oQuery;
        }


        /// <summary>
        /// Busca todos los perfiles que estén activos
        /// </summary>
        /// <returns>La coleccion de perfiles</returns>
        public IQueryable<DAL.perfiles> BuscarEnum()
        {
            var oQuery = from q in oMedicuriEntities.perfiles
                         where q.Activo == true
                         select q;

            return oQuery;
        }

        /// <summary>
        /// DAL - Muestra todos los registros de la tabla usuarios
        /// </summary>
        /// <returns>Objeto</returns>
        public object MostrarLista()
        {

            var oQuery = from q in oMedicuriEntities.perfiles select q;
            return oQuery;

        }


        /// <summary>
        /// Recuperar perfil mediante su llave primaria
        /// </summary>
        /// <param name="iId">Llave primaria</param>
        /// <returns></returns>
        public object Buscar(int iId)
        {
            var oQuery = from q in oMedicuriEntities.perfiles
                         where q.idPerfil == iId
                         select q;

            return (object)oQuery.First<MedDAL.DAL.perfiles>();
        }


        /// <summary>
        /// Recuperar perfil mediante su nombre
        /// </summary>
        /// <param name="sNombre">Nombre Perfil</param>
        /// <returns></returns>
        public object Buscar(string sNombre)
        {
            var oQuery = from q in oMedicuriEntities.perfiles
                         where q.Nombre == sNombre
                         select q;

            return (object)oQuery.First<MedDAL.DAL.perfiles>();
        }

        /// <summary>
        /// Editar perfiles
        /// </summary>
        /// <param name="oPerfil">Perfil a editar</param>
        /// <returns></returns>
        public bool EditarRegistro(DAL.perfiles oPerfil)
        {
            try
            {

                //Recuperar el objeto a editar
                var oQuery = from q in oMedicuriEntities.perfiles.
                             Where("it.idPerfil=@idPerfil",
                             new ObjectParameter("idPerfil", oPerfil.idPerfil))
                             select q;

                DAL.perfiles oPerfilOriginal = oQuery.First<DAL.perfiles>();

                //Modificar los valores
                oPerfilOriginal.Descrpcion = oPerfil.Descrpcion;
                oMedicuriEntities.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        /// Eliminar un perfil
        /// </summary>
        /// <param name="iIdPerfil">Id perfil a eliminar</param>
        /// <returns></returns>
        public bool EliminarRegistro(int iIdPerfil)
        {
            try
            {
                var oQuery = from q in oMedicuriEntities.perfiles.
                              Where("it.idPerfil=@idPerfil",
                              new ObjectParameter("idPerfil", iIdPerfil))
                             select q;

                DAL.perfiles oPerfilOriginal = oQuery.First<DAL.perfiles>();
                oMedicuriEntities.DeleteObject(oPerfilOriginal);
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

