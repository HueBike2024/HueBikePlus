using Core.SeedWork;
using GoogleApi.Entities.Interfaces;
using MasterData.Application.DTOs.Transaction;
using MasterData.Application.Sortings.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterData.Application.Commands.TransactionCommmand
{
    public class ListTransactionCommand : PagingQuery
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public override Dictionary<string, string> GetFieldMapping()
        {
            return TransactionSorting.Mapping;
        }
    }
}
