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
    public class OrgServiceTests
    {
        [TestMethod()]
        public void OrgServiceTest()
        {
            var ctx = new ApplicationContext();
            var svc = new OrgService(ctx);

            Assert.IsNotNull(svc);
            Assert.IsInstanceOfType(svc, typeof(OrgService));

        }

        [TestMethod()]
        public void Add_ValidOrg_ReturnsTrue()
        {
            var ctx = new ApplicationContext();
            var svc = new OrgService(ctx);
            var org = new Organization("ТестОрг");

            bool result = svc.Add(org);

            Assert.IsTrue(result);
            CollectionAssert.Contains(ctx.orgs, org);
        }

        [TestMethod()]
        public void Add_NullOrg_ReturnsFalse()
        {
            var svc = new OrgService(new ApplicationContext());

            bool result = svc.Add(null!);

            Assert.IsFalse(result);
        }

        [TestMethod()]
        public void Add_EmptyName_ReturnsFalse()
        {
            var svc = new OrgService(new ApplicationContext());
            var org = new Organization("");

            bool result = svc.Add(org);

            Assert.IsFalse(result);
        }

        [TestMethod()]
        public void Add_DuplicateOrg_ReturnsFalse()
        {
            var ctx = new ApplicationContext();
            var svc = new OrgService(ctx);
            var org = new Organization("Уникальная");
            svc.Add(org);

            bool result = svc.Add(org); // тот же объект повторно

            Assert.IsFalse(result);
        }

        [TestMethod()]
        public void Update_ExistingOrg_ReturnsTrue()
        {
            var ctx = new ApplicationContext();
            var svc = new OrgService(ctx);
            var org = new Organization("Старое название");
            svc.Add(org);

            org.OrgName = "Новое название";
            bool result = svc.Update(org);

            Assert.IsTrue(result);
        }

        [TestMethod()]
        public void Update_NullOrg_ReturnsFalse()
        {
            var svc = new OrgService(new ApplicationContext());

            bool result = svc.Update(null!);

            Assert.IsFalse(result);
        }

        [TestMethod()]
        public void Update_EmptyName_ReturnsFalse()
        {
            var ctx = new ApplicationContext();
            var svc = new OrgService(ctx);
            var org = new Organization("Корректное");
            svc.Add(org);
            org.OrgName = "";

            bool result = svc.Update(org);

            Assert.IsFalse(result);
        }

        [TestMethod()]
        public void Delete_ExistingOrg_ReturnsTrue()
        {
            var ctx = new ApplicationContext();
            var svc = new OrgService(ctx);
            var org = new Organization("УдалитьМеня");
            svc.Add(org);

            bool result = svc.Delete(org);

            Assert.IsTrue(result);
            CollectionAssert.DoesNotContain(ctx.orgs, org);
        }

        [TestMethod()]
        public void Delete_NullOrg_ReturnsFalse()
        {
            var svc = new OrgService(new ApplicationContext());

            bool result = svc.Delete(null!);

            Assert.IsFalse(result);
        }

        [TestMethod()]
        public void Delete_OrgNotInCollection_ReturnsFalse()
        {
            var svc = new OrgService(new ApplicationContext());
            var org = new Organization("Несуществующая");

            bool result = svc.Delete(org);

            Assert.IsFalse(result);
        }

        [TestMethod()]
        public void GetContext_ReturnsCorrectContext()
        {
            var ctx = new ApplicationContext();
            var svc = new OrgService(ctx);

            Assert.AreEqual(ctx, svc.GetContext());
        }
    }
}