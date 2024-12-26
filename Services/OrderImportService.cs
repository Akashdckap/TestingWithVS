using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using P21_latest_template.Models;
using P21_latest_template.Services;
using System;
using System.Collections.Generic;
using System.IO;

namespace P21_latest_template.Services
{
    public interface IOrderImportService
    {
        IEnumerable<string> ImportOrder(string path, OrderImport model);
        bool ImportHdr(string filename, OrderImportHdr model);
        bool ImportLine(string filename, IEnumerable<OrderImportLine> lines);
        bool ImportRemittance(string filename, OrderImportRemittance model);
        bool ImportCardHolderAddress(string filename, OrderImportCardHolderAddress model);
        void Delete(string filename);
    }

    public class OrderImportService : IOrderImportService
    {
        protected readonly ILogger<OrderImportService> Logger;
        protected readonly IConfiguration Configuration;
        protected readonly IFileService HdrImportService;
        protected readonly IFileService LineImportService;
        protected readonly IFileService RemittanceImportService;
        protected readonly IFileService CardHolderAddressImportService;

        public OrderImportService(ILoggerFactory loggerFactory,
            IConfiguration configuration,
            IFileService hdrImportService,
            IFileService lineImportService,
            IFileService remImportService,
            IFileService addrImportService)
        {
            Logger = loggerFactory.CreateLogger<OrderImportService>();
            Configuration = configuration;
            HdrImportService = hdrImportService;
            LineImportService = lineImportService;
            RemittanceImportService = remImportService;
            CardHolderAddressImportService = addrImportService;
        }

        public virtual IEnumerable<string> ImportOrder(string path, OrderImport model)
        {
            var results = new List<string>();
            string timestamp = DateTime.Now.ToString("MMddyyyyhhmm");

            // create hdr
            string hdrFile = Path.Combine(path, $"{Configuration["OrderImport:HdrPrefix"]}_{model.Hdr.ImportSetNo}_{timestamp}.tmp");
            var createdHdr = ImportHdr(hdrFile, model.Hdr);

            if (!createdHdr)
            {
                Logger.LogError("unable to create order hdr import file");
                return null;
            }
            results.Add(hdrFile);

            // create line
            string lineFile = Path.Combine(path, $"{Configuration["OrderImport:LinePrefix"]}_{model.Hdr.ImportSetNo}_{timestamp}.tmp");
            var createdLine = ImportLine(lineFile, model.Lines);

            if (!createdLine)
            {
                Logger.LogError("unable to create order line import file");
                results.ForEach(file => Delete(file));
                return null;
            }
            results.Add(lineFile);

            // create remittance
            if (model.Remittance != null)
            {
                string remFile = Path.Combine(path, $"{Configuration["OrderImport:RemittancePrefix"]}_{model.Hdr.ImportSetNo}_{timestamp}.tmp");
                var createdRem = ImportRemittance(remFile, model.Remittance);

                if (!createdRem)
                {
                    Logger.LogError("unable to create order remittance import file");
                    results.ForEach(file => Delete(file));
                    return null;
                }
                results.Add(remFile);
            }

            // create card holder address
            if (model.CardHolderAddress != null)
            {
                string addrFile = Path.Combine(path, $"{Configuration["OrderImport:CardHolderAddressPrefix"]}_{model.Hdr.ImportSetNo}_{timestamp}.tmp");
                var createdAddr = ImportCardHolderAddress(addrFile, model.CardHolderAddress);

                if (!createdAddr)
                {
                    Logger.LogError("unable to create order card holder address import file");
                    results.ForEach(file => Delete(file));
                    return null;
                }
                results.Add(addrFile);
            }

            return results;
        }

