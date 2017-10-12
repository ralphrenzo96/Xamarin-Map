using System;
using System.Collections.Generic;
using System.Diagnostics;
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
                Pin = new Pin { Type = PinType.Place, Position = new Position(10.333928, 123.934259), Label = "My Current Location", Address = "Benedicto College"},
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
            customMap.MapType = MapType.Street;
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
    }
}
