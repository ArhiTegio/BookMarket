using Entities.DateBase.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DateBase
{
    public class Product: Identity
    {
        [Column(TypeName = "varchar(256)")]
        public string Author { get; set; }
        [Column(TypeName = "varchar(1024)")]
        public string Title { get; set; }
        public int YearPublication { get; set; }
        public int Quantity { get; set; }
    }
}
