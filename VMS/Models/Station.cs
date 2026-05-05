namespace VMS.Models
{
    public class Station
    {
    }
}


using System;
using System.Collections.Generic;

namespace VMS.Models;

public partial class Station
{
    public int Id { get; set; }

    public string CityName { get; set; } = null!;

    public int? DistrictId { get; set; }

    public int? StateId { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreationDate { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? UpdateDate { get; set; }

    public int? UpdatedBy { get; set; }

    public virtual TblDistrict? District { get; set; }

    public virtual TblState? State { get; set; }

}
