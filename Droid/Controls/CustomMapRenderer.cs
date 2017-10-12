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
		Circle circleInner, circleOuter;
		CircleOptions circleOptionsInner, circleOptionsOuter;

        CustomMap formsMap;
        CustomCircle circle;
        bool isDrawn;
        bool firstLoad = true;

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

                    DrawCircles(new Point(pin.Pin.Position.Latitude, pin.Pin.Position.Longitude));
                }



				//var circleOptions = new CircleOptions();
				//circleOptions.InvokeCenter(new LatLng(circle.Position.Latitude, circle.Position.Longitude));
				//circleOptions.InvokeRadius(circle.Radius);
				//circleOptions.InvokeFillColor(0X6688D5D2);
				//circleOptions.InvokeStrokeColor(0X6688D5D2);
				//circleOptions.InvokeStrokeWidth(0);

				//NativeMap.AddCircle(circleOptions);

                isDrawn = true;
            }
        }

		void DrawCircles(Point e)
		{
			if (circleOptionsInner == null)
			{
				circleOptionsInner = new CircleOptions().InvokeCenter(new LatLng(e.X, e.Y)).InvokeRadius(80).InvokeFillColor(Android.Graphics.Color.ParseColor("#0D15b8fe")).InvokeStrokeWidth(0);
				circleInner = NativeMap.AddCircle(circleOptionsInner);
			}

			if (circleOptionsOuter == null)
			{
				circleOptionsOuter = new CircleOptions().InvokeCenter(new LatLng(e.X, e.Y)).InvokeRadius(350).InvokeFillColor(Android.Graphics.Color.ParseColor("#1A15b8fe")).InvokeStrokeWidth(0);
				circleOuter = NativeMap.AddCircle(circleOptionsOuter);
			}

			if (firstLoad == true)
			{
				NativeMap.MoveCamera(CameraUpdateFactory.NewLatLngZoom(new LatLng(e.X, e.Y), 15.8f));
				var animation = new Animation(v => circleInner.Radius = v, 25, 200);
				animation.Commit(Xamarin.Forms.Application.Current.MainPage, "SimpleAnimation", 500, 2000, Easing.CubicOut, null, () => true);
				var fadeEffect = new Animation(v => circleInner.FillColor = Android.Graphics.Color.ParseColor(string.Format("#{0}15b8fe", ((int)(v < 0 ? 0 : v)).ToString("D2"))), 99, -20);
				fadeEffect.Commit(Xamarin.Forms.Application.Current.MainPage, "FadeAnimation", 500, 2000, Easing.Linear, null, () => true);
				firstLoad = false;
			}
			circleInner.Center = new LatLng(e.X, e.Y);
			circleOuter.Center = new LatLng(e.X, e.Y);
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
