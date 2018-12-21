using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MessengerApplication.WebUI.Models
{
    public class MessengerHub:Hub
    {


 [HubMethodName("NotifyClients")]
        public static void NotifyCurrentEmployeeInformationToAllClients()
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<MessengerHub>();

            // the update client method will update the connected client about any recent changes in the server data
            context.Clients.All.updatedClients();
        }


    }



   

}