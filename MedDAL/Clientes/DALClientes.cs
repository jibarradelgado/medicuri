using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;

namespace MedDAL.Clientes
{
    public class DALClientes
    {
        DAL.medicuriEntities oMedicuriEntities;


        public DALClientes()
        {
            oMedicuriEntities = new DAL.medicuriEntities();
        }

        /// <summary>
        /// DAL - Insertar nuevo registro
        /// </summary>
        /// <param name="oCliente">Registro a guardar</param>
        /// <returns>true registrado, false no registrado</returns>
        public bool NuevoRegistro(DAL.clientes oClientes)
        {
            try
            {
                //Agregar el registro
                oMedicuriEntities.AddToclientes(oClientes);
                oMedicuriEntities.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        /// Mostrar los datos requeridos en el grid
        /// </summary>
        /// <returns></returns>
        public IQueryable<ClientesView> MostrarLista()
        {

            IQueryable<ClientesView> oQuery = from q in oMedicuriEntities.clientes
                         select new ClientesView
                         {
                             idCliente = q.idCliente,
                             idEstado = q.idEstado,
                             idMunicipio = q.idMunicipio,
                             idPoblacion = q.idPoblacion,
                             idColonia = q.idColonia,
                             Clave1 = q.Clave1,
                             Nombre = q.Nombre,
                             Apellidos = q.Apellidos,
                             TipoPersona = q.TipoPersona,
                             RFC = q.Rfc,
                             Telefono = q.Telefono,
                             Celular = q.Celular,
                             CorreoElectronico = q.CorreoElectronico,
                             TipoCliente = q.tipos.Nombre,
                             FechaAlta = (DateTime)q.FechaAlta,
                             Activo = q.Activo
                         };

            return oQuery;
            
        }
        
        /// <summary>
        /// Obtiene los Cliente que coincidan con la búsqueda y el filtro
        /// </summary>
        /// <param name="sCadena">La cadena por la cual buscar</param>
        /// <param name="iFiltro">El filtro a aplicar: 1=Tipo, 2=Clave, 3=Nombre</param>
        /// <returns></returns>
        public IQueryable<ClientesView> Buscar(string sCadena, int iFiltro)
        {
            string sConsulta = "";
            switch (iFiltro)
            {
                case 1:
                    #region case1
                    IQueryable<ClientesView> oQuery1 = sCadena != "" ?
                        from q in oMedicuriEntities.clientes
                        where q.tipos.Nombre.Contains(sCadena)
                        select new ClientesView
                        {
                            idCliente = q.idCliente,
                            idEstado = q.idEstado,
                            idMunicipio = q.idMunicipio,
                            idPoblacion = q.idPoblacion,
                            idColonia = q.idColonia,
                            Clave1 = q.Clave1,
                            Nombre = q.Nombre,
                            Apellidos = q.Apellidos,
                            TipoPersona = q.TipoPersona,
                            RFC = q.Rfc,
                            Telefono = q.Telefono,
                            Celular = q.Celular,
                            CorreoElectronico = q.CorreoElectronico,
                            TipoCliente = q.tipos.Nombre,
                            FechaAlta = (DateTime)q.FechaAlta,
                            Activo = q.Activo
                        }
                        : 
                        from q in oMedicuriEntities.clientes
                        select new ClientesView
                        {
                            idCliente = q.idCliente,
                            idEstado = q.idEstado,
                            idMunicipio = q.idMunicipio,
                            idPoblacion = q.idPoblacion,
                            idColonia = q.idColonia,
                            Clave1 = q.Clave1,
                            Nombre = q.Nombre,
                            Apellidos = q.Apellidos,
                            TipoPersona = q.TipoPersona,
                            RFC = q.Rfc,
                            Telefono = q.Telefono,
                            Celular = q.Celular,
                            CorreoElectronico = q.CorreoElectronico,
                            TipoCliente = q.tipos.Nombre,
                            FechaAlta = (DateTime)q.FechaAlta,
                            Activo = q.Activo
                        };
                    return oQuery1;
                    #endregion
                case 2:
                    sConsulta = "it.Clave1 LIKE '%'+@Dato+'%'";
                    IQueryable<ClientesView> oQuery = from q in oMedicuriEntities.clientes.
                                      Where(sConsulta,
                                      new ObjectParameter("Dato", sCadena))
                                                      select new ClientesView
                                                      {
                                                          idCliente = q.idCliente,
                                                          idEstado = q.idEstado,
                                                          idMunicipio = q.idMunicipio,
                                                          idPoblacion = q.idPoblacion,
                                                          idColonia = q.idColonia,
                                                          Clave1 = q.Clave1,
                                                          Nombre = q.Nombre,
                                                          Apellidos = q.Apellidos,
                                                          TipoPersona = q.TipoPersona,
                                                          RFC = q.Rfc,
                                                          Telefono = q.Telefono,
                                                          Celular = q.Celular,
                                                          CorreoElectronico = q.CorreoElectronico,
                                                          TipoCliente = q.tipos.Nombre,
                                                          FechaAlta = (DateTime)q.FechaAlta,
                                                          Activo = q.Activo
                                                      };
                    return oQuery;                    
                case 3:
                    sConsulta = "it.Nombre LIKE '%'+@Dato+'%'";
                    oQuery = from q in oMedicuriEntities.clientes.
                                      Where(sConsulta,
                                      new ObjectParameter("Dato", sCadena))
                             select new ClientesView
                             {
                                 idCliente = q.idCliente,
                                 idEstado = q.idEstado,
                                 idMunicipio = q.idMunicipio,
                                 idPoblacion = q.idPoblacion,
                                 idColonia = q.idColonia,
                                 Clave1 = q.Clave1,
                                 Nombre = q.Nombre,
                                 Apellidos = q.Apellidos,
                                 TipoPersona = q.TipoPersona,
                                 RFC = q.Rfc,
                                 Telefono = q.Telefono,
                                 Celular = q.Celular,
                                 CorreoElectronico = q.CorreoElectronico,
                                 TipoCliente = q.tipos.Nombre,
                                 FechaAlta = (DateTime)q.FechaAlta,
                                 Activo = q.Activo
                             };
                    return oQuery;                    
                default: return null;
            }
            
            
        }

        public MedDAL.DAL.clientes BuscarPorClave(string sClave) 
        {
            var oQuery = from q in oMedicuriEntities.clientes
                         where q.Clave1 == sClave
                         select q;
            if (oQuery.Count<MedDAL.DAL.clientes>() > 0)
            {
                return oQuery.First<MedDAL.DAL.clientes>();
            }
            else
            {
                return null;
            }
        }

        public MedDAL.DAL.clientes BuscarPorClaveNombreApellido(string sClave, string sNombre, string sApellido)
        {
            var oQuery = from q in oMedicuriEntities.clientes
                         where q.Clave1 == sClave && q.Nombre == sNombre && q.Apellidos == sApellido
                         select q;
            if (oQuery.Count<MedDAL.DAL.clientes>() > 0)
            {
                return oQuery.First<MedDAL.DAL.clientes>();
            }
            else
            {
                return null;
            }
        }

        // GT: 17/Mar/2011 Función que recupera un cliente mediante su nombre, requerida para llenar autocompletar
        //                 en Pedidos,Remisiones y Facturas
        /// <summary>
        /// DAL - Buscar productos mediante el nombre
        /// </summary>
        /// <param name="sCadena">Parametro de busqueda</param>
        /// <returns>Arrey string conteniendo los nombres</returns>
        public string[] BuscarClienteAsincrono(string sCadena)
        {
            string[] asResultados;

            try
            {
                var oQuery = from q in oMedicuriEntities.clientes
                             where q.Nombre.Contains(sCadena) || q.Apellidos.Contains(sCadena)
                             select q.Clave1 + " " + q.Nombre + "," + q.Apellidos;
                             

                return asResultados = oQuery.ToArray<string>();

            }
            catch
            {
                return asResultados = new string[0];
            }
        }

        /// <summary>
        /// DAL - Buscar productos mediante el nombre
        /// </summary>
        /// <param name="sCadena">Parametro de busqueda</param>
        /// <returns>Arrey string conteniendo los nombres</returns>
        public string[] BuscarClave1ClienteAsincrono(string sCadena)
        {
            string[] asResultados;

            try
            {
                var oQuery = from q in oMedicuriEntities.clientes
                             where q.Clave1.Contains(sCadena)
                             select q.Clave1 + " " + q.Nombre + "," + q.Apellidos;


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
        /// <param name="sCadena">Cadena que contenga la clave1</param>
        /// <returns></returns>
        public string[] BuscarClaveClienteAsincrono(string sCadena)
        {
            string[] asResultados;

            try
            {
                var oQuery = from q in oMedicuriEntities.clientes
                             where q.Clave1.Contains(sCadena)
                             select q.Clave1;


                return asResultados = oQuery.ToArray<string>();

            }
            catch
            {
                return asResultados = new string[0];
            }
        }

        // GT: 17/Mar/2011 Función que recupera un cliente mediante su nombre, requerida para llenar autocompletar
        //                 en Pedidos,Remisiones y Facturas
        /// <summary>
        /// Recuperar un cliente mediante su nombre
        /// </summary>
        /// <param name="sNombre">Nombre a buscar</param>
        /// <returns></returns>
        public DAL.clientes BuscarClienteNombre(string sNombre)
        {
            try
            {
                var oQuery = from q in oMedicuriEntities.clientes
                             where q.Nombre == sNombre
                             select q;
                return oQuery.First<MedDAL.DAL.clientes>();
            }
            catch
            {
                return null;
            }
        }

        // GT: 17/Mar/2011 Función que recupera un cliente mediante su nombre, requerida para llenar autocompletar
        //                 en Pedidos,Remisiones y Facturas
        /// <summary>
        /// Recuperar un cliente mediante su id
        /// </summary>
        /// <param name="iIdCliente">Id a buscar</param>
        /// <returns></returns>
        public DAL.clientes BuscarCliente(int iIdCliente)
        {
            try
            {
                var oQuery = from q in oMedicuriEntities.clientes
                             where q.idCliente==iIdCliente
                             select q;
                return oQuery.First<MedDAL.DAL.clientes>();
            }
            catch
            {
                return null;
            }


        }

        /// <summary>
        /// Busca todos los clientes que estén activos
        /// </summary>
        /// <returns>La coleccion de vendedores</returns>
        public IQueryable<DAL.clientes> BuscarEnum()
        {
            var oQuery = from q in oMedicuriEntities.clientes
                         where q.Activo == true
                         select q;

            return oQuery;
        }

        /// <summary>
        /// Busca todos los clientes que estén activos
        /// </summary>
        /// <return>El resultado  de la búsqueda</return></returns>
        public object Buscar()
        {
            var oQuery = from q in oMedicuriEntities.clientes
                         where q.Activo == true
                         select q;

            return oQuery;
        }

        /// <summary>
        /// DAL - Muestra todos los registros de la tabla clientes
        /// </summary>
        /// <returns>Objeto</returns>
        //public object MostrarLista()
        //{

        //    var oQuery = from q in oMedicuriEntities.clientes select q;
        //    return oQuery;

        //}

        /// <summary>
        /// Eliminar un cliente
        /// </summary>
        /// <param name="iIdCliente">Id cliente a eliminar</param>
        /// <returns></returns>
        public bool EliminarRegistro(int iIdcliente)
        {
            try
            {
                var oQuery = from q in oMedicuriEntities.clientes.
                              Where("it.idCliente=@idCliente",
                              new ObjectParameter("idCliente", iIdcliente))
                             select q;

                DAL.clientes oClienteOriginal = oQuery.First<DAL.clientes>();
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
        /// Validar cliente
        /// </summary>REVISAR SEVERAMENTE
        /// <param name="idCliente">Nombre de cliente</param>
        /// <returns>Object</returns>
        public int ValidarClienteRepetido(string claveCliente)
        {
            try
            {

                //Recuperar el objeto a editar
                var oQuery = from q in oMedicuriEntities.clientes.
                             Where("it.Clave1=@Clave1",
                             new ObjectParameter("Clave1", claveCliente))
                             select q;

                return oQuery.Count();

            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// Acutualiza un cliente en la DB
        /// </summary>
        /// <param name="oCliente"> Cliente a actualizar</param>
        /// <returns></returns>
        public bool EditarRegistro(DAL.clientes oCliente)
        {
            try
            {

                //Recuperar el objeto a editar
                var oQuery = from q in oMedicuriEntities.clientes.
                             Where("it.idCliente=@idCliente",
                             new ObjectParameter("idCliente", oCliente.idCliente))
                             select q;


                DAL.clientes oClienteOriginal = oQuery.First<DAL.clientes>();

                //Datos Cliente
                oClienteOriginal.Clave2 = oCliente.Clave2;
                oClienteOriginal.Clave3 = oCliente.Clave3;
                oClienteOriginal.Nombre = oCliente.Nombre;
                oClienteOriginal.Apellidos = oCliente.Apellidos;
                oClienteOriginal.idTipoCliente = oCliente.idTipoCliente;
                oClienteOriginal.Activo = oCliente.Activo;

                //Datos de contacto
                oClienteOriginal.Calle = oCliente.Calle;
                oClienteOriginal.NumeroInt = oCliente.NumeroInt;
                oClienteOriginal.NumeroExt = oCliente.NumeroExt;
                oClienteOriginal.idEstado = oCliente.idEstado;
                oClienteOriginal.idMunicipio = oCliente.idMunicipio;
                oClienteOriginal.idPoblacion = oCliente.idPoblacion;
                oClienteOriginal.idColonia = oCliente.idColonia;
                oClienteOriginal.CodigoPostal = oCliente.CodigoPostal;

                oClienteOriginal.Telefono = oCliente.Telefono;
                oClienteOriginal.Celular = oCliente.Celular;
                oClienteOriginal.Fax = oCliente.Fax;
                oClienteOriginal.CorreoElectronico = oCliente.CorreoElectronico;

                //Datos profesionales
                oClienteOriginal.Rfc= oCliente.Rfc;
                oClienteOriginal.Curp = oCliente.Curp;

                //Campos opcionales
                oClienteOriginal.Campo1 = oCliente.Campo1;
                oClienteOriginal.Campo2 = oCliente.Campo2;
                oClienteOriginal.Campo3 = oCliente.Campo3;
                oClienteOriginal.Campo4 = oCliente.Campo4;
                oClienteOriginal.Campo5 = oCliente.Campo5;
                oClienteOriginal.Campo6 = oCliente.Campo6;
                oClienteOriginal.Campo7 = oCliente.Campo7;
                oClienteOriginal.Campo8 = oCliente.Campo8;
                oClienteOriginal.Campo9 = oCliente.Campo9;
                oClienteOriginal.Campo10 = oCliente.Campo10;
                


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
