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
    /// Логика взаимодействия для OrgModWindow.xaml
    /// </summary>
    public partial class OrgModWindow : Window
    {
        private OrgService _service;
        private Organization? _org;
        private int _mode;

        public OrgModWindow(
            string title,
            Organization? org,
            OrgService service,
            int mode)
        {
            InitializeComponent();

            this._service = service;
            this._org = org;
            this._mode = mode;

            Title = title;

            if (_org != null)
            {
                tbName.Text = _org.OrgName;
            }
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateForm())
                return;

            switch (_mode)
            {
                case 1:
                    Organization newOrg =
                        new Organization(tbName.Text);

                    _service.Add(newOrg);
                    break;

                case 2:
                    if (_org != null)
                    {
                        _org.OrgName = tbName.Text;
                        _service.Update(_org);
                    }
                    break;

                case 3:
                    if (_org != null)
                    {
                        _service.Delete(_org);
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
                ShowError("Введите название организации", tbName);
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
