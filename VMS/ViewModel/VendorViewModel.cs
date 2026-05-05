using System.ComponentModel.DataAnnotations;

namespace VMS.ViewModel
{
    public class VendorViewModel
    {
        public int VendorID { get; set; }

        [Required]
        public string? VendorCode { get; set; }

        [Required]
        public string? VendorType { get; set; }

        [Required]
        public string? VendorName { get; set; }

        [Required]
        public string? Address1 { get; set; }

        public string? Address2 { get; set; }
        public string? Address3 { get; set; }

        [Required]
        public string? City { get; set; }

        [Required]
        public string? District { get; set; }

        [Required]
        public string? State { get; set; }

        public string? Pin { get; set; }

        public string? GSTIN { get; set; }

        public string? PAN { get; set; }

       // [Required]
        public string? LRNoPrefix { get; set; }

       // [Required]
        public string? LRNoStart { get; set; }

        public string? ContactPerson { get; set; }

        public string? ContactMobileNo { get; set; }
    }
}
