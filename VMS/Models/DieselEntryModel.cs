namespace VMS.Models
{
    public class DieselEntryModel
    {
        public int DieselEntryID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal DieselRate { get; set; }
        public string Remarks { get; set; }
        public string Created_By { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
