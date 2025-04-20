using System;
using System.Collections.Generic;

namespace VMS.Models;

public partial class TblCodeMaster
{
    public int Id { get; set; }

    public string CodeType { get; set; } = null!;

    public string Code { get; set; } = null!;

    public string Description { get; set; } = null!;

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreationDate { get; set; }

    public int CreatedBy { get; set; }

    public DateTime UpdateDate { get; set; }

    public int UpdatedBy { get; set; }

    public virtual ICollection<TblExpenseMaster> TblExpenseMasters { get; set; } = new List<TblExpenseMaster>();

    public virtual ICollection<TblModelAverageMaster> TblModelAverageMasters { get; set; } = new List<TblModelAverageMaster>();

    public virtual ICollection<TblVehicleMaster> TblVehicleMasterBodyManufacturers { get; set; } = new List<TblVehicleMaster>();

    public virtual ICollection<TblVehicleMaster> TblVehicleMasterBodyTypes { get; set; } = new List<TblVehicleMaster>();

    public virtual ICollection<TblVehicleMaster> TblVehicleMasterMakes { get; set; } = new List<TblVehicleMaster>();

    public virtual ICollection<TblVehicleMaster> TblVehicleMasterVehicleTypes { get; set; } = new List<TblVehicleMaster>();
}
