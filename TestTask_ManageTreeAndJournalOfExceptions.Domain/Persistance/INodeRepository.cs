using TestTask_ManageTreeAndJournalOfExceptions.Domain.Entities;

namespace TestTask_ManageTreeAndJournalOfExceptions.Domain.Persistance
{
    public interface INodeRepository
    {
        Task<Node> GetOrCreateTree(string treeName);
        Task<Node> Create(string treeName, long parentNodeId, string nodeName);
        Task Delete(string treeName, long nodeId);
        Task<Node> Rename(string treeName, long nodeId, string newName);
    }
}
