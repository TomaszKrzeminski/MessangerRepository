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
        public HomeController(IUserStatsRepository userRepository)
        {
            repository = userRepository;
            GetUserId = () => User.Identity.GetUserId();
        }

        public HomeController(IUserStatsRepository userRepository,Func<string>GetUserId)
        {
            repository = userRepository;
            this.GetUserId = GetUserId;
        }


        public ActionResult SendMessage(Message message)
        {

            if (message.MessageData == null)
            {
                ModelState.AddModelError("MessageData", "Message should not be empty");
                ViewBag.ReceiverName = repository.GetUserNameById(message.ReceiverId);
                ViewBag.ReceiverId = message.ReceiverId;
                List<Message> list = repository.GetMessages(message.ReceiverId, User.Identity.GetUserId());
                return PartialView("GetMessages", list);

            }
            else
            {

                if (repository.AddMessage(message.ReceiverId, User.Identity.GetUserId(), message))
                {
                    // SignalR


                    MessageHub.NotifyClient(repository.GetUserNameForSignalR(message.ReceiverId));

                    ReceiverHub.RefreshReceivers(repository.GetUserNameForSignalR(message.ReceiverId), User.Identity.GetUserId());


                    ////


                    ViewBag.ReceiverName = repository.GetUserNameById(message.ReceiverId);
                    ViewBag.ReceiverId = message.ReceiverId;

                    List<Message> list = repository.GetMessages(message.ReceiverId, User.Identity.GetUserId());

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

            repository.ChangeMessagesToRead(User.Identity.GetUserId(), ReceiverId);




            ViewBag.ReceiverName = repository.GetUserNameById(ReceiverId);
            ViewBag.ReceiverId = ReceiverId;




            return RedirectToAction("GetReceivers");




        }




        public ActionResult Autocomplete(string term)
        {
            string Id = User.Identity.GetUserId();
            List<string> FirstName = repository.AutocompleteName(Id, term).Select(x => x.FirstName).ToList();


            return Json(FirstName, JsonRequestBehavior.AllowGet);
        }



      




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

            UserList = repository.GetUsers(User.Identity.GetUserId(), HowMany, FirstName, Surname, City, Age);


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

                if (list == null)
                {
                    return PartialView(new List<Message>());
                }
                else
                {
                    return PartialView(list);

                }




            }





        }




        public ActionResult GetReceivers()
        {


            List<ReceiverDataViewModel> list = repository.GetReceiverData(User.Identity.GetUserId());




            return PartialView(list);
        }



        public ActionResult AddPersonToConversation(string Id)
        {


            bool AlreadyAdded = repository.CheckIfReceiverIsAdded(Id, User.Identity.GetUserId());


            if (AlreadyAdded)
            {
                return View("AlreadyAdded");
            }
            else
            {
                bool succes = repository.AddEmptyMessage(Id, User.Identity.GetUserId());

                return RedirectToAction("Messanger");
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

        public ActionResult Settings()
        {
            return View();
        }

        public ActionResult NewMessage()
        {
            return View();
        }


        public ActionResult ReciverStats(int id = 1)
        {
            return PartialView();
        }



        public ActionResult Test()
        {

            return View();
        }







    }
}