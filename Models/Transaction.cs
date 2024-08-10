using System;

namespace VendingMachineApp.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public ApplicationUser User { get; set; }
    }
}
