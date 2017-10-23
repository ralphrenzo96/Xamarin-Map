using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Plugin.Geolocator;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using xamarinmap.Controls;

namespace xamarinmap.Views
{
    public partial class MapPage : ContentPage
    {
        bool isMapLoaded;


        public MapPage()
        {
            InitializeComponent();

    

			CustomPin pin = new CustomPin
			{
				Pin = new Pin { Type = PinType.Place, Position = new Position(10.333928, 123.934259), Label = "My Current Location", Address = "Benedicto College" },
				ID = "Xamarin",
				Url = "http://google.com"
			};

			customMap.CustomPins = new List<CustomPin> { pin };

			customMap.Circle = new CustomCircle
			{
				Position = new Position(10.333928, 123.934259),
				Radius = 300
			};

			customMap.Pins.Add(pin.Pin);

			customMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(10.333928, 123.934259), Distance.FromMiles(0.3)));
			isMapLoaded = true;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            //GetLocation();
        }

        public async void GetLocation()
        {
            var locator = CrossGeolocator.Current;
            var position = await locator.GetPositionAsync(TimeSpan.FromSeconds(1.5));

            Debug.WriteLine("Position Status: {0}", position.Timestamp);
			Debug.WriteLine("Position Latitude: {0}", position.Latitude);
			Debug.WriteLine("Position Longitude: {0}", position.Longitude);
        }

        async Task<Plugin.Geolocator.Abstractions.Position> GetPosition()
        {
			var locator = CrossGeolocator.Current;
			locator.DesiredAccuracy = 50;

            return await locator.GetPositionAsync(TimeSpan.FromSeconds(5));
        }

        void SliderValue_Changed(Object sender, ValueChangedEventArgs e)
        {
            if (isMapLoaded)
            {
                var zoomLevel = e.NewValue; // Between 1 and 18
                var latlongDegrees = 360 / (Math.Pow(2, zoomLevel));
                customMap.MoveToRegion(new MapSpan(customMap.VisibleRegion.Center, latlongDegrees, latlongDegrees));
            }
        }

        void MapType_Changed(Object sender, EventArgs e)
        {
            int type = Convert.ToInt32(((Button)sender).CommandParameter.ToString());
            switch(type)
            {
                case 1: customMap.MapType = MapType.Street; break;
                case 2: customMap.MapType = MapType.Satellite; break;
                case 3: customMap.MapType = MapType.Hybrid; break;
            }
        }
    }
}
