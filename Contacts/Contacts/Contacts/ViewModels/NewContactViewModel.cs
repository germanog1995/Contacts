using Contacts.Classes;
using Contacts.Models;
using Contacts.Services;
using GalaSoft.MvvmLight.Command;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Contacts.ViewModels
{
    public class NewContactViewModel : Contact, INotifyPropertyChanged
    {
        #region Attributes
        private DialogService digalogService;
        private ApiService apiService;
        private NavigationService navigationService;
        private ImageSource imageSource;
        private MediaFile file;
        private bool isRunning;
        private bool isEnabled;

        #endregion

        #region Properties
        public ImageSource ImageSource
        {
            set
            {
                if (imageSource != value)
                {
                    imageSource = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ImageSource"));
                }
            }
            get
            {
                return imageSource;
            }
        }
        public bool IsRunning
        {
            set
            {
                if (isRunning != value)
                {
                    isRunning = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsRunning"));
                }
            }
            get
            {
                return isRunning;
            }
        }
        public bool IsEnabled
        {
            set
            {
                if (isEnabled != value)
                {
                    isEnabled = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsEnabled"));
                }
            }
            get
            {
                return isEnabled;
            }
        }
        #endregion

        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Constructor
        public NewContactViewModel()
        {
            digalogService = new DialogService();
            apiService = new ApiService();
            navigationService = new NavigationService();

            IsEnabled = true;
        }
        #endregion

        #region Commands
        public ICommand NewContactCommand { get { return new RelayCommand(NewContact); } }

        private async void NewContact()
        {
            if (string.IsNullOrEmpty(FirstName))
            {
                await digalogService.ShowMessage("Error", "You must enter a first name");
                return;
            }

            if (string.IsNullOrEmpty(LastName))
            {
                await digalogService.ShowMessage("Error", "You must enter a last name");
                return;
            }

            byte[] imageArray = null;

            if (file != null)
            {
                imageArray = FilesHelper.ReadFully(file.GetStream());
                file.Dispose();
            }

            var contact = new Contact
            {
                EmailAddress = EmailAddress,
                FirstName = FirstName,
                ImageArray = imageArray,
                LastName = LastName,
                PhoneNumber = PhoneNumber,
            };

            IsRunning = true;
            IsEnabled = false;
            var response = await apiService.Post(
                "http://contactsxamarintata.azurewebsites.net",
                "/api",
                "/Contacts",
                contact);
            IsRunning = false;
            IsEnabled = true;

            if (!response.IsSuccess)
            {
                await digalogService.ShowMessage("Error", response.Message);
                return;
            }

            await navigationService.Back();
        }

        public ICommand TakePictureCommand { get { return new RelayCommand(TakePicture); } }

        private async void TakePicture()
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await digalogService.ShowMessage("No Camera", ":( No camera available.");
            }

            file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                Directory = "Sample",
                Name = "test.jpg",
                PhotoSize = PhotoSize.Small,
            });

            IsRunning = true;

            if (file != null)
            {
                ImageSource = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();
                    return stream;
                });
            }

            IsRunning = false;
        }

        public ICommand TakePhotoCommand { get { return new RelayCommand(TakePhoto); } }
        private async void TakePhoto()
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await digalogService.ShowMessage("No Camera", ":( No camera available.");
            }

            file = await CrossMedia.Current.PickPhotoAsync();

            IsRunning = true;

            if (file != null)
            {
                ImageSource = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();
                    return stream;
                });
            }

            IsRunning = false;
        }


        #endregion

    }
}
