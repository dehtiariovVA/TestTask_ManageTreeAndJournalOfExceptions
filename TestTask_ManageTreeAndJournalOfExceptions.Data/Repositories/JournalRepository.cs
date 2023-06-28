using Microsoft.EntityFrameworkCore;
using TestTask_ManageTreeAndJournalOfExceptions.Data.EFDatabaseContext;
using TestTask_ManageTreeAndJournalOfExceptions.Domain.Entities;
using TestTask_ManageTreeAndJournalOfExceptions.Domain.Models;
using TestTask_ManageTreeAndJournalOfExceptions.Domain.Persistance;

namespace TestTask_ManageTreeAndJournalOfExceptions.Data.Repositories
{
    public class JournalRepository : IJournalRepository
    {
        private readonly ApplicationContext context;

        public JournalRepository(ApplicationContext context)
        {
            this.context = context;
        }

        public async Task AddAsync(Journal entity)
        {
            await context.AddAsync(entity);
            await context.SaveChangesAsync();
        }

        public async Task<Journal> GetAsync(long id)
        {
            return await context.Journal.FirstOrDefaultAsync(el => el.Id == id);
        }

        public async Task<Domain.Models.Range> GetRangeAsync(int skip, int take, Filter filter)
        {
            if (skip < 0 || take < 0)
            {
                throw new ArgumentException("'skip' and 'take' parameters can't have negative value");
            }

            var totalItems = await context.Journal.CountAsync();

            var items = context.Journal as IQueryable<Journal>;

            if (filter.From != null)
            {
                items = items.Where(el => el.CreatedAt >= filter.From);
            }
            if (filter.To != null)
            {
                items = items.Where(el => el.CreatedAt < filter.To);
            }
            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                items = items.Where(el => el.Text.Contains(filter.Search));
            }

            var result = await items
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            return new Domain.Models.Range
            {
                Skip = skip,
                Count = totalItems,
                Items = result
            };
        }
    }
}
