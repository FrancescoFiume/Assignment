using System.Text.Json.Serialization;

namespace Assignment.Data.Models;

#pragma warning disable CS1591
public class Books : IObjectDb
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Author { get; set; }

    public required string ISBN { get; set; }
    public Availability Status { get; set; }
    public enum Availability : ushort
    {
        Unavailable = 0,
        Available = 1
    }

    [JsonIgnore]
    public virtual ICollection<Reservations> Reservations { get; set; }


}