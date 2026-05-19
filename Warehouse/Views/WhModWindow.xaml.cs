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
    /// Логика взаимодействия для WhModWindow.xaml
    /// </summary>
    public partial class WhModWindow : Window
    {
        private WarehouseService _service;
        private Organization? _org;
        private WarehouseData.Models.Warehouse? _whs;
        private int _mode;

        public WhModWindow(
            string title,
            Organization? org,
            WarehouseData.Models.Warehouse? whs,
            WarehouseService service,
            int mode)
        {
            InitializeComponent();

            this._service = service;
            this._org = org;
            this._whs = whs;
            this._mode = mode;

            Title = title;

            if (_whs != null)
            {
                tbName.Text = _whs.WhName;
                tbAdress.Text = _whs.WhAddress;
            }
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateForm())
                return;

            switch (_mode)
            {
                case 1:
                    if (_org != null)
                    {
                        WarehouseData.Models.Warehouse newWhs =
                            new WarehouseData.Models.Warehouse(
                                tbName.Text,
                                tbAdress.Text,
                                _org.OrgId);
                        _service.Add(newWhs);
                    }
                    break;

                case 2:
                    if (_whs != null)
                    {
                        _whs.WhName = tbName.Text;
                        _whs.WhAddress = tbAdress.Text;
                        _service.Update(_whs);
                    }
                    break;

                case 3:
                    if (_whs != null)
                    {
                        _service.Delete(_whs);
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
            if (string.IsNullOrWhiteSpace(tbName.Text))
            {
                ShowError("Введите название склада", tbName);
                return false;
            }

            if (string.IsNullOrWhiteSpace(tbAdress.Text))
            {
                ShowError("Введите адрес склада", tbAdress);
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
