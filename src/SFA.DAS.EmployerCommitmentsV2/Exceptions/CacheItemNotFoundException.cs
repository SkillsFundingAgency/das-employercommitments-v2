namespace SFA.DAS.EmployerCommitmentsV2.Exceptions
{
    public class CacheItemNotFoundException<T> : Exception
    {
        public CacheItemNotFoundException()
        {
        }

        public CacheItemNotFoundException(string message) : base(message)
        {
        }

        public CacheItemNotFoundException(string message, Exception exception) : base(message, exception)
        {
        }
    }
}
