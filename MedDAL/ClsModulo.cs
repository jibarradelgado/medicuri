using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MedDAL.DAL;

namespace MedDAL
{
    public abstract class ClsModulo
    {
        public medicuriEntities oEntitiesBase;

        protected List<string> lstErrores = new List<string>();

        public bool Add<T>(T objeto) where T : class
        {
            using (medicuriEntities oEntities1 = new medicuriEntities())
            {
                try
                {
                    var os = oEntities1.CreateObjectSet<T>();
                    os.AddObject(objeto);
                    oEntities1.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    lstErrores.Add(ex.Message);
                    return false;
                }
            }
        }

        public bool Add<T>(T objeto, medicuriEntities oEntities) where T : class
        {            
            try
            {
                var os = oEntities.CreateObjectSet<T>();
                os.AddObject(objeto);                
                return true;
            }
            catch (Exception ex)
            {
                lstErrores.Add(ex.Message);
                return false;
            }           
        }

        public bool Delete<T>(T objeto, medicuriEntities oEntities) where T : class
        {
            try
            {
                oEntities.DeleteObject(objeto);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 13/11/2012
        /// Jorge Ibarra
        /// Modifica los datos encontrados en el contexto y modifica la base de datos
        /// </summary>
        /// <returns></returns>
        public bool SaveChanges(medicuriEntities oEntities)
        {
            Clear();
            try
            {
                oEntities.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                lstErrores.Add(ex.ToString());
                return false;
            }
        }

        //public virtual bool Search(Control oControl, int iValor, string sValor,
        //    DateTime tFecha1 = new DateTime(), DateTime tFecha2 = new DateTime())
        //{
        //    return false;
        //}

        /// <summary>
        /// metodo que limpia atributos que deben limpiarse
        /// </summary>
        protected void Clear()
        {
            lstErrores = new List<string>();
        }

        /// <summary>
        /// Obtiene listado de errores
        /// </summary>
        /// <returns>regresa la lista de errores</returns>
        public string GetError()
        {
            string err = "";
            foreach (string e in lstErrores)
            {
                err += e + ",";
            }
            err = err.Substring(0, err.Length - 1);
            return err;
        }
    }
}
