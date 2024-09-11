using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess_Layer.Models
{
    public class Employee
    {
        public int Id { get; set; }
        [StringLength(maximumLength:50,MinimumLength =5)]
        public string Name { get; set; }
        [Range(15,70)]
        public int Age { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [DataType(DataType.Currency)]
        public decimal Salary { get; set; }
        [Phone]
        public string phone { get; set; }

        public bool IsActive { get; set; }



    }
}
