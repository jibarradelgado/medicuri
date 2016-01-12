using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using System.Data.Objects.DataClasses;

namespace MedDAL.Almacenes
{
    public class DALAlmacenes
    {

        DAL.medicuriEntities oMedicuriEntities;

        /// <summary>
        /// Constructor
        /// </summary>
        public DALAlmacenes()
        {
            oMedicuriEntities = new DAL.medicuriEntities();
        }

        /// <summary>
        /// Busca todos los almacenes que estén activos
        /// </summary>
        /// <returns>La coleccion de perfiles IQueryable</returns>
        public IQueryable<DAL.almacenes> BuscarAlmacenesActivos()
        {
            var oQuery = from q in oMedicuriEntities.almacenes
                         where q.Activo == true
                         
                         select q;

            return oQuery;
        }

        public IQueryable<DAL.almacenes> BuscarAlmacenesActivosFiltrado(int iIdAlmacen)
        {
            var oQuery = from q in oMedicuriEntities.almacenes
                         where q.idAlmacen == iIdAlmacen
                         select q;
            return oQuery;
        }

        /*/// <summary>
        /// Obtiene todos los Tipos que sean de almacen. 
        /// </summary>
        /// <returns></returns>
        public IQueryable<DAL.tipos> GetTiposAlmacen()
        {
            var oQuery = from q in oMedicuriEntities.tipos
                         where q.Almacenes==true
                         select q ;
                return oQuery;
        }*/

        /// <summary>
        /// Obtiene todos los almacenes registrados
        /// </summary>
        /// <returns></returns>
        public List<DAL.almacenes> ObtenerAlmacenes()
        {
            List<MedDAL.DAL.almacenes> lstAlmacenes = new List<DAL.almacenes>();

            var oQuery = from q in oMedicuriEntities.almacenes
                         select q;

            lstAlmacenes.AddRange(oQuery);

            return lstAlmacenes;
        }

       

        /// <summary>
        /// Busca a los almacenes que coincidan con la cadena y el filtro especificado.
        /// Si la cadena es vacía, regresa todos los registros.
        /// </summary>
        /// <param name="sCadena">La cadena por la cual buscar</param>
        /// <param name="iFiltro"></param>
        /// <returns></returns>
        public IQueryable<AlmacenesView> Buscar(string sCadena, int iFiltro)
        {
            //List<MedDAL.DAL.almacenes> lstAlmacenes = new List<DAL.almacenes>();
            string sConsulta = "";
            switch (iFiltro)
            {
                case 1:
                    IQueryable<AlmacenesView> oQuery = sCadena != "" ? from q in oMedicuriEntities.almacenes
                                                 where q.tipos.Nombre.Contains(sCadena)
                                                 select new AlmacenesView 
                                                 {
                                                    idAlmacen = q.idAlmacen,
                                                    idEstados = (int)q.idEstado,
                                                    idMunicipios = (int)q.idMunicipio,
                                                    idColonias = (int)q.idColonia,
                                                    idPoblaciones = (int)q.idPoblacion,
                                                    idTipos = q.idTipoAlmacen,
                                                    Clave = q.Clave,
                                                    Nombre = q.Nombre,
                                                    Telefono = q.Telefono,
                                                    Estado = q.estados.Nombre,
                                                    Poblacion = q.poblaciones.Nombre,
                                                    Tipo = q.tipos.Nombre,
                                                    Activo = q.Activo
                                                 } :
                                                 from q in oMedicuriEntities.almacenes
                                                 select new AlmacenesView
                                                 {
                                                     idAlmacen = q.idAlmacen,
                                                     idEstados = (int)q.idEstado,
                                                     idMunicipios = (int)q.idMunicipio,
                                                     idColonias = (int)q.idColonia,
                                                     idPoblaciones = (int)q.idPoblacion,
                                                     idTipos = q.idTipoAlmacen,
                                                     Clave = q.Clave,
                                                     Nombre = q.Nombre,
                                                     Telefono = q.Telefono,
                                                     Estado = q.estados.Nombre,
                                                     Poblacion = q.poblaciones.Nombre,
                                                     Tipo = q.tipos.Nombre,
                                                     Activo = q.Activo
                                                 };
                    return oQuery;                    
                case 2:
                    sConsulta = "it.Clave LIKE '%'+@Dato+'%'";
                    oQuery = from q in oMedicuriEntities.almacenes.Where(sConsulta, new ObjectParameter("Dato", sCadena)) select new AlmacenesView
                                                 {
                                                     idAlmacen = q.idAlmacen,
                                                     idEstados = (int)q.idEstado,
                                                     idMunicipios = (int)q.idMunicipio,
                                                     idColonias = (int)q.idColonia,
                                                     idPoblaciones = (int)q.idPoblacion,
                                                     idTipos = q.idTipoAlmacen,
                                                     Clave = q.Clave,
                                                     Nombre = q.Nombre,
                                                     Telefono = q.Telefono,
                                                     Estado = q.estados.Nombre,
                                                     Poblacion = q.poblaciones.Nombre,
                                                     Tipo = q.tipos.Nombre,
                                                     Activo = q.Activo
                                                 };
                    //lstAlmacenes.AddRange(oQuery);
                    return oQuery;                    
                case 3:
                    sConsulta = "it.Nombre LIKE '%'+@Dato+'%'";
                    oQuery = from q in oMedicuriEntities.almacenes.Where(sConsulta, new ObjectParameter("Dato", sCadena))
                             select new AlmacenesView
                             {
                                 idAlmacen = q.idAlmacen,
                                 idEstados = (int)q.idEstado,
                                 idMunicipios = (int)q.idMunicipio,
                                 idColonias = (int)q.idColonia,
                                 idPoblaciones = (int)q.idPoblacion,
                                 idTipos = q.idTipoAlmacen,
                                 Clave = q.Clave,
                                 Nombre = q.Nombre,
                                 Telefono = q.Telefono,
                                 Estado = q.estados.Nombre,
                                 Poblacion = q.poblaciones.Nombre,
                                 Tipo = q.tipos.Nombre,
                                 Activo = q.Activo
                             };
                    return oQuery;
                default: return null;
            }            
        }


