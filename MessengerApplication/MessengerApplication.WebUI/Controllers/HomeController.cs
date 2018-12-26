using MessengerApplication.WebUI.Abstract;
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







        public ActionResult GetMessages()
        {
            List<string> MessageList = new List<string>();


            for (int i = 0; i < 10; i++)
            {
                MessageList.Add("Lorem Ipsum jest tekstem stosowanym jako przykładowy wypełniacz w przemyśle poligraficznym. Został po raz pierwszy użyty w XV w. przez nieznanego drukarza do wypełnienia tekstem próbnej książki. Pięć wieków później zaczął być używany przemyśle elektronicznym, pozostając praktycznie niezmienionym. Spopularyzował się w latach 60. XX w. wraz z publikacją arkuszy Letrasetu, zawierających fragmenty Lorem Ipsum, a ostatnio z zawierającym różne wersje Lorem Ipsum oprogramowaniem przeznaczonym do realizacji druków na komputerach osobistych, jak Aldus PageMaker");
            }


            return PartialView(MessageList);
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