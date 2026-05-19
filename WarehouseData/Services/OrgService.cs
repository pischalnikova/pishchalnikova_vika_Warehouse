using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseData.Context;
using WarehouseData.Models;

namespace WarehouseData.Services
{
    public interface IOrgService
    {
        public ApplicationContext GetContext();
        public bool Add(Organization org);
        public bool Update(Organization org);
        public bool Delete(Organization org);
    }
    public class OrgService : IOrgService
    {
        private ApplicationContext _context { get; set; }

        public OrgService(ApplicationContext context)
        {
            this._context = context;
        }

        public bool Add(Organization org)
        {
            bool result = false;
            if (org != null)
            {
                if (!String.IsNullOrEmpty(org.OrgName))
                {
                    if (!this._context.orgs.Contains(org))
                    {
                        this._context.orgs.Add(org);
                        result = true;
                    }
                }
            }
            return result;
        }

        public bool Update(Organization org)
        {
            bool result = false;
            if (org != null)
            {
                if (!String.IsNullOrEmpty(org.OrgName))
                {
                    Organization? target = _context.orgs.
                         FirstOrDefault(o => o.OrgId == org.OrgId);
                    if (target != null)
                    {
                        int id = _context.orgs.IndexOf(target);
                        if (id != -1) _context.orgs[id] = org;
                    }
                    result = true;
                }
            }
            return result;
        }

        public bool Delete(Organization org)
        {
            bool result = false;
            if (org != null)
            {
                if (!String.IsNullOrEmpty(org.OrgName))
                {
                    if (this._context.orgs.Contains(org))
                    {
                        this._context.orgs.Remove(org);
                        result = true;
                    }
                }
            }
            return result;
        }

        public ApplicationContext GetContext()
        {
            return _context;
        }
    }
}
