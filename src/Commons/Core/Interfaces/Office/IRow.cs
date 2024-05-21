using System.Collections.Generic;

namespace Core.Interfaces.Office
{
    public interface IRow
    {
        List<ICell> Cells { get; set; }
    }
}
