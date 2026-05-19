using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseData.Context;
using WarehouseData.Models;

namespace WarehouseData.Services
{
    public interface IWarehouseService
    {
        bool Add(Warehouse warehouse);
        bool Update(Warehouse warehouse);
        bool Delete(Warehouse warehouse);
    }
    public class WarehouseService : IWarehouseService
    {
        public ApplicationContext Context { get; set; }
        public Organization Org { get; private set; }

        public WarehouseService(ApplicationContext context, Organization org)
        {
            if (context != null && org != null)
            {
                Context = context;
                if (Context.orgs.Contains(org))
                    Org = org;
            }
        }

        public bool Add(Warehouse warehouse)
        {
            bool result = false;
            if (warehouse != null)
            {
                if (!String.IsNullOrEmpty(warehouse.WhName))
                {
                    if (!Org.warehouses.Contains(warehouse))
                    {
                        this.Org.warehouses.Add(warehouse);
                        result = true;
                    }
                }
            }
            return result;
        }

        public bool Update(Warehouse warehouse)
        {
            bool result = false;
            if (warehouse != null)
            {
                if (!String.IsNullOrEmpty(warehouse.WhName))
                {
                    Warehouse? target = Org.warehouses.FirstOrDefault(x => x.WhId == warehouse.WhId);
                    if (target != null)
                    {
                        int idx = Org.warehouses.IndexOf(target);
                        if (idx != -1)
                        {
                            Org.warehouses[idx] = warehouse;
                        }
                    }
                    result = true;
                }

            }
            return result;
        }

        public bool Delete(Warehouse warehouse)
        {
            bool result = false;
            if (warehouse != null)
            {
                if (Org.warehouses.Contains(warehouse))
                {
                    this.Org.warehouses.Remove(warehouse);
                    result = true;
                }
            }
            return result;
        }
    }
}
