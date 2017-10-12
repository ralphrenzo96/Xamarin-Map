using System;
using System.Collections.Generic;
using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.Android;
using xamarinmap.Controls;
using xamarinmap.Droid.Controls;

[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer))]
namespace xamarinmap.Droid.Controls
{
    public class CustomMapRenderer : MapRenderer
    {
        List<CustomPin> customPins;
        CustomMap formsMap;
        CustomCircle circle;
        bool isDrawn;

        protected override void OnElementChanged(Xamarin.Forms.Platform.Android.ElementChangedEventArgs<Xamarin.Forms.Maps.Map> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                // Insert something
            }

            if(e.NewElement != null)
            {
                formsMap = (CustomMap)e.NewElement;
                customPins = formsMap.CustomPins;
                circle = formsMap.Circle;
                Control.GetMapAsync(this);
            }
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if(e.PropertyName.Equals("VisibleRegion") && !isDrawn)
            {
                NativeMap.Clear();

                foreach (var pin in customPins)
                {
                    var marker = new MarkerOptions();
                    marker.SetPosition(new LatLng(pin.Pin.Position.Latitude, pin.Pin.Position.Longitude));
                    marker.SetTitle(pin.Pin.Label);
                    marker.SetSnippet(pin.Pin.Address);
                    marker.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.custompin));
                    NativeMap.AddMarker(marker);
                }

				var circleOptions = new CircleOptions();
				circleOptions.InvokeCenter(new LatLng(circle.Position.Latitude, circle.Position.Longitude));
				circleOptions.InvokeRadius(circle.Radius);
				circleOptions.InvokeFillColor(0X6688D5D2);
				circleOptions.InvokeStrokeColor(0X6688D5D2);
				circleOptions.InvokeStrokeWidth(0);

				NativeMap.AddCircle(circleOptions);

                isDrawn = true;
            }
        }

        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            base.OnLayout(changed, l, t, r, b);

            if (changed)
                isDrawn = false;
        }

		CustomPin GetCustomPin(Marker annotation)
		{
			var position = new Position(annotation.Position.Latitude, annotation.Position.Longitude);
			foreach (var pin in customPins)
				if (pin.Pin.Position == position)
					return pin;
			
			return null;
		}

    }
}
