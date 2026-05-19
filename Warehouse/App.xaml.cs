using System.Configuration;
using System.Data;
using System.Windows;
using Warehouse.Views;
using WarehouseData.Context;
using WarehouseData.Services;

namespace Warehouse
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ApplicationContext _context;

        public OrgService orgService { get; set; }
        public WarehouseService warehouseService { get; set; }
        public InvoiceService invoiceService { get; set; }
        public ProductService productService { get; set; }

        public App()
        {
            _context = new ApplicationContext();
            orgService = new OrgService(_context);
            warehouseService = new WarehouseService(_context, null);
            invoiceService = new InvoiceService(_context, null);
            productService = new ProductService(_context, null);

            OrgsView orgsView = new OrgsView(orgService);
            orgsView.Show();
        }

    }

}
