using System;
using System.Collections.Generic;

namespace VMS.Models;

public partial class TblUserSubscription
{
    public int Id { get; set; }

    public string FinYear { get; set; } = null!;

    public int UserId { get; set; }

    public decimal Amount { get; set; }

    public DateTime PaidDate { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreationDate { get; set; }

    public int CreatedBy { get; set; }

    public DateTime UpdateDate { get; set; }

    public int UpdatedBy { get; set; }

    public virtual TblUserMaster User { get; set; } = null!;
}
