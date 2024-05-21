using Core.Interfaces.Office;

namespace Core.Implements.Offices
{
    public class ExcelCell : ICell
    {
        public ExcelCell()
        {
        }

        public ExcelCell(string cellValue)
        {
            Value = cellValue;
        }

        public string Value { get; set; }
    }
}
