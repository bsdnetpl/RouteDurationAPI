namespace RouteDurationAPI.Models
{
    public class SpeedLimitConfiguration
    {
        public Guid IdSLA { get; set; }
        public double MaxAllowedSpeed { get; set; } = 60.0; // 60 km/h
        public double SectionLength { get; set; } = 10.0; // 10 km
        
    }
}
