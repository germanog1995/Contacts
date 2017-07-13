using Contacts.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contacts.Services
{
    public class NavigationService
    {
        public async Task Navigate(string pageName)
        {
            switch (pageName)
            {
                case "EditContactPage":
                    await App.Current.MainPage.Navigation.PushAsync(new EditContactPage());
                    break;
                case "NewContactPage":
                    await App.Current.MainPage.Navigation.PushAsync(new NewContactPage());
                    break;
                default:
                    break;
            }
        }

        public async Task Back()
        {
            await App.Current.MainPage.Navigation.PopAsync();
        }
    }

}

