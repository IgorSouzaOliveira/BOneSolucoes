﻿using BOneSolucoes.Models;
using RestSharp;
using SAPbouiCOM.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace BOneSolucoes.Comonn
{
    class SAPCommon
    {
        private static String _slAddress;
        private static String _slServer;
        private static String B1Session;
        public static int SessionTimeout { get; set; }
        public static DateTime ConnectionTime { get; set; }
        private static bool ValidaToken()
        {
            DateTime timeNow = DateTime.Now;

            if (SessionTimeout == 0) { SAPConnect(); return true; };

            if (ConnectionTime.Subtract(timeNow).Minutes >= SessionTimeout)
            {
                return false;
            }

            return true;

        }
        private static void ReadDataConnection()
        {
            SAPbobsCOM.Recordset oRst = (SAPbobsCOM.Recordset)Program.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

            try
            {
                oRst.DoQuery(@"SELECT T0.""U_UrlSL"", T0.""U_PortaSL"", T0.""U_ServidorSL"" FROM [@BONECONFMAIN] T0 ");
                if (oRst.RecordCount == 0)
                    return;

                oRst.MoveFirst();
                for (int i = 0; i < oRst.RecordCount; i++)
                {
                    _slAddress = $"{oRst.Fields.Item("U_UrlSL").Value.ToString()}:{oRst.Fields.Item("U_PortaSL").Value.ToString()}/b1s/v1";
                    _slServer = oRst.Fields.Item("U_ServidorSL").Value.ToString();
                }



            }
            catch (Exception ex)
            {
                Application.SBO_Application.MessageBox(ex.Message, 1, "Ok", "Cancelar");
            }
        }
        public static String SAPConnect()
        {
            try
            {
                LoginModel login = new LoginModel();
                ReadDataConnection();

                var client = new RestClient(_slAddress);
                var request = new RestRequest("/Login", Method.POST);

                var body = Newtonsoft.Json.JsonConvert.SerializeObject(login);
                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("application/json", body, ParameterType.RequestBody);

                ServicePointManager.ServerCertificateValidationCallback += new System.Net.Security.RemoteCertificateValidationCallback(ValidateServerCertificate);

                IRestResponse rest = client.Execute(request);

                B1Session = rest.Cookies.FirstOrDefault()?.Value;
                
                if (rest.StatusCode == HttpStatusCode.OK)
                {
                    dynamic ret = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(rest.Content);
                    SessionTimeout = ret.SessionTimeout;
                    ConnectionTime = DateTime.Now;

                    return B1Session;
                }
                else
                {
                    Application.SBO_Application.StatusBar.SetText($"Service Layer: {rest.StatusDescription}", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
                    return null;
                }
            }
            catch (Exception ex)
            {
                Application.SBO_Application.StatusBar.SetText($"Service Layer: {ex.Message}", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
                return null;
            }
        }

        /*Metodo para atualizar cadastro de PN*/
        public static String UpdateBP(BusinessPartnerModel oBP)
        {
            if (ValidaToken().Equals(false))
                SAPConnect();

            try
            {

                var client = new RestClient(_slAddress);
                var request = new RestRequest($"/BusinessPartners('{oBP.CardCode}')", Method.PATCH);

                var body = Newtonsoft.Json.JsonConvert.SerializeObject(oBP);
                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("application/json", body, ParameterType.RequestBody);

                CookieContainer cookiecon = new CookieContainer();
                cookiecon.Add(new Cookie("B1SESSION", B1Session, "/b1s/v1", _slServer));
                client.CookieContainer = cookiecon;

                ServicePointManager.ServerCertificateValidationCallback += new System.Net.Security.RemoteCertificateValidationCallback(ValidateServerCertificate);

                IRestResponse response = client.Execute(request);

                if (response.StatusCode != HttpStatusCode.NoContent)
                {
                    dynamic ret = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(response.Content);
                    throw new Exception(ret.error.message.value.ToString());
                }              

                return "Sucesso";

            }
            catch (Exception ex)
            {
                Application.SBO_Application.MessageBox($"Service Layer: {oBP.CardCode} - {ex.Message}", 1, "Ok", "Cancelar");
                return null;
            }

        }        

        /*Metodo para faturar pedidos*/
        public static InvoiceModel AddInvoice(InvoiceModel oInvoice, int docEntry)
        {
            if (ValidaToken().Equals(false))
                SAPConnect();

            try
            {
                var client = new RestClient(_slAddress);
                var request = new RestRequest($"/Invoices", Method.POST);

                var body = Newtonsoft.Json.JsonConvert.SerializeObject(oInvoice);
                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("application/json", body, ParameterType.RequestBody);

                CookieContainer cookiecon = new CookieContainer();
                cookiecon.Add(new Cookie("B1SESSION", B1Session, "/b1s/v1", _slServer));
                client.CookieContainer = cookiecon;

                ServicePointManager.ServerCertificateValidationCallback += new System.Net.Security.RemoteCertificateValidationCallback(ValidateServerCertificate);

                IRestResponse response = client.Execute(request);

                InvoiceModel notaRetorno = new InvoiceModel();

                if (response.StatusCode != HttpStatusCode.Created)
                {
                    dynamic ret = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(response.Content);
                    throw new Exception($"Erro ao Gerar Nota Fiscal de Saida. {Environment.NewLine} Pedido Nº {docEntry} {Environment.NewLine} Detalhe: {ret.error.message.value.ToString()}.");
                }

                notaRetorno = Newtonsoft.Json.JsonConvert.DeserializeObject<InvoiceModel>(response.Content);
                Application.SBO_Application.MessageBox($"Nota Fiscal de Saida Gerada com sucesso. {Environment.NewLine} Nº Documento: {notaRetorno.DocEntry} {Environment.NewLine} Nº Nota Fiscal: {notaRetorno.SequenceSerial}");

                return notaRetorno;
            }
            catch (Exception ex)
            {
                Application.SBO_Application.MessageBox(ex.Message,1,"Ok","Cancelar");
                return null;
            }
        }
        public static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

    }
}
