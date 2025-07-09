using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MiFloraBack.Models
{
    public class Contractor
    {
        [Key] public Guid ContractorId { get; set; }
        [Required] public string Name { get; set; } = null!;
        public ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
    }

    public class Invoice
    {
        [Key] public string InvoiceId { get; set; } = null!;
        [Required] public DateTime Date { get; set; }
        
        [Required] public Guid ContractorId { get; set; }
        public Contractor Contractor { get; set; } = null!;
        
        [Required] public Guid BranchId { get; set; }
        public Branch Branch { get; set; } = null!;
        
        public ICollection<InvoiceItem> Items { get; set; } = new List<InvoiceItem>();
    }

    public class InvoiceItem
    {
        [Key] public Guid InvoiceItemId { get; set; }
        
        [Required] public string InvoiceId { get; set; } = null!;
        public Invoice Invoice { get; set; } = null!;
        
        [Required] public Guid ProductId { get; set; }
        public Product Product { get; set; } = null!;
        
        [Required] public int Quantity { get; set; }
        [Required] public string Unit { get; set; } = null!;
        [Required] public decimal PurchasePrice { get; set; }
        [Required] public decimal Total { get; set; }
    }
}