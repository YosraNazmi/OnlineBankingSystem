using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ABC_Bank.Models
{
	public class Transactions
	{
		

		[Key]
		public int TransactionsId { get; set; }
		public string TransactionName { get; set; }
		[Column(TypeName = "decimal(18,4)")]
		public decimal TransactionAmount { get; set; }
		public DateTime TransactionDate { get; set; }
		public int DestinationAccount { get; set; }

		public int AccountNumber { get; set; }
		public Accounts Account { get; set; }





	}
}

