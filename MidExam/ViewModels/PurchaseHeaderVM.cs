using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace MidExam.ViewModels
{
    [NotMapped]
    public class PurchaseHeaderVM()
    {
        public PurchaseHeaderVM(string customerName, string customerPhoneNumber, string customerEmailAddress, string invoiceNumber, DateTime purchaseDate, decimal totalAmount) : this()
        {
            CustomerName = customerName;
            CustomerPhoneNumber = customerPhoneNumber;
            CustomerEmailAddress = customerEmailAddress;
            InvoiceNumber = invoiceNumber;
            PurchaseDate = purchaseDate;
            TotalAmount = totalAmount;
        }

        public PurchaseHeaderVM(int id, string customerName, string customerPhoneNumber, string customerEmailAddress, string invoiceNumber, DateTime purchaseDate, decimal totalAmount) : this(customerName, customerPhoneNumber, customerEmailAddress, invoiceNumber, purchaseDate, totalAmount)
        {
            Id = id;
        }

        public int Id { get; set; }

       
        public string? CustomerName { get; set; } = string.Empty;

        [ DataType(DataType.PhoneNumber)]
        public string? CustomerPhoneNumber { get; set; }

        [ DataType(DataType.EmailAddress)]
        public string? CustomerEmailAddress { get; set; }

        [ Display(Name = "Invoice Number")]
        public string? InvoiceNumber { get; set; }

        [AllowNull, DataType(DataType.Date), Column(TypeName = "DATE")]
        public DateTime PurchaseDate { get; set; }

        [AllowNull, Display(Name = "Total Amount")]
        public decimal TotalAmount { get; set; }

        [AllowNull]
        public ICollection<PurchaseDetailVM>? PurchaseDetails { get; set; } = new List<PurchaseDetailVM>();
    }
}

