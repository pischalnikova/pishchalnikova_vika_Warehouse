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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Warehouse.Views;
using WarehouseData.Models;
using WarehouseData.Services;

namespace Warehouse.Controls
{
    /// <summary>
    /// Логика взаимодействия для UserControlInvoices.xaml
    /// </summary>
    public partial class UserControlInvoices : UserControl
    {
        private InvoiceService _serviceInvoice;
        private ProductService _serviceProducts;
        private WarehouseData.Models.Warehouse _warehouse;

        private Invoice? _invoice;
        private Product? _product;
        public UserControlInvoices(InvoiceService serviceInvoice)
        {
            InitializeComponent();
            _warehouse = serviceInvoice.Warehouse;
            _serviceInvoice = serviceInvoice;

            FillOrdersCollection();

            FillProductsCollection();
            lbxOrders.Focus();

            if (_warehouse != null)
            {
                _serviceProducts = new ProductService(_serviceInvoice.Context, _invoice);
            }
        }

        private void FillOrdersCollection()
        {
            int idx = 0;
            if (lbxOrders.SelectedIndex > 0) idx = lbxOrders.SelectedIndex;
            lbxOrders.ItemsSource = null;
            lbxOrders.Items.Clear();
            lbxOrders.ItemsSource = _warehouse.Invoices;
            if (lbxOrders.Items.Count > 0)
            {
                try
                {
                    lbxOrders.SelectedIndex = idx;
                }
                catch
                {
                    lbxOrders.SelectedIndex = -1;
                }
            }
        }

        private void FillProductsCollection()
        {
            int idx = 0;
            if (DgProducts.SelectedIndex > 0) idx = DgProducts.SelectedIndex;
            DgProducts.ItemsSource = null;
            if (_invoice != null)
            {
                DgProducts.ItemsSource = _invoice.Products;

                if (idx >= 0 && idx < DgProducts.Items.Count)
                    DgProducts.SelectedIndex = idx;
            }
        }

        private void lbxOrders_SelectionChanged(
            object sender,
            SelectionChangedEventArgs e)
        {
            _invoice = (Invoice?)lbxOrders.SelectedItem;
        }

        private void DgProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _product = DgProducts.SelectedItem as Product;
        }

        private void DgProducts_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            FillProductsCollection();
        }

        private void DgOrds_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            FillProductsCollection();
        }

        private void AddOrder_Executed(
            object sender,
            System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            InvModWindow wnd =
                new InvModWindow(
                    "Создать накладную",
                    null,
                    _serviceInvoice,
                    1);

            wnd.Owner = Window.GetWindow(this);

            if (wnd.ShowDialog() == true)
                FillOrdersCollection();
        }

        private void ApproveOrder_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (_invoice != null)
            {
                _serviceInvoice.Update(_invoice);
                FillOrdersCollection();
                FillProductsCollection();
            }
        }

        private void RemoveOrder_Executed(
            object sender,
            System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            if (_invoice == null)
                return;

            InvModWindow wnd =
                new InvModWindow(
                    "Удалить накладную",
                    _invoice,
                    _serviceInvoice,
                    3);

            wnd.Owner = Window.GetWindow(this);

            if (wnd.ShowDialog() == true)
                FillOrdersCollection();
        }


        private void GetProducts_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            FillProductsCollection();
        }

        private void AddProduct_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (_invoice == null) return;

            _serviceProducts.Invoice = _invoice;
            ProdModWindow wnd =
                new ProdModWindow("Добавить товар", null, _serviceProducts, 1);

            wnd.Owner = Window.GetWindow(this);

            if (wnd.ShowDialog() == true)
                FillProductsCollection();
        }

        private void EditProduct_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (_product != null)
            {
                _serviceProducts.Invoice = _invoice;
                ProdModWindow wnd =
                    new ProdModWindow("Изменить товар", _product, _serviceProducts, 2);

                wnd.Owner = Window.GetWindow(this);

                if (wnd.ShowDialog() == true)
                    FillProductsCollection();
            }
        }

        private void RemoveProduct_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (_product != null)
            {
                _serviceProducts.Invoice = _invoice;
                ProdModWindow wnd =
                    new ProdModWindow("Удалить товар", _product, _serviceProducts, 3);

                wnd.Owner = Window.GetWindow(this);
                wnd.tbArticle.IsReadOnly = true;
                wnd.tbName.IsReadOnly = true;
                wnd.tbUnit.IsReadOnly = true;
                wnd.cbCategory.IsEnabled = false;
                wnd.cbManufacturer.IsEnabled = false;
                wnd.cbSupplier.IsEnabled = false;

                if (wnd.ShowDialog() == true)
                    FillProductsCollection();
            }
        }
    }
}
