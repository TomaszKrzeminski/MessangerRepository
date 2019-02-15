using MessengerApplication.WebUI.Abstract;
using MessengerApplication.WebUI.Entities;
using MessengerApplication.WebUI.Infrastructure;
using MessengerApplication.WebUI.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MessengerApplication.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private IUserStatsRepository repository;
        private Func<string> GetUserId;

        private IReceiverHub hubReceiver;
        private IMessagesHub messageHub;

        public HomeController(IUserStatsRepository userRepository)
        {
            repository = userRepository;
            GetUserId = () => User.Identity.GetUserId();
            hubReceiver = new ReceiverHub();
            messageHub = new MessagesHub();
        }

        public HomeController(IUserStatsRepository userRepository, Func<string> GetUserId)
        {
            repository = userRepository;
            this.GetUserId = GetUserId;
            hubReceiver = new ReceiverHub();
            messageHub = new MessagesHub();

        }


        public HomeController(IReceiverHub receiverHub, IUserStatsRepository userRepository, Func<string> GetUserId)
        {
            repository = userRepository;
            this.GetUserId = GetUserId;
            hubReceiver = receiverHub;
        }



        public HomeController(IMessagesHub messagesHub,IReceiverHub receiverHub, IUserStatsRepository userRepository, Func<string> GetUserId)
        {
            repository = userRepository;
            this.GetUserId = GetUserId;
            hubReceiver = receiverHub;
            messageHub = messagesHub;
        }


        public ActionResult DeleteMessage(int MessageId, string SenderId, string ReceiverId)
        {

            if (repository.RemoveMessage(MessageId))
            {

                string userId = GetUserId();
                string Receiver;

                if (SenderId == userId)
                {

                    Receiver = ReceiverId;

                }
                else
                {
                    Receiver = SenderId;

                }



                ViewBag.ReceiverName = repository.GetUserNameById(Receiver);
                ViewBag.ReceiverId = Receiver;

                List<Message> list = repository.GetMessages(Receiver, GetUserId());
                //repository.ChangeMessagesToRead(GetUserId(), Receiver);

                if (list == null)
                {
                    return PartialView("GetMessages", new List<Message>());
                }
                else
                {
                    return PartialView("GetMessages", list);

                }








            }
            else
            {

                return PartialView("RemoveError");
            }





        }



        public ActionResult CheckNumberOfNewMessages()
        {
            //int number = 0;
            
          int  number = repository.CheckUnreadedMessages(GetUserId());
            //userName = repository.GetUserNameById(GetUserId());

            //messageHub.UpdateMessagesNumber(userName, number);


            return PartialView(number);

        }





        public ActionResult SendMessage(Message message)
        {

            if (message.MessageData == null)
            {
                ModelState.AddModelError("MessageData", "Message should not be empty");
                ViewBag.ReceiverName = repository.GetUserNameById(message.ReceiverId);
                ViewBag.ReceiverId = message.ReceiverId;
                List<Message> list = repository.GetMessages(message.ReceiverId, GetUserId());
                return PartialView("GetMessages", list);

            }
            else
            {

                if (repository.AddMessage(message.ReceiverId, GetUserId(), message))
                {
                    // SignalR


                    //MessageHub.NotifyClient(repository.GetUserNameForSignalR(message.ReceiverId));

                    hubReceiver.RefreshReceivers(repository.GetUserNameForSignalR(message.ReceiverId), GetUserId());

                    ////

                    //MessagesHub //
                    int number = repository.CheckUnreadedMessages(GetUserId());
                    string userName = repository.GetUserNameForSignalR(GetUserId());
                    messageHub.UpdateMessagesNumber(userName, number);


                    int number2 = repository.CheckUnreadedMessages(message.ReceiverId);
                    string userName2 = repository.GetUserNameForSignalR(message.ReceiverId);
                    messageHub.UpdateMessagesNumber(userName2, number2);



                    ////


                    ViewBag.ReceiverName = repository.GetUserNameById(message.ReceiverId);
                    ViewBag.ReceiverId = message.ReceiverId;

                    List<Message> list = repository.GetMessages(message.ReceiverId, GetUserId());

                    if (list == null)
                    {
                        return PartialView("GetMessages", new List<Message>());
                    }
                    else
                    {
                        return PartialView("GetMessages", list);

                    }
                }
                else
                {
                    return View("Error");
                }




            }





        }



        public ActionResult UpdateReceivers(string ReceiverId)
        {

            repository.ChangeMessagesToRead(GetUserId(), ReceiverId);

            ///MessagesHub
            int number = repository.CheckUnreadedMessages(GetUserId());
            string userName = repository.GetUserNameForSignalR(GetUserId());
            messageHub.UpdateMessagesNumber(userName, number);


            int number2 = repository.CheckUnreadedMessages(ReceiverId);
            string userName2 = repository.GetUserNameForSignalR(ReceiverId);
            messageHub.UpdateMessagesNumber(userName2, number2);

            /////


            ViewBag.ReceiverName = repository.GetUserNameById(ReceiverId);
            ViewBag.ReceiverId = ReceiverId;




            return RedirectToAction("GetReceivers");




        }



        public ActionResult Autocomplete(string term)
        {
            string Id = GetUserId();
            List<string> FirstName = repository.AutocompleteName(Id, term).Select(x => x.FirstName).ToList();


            return Json(FirstName, JsonRequestBehavior.AllowGet);
        }







        //Not Tested
        public ActionResult ViewToDelete()
        {
            return View(new List<ApplicationUser>());
        }





        [HttpGet]
        public ActionResult SearchForUser()
        {
            List<ApplicationUser> list = repository.GetUsers(GetUserId(), 20);


            return View(list);
        }



        [HttpPost]
        public ActionResult SearchForUser(int HowMany = 20, string FirstName = "", string Surname = "", string City = "", int Age = 0)
        {

            List<Models.ApplicationUser> UserList;

            UserList = repository.GetUsers(GetUserId(), HowMany, FirstName, Surname, City, Age);


            if (UserList == null)
            {
                return View(new List<Models.ApplicationUser>());
            }
            else
            {
                return View(UserList);
            }


        }







        public ActionResult GetMessages(string Id = "None")
        {


            if (Id == "None")
            {
                return PartialView(new List<Message>());

            }
            else
            {

                ViewBag.ReceiverName = repository.GetUserNameById(Id);
                ViewBag.ReceiverId = Id;

                List<Message> list = repository.GetMessages(Id, GetUserId());
                repository.ChangeMessagesToRead(GetUserId(), Id);



                ///MessagesHub
                 int number = repository.CheckUnreadedMessages(GetUserId());
                string userName = repository.GetUserNameForSignalR(GetUserId());
                messageHub.UpdateMessagesNumber(userName, number);

                int number2 = repository.CheckUnreadedMessages(Id);
                string userName2 = repository.GetUserNameForSignalR(Id);
                messageHub.UpdateMessagesNumber(userName2, number2);

                /////



                if (list == null)
                {
                    return PartialView("GetMessages", new List<Message>());
                }
                else
                {
                    return PartialView("GetMessages", list);

                }




            }





        }




        public ActionResult GetReceivers()
        {




            List<ReceiverDataViewModel> list = repository.GetReceiverData(GetUserId());




            return PartialView(list);
        }



        public ActionResult AddPersonToConversation(string Id)
        {


            bool AlreadyAdded = repository.CheckIfReceiverIsAdded(Id, GetUserId());


            if (AlreadyAdded)
            {
                return View("AlreadyAdded");
            }
            else
            {
                bool succes = repository.AddEmptyMessage(Id, GetUserId());
                if (succes)
                {
                    return RedirectToAction("Messanger");
                }
                else
                {
                    return View("AddPersonToConversationError");
                }

            }



        }



        public ActionResult Messanger()
        {
            return View();
        }

        public ActionResult SearchForUsers()
        {
            return View();
        }

        //public ActionResult Settings()
        //{
        //    return View();
        //}

        //public ActionResult NewMessage()
        //{
        //    return View();
        //}


        //public ActionResult ReciverStats(int id = 1)
        //{
        //    return PartialView();
        //}



        //public ActionResult Test()
        //{

        //    return View();
        //}







    }
}