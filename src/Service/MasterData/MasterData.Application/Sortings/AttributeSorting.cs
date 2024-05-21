namespace MasterData.Application.Sortings
{
    public class AttributeSorting
    {
        public static Dictionary<string, string> Mapping = new Dictionary<string, string>
        {
            { "code", "Code" },
            { "name", "Name"},
            { "level", "Level"},
            { "displayorder", "DisplayOrder"},
            { "parentattributeId", "ParentAttributeId"},
            { "isdisplay", "IsDisplay"},
            { "isrequired", "IsRequired"},
            { "iscommon", "IsCommon"},
        };
    }
}