        public virtual bool ImportHdr(string filename, OrderImportHdr model)
        {
            bool succeeded = false;

            try
            {
                const int COUNT = 63;
                HdrImportService.ColumnCount = COUNT;

                string[] data = new string[COUNT];
                data[0] = model.ImportSetNo;
                data[1] = model.CustomerId.ToString("G29");
                data[2] = model.CustomerName;
                data[3] = model.CompanyId;
                data[4] = model.SalesLocationId.ToString("G29");
                data[5] = model.CustomerPoNo ?? "";
                data[6] = model.ContactId;
                data[7] = model.ContactName ?? "";
                data[8] = model.Taker;
                data[9] = model.JobName ?? "";
                data[10] = "";
                data[11] = "";
                data[12] = model.Quote.HasValue ? model.Quote.ToString() : "";
                data[13] = model.Approved.HasValue ? model.Approved.ToString() : "";
                data[14] = model.ShipToId.ToString("G29");
                data[15] = model.ShipToName ?? "";
                data[16] = model.ShipToAddress1 ?? "";
                data[17] = model.ShipToAddress2 ?? "";
                data[18] = model.ShipToCity ?? "";
                data[19] = model.ShipToState ?? "";
                data[20] = model.ShipToZipCode ?? "";
                data[21] = model.ShipToCountry ?? "";
                data[22] = model.SourceLocationId.HasValue ? model.SourceLocationId.Value.ToString("G29") : "";
                data[23] = model.CarrierId.HasValue ? model.CarrierId.Value.ToString("G29") : "";
                data[24] = model.CarrierName ?? "";
                data[25] = model.Route ?? "";
                data[26] = model.PackingBasis ?? "";
                data[27] = model.DeliveryInstructions ?? "";
                data[28] = model.Terms ?? "";
                data[29] = model.TermsDesc ?? "";
                data[30] = model.WillCall.HasValue ? model.WillCall.ToString() : "";
                data[31] = model.Class1 ?? "";
                data[32] = model.Class2 ?? "";
                data[33] = model.Class3 ?? "";
                data[34] = model.Class4 ?? "";
                data[35] = model.Class5 ?? "";
                data[36] = "";
                data[37] = model.FreightCode ?? "";
                data[38] = "";
                data[39] = model.CaptureUsageDefault.HasValue ? model.CaptureUsageDefault.ToString() : "";
                data[40] = model.Allocate.HasValue ? model.Allocate.ToString() : "";
                data[41] = "";
                data[42] = "";
                data[43] = model.ShipToEmail ?? "";
                data[44] = "";
                data[45] = model.ShipToPhone ?? "";
                data[46] = "";
                data[47] = "";
                data[48] = model.QuoteExpirationDate.HasValue ? model.QuoteExpirationDate.Value.ToString() : "";
                data[49] = "";
                data[50] = model.ImportAsQuote.HasValue ? model.ImportAsQuote.ToString() : "";
                data[51] = model.QuoteNumber ?? "";
                data[52] = model.WebReferenceNumber ?? "";
                data[53] = model.CreateInvoice.HasValue ? model.CreateInvoice.ToString() : "";
                data[54] = "";
                data[55] = "";
                data[56] = model.OrderTypePriority ?? "";
                data[57] = model.UpsCode ?? "";
                data[58] = "";
                data[59] = "";
                data[60] = model.PlacedByName ?? "";
                data[61] = "";
                data[62] = model.FreightOut.HasValue ? model.FreightOut.Value.ToString() : "";

                HdrImportService.AppendRow(data);
                HdrImportService.ExportFile(filename);
                succeeded = File.Exists(filename);
            }
            catch (Exception ex)
            {
                succeeded = false;
                Logger.LogError("Unable to create Order Hdr import file");
                Logger.LogError(ex.Message, filename, model);
            }
            return succeeded;
        }

