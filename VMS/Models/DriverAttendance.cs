

using System.ComponentModel.DataAnnotations.Schema;

namespace VMS.Models
{
    public class DriverAttendanceModel
    {

        public int DriverChangeID { get; set; }
        public DateTime ChangeDate { get; set; }
        public int DriverID { get; set; }

        public string VehicleNo { get; set; }
        public string Remarks { get; set; }
        public string Status { get; set; }
        public string Created_By { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; } 
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal Salary { get; set; }

        public DateTime EndDate { get; set; }
        public string SalaryType { get; set; }
        public string DriverName { get; set; }

    }
}
