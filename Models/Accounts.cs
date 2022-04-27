using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ABC_Bank.Models
{

	
	public class Accounts
	{


		[Key]
		public int AccountId { get; set; }
		public int AccountNumber { get; set; }
		[Column(TypeName = "decimal(18,4)")]
		public decimal Balance { get; set; }	
		public virtual ICollection<Transactions> Transactions { get; set; }
		public Customers MyCustomer { get; set; }
		public int MyCustomerId { get; set; }



		public Accounts()
		{
			Balance = 0;
		}
	}

	
}

