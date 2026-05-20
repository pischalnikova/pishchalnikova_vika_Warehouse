using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseData.Context;
using WarehouseData.Models;
using WarehouseData.Services;

namespace WarehouseData.Services.Tests
{
    [TestClass()]
    public class ProductServiceTests
    {
        
        private Category _cat = new Category { Id = 1, Name = "Тест" };
        private Manufacturer _man = new Manufacturer { Id = 1, Name = "Завод" };
        private Supplier _sup = new Supplier { Id = 1, Name = "Снаб" };

        private Product MakeProduct(string article) =>
            new Product
            {
                Article = article,
                Name = article,
                Unit = "шт",
                Price = 5m,
                StockQuantity = 10,
                Category = _cat,
                CategoryId = _cat.Id,
                Manufacturer = _man,
                ManufacturerId = _man.Id,
                Supplier = _sup,
                SupplierId = _sup.Id
            };

        private ProductService CreateService()
        {
            var ctx = new ApplicationContext();
            var inv = new Invoice { Number = "INV-TEST", Type = InvType.Входящая };
            return new ProductService(ctx, inv);
        }

        //Add

        [TestMethod()]
        public void Add_NewProduct_ReturnsTrue()
        {
            var svc = CreateService();
            var product = MakeProduct("A001");

            bool result = svc.Add(product);

            Assert.IsTrue(result);
            CollectionAssert.Contains(svc.Invoice.Products, product);
        }

        [TestMethod()]
        public void Add_DuplicateProduct_ReturnsFalse()
        {
            var svc = CreateService();
            var product = MakeProduct("A001");
            svc.Add(product);

            bool result = svc.Add(product);

            Assert.IsFalse(result);
        }

        // Update 

        [TestMethod()]
        public void Update_ExistingProduct_ReturnsTrue()
        {
            var svc = CreateService();
            var product = MakeProduct("A001");
            svc.Add(product);

            var updated = MakeProduct("A001");
            updated.Price = 99m;

            bool result = svc.Update(updated);

            Assert.IsTrue(result);
            Assert.AreEqual(99m, svc.Invoice.Products.First(p => p.Article == "A001").Price);
        }

        [TestMethod()]
        public void Update_NonExistingProduct_ReturnsFalse()
        {
            var svc = CreateService();
            var product = MakeProduct("NONEXIST");

            bool result = svc.Update(product);

            Assert.IsFalse(result);
        }

        // Delete 

        [TestMethod()]
        public void Delete_ExistingProduct_ReturnsTrue()
        {
            var svc = CreateService();
            var product = MakeProduct("A001");
            svc.Add(product);

            bool result = svc.Delete(product);

            Assert.IsTrue(result);
            CollectionAssert.DoesNotContain(svc.Invoice.Products, product);
        }

        [TestMethod()]
        public void Delete_NullProduct_ReturnsFalse()
        {
            var svc = CreateService();

            bool result = svc.Delete(null!);

            Assert.IsFalse(result);
        }

        [TestMethod()]
        public void Delete_ProductNotInInvoice_ReturnsFalse()
        {
            var svc = CreateService();
            var product = MakeProduct("GHOST");

            bool result = svc.Delete(product);

            Assert.IsFalse(result);
        }
    }
}