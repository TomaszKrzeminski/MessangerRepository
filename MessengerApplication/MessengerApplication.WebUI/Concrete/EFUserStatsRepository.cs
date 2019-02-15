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



    class UserComparer : IEqualityComparer<string>
    {
        public bool Equals(string x, string y)
        {
            // Two items are equal if their keys are equal.
            return x == y;
        }



        public int GetHashCode(string obj)
        {
            return obj.GetHashCode();
        }
    }







    public class EFUserStatsRepository : IUserStatsRepository
    {
        List<ApplicationUser> listofUsers = new List<ApplicationUser>();


        private ApplicationDbContext context = new ApplicationDbContext();


        public bool AddEmptyMessage(string Sender, string Receiver)
        {
            try
            {
                ApplicationUser sender = context.Users.Find(Sender);
                ApplicationUser receiver = context.Users.Find(Receiver);
                Message newMessage = new Message() { IsRead = false, MessageData = " Say Hello :)", ReceiverName = receiver.FirstName + " " + receiver.Surname, SendTime = DateTime.Now, SenderName = sender.FirstName + " " + sender.Surname, ReceiverId = Receiver, SenderId = Sender };
                context.Messages.Add(newMessage);
                sender.Messages.Add(newMessage);
                receiver.Messages.Add(newMessage);
                context.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }





        }






        public bool AddMessage(string ReceiverId, string SenderId, Message message)
        {

            if (ReceiverId == SenderId)
            {


            }
            else
            {



            }


            try
            {
                ApplicationUser sender = context.Users.Find(SenderId);
                ApplicationUser receiver = context.Users.Find(ReceiverId);
                Message newMessage = new Message() { IsRead = false, MessageData = message.MessageData, ReceiverName = receiver.FirstName + " " + receiver.Surname, SendTime = DateTime.Now, SenderName = sender.FirstName + " " + sender.Surname, ReceiverId = ReceiverId, SenderId = SenderId };
                context.Messages.Add(newMessage);
                sender.Messages.Add(newMessage);
                receiver.Messages.Add(newMessage);
                context.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }





        }








        public List<ApplicationUser> AutocompleteName(string Id, string name)
        {

            List<ApplicationUser> list = context.Users.Where(x => x.FirstName.ToLower().StartsWith(name.ToLower())).ToList();
            list = list.Except(list.Where(x => x.Id == Id)).ToList();
            return list;
        }




        public bool ChangeMessagesToRead(string UserId, string SenderId)
        {


            List<Message> messages = new List<Message>();


            try
            {

                messages = context.Users.Where(x => x.Id == UserId).First().Messages.Where(y => (y.ReceiverId == UserId && y.SenderId == SenderId && y.IsRead == false)).OrderBy(x => x.SendTime)
       .ThenBy(x => x.SendTime.Date)
       .ThenBy(x => x.SendTime.Year).ToList();


                foreach (var item in messages)
                {
                    try
                    {
                        Message message = context.Messages.Find(item.MessageId);
                        message.IsRead = true;
                        context.SaveChanges();
                    }
                    catch
                    {
                        return false;
                    }




                }






                return true;

            }
            catch
            {



                return false;
            }






        }


        public bool CheckIfReceiverIsAdded(string NewPerson, string UserId)
        {


            ApplicationUser Sender = context.Users.Find(UserId);

            try
            {
                Message message = Sender.Messages.Where(x => x.ReceiverId == NewPerson || x.SenderId == NewPerson).First();

                if (message != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }


            }
            catch
            {

                return false;

            }



        }

        public int CheckUnreadedMessages(string UserId)
        {

            int number = 0;

            try
            {

                List<Message> list = context.Users.Where(x => x.Id == UserId).First().Messages.Where(m => m.IsRead == false).ToList();

                foreach (var item in list)
                {
                    if (item.SenderId == UserId)
                    {
                        list.Remove(item);
                    }
                    
                }

                number = list.Count();
                return number;

            }
            catch
            {

                return 0;
            }
            


        }

        public List<Message> GetMessages(string SenderId, string UserId)
        {
            List<Message> list;

            try
            {


                list = context.Users.Where(x => x.Id == UserId).First().Messages.Where(y => (y.ReceiverId == SenderId) || (y.SenderId == SenderId)).OrderBy(x => x.SendTime)
        .ThenBy(x => x.SendTime.Date)
        .ThenBy(x => x.SendTime.Year).ToList();
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
            List<string> AllUsers = new List<string>();
            List<ReceiverDataViewModel> listReceivers = new List<ReceiverDataViewModel>();

            try
            {
                list = context.Users.Where(x => x.Id == UserId).First().Messages.OrderByDescending(y => y.SendTime).ToList();
            }
            catch
            {
                list = null;
            }



            if (list != null)
            {



                List<string> ReceiverList = list.Select(l => l.ReceiverId).ToList();
                List<string> SenderList = list.Select(l => l.SenderId).ToList();


                if (ReceiverList != null)
                {

                    AllUsers.AddRange(ReceiverList);
                }

                if (SenderList != null)
                {

                    AllUsers.AddRange(SenderList);
                }


                AllUsers = AllUsers.Distinct().ToList();
                AllUsers.Remove(UserId);


            }









            if (AllUsers.Count() > 0 && list != null)
            {


                foreach (var item in AllUsers)
                {

                    ApplicationUser user = context.Users.Find(item);

                    bool MessageIsRead = true;

                    foreach (var message in list)
                    {



                        if (message.SenderId == item && message.IsRead == false)
                        {
                            MessageIsRead = false;
                        }



                    }

                    listReceivers.Add(new ReceiverDataViewModel() { Id = item, IsRead = MessageIsRead, FullName = user.FirstName + " " + user.Surname });

                }









            }


            return listReceivers;


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

        public string GetUserNameById(string Id)
        {

            try
            {
                ApplicationUser user = context.Users.Find(Id);
                string FullName = user.FirstName + " " + user.Surname;
                return FullName;
            }
            catch
            {
                return " ";
            }

        }

        public string GetUserNameForSignalR(string Id)
        {



            try
            {
                ApplicationUser user = context.Users.Find(Id);

                return user.UserName;

            }
            catch
            {
                return " ";
            }









        }

        public List<ApplicationUser> GetUsers(string Id, int HowMany = 10, string FirstName = "", string Surname = "", string City = "", int Age = 0)
        {

            bool AddToList = false;

            listofUsers = context.Users.ToList();
            listofUsers = listofUsers.Take(HowMany).ToList();

            List<Func<ApplicationUser, bool>> listfunc = new List<Func<ApplicationUser, bool>>();
            if (FirstName != "")
            {
                listfunc.Add((x) => x.FirstName == FirstName);
            }

            if (Surname != "")
            {
                listfunc.Add((x) => x.Surname == Surname);
            }

            if (City != "")
            {
                listfunc.Add((x) => x.City == City);
            }


            if (Age > 0)
            {
                listfunc.Add((x) => x.Age == Age);
            }


            foreach (var item in listfunc)
            {
                if (AddToList)
                {
                    listofUsers = GetUserFromList(item);
                }
                else
                {
                    listofUsers = GetUserFromDB(item);
                    AddToList = true;
                    if (listofUsers == null)
                    {
                        return new List<ApplicationUser>();
                    }
                }


            }
            listofUsers = listofUsers.Except(listofUsers.Where(x => x.Id == Id)).ToList();
            return listofUsers;

        }




        public List<ApplicationUser> GetUsers(string Id, int HowMany = 20)
        {
            ApplicationUser user = context.Users.Where(u => u.Id == Id).First();
            List<ApplicationUser> list = context.Users.Take(HowMany).ToList();

            list = list.Except(list.Where(x => x.Id == user.Id)).ToList();


            return list;
        }

        public bool RemoveMessage(int MessageId)
        {

            //Message messageToRemove;

            try
            {

                Message message = context.Messages.Include("ApplicationUsers").Where(m => m.MessageId == MessageId).First();

                context.Messages.Remove(message);

                context.SaveChanges();



                return true;

            }
            catch (Exception ex)
            {
                return false;
            }




        }
    }














}