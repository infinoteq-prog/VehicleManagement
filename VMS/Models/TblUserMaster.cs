using System;
using System.Collections.Generic;

namespace VMS.Models;

public partial class TblUserMaster
{
    public int Id { get; set; }

    public string UserName { get; set; } = null!;

    public string EmailId { get; set; } = null!;

    public string MobileNo { get; set; } = null!;

    public string Password { get; set; } = null!;

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreationDate { get; set; }

    public int CreatedBy { get; set; }

    public DateTime UpdateDate { get; set; }

    public int UpdatedBy { get; set; }

    public int? RoleId { get; set; }

    public string? UserId { get; set; }

    public virtual TblRoleMaster? Role { get; set; }

    public virtual ICollection<TblTransporterMaster> TblTransporterMasters { get; set; } = new List<TblTransporterMaster>();

    public virtual ICollection<TblUserFunction> TblUserFunctions { get; set; } = new List<TblUserFunction>();

    public virtual ICollection<TblUserSubscription> TblUserSubscriptions { get; set; } = new List<TblUserSubscription>();
}
