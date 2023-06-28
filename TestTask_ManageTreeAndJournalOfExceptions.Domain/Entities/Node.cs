using TestTask_ManageTreeAndJournalOfExceptions.Domain.Models;

namespace TestTask_ManageTreeAndJournalOfExceptions.Domain.Entities
{
    public class Node
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long? ParentId { get; set; }
        public Node Parent { get; set; }
        public string TreeName { get; set; }
        public List<Node> Children { get; set; }

        public NodeModel ToNodeModel()
        {
            var model = new NodeModel { Id = Id, Name = Name };
            foreach (var child in Children ?? Enumerable.Empty<Node>())
            {
                model.Children.Add(child.ToNodeModel());
            }
            return model;
        }
    }
}