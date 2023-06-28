using TestTask_ManageTreeAndJournalOfExceptions.Data.EFDatabaseContext;
using TestTask_ManageTreeAndJournalOfExceptions.Domain.Entities;
using TestTask_ManageTreeAndJournalOfExceptions.Domain.Exceptions;
using TestTask_ManageTreeAndJournalOfExceptions.Domain.Persistance;

namespace TestTask_ManageTreeAndJournalOfExceptions.Data.Repositories
{
    public class NodeRepository : INodeRepository
    {
        private readonly ApplicationContext context;

        public NodeRepository(ApplicationContext context)
        {
            this.context = context;
        }

        public async Task<Node> GetOrCreateTree(string treeName)
        {
            var nodes = context.Nodes.Where(t => t.TreeName == treeName).ToList();

            if (!nodes.Any())
            {
                var rootNode = new Node { Name = treeName, TreeName = treeName };
                await context.Nodes.AddAsync(rootNode);
                await context.SaveChangesAsync();
                return rootNode;
            }

            return GetTreeFromList(nodes, parentNodeId: nodes.FirstOrDefault(n => n.ParentId == null).Id);
        }

        public async Task<Node> Create(string treeName, long parentNodeId, string nodeName)
        {
            var nodes = context.Nodes.Where(n => n.TreeName == treeName).ToList();

            Validate(nodes, treeName, parentNodeId, nodeName);

            var node = new Node { Name = nodeName, ParentId = parentNodeId, TreeName = treeName };

            await context.Nodes.AddAsync(node);
            await context.SaveChangesAsync();

            return node;
        }

        public async Task Delete(string treeName, long nodeId)
        {
            var nodes = context.Nodes.Where(n => n.TreeName == treeName).ToList();
            var node = nodes.FirstOrDefault(n => n.TreeName == treeName && n.Id == nodeId);

            if (node == null)
            {
                throw new SecureException($"A node with id {nodeId} is not found in tree {treeName}.");
            }
            if (GetTreeFromList(nodes, parentNodeId: node.Id).Children.Any())
            {
                throw new SecureException("You have to delete all children nodes first.");
            }

            context.Nodes.Remove(node);
            await context.SaveChangesAsync();
        }

        public async Task<Node> Rename(string treeName, long nodeId, string newName)
        {
            var nodes = context.Nodes.Where(n => n.TreeName == treeName).ToList();

            Validate(nodes, treeName, nodeId, newName);

            var node = nodes.FirstOrDefault(n => n.Id == nodeId);
            node.Name = newName;

            await context.SaveChangesAsync();

            return GetTreeFromList(nodes, parentNodeId: node.Id);
        }

        private void Validate(List<Node> nodes, string treeName, long nodeId, string nodeName)
        {
            if (!nodes.Any())
            {
                throw new SecureException($"Tree with name {treeName} is not found.");
            }
            if (!nodes.Any(n => n.Id == nodeId))
            {
                throw new SecureException($"Node with id {nodeId} is not found.");
            }
            if (nodes.Any(n => n.Name == nodeName))
            {
                throw new SecureException($"A node with name {nodeName} already exists.");
            }
        }

        private Node GetTreeFromList(List<Node> nodes, long? parentNodeId)
        {
            var children = nodes.ToLookup(n => n.ParentId);
            foreach (var child in nodes)
            {
                child.Children = children[child.Id].ToList();
            }
            return nodes.FirstOrDefault(n => n.Id == parentNodeId);
        }
    }
}
