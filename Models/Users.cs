using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BPITest.Models
{
    [Table("Users")]
    public class User
    {
        [Key]
        [DisplayName("รหัสพนักงาน")]
        public string ID { get; set; }

        [Required]
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        [DisplayName("Password")]
        public string Pwd { get; set; }
        public string Department { get; set; }
    }
}
