using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using VMS.ViewModel;
using VMS.Helper;
using VMS.Models;

namespace VMS.ViewModel
{
    public class VMTransporterBill : BaseModel
    {
        public VMTransporterBill()
        {
            TransactionMessage = new TransactionMessageVM();
            CreatedOn = utilityHelper.CurrentDateTime;
            UpdatedOn = utilityHelper.CurrentDateTime;
        }

        public long Id { get; set; }

        public string BillNumber { get; set; } = null!;

        public string BillDate { get; set; }

        public string BillDateInString { get; set; }

        public int TransporterId { get; set; }

        public int userId { get; set; }

        public int CompanyId { get; set; }

        public decimal? SgstAmount { get; set; }

        public decimal? CgstAmount { get; set; }

        public decimal? IgstAmount { get; set; }

        public decimal? UgstAmount { get; set; }

        public decimal TotalBillAmount { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreationDate { get; set; }

        public int CreatedBy { get; set; }

        public DateTime UpdateDate { get; set; }

        public int UpdatedBy { get; set; }

        public string TransporterName { get; set; }

        public string CompanyName { get; set; }

        public string CreatedByName { get; set; }
    }
}
