using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using System.Data.Objects.DataClasses;

namespace MedDAL.Proveedores
{
    public class DALProveedores
    {
        DAL.medicuriEntities oMedicuriEntities;
        
        public DALProveedores()
        {
            oMedicuriEntities = new DAL.medicuriEntities();
        }

        /// <summary>
        /// Obtiene todos los proveedores activos
        /// </summary>
        /// <returns></returns>
        public IQueryable<DAL.proveedores> ObtenerProveedoresActivos()
        {
            var oQuery = from q in oMedicuriEntities.proveedores
                         where q.Activo == true
                         select q;

            return oQuery;
        }

        public List<DAL.proveedores> ObtenerProveedores() 
        {
            List<MedDAL.DAL.proveedores> lstProveedores = new List<DAL.proveedores>();

            var oQuery = from q in oMedicuriEntities.proveedores
                         select q;

            lstProveedores.AddRange(oQuery);

            return lstProveedores;
        }

        /// <summary>
        /// Busca a los proveedores que coincidan con la cadena y el filtro especificado.
        /// Si la cadena es vacía, regresa todos los registros.
        /// </summary>
        /// <param name="sCadena">La cadena por la cual buscar</param>
        /// <param name="iFiltro"></param>
        /// <returns></returns>
        public IQueryable<ProveedoresView> Buscar(string sCadena, int iFiltro)
        {
            //List<MedDAL.DAL.proveedores> lstProveedores = new List<DAL.proveedores>();

            string sConsulta = "";
            switch (iFiltro)
            {
                case 1:
                    IQueryable<ProveedoresView> oQuery = sCadena != "" ? from q in oMedicuriEntities.proveedores
                                                                         where q.tipos.Nombre.Contains(sCadena)
                                                                         select new ProveedoresView
                                                                         {
                                                                             idProveedor = q.IdProveedor,                                                                             
                                                                             idEstado = q.IdEstado,
                                                                             idMunicipio = q.IdMunicipio,
                                                                             idPoblacion = q.IdPoblacion,
                                                                             idColonia = q.IdColonia,
                                                                             idTipoProveedor = q.IdTipoProveedor,
                                                                             Clave = q.Clave,
                                                                             Nombre = q.Nombre,
                                                                             Apellidos = q.Apellidos,
                                                                             TipoPersona = q.TipoPersona,
                                                                             RFC = q.Rfc,
                                                                             Telefono = q.Telefono,
                                                                             Celular = q.Celular,
                                                                             CorreoElectronico = q.CorreoElectronico,
                                                                             Tipo = q.tipos.Nombre,
                                                                             Fecha = q.Fecha == null ? new DateTime() : (DateTime)q.Fecha,
                                                                             Activo = q.Activo
                                                                         }
                                                                         :
                                                 from q in oMedicuriEntities.proveedores
                                                 select new ProveedoresView
                                                 {
                                                     idProveedor = q.IdProveedor,
                                                     idEstado = q.IdEstado,
                                                     idMunicipio = q.IdMunicipio,
                                                     idPoblacion = q.IdPoblacion,
                                                     idColonia = q.IdColonia,
                                                     idTipoProveedor = q.IdTipoProveedor,
                                                     Clave = q.Clave,
                                                     Nombre = q.Nombre,
                                                     Apellidos = q.Apellidos,
                                                     TipoPersona = q.TipoPersona,
                                                     RFC = q.Rfc,
                                                     Telefono = q.Telefono,
                                                     Celular = q.Celular,
                                                     CorreoElectronico = q.CorreoElectronico,
                                                     Tipo = q.tipos.Nombre,
                                                     Fecha = q.Fecha == null ? new DateTime() : (DateTime)q.Fecha,
                                                     Activo = q.Activo
                                                 };
                    return oQuery;                   
                case 2:
                    sConsulta = "it.Clave LIKE '%'+@Dato+'%'";
                    oQuery = from q in oMedicuriEntities.proveedores.Where(sConsulta, new ObjectParameter("Dato", sCadena))
                             select new ProveedoresView
                             {
                                 idProveedor = q.IdProveedor,
                                 idEstado = q.IdEstado,
                                 idMunicipio = q.IdMunicipio,
                                 idPoblacion = q.IdPoblacion,
                                 idColonia = q.IdColonia,
                                 idTipoProveedor = q.IdTipoProveedor,
                                 Clave = q.Clave,
                                 Nombre = q.Nombre,
                                 Apellidos = q.Apellidos,
                                 TipoPersona = q.TipoPersona,
                                 RFC = q.Rfc,
                                 Telefono = q.Telefono,
                                 Celular = q.Celular,
                                 CorreoElectronico = q.CorreoElectronico,
                                 Tipo = q.tipos.Nombre,
                                 Fecha = q.Fecha == null ? new DateTime() : (DateTime)q.Fecha,
                                 Activo = q.Activo
                             };
                    return oQuery;                   
                    
                case 3:
                    sConsulta = "it.Nombre LIKE '%'+@Dato+'%'";
                    oQuery = from q in oMedicuriEntities.proveedores.Where(sConsulta, new ObjectParameter("Dato", sCadena))
                             select new ProveedoresView
                             {
                                 idProveedor = q.IdProveedor,
                                 idEstado = q.IdEstado,
                                 idMunicipio = q.IdMunicipio,
                                 idPoblacion = q.IdPoblacion,
                                 idColonia = q.IdColonia,
                                 idTipoProveedor = q.IdTipoProveedor,
                                 Clave = q.Clave,
                                 Nombre = q.Nombre,
                                 Apellidos = q.Apellidos,
                                 TipoPersona = q.TipoPersona,
                                 RFC = q.Rfc,
                                 Telefono = q.Telefono,
                                 Celular = q.Celular,
                                 CorreoElectronico = q.CorreoElectronico,
                                 Tipo = q.tipos.Nombre,
                                 Fecha = q.Fecha == null ? new DateTime() : (DateTime)q.Fecha,
                                 Activo = q.Activo
                             };
                    return oQuery;
            }
            return null ;
        }

