namespace VMS.Models
{
    public class TripSummaryModel
    {
        public int ExpenseID { get; set; }
        public string VehicleNo { get; set; }
        public string FromLocation { get; set; }
        public string ToLocation { get; set; }
        public decimal Weight { get; set; }
        public string Material { get; set; }
        public decimal EmptyKM { get; set; }
        public decimal LoadKM { get; set; }
        public decimal FreightRate { get; set; }
        public decimal FreightAmt { get; set; }
        public decimal ExpAmt { get; set; }
        public decimal PnL { get; set; }
    }
}
