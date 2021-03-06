﻿using System;
using Xamarin.Forms;
using MapOverlay.Droid;
using Xamarin.Forms.Maps.Android;
using customerapp;
using System.Collections.Generic;
using customerapp.Dto;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Xamarin.Forms.Maps;
using System.ComponentModel;

[assembly:ExportRenderer (typeof(MapWithRoute), typeof(CustomMapRenderer))]
namespace MapOverlay.Droid
{
	public class CustomMapRenderer : MapRenderer, IOnMapReadyCallback
	{
		
		GoogleMap map;
		MapWithRoute mapWithRoute;

		List<customerapp.Dto.Position> calculatedRoute;
		List<customerapp.Dto.Position> flownRoute;

		protected override void OnElementChanged (Xamarin.Forms.Platform.Android.ElementChangedEventArgs<View> e)
		{
			base.OnElementChanged (e);

			if (e.OldElement != null) {
				// Unsubscribe
			}

			if (e.NewElement != null) {
				var formsMap = (MapWithRoute)e.NewElement;
                mapWithRoute = formsMap;
				calculatedRoute = formsMap.CalculatedRoute;
				flownRoute = formsMap.FlownRoute;

				formsMap.PropertyChanged += OnChange;

				((MapView)Control).GetMapAsync (this);
			}
		
		}

		public void OnChange(object o,PropertyChangedEventArgs ar){
            Device.BeginInvokeOnMainThread( () => {

                addDroneImage();

    			var pos = new Xamarin.Forms.Maps.Position (
                    mapWithRoute.CurrentPosition.Lat,
                    mapWithRoute.CurrentPosition.Lon
    			);
    		
                mapWithRoute.MoveToRegion(MapSpan.FromCenterAndRadius(pos, Distance.FromMeters(100)));
           
            });
		}

        private void addDroneImage(){
            var currentPositionAsLatLon = new LatLng(
                mapWithRoute.CurrentPosition.Lat,
                mapWithRoute.CurrentPosition.Lon
            );
                
            var droneResource = customerapp.Droid.Resource.Drawable.drone;
            var image = BitmapDescriptorFactory.FromResource(droneResource);
            var groundOverLay = new GroundOverlayOptions()
                .Position(currentPositionAsLatLon, 16)
                .InvokeImage(image);
            map.AddGroundOverlay(groundOverLay);
        }

		public void OnMapReady (GoogleMap googleMap)
		{
			map = googleMap;

			var polylineOptions = new PolylineOptions ();
			polylineOptions.InvokeColor (0x66FF0000);

			foreach (var position in calculatedRoute) {
				polylineOptions.Add (new LatLng (position.Lat, position.Lon));
			}
			map.AddPolyline (polylineOptions);


			polylineOptions = new PolylineOptions ();
            polylineOptions.InvokeColor (customerapp.Droid.Resource.Color.green);
            polylineOptions.InvokeWidth(10);

			foreach (var position in flownRoute) {
				polylineOptions.Add (new LatLng (position.Lat, position.Lon));
			}
			map.AddPolyline (polylineOptions);
		}
	}
}