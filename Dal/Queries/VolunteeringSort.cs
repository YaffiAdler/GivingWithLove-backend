namespace Dal.Queries
{
    // The requested direction applies to the sorted column; ties are always broken newest first.
    public enum VolunteeringSort
    {
        // Date, then VolunteeringCode descending.
        Date,
        // Volunteer family name, then first name, then newest first.
        Volunteer,
        // Project name, then newest first.
        Project
    }
}
