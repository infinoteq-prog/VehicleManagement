using System;
using System.Collections.Generic;
using VMS.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using VMS.Helper;
using VMS.ViewModel;

namespace VMS.ViewModel
{
    public class VMDriverMaster : BaseModel
    {
        public VMDriverMaster()
        {
            TransactionMessage = new TransactionMessageVM();
            CreatedOn = utilityHelper.CurrentDateTime;
            UpdatedOn = utilityHelper.CurrentDateTime;
        }

        public int Id { get; set; }

        public string DriverName { get; set; } = null!;

        public string FatherName { get; set; } = null!;

        public string DriverAddress1 { get; set; } = null!;

        public string DriverAddress2 { get; set; } = null!;

        public string DriverAddress3 { get; set; } = null!;

        public int CityId { get; set; }

        public int DistrictId { get; set; }

        public int StateId { get; set; }

        public string PinCode { get; set; } = null!;

        public string DriverPhoto { get; set; }

        public string AadharNo { get; set; } = null!;

        public string AadharNoImage { get; set; }

        public string PanNo { get; set; } = null!;

        public string PanNoImage { get; set; }

        public string BankName { get; set; } = null!;

        public string BankAccountNumber { get; set; } = null!;

        public string BankIFSCCode { get; set; } = null!;

        public string DrivingLicenceNo { get; set; } = null!;

        public string DrivingLicenceIssueAuth { get; set; } = null!;

        public string DrivingLicenceValidity { get; set; }

        public string DrivingLicenceImage { get; set; }

        public string MobileNumber1 { get; set; } = null!;

        public string MobileNumber2 { get; set; } = null!;

        public bool IsExistingReference { get; set; }

        public string ReferenceName { get; set; }

        public string ReferenceAddress1 { get; set; }
        public string ReferenceAddress2 { get; set; }

        public string ReferenceAddress3 { get; set; }

        public string ReferenceCity { get; set; }

        public string ReferencePinCode { get; set; }

        public string ReferenceMobile { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreationDate { get; set; }

        public int CreatedBy { get; set; }

        public DateTime UpdateDate { get; set; }

        public int UpdatedBy { get; set; }

        public string CityName { get; set; }

        public string DistrictName { get; set; }

        public string StateName { get; set; }

        public string CreatedByName { get; set; }

        public string UpdatedByName { get; set; }
        public string OldFirm { get; set; }
        public string Remark { get; set; }

    }
}
