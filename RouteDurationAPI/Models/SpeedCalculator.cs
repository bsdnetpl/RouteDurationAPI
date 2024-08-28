namespace RouteDurationAPI.Models
{
    public class SpeedCalculator
    {
        public static double CalculateSpeed(double distanceInKilometers, DateTime startTime, DateTime endTime)
        {
            // Obliczamy różnicę czasu pomiędzy startem a końcem
            var timeTaken = endTime - startTime;
            var totalHours = timeTaken.TotalHours;

            // Unikamy dzielenia przez 0
            if (totalHours == 0)
            {
                throw new ArgumentException("Time taken cannot be zero.");
            }

            // Obliczamy prędkość: prędkość = odległość / czas
            return distanceInKilometers / totalHours;
        }
    }
}
