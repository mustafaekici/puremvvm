using System;
using System.Collections.Generic;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.Linq;
using System.ServiceModel;
using System.Web;

namespace WcfService
{
    public class CustomUserNameValidator : UserNamePasswordValidator
    {
        public override void Validate(string userName, string password)
        {
            string userpass = userName + password;
            List<string> users = new List<string> {"mustafa1","metin1","feyyaz1","ali1","selim1","timur1"};
            if (users.Contains(userpass))
            {
                //ok
            }
            else
            {
                throw new SecurityTokenException("Unknown Username or Password");
            }
        }
    }
}
