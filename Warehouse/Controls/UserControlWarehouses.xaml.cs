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
    /// Логика взаимодействия для UserControlWarehouses.xaml
    /// </summary>
    public partial class UserControlWarehouses : UserControl
    {
        public WarehouseService ServiceWarehouse { get; private set; }

        public Organization Org { get; private set; }

        private WarehouseData.Models.Warehouse? _whs;

        private ProductService _serviceProducts;
        public UserControlWarehouses(WarehouseService serviceWhs)
        {
            InitializeComponent();
            Org = serviceWhs.Org;
            ServiceWarehouse = serviceWhs;

            FillWarehouseCollection();
            FillProductsCollection();
            lbxWhList.Focus();
        }

        private void FillWarehouseCollection()
        {
            int idx = 0;
            if (lbxWhList.SelectedIndex > 0) idx = lbxWhList.SelectedIndex;
            lbxWhList.ItemsSource = null;
            lbxWhList.Items.Clear();
            lbxWhList.ItemsSource = Org.warehouses;
            if (lbxWhList.Items.Count > 0)
            {
                try
                {
                    lbxWhList.SelectedIndex = idx;
                }
                catch
                {
                    lbxWhList.SelectedIndex = -1;
                }
            }
        }

        private void FillProductsCollection()
        {
            int idx = 0;
            if (DgProducts.SelectedIndex > 0) idx = DgProducts.SelectedIndex;
            DgProducts.ItemsSource = null;
            if (_whs != null)
            {
                DgProducts.ItemsSource = _whs.products;

                if (idx >= 0 && idx < DgProducts.Items.Count)
                    DgProducts.SelectedIndex = idx;
            }
        }

        private void WhAdd_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            WhModWindow cmdWindow = new WhModWindow("Добавить склад", Org, null, ServiceWarehouse, 1);
            cmdWindow.Owner = Window.GetWindow(this);
            if (cmdWindow.ShowDialog() == true) FillWarehouseCollection();
        }

        private void WhEdit_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (_whs != null)
            {
                WhModWindow cmdWindow = new WhModWindow("Изменить склад", Org, _whs, ServiceWarehouse, 2);
                cmdWindow.Owner = Window.GetWindow(this);
                if (cmdWindow.ShowDialog() == true) FillWarehouseCollection();
            }
        }

        private void WhDelete_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (_whs != null)
            {
                WhModWindow cmdWindow = new WhModWindow("Удалить склад", Org, _whs, ServiceWarehouse, 3);
                cmdWindow.Owner = Window.GetWindow(this);
                cmdWindow.tbName.IsReadOnly = true;
                cmdWindow.tbAdress.IsReadOnly = true;
                if (cmdWindow.ShowDialog() == true)
                {
                    FillWarehouseCollection();
                }
            }
        }

        private void DgWhs_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            FillProductsCollection();
        }

        private void GetProducts_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            FillProductsCollection();
        }

        private void lbxWhList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _whs = (WarehouseData.Models.Warehouse?)lbxWhList.SelectedItem;
        }

        public bool IsEmptyWhs()
        {
            return _whs == null;
        }

        public WarehouseData.Models.Warehouse? GetCurrentWarehouse()
        {
            return _whs;
        }
    }
}
