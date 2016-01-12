using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MedNeg.Ensambles
{
    public class BlEnsambles
    {
        MedDAL.Ensambles.DALEnsambles dalEnsambles;
        MedDAL.EnsambleProductos.DALEnsambleProductos dalEnsambleProductos;

        public BlEnsambles() {
            dalEnsambles = new MedDAL.Ensambles.DALEnsambles();
            dalEnsambleProductos = new MedDAL.EnsambleProductos.DALEnsambleProductos();
        }

        public bool NuevoRegistroEnsamble(MedDAL.DAL.ensamble oEnsamble) {
            return dalEnsambles.NuevoRegistro(oEnsamble);
        }

        public bool NuevoRegistroEnsambleProductos(MedDAL.DAL.ensamble_productos oEnsambleProductos) {
            return dalEnsambleProductos.NuevoRegistro(oEnsambleProductos);
        }

        public bool NuevoRegistroEnsambleProductos(List<MedDAL.DAL.ensamble_productos> lstEnsambleProductos, int idEnsamble) {
            bool result = true;
            MedDAL.DAL.ensamble_productos nvoEnsambleProducto;
            foreach(MedDAL.DAL.ensamble_productos eProductos in lstEnsambleProductos){
                nvoEnsambleProducto = new MedDAL.DAL.ensamble_productos();
                nvoEnsambleProducto.idEnsamble = idEnsamble;
                nvoEnsambleProducto.idProducto = eProductos.idProducto;
                nvoEnsambleProducto.Cantidad = eProductos.Cantidad;
                result = result & dalEnsambleProductos.NuevoRegistro(nvoEnsambleProducto);
            }
            return result;
        }

        public bool EliminarEnsamble(int idEnsamble) {
            return dalEnsambles.EliminarRegistro(idEnsamble);
        }

        public bool EliminarEnsambleProductos(int idEnsamble){
            return dalEnsambleProductos.EliminarRegistro(idEnsamble);
        }

        public int ValidarEnsamble(string claveBom) {
            return dalEnsambles.ValidarEnsambleRepetido(claveBom);
        }

        public bool EditarRegistroEnsamble(MedDAL.DAL.ensamble oEnsamble) {
            return dalEnsambles.EditarRegistro(oEnsamble);
        }

        public object BuscarEnsamble(string claveBom) {
            return dalEnsambles.Buscar(claveBom);
        }

        public MedDAL.DAL.ensamble BuscarEnsamble1(string sClaveBom)
        {
            return dalEnsambles.BuscarEnsamble1(sClaveBom);
        }

        public MedDAL.DAL.ensamble BuscarNombre(string sNombre)
        {
            return dalEnsambles.BuscarNombre(sNombre);
        }
        public List<MedDAL.DAL.ensamble_productos> BuscarProductosEnsamble(int idEnsamble) {
            return dalEnsambleProductos.Buscar(idEnsamble);
        }

        public MedDAL.DAL.ensamble BuscarEnsamble(int idEnsamble)
        {
            return dalEnsambles.Buscar(idEnsamble);
        }

        public IQueryable<MedDAL.DAL.ensamble_productos> RecuperarProductos(string sClaveBOM)
        {
            return dalEnsambleProductos.RecuperarProductos(sClaveBOM);
        }

        public MedDAL.DAL.ensamble_productos RecuperarProducto(string sClaveBOM)
        {
            return dalEnsambleProductos.RecuperarProducto(sClaveBOM);
        }
    }
}
