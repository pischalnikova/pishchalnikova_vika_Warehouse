using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseData.Models
{
    public enum InvType
    {
        Входящая = 1,
        Исходящая = 2
    }

    public enum InvStatus
    {
        НеСогласована = 1,
        Согласована = 2
    }
    [Table("invoices")]
    public class Invoice
    {
        public static BigInteger OrderCounter = BigInteger.Zero;

        [Key]
        [Column("id")]
        public BigInteger OrderId { get; set; }

        [Column("number")]
        public string Number { get; set; } = string.Empty;

        [Column("date")]
        public DateTime Date { get; set; }

        [Column("type")]
        public InvType Type { get; set; }

        [Column("status")]
        public InvStatus Status { get; set; }

        [Column("warehouse_id")]
        public BigInteger WarehouseId { get; set; }

        public ObservableCollection<Product> Products { get; set; }
            = new ObservableCollection<Product>();

        public Invoice()
        {
            OrderId = ++OrderCounter;
            Date = DateTime.Now;
            Status = InvStatus.НеСогласована;
        }
    }
}
