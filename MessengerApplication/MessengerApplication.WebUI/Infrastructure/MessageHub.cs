using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MessengerApplication.WebUI.Infrastructure
{
    //public class MessageHub:Hub
    //{

    //    public static void NotifyClient(string userName)
    //    {

    //        IHubContext context = GlobalHost.ConnectionManager.GetHubContext<MessageHub>();

    //        //context.Clients.All.notify();

    //        context.Clients.User(userName).notify();


    //    }


    //  }




    public interface IReceiverHub
    {

         void  RefreshReceivers(string userName, string SenderId);

    }


    public interface IMessagesHub
    {

        void UpdateMessagesNumber(string userName, int Number);

    }


    public class MessagesHub : Hub, IMessagesHub
    {
        public void UpdateMessagesNumber(string userName, int Number)
        {

            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<MessagesHub>();



            context.Clients.User(userName).refreshMessages(Number);

        }
    }












    public class ReceiverHub : Hub,IReceiverHub
    {

       



        public  void RefreshReceivers(string userName, string SenderId)
        {

            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<ReceiverHub>();

           

            context.Clients.User(userName).refreshPage(SenderId);


        }


    }
}