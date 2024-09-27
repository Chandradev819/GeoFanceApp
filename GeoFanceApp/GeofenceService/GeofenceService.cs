namespace GeoFanceApp.GeofenceService
{
    public class GeofenceService
    {
        public async Task<Location> GetCurrentLocationAsync()
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Best);
                return await Geolocation.GetLocationAsync(request);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching location: {ex.Message}");
                return null;
            }
        }

        public bool IsWithinGeofence(Location currentLocation, double fenceLat, double fenceLon, double radiusInMeters)
        {
            double distance = CalculateDistance(currentLocation.Latitude, currentLocation.Longitude, fenceLat, fenceLon);
            return distance <= radiusInMeters;
        }

        private double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            const double R = 6371000; // Earth radius in meters
            var latDelta = ToRadians(lat2 - lat1);
            var lonDelta = ToRadians(lon2 - lon1);
            var latRad1 = ToRadians(lat1);
            var latRad2 = ToRadians(lat2);

            var a = Math.Sin(latDelta / 2) * Math.Sin(latDelta / 2) +
                    Math.Cos(latRad1) * Math.Cos(latRad2) *
                    Math.Sin(lonDelta / 2) * Math.Sin(lonDelta / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return R * c; // Return distance in meters
        }

        private double ToRadians(double degrees) => degrees * Math.PI / 180;
    }
}
