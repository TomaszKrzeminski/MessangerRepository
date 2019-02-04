using NUnit.Framework;
using System.Collections.Generic;
using Moq;
using MessengerApplication.WebUI.Abstract;
using MessengerApplication.WebUI.Models;
using Microsoft.AspNet.Identity;
using MessengerApplication.WebUI.Entities;

using MessengerApplication.WebUI.Controllers;
using System.Web.Mvc;

namespace MessengerApplication.Tests.Unit
{
   

    public class MessengerTests
    { 
      
       
   



        [Test]
        public void SearchForUser_ReturningUsers_ReturnUsers()
        {

        List<ApplicationUser> ApplicationUsers = new List<ApplicationUser>();
        ApplicationUsers.Add(new ApplicationUser() {Id= "2c06feb7-86ef-4c0a-aced-2bad02f4bd22", Email="ewa2323@gmail.com" });
        ApplicationUsers.Add(new ApplicationUser() { Id = "95ca1c5e-c81b-456e-a7f5-a84293b01007", Email = "janusz2323@gmail.com" });
        //ApplicationUsers.Add(new ApplicationUser() { Id = "cba4d57a-198f-443e-8cac-af6c871736de", Email = "koral2323@gmail.com" });

        


            Mock<IUserStatsRepository> repository = new Mock<IUserStatsRepository>();
            repository.Setup(r => r.GetUsers("cba4d57a-198f-443e-8cac-af6c871736de", 20)).Returns(ApplicationUsers);


            HomeController controller = new HomeController(repository.Object,()=> "cba4d57a-198f-443e-8cac-af6c871736de");

            ViewResult result = controller.SearchForUser() as ViewResult;
            List<ApplicationUser> list = (List<ApplicationUser>)result.Model;

            Assert.AreEqual(list, ApplicationUsers);
        



        }



        [Test]
        public void GetMesseges_IdIsNone_ReturnsEmptyList()
        {

            Mock<IUserStatsRepository> mock = new Mock<IUserStatsRepository>();


            HomeController controller = new HomeController(mock.Object);


            PartialViewResult result = controller.GetMessages("None") as PartialViewResult;


            Assert.AreEqual(new List<Message>(), result.Model);




        }


        [Test]
        public void GetMesseges_IdIsNotNone_ReturnsList()
        {

            List<Message> messages = new List<Message>();
            messages.Add(new Message {MessageData="One",IsRead=true,MessageId=1 });
            messages.Add(new Message { MessageData = "Two", IsRead = true, MessageId = 2});
            messages.Add(new Message { MessageData = "Three", IsRead = true, MessageId = 3 });
            messages.Add(new Message { MessageData = "Four", IsRead = true, MessageId = 4 });
            messages.Add(new Message { MessageData = "Five", IsRead = true, MessageId = 5 });

            Mock<IUserStatsRepository> mock = new Mock<IUserStatsRepository>();
            mock.Setup(r => r.GetUserNameById("cba4d57a - 198f - 443e-8cac - af6c871736de")).Returns("koral2323@gmail.com");
            mock.Setup(r => r.GetMessages(It.IsAny<string>(), It.IsAny<string>())).Returns(messages);
            mock.Setup(r => r.ChangeMessagesToRead(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            HomeController controller = new HomeController(mock.Object,()=> "cba4d57a-198f-443e-8cac-af6c871736de");
          


            PartialViewResult result = controller.GetMessages("cba4d57a-198f-443e-8cac-af6c871736de") as PartialViewResult;


            Assert.AreEqual(messages, result.Model);




        }



        [Test]
        public void GetMesseges_ListIsNull_ReturnsEmptyList()
        {

            List<Message> messages = new List<Message>();
            
            Mock<IUserStatsRepository> mock = new Mock<IUserStatsRepository>();
            mock.Setup(r => r.GetUserNameById("cba4d57a - 198f - 443e-8cac - af6c871736de")).Returns("koral2323@gmail.com");
            mock.Setup(r => r.GetMessages(It.IsAny<string>(), It.IsAny<string>())).Returns<List<Message>>(null);
            mock.Setup(r => r.ChangeMessagesToRead(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            HomeController controller = new HomeController(mock.Object, () => "cba4d57a-198f-443e-8cac-af6c871736de");



            PartialViewResult result = controller.GetMessages("cba4d57a-198f-443e-8cac-af6c871736de") as PartialViewResult;


            Assert.AreEqual(new List<Message>(), result.Model);




        }














    }
}
