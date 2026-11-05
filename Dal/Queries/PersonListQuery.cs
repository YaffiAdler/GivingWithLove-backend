namespace Dal.Queries
{
    // Shared by the Eichud and Volunteer lists, which both search and sort a person's Eichud name.
    // Code and NameContains are optional and combined with AND.
    public sealed record PersonListQuery(
        int? Code,
        string? NameContains,
        PersonSort Sort,
        PageRequest Page);
}
