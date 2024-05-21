using System.Collections.Generic;

namespace Core.Interfaces.Office
{
    public interface IDocument
    {
        List<ISheet> Sheets { get; set; }
    }
}
