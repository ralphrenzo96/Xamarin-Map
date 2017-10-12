﻿using System;
using System.Collections.Generic;
using System.Linq;
using CoreGraphics;
using MapKit;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.iOS;
using Xamarin.Forms.Platform.iOS;
using xamarinmap.Controls;
using xamarinmap.iOS.Controls;

//[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer))]
namespace xamarinmap.iOS.Controls
{
    public class CustomMapRenderer : MapRenderer
    {
		UIView customPinView;
		List<CustomPin> customPins;

		protected override void OnElementChanged(ElementChangedEventArgs<View> e)
		{
			base.OnElementChanged(e);

			if (e.OldElement != null)
			{
				var nativeMap = Control as MKMapView;
				if (nativeMap != null)
				{
					nativeMap.RemoveAnnotations(nativeMap.Annotations);
					nativeMap.GetViewForAnnotation = null;
					//nativeMap.CalloutAccessoryControlTapped -= OnCalloutAccessoryControlTapped;
					nativeMap.DidSelectAnnotationView -= OnDidSelectAnnotationView;
					nativeMap.DidDeselectAnnotationView -= OnDidDeselectAnnotationView;
				}
			}

			if (e.NewElement != null)
			{
				var formsMap = (CustomMap)e.NewElement;
				var nativeMap = Control as MKMapView;
				customPins = formsMap.CustomPins;

				nativeMap.GetViewForAnnotation = GetViewForAnnotation;
				//nativeMap.CalloutAccessoryControlTapped += OnCalloutAccessoryControlTapped;
				nativeMap.DidSelectAnnotationView += OnDidSelectAnnotationView;
				nativeMap.DidDeselectAnnotationView += OnDidDeselectAnnotationView;
			}
		}

		MKAnnotationView GetViewForAnnotation(MKMapView mapView, IMKAnnotation annotation)
		{
			MKAnnotationView annotationView = null;

            System.Diagnostics.Debug.WriteLine("Location " + annotation.Coordinate.Latitude);

			if (annotation is MKUserLocation)
				return null;

			var anno = annotation as MKPointAnnotation;
			 var customPin = GetCustomPin(anno);
			if (customPin == null)
			{
				throw new Exception("Custom pin not found");
			}

            annotationView = mapView.DequeueReusableAnnotation(customPin.ID);
			if (annotationView == null)
			{
                annotationView = new CustomMKAnnotationView(annotation, customPin.ID)
				{
					Image = UIImage.FromFile("custompin.png"),
					//CalloutOffset = new CGPoint(0, 0),
					//LeftCalloutAccessoryView = new UIImageView(UIImage.FromFile("monkey.png")),
					//RightCalloutAccessoryView = UIButton.FromType(UIButtonType.DetailDisclosure)
				};
				((CustomMKAnnotationView)annotationView).Id = customPin.ID;
				((CustomMKAnnotationView)annotationView).Url = customPin.Url;
			}
			annotationView.CanShowCallout = true;
			return annotationView;
		}

		//void OnCalloutAccessoryControlTapped(object sender, MKMapViewAccessoryTappedEventArgs e)
		//{
		//	var customView = e.View as CustomMKAnnotationView;
		//	if (!string.IsNullOrWhiteSpace(customView.Url))
		//	{
		//		UIApplication.SharedApplication.OpenUrl(new Foundation.NSUrl(customView.Url));
		//	}
		//}

		void OnDidSelectAnnotationView(object sender, MKAnnotationViewEventArgs e)
		{
			//var customView = e.View as CustomMKAnnotationView;
			//customPinView = new UIView();

			//if (customView.Id == "Xamarin")
			//{
			//	customPinView.Frame = new CGRect(0, 0, 200, 84);
			//	var image = new UIImageView(new CGRect(0, 0, 200, 84));
			//	image.Image = UIImage.FromFile("xamarin.png");
			//	customPinView.AddSubview(image);
			//	customPinView.Center = new CGPoint(0, -(e.View.Frame.Height + 75));
			//	e.View.AddSubview(customPinView);
			//}
		}

		void OnDidDeselectAnnotationView(object sender, MKAnnotationViewEventArgs e)
		{
			if (!e.View.Selected)
			{
				customPinView.RemoveFromSuperview();
				customPinView.Dispose();
				customPinView = null;
			}
		}

		CustomPin GetCustomPin(MKPointAnnotation annotation)
		{
			var position = new Position(annotation.Coordinate.Latitude, annotation.Coordinate.Longitude);
			return customPins.FirstOrDefault(pin => pin.Pin.Position == position);
		}
    }
}