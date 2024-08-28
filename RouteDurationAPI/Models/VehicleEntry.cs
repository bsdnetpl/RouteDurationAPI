using System.Text.Json.Serialization;

namespace RouteDurationAPI.Models
{
    public class VehicleEntry
    {
        public int Id { get; set; }
        public string? LicensePlate { get; set; }
        public DateTime StartTimestamp { get; set; } 
        public DateTime? StopTimestamp { get; set; }
        public bool IsSpeeding { get; set; }
        [JsonIgnore]
        public Guid IdDuration { get; set; }
        public Guid idCammera { get; set; }
    }
}
