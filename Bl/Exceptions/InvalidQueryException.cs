namespace Bl.Exceptions
{
    // A list query that model validation cannot reject by itself, such as an unknown sort value.
    public class InvalidQueryException : Exception
    {
        public InvalidQueryException(string message)
            : base(message)
        {
        }
    }
}
