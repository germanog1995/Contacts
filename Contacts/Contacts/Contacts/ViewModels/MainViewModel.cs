using Contacts.Services;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Contacts.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {

        #region Events
        public event PropertyChangedEventHandler PropertyChanged; 
        #endregion

        #region Attributes
        private NavigationService navigationService;
        #endregion

        #region Properties
        public EditContactViewModel EditContact { get; set; }

        public ContactsViewModel Contacts { get; set; }

        public NewContactViewModel NewContact { get; set; }
        #endregion

        #region Constructors
        public MainViewModel()
        {
            Contacts = new ContactsViewModel();
            navigationService = new NavigationService();

            instance = this;
        }
        #endregion

        #region Commands
        public ICommand AddContactCommand { get { return new RelayCommand(AddContact); } }

        private async void AddContact()
        {
            NewContact = new NewContactViewModel();
            await navigationService.Navigate("NewContactPage");
        }
        #endregion

        #region Singleton
        private static MainViewModel instance;       

        public static MainViewModel GetInstance()
        {
            if (instance == null)
            {
                instance = new MainViewModel();
            }

            return instance;
        }
        #endregion
        
    }
}
