﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.239
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.239.
// 
#pragma warning disable 1591

namespace MedNeg.FacturaService {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.ComponentModel;
    using System.Xml.Serialization;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="BasicHttpBinding_ITimbrado", Namespace="http://tempuri.org/")]
    public partial class svcTimbrado : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback TimbrarOperationCompleted;
        
        private System.Threading.SendOrPostCallback TimbrarTestOperationCompleted;
        
        private System.Threading.SendOrPostCallback PDFOperationCompleted;
        
        private System.Threading.SendOrPostCallback CancelarOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public svcTimbrado() {
            this.Url = global::MedNeg.Properties.Settings.Default.MedNeg_FacturaService_svcTimbrado;
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event TimbrarCompletedEventHandler TimbrarCompleted;
        
        /// <remarks/>
        public event TimbrarTestCompletedEventHandler TimbrarTestCompleted;
        
        /// <remarks/>
        public event PDFCompletedEventHandler PDFCompleted;
        
        /// <remarks/>
        public event CancelarCompletedEventHandler CancelarCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/ITimbrado/Timbrar", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public RespuestaCFDi Timbrar([System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string Usuario, [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string Password, [System.Xml.Serialization.XmlElementAttribute(DataType="base64Binary", IsNullable=true)] byte[] ArchivoXML) {
            object[] results = this.Invoke("Timbrar", new object[] {
                        Usuario,
                        Password,
                        ArchivoXML});
            return ((RespuestaCFDi)(results[0]));
        }
        
        /// <remarks/>
        public void TimbrarAsync(string Usuario, string Password, byte[] ArchivoXML) {
            this.TimbrarAsync(Usuario, Password, ArchivoXML, null);
        }
        
        /// <remarks/>
        public void TimbrarAsync(string Usuario, string Password, byte[] ArchivoXML, object userState) {
            if ((this.TimbrarOperationCompleted == null)) {
                this.TimbrarOperationCompleted = new System.Threading.SendOrPostCallback(this.OnTimbrarOperationCompleted);
            }
            this.InvokeAsync("Timbrar", new object[] {
                        Usuario,
                        Password,
                        ArchivoXML}, this.TimbrarOperationCompleted, userState);
        }
        
        private void OnTimbrarOperationCompleted(object arg) {
            if ((this.TimbrarCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.TimbrarCompleted(this, new TimbrarCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/ITimbrado/TimbrarTest", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public RespuestaCFDi TimbrarTest([System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string Usuario, [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string Password, [System.Xml.Serialization.XmlElementAttribute(DataType="base64Binary", IsNullable=true)] byte[] ArchivoXML) {
            object[] results = this.Invoke("TimbrarTest", new object[] {
                        Usuario,
                        Password,
                        ArchivoXML});
            return ((RespuestaCFDi)(results[0]));
        }
        
        /// <remarks/>
        public void TimbrarTestAsync(string Usuario, string Password, byte[] ArchivoXML) {
            this.TimbrarTestAsync(Usuario, Password, ArchivoXML, null);
        }
        
        /// <remarks/>
        public void TimbrarTestAsync(string Usuario, string Password, byte[] ArchivoXML, object userState) {
            if ((this.TimbrarTestOperationCompleted == null)) {
                this.TimbrarTestOperationCompleted = new System.Threading.SendOrPostCallback(this.OnTimbrarTestOperationCompleted);
            }
            this.InvokeAsync("TimbrarTest", new object[] {
                        Usuario,
                        Password,
                        ArchivoXML}, this.TimbrarTestOperationCompleted, userState);
        }
        
        private void OnTimbrarTestOperationCompleted(object arg) {
            if ((this.TimbrarTestCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.TimbrarTestCompleted(this, new TimbrarTestCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/ITimbrado/PDF", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public RespuestaCFDi PDF([System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string Usuario, [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string Password, [System.Xml.Serialization.XmlElementAttribute(DataType="base64Binary", IsNullable=true)] byte[] ArchivoXML, [System.Xml.Serialization.XmlElementAttribute(DataType="base64Binary", IsNullable=true)] byte[] ArchivoACK) {
            object[] results = this.Invoke("PDF", new object[] {
                        Usuario,
                        Password,
                        ArchivoXML,
                        ArchivoACK});
            return ((RespuestaCFDi)(results[0]));
        }
        
        /// <remarks/>
        public void PDFAsync(string Usuario, string Password, byte[] ArchivoXML, byte[] ArchivoACK) {
            this.PDFAsync(Usuario, Password, ArchivoXML, ArchivoACK, null);
        }
        
        /// <remarks/>
        public void PDFAsync(string Usuario, string Password, byte[] ArchivoXML, byte[] ArchivoACK, object userState) {
            if ((this.PDFOperationCompleted == null)) {
                this.PDFOperationCompleted = new System.Threading.SendOrPostCallback(this.OnPDFOperationCompleted);
            }
            this.InvokeAsync("PDF", new object[] {
                        Usuario,
                        Password,
                        ArchivoXML,
                        ArchivoACK}, this.PDFOperationCompleted, userState);
        }
        
        private void OnPDFOperationCompleted(object arg) {
            if ((this.PDFCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.PDFCompleted(this, new PDFCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/ITimbrado/Cancelar", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public RespuestaCFDi Cancelar([System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string Usuario, [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string Password, [System.Xml.Serialization.XmlElementAttribute(DataType="base64Binary", IsNullable=true)] byte[] PFX, [System.Xml.Serialization.XmlArrayAttribute(IsNullable=true)] [System.Xml.Serialization.XmlArrayItemAttribute(Namespace="http://schemas.microsoft.com/2003/10/Serialization/Arrays")] string[] UUID, [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string ContraseñaPFX) {
            object[] results = this.Invoke("Cancelar", new object[] {
                        Usuario,
                        Password,
                        PFX,
                        UUID,
                        ContraseñaPFX});
            return ((RespuestaCFDi)(results[0]));
        }
        
        /// <remarks/>
        public void CancelarAsync(string Usuario, string Password, byte[] PFX, string[] UUID, string ContraseñaPFX) {
            this.CancelarAsync(Usuario, Password, PFX, UUID, ContraseñaPFX, null);
        }
        
        /// <remarks/>
        public void CancelarAsync(string Usuario, string Password, byte[] PFX, string[] UUID, string ContraseñaPFX, object userState) {
            if ((this.CancelarOperationCompleted == null)) {
                this.CancelarOperationCompleted = new System.Threading.SendOrPostCallback(this.OnCancelarOperationCompleted);
            }
            this.InvokeAsync("Cancelar", new object[] {
                        Usuario,
                        Password,
                        PFX,
                        UUID,
                        ContraseñaPFX}, this.CancelarOperationCompleted, userState);
        }
        
        private void OnCancelarOperationCompleted(object arg) {
            if ((this.CancelarCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.CancelarCompleted(this, new CancelarCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://schemas.datacontract.org/2004/07/FacturaService")]
    public partial class RespuestaCFDi {
        
        private byte[] documentoField;
        
        private string mensajeField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="base64Binary", IsNullable=true)]
        public byte[] Documento {
            get {
                return this.documentoField;
            }
            set {
                this.documentoField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string Mensaje {
            get {
                return this.mensajeField;
            }
            set {
                this.mensajeField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void TimbrarCompletedEventHandler(object sender, TimbrarCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class TimbrarCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal TimbrarCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public RespuestaCFDi Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((RespuestaCFDi)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void TimbrarTestCompletedEventHandler(object sender, TimbrarTestCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class TimbrarTestCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal TimbrarTestCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public RespuestaCFDi Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((RespuestaCFDi)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void PDFCompletedEventHandler(object sender, PDFCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class PDFCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal PDFCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public RespuestaCFDi Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((RespuestaCFDi)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void CancelarCompletedEventHandler(object sender, CancelarCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class CancelarCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal CancelarCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public RespuestaCFDi Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((RespuestaCFDi)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591