        public virtual bool ImportLine(string filename, IEnumerable<OrderImportLine> lines)
        {
            bool succeeded = false;

            try
            {
                const int COUNT = 37;
                LineImportService.ColumnCount = COUNT;

                string[] data = new string[COUNT];
                foreach (var line in lines)
                {
                    data[0] = line.ImportSetNo;
                    data[1] = line.LineNo.ToString("G29");
                    data[2] = line.ItemID;
                    data[3] = line.UnitQuantity.ToString("G29");
                    data[4] = line.UnitOfMeasure;
                    data[5] = line.UnitPrice.HasValue ? line.UnitPrice.Value.ToString("G29") : "";
                    data[6] = "";
                    data[7] = line.SourceLocationID.HasValue ? line.SourceLocationID.Value.ToString("G29") : "";
                    data[8] = line.ShipLocationID.HasValue ? line.ShipLocationID.Value.ToString("G29") : "";
                    data[9] = "";
                    data[10] = "";
                    data[11] = "";
                    data[12] = "";
                    data[13] = "";
                    data[14] = line.WillCall.HasValue ? line.WillCall.ToString() : "";
                    data[15] = line.TaxItem.HasValue ? line.TaxItem.ToString() : "";
                    data[16] = "";
                    data[17] = "";
                    data[18] = "";
                    data[19] = "";
                    data[20] = "";
                    data[21] = line.Disposition.HasValue ? line.Disposition.ToString() : "";
                    data[22] = "";
                    data[23] = line.ManualPriceOverride.HasValue ? line.ManualPriceOverride.ToString() : "";
                    data[24] = "";
                    data[25] = "";
                    data[26] = line.CaptureUsage.HasValue ? line.CaptureUsage.ToString() : "";
                    data[27] = "";
                    data[28] = "";
                    data[29] = "";
                    data[30] = "";
                    data[31] = "";
                    data[32] = "";
                    data[33] = "";
                    data[34] = "";
                    data[35] = "";
                    data[36] = "";

                    LineImportService.AppendRow(data);
                }

                LineImportService.ExportFile(filename);
                succeeded = File.Exists(filename);
            }
            catch (Exception ex)
            {
                succeeded = false;
                Logger.LogError("Unable to create Order Lines import file");
                Logger.LogError(ex.Message, filename, lines);
            }
            return succeeded;
        }

        public virtual bool ImportRemittance(string filename, OrderImportRemittance model)
        {
            bool succeeded = false;

            try
            {
                const int COUNT = 16;
                RemittanceImportService.ColumnCount = COUNT;

                string[] data = new string[COUNT];

                data[0] = model.ImportSetNo;
                data[1] = model.PaymentTypeId.ToString();
                data[2] = model.PaymentAmount.ToString();
                data[3] = model.PaymentDate.HasValue ? model.PaymentDate.Value.ToShortDateString() : "";
                data[4] = model.PaymentDescription;
                data[5] = model.CheckNumber;
                data[6] = model.CCName;
                data[7] = model.CCNumber;
                data[8] = model.CustomerVerificationValue;
                data[9] = model.CCExpirationDate.HasValue ? model.CCExpirationDate.Value.ToShortDateString() : "";
                data[10] = model.CCAuthorizedDate.HasValue ? model.CCAuthorizedDate.Value.ToShortDateString() : "";
                data[11] = model.CCAuthorizedNumber.ToString();
                data[12] = model.Period.HasValue ? model.Period.ToString() : "";
                data[13] = model.Year.HasValue ? model.Year.ToString() : "";
                data[14] = model.CreditNumber;
                data[15] = model.WebProcessingCenterId;

                RemittanceImportService.AppendRow(data);
                RemittanceImportService.ExportFile(filename);
                succeeded = File.Exists(filename);
            }
            catch (Exception ex)
            {
                succeeded = false;
                Logger.LogError("Unable to create Order Remittance import file");
                Logger.LogError(ex.Message, filename, model);
            }
            return succeeded;
        }

        public virtual bool ImportCardHolderAddress(string filename, OrderImportCardHolderAddress model)
        {
            bool succeeded = false;

            try
            {
                const int COUNT = 14;
                LineImportService.ColumnCount = COUNT;

                string[] data = new string[COUNT];

                data[0] = model.ImportSetNo;
                data[1] = model.CardNumber;
                data[2] = model.FirstName;
                data[3] = model.LastName;
                data[4] = model.Street1;
                data[5] = model.Street2;
                data[6] = model.City;
                data[7] = model.State;
                data[8] = model.Zip;
                data[9] = model.SwitchIssueNumber;
                data[10] = model.ReferenceNumber;
                data[11] = model.TransactionId;
                data[12] = model.AvsResponseCode;
                data[13] = model.CvvResponseCode;

                LineImportService.AppendRow(data);
                LineImportService.ExportFile(filename);
                succeeded = File.Exists(filename);
            }
            catch (Exception ex)
            {
                succeeded = false;
                Logger.LogError("Unable to create Order Card Holder Address import file");
                Logger.LogError(ex.Message, filename, model);
            }
            return succeeded;
        }

        public virtual void Delete(string filename)
        {
            if (!File.Exists(filename))
            {
                Logger.LogDebug($"{filename} not found");
                return;
            }

            try
            {
                File.Delete(filename);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
            }
        }
    }
}
