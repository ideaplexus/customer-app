﻿using System;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using customerapp.Dto;

namespace customerapp
{
    public class OrderConfirmPage : ContentPage
    {

		private Order order; 

		public OrderConfirmPage(Order order)
        {
			this.order = order;
			var map = initialiseMap (order);

			Button button = new Button
			{
				Text = "Confirm Position",
				Font = Font.SystemFontOfSize(NamedSize.Medium),
				BorderWidth = 0,
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.Center
			};
			button.Clicked += OnButtonClicked;


            var stack = new StackLayout { Spacing = 0 };
            stack.Children.Add(map);
			stack.Children.Add (button);
            Content = stack;
        }

		MapWithRoute initialiseMap(Order orderApiOutput){
			
			var map = new MapWithRoute(){
				IsShowingUser = true,
				HeightRequest = 100,
				WidthRequest = 960,
				VerticalOptions = LayoutOptions.FillAndExpand
			};

			map.MapType = MapType.Satellite;
			var position = new Xamarin.Forms.Maps.Position(
				orderApiOutput.DeliveryPosition.Lat, 
				orderApiOutput.DeliveryPosition.Lon
			);
				
			foreach (var b in orderApiOutput.Route.WayPoints){
				map.RouteCoordinates.Add (b.Position);	
			}


			var pin = new Pin {
				Type = PinType.Generic,
				Position = position,
				Label = "Drop position",
				Address = "Expected drop position for your delivery"
			};

			map.Pins.Add(pin);
			map.MoveToRegion(MapSpan.FromCenterAndRadius(position, Distance.FromMeters(100)));

			return map;
		}

		async void OnButtonClicked(object sender, EventArgs e)
		{
			
			RestService rest = new RestService ();
			await rest.ConfirmOrder (orderApiOutput.id);

			// TODO change to another page 
			var stack = new StackLayout { Spacing = 0 };
			stack.Children.Add(initialiseMap(orderApiOutput));
			Content = stack;
		}
    }
}


