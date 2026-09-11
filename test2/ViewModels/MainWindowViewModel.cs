using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using test2.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using test2.Data;
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

        [ObservableProperty]
        private string _searchId;

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

        partial void OnSearchIdChanged(string value)
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
                
                bool isIdSearch = int.TryParse(SearchText, out int idValue);

                query = query.Where(p =>
                    p.Name.ToLower().Contains(SearchText.ToLower()) ||
                    (isIdSearch && p.Id == idValue)
                );
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

        [ObservableProperty]
        private string productName = string.Empty;

        [ObservableProperty]
        private decimal productPrice;

        [ObservableProperty]
        private int productId;
        [ObservableProperty]
        private string productCategory = string.Empty;
        [ObservableProperty]
        private string productTittleOfStatus = string.Empty;
        [ObservableProperty]
        private string productAddres = string.Empty;
        [ObservableProperty]
        private string productStatus = string.Empty;


        [RelayCommand]
        private async Task LoadProducts()
        {
            await using var db = new AppDbContext();

            var products = await db.Products
                .AsNoTracking()
                .ToListAsync();

            Products.Clear();

            foreach (var product in products)
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
                _currentId = 0;
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

        private int _currentId;

        [ObservableProperty]
        private string newtitleOfStatus = "";
        [RelayCommand]
        private async Task AddProduct()
        {

            if (string.IsNullOrWhiteSpace(ProductName))
                return;

            await using var db = new AppDbContext();

            var product = new Product
            {   Id = ProductId, 
                Name = ProductName,
                Category = ProductCategory,
                Price = ProductPrice,
                Status = ProductStatus,
                Addres = ProductAddres,
                TitleOfStatus = ProductTittleOfStatus
            };

            db.Products.Add(product);
            await db.SaveChangesAsync();

            Products.Add(product);

            ProductName = string.Empty;
            ProductPrice = 0;
            ProductStatus = string.Empty;
            ProductAddres = string.Empty;
            ProductId = 1;
            ProductCategory = string.Empty;
            ProductTittleOfStatus = string.Empty;

        }

    }
}
