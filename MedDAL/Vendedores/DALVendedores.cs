using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;

namespace MedDAL.Vendedores
{
    public class DALVendedores
    {
        DAL.medicuriEntities oMedicuriEntities;

        public DALVendedores()
        {
            oMedicuriEntities = new DAL.medicuriEntities();
        }

        /// <summary>
        /// DAL - Insertar nuevo registro
        /// </summary>
        /// <param name="oVendedor">Registro a guardar</param>
        /// <returns>true registrado, false no registrado</returns>
        public bool NuevoRegistro(DAL.vendedores oVendedor)
        {
            try
            {
                //Agregar el registro
                oMedicuriEntities.AddTovendedores(oVendedor);
                oMedicuriEntities.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }



        /// <summary>
        /// Obtiene los estados que coincidan con la búsqueda y el filtro
        /// </summary>
        /// <param name="sCadena">La cadena por la cual buscar</param>
        /// <param name="iFiltro">El filtro a aplicar: 1=todo, 2=Clave, 3=Nombre</param>
        /// <returns></returns>
        public IQueryable<VendedoresView> Buscar(string sCadena, int iFiltro)
        {
            string sConsulta = "";
            switch (iFiltro)
            {
                case 1:
                    var oQuery = sCadena != "" ? from q in oMedicuriEntities.vendedores 
                                                 where q.tipos.Nombre.Contains(sCadena) 
                                                 select new VendedoresView
                                                    {
                                                         idVendedor = q.idVendedor,
                                                         idEstado = (int)q.IdEstado,
                                                         idMunicipio = (int)q.IdMunicipio,
                                                         idPoblacion = (int)q.IdPoblacion,
                                                         idColonia = (int)q.IdColonia,
                                                         Clave = q.Clave,
                                                         Nombre = q.Nombre,
                                                         Apellidos = q.Apellidos,
                                                         TipoPersona = q.TipoPersona,
                                                         RFC = q.Rfc,
                                                         Telefono = q.Telefono,
                                                         Celular = q.Celular,
                                                         CorreoElectronico = q.CorreoElectronico,
                                                         TipoVendedor = q.tipos.Nombre,
                                                         FechaAlta = (DateTime)q.FechaAlta,
                                                         Activo = q.Activo
                                                    } 
                                                 : 
                                                 from q in oMedicuriEntities.vendedores
                                                 select new VendedoresView
                                                 {
                                                     idVendedor = q.idVendedor,
                                                     idEstado = (int)q.IdEstado,
                                                     idMunicipio = (int)q.IdMunicipio,
                                                     idPoblacion = (int)q.IdPoblacion,
                                                     idColonia = (int)q.IdColonia,
                                                     Clave = q.Clave,
                                                     Nombre = q.Nombre,
                                                     Apellidos = q.Apellidos,
                                                     TipoPersona = q.TipoPersona,
                                                     RFC = q.Rfc,
                                                     Telefono = q.Telefono,
                                                     Celular = q.Celular,
                                                     CorreoElectronico = q.CorreoElectronico,
                                                     TipoVendedor = q.tipos.Nombre,
                                                     FechaAlta = (DateTime)q.FechaAlta,
                                                     Activo = q.Activo
                                                 };
                    return oQuery;                    
                case 2:
                    sConsulta = "it.Clave LIKE '%'+@Dato+'%'";
                    oQuery = from q in oMedicuriEntities.vendedores.Where(sConsulta, new ObjectParameter("Dato", sCadena))
                             select new VendedoresView
                             {
                                 idVendedor = q.idVendedor,
                                 idEstado = (int)q.IdEstado,
                                 idMunicipio = (int)q.IdMunicipio,
                                 idPoblacion = (int)q.IdPoblacion,
                                 idColonia = (int)q.IdColonia,
                                 Clave = q.Clave,
                                 Nombre = q.Nombre,
                                 Apellidos = q.Apellidos,
                                 TipoPersona = q.TipoPersona,
                                 RFC = q.Rfc,
                                 Telefono = q.Telefono,
                                 Celular = q.Celular,
                                 CorreoElectronico = q.CorreoElectronico,
                                 TipoVendedor = q.tipos.Nombre,
                                 FechaAlta = (DateTime)q.FechaAlta,
                                 Activo = q.Activo
                             };
                    return oQuery;                    
                case 3:
                    sConsulta = "it.Nombre LIKE '%'+@Dato+'%'";
                    oQuery = from q in oMedicuriEntities.vendedores.Where(sConsulta, new ObjectParameter("Dato", sCadena))
                             select new VendedoresView
                             {
                                 idVendedor = q.idVendedor,
                                 idEstado = (int)q.IdEstado,
                                 idMunicipio = (int)q.IdMunicipio,
                                 idPoblacion = (int)q.IdPoblacion,
                                 idColonia = (int)q.IdColonia,
                                 Clave = q.Clave,
                                 Nombre = q.Nombre,
                                 Apellidos = q.Apellidos,
                                 TipoPersona = q.TipoPersona,
                                 RFC = q.Rfc,
                                 Telefono = q.Telefono,
                                 Celular = q.Celular,
                                 CorreoElectronico = q.CorreoElectronico,
                                 TipoVendedor = q.tipos.Nombre,
                                 FechaAlta = (DateTime)q.FechaAlta,
                                 Activo = q.Activo
                             };
                    return oQuery;
                default: return null;
            }            
        }

        /// <summary>
        /// Obtiene un vendedor. 
        /// </summary>
        /// <param name="iId">El id del vendedores</param>
        /// <returns></returns>
        public object Buscar(int iId)
        {
            var oQuery = from q in oMedicuriEntities.vendedores
                         where q.idVendedor == iId
                         select q;

            return (object)oQuery.First<MedDAL.DAL.vendedores>();
        }

        /// <summary>
        /// Busca todos los vendedores que estén activos
        /// </summary>
        /// <returns>La coleccion de estados</returns>
        public IQueryable<DAL.vendedores> BuscarEnum()
        {
            var oQuery = from q in oMedicuriEntities.vendedores
                         where q.Activo == true
                         select q;

            return oQuery;
        }

        /// <summary>
        /// Busca todos los vendedores que estén activos
        /// </summary>
        /// <return>El resultado  de la búsqueda</return>
        public object Buscar()
        {
            var oQuery = from q in oMedicuriEntities.vendedores
                         where q.Activo == true
                         select q;

            return oQuery;
        }

        public MedDAL.DAL.vendedores Buscar(string cedProf)
        {
            try
            {
                var oQuery = from q in oMedicuriEntities.vendedores
                             where q.Activo == true && q.CedulaProfesional.Equals(cedProf)
                             select q;

                return oQuery.First();
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 0415 GT 15-Ago-11
        /// Metodo que recupera un vendedor mediante su nombre
        /// </summary>
        /// <param name="sNombre"></param>
        /// <returns></returns>
        public MedDAL.DAL.vendedores BuscarVendedorNombre(string sNombre)
        {
            try
            {
                var oQuery = from q in oMedicuriEntities.vendedores
                             where q.Activo == true && q.Nombre.Equals(sNombre)
                             select q;

                return oQuery.First();
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// DAL - Muestra todos los registros de la tabla vendedores
        /// </summary>
        /// <returns>Objeto</returns>
        public object MostrarLista()
        {

            var oQuery = from q in oMedicuriEntities.vendedores select q;
            return oQuery;

        }

        /// <summary>
        /// Eliminar un vendedor
        /// </summary>
        /// <param name="iIdVendedor">Id vendedor a eliminar</param>
        /// <returns></returns>
        public bool EliminarRegistro(int iIdvendedor)
        {
            try
            {
                var oQuery = from q in oMedicuriEntities.vendedores.
                              Where("it.idVendedor=@idVendedor",
                              new ObjectParameter("idVendedor", iIdvendedor))
                             select q;

                DAL.vendedores oVendedorOriginal = oQuery.First<DAL.vendedores>();
                oMedicuriEntities.DeleteObject(oVendedorOriginal);
                oMedicuriEntities.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Validar vendedor
        /// </summary>REVISAR SEVERAMENTE
        /// <param name="idVendedor">Nombre de vendedor</param>
        /// <returns>Object</returns>
        public int ValidarVendedorRepetido(string claveVendedor)
        {
            try
            {
                
                //Recuperar el objeto a editar
                var oQuery = from q in oMedicuriEntities.vendedores.
                             Where("it.Clave=@Clave",
                             new ObjectParameter("Clave", claveVendedor))
                             select q;

                return oQuery.Count();

            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// Acutualiza un vendedor en la DB
        /// </summary>
        /// <param name="oVendedores"> Vendedor a actualizar</param>
        /// <returns></returns>
        public bool EditarRegistro(DAL.vendedores oVendedores)
        {
            try
            {

                //Recuperar el objeto a editar
                var oQuery = from q in oMedicuriEntities.vendedores.
                             Where("it.idVendedor=@idVendedor",
                             new ObjectParameter("idVendedor", oVendedores.idVendedor))
                             select q;


                DAL.vendedores oVendedorOriginal = oQuery.First<DAL.vendedores>();

                //Datos vendedor
                oVendedorOriginal.Nombre = oVendedores.Nombre;
                oVendedorOriginal.Apellidos = oVendedores.Apellidos;
                oVendedorOriginal.IdTipoVendedor = oVendedores.IdTipoVendedor;
                oVendedorOriginal.Activo = oVendedores.Activo;

                //Datos de contacto
                oVendedorOriginal.Calle = oVendedores.Calle;
                oVendedorOriginal.NumeroInt = oVendedores.NumeroInt;
                oVendedorOriginal.NumeroExt = oVendedores.NumeroExt;
                oVendedorOriginal.IdEstado = oVendedores.IdEstado;
                oVendedorOriginal.IdMunicipio = oVendedores.IdMunicipio;
                oVendedorOriginal.IdPoblacion = oVendedores.IdPoblacion;
                oVendedorOriginal.IdColonia = oVendedores.IdColonia;
                oVendedorOriginal.CodigoPostal = oVendedores.CodigoPostal;

                oVendedorOriginal.Telefono = oVendedores.Telefono;
                oVendedorOriginal.Celular = oVendedores.Celular;
                oVendedorOriginal.Fax = oVendedores.Fax;
                oVendedorOriginal.CorreoElectronico = oVendedores.CorreoElectronico;

                //Datos profesionales
                oVendedorOriginal.Rfc= oVendedores.Rfc;
                oVendedorOriginal.Curp = oVendedores.Curp;
                oVendedorOriginal.CedulaProfesional = oVendedores.CedulaProfesional;
                oVendedorOriginal.TituloExpedido = oVendedores.TituloExpedido;
                oVendedorOriginal.IdVinculacion = oVendedores.IdVinculacion;
                oVendedorOriginal.IdEspecialidad = oVendedores.IdEspecialidad;

                //Campos opcionales
                oVendedorOriginal.Campo1 = oVendedores.Campo1;
                oVendedorOriginal.Campo2 = oVendedores.Campo2;
                oVendedorOriginal.Campo3 = oVendedores.Campo3;
                oVendedorOriginal.Campo4 = oVendedores.Campo4;
                oVendedorOriginal.Campo5 = oVendedores.Campo5;
                oVendedorOriginal.Campo6 = oVendedores.Campo6;
                oVendedorOriginal.Campo7 = oVendedores.Campo7;
                oVendedorOriginal.Campo8 = oVendedores.Campo8;
                oVendedorOriginal.Campo9 = oVendedores.Campo9;
                oVendedorOriginal.Campo10 = oVendedores.Campo10;
                


                oMedicuriEntities.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// DAL - Buscar vendedores mediante la cedula profesional
        /// </summary>
        /// <param name="sCadena">Parametro de busqueda</param>
        /// <returns></returns>
        public string[] BuscarVendedorCedulaAsincrono(string sCadena)
        {
            string[] asResultados;

            try
            {
               
                var oQuery = from q in oMedicuriEntities.vendedores
                             where q.CedulaProfesional.Contains(sCadena)
                             select q.CedulaProfesional;
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
        public string[] BuscarClaveVendedorAsincrono(string sCadena)
        {
            string[] asResultados;

            try
            {
                var oQuery = from q in oMedicuriEntities.vendedores
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
        /// 0415 GT 15-Ago-2011
        /// DAL Metodo que regresa el nombre del vendedor de manera asincrina
        /// </summary>
        /// <param name="sCadena">Cadena que contenga el Nombre</param>
        /// <returns></returns>
        public string[] BuscarNombreVendedorAsincrono(string sCadena)
        {
            string[] asResultados;

            try
            {
                var oQuery = from q in oMedicuriEntities.vendedores
                             where q.Nombre.Contains(sCadena)
                             select q.Nombre;


                return asResultados = oQuery.ToArray<string>();

            }
            catch
            {
                return asResultados = new string[0];
            }
        }

        /// <summary>
        /// 0415 GT 15-Ago-2011
        /// DAL Metodo que regresa la clave, nombre y apellido del vendedor de manera asíncrona
        /// </summary>
        /// <param name="sCadena">Cadena que contenga el nombre o apellido</param>
        /// <returns></returns>
        public string[] BuscarNombreVendedorAsincrono2(string sCadena)
        {
            string[] asResultados;

            try
            {
                var oQuery = from q in oMedicuriEntities.vendedores
                             where q.Nombre.Contains(sCadena) || q.Apellidos.Contains(sCadena)
                             select q.Clave + " " + q.Nombre + " " + q.Apellidos;


                return asResultados = oQuery.ToArray<string>();

            }
            catch
            {
                return asResultados = new string[0];
            }
        }

        /// <summary>
        /// Busca un Vendedor por su Clave, asegurarse de buscar por claves existentes!
        /// </summary>
        /// <param name="sClave">Cadena que contiene la clave a obtener</param>
        /// <returns></returns>
        public MedDAL.DAL.vendedores BuscarVendedorClave(string sClave)
        {
            try
            {
                var oQuery = from q in oMedicuriEntities.vendedores
                             where q.Activo == true && q.Clave.Equals(sClave)
                             select q;

                return oQuery.First();
            }
            catch
            {
                return null;
            }
        }
    }
}
