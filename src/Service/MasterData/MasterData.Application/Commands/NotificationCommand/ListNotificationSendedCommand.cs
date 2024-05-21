using Core.SeedWork;
using MasterData.Application.Sortings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterData.Application.Commands.NotificationCommand
{
    public class ListNotificationSendedCommand : PagingQuery
    {
        public override Dictionary<string, string> GetFieldMapping()
        {
            return NotificationSenderSorting.Mapping;
        }
    }
}
