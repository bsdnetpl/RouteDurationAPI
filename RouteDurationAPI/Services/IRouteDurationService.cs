using RouteDurationAPI.Models;

namespace RouteDurationAPI.Services
{
    public interface IRouteDurationService
    {
        Task<int> CleanupExpiredVehicles(int minutes);
        Task<string> RegisterVehicleEntry(VehicleEntry entry);
        Task<(string message, double? averageSpeed)> RegisterVehicleExit(VehicleEntry exitEntry);
    }
}