using System;
using System.Collections.Generic;

namespace VMS.Models;

public partial class TblExpenseMaster
{
    public int Id { get; set; }

    public int ExpType { get; set; }

    public string ExpCode { get; set; } = null!;

    public string? ExpOther { get; set; }

    public string ExpDescription { get; set; } = null!;

    public bool IsActive { get; set; }

    public DateTime CreationDate { get; set; }

    public int CreatedBy { get; set; }

    public DateTime UpdateDate { get; set; }

    public int UpdatedBy { get; set; }

    public virtual TblCodeMaster ExpTypeNavigation { get; set; } = null!;
}
