using Core.Interfaces.Office;
using System.Collections.Generic;

namespace Core.Implements.Offices
{
    public class ExcelSheet : ISheet
    {
        public ExcelSheet()
        {
            Headers = new ExcelRow();
            Rows = new List<IRow>();
        }

        public string SheetName { get; set; }
        public byte[] ImageHeader { get; set; }
        public IRow Headers { get; set; }
        public List<IRow> Rows { get; set; }
    }
}
