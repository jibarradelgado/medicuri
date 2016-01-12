using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;

namespace MedNeg.RecuperarContraseña
{
    public class BlRecuperarContraseña
    {
        MedDAL.DAL.usuarios userValidate;
        Usuarios.BlUsuarios blUser;
        string newPassword;
        string currentUser;

        /// <summary>
        /// Constructor
        /// </summary>
        public BlRecuperarContraseña() {
            userValidate = new MedDAL.DAL.usuarios();
            blUser = new Usuarios.BlUsuarios();
        }

        /// <summary>
        /// Envia un correo por medio de un servidor SMTP
        /// </summary>
        /// <param name="from">correo remitente</param>
        /// <param name="fromPwd">contraseña del correo del remitente</param>
        /// <param name="userTo">usuario que solicito la recuperación de la contraseña</param>
        /// <param name="subject">encabezado del correo</param>
        /// <param name="smtpClient">sercidor smtp</param>
        /// <param name="port">puerto del servidor smtp</param>
        /// <returns>respuesta del envio</returns>
        public string SendMail(string from, string fromPwd, string userTo, string subject, string smtpClient, int port)
        {
            currentUser = userTo;
            userTo = GetCorreoUsuario(currentUser);
            if (userTo.Equals(string.Empty))
                return "El usuario no esta registrado.";
            else if (!InsertPassword(currentUser))
                return "No se ha podido crear una nueva contraseña. Contacte a su administrador";
            else
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(from);
                mail.To.Add(userTo);
                mail.Subject = subject;
                mail.IsBodyHtml = true;
                mail.Body = GetMsg(from, currentUser);
                SmtpClient smtp = new SmtpClient(smtpClient);
                smtp.Credentials = new System.Net.NetworkCredential(from, fromPwd);
                smtp.Port = port;
                try
                {
                    smtp.Send(mail);
                    return "Se ha enviado su nueva contraseña. Revise su correo y vuelva a intentarlo";
                }
                catch (Exception ex)
                {
                    return "No se ha podido completar la solicitud: " + ex.Message;
                }
            }   
        }

        /// <summary>
        /// Crea el cuerpo del correo
        /// </summary>
        /// <param name="from">remitente</param>
        /// <param name="currentUser">usuario actual que solicito el servicio</param>
        /// <returns></returns>
        private string GetMsg(string from, string currentUser)
        {

            string s = "";
            s = " <div style=\"font-family:Arial;font-size:15px;\" > <br>Este es un correo automatico de:";
            s = s + "<br>email: " + from;

            s = s + "<br><br>Mensaje:";
            s = s + "<br>Se ha actualizado tu contraseña, ahora intenta iniciar sesión de nuevo";
            s = s + "<br>Usuario: " + currentUser;
            s = s + "<br>Contraseña: " + newPassword;
            s = s + "<br><br>---<br>Favor de NO contestar a este dirección de correo. Para mayor información consulte a su administrador </div>";
            return s;
        }


        /// <summary>
        /// registra la nueva contraseña
        /// </summary>
        /// <param name="sUsuario">usuario que colicito el servicio</param>
        /// <returns></returns>
        private bool InsertPassword(string sUsuario) {
            newPassword = CreateRandomPassword(6);
            return blUser.CambiarContraseña(sUsuario, blUser.EncriptarMD5(newPassword));
        }

        /// <summary>
        /// Obtiene el correo del usuario que solicito el servicio
        /// </summary>
        /// <param name="sUsuario"></param>
        /// <returns></returns>
        private string GetCorreoUsuario(string sUsuario)
        {
            userValidate = (MedDAL.DAL.usuarios)blUser.Buscar(sUsuario);
            if (userValidate != null)
            {
                return userValidate.CorreoElectronico;
            }
            else
                return string.Empty;
        }

        /// <summary>
        /// Crea una nueva contraseña alfanumerica
        /// </summary>
        /// <param name="PasswordLength">tamaño de la contraseña</param>
        /// <returns></returns>
        private static string CreateRandomPassword(int PasswordLength)
        {
            string _allowedChars = "abcdefghijkmnpqrstuvwxyzABCDEFGHJKLMNPQRSTUVWXYZ23456789!@$?";
            Byte[] randomBytes = new Byte[PasswordLength];
            char[] chars = new char[PasswordLength];
            int allowedCharCount = _allowedChars.Length;

            for (int i = 0; i < PasswordLength; i++)
            {
                Random randomObj = new Random();
                randomObj.NextBytes(randomBytes);
                chars[i] = _allowedChars[(int)randomBytes[i] % allowedCharCount];
            }

            return new string(chars);
        } 
    }
}
