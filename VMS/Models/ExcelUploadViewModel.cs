namespace VMS.Models
{
    public class ExcelUploadViewModel
    {
        public IFormFile ExcelFile { get; set; }

        public bool IncludeFreight { get; set; }
        public bool IncludePartyBill { get; set; }
        public bool IncludeShortage { get; set; }


    }
}
