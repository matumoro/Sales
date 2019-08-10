using GalaSoft.MvvmLight.Command;
using Sales.Views;
using System.Windows.Input;
using Xamarin.Forms;

namespace Sales.ViewModels
{
    public class MainViewModel
    {
        public ProductsViewModel Products { get; set; }

        public AddProductViewModel addProduct { get; set; }
        public MainViewModel()
        {
            this.Products = new ProductsViewModel();
        }

        public ICommand AddProductCommand
        {
            get
            {
                return new RelayCommand(GoToAddProduct);
            }
        
        }

        private async void GoToAddProduct()
        {
            this.addProduct = new AddProductViewModel();
            await Application.Current.MainPage.Navigation.PushAsync(new AddProductPage());
        }
    }

}
