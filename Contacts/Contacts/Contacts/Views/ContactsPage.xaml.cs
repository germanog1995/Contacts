using Contacts.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Contacts.Views
{
    public partial class ContactsPage : ContentPage
    {
        public ContactsPage()
        {
            InitializeComponent();

            var contactsViewModel = ContactsViewModel.GetInstance();
            base.Appearing += (object sender, EventArgs e) =>
            {
                contactsViewModel.RefreshCommand.Execute(this);
            };
        
        }
    }
}