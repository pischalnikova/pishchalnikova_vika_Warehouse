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
    public interface IProductService
    {
        bool Add(Product product);
        bool Update(Product product);
        bool Delete(Product product);
    }
    public class ProductService : IProductService
    {
        public ApplicationContext Context { get; set; }
        public Invoice Invoice { get; set; }

        public ProductService(ApplicationContext context, Invoice invoice)
        {
            Context = context;
            Invoice = invoice;
        }

        public bool Add(Product product)
        {
            bool result = false;
            if (!Invoice.Products.Contains(product))
            {
                Invoice.Products.Add(product);
                result = true;
            }
            return result;
        }

        public bool Update(Product product)
        {
            bool result = false;
            Product? target = Invoice.Products
                .FirstOrDefault(p => p.Article == product.Article);

            if (target != null)
            {
                int idx = Invoice.Products.IndexOf(target);

                if (idx != -1)
                {
                    Invoice.Products[idx] = product;
                    result = true;
                }
            }
            return result;
        }

        public bool Delete(Product product)
        {
            bool result = false;
            if (product != null)
            {
                if (Invoice.Products.Contains(product))
                {
                    Invoice.Products.Remove(product);
                    result = true;
                }
            }
            return result;
        }
    }
}
