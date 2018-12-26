using MessengerApplication.WebUI.Abstract;
using MessengerApplication.WebUI.Entities;
using MessengerApplication.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MessengerApplication.WebUI.Concrete
{

    class ReceiverComparer : IEqualityComparer<ReceiverDataViewModel>
    {
        public bool Equals(ReceiverDataViewModel x, ReceiverDataViewModel y)
        {
            // Two items are equal if their keys are equal.
            return x.Id == y.Id;
        }

        public int GetHashCode(ReceiverDataViewModel obj)
        {
            return obj.Id.GetHashCode();
        }
    }




    public class EFUserStatsRepository : IUserStatsRepository
    {
        List<ApplicationUser> listofUsers = new List<ApplicationUser>();


        private ApplicationDbContext context = new ApplicationDbContext();


        public bool AddEmptyMessage(string Sender, string Receiver)
        {
            //try
            //{
             ApplicationUser sender= context.Users.Find(Sender);
            ApplicationUser receiver = context.Users.Find(Receiver);
            Message newMessage = new Message() { IsRead = false, MessageData = " Say Hello :)", ReceiverName = Receiver, SendTime =DateTime.Now, SenderName = Sender };
            context.Messages.Add(newMessage);
            sender.Messages.Add(newMessage);
            receiver.Messages.Add(newMessage);
            context.SaveChanges();

                return true;
          //  }
          //catch
          //  {
          //      return false;
          //  }





        }

        public List<ApplicationUser> AutocompleteName(string name)
        {
            List<ApplicationUser> list = context.Users.Where(x => x.FirstName.ToLower().StartsWith(name.ToLower())).ToList();
            return list;
        }






        public List<Message> GetMessages(string SenderId,string UserId)
        {
            List<Message> list;

            try
            {

                list = context.Users.Where(x => x.Id == UserId).First().Messages.Where(y => (y.ReceiverName == SenderId) || (y.SenderName == SenderId)).OrderBy(z => z.MessageData).ToList();

            }
            catch
            {

                list = null;

            }


            return list;


        }






        public List<ReceiverDataViewModel> GetReceiverData(string UserId)
        {

            List<Message> list;

            try
            {
           list= context.Users.Where(x => x.Id == UserId).First().Messages.OrderBy(y => y.SendTime).ToList();
            }
            catch
            {
                list = null;
            }
            
            List<ReceiverDataViewModel> listReceivers = new List<ReceiverDataViewModel>();

            if (list!=null)
            {

                foreach (var item in list)
                {
                    ApplicationUser user = context.Users.Find(item.SenderName);
                    ReceiverDataViewModel receiver = new ReceiverDataViewModel() { Id = item.SenderName , IsRead = item.IsRead, FullName = user.FirstName+ " "+user.Surname  };//get Name by context

                    listReceivers.Add(receiver);


                }



            }


            return listReceivers.Distinct(new ReceiverComparer()).ToList();


        }






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