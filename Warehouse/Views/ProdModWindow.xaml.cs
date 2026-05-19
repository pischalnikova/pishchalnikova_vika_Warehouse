using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WarehouseData.Models;
using WarehouseData.Services;

namespace Warehouse.Views
{
    /// <summary>
    /// Логика взаимодействия для ProdModWindow.xaml
    /// </summary>
    public partial class ProdModWindow : Window
    {
        private ProductService _service;
        private Product? _product;
        private int _mode;
        public ProdModWindow(string title,
            Product? product,
            ProductService service,
            int mode)
        {
            InitializeComponent();

            _service = service;
            _product = product;
            _mode = mode;

            Title = title;

            FillDirectories();

            if (_product != null)
            {
                tbArticle.Text = _product.Article;
                tbName.Text = _product.Name;
                tbUnit.Text = _product.Unit;
                tbPrice.Text = _product.Price.ToString();
                tbQuantity.Text = _product.StockQuantity.ToString();
                tbDescription.Text = _product.Description;

                cbCategory.SelectedItem = _product.Category;
                cbManufacturer.SelectedItem = _product.Manufacturer;
                cbSupplier.SelectedItem = _product.Supplier;
            }
        }

        private void FillDirectories()
        {
            cbCategory.ItemsSource = _service.Context.Categories;
            cbManufacturer.ItemsSource = _service.Context.Manufacturers;
            cbSupplier.ItemsSource = _service.Context.Suppliers;
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateForm())
                return;

            switch (_mode)
            {
                case 1:
                    if (_service.Invoice != null)
                    {
                        Product newProduct = new Product
                        {
                            Article = tbArticle.Text,
                            Name = tbName.Text,
                            Unit = tbUnit.Text,
                            Price = decimal.Parse(tbPrice.Text),
                            StockQuantity = int.Parse(tbQuantity.Text),
                            Description = tbDescription.Text,

                            Category = (Category)cbCategory.SelectedItem,
                            Manufacturer = (Manufacturer)cbManufacturer.SelectedItem,
                            Supplier = (Supplier)cbSupplier.SelectedItem
                        };

                        _service.Add(newProduct);
                    }

                    break;

                case 2:
                    if (_service.Invoice != null && _product != null)
                    {
                        _product.Article = tbArticle.Text;
                        _product.Name = tbName.Text;
                        _product.Unit = tbUnit.Text;
                        _product.Price = decimal.Parse(tbPrice.Text);
                        _product.StockQuantity = int.Parse(tbQuantity.Text);
                        _product.Description = tbDescription.Text;

                        _product.Category = (Category)cbCategory.SelectedItem;
                        _product.Manufacturer = (Manufacturer)cbManufacturer.SelectedItem;
                        _product.Supplier = (Supplier)cbSupplier.SelectedItem;

                        _service.Update(_product);
                    }
                    break;

                case 3:
                    if (_service.Invoice != null && _product != null)
                    {
                        _service.Delete(_product);
                    }
                    break;
            }

            DialogResult = true;
            Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private bool ValidateForm()
        {
            if (string.IsNullOrWhiteSpace(tbArticle.Text))
            {
                ShowError("Введите артикул", tbArticle);
                return false;
            }

            if (string.IsNullOrWhiteSpace(tbName.Text))
            {
                ShowError("Введите наименование товара", tbName);
                return false;
            }

            if (string.IsNullOrWhiteSpace(tbUnit.Text))
            {
                ShowError("Введите единицу измерения", tbUnit);
                return false;
            }

            if (!decimal.TryParse(tbPrice.Text, out decimal price))
            {
                ShowError("Цена должна быть числом", tbPrice);
                return false;
            }

            if (price < 0)
            {
                ShowError("Цена не может быть отрицательной", tbPrice);
                return false;
            }

            if (!int.TryParse(tbQuantity.Text, out int qty))
            {
                ShowError("Количество должно быть целым числом", tbQuantity);
                return false;
            }

            if (qty < 0)
            {
                ShowError("Количество не может быть отрицательным", tbQuantity);
                return false;
            }

            if (cbCategory.SelectedItem == null)
            {
                ShowError("Выберите категорию");
                cbCategory.Focus();
                return false;
            }

            if (cbManufacturer.SelectedItem == null)
            {
                ShowError("Выберите производителя");
                cbManufacturer.Focus();
                return false;
            }

            if (cbSupplier.SelectedItem == null)
            {
                ShowError("Выберите поставщика");
                cbSupplier.Focus();
                return false;
            }

            return true;
        }

        private void ShowError(string message, Control? control = null)
        {
            MessageBox.Show(
                message,
                "Ошибка ввода",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);

            control?.Focus();
        }
    }
}
