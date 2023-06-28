using TestTask_ManageTreeAndJournalOfExceptions.Domain.Entities;

namespace TestTask_ManageTreeAndJournalOfExceptions.Domain.Models
{
    public class Range
    {
        public int Skip { get; set; }
        public int Count { get; set; }
        public IEnumerable<JournalInfo> Items { get; set; }
    }
}