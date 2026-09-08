using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using test2.Models;

namespace test2.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        [ObservableProperty]
        private ViewModelBase currentPage;

        public MainWindowViewModel()
        {
            currentPage = new Page1ViewModel();
            SelectedCategory = "All";
            SelectedSort = "Reset";
            LoadProducts();
            ApplyFilters();
        }

        [ObservableProperty]
        private string _selectedCategory;

        [ObservableProperty]
        private string _selectedSort;

        [ObservableProperty]
        private string _searchText = string.Empty;

        public List<string> SortOptions { get; } = new()
        {
            "Reset",
            "Name ↑",
            "Name ↓",
            "Price ↑",
            "Price ↓"
        };
        public List<string> Categories { get; } = new()
        {
            "All",
            "Electronics",
            "Furniture"
        };

        partial void OnSelectedCategoryChanged(string value)
        {
            ApplyFilters();
        }

        partial void OnSelectedSortChanged(string value)
        {
            ApplyFilters();
        }

        partial void OnSearchTextChanged(string value)
        {
            ApplyFilters();
        }

        private void ApplyFilters()
        {
            var query = _allproducts.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                query = query.Where(p => p.Name.ToLower().Contains(SearchText.ToLower()));
            }

            if (SelectedCategory != "All")
            {
                query = query.Where(p => p.Category == SelectedCategory);
            }

            query = SelectedSort switch
            {
                "Name ↑" => query.OrderBy(p => p.Name),
                "Name ↓" => query.OrderByDescending(p => p.Name),
                "Price ↑" => query.OrderBy(p => p.Price),
                "Price ↓" => query.OrderByDescending(p => p.Price),
                _ => query
            };

            Debug.WriteLine(query.ToString());

            Products.Clear();

            foreach (var item in query)
            {
                Products.Add(item);
            }
        }
        [ObservableProperty]
        private bool sortIsVisible = false;
        [RelayCommand]
        private void ShowSort()
        {
            SortIsVisible = !(SortIsVisible);
        }

        [RelayCommand]
        private void GoPage1()
        {
            CurrentPage = new Page1ViewModel();
        }

        [RelayCommand]
        private void GoPage2()
        {
            CurrentPage = new Page2ViewModel();

        }

        [RelayCommand]
        private void GoPage3()
        {
            CurrentPage = new Page3ViewModel();

        }


        private List<Product> _allproducts = new();
        public ObservableCollection<Product> Products { get; } = new();
        private void LoadProducts()
        {
            _allproducts = new List<Product>
            {
                new Product { Id = 1,Name = "Laptop", Category = "Electronics", Price = 999.99m, Status = true, Addres = "Rd.1" , TitleOfStatus = "есть"},
                new Product { Id = 2,Name = "Smartphone", Category = "Electronics", Price = 499.99m, Status = true, Addres = "Rd.2", TitleOfStatus = "есть" },
                new Product { Id = 3,Name = "Table", Category = "Furniture", Price = 199.99m, Status = false, Addres = "Rd.3",TitleOfStatus = "нет"  },
                new Product { Id = 4,Name = "Chair", Category = "Furniture", Price = 89.99m, Status = true, Addres = "Rd.4",TitleOfStatus = "есть"  },
                new Product { Id = 5,Name = "Headphones", Category = "Electronics", Price = 199.99m , Status = false, Addres = "Rd.5",TitleOfStatus = "нет" },
                new Product { Id = 6,Name = "Sofa", Category = "Furniture", Price = 899.99m, Status = true, Addres = "Rd.6", TitleOfStatus = "есть"  }
            };
            Products.Clear();
            foreach (var product in _allproducts)
            {
                Products.Add(product);
            }
        }
        [ObservableProperty]
        private string verificationOfClear = "Очистить";

        [RelayCommand]
        private void ClearProducts()
        {

            Count += 1;
            if (Count == 1)
            {
                VerificationOfClear = "Вы уверены?";
            }
            if (Count == 2)
            {
                VerificationOfClear = "Очистить";
                _allproducts.Clear();
                Products.Clear();
                Count = 0;
            }


        }
        [ObservableProperty]
        private string newProductName = "";


        [ObservableProperty]
        private bool isvisible = false;

        [ObservableProperty]
        private int count = 0;

        [ObservableProperty]
        private Product? selectedProduct;

        [ObservableProperty]
        private string newProductCategory = "";

        [RelayCommand]
        private void SelectProduct(Product product)
        {
            SelectedProduct = product;
        }

        [ObservableProperty]
        private decimal newProductPrice;

        [ObservableProperty]
        private bool newProductStatus;

        [ObservableProperty]
        private string newProductAddress = "";

        [ObservableProperty]
        private string titleNewProduct = "Открыть добавление продукта";

        [RelayCommand]
        private void ShowAddProductDialog()
        {
            Isvisible = !(Isvisible);
            if (Isvisible == true)
            {
                TitleNewProduct = "Закрыть";
            }
            else
            {
                TitleNewProduct = "Открыть добавление продукта";
            }

        }
        [ObservableProperty]
        private string newtitleOfStatus = "";
        [RelayCommand]
        private void AddProduct()
        {
            if (NewProductStatus == true)
            {
                NewtitleOfStatus = "есть";
            }
            else
            {
                NewtitleOfStatus = "нет";
            }
            var newProduct = new Product
            {
                Name = NewProductName,
                Category = NewProductCategory,
                Price = NewProductPrice,
                Status = NewProductStatus,
                Addres = NewProductAddress,
                TitleOfStatus = NewtitleOfStatus,
            };


            Products.Add(newProduct);
            _allproducts.Add(newProduct);
            NewProductName = "";
            NewProductCategory = "";
            NewProductPrice = 0;
            NewProductStatus = false;
            NewProductAddress = "";
            NewtitleOfStatus = "";
        }

    }
}
