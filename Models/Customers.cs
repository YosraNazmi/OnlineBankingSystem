using System;
using System.Collections.Generic;
namespace ABC_Bank.Models
{

    public class Customers
    {
        public int Id { get; set; }
        public string Role { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string Email { get; set; }
        public DateTime DOB { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostCode { get; set; }
        public string Tell { get; set; }
        public string AccountType { get; set; }
        public string Password { get; set; }

        public virtual ICollection<Accounts> Accounts { get; set; }

    }

}

