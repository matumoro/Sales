using GalaSoft.MvvmLight.Command;
using Sales.Helpers;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace Sales.ViewModels
{
    public class AddProductViewModel:BaseViewModel
    {
      
        #region Attributes
            private bool isRunning;
            private bool isEnabled;
        #endregion

        #region Properties
            public string Description { get; set; }
            public string Price { get; set; }
            public string Remarks{ get; set; }

            public bool IsRunning
            {
                get { return this.isRunning; }
                set { SetValue(ref this.isRunning, value); }
            }
            public bool IsEnabled
            {
                get { return this.isEnabled; }
                set { SetValue(ref this.isEnabled, value); }
            }
        #endregion

        #region Constructors
            public AddProductViewModel()
            {
                this.IsEnabled = true;
            }
        #endregion

        #region Commands
            public ICommand SaveCommand
            {
                get
                {
                    return new RelayCommand(Save);
                }
            }

            private async void Save()
            {
                if (string.IsNullOrEmpty(this.Description))
                {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error, 
                    Languages.DescriptionError, 
                    Languages.Accept);
                return;
                }
                if (string.IsNullOrEmpty(this.Price))
                {
                    await Application.Current.MainPage.DisplayAlert(
                        Languages.Error,
                        Languages.PriceError,
                        Languages.Accept);
                return;
                }

            var price = decimal.Parse(this.Price);

            if (price<0)
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.PriceError,
                    Languages.Accept);
                return;
            }
        }

        #endregion

    }
}
