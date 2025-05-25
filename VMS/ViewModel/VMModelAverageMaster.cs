using System;
using System.Collections.Generic;
using VMS.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using VMS.Helper;
using VMS.ViewModel;

namespace VMS.ViewModel
{
    public class VMModelAverageMaster : BaseModel
    {
        public VMModelAverageMaster()
        {
            TransactionMessage = new TransactionMessageVM();
            CreatedOn = utilityHelper.CurrentDateTime;
            UpdatedOn = utilityHelper.CurrentDateTime;
        }

        public int Id { get; set; }

        public int MakeId { get; set; }

        public string ModelNo { get; set; }

        public decimal UlAvg { get; set; }

        public decimal MegaHw { get; set; }

        public decimal Khali { get; set; }

        public decimal Nh { get; set; }

        public decimal OffRoad { get; set; }

        public decimal OverLoad { get; set; }

        public decimal? Other { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreationDate { get; set; }

        public int CreatedBy { get; set; }

        public DateTime UpdateDate { get; set; }

        public int UpdatedBy { get; set; }

        public virtual TblCodeMaster Make { get; set; } = null!;

        public string MakeName { get; set; }

        public string CreatedByName { get; set; }

        public string UpdatedByName { get; set; }
    }
}
