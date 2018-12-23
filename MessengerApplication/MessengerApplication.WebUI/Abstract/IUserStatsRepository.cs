using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessengerApplication.WebUI.Abstract
{
   public interface IUserStatsRepository
    {

        List<Models.ApplicationUser> GetUsers(int HowMany=10, string FirstName="",string Surname="",string City="",int Age=0);
        List<Models.ApplicationUser> GetUsers(int HowMany = 20);


    }
}
