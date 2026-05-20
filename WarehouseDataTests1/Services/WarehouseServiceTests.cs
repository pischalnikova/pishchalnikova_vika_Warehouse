using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseData.Models;
using WarehouseData.Services;

namespace WarehouseData.Services.Tests
{
    [TestClass()]
    public class WarehouseServiceTests
    {
        // Add
        [TestMethod()]
        public void Add_ValidWarehouse_ReturnsTrue()
        {
            var ctx = new Context.ApplicationContext();
            var org = new Organization("ТестОрг");
            ctx.orgs.Add(org);
            var svc = new WarehouseService(ctx, org);

            var wh = new Warehouse("Склад А", "Улица 1", org.OrgId);

            bool result = svc.Add(wh);

            Assert.IsTrue(result);
            CollectionAssert.Contains(org.warehouses, wh);
        }

        [TestMethod()]
        public void Add_NullWarehouse_ReturnsFalse()
        {
            var ctx = new Context.ApplicationContext();
            var org = new Organization("ТестОрг");
            ctx.orgs.Add(org);
            var svc = new WarehouseService(ctx, org);

            bool result = svc.Add(null!);

            Assert.IsFalse(result);
        }

        [TestMethod()]
        public void Add_EmptyName_ReturnsFalse()
        {
            var ctx = new Context.ApplicationContext();
            var org = new Organization("ТестОрг");
            ctx.orgs.Add(org);
            var svc = new WarehouseService(ctx, org);
            var wh = new Warehouse("", "Адрес", org.OrgId);

            bool result = svc.Add(wh);

            Assert.IsFalse(result);
        }

        [TestMethod()]
        public void Add_DuplicateWarehouse_ReturnsFalse()
        {
            var ctx = new Context.ApplicationContext();
            var org = new Organization("ТестОрг");
            ctx.orgs.Add(org);
            var svc = new WarehouseService(ctx, org);
            var wh = new Warehouse("Склад А", "Улица 1", org.OrgId);
            svc.Add(wh);

            bool result = svc.Add(wh);

            Assert.IsFalse(result);
        }

        // Update
        [TestMethod()]
        public void Update_ExistingWarehouse_ReturnsTrue()
        {
            var ctx = new Context.ApplicationContext();
            var org = new Organization("ТестОрг");
            ctx.orgs.Add(org);
            var svc = new WarehouseService(ctx, org);
            var wh = new Warehouse("Склад Старый", "Адрес", org.OrgId);
            svc.Add(wh);

            var updated = new Warehouse("Склад Новый", "Новый адрес", org.OrgId);
            // тот же WhId
            var field = typeof(WarehouseData.Models.Warehouse)
                .GetProperty("WhId")!;
            field.SetValue(updated, wh.WhId);

            bool result = svc.Update(updated);

            Assert.IsTrue(result);
        }

        [TestMethod()]
        public void Update_NullWarehouse_ReturnsFalse()
        {
            var ctx = new Context.ApplicationContext();
            var org = new Organization("ТестОрг");
            ctx.orgs.Add(org);
            var svc = new WarehouseService(ctx, org);

            bool result = svc.Update(null!);

            Assert.IsFalse(result);
        }

        [TestMethod()]
        public void Update_EmptyName_ReturnsFalse()
        {
            var ctx = new Context.ApplicationContext();
            var org = new Organization("ТестОрг");
            ctx.orgs.Add(org);
            var svc = new WarehouseService(ctx, org);
            var wh = new Warehouse("", "Адрес", org.OrgId);

            bool result = svc.Update(wh);

            Assert.IsFalse(result);
        }

        // Delete 

        [TestMethod()]
        public void Delete_ExistingWarehouse_ReturnsTrue()
        {
            var ctx = new Context.ApplicationContext();
            var org = new Organization("ТестОрг");
            ctx.orgs.Add(org);
            var svc = new WarehouseService(ctx, org);
            var wh = new Warehouse("Склад Удалить", "Адрес", org.OrgId);
            svc.Add(wh);

            bool result = svc.Delete(wh);

            Assert.IsTrue(result);
            CollectionAssert.DoesNotContain(org.warehouses, wh);
        }

        [TestMethod()]
        public void Delete_NullWarehouse_ReturnsFalse()
        {
            var ctx = new Context.ApplicationContext();
            var org = new Organization("ТестОрг");
            ctx.orgs.Add(org);
            var svc = new WarehouseService(ctx, org);

            bool result = svc.Delete(null!);

            Assert.IsFalse(result);
        }

        [TestMethod()]
        public void Delete_WarehouseNotInOrg_ReturnsFalse()
        {
            var ctx = new Context.ApplicationContext();
            var org = new Organization("ТестОрг");
            ctx.orgs.Add(org);
            var svc = new WarehouseService(ctx, org);
            var wh = new Warehouse("Чужой склад", "Адрес", org.OrgId);

            bool result = svc.Delete(wh);

            Assert.IsFalse(result);
        }
    }
}