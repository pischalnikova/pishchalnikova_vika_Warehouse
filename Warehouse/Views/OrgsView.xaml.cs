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
    /// Логика взаимодействия для OrgsView.xaml
    /// </summary>
    public partial class OrgsView : Window
    {
        private OrgService _service;
        private Organization? _org { get; set; }

        public OrgsView(OrgService orgService)
        {
            InitializeComponent();

            this._service = orgService;
            FillCompaniesCollection();
            this.lbxOrgsList.Focus();
        }

        private void Exit_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }

        private void Ok_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (_org != null && !String.IsNullOrEmpty(_org.OrgName))
            {
                MainWindow mainWindow = new MainWindow(_service.GetContext(), _org);
                mainWindow.Show();
            }
        }

        private void FillCompaniesCollection()
        {
            int idx = 0;
            if (lbxOrgsList.SelectedIndex > 0) idx = lbxOrgsList.SelectedIndex;
            lbxOrgsList.ItemsSource = null;
            lbxOrgsList.Items.Clear();
            lbxOrgsList.ItemsSource = _service.GetContext().orgs;
            if (lbxOrgsList.Items.Count > 0)
            {
                try
                {
                    lbxOrgsList.SelectedIndex = idx;
                }
                catch
                {
                    lbxOrgsList.SelectedIndex = -1;
                }
            }
        }

        private void OrgAdd_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OrgModWindow cmdWindow = new OrgModWindow("Добавить компанию", null, _service, 1);
            cmdWindow.Owner = this;
            if (cmdWindow.ShowDialog() == true) FillCompaniesCollection();
        }

        private void OrgEdit_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (_org != null)
            {
                OrgModWindow cmdWindow = new OrgModWindow("Изменить компанию", _org, _service, 2);
                cmdWindow.Owner = this;
                if (cmdWindow.ShowDialog() == true) FillCompaniesCollection();
            }
        }

        private void OrgDelete_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (_org != null)
            {
                OrgModWindow cmdWindow = new OrgModWindow("Удалить компанию", _org, _service, 3);
                cmdWindow.Owner = this;
                cmdWindow.tbName.IsReadOnly = true;
                if (cmdWindow.ShowDialog() == true)
                {
                    FillCompaniesCollection();
                }
            }
        }

        private void lbxOrgList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _org = (Organization?)lbxOrgsList.SelectedItem;
            if (_org != null && !String.IsNullOrEmpty(_org.OrgName))
            {
                tbStatus.Text = _org.OrgName;
            }
        }
    }
}
