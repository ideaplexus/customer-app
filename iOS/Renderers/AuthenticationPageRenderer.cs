﻿using Newtonsoft.Json;
using System;
using System.Linq;
using TodoAWSSimpleDB;
using TodoAWSSimpleDB.iOS;
using Xamarin.Auth;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using customerapp;
using System.Diagnostics;

[assembly: ExportRenderer(typeof(AuthenticationPage), typeof(AuthenticationPageRenderer))]

namespace TodoAWSSimpleDB.iOS
{
    // Use a custom page renderer to display the authentication UI on the AuthenticationPage
    public class AuthenticationPageRenderer : PageRenderer
    {
        bool isShown;
		RestService rest = App.Rest;

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            if (!App.IsLoggedIn)
            {
                if (!isShown)
                {
                    isShown = true;

                    // Initialize the object that communicates with the OAuth service
                    var auth = new OAuth2Authenticator(
                        Constants.ClientId,
                        Constants.ClientSecret,
                        Constants.Scope,
                        new Uri(Constants.AuthorizeUrl),
                        new Uri(Constants.RedirectUrl),
                        new Uri(Constants.AccessTokenUrl));

                    // Register an event handler for when the authentication process completes
                    auth.Completed += OnAuthenticationCompleted;

                    // Display the UI
                    PresentViewController(auth.GetUI(), true, null);
                }
            }
            else
            {
                if (!isShown)
                {
//					App.Customer.Email = account.Username;
                    App.SuccessfulLoginAction.Invoke();					
                }
            }
        }

        async void OnAuthenticationCompleted(object sender, AuthenticatorCompletedEventArgs e)
        {
            if (e.IsAuthenticated)
            {
				App.Customer = await rest.GetCustomerInfo (e.Account);
				App.Customer = await rest.SaveCustomer (App.Customer);
				Application.Current.Properties ["customerId"] = App.Customer.Id;

				Debug.WriteLine ("Set customerId to {0}", Application.Current.Properties ["customerId"]);
				await Application.Current.SavePropertiesAsync ();
            }
			
            App.SuccessfulLoginAction.Invoke();
        }
    }
}
