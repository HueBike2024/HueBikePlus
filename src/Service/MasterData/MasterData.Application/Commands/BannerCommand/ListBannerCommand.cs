using Core.SeedWork;
using MasterData.Application.Sortings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterData.Application.Commands.BannerCommand
{
    public class ListBannerCommand : PagingQuery
    {
        public string Type { get; set; }
        public override Dictionary<string, string> GetFieldMapping()
        {
            return BannerSorting.Mapping;
        }
    }
}