        /// <summary>
        /// Obtiene al proveedor que coincida con la clave
        /// </summary>
        /// <param name="sClave"></param>
        /// <returns>Un proveedor si coincide con la clave, si no, null</returns>
        public MedDAL.DAL.proveedores Buscar(string sClave)
        {
            var oQuery = from q in oMedicuriEntities.proveedores
                         where q.Clave.ToUpper() == sClave.ToUpper()
                         select q;

            return oQuery.Count<MedDAL.DAL.proveedores>() != 0 ? oQuery.First<MedDAL.DAL.proveedores>() : null;
        }

        public MedDAL.DAL.proveedores Buscar(int idProveedor)
        {
            var oQuery = from q in oMedicuriEntities.proveedores
                         where q.IdProveedor == idProveedor
                         select q;

            return oQuery.First<MedDAL.DAL.proveedores>();
        }

        /// <summary>
        /// Registra un nuevo proveedor
        /// </summary>
        /// <param name="oProveedor"></param>
        /// <returns></returns>
        public bool NuevoRegistro(MedDAL.DAL.proveedores oProveedor)
        {
            try
            {
                oMedicuriEntities.AddToproveedores(oProveedor);
                oMedicuriEntities.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Actualiza un proveedor 
        /// </summary>
        /// <param name="oProveedor"></param>
        /// <returns></returns>
        public bool EditarRegistro(DAL.proveedores oProveedor)
        {
            try
            {
                var oQuery = from q in oMedicuriEntities.proveedores.
                         Where("it.idProveedor = @idProveedor",
                         new ObjectParameter("idProveedor", oProveedor.IdProveedor))
                             select q;

                DAL.proveedores oProveedorOriginal = oQuery.First<DAL.proveedores>();
                //oProveedorOriginal.Clave = oProveedor.Clave;
                oProveedorOriginal.Nombre = oProveedor.Nombre;
                oProveedorOriginal.Apellidos = oProveedor.Apellidos;
                oProveedorOriginal.Rfc = oProveedor.Rfc;
                oProveedorOriginal.Curp = oProveedor.Curp;
                oProveedorOriginal.Telefono = oProveedor.Telefono;
                oProveedorOriginal.Celular = oProveedor.Celular;
                oProveedorOriginal.Fax = oProveedor.Fax;
                oProveedorOriginal.CorreoElectronico = oProveedor.CorreoElectronico;
                oProveedorOriginal.Calle = oProveedor.Calle;
                oProveedorOriginal.NumeroExt = oProveedor.NumeroExt;
                oProveedorOriginal.NumeroInt = oProveedor.NumeroInt;
                oProveedorOriginal.CodigoPostal = oProveedor.CodigoPostal;
                oProveedorOriginal.TipoPersona = oProveedor.TipoPersona;

                oProveedorOriginal.Campo1 = oProveedor.Campo1;
                oProveedorOriginal.Campo2 = oProveedor.Campo2;
                oProveedorOriginal.Campo3 = oProveedor.Campo3;
                oProveedorOriginal.Campo4 = oProveedor.Campo4;
                oProveedorOriginal.Campo5 = oProveedor.Campo5;
                oProveedorOriginal.Campo6 = oProveedor.Campo6;
                oProveedorOriginal.Campo7 = oProveedor.Campo7;
                oProveedorOriginal.Campo8 = oProveedor.Campo8;
                oProveedorOriginal.Campo9 = oProveedor.Campo9;
                oProveedorOriginal.Campo10 = oProveedor.Campo10;
                oProveedorOriginal.Activo = oProveedor.Activo;

                oProveedorOriginal.IdTipoProveedor = oProveedor.IdTipoProveedor;
                oProveedorOriginal.IdEstado = oProveedor.IdEstado;
                oProveedorOriginal.IdMunicipio = oProveedor.IdMunicipio;
                oProveedorOriginal.IdPoblacion = oProveedor.IdPoblacion;
                oProveedorOriginal.IdColonia = oProveedor.IdColonia;

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
        /// <param name="oProveedor"></param>
        /// <returns></returns>
        public bool EliminarRegistro(DAL.proveedores oProveedor)
        {
            try
            {
                var oQuery = from q in oMedicuriEntities.proveedores.
                            Where("it.idProveedor = @idProveedor",
                            new ObjectParameter("idProveedor", oProveedor.IdProveedor))
                             select q;

                DAL.proveedores oProveedorOriginal = oQuery.First<DAL.proveedores>();

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
        /// Busqueda asincrona de proveedor
        /// </summary>
        /// <param name="sCadena">codigo</param>
        /// <returns></returns>
        public string[] BuscarProveedorAsincrono(string sCadena)
        {
            string[] asResultados;

            try
            {
                var oQuery = from q in oMedicuriEntities.proveedores
                             where q.Clave.Contains(sCadena)
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
