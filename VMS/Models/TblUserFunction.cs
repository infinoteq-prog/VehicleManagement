using System;
using System.Collections.Generic;

namespace VMS.Models;

public partial class TblUserFunction
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int RoleId { get; set; }

    public int FunctionId { get; set; }

    public string FunctionName { get; set; } = null!;

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreationDate { get; set; }

    public int CreatedBy { get; set; }

    public DateTime UpdateDate { get; set; }

    public int UpdatedBy { get; set; }

    public virtual TblFunctionMaster Function { get; set; } = null!;

    public virtual TblRoleMaster Role { get; set; } = null!;

    public virtual TblUserMaster User { get; set; } = null!;
}
