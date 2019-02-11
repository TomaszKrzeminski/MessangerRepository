using NUnit.Framework;
using System.Collections.Generic;
using Moq;
using MessengerApplication.WebUI.Abstract;
using MessengerApplication.WebUI.Models;
using Microsoft.AspNet.Identity;
using MessengerApplication.WebUI.Entities;

using MessengerApplication.WebUI.Controllers;
using System.Web.Mvc;
using MessengerApplication.WebUI.Infrastructure;

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


        [Test]
        public void AddPersonToConversation_PersonExists_ReturnsViewAlreadyAdded()
        {

            Mock<IUserStatsRepository> mock = new Mock<IUserStatsRepository>();
            mock.Setup(r => r.CheckIfReceiverIsAdded(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            HomeController controller = new HomeController(mock.Object, () => "Id");

            ViewResult result = controller.AddPersonToConversation("Id") as ViewResult;


            Assert.AreEqual("AlreadyAdded", result.ViewName);

        }



        [Test]
        public void AddPersonToConversation_PersonDoesntExists_RedirectToActionMessenger()
        {

            Mock<IUserStatsRepository> mock = new Mock<IUserStatsRepository>();
            mock.Setup(r => r.CheckIfReceiverIsAdded(It.IsAny<string>(), It.IsAny<string>())).Returns(false);
            mock.Setup(r => r.AddEmptyMessage(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            HomeController controller = new HomeController(mock.Object, () => "Id");

            RedirectToRouteResult result = controller.AddPersonToConversation("Id") as RedirectToRouteResult;


            Assert.AreEqual(result.RouteValues["action"], "Messanger");

        }




        [Test]
        public void AddPersonToConversation_PersonDoesntExistsError_RedirectToViewError()
        {

            Mock<IUserStatsRepository> mock = new Mock<IUserStatsRepository>();
            mock.Setup(r => r.CheckIfReceiverIsAdded(It.IsAny<string>(), It.IsAny<string>())).Returns(false);
            mock.Setup(r => r.AddEmptyMessage(It.IsAny<string>(), It.IsAny<string>())).Returns(false);
            HomeController controller = new HomeController(mock.Object, () => "Id");

            ViewResult result = controller.AddPersonToConversation("Id") as ViewResult;


            Assert.AreEqual(result.ViewName, "AddPersonToConversationError");

        }




        [Test]
        public void GetReceivers_GettingReceivers_ReturnsList()
        {
            List<ReceiverDataViewModel> list = new List<ReceiverDataViewModel>();
            list.Add(new ReceiverDataViewModel() {FullName="Full Name 1",Id="one",IsRead=true });
            list.Add(new ReceiverDataViewModel() { FullName = "Full Name 2", Id = "two", IsRead = true });
            list.Add(new ReceiverDataViewModel() { FullName = "Full Name 3", Id = "three", IsRead = true });
            list.Add(new ReceiverDataViewModel() { FullName = "Full Name 4", Id = "four", IsRead = true });

            Mock<IUserStatsRepository> mock = new Mock<IUserStatsRepository>();
            mock.Setup(r => r.GetReceiverData(It.IsAny<string>())).Returns(list);

            HomeController controller = new HomeController(mock.Object, () => "Id");

            PartialViewResult result = controller.GetReceivers() as PartialViewResult;

            Assert.AreEqual(result.Model, list);


        }

        [Test]
        public void SearchForUser_ReturnsNull_ViewWitchNewList()
        {
            List<ApplicationUser> ApplicationUsers = new List<ApplicationUser>();
            ApplicationUsers.Add(new ApplicationUser() { Id = "2c06feb7-86ef-4c0a-aced-2bad02f4bd22", Email = "ewa2323@gmail.com" });
            ApplicationUsers.Add(new ApplicationUser() { Id = "95ca1c5e-c81b-456e-a7f5-a84293b01007", Email = "janusz2323@gmail.com" });


            Mock<IUserStatsRepository> mock = new Mock<IUserStatsRepository>();
            mock.Setup(r => r.GetUsers(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>())).Returns<List<ApplicationUser>>(null);

            HomeController controller = new HomeController(mock.Object, () => "Id");

            ViewResult result = controller.SearchForUser(20, "", "", "", 0) as ViewResult;

            Assert.AreEqual(result.Model,new List<ApplicationUser>());




        }



        [Test]
        public void SearchForUser_GetUsersIsntNull_ViewWitchList()
        {
            List<ApplicationUser> ApplicationUsers = new List<ApplicationUser>();
            ApplicationUsers.Add(new ApplicationUser() { Id = "2c06feb7-86ef-4c0a-aced-2bad02f4bd22", Email = "ewa2323@gmail.com" });
            ApplicationUsers.Add(new ApplicationUser() { Id = "95ca1c5e-c81b-456e-a7f5-a84293b01007", Email = "janusz2323@gmail.com" });


            Mock<IUserStatsRepository> mock = new Mock<IUserStatsRepository>();
            mock.Setup(r => r.GetUsers(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>())).Returns(ApplicationUsers);

            HomeController controller = new HomeController(mock.Object, () => "Id");

            ViewResult result = controller.SearchForUser(20, "", "", "", 0) as ViewResult;

            Assert.AreEqual(result.Model, ApplicationUsers);




        }


        [Test]
        public void Autocomplete_GettingNames_ReturnsList()
        {


            List<ApplicationUser> list = new List<ApplicationUser>();
            list.Add(new ApplicationUser() { Id = "2c06feb7-86ef-4c0a-aced-2bad02f4bd22", Email = "ewa2323@gmail.com" ,FirstName="Ewa"});
            list.Add(new ApplicationUser() { Id = "95ca1c5e-c81b-456e-a7f5-a84293b01007", Email = "janusz2323@gmail.com",FirstName="Janusz" });



            Mock<IUserStatsRepository> mock = new Mock<IUserStatsRepository>();

            mock.Setup(r => r.AutocompleteName(It.IsAny<string>(), It.IsAny<string>())).Returns(list);

            HomeController controller = new HomeController(mock.Object, () => "Id");

            JsonResult result = controller.Autocomplete("Id") as JsonResult;

            List<string> listNames = new List<string>() { "Ewa", "Janusz" };

            Assert.AreEqual(listNames,result.Data);


        }




        [Test]
        public void UpdateReceivers_ChangingMessagesToSend_RedirectToAction()
        {

            Mock<IUserStatsRepository> mock = new Mock<IUserStatsRepository>();
            mock.Setup(r => r.ChangeMessagesToRead(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            mock.Setup(r => r.GetUserNameById(It.IsAny<string>())).Returns("koral2323@gmail.com");


            HomeController controller = new HomeController(mock.Object,()=>"koral2323@gmail.com");


            RedirectToRouteResult result = controller.UpdateReceivers("koral2323@gmail.com") as RedirectToRouteResult;


            Assert.AreEqual(result.RouteValues["action"], "GetReceivers"); 


        }


        [Test]
        public void SendMessege_MessageDataIsNull_AddsModelError()
        {
            List<Message> list = new List<Message>();
            list.Add(new Message() {MessageData="Data",MessageId=1 });
            list.Add(new Message() { MessageData = "Data", MessageId = 2 });
            list.Add(new Message() { MessageData = "Data", MessageId = 3 });
            list.Add(new Message() { MessageData = "Data", MessageId = 4 });
            list.Add(new Message() { MessageData = "Data", MessageId = 5 });
            list.Add(new Message() { MessageData = "Data", MessageId = 6});

            Mock<IUserStatsRepository> mock = new Mock<IUserStatsRepository>();
            mock.Setup(r => r.GetUserNameById(It.IsAny<string>())).Returns("koral2323@gmail.com");
            mock.Setup(r => r.GetMessages(It.IsAny<string>(), It.IsAny<string>())).Returns(list);

            Message message = new Message() { MessageData = null };

            HomeController controller = new HomeController(mock.Object, () => "koral2323@gmail.com");
            ViewResult result = controller.SendMessage(message) as ViewResult;
            
             Assert.IsTrue(controller.ViewData.ModelState["MessageData"].Errors.Count == 1);
           var errorList = controller.ViewData.ModelState["MessageData"].Errors;
            List<string> listToCheck = new List<string>();
            foreach (var item in errorList)
            {
                listToCheck.Add(item.ErrorMessage);
            }

           

            Assert.Contains( "Message should not be empty",listToCheck);

        }



        [Test]
        public void SendMessege_MessageDataIsntNull_ReturnsList()
        {

           Message message = new Message() { MessageData = "Message Text",MessageId=1 ,ReceiverId="ReceiverId"};


            Mock<IReceiverHub> mockHub = new Mock<IReceiverHub>();
            mockHub.Setup(m => m.RefreshReceivers(It.IsAny<string>(), It.IsAny<string>()));
 


            List<Message> list = new List<Message>();
            list.Add(new Message() { MessageData = "Data", MessageId = 1 });
            list.Add(new Message() { MessageData = "Data", MessageId = 2 });
            list.Add(new Message() { MessageData = "Data", MessageId = 3 });
            list.Add(new Message() { MessageData = "Data", MessageId = 4 });
            list.Add(new Message() { MessageData = "Data", MessageId = 5 });
            list.Add(new Message() { MessageData = "Data", MessageId = 6 });

            Mock<IUserStatsRepository> mock = new Mock<IUserStatsRepository>();
            mock.Setup(r => r.GetUserNameById(It.IsAny<string>())).Returns("koral2323@gmail.com");
            mock.Setup(r => r.GetMessages(It.IsAny<string>(), It.IsAny<string>())).Returns(list);
            mock.Setup(r => r.AddMessage(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Message>())).Returns(true);
            mock.Setup(r => r.GetUserNameForSignalR(It.IsAny<string>())).Returns("User");
           

            HomeController controller = new HomeController(mockHub.Object,mock.Object, () => "koral2323@gmail.com");
            PartialViewResult result = controller.SendMessage(message) as PartialViewResult;

            Assert.AreEqual("GetMessages",result.ViewName);

        }



        [Test]
        public void SendMessege_MessageDataIsntNull_ReturnsError()
        {



            Message message = new Message() { MessageData = "Message Text", MessageId = 1, ReceiverId = "ReceiverId" };


            Mock<IReceiverHub> mockHub = new Mock<IReceiverHub>();
            mockHub.Setup(m => m.RefreshReceivers(It.IsAny<string>(), It.IsAny<string>()));



            List<Message> list = new List<Message>();
            list.Add(new Message() { MessageData = "Data", MessageId = 1 });
            list.Add(new Message() { MessageData = "Data", MessageId = 2 });
            list.Add(new Message() { MessageData = "Data", MessageId = 3 });
            list.Add(new Message() { MessageData = "Data", MessageId = 4 });
            list.Add(new Message() { MessageData = "Data", MessageId = 5 });
            list.Add(new Message() { MessageData = "Data", MessageId = 6 });

            Mock<IUserStatsRepository> mock = new Mock<IUserStatsRepository>();
            mock.Setup(r => r.GetUserNameById(It.IsAny<string>())).Returns("koral2323@gmail.com");
            mock.Setup(r => r.GetMessages(It.IsAny<string>(), It.IsAny<string>())).Returns(list);
            mock.Setup(r => r.AddMessage(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Message>())).Returns(false);
            mock.Setup(r => r.GetUserNameForSignalR(It.IsAny<string>())).Returns("User");


            HomeController controller = new HomeController(mockHub.Object, mock.Object, () => "koral2323@gmail.com");
            ViewResult result = controller.SendMessage(message) as ViewResult;

            Assert.AreEqual("Error", result.ViewName);




        }


    }
}
