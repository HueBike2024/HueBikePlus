using System.Collections.Generic;

namespace Core.Interfaces.Office
{
    public interface ISheet
    {
        string SheetName { get; set; }
        byte[] ImageHeader { get; set; }
        IRow Headers { get; set; }
        List<IRow> Rows { get; set; }
    }
}
