using Microsoft.AspNetCore.Mvc;
using TestTask_ManageTreeAndJournalOfExceptions.Domain.Persistance;
using TestTask_ManageTreeAndJournalOfExceptions.Domain.Models;

namespace TestTask_ManageTreeAndJournalOfExceptions.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TreesController
    {
        private readonly INodeRepository nodeRepository;
        public TreesController(INodeRepository nodeRepository)
        {
            this.nodeRepository = nodeRepository;
        }

        [HttpPost()]
        public async Task<NodeModel> GetOrCreate(string treeName)
        {
            var tree = await nodeRepository.GetOrCreateTree(treeName);

            return tree.ToNodeModel();
        }
    }
}
