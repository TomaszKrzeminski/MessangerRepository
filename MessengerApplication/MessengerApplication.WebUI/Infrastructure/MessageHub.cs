using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MessengerApplication.WebUI.Infrastructure
{
    public class MessageHub:Hub
    {

        public static void NotifyClient(string userName)
        {

            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<MessageHub>();

            //context.Clients.All.notify();

            context.Clients.User(userName).notify();


        }


      }




    public interface IReceiverHub
    {

         void  RefreshReceivers(string userName, string SenderId);

    }



    public class ReceiverHub : Hub,IReceiverHub
    {

        //public static void RefreshReceivers(string userName,string SenderId)
        //{

        //    IHubContext context = GlobalHost.ConnectionManager.GetHubContext<ReceiverHub>();

        //    //context.Clients.All.notify();

        //    context.Clients.User(userName).refreshPage(SenderId);


        //}



        public  void RefreshReceivers(string userName, string SenderId)
        {

            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<ReceiverHub>();

            //context.Clients.All.notify();

            context.Clients.User(userName).refreshPage(SenderId);


        }


    }
}