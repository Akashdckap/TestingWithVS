using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace P21_latest_template.Models
{
    public class OrderImport
    {
        [Required]
        public OrderImportHdr Hdr { get; set; }
        [Required]
        public IEnumerable<OrderImportLine> Lines { get; set; } = new List<OrderImportLine>();

        public OrderImportRemittance Remittance { get; set; }
        public OrderImportCardHolderAddress CardHolderAddress { get; set; }
    }

    public class OrderImportHdr
    {
        [Required]
        public string ImportSetNo { get; set; }
        [Required]
        public decimal CustomerId { get; set; }
        public string CustomerName { get; set; }
        [Required]
        public string CompanyId { get; set; }
        [Required]
        public decimal SalesLocationId { get; set; }
        public string CustomerPoNo { get; set; }
        public string ContactId { get; set; }
        public string ContactName { get; set; }
        [Required]
        public string Taker { get; set; }
        public string JobName { get; set; }
        public char? Quote { get; set; }
        public char? Approved { get; set; }
        [Required]
        public decimal ShipToId { get; set; }
        [Required]
        public string ShipToName { get; set; }
        public string ShipToAddress1 { get; set; }
        public string ShipToAddress2 { get; set; }
        public string ShipToAddress3 { get; set; }
        public string ShipToCity { get; set; }
        public string ShipToState { get; set; }
        public string ShipToZipCode { get; set; }
        public string ShipToCountry { get; set; }
        public decimal? SourceLocationId { get; set; }
        public decimal? CarrierId { get; set; }
        public string CarrierName { get; set; }
        public string Route { get; set; }
        public string PackingBasis { get; set; }
        public string DeliveryInstructions { get; set; }
        public string Terms { get; set; }
        public string TermsDesc { get; set; }
        public char? WillCall { get; set; }
        public string Class1 { get; set; }
        public string Class2 { get; set; }
        public string Class3 { get; set; }
        public string Class4 { get; set; }
        public string Class5 { get; set; }
        public string FreightCode { get; set; }
        public char? CaptureUsageDefault { get; set; }
        public char? Allocate { get; set; }
        public string ShipToEmail { get; set; }
        public string ShipToPhone { get; set; }
        public DateTime? QuoteExpirationDate { get; set; }
        public char? ImportAsQuote { get; set; }
        public string QuoteNumber { get; set; }
        public string WebReferenceNumber { get; set; }
        public char? CreateInvoice { get; set; }
        public string OrderTypePriority { get; set; }
        public string UpsCode { get; set; }
        public string PlacedByName { get; set; }
        public decimal? FreightOut { get; set; }
    }

    public class OrderImportLine
    {
        [Required]
        public string ImportSetNo { get; set; }
        [Required]
        public int LineNo { get; set; }
        [Required]
        public string ItemID { get; set; }
        [Required]
        public decimal UnitQuantity { get; set; }
        [Required]
        public string UnitOfMeasure { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? SourceLocationID { get; set; }
        public decimal? ShipLocationID { get; set; }
        public char? WillCall { get; set; }
        public char? TaxItem { get; set; }
        public char? Disposition { get; set; }
        public char? ManualPriceOverride { get; set; }
        public char? CaptureUsage { get; set; }
    }

    public class OrderImportRemittance
    {
        [Required]
        public string ImportSetNo { get; set; }
        [Required]
        public int PaymentTypeId { get; set; }
        [Required]
        public decimal PaymentAmount { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string PaymentDescription { get; set; }
        public string CheckNumber { get; set; }
        public string CCName { get; set; }
        public string CCNumber { get; set; }
        public string CustomerVerificationValue { get; set; }
        public DateTime? CCExpirationDate { get; set; }
        public DateTime? CCAuthorizedDate { get; set; }
        public int CCAuthorizedNumber { get; set; }
        public int? Period { get; set; }
        public int? Year { get; set; }
        public string CreditNumber { get; set; }
        public string WebProcessingCenterId { get; set; }
    }

    public class OrderImportCardHolderAddress
    {
        [Required]
        public string ImportSetNo { get; set; }
        [Required]
        public string CardNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Street1 { get; set; }
        public string Street2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string SwitchIssueNumber { get; set; }
        public string ReferenceNumber { get; set; }
        public string TransactionId { get; set; }
        public string AvsResponseCode { get; set; }
        public string CvvResponseCode { get; set; }
    }
}
