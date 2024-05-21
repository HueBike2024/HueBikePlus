using Core.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterData.Application.Commands.NotificationCommand
{
    public class ListNotificationReceivedCommand : PagingQuery
    {
        public long UserId { get; set; }
    }
}
