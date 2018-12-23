using MessengerApplication.WebUI.Abstract;
using MessengerApplication.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MessengerApplication.WebUI.Concrete
{
    public class EFUserStatsRepository : IUserStatsRepository
    {
        List<ApplicationUser> listofUsers = new List<ApplicationUser>();


        private ApplicationDbContext context = new ApplicationDbContext();



        public List<ApplicationUser> GetUserFromDB(Func<ApplicationUser, bool> func)
        {

            List<ApplicationUser> userlist = context.Users.Where(func).AsEnumerable<ApplicationUser>().ToList();
            
            return userlist;
         

        }

        public List<ApplicationUser> GetUserFromList(Func<ApplicationUser, bool> func)
        {

            List<ApplicationUser> userlist = listofUsers.Where(func).AsEnumerable<ApplicationUser>().ToList();

            return userlist;


        }



        public List<ApplicationUser> GetUsers(int HowMany = 10, string FirstName = "", string Surname = "", string City = "", int Age = 0)
        {

            bool AddToList = false;

            listofUsers = context.Users.ToList();
           listofUsers= listofUsers.Take(HowMany).ToList();

            List<Func<ApplicationUser, bool>> listfunc = new List<Func<ApplicationUser, bool>>();
            if(FirstName!="")
            {
              listfunc.Add((x) => x.FirstName == FirstName);
            }
            
            if(Surname!="")
            {
              listfunc.Add((x) => x.Surname == Surname);
            }
            
            if(City!="")
            {
               listfunc.Add((x) => x.City == City);
            }
           

            if (Age > 0)
            {
                listfunc.Add((x) => x.Age == Age);
            }


            foreach (var item in listfunc)
            {

                if(AddToList)
                {
                    
                       listofUsers=GetUserFromList(item);
                                   
                   


                }
                else
                {

                    listofUsers = GetUserFromDB(item);
                    AddToList = true;
                    if(listofUsers==null)
                    {
                        return new List<ApplicationUser>();
                    }
                    
                }


              


            }

  return listofUsers;

        }




        public List<ApplicationUser> GetUsers(int HowMany = 20)
        {
            List<ApplicationUser> list = context.Users.Take(HowMany).ToList();
            return list;
        }







    }








    



       

}