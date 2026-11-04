namespace Bl.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string entityName, int id)
            : base($"{entityName} with id {id} was not found.")
        {
            EntityName = entityName;
            Id = id;
        }

        public string EntityName { get; }

        public int Id { get; }
    }
}
