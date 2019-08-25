namespace Sales.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Sales.Common.Models;
    using Sales.Helpers;
    using Sales.Services;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;
    using Xamarin.Forms;

    public class ProductsViewModel: BaseViewModel
    {
        #region Services
            private ApiService apiService;
        #endregion

        #region Attibutes
            private bool isRefreshing;
            private ObservableCollection<ProductItemViewModel> products;
        #endregion

        #region Properties
        public List<Product> MyProducts { get; set; }
        public ObservableCollection<ProductItemViewModel> Products
            {
                get { return this.products; }
                set { SetValue(ref this.products, value); }
            }
            public bool IsRefreshing
            {
                get { return this.isRefreshing; }
                set { SetValue(ref this.isRefreshing, value); }
            }

        #endregion

        #region Constructors
            public ProductsViewModel()
            {
                instance = this;
                this.apiService = new ApiService();
                this.LoadProducts();
            }
        #endregion

        #region Singleton
            private static ProductsViewModel instance;
            public static ProductsViewModel GetInstance()
            {
                if (instance==null)
                {
                    return new ProductsViewModel();
                }
                return instance;
            }
        #endregion

        #region Methods
        private async void LoadProducts()
        {
            this.IsRefreshing = true;
            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert(Languages.Error, connection.Message, Languages.Accept);
                return;
            }

            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.GetList<Product>(url, "/Api", "/products");

            if (!response.IsSuccess)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert(Languages.Error, response.Message, Languages.Accept);
                return;
            }
            this.MyProducts = (List<Product>)response.Result;
            this.RefreshList();
            var myListProductItemViewModel = MyProducts.Select(p => new ProductItemViewModel
            {
                Description=p.Description,
                ImageArray = p.ImageArray,
                ImagePath = p.ImagePath,
                IsAvailable = p.IsAvailable,
                Price = p.Price,
                ProductId = p.ProductId,
                PublishOn = p.PublishOn,
                Remarks = p.Remarks,


            });


            this.Products = new ObservableCollection<ProductItemViewModel>(
                myListProductItemViewModel.OrderBy(p => p.Description));
            this.IsRefreshing = false;
        }

        public void RefreshList()
        {
            var myListProductItemViewModel = MyProducts.Select(p => new ProductItemViewModel
            {
                Description = p.Description,
                ImageArray = p.ImageArray,
                ImagePath = p.ImagePath,
                IsAvailable = p.IsAvailable,
                Price = p.Price,
                ProductId = p.ProductId,
                PublishOn = p.PublishOn,
                Remarks = p.Remarks,


            });


            this.Products = new ObservableCollection<ProductItemViewModel>(
                myListProductItemViewModel.OrderBy(p => p.Description));
        }
        #endregion

        #region Commands
        public ICommand RefreshCommand
            {
                get
                {
                    return new RelayCommand(LoadProducts);

                }
            }
        #endregion

    }
}
