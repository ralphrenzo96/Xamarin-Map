using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace xamarinmap.Views
{
    public partial class MapPage : ContentPage
    {
        bool isMapLoaded;

        public MapPage()
        {
            InitializeComponent();

            myMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(10.333928, 123.934259), Distance.FromMiles(0.3)));
            isMapLoaded = true;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

			myMap.MapType = MapType.Street;
        }

        void SliderValue_Changed(Object sender, ValueChangedEventArgs e)
        {
            if (isMapLoaded)
            {
                var zoomLevel = e.NewValue; // Between 1 and 18
                var latlongDegrees = 360 / (Math.Pow(2, zoomLevel));
                myMap.MoveToRegion(new MapSpan(myMap.VisibleRegion.Center, latlongDegrees, latlongDegrees));
            }
        }
    }
}
