using Microsoft.AspNetCore.Mvc;
using TestTask_ManageTreeAndJournalOfExceptions.Domain.Entities;
using TestTask_ManageTreeAndJournalOfExceptions.Domain.Models;
using TestTask_ManageTreeAndJournalOfExceptions.Domain.Persistance;

namespace TestTask_ManageTreeAndJournalOfExceptions.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class JournalController : ControllerBase
    {
        private readonly IJournalRepository journalRepository;

        public JournalController(IJournalRepository journalRepository)
        {
            this.journalRepository = journalRepository;
        }

        [HttpGet("getSingle")]
        public async Task<Journal> GetSingle(int id)
        {
            return await journalRepository.GetAsync(id);
        }

        [HttpPost("getRange")]
        public async Task<Domain.Models.Range> GetRange(int skip, int take, [FromBody] Filter filter)
        {
            return await journalRepository.GetRangeAsync(skip, take, filter);
        }
    }
}