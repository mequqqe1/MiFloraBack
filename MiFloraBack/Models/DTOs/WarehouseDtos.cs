using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MiFloraBack.Models.DTOs
{
    public class ContractorDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
    }

    public class CreateContractorDto
    {
        [Required] public string Name { get; set; } = null!;
    }

    public class InvoiceProductDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public int AvailableStock { get; set; }
        public string Unit { get; set; } = null!;
    }

    public class CreateInvoiceItemDto
    {
        [Required] public Guid ProductId { get; set; }
        [Required] public int Quantity { get; set; }
        [Required] public string Unit { get; set; } = null!;
        [Required] public decimal PurchasePrice { get; set; }
    }

    public class CreateInvoiceDto
    {
        [Required] public string Id { get; set; } = null!;
        [Required] public DateTime Date { get; set; }
        [Required] public Guid ContractorId { get; set; }
        [Required] public Guid BranchId { get; set; }
        [Required] public List<CreateInvoiceItemDto> Items { get; set; } = new();
    }
}