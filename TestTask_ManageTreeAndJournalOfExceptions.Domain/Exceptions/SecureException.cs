namespace TestTask_ManageTreeAndJournalOfExceptions.Domain.Exceptions
{
    public class SecureException : Exception
    {
        public SecureException(string message) : base(message) { }
    }
}