using SAPbouiCOM.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOneSolucoes.Models
{
    class LoginModel
    {
        public string CompanyDB { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public string Language { get; set; }

        public LoginModel()
        {
            ConnectData();
        }

        private void ConnectData()
        {

            SAPbobsCOM.Recordset oRst = (SAPbobsCOM.Recordset)Program.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

            try
            {
                oRst.DoQuery(@"SELECT T0.""U_UsuarioSL"", T0.""U_SenhaSL"" FROM [@BONECONFMAIN] T0 WHERE T0.""Code"" = 1");
                if (oRst.RecordCount > 0)
                {
                    oRst.MoveFirst();
                    for (int i = 0; i < oRst.RecordCount; i++)
                    {
                        CompanyDB = Program.oCompany.CompanyDB;
                        UserName = oRst.Fields.Item("U_UsuarioSL").Value.ToString();
                        Password = oRst.Fields.Item("U_SenhaSL").Value.ToString();
                        Language = "19"; //ln_Portuguese = 19
                    }
                    
                }

            }
            catch (Exception ex)
            {
                Application.SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error);

            }
            finally
            {
                if (oRst != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oRst);
                }
            }
        }
    }
}
