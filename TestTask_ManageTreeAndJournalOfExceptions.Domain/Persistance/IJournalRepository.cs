using TestTask_ManageTreeAndJournalOfExceptions.Domain.Entities;
using TestTask_ManageTreeAndJournalOfExceptions.Domain.Models;

namespace TestTask_ManageTreeAndJournalOfExceptions.Domain.Persistance
{
    public interface IJournalRepository
    {
        Task AddAsync(Journal entity);
        Task<Journal> GetAsync(long id);
        Task<Models.Range> GetRangeAsync(int skip, int take, Filter filter);
    }
}
