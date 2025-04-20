using System;
using System.Collections.Generic;

namespace VMS.Models;

public partial class TblFunctionMaster
{
    public int Id { get; set; }

    public string FunctionName { get; set; } = null!;

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreationDate { get; set; }

    public int CreatedBy { get; set; }

    public DateTime UpdateDate { get; set; }

    public int UpdatedBy { get; set; }

    public virtual ICollection<TblUserFunction> TblUserFunctions { get; set; } = new List<TblUserFunction>();
}
