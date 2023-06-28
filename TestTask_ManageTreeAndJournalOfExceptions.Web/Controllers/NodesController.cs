using Microsoft.AspNetCore.Mvc;
using TestTask_ManageTreeAndJournalOfExceptions.Domain.Models;
using TestTask_ManageTreeAndJournalOfExceptions.Domain.Persistance;

namespace TestTask_ManageTreeAndJournalOfExceptions.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NodesController
    {
        private readonly INodeRepository nodeRepository;
        public NodesController(INodeRepository nodeRepository)
        {
            this.nodeRepository = nodeRepository;
        }

        [HttpPost()]
        public async Task<NodeModel> Create(string treeName, long parentNodeId, string nodeName)
        {
            var node = await nodeRepository.Create(treeName, parentNodeId, nodeName);
            return node.ToNodeModel();
        }

        [HttpDelete()]
        public async Task Delete(string treeName, long nodeId)
        {
            await nodeRepository.Delete(treeName, nodeId);
        }

        [HttpPut()]
        public async Task<NodeModel> Rename(string treeName, long nodeId, string newNodeName)
        {
            var node = await nodeRepository.Rename(treeName, nodeId, newNodeName);
            return node.ToNodeModel();
        }
    }
}
