using System;
using System.Collections.Generic;
using VMS.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using VMS.Helper;
using VMS.ViewModel;

namespace VMS.ViewModel 
{
    public class VMModelMaster : BaseModel
    {
        public VMModelMaster()
        {
            TransactionMessage = new TransactionMessageVM();
            CreatedOn = utilityHelper.CurrentDateTime;
            UpdatedOn = utilityHelper.CurrentDateTime;
        }

        public int Id { get; set; }

        public int MakeId { get; set; }

        public string MakeName { get; set; }

        public int ModelNo { get; set; }

        public int NoOfTyres { get; set; }

        public string VehicleType { get; set; } = null!;

        public string FuleType { get; set; } = null!;

        public bool IsActive { get; set; }

        public DateTime CreationDate { get; set; }

        public int CreatedBy { get; set; }

        public DateTime UpdateDate { get; set; }

        public int UpdatedBy { get; set; }

        public string CreatedByName { get; set; }

        public string UpdatedByName { get; set; }
    }
}
