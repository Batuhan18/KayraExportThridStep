using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KayraExportThridStep.Core.Entities
{
    [Table("Products")]
    public class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal ProductPrice{ get; set; }
        public string ProductImageUrl{ get; set; }
    }
}
