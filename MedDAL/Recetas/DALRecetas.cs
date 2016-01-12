using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using System.Globalization;

namespace MedDAL.Recetas
{
    public class DALRecetas
    {
        DAL.medicuriEntities oMedicuriEntities = new DAL.medicuriEntities();
        public DALRecetas()
        {
            oMedicuriEntities = new DAL.medicuriEntities();

        }

        public void agregarReceta(MedDAL.DAL.recetas receta)
        {
            oMedicuriEntities.AddTorecetas(receta);
            oMedicuriEntities.SaveChanges();
        }

        /// <summary>
        /// Nueva Forma de Registrar las partidas de las recetas
        /// </summary>
        /// <param name="oRecetaPartida"></param>
        /// <returns></returns>
        public bool NuevoRegistroPartida(MedDAL.DAL.recetas_partida oRecetaPartida)
        {
            try
            {
                oMedicuriEntities.AddTorecetas_partida(oRecetaPartida);
                oMedicuriEntities.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// Elimina las partidas de una receta.
        /// </summary>
        /// <param name="oReceta"></param>
        /// <returns></returns>
        public bool EliminarRegistroPartida(MedDAL.DAL.recetas oReceta)
        {
            try
            {
                var oQuery = from q in oMedicuriEntities.recetas_partida
                             where q.idReceta == oReceta.idReceta
                             select q;

                foreach (MedDAL.DAL.recetas_partida oRecetaPartida in oQuery)
                {
                    oMedicuriEntities.DeleteObject(oRecetaPartida);
                }

                oMedicuriEntities.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public void agregaRecetaPartida(List<MedDAL.DAL.recetas_partida> lPartida)
        {
            foreach(MedDAL.DAL.recetas_partida partida in lPartida)
            {
                oMedicuriEntities.AddTorecetas_partida(partida);
            }

            oMedicuriEntities.SaveChanges();
        }

        /// <summary>
        /// GT - Buscar folios asincronamente para llenar se necesita para la facturacion
        /// </summary>
        /// <param name="sCadena"></param>
        /// <returns></returns>
        public string[] BuscarFolioAsincrono(string sCadena)
        {
            string[] asResultados;

            try
            {
                var oQuery = from q in oMedicuriEntities.recetas
                             where q.Folio.Contains(sCadena) && q.Estatus == "1"
                             select q.Folio;

                return asResultados = oQuery.ToArray<string>();

            }
            catch
            {
                return asResultados = new string[0];
            }
        }

        /// <summary>
        /// Recuperar un receta mediante su folio
        /// </summary>
        /// <param name="sFolio">Folio a buscar</param>
        /// <returns></returns>
        public DAL.recetas BuscarRecetaFolio(string sFolio)
        {
            try
            {
                var oQuery = from q in oMedicuriEntities.recetas
                             where q.Folio == sFolio
                             select q;
                return oQuery.First<MedDAL.DAL.recetas>();
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Busca y recupera una receta con el folio buscado y que no se encuentre cancelada
        /// </summary>
        /// <param name="sFolio">Folio a buscar</param>
        /// <returns></returns>
        public DAL.recetas BuscarRecetaFolioRepetido(string sFolio)
        {
            try
            {
                var oQuery = from q in oMedicuriEntities.recetas
                             where q.Folio == sFolio && q.EstatusMedico != "4"
                             select q;
                return oQuery.First<MedDAL.DAL.recetas>();
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Recuperar un receta mediante su id
        /// </summary>
        /// <param name="iIdReceta">Id a buscar</param>
        /// <returns></returns>
        public DAL.recetas BuscarReceta(int iIdReceta)
        {
            try
            {
                var oQuery = from q in oMedicuriEntities.recetas
                             where q.idReceta == iIdReceta
                             select q;
                return oQuery.First<MedDAL.DAL.recetas>();
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Recuperar todas las recetas
        /// </summary>
        /// <returns></returns>
        public object BuscarReceta() 
        {
            var oQuery = (from q in oMedicuriEntities.recetas
                          select new { 
                            q.idReceta,
                            q.Folio,
                            q.ClaveMed,
                            q.Paciente,
                            q.Domicilio,
                            q.Telefono,
                            q.Celular,
                            q.CorreoElectronico,
                            q.Fecha,
                            q.EstatusMedico,
                            q.Estatus,
                            q.poblaciones.idPoblacion,
                            q.poblaciones.Nombre                            
                          });
            return oQuery;
        }

        public IQueryable<RecetasView> BuscarTodasRecetas()
        {
            var oQuery = from q in oMedicuriEntities.recetas
                         select new RecetasView
                         {
                            idReceta = q.idReceta,
                            idEstadoExp = (int)q.idEstadoExp,
                            idMunicipioExp = (int)q.idMunicipioExp,
                            idPoblacionExp = (int)q.idPoblacionExp,
                            idColoniaExp = (int)q.idColoniaExp,
                            idEstadoSur = (int)q.idEstadoSur,
                            idMunicipioSur = (int)q.idMunicipioSur,
                            idColoniaSur = (int)q.idColoniaSur,
                            idPoblacionSur = (int)q.idPoblacionSur,
                            Folio = q.Folio,
                            Paciente = q.Paciente,
                            Tipo = q.tipos.Nombre,
                            //GT 14/10/11 0578 Se corrige el campo de los estatus por estar cambiados y se presenta mal la info en el gridview
                            //EstatusMedico = q.Estatus,
                            //Estatus = q.EstatusMedico,
                            EstatusMedico=q.EstatusMedico,
                            Estatus=q.Estatus,
                            Fecha = q.Fecha
                         };

            return oQuery;

        }

        public IQueryable<RecetasView> BuscarTodasRecetas(int idAlmacen)
        {
            var oQuery = from q in oMedicuriEntities.recetas where q.almacenes.idAlmacen == idAlmacen
                         select new RecetasView
                         {
                             idReceta = q.idReceta,
                             idEstadoExp = (int)q.idEstadoExp,
                             idMunicipioExp = (int)q.idMunicipioExp,
                             idPoblacionExp = (int)q.idPoblacionExp,
                             idColoniaExp = (int)q.idColoniaExp,
                             idEstadoSur = (int)q.idEstadoSur,
                             idMunicipioSur = (int)q.idMunicipioSur,
                             idColoniaSur = (int)q.idColoniaSur,
                             idPoblacionSur = (int)q.idPoblacionSur,
                             Folio = q.Folio,
                             Paciente = q.Paciente,
                             Tipo = q.tipos.Nombre,
                             //GT 14/10/11 0578 Se corrige el campo de los estatus por estar cambiados y se presenta mal la info en el gridview
                             //EstatusMedico = q.Estatus,
                             //Estatus = q.EstatusMedico,
                             EstatusMedico = q.EstatusMedico,
                             Estatus = q.Estatus,
                             Fecha = q.Fecha
                         };

            return oQuery;

        }

        public void eliminaPartidaReceta(int idReceta)
        {
            var oQuery = from q in oMedicuriEntities.recetas_partida
                         where q.idReceta == idReceta
                         select q;
            
            foreach(DAL.recetas_partida rp in oQuery)
                oMedicuriEntities.DeleteObject(rp);

            oMedicuriEntities.SaveChanges();
        }

        /// <summary>
        /// Recuperar la partida de una receta
        /// </summary>
        /// <param name="iIdReceta"></param>
        /// <returns></returns>
        public IQueryable<DAL.recetas_partida> RecuperarPartidaRecetas(int iIdReceta)
        {
            try
            {
                var oQuery = from q in oMedicuriEntities.recetas_partida
                             where q.idReceta == iIdReceta
                             select q;

                return oQuery;

            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Editar registro
        /// </summary>
        /// <param name="oReceta"></param>
        /// <returns></returns>
        public bool EditarRegistro(DAL.recetas oReceta)
        {
            try
            {
                var oQuery = from q in oMedicuriEntities.recetas.Where("it.idReceta=@idReceta",
                             new ObjectParameter("idReceta", oReceta.idReceta))
                             select q;

                DAL.recetas oRecetaOriginal = oQuery.First<DAL.recetas>();
                oRecetaOriginal.Estatus = oReceta.Estatus;

                oMedicuriEntities.SaveChanges();
                return true;

            }
            catch
            {
                return false;
            }

        }

        public void EditarReceta(DAL.recetas receta)
        {
            var oQuery = from q in oMedicuriEntities.recetas
                         where q.idReceta==receta.idReceta
                         select q;
            DAL.recetas oRecetaOriginal = oQuery.First<DAL.recetas>();
            

            oRecetaOriginal.idTipoReceta=       receta.idTipoReceta;
            oRecetaOriginal.idVendedor=         receta.idVendedor;
            oRecetaOriginal.idAlmacen =         receta.idAlmacen;
            oRecetaOriginal.idEstadoExp=        receta.idEstadoExp;
            oRecetaOriginal.idMunicipioExp=     receta.idMunicipioExp;
            oRecetaOriginal.idPoblacionExp =    receta.idPoblacionExp;
            oRecetaOriginal.idColoniaExp =      receta.idColoniaExp;
            oRecetaOriginal.idEstadoSur=        receta.idEstadoSur;
            oRecetaOriginal.idMunicipioSur=     receta.idMunicipioSur;
            oRecetaOriginal.idPoblacionSur =    receta.idPoblacionSur;
            oRecetaOriginal.idColoniaSur =      receta.idColoniaSur;
            oRecetaOriginal.idVendedor=         receta.idVendedor;
            oRecetaOriginal.Folio    =          receta.Folio;
            oRecetaOriginal.ClaveMed =          receta.ClaveMed;
            oRecetaOriginal.Paciente =          receta.Paciente;
            oRecetaOriginal.Telefono  =         receta.Telefono;
            oRecetaOriginal.Fecha  =            receta.Fecha ;
            oRecetaOriginal.EstatusMedico=      receta.EstatusMedico;
            oRecetaOriginal.Estatus      =      receta.Estatus;

            oMedicuriEntities.SaveChanges();
        }

        public void EliminarReceta(DAL.recetas receta)
        {
            oMedicuriEntities.DeleteObject(receta); 
            oMedicuriEntities.SaveChanges();
        }

        public IQueryable<RecetasView> Buscar(string sCadena, int iFiltro)
        {

            string sConsulta = "";

            switch (iFiltro)
            {
                case 1:
                    sConsulta = "it.Folio = @Dato"; //"(it.Folio LIKE '%'+@Dato+'%' )";
                    var oQuery = from q in oMedicuriEntities.recetas.
                                     Where(sConsulta,
                                     new ObjectParameter("Dato", sCadena))
                                 select new RecetasView
                                 {
                                     idReceta = q.idReceta,
                                     idEstadoExp = (int)q.idEstadoExp,
                                     idMunicipioExp = (int)q.idMunicipioExp,
                                     idPoblacionExp = (int)q.idPoblacionExp,
                                     idColoniaExp = (int)q.idColoniaExp,
                                     idEstadoSur = (int)q.idEstadoSur,
                                     idMunicipioSur = (int)q.idMunicipioSur,
                                     idColoniaSur = (int)q.idColoniaSur,
                                     idPoblacionSur = (int)q.idPoblacionSur,
                                     Folio = q.Folio,
                                     Paciente = q.Paciente,
                                     Tipo = q.tipos.Nombre,
                                     //GT 14/10/11 0578 Se corrige el campo de los estatus por estar cambiados y se presenta mal la info en el gridview
                                     //EstatusMedico = q.Estatus,
                                     //Estatus = q.EstatusMedico,
                                     Estatus = q.Estatus,
                                     EstatusMedico = q.EstatusMedico,
                                     Fecha = q.Fecha
                                 };
                    return oQuery;
                case 2:
                    oQuery = sCadena != "" ? from q in oMedicuriEntities.recetas
                                             where q.tipos.Nombre.Contains(sCadena)
                                             select new RecetasView
                                             {
                                                 idReceta = q.idReceta,
                                                 idEstadoExp = (int)q.idEstadoExp,
                                                 idMunicipioExp = (int)q.idMunicipioExp,
                                                 idPoblacionExp = (int)q.idPoblacionExp,
                                                 idColoniaExp = (int)q.idColoniaExp,
                                                 idEstadoSur = (int)q.idEstadoSur,
                                                 idMunicipioSur = (int)q.idMunicipioSur,
                                                 idColoniaSur = (int)q.idColoniaSur,
                                                 idPoblacionSur = (int)q.idPoblacionSur,
                                                 Folio = q.Folio,
                                                 Paciente = q.Paciente,
                                                 Tipo = q.tipos.Nombre,
                                                 //GT 14/10/11 0578 Se corrige el campo de los estatus por estar cambiados y se presenta mal la info en el gridview
                                                 //EstatusMedico = q.Estatus,
                                                 //Estatus = q.EstatusMedico,
                                                 Estatus = q.Estatus,
                                                 EstatusMedico = q.EstatusMedico,
                                                 Fecha = q.Fecha
                                             } : from q in oMedicuriEntities.recetas                                                 
                                                 select new RecetasView
                                                 {
                                                     idReceta = q.idReceta,
                                                     idEstadoExp = (int)q.idEstadoExp,
                                                     idMunicipioExp = (int)q.idMunicipioExp,
                                                     idPoblacionExp = (int)q.idPoblacionExp,
                                                     idColoniaExp = (int)q.idColoniaExp,
                                                     idEstadoSur = (int)q.idEstadoSur,
                                                     idMunicipioSur = (int)q.idMunicipioSur,
                                                     idColoniaSur = (int)q.idColoniaSur,
                                                     idPoblacionSur = (int)q.idPoblacionSur,
                                                     Folio = q.Folio,
                                                     Paciente = q.Paciente,
                                                     Tipo = q.tipos.Nombre,
                                                     //GT 14/10/11 0578 Se corrige el campo de los estatus por estar cambiados y se presenta mal la info en el gridview
                                                     //EstatusMedico = q.Estatus,
                                                     //Estatus = q.EstatusMedico,
                                                     Estatus = q.Estatus,
                                                     EstatusMedico = q.EstatusMedico,
                                                     Fecha = q.Fecha
                                                 }; ;
                    return oQuery;
                case 3:
                    DateTime fecha = DateTime.Parse(sCadena);
                    oQuery = from q in oMedicuriEntities.recetas
                             where q.Fecha.Equals(fecha) 
                             select new RecetasView
                             {
                                 idReceta = q.idReceta,
                                 idEstadoExp = (int)q.idEstadoExp,
                                 idMunicipioExp = (int)q.idMunicipioExp,
                                 idPoblacionExp = (int)q.idPoblacionExp,
                                 idColoniaExp = (int)q.idColoniaExp,
                                 idEstadoSur = (int)q.idEstadoSur,
                                 idMunicipioSur = (int)q.idMunicipioSur,
                                 idColoniaSur = (int)q.idColoniaSur,
                                 idPoblacionSur = (int)q.idPoblacionSur,
                                 Folio = q.Folio,
                                 Paciente = q.Paciente,
                                 Tipo = q.tipos.Nombre,
                                 //GT 14/10/11 0578 Se corrige el campo de los estatus por estar cambiados y se presenta mal la info en el gridview
                                 //EstatusMedico = q.Estatus,
                                 //Estatus = q.EstatusMedico,
                                 Estatus = q.Estatus,
                                 EstatusMedico = q.EstatusMedico,
                                 Fecha = q.Fecha
                             };
                    return oQuery;
                default: return null;
            }
        }

        public IQueryable<RecetasView> Buscar(string sCadena, int iFiltro, int iIdAlmacen)
        {

            string sConsulta = "";
            
            switch (iFiltro)
            {
                case 1:
                    sConsulta =   "it.Folio = @Dato and it.idAlmacen = @idAlmacen"; //"(it.Folio LIKE '%'+@Dato+'%' )";
                    var oQuery =  from q in oMedicuriEntities.recetas.
                                      Where(sConsulta,
                                      new ObjectParameter("Dato", sCadena),
                                      new ObjectParameter("idAlmacen", iIdAlmacen))
                                  select new RecetasView
                                  {
                                      idReceta = q.idReceta,
                                      idEstadoExp = (int)q.idEstadoExp,
                                      idMunicipioExp = (int)q.idMunicipioExp,
                                      idPoblacionExp = (int)q.idPoblacionExp,
                                      idColoniaExp = (int)q.idColoniaExp,
                                      idEstadoSur = (int)q.idEstadoSur,
                                      idMunicipioSur = (int)q.idMunicipioSur,
                                      idColoniaSur = (int)q.idColoniaSur,
                                      idPoblacionSur = (int)q.idPoblacionSur,
                                      Folio = q.Folio,
                                      Paciente = q.Paciente,
                                      Tipo = q.tipos.Nombre,
                                      //GT 14/10/11 0578 Se corrige el campo de los estatus por estar cambiados y se presenta mal la info en el gridview
                                      //EstatusMedico = q.Estatus,
                                      //Estatus = q.EstatusMedico,
                                      Estatus=q.Estatus,
                                      EstatusMedico=q.EstatusMedico,
                                      Fecha = q.Fecha
                                  };
                    return oQuery;
                case 2:
                    oQuery = sCadena != "" ? from q in oMedicuriEntities.recetas where q.tipos.Nombre.Contains(sCadena) && q.almacenes.idAlmacen == iIdAlmacen select new RecetasView
                         {
                            idReceta = q.idReceta,
                            idEstadoExp = (int)q.idEstadoExp,
                            idMunicipioExp = (int)q.idMunicipioExp,
                            idPoblacionExp = (int)q.idPoblacionExp,
                            idColoniaExp = (int)q.idColoniaExp,
                            idEstadoSur = (int)q.idEstadoSur,
                            idMunicipioSur = (int)q.idMunicipioSur,
                            idColoniaSur = (int)q.idColoniaSur,
                            idPoblacionSur = (int)q.idPoblacionSur,
                            Folio = q.Folio,
                            Paciente = q.Paciente,
                            Tipo = q.tipos.Nombre,
                            //GT 14/10/11 0578 Se corrige el campo de los estatus por estar cambiados y se presenta mal la info en el gridview
                            //EstatusMedico = q.Estatus,
                            //Estatus = q.EstatusMedico,
                            Estatus = q.Estatus,
                            EstatusMedico = q.EstatusMedico,
                            Fecha = q.Fecha
                         } : from q in oMedicuriEntities.recetas where q.almacenes.idAlmacen == iIdAlmacen select new RecetasView
                         {
                            idReceta = q.idReceta,
                            idEstadoExp = (int)q.idEstadoExp,
                            idMunicipioExp = (int)q.idMunicipioExp,
                            idPoblacionExp = (int)q.idPoblacionExp,
                            idColoniaExp = (int)q.idColoniaExp,
                            idEstadoSur = (int)q.idEstadoSur,
                            idMunicipioSur = (int)q.idMunicipioSur,
                            idColoniaSur = (int)q.idColoniaSur,
                            idPoblacionSur = (int)q.idPoblacionSur,
                            Folio = q.Folio,
                            Paciente = q.Paciente,
                            Tipo = q.tipos.Nombre,
                            //GT 14/10/11 0578 Se corrige el campo de los estatus por estar cambiados y se presenta mal la info en el gridview
                            //EstatusMedico = q.Estatus,
                            //Estatus = q.EstatusMedico,
                            Estatus = q.Estatus,
                            EstatusMedico = q.EstatusMedico,
                            Fecha = q.Fecha
                         };;
                    return oQuery;
                case 3:
                     DateTime fecha = DateTime.Parse(sCadena);
                    oQuery =  from q in oMedicuriEntities.recetas
                            where q.Fecha.Equals(fecha) && q.almacenes.idAlmacen == iIdAlmacen
                              select new RecetasView
                              {
                                  idReceta = q.idReceta,
                                  idEstadoExp = (int)q.idEstadoExp,
                                  idMunicipioExp = (int)q.idMunicipioExp,
                                  idPoblacionExp = (int)q.idPoblacionExp,
                                  idColoniaExp = (int)q.idColoniaExp,
                                  idEstadoSur = (int)q.idEstadoSur,
                                  idMunicipioSur = (int)q.idMunicipioSur,
                                  idColoniaSur = (int)q.idColoniaSur,
                                  idPoblacionSur = (int)q.idPoblacionSur,
                                  Folio = q.Folio,
                                  Paciente = q.Paciente,
                                  Tipo = q.tipos.Nombre,
                                  //GT 14/10/11 0578 Se corrige el campo de los estatus por estar cambiados y se presenta mal la info en el gridview
                                  //EstatusMedico = q.Estatus,
                                  //Estatus = q.EstatusMedico,
                                  Estatus = q.Estatus,
                                  EstatusMedico = q.EstatusMedico,
                                  Fecha = q.Fecha
                              };
                    return oQuery;
                default: return null;
            }            
        }



        
        /// <summary>
        /// GT Función que regresa los datos de la receta para la pantalla de Facturas por programa 
        /// </summary>
        /// <returns></returns>
        public object RecetasFacturarPorPrograma()
        {
            try
            {
                var oQuery = (from q in oMedicuriEntities.recetas
                              where (q.EstatusMedico=="1" || q.EstatusMedico=="2") && q.Estatus=="1"
                              select new
                              {
                                  q.idReceta,
                                  q.Folio,
                                  q.Fecha,
                                  q.EstatusMedico,
                                  q.Estatus,
                                  Total = (from rp in oMedicuriEntities.recetas_partida
                                           where rp.idReceta == q.idReceta
                                           select (rp.Precio) * (rp.CantidaSurtida)).Sum()

                              });

                return oQuery;
            }
            catch
            {
                return null;
            }
           
        }

        /// <summary>
        /// GT Función que regresa los datos de la receta para la pantalla de Facturas por programa 
        /// </summary>
        /// <returns></returns>
        public object RecetasFacturarPorPrograma(int iIdAlmacen)
        {
            try
            {
                var oQuery = (from q in oMedicuriEntities.recetas
                              where q.idAlmacen == iIdAlmacen && (q.EstatusMedico == "1" || q.EstatusMedico == "2") && q.Estatus=="1"
                              select new
                              {
                                  q.idReceta,
                                  q.Folio,
                                  q.Fecha,
                                  q.EstatusMedico,
                                  q.Estatus,
                                  Total = (from rp in oMedicuriEntities.recetas_partida
                                           where rp.idReceta == q.idReceta
                                           select (rp.Precio) * (rp.CantidaSurtida)).Sum()

                              });

                return oQuery;
            }
            catch
            {
                return null;
            }

        }

        /// <summary>
        /// GT Función que regresa los datos de la receta para la pantalla de Facturas por programa filtrado por fecha
        /// </summary>
        /// <returns></returns>
        public object RecetasFacturarPorProgramaFecha(string sFecha1,string sFecha2)
        {
            //GT Esta es la funcion para cuando me filtren por fecha
            DateTime fecha1 = Convert.ToDateTime(sFecha1);
            DateTime fecha2 = Convert.ToDateTime(sFecha2);

            try
            {
                var oQuery = (from q in oMedicuriEntities.recetas
                              where q.Fecha >= fecha1 && q.Fecha <= fecha2 && (q.EstatusMedico == "1" || q.EstatusMedico == "2") && q.Estatus=="1"
                              select new
                              {
                                  q.idReceta,
                                  q.Folio,
                                  q.Fecha,
                                  q.EstatusMedico,
                                  q.Estatus,
                                  Total = (from rp in oMedicuriEntities.recetas_partida
                                           where rp.idReceta == q.idReceta
                                           select (rp.Precio) * (rp.CantidaSurtida)).Sum()

                              });
            
               return oQuery;
            }
            catch
            {
                return null;
            }

        }

        /// <summary>
        /// GT Función que regresa los datos de la receta para la pantalla de Facturas por programa filtrado por fecha
        /// </summary>
        /// <returns></returns>
        public object RecetasFacturarPorProgramaFecha(string sFecha1, string sFecha2,int iIdAlmacen)
        {
            //GT Esta es la funcion para cuando me filtren por fecha
            DateTime fecha1 = Convert.ToDateTime(sFecha1);
            DateTime fecha2 = Convert.ToDateTime(sFecha2);

            try
            {
                var oQuery = (from q in oMedicuriEntities.recetas
                              where q.Fecha >= fecha1 && q.Fecha <= fecha2 && q.idAlmacen == iIdAlmacen && (q.EstatusMedico == "1" || q.EstatusMedico == "2") && q.Estatus == "1"
                              select new
                              {
                                  q.idReceta,
                                  q.Folio,
                                  q.Fecha,
                                  q.EstatusMedico,
                                  q.Estatus,
                                  Total = (from rp in oMedicuriEntities.recetas_partida
                                           where rp.idReceta == q.idReceta
                                           select (rp.Precio) * (rp.CantidaSurtida)).Sum()

                              });

                return oQuery;
            }
            catch
            {
                return null;
            }

        }

        // <summary>
        /// GT Función que regresa los datos de la receta para la pantalla de Facturas por programa filstrado por folio
        /// </summary>
        /// <returns></returns>
        public object RecetasFacturarPorProgramaFolio(string sFolio1, string sFolio2)
        {
           
            //Esta es la consulta por folio
            try
            {
                var oQuery = (from q in oMedicuriEntities.recetas
                              where q.Folio.CompareTo(sFolio1) >= 0 && q.Folio.CompareTo(sFolio2) <= 0 && (q.EstatusMedico == "1" || q.EstatusMedico == "2") && q.Estatus == "1"
                              select new
                              {
                                  q.idReceta,
                                  q.Folio,
                                  q.Fecha,
                                  q.EstatusMedico,
                                  q.Estatus,
                                  Total = (from rp in oMedicuriEntities.recetas_partida
                                           where rp.idReceta == q.idReceta
                                           select (rp.Precio) * (rp.CantidaSurtida)).Sum()

                              });

                return oQuery;
            }
            catch
            {
                return null;
            }

        }

        // <summary>
        /// GT Función que regresa los datos de la receta para la pantalla de Facturas por programa filstrado por folio
        /// </summary>
        /// <returns></returns>
        public object RecetasFacturarPorProgramaFolio(string sFolio1, string sFolio2,int iIdAlmacen)
        {

            //Esta es la consulta por folio
            try
            {
                var oQuery = (from q in oMedicuriEntities.recetas
                              where q.Folio.CompareTo(sFolio1) >= 0 && q.Folio.CompareTo(sFolio2) <= 0 && q.idAlmacen == iIdAlmacen && (q.EstatusMedico == "1" || q.EstatusMedico == "2") && q.Estatus == "1"
                              select new
                              {
                                  q.idReceta,
                                  q.Folio,
                                  q.Fecha,
                                  q.EstatusMedico,
                                  q.Estatus,
                                  Total = (from rp in oMedicuriEntities.recetas_partida
                                           where rp.idReceta == q.idReceta
                                           select (rp.Precio) * (rp.CantidaSurtida)).Sum()

                              });

                return oQuery;
            }
            catch
            {
                return null;
            }

        }

        //// <summary>
        ///// GT Función que regresa los datos de la receta para la pantalla de Facturas por programa filtrado por almacen
        ///// </summary>
        ///// <returns></returns>
        //public object RecetasFacturarPorProgramaAlmacen(int iIdAlmacen)
        //{
        //    try
        //    {
        //        var oQuery = (from q in oMedicuriEntities.recetas
        //                      where q.idAlmacen == iIdAlmacen
        //                      select new
        //                      {
        //                          q.idReceta,
        //                          q.Folio,
        //                          q.Fecha,
        //                          q.EstatusMedico,
        //                          q.Estatus,
        //                          Total = (from rp in oMedicuriEntities.recetas_partida
        //                                   where rp.idReceta == q.idReceta
        //                                   select (rp.Precio) * (rp.CantidaSurtida)).Sum()

        //                      });

        //        return oQuery;
        //    }
        //    catch
        //    {
        //        return null;
        //    }

        //}

        /// <summary>
        /// GT Recuperar el monto total de una receta 
        /// </summary>
        /// <param name="sFolioReceta"></param>
        /// <returns></returns>
        public object RecetaPartidaMontoTotal(string sFolioReceta)
        {
            try
            {
                var aQuery =
                from p in oMedicuriEntities.recetas_partida
                where p.recetas.Folio == sFolioReceta
                group p by p.idReceta into g
                //select new { MontoTotal = g.Sum(p => p.Precio) };
                select new { MontoTotal=g.Sum(p=>p.Precio) };

                return aQuery;
               
            }
            catch
            {
                return 0;
            }
        }
   }
}
