using Contacts.Services;
using System.Collections.ObjectModel;
using System;
using Contacts.Models;
using Plugin.Connectivity;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;

namespace Contacts.ViewModels
{
    public class ContactsViewModel : INotifyPropertyChanged
    {
        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Attributes
        private ApiService apiService;
        private DialogService dialogService;
        private NavigationService navigationService;
        private bool isRefreshing;
        #endregion

        #region Properties
        public ObservableCollection<ContactItemViewModel> MyContacts { get; set; }

        public NewContactViewModel NewContact { get; set; }

        public bool IsRefreshing
        {
            set
            {
                if (isRefreshing != value)
                {
                    isRefreshing = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsRefreshing"));
                }
            }
            get
            {
                return isRefreshing;
            }
        }

        #endregion

        #region Constructors
        public ContactsViewModel()
        {
            instance = this;

            apiService = new ApiService();
            dialogService = new DialogService();
            navigationService = new NavigationService();
            IsRefreshing = false;
            
            MyContacts = new ObservableCollection<ContactItemViewModel>();
        }


        #endregion

        #region Methods
        private async void LoadContacts()
        {
            //if(!CrossConnectivity.Current.IsConnected)
            // {
            //     await dialogService.ShowMessage("Error", "Check you internet connection.");
            //     return;
            // }
            
            var response = await apiService.Get<Contact>("http://contactsxamarintata.azurewebsites.net/", "/api", "/Contacts");
            if (!response.IsSuccess)
            {
                await dialogService.ShowMessage("Error", response.Message);
                return;
            }

            ReloadContacts((List<Contact>)response.Result);
        }

        private void ReloadContacts(List<Contact> contacts)
        {
            MyContacts.Clear();
            foreach (var contact in contacts.OrderBy(c => c.FirstName).ThenBy(c => c.LastName))
            {
                MyContacts.Add(new ContactItemViewModel
                {
                    ContactId = contact.ContactId,
                    EmailAddress = contact.EmailAddress,
                    FirstName = contact.FirstName,
                    Image = contact.Image,
                    LastName = contact.LastName,
                    PhoneNumber = contact.PhoneNumber,
                });
            }
        }
        #endregion

        #region Commands
        public ICommand RefreshCommand { get { return new RelayCommand(Refresh); } }
        private void Refresh()
        {
            IsRefreshing = true;
            LoadContacts();
            IsRefreshing = false;
        }

        #endregion

        #region Singleton
        private static ContactsViewModel instance;

        public static ContactsViewModel GetInstance()
        {
            if (instance == null)
            {
                instance = new ContactsViewModel();
            }

            return instance;
        }
        #endregion
    }
}
