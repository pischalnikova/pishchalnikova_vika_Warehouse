using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using WarehouseData.Models;

namespace WarehouseData.Context
{
    public class ApplicationContext
    {
        public ObservableCollection<Organization> orgs;

        public List<Category> Categories { get; set; }
        public List<Manufacturer> Manufacturers { get; set; }
        public List<Supplier> Suppliers { get; set; }

        public ApplicationContext()
        {
            orgs = new ObservableCollection<Organization>();

            OrgsFill();
        }

        public void OrgsFill()
        {
            Organization org1 = new Organization("Poгa и копыта, 000");
            Organization org2 = new Organization("Пупкин и сыновья, 000");

            Warehouse wh1 = new Warehouse("Склад Рогов и Копыт №1",
            "Tам, за лесом.", org1.OrgId);
            Warehouse wh2 = new Warehouse("Склад Рогов и Копыт №2",
            "Tам, за лесом.", org1.OrgId);
            Warehouse wh3 = new Warehouse("Склад Пупкина №1",
            "Там, за горой.", org2.OrgId);
            Warehouse wh4 = new Warehouse("Cклад Пупкина №2",
                "Taм, зa гopoй.", org2.OrgId);

            // Справочники
            // Категории
            Category catFood = new Category { Id = 1, Name = "Продукция" };
            Category catRaw = new Category { Id = 2, Name = "Сырье" };
            Category catTools = new Category { Id = 3, Name = "Инструменты" };
            Category catBuild = new Category { Id = 4, Name = "Стройматериалы" };

            Categories = new List<Category>
            {
                catFood,
                catRaw,
                catTools,
                catBuild
            };

            // Производители
            Manufacturer manFarm = new Manufacturer { Id = 1, Name = "Ферма Агро" };
            Manufacturer manLeather = new Manufacturer { Id = 2, Name = "КожПром" };
            Manufacturer manBosch = new Manufacturer { Id = 3, Name = "Bosch" };
            Manufacturer manKnauf = new Manufacturer { Id = 4, Name = "Knauf" };

            Manufacturers = new List<Manufacturer>
            {
                manFarm,
                manLeather,
                manBosch,
                manKnauf
            };

            // Поставщики
            Supplier supVillage = new Supplier { Id = 1, Name = "СельхозСнаб" };
            Supplier supIndustrial = new Supplier { Id = 2, Name = "ПромПоставка" };
            Supplier supBuild = new Supplier { Id = 3, Name = "СтройОпт" };

            Suppliers = new List<Supplier>
            {
                supVillage,
                supIndustrial,
                supBuild
            };


            wh1.products.Add(new Product
            {
                Article = "MILK001",
                Name = "Молоко фермерское",
                Unit = "л",
                Price = 2.30m,
                Category = catFood,
                CategoryId = catFood.Id,
                Manufacturer = manFarm,
                ManufacturerId = manFarm.Id,
                Supplier = supVillage,
                SupplierId = supVillage.Id,
                StockQuantity = 150
            });

            wh1.products.Add(new Product
            {
                Article = "MEAT001",
                Name = "Говядина охлажденная",
                Unit = "кг",
                Price = 8.90m,
                Category = catFood,
                CategoryId = catFood.Id,
                Manufacturer = manFarm,
                ManufacturerId = manFarm.Id,
                Supplier = supVillage,
                SupplierId = supVillage.Id,
                StockQuantity = 65
            });

            wh2.products.Add(new Product
            {
                Article = "LEATH001",
                Name = "Кожа натуральная",
                Unit = "м2",
                Price = 25.00m,
                Category = catRaw,
                CategoryId = catRaw.Id,
                Manufacturer = manLeather,
                ManufacturerId = manLeather.Id,
                Supplier = supIndustrial,
                SupplierId = supIndustrial.Id,
                StockQuantity = 40
            });

            wh2.products.Add(new Product
            {
                Article = "FEED001",
                Name = "Комбикорм",
                Unit = "мешок",
                Price = 14.20m,
                Category = catRaw,
                CategoryId = catRaw.Id,
                Manufacturer = manFarm,
                ManufacturerId = manFarm.Id,
                Supplier = supVillage,
                SupplierId = supVillage.Id,
                StockQuantity = 90
            });

            wh3.products.Add(new Product
            {
                Article = "HAM001",
                Name = "Молоток",
                Unit = "шт",
                Price = 12.50m,
                Category = catTools,
                CategoryId = catTools.Id,
                Manufacturer = manBosch,
                ManufacturerId = manBosch.Id,
                Supplier = supIndustrial,
                SupplierId = supIndustrial.Id,
                StockQuantity = 25
            });

            wh3.products.Add(new Product
            {
                Article = "DRILL001",
                Name = "Дрель ударная",
                Unit = "шт",
                Price = 95.00m,
                Category = catTools,
                CategoryId = catTools.Id,
                Manufacturer = manBosch,
                ManufacturerId = manBosch.Id,
                Supplier = supIndustrial,
                SupplierId = supIndustrial.Id,
                StockQuantity = 12
            });

            wh4.products.Add(new Product
            {
                Article = "CEM001",
                Name = "Цемент М500",
                Unit = "мешок",
                Price = 6.80m,
                Category = catBuild,
                CategoryId = catBuild.Id,
                Manufacturer = manKnauf,
                ManufacturerId = manKnauf.Id,
                Supplier = supBuild,
                SupplierId = supBuild.Id,
                StockQuantity = 220
            });

            wh4.products.Add(new Product
            {
                Article = "GYPS001",
                Name = "Гипсокартон",
                Unit = "лист",
                Price = 9.70m,
                Category = catBuild,
                CategoryId = catBuild.Id,
                Manufacturer = manKnauf,
                ManufacturerId = manKnauf.Id,
                Supplier = supBuild,
                SupplierId = supBuild.Id,
                StockQuantity = 55
            });

            Invoice invoice1 = new Invoice
            {
                Number = "RK-001",
                Type = InvType.Входящая,
                Status = InvStatus.НеСогласована,
                WarehouseId = wh1.WhId
            };

            invoice1.Products.Add(new Product
            {
                Article = "MILK002",
                Name = "Молоко ультрапастеризованное",
                Unit = "л",
                Price = 2.70m,
                Category = catFood,
                CategoryId = catFood.Id,
                Manufacturer = manFarm,
                ManufacturerId = manFarm.Id,
                Supplier = supVillage,
                SupplierId = supVillage.Id,
                StockQuantity = 50
            });

            invoice1.Products.Add(new Product
            {
                Article = "MEAT002",
                Name = "Фарш говяжий",
                Unit = "кг",
                Price = 6.50m,
                Category = catFood,
                CategoryId = catFood.Id,
                Manufacturer = manFarm,
                ManufacturerId = manFarm.Id,
                Supplier = supVillage,
                SupplierId = supVillage.Id,
                StockQuantity = 20
            });

            Invoice invoice2 = new Invoice
            {
                Number = "RK-002",
                Type = InvType.Исходящая,
                Status = InvStatus.Согласована,
                WarehouseId = wh1.WhId
            };

            invoice2.Products.Add(new Product
            {
                Article = "MILK001",
                Name = "Молоко фермерское",
                Unit = "л",
                Price = 2.30m,
                Category = catFood,
                CategoryId = catFood.Id,
                Manufacturer = manFarm,
                ManufacturerId = manFarm.Id,
                Supplier = supVillage,
                SupplierId = supVillage.Id,
                StockQuantity = 15
            });

            Invoice invoice3 = new Invoice
            {
                Number = "PS-001",
                Type = InvType.Входящая,
                Status = InvStatus.НеСогласована,
                WarehouseId = wh3.WhId
            };

            invoice3.Products.Add(new Product
            {
                Article = "HAM002",
                Name = "Кувалда",
                Unit = "шт",
                Price = 18.40m,
                Category = catTools,
                CategoryId = catTools.Id,
                Manufacturer = manBosch,
                ManufacturerId = manBosch.Id,
                Supplier = supIndustrial,
                SupplierId = supIndustrial.Id,
                StockQuantity = 10
            });

            invoice3.Products.Add(new Product
            {
                Article = "DRILL002",
                Name = "Шуруповерт",
                Unit = "шт",
                Price = 75.00m,
                Category = catTools,
                CategoryId = catTools.Id,
                Manufacturer = manBosch,
                ManufacturerId = manBosch.Id,
                Supplier = supIndustrial,
                SupplierId = supIndustrial.Id,
                StockQuantity = 8
            });

            Invoice invoice4 = new Invoice
            {
                Number = "PS-002",
                Type = InvType.Исходящая,
                Status = InvStatus.Согласована,
                WarehouseId = wh4.WhId
            };

            invoice4.Products.Add(new Product
            {
                Article = "CEM001",
                Name = "Цемент М500",
                Unit = "мешок",
                Price = 6.80m,
                Category = catBuild,
                CategoryId = catBuild.Id,
                Manufacturer = manKnauf,
                ManufacturerId = manKnauf.Id,
                Supplier = supBuild,
                SupplierId = supBuild.Id,
                StockQuantity = 30
            });

            wh1.Invoices.Add(invoice1);
            wh2.Invoices.Add(invoice2);

            wh3.Invoices.Add(invoice3);
            wh4.Invoices.Add(invoice4);

            org1.warehouses.Add(wh1);
            org1.warehouses.Add(wh2);
            org2.warehouses.Add(wh3);
            org2.warehouses.Add(wh4);

            orgs.Add(org1);
            orgs.Add(org2);


        }
    }
}
