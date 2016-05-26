﻿using System;

using Xamarin.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;
using customerapp.Dto;
using Xamarin.Auth;
using System.Diagnostics;

namespace customerapp
{
	public class MockRestService : IRestService
	{
		public MockRestService ()
		{
		}

		public Task<List<Product>> GetAllProducts ()
		{
			Debug.WriteLine ("GetAllProducts");

			var list = new List <Product> ();
			list.Add (new Product{
				Id = "SOME=ID",
				Name = "Red Bull",
				Price = 2.50m,
				ProjectId = "Projectid", 
				Organisation = new Organisation{
					Id = "id",
					Name = "HSR"
				}
			});

			return Task<List<Product>>.Factory.StartNew (() => list);


		}

		public Task ConfirmOrder(String orderId, String customerId){
			Debug.WriteLine ("ConfirmOrder");
			return Task.Factory.StartNew (() => "");
		}


		public Task<Order> CreateOrder (ICollection<Product> orderProducts)
		{
			Debug.WriteLine ("CreateOrder");
			var waypoints = new List<Waypoint> ();
			waypoints.Add (new Waypoint{
				Position = new Position{
					Lat = 8, 
					Lon = 47
				}, 
				Action = "FLY"
			});

			waypoints.Add (new Waypoint{
				Position = new Position{
					Lat = 8.1234, 
					Lon = 47
				}, 
				Action = "DROP"
			});


			var missions = new List<Mission> ();
			missions.Add (new Mission{
				Id = "Id", 
				State = "Unkown", 
				Route = new Route{
					DistanceInMeters = 10, 
					WayPoints = waypoints
				}
			});

			var o = new Order{
				Id = "id",
				Missions = missions
			};

			return Task<Order>.Factory.StartNew (() => o);
		}

		public Task DeleteOrder (string orderId)
		{
			Debug.WriteLine ("DeleteOrder");
			return Task.Factory.StartNew (() => "");
		}

	
		public Task<Customer> GetCustomerInfo (Account account)
		{
			Debug.WriteLine ("GetCustomerInfo");
			var customer = new Customer {
				Id = "id",
				GivenName = "Bruce",
				FamilyName = "Wayne",
				Email = "batman@gmail.com"
			};
			return Task<Customer>.Factory.StartNew (() => customer);
		}

		public Task<Customer> SaveCustomer (Customer c)
		{
			Debug.WriteLine ("SaveCustomer");
			var customer = new Customer {
				Id = "id",
				GivenName = "Bruce",
				FamilyName = "Wayne",
				Email = "batman@gmail.com"
			};
			return Task<Customer>.Factory.StartNew (() => customer);
		}


		public Task<Customer> GetCustomerById (String customerId)
		{
			Debug.WriteLine ("GetCustomerById");
			var customer = new Customer {
				Id = "id",
				GivenName = "Bruce",
				FamilyName = "Wayne",
				Email = "batman@gmail.com"
			};
			return Task<Customer>.Factory.StartNew (() => customer);
		}

	}
}


