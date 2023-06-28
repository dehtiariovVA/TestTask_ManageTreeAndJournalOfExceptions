using Microsoft.EntityFrameworkCore;
using TestTask_ManageTreeAndJournalOfExceptions.Domain.Entities;

namespace TestTask_ManageTreeAndJournalOfExceptions.Data.EFDatabaseContext
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }

        public DbSet<Journal> Journal { get; set; }

        public DbSet<Node> Nodes { get; set; }
    }
}
