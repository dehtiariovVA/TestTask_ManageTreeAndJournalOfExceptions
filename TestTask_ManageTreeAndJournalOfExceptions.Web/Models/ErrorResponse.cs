namespace TestTask_ManageTreeAndJournalOfExceptions.Web.Models
{
    public class ErrorResponse
    {
        public string Type { get; set; }
        public long Id { get; set; }
        public ErrorDetails Data { get; set; }
    }
}
