using GoogleApi.Entities.Interfaces;
using MasterData.Application.DTOs.Transaction;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterData.Application.Commands.TransactionCommmand
{
    public class TransactionDetailCommand : IRequest<ListTransactionUserCommand>
    {
        public long TransactionId { get; set; }
    }
}