        public IQueryable<AlmacenesView> BuscarFiltradaAlmacenes(string sCadena, int iFiltro, int iIdAlmacen)
        {

            //object lstAlmacenes = new List<DAL.almacenes>();
            //string sConsulta = "";
            switch (iFiltro)
            {
                case 1:
                    IQueryable<AlmacenesView> oQuery = sCadena != "" ? 
                        from q in oMedicuriEntities.almacenes 
                        where q.tipos.Nombre.Contains(sCadena) && q.idAlmacen==iIdAlmacen
                        select new AlmacenesView
                        {
                            idAlmacen = q.idAlmacen,
                            idEstados = (int)q.idEstado,
                            idMunicipios = (int)q.idMunicipio,
                            idColonias = (int)q.idColonia,
                            idPoblaciones = (int)q.idPoblacion,
                            idTipos = q.idTipoAlmacen,
                            Clave = q.Clave,
                            Nombre = q.Nombre,
                            Telefono = q.Telefono,
                            Estado = q.estados.Nombre,
                            Poblacion = q.poblaciones.Nombre,
                            Tipo = q.tipos.Nombre,
                            Activo = q.Activo
                        } : from q in oMedicuriEntities.almacenes
                            where q.idAlmacen == iIdAlmacen
                            select new AlmacenesView
                            {
                                idAlmacen = q.idAlmacen,
                                idEstados = (int)q.idEstado,
                                idMunicipios = (int)q.idMunicipio,
                                idColonias = (int)q.idColonia,
                                idPoblaciones = (int)q.idPoblacion,
                                idTipos = q.idTipoAlmacen,
                                Clave = q.Clave,
                                Nombre = q.Nombre,
                                Telefono = q.Telefono,
                                Estado = q.estados.Nombre,
                                Poblacion = q.poblaciones.Nombre,
                                Tipo = q.tipos.Nombre,
                                Activo = q.Activo
                            };
                    return oQuery;                    
                case 2:
                    
                    oQuery = from q in oMedicuriEntities.almacenes
                             where q.Clave.Contains(sCadena) && q.idAlmacen == iIdAlmacen
                             select new AlmacenesView
                             {
                                 idAlmacen = q.idAlmacen,
                                 idEstados = (int)q.idEstado,
                                 idMunicipios = (int)q.idMunicipio,
                                 idColonias = (int)q.idColonia,
                                 idPoblaciones = (int)q.idPoblacion,
                                 idTipos = q.idTipoAlmacen,
                                 Clave = q.Clave,
                                 Nombre = q.Nombre,
                                 Telefono = q.Telefono,
                                 Estado = q.estados.Nombre,
                                 Poblacion = q.poblaciones.Nombre,
                                 Tipo = q.tipos.Nombre,
                                 Activo = q.Activo
                             };
                    return oQuery;                    
                case 3:
                    //sConsulta = "it.Nombre LIKE '%'+@Dato+'%'";
                    //oQuery = from q in oMedicuriEntities.almacenes.Where(sConsulta, new ObjectParameter("Dato", sCadena)) select q;
                    oQuery = from q in oMedicuriEntities.almacenes
                             where q.Nombre.Contains(sCadena) && q.idAlmacen == iIdAlmacen
                             select new AlmacenesView
                             {
                                 idAlmacen = q.idAlmacen,
                                 idEstados = (int)q.idEstado,
                                 idMunicipios = (int)q.idMunicipio,
                                 idColonias = (int)q.idColonia,
                                 idPoblaciones = (int)q.idPoblacion,
                                 idTipos = q.idTipoAlmacen,
                                 Clave = q.Clave,
                                 Nombre = q.Nombre,
                                 Telefono = q.Telefono,
                                 Estado = q.estados.Nombre,
                                 Poblacion = q.poblaciones.Nombre,
                                 Tipo = q.tipos.Nombre,
                                 Activo = q.Activo
                             };
                    return oQuery;                    
                default:
                    return null;
            }            
        }





