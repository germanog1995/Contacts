using Contacts.Models;
using Contacts.Services;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Contacts.ViewModels
{
    public class ContactItemViewModel : Contact
    {
        #region Attributes
        private NavigationService navigationService;
        #endregion

        #region Constructors

        public ContactItemViewModel()
        {
            navigationService = new NavigationService();
        }
        #endregion

        #region Commands
        public ICommand EditContactCommand { get { return new RelayCommand(EditContact); } }

        private async void EditContact()
        {
            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.EditContact = new EditContactViewModel(this);
            await navigationService.Navigate("EditContactPage");
        }
        #endregion
    }
}
