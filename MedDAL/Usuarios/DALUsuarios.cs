using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using System.Data.Objects.DataClasses;


namespace MedDAL.Usuarios
{
    public class DALUsuarios
    {

        //Instancia de un objeto de la DAL
        DAL.medicuriEntities oMedicuriEntities;


        /// <summary>
        /// Constructor
        /// </summary>
        public DALUsuarios()
        {
            oMedicuriEntities = new DAL.medicuriEntities();
        }


        /// <summary>
        /// DAL - Insertar nuevo registro
        /// </summary>
        /// <param name="oUsusario">Registro a guardar</param>
        /// <returns>true registrado, false no registrado</returns>
        public bool NuevoRegistro(DAL.usuarios oUsuario)
        {
            try
            {
                //Agregar el registro
                oMedicuriEntities.AddTousuarios(oUsuario);
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
        public IQueryable<UsuarioView> Buscar(string sCadena, int iFiltro)
        {
            string sConsulta = "";
            switch (iFiltro)
            {
                //Filtro 1
                case 1:
                    sConsulta = "it.Usuario LIKE '%'+@Dato+'%'";
                    break;
                //Filtro 2
                case 2:
                    sConsulta = "it.Nombre LIKE '%'+@Dato+'%'";
                    break;
                //Filtro 3
                case 3:
                    sConsulta = "it.CorreoElectronico LIKE '%'+@Dato+'%'";
                    break;
            }

           
            var oQuery = from q in oMedicuriEntities.usuarios.Where(sConsulta, new ObjectParameter("Dato", sCadena))
                         select new UsuarioView
                         {
                             idUsuario = q.idUsuario,
                             Usuario = q.Usuario,
                             Nombre = q.Nombre,
                             Apellidos = q.Apellidos,
                             CorreoElectronico = q.CorreoElectronico,
                             Perfil = q.perfiles.Nombre,
                             Activo = q.Activo
                         };

            return oQuery;
        }


        /// <summary>
        /// DAL - Muestra todos los registros de la tabla usuarios
        /// </summary>
        /// <returns>Objeto</returns>
        public IQueryable<UsuarioView> MostrarLista()
        {

            var oQuery = from q in oMedicuriEntities.usuarios
                         select new UsuarioView
                         {
                             idUsuario = q.idUsuario,
                             Usuario = q.Usuario,
                             Nombre = q.Nombre,
                             Apellidos = q.Apellidos,
                             CorreoElectronico = q.CorreoElectronico,
                             Perfil = q.perfiles.Nombre,
                             Activo = q.Activo
                         };
            return oQuery;

        }

        /// <summary>
        /// DAL - Muestra todos los registros de la tabla usuarios que corresponden a un almacen
        /// </summary>
        /// <returns>Objeto</returns>
        public IQueryable<UsuarioView> MostrarListaAlmacenFiltrada(int iIdAlmacen)
        {

            var oQuery = from q in oMedicuriEntities.usuarios
                         where q.idAlmacen==iIdAlmacen
                         select new UsuarioView
                         {
                             idUsuario = q.idUsuario,
                             Usuario = q.Usuario,
                             Nombre = q.Nombre,
                             Apellidos = q.Apellidos,
                             CorreoElectronico = q.CorreoElectronico,
                             Perfil = q.perfiles.Nombre,
                             Activo = q.Activo
                         };
            return oQuery;

        }

        /// <summary>
        /// Recuperar un usuario mediante su llave primaria
        /// </summary>
        /// <param name="iId">Llave primaria</param>
        /// <returns>(object)oQuery.First<MedDAL.DAL.lineas_creditos></returns>
        public object Buscar(int iId)
        {
            var oQuery = from q in oMedicuriEntities.usuarios
                         where q.idUsuario == iId
                         select q;

            return (object)oQuery.First<MedDAL.DAL.usuarios>();
        }

        /// <summary>
        /// Recuperar un usuario mediante su nombre de usuario
        /// </summary>
        /// <param name="sUsuario">Usuario</param>
        /// <returns>(object)oQuery.First<MedDAL.DAL.usuario></returns>
        public object Buscar(string sUsuario)
        {
            var oQuery = from q in oMedicuriEntities.usuarios
                         where q.Usuario == sUsuario
                         select q;

            if (oQuery.Count<MedDAL.DAL.usuarios>() > 0)
            {
                return (object)oQuery.First<MedDAL.DAL.usuarios>();
            }
            else return null;            
        }


        /// <summary>
        /// Recuperar permisos de un usuario 
        /// </summary>
        /// <param name="iId">idUsuario</param>
        /// <returns>Object</returns>
        public object RecuperarPermisos(int iId)
        {
            try
            {

                //Recuperar el objeto a editar
                var oQuery = from q in oMedicuriEntities.permisos_usuarios.
                             Where("it.idUsuario=@idUsuario",
                             new ObjectParameter("idUsuario", iId))
                             select q;
                                            
                return (object)oQuery.FirstOrDefault<MedDAL.DAL.permisos_usuarios>();

            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        /// Validar usuario
        /// </summary>
        /// <param name="sUsuario">Nombre de usuario</param>
        /// <returns>Object</returns>
        public int ValidarUsuarioRepetido(string sUsuario)
        {
            try
            {

                //Recuperar el objeto a editar
                var oQuery = from q in oMedicuriEntities.usuarios.
                             Where("it.usuario=@usuario",
                             new ObjectParameter("usuario", sUsuario))
                             select q;

                return oQuery.Count();


                //return (object)oQuery.FirstOrDefault<MedDAL.DAL.permisos_usuarios>();

            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// Recuperar un almacen mediante su clave
        /// </summary>
        /// <param name="sClave">Clave</param>
        /// <returns>(object)oQuery.First<MedDAL.DAL.almacen></returns>
        public object RecuperarAlmacen(string sClave)
        {
            var oQuery = from q in oMedicuriEntities.almacenes
                         where q.Clave == sClave
                         select q;

            return (object)oQuery.First<MedDAL.DAL.almacenes>();
        }

        /// <summary>
        /// Editar un usuario
        /// </summary>
        /// <param name="oUsuario">usuario a editar</param>
        /// <returns></returns>
        public bool EditarRegistro(DAL.usuarios oUsuario)
        {
            try
            {
              
                //Recuperar el objeto a editar
                var oQuery = from q in oMedicuriEntities.usuarios.
                             Where("it.idUsuario=@idUsuario",
                             new ObjectParameter("idUsuario", oUsuario.idUsuario))
                             select q;

               
                DAL.usuarios oUsuarioOriginal = oQuery.First<DAL.usuarios>();
               
                //Modificar los valores
                oUsuarioOriginal.Usuario = oUsuario.Usuario;
                oUsuarioOriginal.Contrasena = oUsuario.Contrasena;
                oUsuarioOriginal.idPerfil = oUsuario.idPerfil;
                oUsuarioOriginal.idAlmacen = oUsuario.idAlmacen;
                oUsuarioOriginal.Nombre=oUsuario.Nombre;
                oUsuarioOriginal.Apellidos = oUsuario.Apellidos;
                oUsuarioOriginal.CorreoElectronico = oUsuario.CorreoElectronico;
                oUsuarioOriginal.Campo1 = oUsuario.Campo1;
                oUsuarioOriginal.Campo2 = oUsuario.Campo2;
                oUsuarioOriginal.Campo3 = oUsuario.Campo3;
                oUsuarioOriginal.Campo4 = oUsuario.Campo4;
                oUsuarioOriginal.Campo5 = oUsuario.Campo5;
                oUsuarioOriginal.Campo6 = oUsuario.Campo6;
                oUsuarioOriginal.Campo7 = oUsuario.Campo7;
                oUsuarioOriginal.Campo8 = oUsuario.Campo8;
                oUsuarioOriginal.Campo9 = oUsuario.Campo9;
                oUsuarioOriginal.Campo10 = oUsuario.Campo10;
                oUsuarioOriginal.Activo = oUsuario.Activo;
                oUsuarioOriginal.FiltradoActivado = oUsuario.FiltradoActivado;

                
                oMedicuriEntities.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Editar la contraseña de usuario
        /// </summary>
        /// <param name="sNombreUsuario">Nombre de usuario</param>
        /// <param name="sContrasena">Contraseña encriptada en MD5</param>
        /// <returns></returns>
        public bool CambiarContraseña(string sNombreUsuario, string sContrasena)
        {
            try
            {

                //Recuperar el objeto a editar
                var oQuery = from q in oMedicuriEntities.usuarios.
                             Where("it.Usuario=@Usuario",
                             new ObjectParameter("Usuario", sNombreUsuario))
                             select q;


                DAL.usuarios oUsuarioOriginal = oQuery.First<DAL.usuarios>();

                //Modificar los valores
                oUsuarioOriginal.Contrasena = sContrasena;
                oMedicuriEntities.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Eliminar un usuario
        /// </summary>
        /// <param name="iIdUsuario">Id usuario a eliminar</param>
        /// <returns></returns>
        public bool EliminarRegistro(int iIdUsuario)
        {
            try
            {
                var oQuery = from q in oMedicuriEntities.usuarios.
                              Where("it.idUsuario=@idUsuario",
                              new ObjectParameter("idUsuario", iIdUsuario))
                             select q;

                DAL.usuarios oUsuarioOriginal = oQuery.First<DAL.usuarios>();
                oMedicuriEntities.DeleteObject(oUsuarioOriginal);
                oMedicuriEntities.SaveChanges();
               
                return true;
            }
            catch
            {
               return false;
            }
        }

        /// <summary>
        /// Buscar los nombres de los usuarios de forma asincrona
        /// </summary>
        /// <param name="sCadena"></param>
        /// <returns></returns>
        public string[] BuscarUsarioAsincrono(string sCadena)
        {
            try
            {
               var oQuery = from q in oMedicuriEntities.usuarios
                             where q.Nombre.Contains(sCadena)
                             select new
                             {
                                 q.Nombre,
                                 q.Apellidos
                             };

              
               int iContador=0;
               string[] asResultados = new string[oQuery.Count()];
                
               foreach (var vRegistro in oQuery)
               {
                   asResultados[iContador] = vRegistro.Nombre.ToString()+ " " + vRegistro.Apellidos.ToString();
                   iContador++;
               }

               return asResultados;

            }
            catch
            {
                string[] asResultados = new string[0];
                return asResultados;
            }
        }


       

        
   }
}
