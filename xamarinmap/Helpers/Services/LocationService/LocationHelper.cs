//using System;
//using System.Threading.Tasks;
//using Plugin.Geolocator;
//using Plugin.Geolocator.Abstractions;

//namespace xamarinmap.Helpers.Services.LocationService
//{
//    public class LocationHelper : ILocationHelper
//    {
//        public static LocationHelper location;
//        public static LocationHelper GetInstance
//        {
//			get
//			{
//				if (location == null)
//					location = new LocationHelper();

//				return location;
//			}
//        }

//        public async Task<Position> GetLocation()
//        {
//			Position position = null;
//			try
//			{
//				var locator = CrossGeolocator.Current;
//				locator.DesiredAccuracy = 100;

//				position = await locator.GetLastKnownLocationAsync();

//				if (position != null)
//					return position;

//				if (!locator.IsGeolocationEnabled || !locator.IsGeolocationAvailable)
//					return null;

//				position = await locator.GetPositionAsync(TimeSpan.FromSeconds(2), null, true);

//				if (position != null)
//					return position;
//				else
//					return null;
//			}
//			catch (Exception ex)
//			{
//				System.Diagnostics.Debug.WriteLine(ex.ToString());
//				return null;
//			}
//        }

//        public bool IsLocationAvailable()
//        {
//			if (!CrossGeolocator.IsSupported)
//				return false;

//			return CrossGeolocator.Current.IsGeolocationAvailable;
//        }

//        public bool IsLocationEnabled()
//        {
//            return CrossGeolocator.Current.IsGeolocationEnabled;
//        }
//    }
//}
