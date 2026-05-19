using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WarehouseData.Models
{
    [Table("warehouses")]
    public class Warehouse
    {
        public static BigInteger WarehousesCounter = BigInteger.Zero;

        [Column("id")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public BigInteger WhId { get; set; }

        [Column("whname")]
        public string WhName { get; set; }

        [Column("whaddress")]
        public string WhAddress { get; set; }

        [Column("orgid")]
        public BigInteger OrgId { get; set; }

        [ForeignKey("OrgId")]
        Organization Organization { get; set; }

        public ObservableCollection<Invoice> Invoices { get; set; }
            = new ObservableCollection<Invoice>();

        public ObservableCollection<Product> products { get; set; }
            = new ObservableCollection<Product>();

        public Warehouse() { }

        public Warehouse(string name, string address, BigInteger orgid)
        {
            this.WhId = ++WarehousesCounter;
            this.WhName = name;
            this.WhAddress = address;
            this.OrgId = orgid;
        }
    }
}
