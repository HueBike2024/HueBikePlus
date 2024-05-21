namespace Infrastructure.AggregatesModel.Authen.FunctionAggregate
{
    public class Module
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? DisplayOrder { get; set; }
        public bool IsDisplay { get; set; }
        public virtual ICollection<Function> Functions { get; set; }
    }
}