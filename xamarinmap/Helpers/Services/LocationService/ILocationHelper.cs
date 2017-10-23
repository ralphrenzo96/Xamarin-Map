using System;
using System.Threading.Tasks;
using Plugin.Geolocator.Abstractions;

namespace xamarinmap.Helpers.Services.LocationService
{
    public interface ILocationHelper
    {
        bool IsLocationAvailable();
        bool IsLocationEnabled();
        Task<Position> GetLocation();
    }
}
