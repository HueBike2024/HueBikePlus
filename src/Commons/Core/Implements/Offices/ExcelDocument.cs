using Core.Interfaces.Office;
using System.Collections.Generic;

namespace Core.Implements.Offices
{
    public class ExcelDocument : IDocument
    {
        public ExcelDocument()
        {
            Sheets = new List<ISheet>();
        }
        public List<ISheet> Sheets { get; set; }
    }
}
