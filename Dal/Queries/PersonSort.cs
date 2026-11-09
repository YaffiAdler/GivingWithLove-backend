namespace Dal.Queries
{
    public enum PersonSort
    {
        // Family name, then first name, in the requested direction; then code ascending.
        Name,
        // Code in the requested direction.
        Code
    }
}
