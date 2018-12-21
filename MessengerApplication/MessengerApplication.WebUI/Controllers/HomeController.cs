using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MessengerApplication.WebUI.Controllers
{
    public class HomeController : Controller
    {
       



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
            List<string> ReceiversList = new List<string>();
            

            for (int i = 0; i < 10; i++)
            {
              ReceiversList.Add("Tomasz Krzemiński");
            }


            return PartialView(ReceiversList);
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