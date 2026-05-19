using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using WarehouseData.Context;
using WarehouseData.Models;

namespace WarehouseData.Services
{
    public interface IInvoiceService
    {
        bool Add(Invoice invoice);
        bool Update(Invoice invoice);
        bool Delete(Invoice invoice);
    }

    public class InvoiceService : IInvoiceService
    {
        public ApplicationContext Context { get; set; }
        public Warehouse Warehouse { get; set; }

        public InvoiceService(
            ApplicationContext context,
            Warehouse warehouse)
        {
                Context = context;
                Warehouse = warehouse;
        }

        public bool Add(Invoice invoice)
        {
            if (invoice == null)
                return false;

            if (string.IsNullOrWhiteSpace(invoice.Number))
                return false;

            Warehouse.Invoices.Add(invoice);
            return true;
        }

        public bool Update(Invoice invoice)
        {
            if (invoice == null)
                return false;

            if (invoice.Status == InvStatus.Согласована)
                return false;

            if (invoice.Products.Count == 0)
                return false;

            // Приход
            if (invoice.Type == InvType.Входящая)
            {
                foreach (Product product in invoice.Products)
                {
                    Product? existing =
                        Warehouse.products.FirstOrDefault(
                            p => p.Article == product.Article);

                    if (existing == null)
                    {
                        Warehouse.products.Add(product);
                    }
                    else
                    {
                        existing.StockQuantity += product.StockQuantity;
                    }
                }
            }

            // расход
            if (invoice.Type == InvType.Исходящая)
            {
                foreach (Product product in invoice.Products)
                {
                    Product? existing =
                        Warehouse.products.FirstOrDefault(
                            p => p.Article == product.Article);

                    if (existing == null)
                        return false;

                    if (existing.StockQuantity < product.StockQuantity)
                        return false;
                }

                foreach (Product product in invoice.Products)
                {
                    Product existing =
                        Warehouse.products.First(
                            p => p.Article == product.Article);

                    existing.StockQuantity -= product.StockQuantity;

                    if (existing.StockQuantity == 0)
                        Warehouse.products.Remove(existing);
                }
            }

            invoice.Status = InvStatus.Согласована;
            return true;
        }

        public bool Delete(Invoice invoice)
        {
            if (invoice == null)
                return false;

            // утвержденные удалять нельзя
            if (invoice.Status == InvStatus.Согласована)
                return false;

            return Warehouse.Invoices.Remove(invoice);
        }
    }
}
