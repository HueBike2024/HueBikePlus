using Core.Interfaces.Office;
using System.Collections.Generic;

namespace Core.Implements.Offices
{
    public class ExcelRow : IRow
    {
        public ExcelRow()
        {
            Cells = new List<ICell>();
        }
        public List<ICell> Cells { get; set; }
    }
}
