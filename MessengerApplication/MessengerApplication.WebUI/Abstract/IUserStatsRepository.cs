using MessengerApplication.WebUI.Entities;
using MessengerApplication.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessengerApplication.WebUI.Abstract
{
    public interface IUserStatsRepository
    {

        string GetUserNameForSignalR(string Id);

        string GetUserNameById(string Id);
        List<Models.ApplicationUser> GetUsers(string Id,int HowMany = 10, string FirstName = "", string Surname = "", string City = "", int Age = 0);

        List<Models.ApplicationUser> GetUsers(string Id,int HowMany=20);

        List<Models.ApplicationUser> AutocompleteName(string Id,string name);

        List<ReceiverDataViewModel> GetReceiverData(string UserId);

        bool AddEmptyMessage(string Sender, string Receiver);

        List<Message> GetMessages(string SenderId,string UserId);

        bool AddMessage(string ReceiverId, string SenderId, Message message);

        bool ChangeMessagesToRead(string UserId,string SenderId);

        bool CheckIfReceiverIsAdded(string NewPerson,string UserId);

        bool RemoveMessage(int MessageId);

        int CheckUnreadedMessages(string UserId);

    }
}
