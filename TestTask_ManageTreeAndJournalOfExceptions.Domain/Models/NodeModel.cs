namespace TestTask_ManageTreeAndJournalOfExceptions.Domain.Models
{
    public class NodeModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public List<NodeModel> Children { get; set; } = new List<NodeModel> { };
    }
}
