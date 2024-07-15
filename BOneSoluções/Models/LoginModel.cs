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
            CompanyDB = Program.oCompany.CompanyDB;
            UserName = Program.oCompany.UserName;
            Password = "3060";
            Language = "19"; //ln_Portuguese = 19
        }
    }
}
