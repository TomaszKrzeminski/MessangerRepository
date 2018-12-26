using MessengerApplication.WebUI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MessengerApplication.WebUI.Entities
{
    public class Message
    {
        public int MessageId { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime SendTime { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime ReceiveTime { get; set; }
        public bool IsRead { get; set; }
        public string SenderName { get; set; }
        public string ReceiverName { get; set; }
        public string MessageData { get; set; }

                public virtual ICollection<ApplicationUser> ApplicationUsers { get; set; }
    }
}