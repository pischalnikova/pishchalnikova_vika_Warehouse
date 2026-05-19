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
    /// Логика взаимодействия для InvModWindow.xaml
    /// </summary>
    public partial class InvModWindow : Window
    {
        private InvoiceService _service;
        private Invoice? _invoice;
        private int _mode;
        public InvModWindow(string title,
            Invoice? order,
            InvoiceService service,
            int mode)
        {
            InitializeComponent();

            _service = service;
            _invoice = order;
            _mode = mode;

            Title = title;

            if (_invoice != null)
            {
                tbNumber.Text = _invoice.Number;

                if (_invoice.Type == InvType.Входящая)
                    cbType.SelectedIndex = 0;
                else
                    cbType.SelectedIndex = 1;
            }

            if (_mode == 3)
            {
                tbNumber.IsReadOnly = true;
                cbType.IsEnabled = false;
            }
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateForm())
                return;

            switch (_mode)
            {
                // создание
                case 1:
                    Invoice newOrder = new Invoice
                    {
                        Number = tbNumber.Text,
                        Type = GetSelectedType()
                    };

                    _service.Add(newOrder);
                    break;

                // удаление
                case 3:
                    if (_invoice != null)
                    {
                        _service.Delete(_invoice);
                    }
                    break;
            }

            DialogResult = true;
            Close();
        }

        private InvType GetSelectedType()
        {
            ComboBoxItem item =
                (ComboBoxItem)cbType.SelectedItem;

            if (item.Content.ToString() == "Входящая")
                return InvType.Входящая;

            return InvType.Исходящая;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private bool ValidateForm()
        {
            if (string.IsNullOrWhiteSpace(tbNumber.Text))
            {
                ShowError("Введите номер накладной", tbNumber);
                return false;
            }

            if (cbType.SelectedItem == null)
            {
                ShowError("Выберите тип накладной");
                cbType.Focus();
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
