using MessengerApplication.WebUI.Abstract;
using MessengerApplication.WebUI.Entities;
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

        
        public ActionResult Autocomplete(string term)
        {
            List<string> FirstName = repository.AutocompleteName(term).Select(x => x.FirstName).ToList();


            return Json(FirstName, JsonRequestBehavior.AllowGet);
        }









        public ActionResult ViewToDelete()
        {
            return View(new List<ApplicationUser>());
        }





        [HttpGet]
        public ActionResult SearchForUser()
        {
            List<ApplicationUser> list = repository.GetUsers(20);

           
            return View(list);
        }



        [HttpPost]
        public ActionResult SearchForUser(int HowMany=20,string FirstName="",string Surname = "", string City = "", int Age =0)
        {

            List<Models.ApplicationUser> UserList;

            UserList = repository.GetUsers(HowMany, FirstName, Surname, City, Age);


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

                List<Message> list = repository.GetMessages(Id, User.Identity.GetUserId());

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

        //public ActionResult Index()
        //{
        //    return View();
        //}

        //public ActionResult About()
        //{
        //    ViewBag.Message = "Your application description page.";

        //    return View();
        //}

        //public ActionResult Contact()
        //{
        //    ViewBag.Message = "Your contact page.";

        //    return View();
        //}
    }
}