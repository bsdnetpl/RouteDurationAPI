using Microsoft.EntityFrameworkCore;
using RouteDurationAPI.DB;
using RouteDurationAPI.Models;

namespace RouteDurationAPI.Services
{
    public class RouteDurationService : IRouteDurationService
    {
        private readonly RouteDurationContext _context;
        private readonly SpeedLimitConfiguration _speedLimitConfig;

        public RouteDurationService(RouteDurationContext context)
        {
            _context = context;
            _speedLimitConfig = new SpeedLimitConfiguration();
        }

        // Rejestracja pojazdu w punkcie startowym
        public async Task<string> RegisterVehicleEntry(VehicleEntry entry)
        {
            var existingEntry = await _context.VehicleEntries
                .FirstOrDefaultAsync(v => v.LicensePlate == entry.LicensePlate && v.StopTimestamp == null);

            if (existingEntry != null)
            {
                return "Vehicle is already registered on the route.";
            }

            // Zapisz czas startu
            entry.StartTimestamp = DateTime.Now;
            _context.VehicleEntries.Add(entry);
            await _context.SaveChangesAsync();
            return "Vehicle registered at checkpoint 1.";
        }

        // Rejestracja wyjazdu pojazdu i obliczenie prędkości
        public async Task<(string message, double? averageSpeed)> RegisterVehicleExit(VehicleEntry exitEntry)
        {
            var licensePlateLowered = exitEntry.LicensePlate.Trim().ToLower();  // Konwersja numeru rejestracyjnego do małych liter

            // Znalezienie wpisu w bazie na podstawie numeru rejestracyjnego
            var entry = await _context.VehicleEntries
                .FirstOrDefaultAsync(v => v.LicensePlate.Trim().ToLower() == licensePlateLowered);

            if (entry == null)
            {
                return ("Vehicle with the given LicensePlate not found.", null);
            }

            // Sprawdzenie, czy pojazd ma ustawiony czas startu
            if (entry.StartTimestamp == DateTime.MinValue)
            {
                return ("StartTimestamp not set for this vehicle.", null);
            }

            // Ustawienie czasu zakończenia
            entry.StopTimestamp = DateTime.Now;

            // Obliczanie prędkości na podstawie StartTimestamp i StopTimestamp
            var startTime = entry.StartTimestamp;
            var stopTime = entry.StopTimestamp.Value; // StopTimestamp jest Nullable, więc używamy .Value

            try
            {
                var averageSpeed = SpeedCalculator.CalculateSpeed(_speedLimitConfig.SectionLength, startTime, stopTime);

                if (averageSpeed > _speedLimitConfig.MaxAllowedSpeed)
                {
                    // Oznacz, że prędkość została przekroczona
                    entry.IsSpeeding = true;
                    await _context.SaveChangesAsync();
                    return ("Speed limit exceeded.", averageSpeed);
                }

                // Usuń wpis, jeśli prędkość była zgodna z przepisami
                _context.VehicleEntries.Remove(entry);
                await _context.SaveChangesAsync();
                return ("Vehicle passed within the speed limit.", averageSpeed);
            }
            catch (ArgumentException ex)
            {
                return (ex.Message, null);
            }
        }

        // Czyszczenie pojazdów, które nie zakończyły trasy w określonym czasie
        public async Task<int> CleanupExpiredVehicles(int minutes)
        {
            var currentTime = DateTime.Now;

            // Wyszukaj pojazdy, które nie mają ustawionego czasu zakończenia i są na trasie dłużej niż określona liczba minut
            var expiredVehicles = await _context.VehicleEntries
                .Where(v => v.StopTimestamp == null && (currentTime - v.StartTimestamp).TotalMinutes > minutes)
                .ToListAsync();

            // Usuń pojazdy, które spełniają powyższe kryteria
            _context.VehicleEntries.RemoveRange(expiredVehicles);
            await _context.SaveChangesAsync();

            return expiredVehicles.Count;  // Zwróć liczbę usuniętych pojazdów
        }
    }
}
