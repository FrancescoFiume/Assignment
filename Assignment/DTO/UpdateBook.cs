namespace Assignment.DTO;
#pragma warning disable CS1591
public struct UpdateBook
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Author { get; set; }
    public string? ISBN { get; set; }
}