using Core.SeedWork;
using MasterData.Application.Sortings.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterData.Application.Commands.TransactionCommmand
{
    public class ListTransactionUserCommand : PagingQuery
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public override Dictionary<string, string> GetFieldMapping()
        {
            return UserTransactionSorting.Mapping;
        }
    }
}
