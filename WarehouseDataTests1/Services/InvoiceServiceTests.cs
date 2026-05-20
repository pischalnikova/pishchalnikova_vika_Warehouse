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
    public class InvoiceServiceTests
    {

        private Category _cat = new Category { Id = 1, Name = "Тест" };
        private Manufacturer _man = new Manufacturer { Id = 1, Name = "Завод" };
        private Supplier _sup = new Supplier { Id = 1, Name = "Снаб" };

        private Product MakeProduct(string article, int qty, decimal price = 10m) =>
            new Product
            {
                Article = article,
                Name = article,
                Unit = "шт",
                Price = price,
                StockQuantity = qty,
                Category = _cat,
                CategoryId = _cat.Id,
                Manufacturer = _man,
                ManufacturerId = _man.Id,
                Supplier = _sup,
                SupplierId = _sup.Id
            };

        private (ApplicationContext ctx, Warehouse wh, InvoiceService svc) CreateSetup()
        {
            var ctx = new ApplicationContext();
            var org = new Organization("ТестОрг");
            ctx.orgs.Add(org);
            var wh = new Warehouse("Склад", "Адрес", org.OrgId);
            org.warehouses.Add(wh);
            var svc = new InvoiceService(ctx, wh);
            return (ctx, wh, svc);
        }


        // Add 

        [TestMethod()]
        public void Add_ValidInvoice_ReturnsTrue()
        {
            var (_, wh, svc) = CreateSetup();
            var inv = new Invoice { Number = "INV-001", Type = InvType.Входящая };

            bool result = svc.Add(inv);

            Assert.IsTrue(result);
            CollectionAssert.Contains(wh.Invoices, inv);
        }

        [TestMethod()]
        public void Add_NullInvoice_ReturnsFalse()
        {
            var (_, _, svc) = CreateSetup();

            bool result = svc.Add(null!);

            Assert.IsFalse(result);
        }

        [TestMethod()]
        public void Add_EmptyNumber_ReturnsFalse()
        {
            var (_, _, svc) = CreateSetup();
            var inv = new Invoice { Number = "  ", Type = InvType.Входящая };

            bool result = svc.Add(inv);

            Assert.IsFalse(result);
        }

        // Update (согласование) 

        [TestMethod()]
        public void Update_IncomingInvoice_AddsProductsToWarehouse()
        {
            var (_, wh, svc) = CreateSetup();
            var inv = new Invoice { Number = "INV-IN", Type = InvType.Входящая };
            var prod = MakeProduct("P001", 10);
            
            inv.Products.Add(prod);
            svc.Add(inv);

            bool result = svc.Update(inv);

            Assert.IsTrue(result);
            Assert.AreEqual(InvStatus.Согласована, inv.Status);
            CollectionAssert.Contains(wh.products, prod);
        }

        [TestMethod()]
        public void Update_IncomingInvoice_IncrementsExistingStock()
        {
            var (_, wh, svc) = CreateSetup();
            wh.products.Add(MakeProduct("P001", 5));

            var inv = new Invoice { Number = "INV-IN2", Type = InvType.Входящая };
            inv.Products.Add(MakeProduct("P001", 10));
            svc.Add(inv);

            svc.Update(inv);

            Assert.AreEqual(15, wh.products.First(p => p.Article == "P001").StockQuantity);
        }

        [TestMethod()]
        public void Update_OutgoingInvoice_DeductsStock()
        {
            var (_, wh, svc) = CreateSetup();
            wh.products.Add(MakeProduct("P001", 20));

            var inv = new Invoice { Number = "INV-OUT", Type = InvType.Исходящая };
            inv.Products.Add(MakeProduct("P001", 8));
            svc.Add(inv);

            bool result = svc.Update(inv);

            Assert.IsTrue(result);
            Assert.AreEqual(12, wh.products.First(p => p.Article == "P001").StockQuantity);
        }

        [TestMethod()]
        public void Update_OutgoingInvoice_RemovesProductWhenStockZero()
        {
            var (_, wh, svc) = CreateSetup();
            wh.products.Add(MakeProduct("P001", 10));

            var inv = new Invoice { Number = "INV-OUT2", Type = InvType.Исходящая };
            var prod = MakeProduct("P001", 10);

            inv.Products.Add(prod);
            svc.Add(inv);

            svc.Update(inv);

            CollectionAssert.DoesNotContain(wh.products, prod);
        }

        [TestMethod()]
        public void Update_OutgoingInvoice_InsufficientStock_ReturnsFalse()
        {
            var (_, wh, svc) = CreateSetup();
            wh.products.Add(MakeProduct("P001", 5));

            var inv = new Invoice { Number = "INV-OUT3", Type = InvType.Исходящая };
            
            inv.Products.Add(MakeProduct("P001", 100));
            svc.Add(inv);

            bool result = svc.Update(inv);

            Assert.IsFalse(result);
            Assert.AreEqual(5, wh.products.First(p => p.Article == "P001").StockQuantity);
        }

        [TestMethod()]
        public void Update_AlreadyApprovedInvoice_ReturnsFalse()
        {
            var (_, _, svc) = CreateSetup();
            var inv = new Invoice { Number = "INV-DONE", Type = InvType.Входящая, Status = InvStatus.Согласована };
            inv.Products.Add(MakeProduct("P001", 5));

            bool result = svc.Update(inv);

            Assert.IsFalse(result);
        }

        [TestMethod()]
        public void Update_EmptyProducts_ReturnsFalse()
        {
            var (_, _, svc) = CreateSetup();
            var inv = new Invoice { Number = "INV-EMPTY", Type = InvType.Входящая };
            svc.Add(inv);

            bool result = svc.Update(inv);

            Assert.IsFalse(result);
        }

        // Delete 

        [TestMethod()]
        public void Delete_NotApprovedInvoice_ReturnsTrue()
        {
            var (_, wh, svc) = CreateSetup();
            var inv = new Invoice { Number = "INV-DEL", Type = InvType.Входящая };
            svc.Add(inv);

            bool result = svc.Delete(inv);

            Assert.IsTrue(result);
            CollectionAssert.DoesNotContain(wh.Invoices, inv);
        }

        [TestMethod()]
        public void Delete_ApprovedInvoice_ReturnsFalse()
        {
            var (_, wh, svc) = CreateSetup();
            var inv = new Invoice { Number = "INV-NODL", Type = InvType.Входящая, Status = InvStatus.Согласована };
            wh.Invoices.Add(inv);

            bool result = svc.Delete(inv);

            Assert.IsFalse(result);
        }

        [TestMethod()]
        public void Delete_NullInvoice_ReturnsFalse()
        {
            var (_, _, svc) = CreateSetup();

            bool result = svc.Delete(null!);

            Assert.IsFalse(result);
        }
    }
}