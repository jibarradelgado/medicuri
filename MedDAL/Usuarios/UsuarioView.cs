using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MedDAL.Usuarios
{
    public class UsuarioView
    {
        int iIdUsuario;
        string sUsuario, sNombre, sApellidos, sCorreoElectronico, sPerfil;
        bool bActivo;

        public int idUsuario{
            get { return iIdUsuario; }
            set { this.iIdUsuario = value; } 
        }
        public string Usuario{
            get { return sUsuario; }
            set { this.sUsuario = value; }
        }
        public string Nombre{
            get { return sNombre; }
            set { this.sNombre = value; }
        }
        public string Apellidos{
            get { return sApellidos; }
            set { this.sApellidos = value; }
        }
        public string CorreoElectronico{
            get { return sCorreoElectronico; }
            set { this.sCorreoElectronico = value; }
        }
        public string Perfil{
            get { return sPerfil; }
            set { this.sPerfil = value; }
        }
        public bool Activo{
            get { return bActivo; }
            set { this.bActivo = value; }
        }        

        public UsuarioView() { }
    }
}