        /// <summary>
        /// Obtiene al almacén que coincida con la clave
        /// </summary>
        /// <param name="sClave"></param>
        /// <returns>Un almacén si coincide con la clave, si no, null</returns>
        public MedDAL.DAL.almacenes Buscar(string sClave)
        {
            var oQuery = from q in oMedicuriEntities.almacenes
                         where q.Clave.ToUpper() == sClave.ToUpper()
                         select q;

            return oQuery.Count<MedDAL.DAL.almacenes>() != 0 ? oQuery.First<MedDAL.DAL.almacenes>() : null;
        }

        public MedDAL.DAL.almacenes Buscar(int idAlmacen)
        {
            var oQuery = from q in oMedicuriEntities.almacenes
                         where q.idAlmacen == idAlmacen
                         select q;

            return oQuery.Count<MedDAL.DAL.almacenes>() != 0 ? oQuery.First<MedDAL.DAL.almacenes>() : null;
        }

        /// <summary>
        /// Registra un nuevo almacen
        /// </summary>
        /// <param name="oAlmacen"></param>
        /// <returns></returns>
        public bool NuevoRegistro(MedDAL.DAL.almacenes oAlmacen)
        {
            try
            {
                oMedicuriEntities.AddToalmacenes(oAlmacen);
                oMedicuriEntities.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Actualiza un almacén 
        /// </summary>
        /// <param name="oAlmacen"></param>
        /// <returns></returns>
        public bool EditarRegistro(DAL.almacenes oAlmacen)
        {
            try
            {
                var oQuery = from q in oMedicuriEntities.almacenes.
                         Where("it.idAlmacen = @idAlmacen",
                         new ObjectParameter("idAlmacen", oAlmacen.idAlmacen))
                             select q;

                DAL.almacenes oAlmacenOriginal = oQuery.First<DAL.almacenes>();                
                oAlmacenOriginal.Nombre = oAlmacen.Nombre;                
                oAlmacenOriginal.Telefono = oAlmacen.Telefono;
                oAlmacenOriginal.Fax = oAlmacen.Fax;
                oAlmacenOriginal.Calle = oAlmacen.Calle;
                oAlmacenOriginal.NumeroExt = oAlmacen.NumeroExt;
                oAlmacenOriginal.NumeroInt = oAlmacen.NumeroInt;
                oAlmacenOriginal.CodigoPostal = oAlmacen.CodigoPostal;

                oAlmacenOriginal.Campo1 = oAlmacen.Campo1;
                oAlmacenOriginal.Campo2 = oAlmacen.Campo2;
                oAlmacenOriginal.Campo3 = oAlmacen.Campo3;
                oAlmacenOriginal.Campo4 = oAlmacen.Campo4;
                oAlmacenOriginal.Campo5 = oAlmacen.Campo5;
                oAlmacenOriginal.Campo6 = oAlmacen.Campo6;
                oAlmacenOriginal.Campo7 = oAlmacen.Campo7;
                oAlmacenOriginal.Campo8 = oAlmacen.Campo8;
                oAlmacenOriginal.Campo9 = oAlmacen.Campo9;
                oAlmacenOriginal.Campo10 = oAlmacen.Campo10;
                oAlmacenOriginal.Activo = oAlmacen.Activo;

                oAlmacenOriginal.idTipoAlmacen = oAlmacen.idTipoAlmacen;
                oAlmacenOriginal.idEstado = oAlmacen.idEstado;
                oAlmacenOriginal.idMunicipio = oAlmacen.idMunicipio;
                oAlmacenOriginal.idPoblacion = oAlmacen.idPoblacion;
                oAlmacenOriginal.idColonia = oAlmacen.idColonia;

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
        /// <param name="oAlmacen"></param>
        /// <returns></returns>
        public bool EliminarRegistro(DAL.almacenes oAlmacen)
        {
            try
            {
                var oQuery = from q in oMedicuriEntities.almacenes.
                            Where("it.idAlmacen = @idAlmacen",
                            new ObjectParameter("idAlmacen", oAlmacen.idAlmacen))
                             select q;

                DAL.almacenes oAlmacenOriginal = oQuery.First<DAL.almacenes>();

                oMedicuriEntities.DeleteObject(oAlmacenOriginal);
                oMedicuriEntities.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// DAL Metodo que regresa las claves que contengan el parametro
        /// </summary>
        /// <param name="sCadena">Cadena que contenga la clave</param>
        /// <returns></returns>
        public string[] BuscarClaveAlmacenesAsincrono(string sCadena)
        {
            string[] asResultados;

            try
            {
                var oQuery = from q in oMedicuriEntities.almacenes
                             where q.Clave.Contains(sCadena)
                             select q.Clave;


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
        public string[] BuscarClaveAlmacenesAsincrono(int iIdAlmacen)
        {
            string[] asResultados;

            try
            {
                var oQuery = from q in oMedicuriEntities.almacenes
                             where q.idAlmacen == iIdAlmacen
                             select q.Clave;


                return asResultados = oQuery.ToArray<string>();

            }
            catch
            {
                return asResultados = new string[0];
            }
        }
    }
}
