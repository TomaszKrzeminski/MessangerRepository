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

        public HomeController(IUserStatsRepository userRepository)
        {
            repository = userRepository;
        }



        //Old version

        //public ActionResult SendMessage(Message message)
        //{

        //   if(message.MessageData.Count()>0)
        //    {

        //      if(repository.AddMessage(message.ReceiverId, User.Identity.GetUserId(),message))
        //        {
        //            return RedirectToAction("Messanger");
        //        }
        //        else
        //        {
        //            return View("Error");
        //        }


              

        //    }
        //   else
        //    {
        //        return View("Error");
        //    }



            
        //}

            // changed return View
        public ActionResult SendMessage(Message message)
        {

            if (message.MessageData.Count() > 0)
            {

                if (repository.AddMessage(message.ReceiverId, User.Identity.GetUserId(), message))
                {
                    // SignalR


                    MessageHub.NotifyClient(repository.GetUserNameForSignalR(message.ReceiverId));

                    ReceiverHub.RefreshReceivers(repository.GetUserNameForSignalR(message.ReceiverId),User.Identity.GetUserId());


                    ////


                    ViewBag.ReceiverName = repository.GetUserNameById(message.ReceiverId);
                    ViewBag.ReceiverId = message.ReceiverId;

                    List<Message> list = repository.GetMessages(message.ReceiverId, User.Identity.GetUserId());

                    if (list == null)
                    {
                        return PartialView("GetMessages",new List<Message>());
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
            else
            {
                return View("Error");
            }




        }



        public ActionResult UpdateReceivers(string ReceiverId)
        {

            repository.ChangeMessagesToRead(User.Identity.GetUserId(), ReceiverId);

            //MessageHub.NotifyClient(repository.GetUserNameForSignalR(ReceiverId));

            //        ReceiverHub.RefreshReceivers(repository.GetUserNameForSignalR(ReceiverId), User.Identity.GetUserId());


            //        ////


                    ViewBag.ReceiverName = repository.GetUserNameById(ReceiverId);
                    ViewBag.ReceiverId = ReceiverId;

                    //List<Message> list = repository.GetMessages(ReceiverId, User.Identity.GetUserId());

                    //if (list == null)
                    //{
                    //    return PartialView("GetMessages", new List<Message>());
                    //}
                    //else
                    //{
                    //    return PartialView("GetMessages", list);

                    //}
               

       
          return  RedirectToAction("GetReceivers");




        }




        public ActionResult Autocomplete(string term)
        {
            string Id = User.Identity.GetUserId();
            List<string> FirstName = repository.AutocompleteName(Id,term).Select(x => x.FirstName).ToList();


            return Json(FirstName, JsonRequestBehavior.AllowGet);
        }









        public ActionResult ViewToDelete()
        {
            return View(new List<ApplicationUser>());
        }





        [HttpGet]
        public ActionResult SearchForUser()
        {
            List<ApplicationUser> list = repository.GetUsers(User.Identity.GetUserId(),20);

           
            return View(list);
        }



        [HttpPost]
        public ActionResult SearchForUser(int HowMany=20,string FirstName="",string Surname = "", string City = "", int Age =0)
        {

            List<Models.ApplicationUser> UserList;

            UserList = repository.GetUsers(User.Identity.GetUserId(), HowMany, FirstName, Surname, City, Age);


            if(UserList==null)
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

                List<Message> list = repository.GetMessages(Id, User.Identity.GetUserId());
                repository.ChangeMessagesToRead(User.Identity.GetUserId(),Id);

                if(list==null)
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

        bool succes=  repository.AddEmptyMessage( Id, User.Identity.GetUserId());
                
            return RedirectToAction("Messanger"); 
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
        

        public ActionResult ReciverStats(int id=1)
        {
            return PartialView();
        }

        

        public ActionResult Test()
        {

            return View();
        }







    }
}