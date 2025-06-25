using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace VMS.Models;

public partial class VmsDbContext : DbContext
{
    public VmsDbContext()
    {
    }

    public VmsDbContext(DbContextOptions<VmsDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TblAssetMaster> TblAssetMasters { get; set; }

    public virtual DbSet<TblBillToMaster> TblBillToMasters { get; set; }

    public virtual DbSet<TblCity> TblCities { get; set; }

    public virtual DbSet<TblCodeMaster> TblCodeMasters { get; set; }

    public virtual DbSet<TblCountryMaster> TblCountryMasters { get; set; }

    public virtual DbSet<TblDieselFilling> TblDieselFillings { get; set; }

    public virtual DbSet<TblDieselHeader> TblDieselHeaders { get; set; }

    public virtual DbSet<TblDieselLine> TblDieselLines { get; set; }

    public virtual DbSet<TblDieselRouteMaster> TblDieselRouteMasters { get; set; }

    public virtual DbSet<TblDispatchDetail> TblDispatchDetails { get; set; }

    public virtual DbSet<TblDistanceMaster> TblDistanceMasters { get; set; }

    public virtual DbSet<TblDistrict> TblDistricts { get; set; }

    public virtual DbSet<TblDriverHisabHeader> TblDriverHisabHeaders { get; set; }

    public virtual DbSet<TblDriverHisabLine> TblDriverHisabLines { get; set; }

    public virtual DbSet<TblDriverMaster> TblDriverMasters { get; set; }

    public virtual DbSet<TblExpenseMaster> TblExpenseMasters { get; set; }

    public virtual DbSet<TblFunctionMaster> TblFunctionMasters { get; set; }

    public virtual DbSet<TblGstMaster> TblGstMasters { get; set; }

    public virtual DbSet<TblGstRateMaster> TblGstRateMasters { get; set; }

    public virtual DbSet<TblMakeMaster> TblMakeMasters { get; set; }

    public virtual DbSet<TblModelAverageMaster> TblModelAverageMasters { get; set; }

    public virtual DbSet<TblModelMaster> TblModelMasters { get; set; }

    public virtual DbSet<TblRoleMaster> TblRoleMasters { get; set; }

    public virtual DbSet<TblState> TblStates { get; set; }

    public virtual DbSet<TblTempDispatchDetail> TblTempDispatchDetails { get; set; }

    public virtual DbSet<TblTransporterBill> TblTransporterBills { get; set; }

    public virtual DbSet<TblTransporterMaster> TblTransporterMasters { get; set; }

    public virtual DbSet<TblUserFunction> TblUserFunctions { get; set; }

    public virtual DbSet<TblUserMaster> TblUserMasters { get; set; }

    public virtual DbSet<TblUserSubscription> TblUserSubscriptions { get; set; }

    public virtual DbSet<TblVehicleMaster> TblVehicleMasters { get; set; }
    public virtual DbSet<TblServiceDueMaster> TblServiceDueMaster { get; set; }
    public virtual DbSet<TblServiceCompletion> TblServiceCompletion { get; set; }

    //    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
    //        => optionsBuilder.UseSqlServer("Data Source=SQL1003.net;Initial Catalog=db_ab27dd;User Id=db_ab27dd;Password=Chang;TrustServerCertificate=True");

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            // Get the current environment
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";

            // Build the configuration
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environmentName}.json", optional: true) // Environment-specific settings
                .Build();

            // Get the connection string from the configuration
            var connectionString = configuration.GetConnectionString("VMSContext"); // Replace with your actual connection string name

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Connection string 'YourConnectionStringName' not found in appsettings.");
            }

            optionsBuilder.UseSqlServer(connectionString);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TblAssetMaster>(entity =>
        {
            entity.ToTable("tbl_Asset_Master");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Address).HasMaxLength(1000);
            entity.Property(e => e.Amc)
                .HasMaxLength(1000)
                .HasColumnName("AMC");
            entity.Property(e => e.BodyManufacturer)
                .HasMaxLength(500)
                .HasColumnName("Body_Manufacturer");
            entity.Property(e => e.BodyType)
                .HasMaxLength(500)
                .HasColumnName("Body_Type");
            entity.Property(e => e.ChasisNo)
                .HasMaxLength(500)
                .HasColumnName("Chasis_No");
            entity.Property(e => e.CreatedBy).HasColumnName("Created_By");
            entity.Property(e => e.CreationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Creation_Date");
            entity.Property(e => e.EngineNo)
                .HasMaxLength(500)
                .HasColumnName("Engine_No");
            entity.Property(e => e.FinancerName)
                .HasMaxLength(500)
                .HasColumnName("Financer_Name");
            entity.Property(e => e.FitnessDoc)
                .HasMaxLength(1000)
                .HasColumnName("Fitness_Doc");
            entity.Property(e => e.FitnessDue)
                .HasColumnType("datetime")
                .HasColumnName("Fitness_Due");
            entity.Property(e => e.InsuranceDoc)
                .HasMaxLength(1000)
                .HasColumnName("Insurance_Doc");
            entity.Property(e => e.InsuranceDue)
                .HasColumnType("datetime")
                .HasColumnName("Insurance_Due");
            entity.Property(e => e.IsActive).HasColumnName("Is_Active");
            entity.Property(e => e.LocalPermitDoc)
                .HasMaxLength(1000)
                .HasColumnName("Local_Permit_Doc");
            entity.Property(e => e.LocalPermitDue)
                .HasColumnType("datetime")
                .HasColumnName("Local_Permit_Due");
            entity.Property(e => e.MfgYear).HasColumnName("Mfg_Year");
            entity.Property(e => e.NationalPermitDoc)
                .HasMaxLength(1000)
                .HasColumnName("National_Permit_Doc");
            entity.Property(e => e.NationalPermitDue)
                .HasColumnType("datetime")
                .HasColumnName("National_Permit_Due");
            entity.Property(e => e.NoOfTyres).HasColumnName("No_Of_Tyres");
            entity.Property(e => e.PanNo)
                .HasMaxLength(200)
                .HasColumnName("Pan_No");
            entity.Property(e => e.PanNoImage)
                .HasMaxLength(1000)
                .HasColumnName("Pan_No_Image");
            entity.Property(e => e.PollutionDoc)
                .HasMaxLength(1000)
                .HasColumnName("Pollution_Doc");
            entity.Property(e => e.PollutionDue)
                .HasColumnType("datetime")
                .HasColumnName("Pollution_Due");
            entity.Property(e => e.PurchaseDate)
                .HasColumnType("datetime")
                .HasColumnName("Purchase_Date");
            entity.Property(e => e.RcDoc)
                .HasMaxLength(1000)
                .HasColumnName("RC_Doc");
            entity.Property(e => e.RcValidityDue)
                .HasColumnType("datetime")
                .HasColumnName("RC_Validity_Due");
            entity.Property(e => e.RtoDoc)
                .HasMaxLength(1000)
                .HasColumnName("RTO_Doc");
            entity.Property(e => e.RtoDue)
                .HasColumnType("datetime")
                .HasColumnName("RTO_Due");
            entity.Property(e => e.RunningKm)
                .HasDefaultValue(0)
                .HasColumnName("Running_Km");
            entity.Property(e => e.UpdateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Update_Date");
            entity.Property(e => e.UpdatedBy).HasColumnName("Updated_By");
            entity.Property(e => e.VehicleClassification)
                .HasMaxLength(200)
                .HasColumnName("Vehicle_Classification");
            entity.Property(e => e.VehicleNo)
                .HasMaxLength(500)
                .HasColumnName("Vehicle_No");
            entity.Property(e => e.VehicleOwner)
                .HasMaxLength(2000)
                .HasColumnName("Vehicle_Owner");
            entity.Property(e => e.VehicleType)
                .HasMaxLength(500)
                .HasColumnName("Vehicle_Type");

            entity.HasOne(d => d.Make).WithMany(p => p.TblAssetMasters)
                .HasForeignKey(d => d.MakeId)
                .HasConstraintName("FK_tbl_Asset_Master_tbl_Make_Master");

            entity.HasOne(d => d.Model).WithMany(p => p.TblAssetMasters)
                .HasForeignKey(d => d.ModelId)
                .HasConstraintName("FK_tbl_Asset_Master_tbl_Model_Master");
        });

        modelBuilder.Entity<TblBillToMaster>(entity =>
        {
            entity.ToTable("tbl_Bill_To_Master");

            entity.HasIndex(e => e.BillToCode, "Index_Bill_To_Master_BillTo_Code");

            entity.HasIndex(e => e.CityId, "Index_Bill_To_Master_City_ID");

            entity.HasIndex(e => e.DistrictId, "Index_Bill_To_Master_District_ID");

            entity.HasIndex(e => e.EmailId, "Index_Bill_To_Master_Email_Id");

            entity.HasIndex(e => e.GstinNo, "Index_Bill_To_Master_GSTIN_No");

            entity.HasIndex(e => e.MobileNo, "Index_Bill_To_Master_Mobile_No");

            entity.HasIndex(e => e.PanNo, "Index_Bill_To_Master_Pan_No");

            entity.HasIndex(e => e.StartDate, "Index_Bill_To_Master_Start_Date");

            entity.HasIndex(e => e.StateId, "Index_Bill_To_Master_State_ID");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Address1).HasMaxLength(2000);
            entity.Property(e => e.Address2).HasMaxLength(2000);
            entity.Property(e => e.Address3).HasMaxLength(2000);
            entity.Property(e => e.BillToCode)
                .HasMaxLength(200)
                .HasColumnName("BillTo_Code");
            entity.Property(e => e.BillToCompany)
                .HasMaxLength(500)
                .HasColumnName("BillTo_Company");
            entity.Property(e => e.CityId).HasColumnName("City_ID");
            entity.Property(e => e.CreatedBy).HasColumnName("Created_By");
            entity.Property(e => e.CreationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Creation_Date");
            entity.Property(e => e.DistrictId).HasColumnName("District_ID");
            entity.Property(e => e.EmailId)
                .HasMaxLength(500)
                .HasColumnName("Email_Id");
            entity.Property(e => e.EndDate)
                .HasColumnType("datetime")
                .HasColumnName("End_Date");
            entity.Property(e => e.GstinNo)
                .HasMaxLength(200)
                .HasColumnName("GSTIN_No");
            entity.Property(e => e.IsActive).HasColumnName("Is_Active");
            entity.Property(e => e.MobileNo)
                .HasMaxLength(200)
                .HasColumnName("Mobile_No");
            entity.Property(e => e.PanNo)
                .HasMaxLength(200)
                .HasColumnName("Pan_No");
            entity.Property(e => e.PinCode)
                .HasMaxLength(500)
                .HasColumnName("Pin_Code");
            entity.Property(e => e.StartDate)
                .HasColumnType("datetime")
                .HasColumnName("Start_Date");
            entity.Property(e => e.StateId).HasColumnName("State_ID");
            entity.Property(e => e.UpdateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Update_Date");
            entity.Property(e => e.UpdatedBy).HasColumnName("Updated_By");

            entity.HasOne(d => d.City).WithMany(p => p.TblBillToMasters)
                .HasForeignKey(d => d.CityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_Bill_To_Master_tbl_City");

            entity.HasOne(d => d.District).WithMany(p => p.TblBillToMasters)
                .HasForeignKey(d => d.DistrictId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_Bill_To_Master_tbl_District");

            entity.HasOne(d => d.State).WithMany(p => p.TblBillToMasters)
                .HasForeignKey(d => d.StateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_Bill_To_Master_tbl_State");
        });

        modelBuilder.Entity<TblCity>(entity =>
        {
            entity.ToTable("tbl_City");

            entity.HasIndex(e => e.CityName, "Index_tbl_City_City_Name");

            entity.HasIndex(e => e.DistrictId, "Index_tbl_City_District_ID");

            entity.HasIndex(e => e.IsActive, "Index_tbl_City_Is_Active");

            entity.HasIndex(e => e.StateId, "Index_tbl_City_State_ID");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.CityName)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("City_Name");
            entity.Property(e => e.CreatedBy).HasColumnName("Created_By");
            entity.Property(e => e.CreationDate)
                .HasColumnType("datetime")
                .HasColumnName("Creation_Date");
            entity.Property(e => e.DistrictId)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("District_ID");
            entity.Property(e => e.IsActive)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("Is_Active");
            entity.Property(e => e.StateId)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("State_ID");
            entity.Property(e => e.UpdateDate)
                .HasColumnType("datetime")
                .HasColumnName("Update_Date");
            entity.Property(e => e.UpdatedBy).HasColumnName("Updated_By");

            entity.HasOne(d => d.District).WithMany(p => p.TblCities)
                .HasForeignKey(d => d.DistrictId)
                .HasConstraintName("FK_tbl_City_tbl_District");

            entity.HasOne(d => d.State).WithMany(p => p.TblCities)
                .HasForeignKey(d => d.StateId)
                .HasConstraintName("FK_tbl_City_tbl_State");
        });

        modelBuilder.Entity<TblCodeMaster>(entity =>
        {
            entity.ToTable("tbl_Code_Master");

            entity.HasIndex(e => e.EndDate, "Index_tbl_Code_End_Date");

            entity.HasIndex(e => e.IsActive, "Index_tbl_Code_Is_Active");

            entity.HasIndex(e => e.Code, "Index_tbl_Code_Master_Code");

            entity.HasIndex(e => e.CodeType, "Index_tbl_Code_Master_Code_Type");

            entity.HasIndex(e => e.StartDate, "Index_tbl_Code_Start_Date");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Code).HasMaxLength(500);
            entity.Property(e => e.CodeType)
                .HasMaxLength(500)
                .HasColumnName("Code_Type");
            entity.Property(e => e.CreatedBy).HasColumnName("Created_By");
            entity.Property(e => e.CreationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Creation_Date");
            entity.Property(e => e.Description).HasMaxLength(2000);
            entity.Property(e => e.EndDate)
                .HasColumnType("datetime")
                .HasColumnName("End_Date");
            entity.Property(e => e.IsActive).HasColumnName("Is_Active");
            entity.Property(e => e.StartDate)
                .HasColumnType("datetime")
                .HasColumnName("Start_Date");
            entity.Property(e => e.UpdateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Update_Date");
            entity.Property(e => e.UpdatedBy).HasColumnName("Updated_By");
        });

        modelBuilder.Entity<TblCountryMaster>(entity =>
        {
            entity.ToTable("tbl_CountryMaster");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CountryName)
                .HasMaxLength(2000)
                .HasColumnName("Country_Name");
            entity.Property(e => e.CreatedBy).HasColumnName("Created_By");
            entity.Property(e => e.CreationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Creation_Date");
            entity.Property(e => e.IsActive).HasColumnName("Is_Active");
            entity.Property(e => e.UpdateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Update_Date");
            entity.Property(e => e.UpdatedBy).HasColumnName("Updated_By");
        });

        modelBuilder.Entity<TblDieselFilling>(entity =>
        {
            entity.ToTable("tbl_Diesel_Filling");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedBy).HasColumnName("Created_By");
            entity.Property(e => e.CreationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Creation_Date");
            entity.Property(e => e.DieselFillingDate)
                .HasColumnType("datetime")
                .HasColumnName("Diesel_Filling_Date");
            entity.Property(e => e.DieselQty).HasColumnName("Diesel_Qty");
            entity.Property(e => e.VendorId).HasColumnName("VendorId");
            entity.Property(e => e.UpdateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Update_Date");
            entity.Property(e => e.UpdatedBy).HasColumnName("Updated_By");

            entity.HasOne(d => d.Trip).WithMany(p => p.TblDieselFillings)
                .HasForeignKey(d => d.TripId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_Diesel_Filling_tbl_Diesel_Header");
        });

        modelBuilder.Entity<TblDieselHeader>(entity =>
        {
            entity.HasKey(e => e.TripId);

            entity.ToTable("tbl_Diesel_Header");

            entity.Property(e => e.TripId).ValueGeneratedNever();
            entity.Property(e => e.CreatedBy).HasColumnName("Created_By");
            entity.Property(e => e.LastTripId).HasColumnName("Last_Trip_Id");
            entity.Property(e => e.CreationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Creation_Date");
            entity.Property(e => e.EndOdometer).HasColumnName("End_odometer");
            entity.Property(e => e.IsActive).HasColumnName("Is_Active");
            entity.Property(e => e.IsDifferenceAdded).HasColumnName("Is_DifferenceAdded");
            entity.Property(e => e.IsLoadingAdded).HasColumnName("Is_LoadingAdded");
            entity.Property(e => e.LastTripRouteDescr)
                .HasMaxLength(500)
                .HasColumnName("Last_Trip_route_descr");
            entity.Property(e => e.OpeningDiesel).HasColumnName("Opening_diesel");
            entity.Property(e => e.ClosingDiesel).HasColumnName("Closing_diesel");
            entity.Property(e => e.StartOdometer).HasColumnName("Start_odometer");
            entity.Property(e => e.ApprovedBy).HasColumnName("Approved_By");
            entity.Property(e => e.ApprovedDate)
                .HasColumnType("datetime")
                .HasColumnName("Approved_Date");
            entity.Property(e => e.TripEndDate)
                .HasColumnType("datetime")
                .HasColumnName("Trip_end_date");
            entity.Property(e => e.TripStartDate)
                .HasColumnType("datetime")
                .HasColumnName("Trip_start_date");
            entity.Property(e => e.UpdateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Update_Date");
            entity.Property(e => e.UpdatedBy).HasColumnName("Updated_By");
            entity.Property(e => e.Percent_Loss).HasColumnType("decimal(10, 3)");
            entity.Property(e => e.Profit_Loss).HasColumnType("decimal(10, 3)");
            entity.Property(e => e.Bhari_Ka_Average).HasColumnType("decimal(10, 3)");

            entity.HasOne(d => d.Driver).WithMany(p => p.TblDieselHeaders)
                .HasForeignKey(d => d.DriverId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_Diesel_Header_tbl_Driver_Master");

            entity.HasOne(d => d.VehicleNoNavigation).WithMany(p => p.TblDieselHeaders)
                .HasForeignKey(d => d.VehicleNo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_Diesel_Header_tbl_Vehicle_Master");
        });

        modelBuilder.Entity<TblDieselLine>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_[tbl_Diesel_Line");

            entity.ToTable("tbl_Diesel_Line");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedBy).HasColumnName("Created_By");
            entity.Property(e => e.CreationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Creation_Date");
            entity.Property(e => e.LoadType)
                .HasMaxLength(200)
                .HasColumnName("Load_Type");
            entity.Property(e => e.RouteDesc)
                .HasMaxLength(1000)
                .HasColumnName("Route_Desc");
            entity.Property(e => e.RouteId).HasColumnName("Route_ID");
            entity.Property(e => e.Average).HasColumnName("Average");
            entity.Property(e => e.Distance).HasColumnName("Distance");
            entity.Property(e => e.EstimatedDiesel).HasColumnName("Estimated_Diesel");
            entity.Property(e => e.UpdateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Update_Date");
            entity.Property(e => e.UpdatedBy).HasColumnName("Updated_By");

            entity.HasOne(d => d.Trip).WithMany(p => p.TblDieselLines)
                .HasForeignKey(d => d.TripId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_Diesel_Line_tbl_Diesel_Header");
        });

        modelBuilder.Entity<TblDieselRouteMaster>(entity =>
        {
            entity.ToTable("tbl_DieselRouteMaster");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedBy).HasColumnName("Created_By");
            entity.Property(e => e.CreationDate)
                .HasColumnType("datetime")
                .HasColumnName("Creation_Date");
            entity.Property(e => e.IsActive).HasColumnName("Is_Active");
            entity.Property(e => e.RouteDescription)
                .HasMaxLength(1000)
                .HasColumnName("Route_Description");
            entity.Property(e => e.UpdateDate)
                .HasColumnType("datetime")
                .HasColumnName("Update_Date");
            entity.Property(e => e.UpdatedBy).HasColumnName("Updated_By");
        });

        modelBuilder.Entity<TblDispatchDetail>(entity =>
        {
            entity.ToTable("tbl_Dispatch_Details");

            entity.HasIndex(e => e.BillNumber, "Index_Dispatch_Details_BillNumber");

            entity.HasIndex(e => e.DispatchUniqueId, "Index_Dispatch_Details_Dispatch_Unique_ID");

            entity.HasIndex(e => e.Division, "Index_Dispatch_Details_Division");

            entity.HasIndex(e => e.ForwardingAgentCode, "Index_Dispatch_Details_Forwarding_Agent_Code");

            entity.HasIndex(e => e.IncoTerm, "Index_Dispatch_Details_Inco_Term");

            entity.HasIndex(e => e.PgiDate, "Index_Dispatch_Details_Pgi_Date_Desc").IsDescending();

            entity.HasIndex(e => e.SupplyingPlant, "Index_Dispatch_Details_Supplying_Plant");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.BillDate)
                .HasColumnType("datetime")
                .HasColumnName("Bill_Date");
            entity.Property(e => e.BillNumber)
                .HasMaxLength(100)
                .HasColumnName("Bill_Number");
            entity.Property(e => e.CgstRate)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("Cgst_Rate");
            entity.Property(e => e.CreatedBy).HasColumnName("Created_By");
            entity.Property(e => e.CreationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Creation_Date");
            entity.Property(e => e.DeliveryNo)
                .HasMaxLength(200)
                .HasColumnName("Delivery_No");
            entity.Property(e => e.DispatchQtyRoad)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("Dispatch_Qty_Road");
            entity.Property(e => e.DispatchUniqueId)
                .HasMaxLength(100)
                .HasColumnName("Dispatch_Unique_ID");
            entity.Property(e => e.DistributionChannel)
                .HasMaxLength(10)
                .HasColumnName("Distribution_Channel");
            entity.Property(e => e.Division).HasMaxLength(500);
            entity.Property(e => e.EbidFrtRate)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("Ebid_frt_rate");
            entity.Property(e => e.EbidNetAmt)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("Ebid_net_amt");
            entity.Property(e => e.EpodDate)
                .HasColumnType("datetime")
                .HasColumnName("Epod_Date");
            entity.Property(e => e.EpodNo)
                .HasMaxLength(500)
                .HasColumnName("Epod_No");
            entity.Property(e => e.ExceptionalEntry).HasMaxLength(100);
            entity.Property(e => e.ForwardingAgentCode)
                .HasMaxLength(500)
                .HasColumnName("Forwarding_Agent_Code");
            entity.Property(e => e.ForwardingAgentName)
                .HasMaxLength(1000)
                .HasColumnName("Forwarding_Agent_Name");
            entity.Property(e => e.FreightRoad)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("Freight_Road");
            entity.Property(e => e.IgstRate)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("Igst_Rate");
            entity.Property(e => e.IncoTerm)
                .HasMaxLength(500)
                .HasColumnName("Inco_Term");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("Is_Active");
            entity.Property(e => e.LrGrDate)
                .HasColumnType("datetime")
                .HasColumnName("LR_GR_date");
            entity.Property(e => e.LrGrNo)
                .HasMaxLength(200)
                .HasColumnName("LR_GR_No");
            entity.Property(e => e.LrGrNoUniqueId)
                .HasMaxLength(1000)
                .HasColumnName("LR_GR_NO_Unique_Id");
            entity.Property(e => e.PgiDate)
                .HasColumnType("datetime")
                .HasColumnName("Pgi_Date");
            entity.Property(e => e.PgiNo)
                .HasMaxLength(500)
                .HasColumnName("Pgi_No");
            entity.Property(e => e.RegionState)
                .HasMaxLength(500)
                .HasColumnName("Region_State");
            entity.Property(e => e.RouteCode)
                .HasMaxLength(500)
                .HasColumnName("Route_Code");
            entity.Property(e => e.RouteDescription)
                .HasMaxLength(2000)
                .HasColumnName("Route_Description");
            entity.Property(e => e.SgstRate)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("Sgst_Rate");
            entity.Property(e => e.ShipToPartyTzone)
                .HasMaxLength(500)
                .HasColumnName("Ship_To_Party_TZone");
            entity.Property(e => e.ShipmentNo)
                .HasMaxLength(200)
                .HasColumnName("Shipment_No");
            entity.Property(e => e.SupplyingPlant)
                .HasMaxLength(100)
                .HasColumnName("Supplying_Plant");
            entity.Property(e => e.TotalAmount)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("Total_Amount");
            entity.Property(e => e.TruckNo)
                .HasMaxLength(200)
                .HasColumnName("Truck_No");
            entity.Property(e => e.UpdateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Update_Date");
            entity.Property(e => e.UpdatedBy).HasColumnName("Updated_By");
            entity.Property(e => e.UtgstRate)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("Utgst_Rate");
        });

        modelBuilder.Entity<TblDistanceMaster>(entity =>
        {
            entity.ToTable("tbl_Distance_Master");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedBy).HasColumnName("Created_By");
            entity.Property(e => e.CreationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Creation_Date");
            entity.Property(e => e.IsActive).HasColumnName("Is_Active");
            entity.Property(e => e.RouteDescription)
                .HasMaxLength(1000)
                .HasColumnName("Route_Description");
            entity.Property(e => e.UpdateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Update_Date");
            entity.Property(e => e.UpdatedBy).HasColumnName("Updated_By");
        });

        modelBuilder.Entity<TblDistrict>(entity =>
        {
            entity.ToTable("tbl_District");

            entity.HasIndex(e => e.DistrictName, "Index_tbl_District_District_Name");

            entity.HasIndex(e => e.IsActive, "Index_tbl_District_Is_Active");

            entity.HasIndex(e => e.StateId, "Index_tbl_District_State_ID");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.CreatedBy).HasColumnName("Created_By");
            entity.Property(e => e.CreationDate)
                .HasColumnType("datetime")
                .HasColumnName("Creation_Date");
            entity.Property(e => e.DistrictName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("District_Name");
            entity.Property(e => e.IsActive).HasColumnName("Is_Active");
            entity.Property(e => e.StateId)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("State_ID");
            entity.Property(e => e.UpdateDate)
                .HasColumnType("datetime")
                .HasColumnName("Update_Date");
            entity.Property(e => e.UpdatedBy).HasColumnName("Updated_By");

            entity.HasOne(d => d.State).WithMany(p => p.TblDistricts)
                .HasForeignKey(d => d.StateId)
                .HasConstraintName("FK_tbl_District_tbl_State");
        });

        modelBuilder.Entity<TblDriverHisabHeader>(entity =>
        {
            entity.HasKey(e => e.SettlementNo);

            entity.ToTable("tbl_Driver_Hisab_Header");

            entity.Property(e => e.SettlementNo).HasColumnName("Settlement_No");
            entity.Property(e => e.CreatedBy).HasColumnName("Created_By");
            entity.Property(e => e.LastSettlementId).HasColumnName("Last_Settlement_Id");
            entity.Property(e => e.CreationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Creation_Date");
            entity.Property(e => e.DriverId).HasColumnName("Driver_ID");
            entity.Property(e => e.IsActive).HasColumnName("Is_Active");
            entity.Property(e => e.Remarks).HasColumnName("Remarks"); 
            entity.Property(e => e.OpeningBalance)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("Opening_Balance");
            entity.Property(e => e.ClosingBalance)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("Closing_Balance");
            entity.Property(e => e.RouteDescription)
                .HasMaxLength(500)
                .HasColumnName("Route_Description");
            entity.Property(e => e.SettlementDate)
                .HasColumnType("datetime")
                .HasColumnName("Settlement_date");
            entity.Property(e => e.TripEndDate)
                .HasColumnType("datetime")
                .HasColumnName("Trip_end_date");
            entity.Property(e => e.TripStartDate)
                .HasColumnType("datetime")
                .HasColumnName("Trip_start_date");
            entity.Property(e => e.UpdateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Update_Date");
            entity.Property(e => e.ApprovedBy).HasColumnName("Approved_By"); 
            entity.Property(e => e.ApprovedDate)
               .HasColumnType("datetime")
               .HasColumnName("Approved_Date");
            entity.Property(e => e.UpdatedBy).HasColumnName("Updated_By");
            entity.Property(e => e.VehicleNo).HasColumnName("Vehicle_No");
            entity.Property(e => e.Weight).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Driver).WithMany(p => p.TblDriverHisabHeaders)
                .HasForeignKey(d => d.DriverId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_Driver_Hisab_Header_tbl_Driver_Master");

            entity.HasOne(d => d.VehicleNoNavigation).WithMany(p => p.TblDriverHisabHeaders)
                .HasForeignKey(d => d.VehicleNo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_Driver_Hisab_Header_tbl_Vehicle_Master");
        });

        modelBuilder.Entity<TblDriverHisabLine>(entity =>
        {
            entity.HasKey(e => e.Sno);

            entity.ToTable("tbl_Driver_Hisab_Lines");

            entity.Property(e => e.CrAmt)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("Cr_Amt");
            entity.Property(e => e.CreatedBy).HasColumnName("Created_By");
            entity.Property(e => e.CreationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Creation_Date");
            entity.Property(e => e.DrAmt)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("Dr_amt");
            entity.Property(e => e.DriverId).HasColumnName("Driver_ID");
            entity.Property(e => e.ExpenseCode)
                .HasMaxLength(100)
                .HasColumnName("Expense_Code");
            entity.Property(e => e.ExpenseType)
                .HasMaxLength(100)
                .HasColumnName("Expense_type");
            entity.Property(e => e.SettlementNo).HasColumnName("Settlement_No");
            entity.Property(e => e.UpdateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Update_Date");
            entity.Property(e => e.UpdatedBy).HasColumnName("Updated_By");

            entity.HasOne(d => d.Driver).WithMany(p => p.TblDriverHisabLines)
                .HasForeignKey(d => d.DriverId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_Driver_Hisab_Lines_tbl_Driver_Master");

            entity.HasOne(d => d.SettlementNoNavigation).WithMany(p => p.TblDriverHisabLines)
                .HasForeignKey(d => d.SettlementNo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_Driver_Hisab_Lines_tbl_Driver_Hisab_Header");
        });

        modelBuilder.Entity<TblDriverMaster>(entity =>
        {
            entity.ToTable("tbl_Driver_Master");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AadharNo)
                .HasMaxLength(500)
                .HasColumnName("Aadhar_No");
            entity.Property(e => e.AadharNoImage)
                .HasMaxLength(1000)
                .HasColumnName("Aadhar_No_Image");
            entity.Property(e => e.BankAccountNumber)
                .HasMaxLength(500)
                .HasColumnName("Bank_AccountNumber");
            entity.Property(e => e.BankIfsccode)
                .HasMaxLength(500)
                .HasColumnName("Bank_IFSCCode");
            entity.Property(e => e.BankName)
                .HasMaxLength(500)
                .HasColumnName("Bank_Name");
            entity.Property(e => e.CreatedBy).HasColumnName("Created_By");
            entity.Property(e => e.CreationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Creation_Date");
            entity.Property(e => e.DriverAddress1)
                .HasMaxLength(2000)
                .HasColumnName("Driver_Address1");
            entity.Property(e => e.DriverAddress2)
                .HasMaxLength(2000)
                .HasColumnName("Driver_Address2");
            entity.Property(e => e.DriverAddress3)
                .HasMaxLength(2000)
                .HasColumnName("Driver_Address3");
            entity.Property(e => e.DriverName)
                .HasMaxLength(500)
                .HasColumnName("Driver_Name");
            entity.Property(e => e.DriverPhoto)
                .HasMaxLength(1000)
                .HasColumnName("Driver_Photo");
            entity.Property(e => e.DrivingLicenceImage).HasMaxLength(1000);
            entity.Property(e => e.DrivingLicenceIssueAuth).HasMaxLength(500);
            entity.Property(e => e.DrivingLicenceNo).HasMaxLength(500);
            entity.Property(e => e.DrivingLicenceValidity).HasColumnType("datetime");
            entity.Property(e => e.FatherName)
                .HasMaxLength(500)
                .HasColumnName("Father_Name");
            entity.Property(e => e.IsActive).HasColumnName("Is_Active");
            entity.Property(e => e.IsExistingReference).HasColumnName("Is_Existing_Reference");
            entity.Property(e => e.MobileNumber1).HasMaxLength(500);
            entity.Property(e => e.MobileNumber2).HasMaxLength(500);
            entity.Property(e => e.PanNo).HasMaxLength(500);
            entity.Property(e => e.PanNoImage)
                .HasMaxLength(1000)
                .HasColumnName("Pan_No_Image");
            entity.Property(e => e.PinCode).HasMaxLength(500);
            entity.Property(e => e.ReferenceAddress1)
                .HasMaxLength(500)
                .HasColumnName("Reference_Address1");
            entity.Property(e => e.ReferenceAddress2)
                .HasMaxLength(500)
                .HasColumnName("Reference_Address2");
            entity.Property(e => e.ReferenceAddress3)
                .HasMaxLength(500)
                .HasColumnName("Reference_Address3");
            entity.Property(e => e.ReferenceCity)
                .HasMaxLength(500)
                .HasColumnName("Reference_City");
            entity.Property(e => e.ReferenceMobile)
                .HasMaxLength(500)
                .HasColumnName("Reference_Mobile");
            entity.Property(e => e.ReferenceName)
                .HasMaxLength(500)
                .HasColumnName("Reference_Name");
            entity.Property(e => e.ReferencePin)
                .HasMaxLength(500)
                .HasColumnName("Reference_Pin");
            entity.Property(e => e.UpdateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Update_Date");
            entity.Property(e => e.UpdatedBy).HasColumnName("Updated_By");

            entity.HasOne(d => d.City).WithMany(p => p.TblDriverMasters)
                .HasForeignKey(d => d.CityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_Driver_Master_tbl_City");

            entity.HasOne(d => d.District).WithMany(p => p.TblDriverMasters)
                .HasForeignKey(d => d.DistrictId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_Driver_Master_tbl_District");

            entity.HasOne(d => d.State).WithMany(p => p.TblDriverMasters)
                .HasForeignKey(d => d.StateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_Driver_Master_tbl_State");
        });

        modelBuilder.Entity<TblExpenseMaster>(entity =>
        {
            entity.ToTable("tbl_Expense_Master");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedBy).HasColumnName("Created_By");
            entity.Property(e => e.CreationDate)
                .HasColumnType("datetime")
                .HasColumnName("Creation_Date");
            entity.Property(e => e.ExpCode)
                .HasMaxLength(200)
                .HasColumnName("Exp_Code");
            entity.Property(e => e.ExpDescription)
                .HasMaxLength(2000)
                .HasColumnName("Exp_Description");
            entity.Property(e => e.ExpOther)
                .HasMaxLength(200)
                .HasColumnName("Exp_Other");
            entity.Property(e => e.ExpType).HasColumnName("Exp_Type");
            entity.Property(e => e.IsActive).HasColumnName("Is_Active");
            entity.Property(e => e.UpdateDate)
                .HasColumnType("datetime")
                .HasColumnName("Update_Date");
            entity.Property(e => e.UpdatedBy).HasColumnName("Updated_By");

            entity.HasOne(d => d.ExpTypeNavigation).WithMany(p => p.TblExpenseMasters)
                .HasForeignKey(d => d.ExpType)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_Expense_Master_tbl_Code_Master");
        });

        modelBuilder.Entity<TblFunctionMaster>(entity =>
        {
            entity.ToTable("tbl_FunctionMaster");

            entity.HasIndex(e => e.EndDate, "Index_tbl_FunctionMaster_End_Date");

            entity.HasIndex(e => e.FunctionName, "Index_tbl_FunctionMaster_Function_Name");

            entity.HasIndex(e => e.IsActive, "Index_tbl_FunctionMaster_Is_Active");

            entity.HasIndex(e => e.StartDate, "Index_tbl_FunctionMaster_Start_Date");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedBy).HasColumnName("Created_By");
            entity.Property(e => e.CreationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Creation_Date");
            entity.Property(e => e.EndDate)
                .HasColumnType("datetime")
                .HasColumnName("End_Date");
            entity.Property(e => e.FunctionName)
                .HasMaxLength(500)
                .HasColumnName("Function_Name");
            entity.Property(e => e.IsActive).HasColumnName("Is_Active");
            entity.Property(e => e.StartDate)
                .HasColumnType("datetime")
                .HasColumnName("Start_Date");
            entity.Property(e => e.UpdateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Update_Date");
            entity.Property(e => e.UpdatedBy).HasColumnName("Updated_By");
        });

        modelBuilder.Entity<TblGstMaster>(entity =>
        {
            entity.ToTable("tbl_GST_Master");

            entity.HasIndex(e => e.EffectiveDate, "Index_tbl_GST_Master_Effective_Date");

            entity.HasIndex(e => e.IsRcm, "Index_tbl_GST_Master_IsRCM");

            entity.HasIndex(e => e.TransporterCode, "Index_tbl_GST_Master_Transporter_Code");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CgstRate)
                .HasColumnType("decimal(19, 2)")
                .HasColumnName("CGST_Rate");
            entity.Property(e => e.CreatedBy).HasColumnName("Created_By");
            entity.Property(e => e.CreationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Creation_Date");
            entity.Property(e => e.EffectiveDate)
                .HasColumnType("datetime")
                .HasColumnName("Effective_Date");
            entity.Property(e => e.EndDate)
                .HasColumnType("datetime")
                .HasColumnName("End_Date");
            entity.Property(e => e.IgstRate)
                .HasColumnType("decimal(19, 2)")
                .HasColumnName("IGST_Rate");
            entity.Property(e => e.IsActive).HasColumnName("Is_Active");
            entity.Property(e => e.IsRcm)
                .HasDefaultValue(true)
                .HasColumnName("IsRCM");
            entity.Property(e => e.SgstRate)
                .HasColumnType("decimal(19, 2)")
                .HasColumnName("SGST_Rate");
            entity.Property(e => e.TransporterCode)
                .HasMaxLength(200)
                .HasColumnName("Transporter_Code");
            entity.Property(e => e.UpdateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Update_Date");
            entity.Property(e => e.UpdatedBy).HasColumnName("Updated_By");
            entity.Property(e => e.UtgstRate)
                .HasColumnType("decimal(19, 2)")
                .HasColumnName("UTGST_Rate");
        });

        modelBuilder.Entity<TblGstRateMaster>(entity =>
        {
            entity.ToTable("tbl_GST_Rate_Master");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CgstRate)
                .HasColumnType("decimal(19, 2)")
                .HasColumnName("CGST_Rate");
            entity.Property(e => e.CreatedBy).HasColumnName("Created_By");
            entity.Property(e => e.CreationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Creation_Date");
            entity.Property(e => e.EffectiveDate)
                .HasColumnType("datetime")
                .HasColumnName("Effective_Date");
            entity.Property(e => e.EndDate)
                .HasColumnType("datetime")
                .HasColumnName("End_Date");
            entity.Property(e => e.IgstRate)
                .HasColumnType("decimal(19, 2)")
                .HasColumnName("IGST_Rate");
            entity.Property(e => e.IsActive).HasColumnName("Is_Active");
            entity.Property(e => e.SgstRate)
                .HasColumnType("decimal(19, 2)")
                .HasColumnName("SGST_Rate");
            entity.Property(e => e.UpdateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Update_Date");
            entity.Property(e => e.UpdatedBy).HasColumnName("Updated_By");
            entity.Property(e => e.UtgstRate)
                .HasColumnType("decimal(19, 2)")
                .HasColumnName("UTGST_Rate");
        });

        modelBuilder.Entity<TblMakeMaster>(entity =>
        {
            entity.ToTable("tbl_Make_Master");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedBy).HasColumnName("Created_By");
            entity.Property(e => e.CreationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Creation_Date");
            entity.Property(e => e.IsActive).HasColumnName("Is_Active");
            entity.Property(e => e.Make).HasMaxLength(500);
            entity.Property(e => e.MakeName)
                .HasMaxLength(500)
                .HasColumnName("Make_Name");
            entity.Property(e => e.UpdateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Update_Date");
            entity.Property(e => e.UpdatedBy).HasColumnName("Updated_By");
        });

        modelBuilder.Entity<TblModelAverageMaster>(entity =>
        {
            entity.ToTable("tbl_Model_Average_Master");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedBy).HasColumnName("Created_By");
            entity.Property(e => e.CreationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Creation_Date");
            entity.Property(e => e.IsActive).HasColumnName("Is_Active");
            entity.Property(e => e.Khali).HasColumnType("decimal(19, 2)");
            entity.Property(e => e.MakeId).HasColumnName("Make_ID");
            entity.Property(e => e.MegaHw)
                .HasColumnType("decimal(19, 2)")
                .HasColumnName("Mega_HW");
            entity.Property(e => e.ModelNo).HasColumnName("model_no");
            entity.Property(e => e.Nh)
                .HasColumnType("decimal(19, 2)")
                .HasColumnName("NH");
            entity.Property(e => e.OffRoad).HasColumnType("decimal(19, 2)");
            entity.Property(e => e.Other).HasColumnType("decimal(19, 2)");
            entity.Property(e => e.OverLoad).HasColumnType("decimal(19, 2)");
            entity.Property(e => e.UlAvg)
                .HasColumnType("decimal(19, 2)")
                .HasColumnName("UL_Avg");
            entity.Property(e => e.UpdateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Update_Date");
            entity.Property(e => e.UpdatedBy).HasColumnName("Updated_By");

            entity.HasOne(d => d.Make).WithMany(p => p.TblModelAverageMasters)
                .HasForeignKey(d => d.MakeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_Model_Average_Master_tbl_Code_Master");
        });

        modelBuilder.Entity<TblModelMaster>(entity =>
        {
            entity.ToTable("tbl_Model_Master");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedBy).HasColumnName("Created_By");
            entity.Property(e => e.CreationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Creation_Date");
            entity.Property(e => e.FuleType)
                .HasMaxLength(500)
                .HasColumnName("Fule_Type");
            entity.Property(e => e.IsActive).HasColumnName("Is_Active");
            entity.Property(e => e.MakeId).HasColumnName("Make_ID");
            entity.Property(e => e.ModelNo).HasColumnName("model_no");
            entity.Property(e => e.NoOfTyres).HasColumnName("No_Of_Tyres");
            entity.Property(e => e.UpdateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Update_Date");
            entity.Property(e => e.UpdatedBy).HasColumnName("Updated_By");
            entity.Property(e => e.VehicleType)
                .HasMaxLength(500)
                .HasColumnName("Vehicle_Type");

            entity.HasOne(d => d.Make).WithMany(p => p.TblModelMasters)
                .HasForeignKey(d => d.MakeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_Model_Master_tbl_Make_Master");
        });

        modelBuilder.Entity<TblRoleMaster>(entity =>
        {
            entity.ToTable("tbl_RoleMaster");

            entity.HasIndex(e => e.EndDate, "Index_tbl_RoleMaster_End_Date");

            entity.HasIndex(e => e.IsActive, "Index_tbl_RoleMaster_Is_Active");

            entity.HasIndex(e => e.Role, "Index_tbl_RoleMaster_Role");

            entity.HasIndex(e => e.RoleName, "Index_tbl_RoleMaster_Role_Name");

            entity.HasIndex(e => e.StartDate, "Index_tbl_RoleMaster_Start_Date");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedBy).HasColumnName("Created_By");
            entity.Property(e => e.CreationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Creation_Date");
            entity.Property(e => e.EndDate)
                .HasColumnType("datetime")
                .HasColumnName("End_Date");
            entity.Property(e => e.IsActive).HasColumnName("Is_Active");
            entity.Property(e => e.Role).HasMaxLength(2000);
            entity.Property(e => e.RoleName)
                .HasMaxLength(2000)
                .HasColumnName("Role_Name");
            entity.Property(e => e.StartDate)
                .HasColumnType("datetime")
                .HasColumnName("Start_Date");
            entity.Property(e => e.UpdateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Update_Date");
            entity.Property(e => e.UpdatedBy).HasColumnName("Updated_By");
        });

        modelBuilder.Entity<TblState>(entity =>
        {
            entity.ToTable("tbl_State");

            entity.HasIndex(e => e.CountryId, "Index_tbl_State_Country_ID");

            entity.HasIndex(e => e.IsActive, "Index_tbl_State_Is_Active");

            entity.HasIndex(e => e.StateName, "Index_tbl_State_State_Name");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.CountryId)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("Country_ID");
            entity.Property(e => e.CreatedBy).HasColumnName("Created_By");
            entity.Property(e => e.CreationDate)
                .HasColumnType("datetime")
                .HasColumnName("Creation_Date");
            entity.Property(e => e.IsActive).HasColumnName("Is_Active");
            entity.Property(e => e.IsUnionTerritory).HasDefaultValue(false);
            entity.Property(e => e.StateCode)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("State_Code");
            entity.Property(e => e.StateName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("State_Name");
            entity.Property(e => e.UpdateDate)
                .HasColumnType("datetime")
                .HasColumnName("Update_Date");
            entity.Property(e => e.UpdatedBy).HasColumnName("Updated_By");

            entity.HasOne(d => d.Country).WithMany(p => p.TblStates)
                .HasForeignKey(d => d.CountryId)
                .HasConstraintName("FK_tbl_State_tbl_CountryMaster");
        });

        modelBuilder.Entity<TblTempDispatchDetail>(entity =>
        {
            entity.ToTable("tbl_Temp_Dispatch_Details");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.BillDate)
                .HasColumnType("datetime")
                .HasColumnName("Bill_Date");
            entity.Property(e => e.BillNo)
                .HasMaxLength(500)
                .HasColumnName("Bill_No");
            entity.Property(e => e.CgstRate)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("Cgst_Rate");
            entity.Property(e => e.CreatedBy).HasColumnName("Created_By");
            entity.Property(e => e.CreationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Creation_Date");
            entity.Property(e => e.DeliveryNo)
                .HasMaxLength(200)
                .HasColumnName("Delivery_No");
            entity.Property(e => e.DispatchQtyRoad).HasColumnName("Dispatch_Qty_Road");
            entity.Property(e => e.DispatchUniqueId)
                .HasMaxLength(100)
                .HasColumnName("Dispatch_Unique_ID");
            entity.Property(e => e.DistributionChannel)
                .HasMaxLength(10)
                .HasColumnName("Distribution_Channel");
            entity.Property(e => e.Division).HasMaxLength(500);
            entity.Property(e => e.EbidFrtRate)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("Ebid_frt_rate");
            entity.Property(e => e.EbidNetAmt)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("Ebid_net_amt");
            entity.Property(e => e.EpodDate)
                .HasColumnType("datetime")
                .HasColumnName("Epod_Date");
            entity.Property(e => e.EpodNo)
                .HasMaxLength(500)
                .HasColumnName("Epod_No");
            entity.Property(e => e.ForwardingAgentCode)
                .HasMaxLength(500)
                .HasColumnName("Forwarding_Agent_Code");
            entity.Property(e => e.ForwardingAgentName)
                .HasMaxLength(1000)
                .HasColumnName("Forwarding_Agent_Name");
            entity.Property(e => e.IgstRate)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("Igst_Rate");
            entity.Property(e => e.IncoTerm)
                .HasMaxLength(500)
                .HasColumnName("Inco_Term");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("Is_Active");
            entity.Property(e => e.LrGrDate)
                .HasColumnType("datetime")
                .HasColumnName("LR_GR_date");
            entity.Property(e => e.LrGrNo)
                .HasMaxLength(200)
                .HasColumnName("LR_GR_No");
            entity.Property(e => e.PgiDate)
                .HasColumnType("datetime")
                .HasColumnName("Pgi_Date");
            entity.Property(e => e.PgiNo)
                .HasMaxLength(500)
                .HasColumnName("Pgi_No");
            entity.Property(e => e.RegionState)
                .HasMaxLength(500)
                .HasColumnName("Region_State");
            entity.Property(e => e.RouteCode)
                .HasMaxLength(500)
                .HasColumnName("Route_Code");
            entity.Property(e => e.RouteDescription)
                .HasMaxLength(2000)
                .HasColumnName("Route_Description");
            entity.Property(e => e.SgstRate)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("Sgst_Rate");
            entity.Property(e => e.ShipToPartyTzone)
                .HasMaxLength(500)
                .HasColumnName("Ship_To_Party_TZone");
            entity.Property(e => e.ShipmentNo)
                .HasMaxLength(200)
                .HasColumnName("Shipment_No");
            entity.Property(e => e.SupplyingPlant)
                .HasMaxLength(100)
                .HasColumnName("Supplying_Plant");
            entity.Property(e => e.TotalAmount)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("Total_Amount");
            entity.Property(e => e.TruckNo)
                .HasMaxLength(200)
                .HasColumnName("Truck_No");
            entity.Property(e => e.UpdateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Update_Date");
            entity.Property(e => e.UpdatedBy).HasColumnName("Updated_By");
            entity.Property(e => e.UtgstRate)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("Utgst_Rate");
        });

        modelBuilder.Entity<TblTransporterBill>(entity =>
        {
            entity.ToTable("tbl_Transporter_Bill");

            entity.HasIndex(e => e.BillDate, "Index_Dispatch_Details_Bill_Date");

            entity.HasIndex(e => e.BillNumber, "Index_Dispatch_Details_Bill_Number");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.BillDate)
                .HasColumnType("datetime")
                .HasColumnName("Bill_Date");
            entity.Property(e => e.BillNumber)
                .HasMaxLength(100)
                .HasColumnName("Bill_Number");
            entity.Property(e => e.CgstAmount)
                .HasDefaultValue(0.00m)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("CGST_AMOUNT");
            entity.Property(e => e.CompanyId).HasColumnName("Company_ID");
            entity.Property(e => e.CreatedBy).HasColumnName("Created_By");
            entity.Property(e => e.CreationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Creation_Date");
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.IgstAmount)
                .HasDefaultValue(0.00m)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("IGST_AMOUNT");
            entity.Property(e => e.IsActive).HasColumnName("Is_Active");
            entity.Property(e => e.SgstAmount)
                .HasDefaultValue(0.00m)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("SGST_AMOUNT");
            entity.Property(e => e.StartDate).HasColumnType("datetime");
            entity.Property(e => e.TotalBillAmount)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("Total_Bill_Amount");
            entity.Property(e => e.TransporterId).HasColumnName("Transporter_ID");
            entity.Property(e => e.UgstAmount)
                .HasDefaultValue(0.00m)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("UGST_AMOUNT");
            entity.Property(e => e.UpdateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Update_Date");
            entity.Property(e => e.UpdatedBy).HasColumnName("Updated_By");
        });

        modelBuilder.Entity<TblTransporterMaster>(entity =>
        {
            entity.ToTable("tbl_TransporterMaster");

            entity.HasIndex(e => e.CityId, "Index_TransporterMaster_City_ID");

            entity.HasIndex(e => e.DistrictId, "Index_TransporterMaster_District_ID");

            entity.HasIndex(e => e.GstinNo, "Index_TransporterMaster_GSTIN_No");

            entity.HasIndex(e => e.PanNo, "Index_TransporterMaster_Pan_No");

            entity.HasIndex(e => e.StateId, "Index_TransporterMaster_State_ID");

            entity.HasIndex(e => e.TransporterCode, "Index_TransporterMaster_Transporter_Code");

            entity.HasIndex(e => e.UserId, "Index_TransporterMaster_User_ID");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Address1).HasMaxLength(2000);
            entity.Property(e => e.Address2).HasMaxLength(2000);
            entity.Property(e => e.Address3).HasMaxLength(2000);
            entity.Property(e => e.BillPrefix)
                .HasMaxLength(500)
                .HasColumnName("Bill_Prefix");
            entity.Property(e => e.BillStartNo)
                .HasMaxLength(500)
                .HasColumnName("Bill_Start_No");
            entity.Property(e => e.CityId).HasColumnName("City_ID");
            entity.Property(e => e.CreatedBy).HasColumnName("Created_By");
            entity.Property(e => e.CreationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Creation_Date");
            entity.Property(e => e.DistrictId).HasColumnName("District_ID");
            entity.Property(e => e.EmailId)
                .HasMaxLength(200)
                .HasColumnName("Email_ID");
            entity.Property(e => e.GstinNo)
                .HasMaxLength(200)
                .HasColumnName("GSTIN_No");
            entity.Property(e => e.IsActive).HasColumnName("Is_Active");
            entity.Property(e => e.MobileNumber)
                .HasMaxLength(200)
                .HasColumnName("Mobile_Number");
            entity.Property(e => e.OwnerName)
                .HasMaxLength(2000)
                .HasColumnName("Owner_Name");
            entity.Property(e => e.PanNo)
                .HasMaxLength(200)
                .HasColumnName("Pan_No");
            entity.Property(e => e.PanNoImage)
                .HasMaxLength(1000)
                .HasColumnName("Pan_No_Image");
            entity.Property(e => e.PinCode)
                .HasMaxLength(500)
                .HasColumnName("Pin_Code");
            entity.Property(e => e.StateId).HasColumnName("State_ID");
            entity.Property(e => e.TransporterCode)
                .HasMaxLength(200)
                .HasColumnName("Transporter_Code");
            entity.Property(e => e.TransporterName)
                .HasMaxLength(2000)
                .HasColumnName("Transporter_Name");
            entity.Property(e => e.UpdateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Update_Date");
            entity.Property(e => e.UpdatedBy).HasColumnName("Updated_By");
            entity.Property(e => e.UserId).HasColumnName("User_ID");

            entity.HasOne(d => d.City).WithMany(p => p.TblTransporterMasters)
                .HasForeignKey(d => d.CityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_TransporterMaster_tbl_City");

            entity.HasOne(d => d.District).WithMany(p => p.TblTransporterMasters)
                .HasForeignKey(d => d.DistrictId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_TransporterMaster_tbl_District");

            entity.HasOne(d => d.State).WithMany(p => p.TblTransporterMasters)
                .HasForeignKey(d => d.StateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_TransporterMaster_tbl_State");

            entity.HasOne(d => d.User).WithMany(p => p.TblTransporterMasters)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_TransporterMaster_tbl_UserMaster");
        });

        modelBuilder.Entity<TblUserFunction>(entity =>
        {
            entity.ToTable("tbl_User_Function");

            entity.HasIndex(e => e.EndDate, "Index_tbl_User_Function_End_Date");

            entity.HasIndex(e => e.FunctionId, "Index_tbl_User_Function_Function_ID");

            entity.HasIndex(e => e.FunctionName, "Index_tbl_User_Function_Function_Name");

            entity.HasIndex(e => e.IsActive, "Index_tbl_User_Function_Is_Active");

            entity.HasIndex(e => e.RoleId, "Index_tbl_User_Function_Role_ID");

            entity.HasIndex(e => e.StartDate, "Index_tbl_User_Function_Start_Date");

            entity.HasIndex(e => e.UserId, "Index_tbl_User_Function_User_ID");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedBy).HasColumnName("Created_By");
            entity.Property(e => e.CreationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Creation_Date");
            entity.Property(e => e.EndDate)
                .HasColumnType("datetime")
                .HasColumnName("End_Date");
            entity.Property(e => e.FunctionId).HasColumnName("Function_ID");
            entity.Property(e => e.FunctionName)
                .HasMaxLength(500)
                .HasColumnName("Function_Name");
            entity.Property(e => e.IsActive).HasColumnName("Is_Active");
            entity.Property(e => e.RoleId).HasColumnName("Role_ID");
            entity.Property(e => e.StartDate)
                .HasColumnType("datetime")
                .HasColumnName("Start_Date");
            entity.Property(e => e.UpdateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Update_Date");
            entity.Property(e => e.UpdatedBy).HasColumnName("Updated_By");
            entity.Property(e => e.UserId).HasColumnName("User_ID");

            entity.HasOne(d => d.Function).WithMany(p => p.TblUserFunctions)
                .HasForeignKey(d => d.FunctionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_User_Function_tbl_FunctionMaster");

            entity.HasOne(d => d.Role).WithMany(p => p.TblUserFunctions)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_User_Function_tbl_RoleMaster");

            entity.HasOne(d => d.User).WithMany(p => p.TblUserFunctions)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_User_Function_tbl_UserMaster");
        });

        modelBuilder.Entity<TblUserMaster>(entity =>
        {
            entity.ToTable("tbl_UserMaster");

            entity.HasIndex(e => e.EmailId, "Index_tbl_UserMaster_Email_ID");

            entity.HasIndex(e => e.EndDate, "Index_tbl_UserMaster_End_Date");

            entity.HasIndex(e => e.IsActive, "Index_tbl_UserMaster_Is_Active");

            entity.HasIndex(e => e.MobileNo, "Index_tbl_UserMaster_Mobile_No");

            entity.HasIndex(e => e.StartDate, "Index_tbl_UserMaster_Start_Date");

            entity.HasIndex(e => e.UserName, "Index_tbl_UserMaster_User_Name");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedBy).HasColumnName("Created_By");
            entity.Property(e => e.CreationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Creation_Date");
            entity.Property(e => e.EmailId)
                .HasMaxLength(2000)
                .HasColumnName("Email_ID");
            entity.Property(e => e.EndDate)
                .HasColumnType("datetime")
                .HasColumnName("End_Date");
            entity.Property(e => e.IsActive).HasColumnName("Is_Active");
            entity.Property(e => e.MobileNo)
                .HasMaxLength(2000)
                .HasColumnName("Mobile_No");
            entity.Property(e => e.Password).HasMaxLength(2000);
            entity.Property(e => e.RoleId).HasColumnName("Role_ID");
            entity.Property(e => e.StartDate)
                .HasColumnType("datetime")
                .HasColumnName("Start_Date");
            entity.Property(e => e.UpdateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Update_Date");
            entity.Property(e => e.UpdatedBy).HasColumnName("Updated_By");
            entity.Property(e => e.UserId).HasMaxLength(100);
            entity.Property(e => e.UserName)
                .HasMaxLength(2000)
                .HasColumnName("User_Name");

            entity.HasOne(d => d.Role).WithMany(p => p.TblUserMasters)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK_tbl_UserMaster_tbl_RoleMaster");
        });

        modelBuilder.Entity<TblUserSubscription>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_tbl_User_Subacription");

            entity.ToTable("tbl_User_Subscription");

            entity.HasIndex(e => e.EndDate, "Index_tbl_User_Subscription_End_Date");

            entity.HasIndex(e => e.PaidDate, "Index_tbl_User_Subscription_Paid_Date");

            entity.HasIndex(e => e.StartDate, "Index_tbl_User_Subscription_Start_Date");

            entity.HasIndex(e => e.UserId, "Index_tbl_User_Subscription_User_ID");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Amount).HasColumnType("decimal(19, 2)");
            entity.Property(e => e.CreatedBy).HasColumnName("Created_By");
            entity.Property(e => e.CreationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Creation_Date");
            entity.Property(e => e.EndDate)
                .HasColumnType("datetime")
                .HasColumnName("End_Date");
            entity.Property(e => e.FinYear)
                .HasMaxLength(500)
                .HasColumnName("Fin_Year");
            entity.Property(e => e.IsActive).HasColumnName("Is_Active");
            entity.Property(e => e.PaidDate)
                .HasColumnType("datetime")
                .HasColumnName("Paid_Date");
            entity.Property(e => e.StartDate)
                .HasColumnType("datetime")
                .HasColumnName("Start_Date");
            entity.Property(e => e.UpdateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Update_Date");
            entity.Property(e => e.UpdatedBy).HasColumnName("Updated_By");
            entity.Property(e => e.UserId).HasColumnName("User_ID");

            entity.HasOne(d => d.User).WithMany(p => p.TblUserSubscriptions)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_User_Subacription_tbl_UserMaster");
        });

        modelBuilder.Entity<TblVehicleMaster>(entity =>
        {
            entity.ToTable("tbl_Vehicle_Master");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ActualCapicity).HasMaxLength(500);
            entity.Property(e => e.BranchOffice).HasMaxLength(1000);
            entity.Property(e => e.ChasisNo)
                .HasMaxLength(500)
                .HasColumnName("Chasis_No");
            entity.Property(e => e.CreatedBy).HasColumnName("Created_By");
            entity.Property(e => e.CreationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Creation_Date");
            entity.Property(e => e.Dimension).HasMaxLength(200);
            entity.Property(e => e.EngineNo)
                .HasMaxLength(500)
                .HasColumnName("Engine_No");
            entity.Property(e => e.FinancerName).HasMaxLength(200);
            entity.Property(e => e.FitnessDoc)
                .HasMaxLength(1000)
                .HasColumnName("Fitness_Doc");
            entity.Property(e => e.FitnessDue)
                .HasColumnType("datetime")
                .HasColumnName("Fitness_Due");
            entity.Property(e => e.InsuranceDoc)
                .HasMaxLength(1000)
                .HasColumnName("Insurance_Doc");
            entity.Property(e => e.InsuranceDue)
                .HasColumnType("datetime")
                .HasColumnName("Insurance_Due");
            entity.Property(e => e.IsActive).HasColumnName("Is_Active");
            entity.Property(e => e.LocalPermitDoc)
                .HasMaxLength(1000)
                .HasColumnName("Local_Permit_Doc");
            entity.Property(e => e.LocalPermitDue)
                .HasColumnType("datetime")
                .HasColumnName("Local_Permit_Due");
            entity.Property(e => e.MfgYear).HasColumnName("Mfg_Year");
            entity.Property(e => e.NationalPermitDoc)
                .HasMaxLength(1000)
                .HasColumnName("National_Permit_Doc");
            entity.Property(e => e.NationalPermitDue)
                .HasColumnType("datetime")
                .HasColumnName("National_Permit_Due");
            entity.Property(e => e.NoOfTyres).HasColumnName("No_Of_Tyres");
            entity.Property(e => e.PanNo)
                .HasMaxLength(200)
                .HasColumnName("Pan_No");
            entity.Property(e => e.PanNoImage)
                .HasMaxLength(1000)
                .HasColumnName("Pan_No_Image");
            entity.Property(e => e.PollutionDoc)
                .HasMaxLength(1000)
                .HasColumnName("Pollution_Doc");
            entity.Property(e => e.PollutionDue)
                .HasColumnType("datetime")
                .HasColumnName("Pollution_Due");
            entity.Property(e => e.PurchaseDate)
                .HasColumnType("datetime")
                .HasColumnName("Purchase_Date");
            entity.Property(e => e.RcCapicity)
                .HasMaxLength(500)
                .HasColumnName("RC_Capicity");
            entity.Property(e => e.RcDoc)
                .HasMaxLength(1000)
                .HasColumnName("RC_Doc");
            entity.Property(e => e.RcValidityDue)
                .HasColumnType("datetime")
                .HasColumnName("RC_Validity_Due");
            entity.Property(e => e.RtoDoc)
                .HasMaxLength(1000)
                .HasColumnName("RTO_Doc");
            entity.Property(e => e.RtoDue)
                .HasColumnType("datetime")
                .HasColumnName("RTO_Due");
            entity.Property(e => e.RunningKm)
                .HasDefaultValue(0)
                .HasColumnName("Running_Km");
            entity.Property(e => e.UpdateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Update_Date");
            entity.Property(e => e.UpdatedBy).HasColumnName("Updated_By");
            entity.Property(e => e.VehicleNo)
                .HasMaxLength(500)
                .HasColumnName("Vehicle_No");
            entity.Property(e => e.VehicleOwner)
                .HasMaxLength(2000)
                .HasColumnName("Vehicle_Owner");
            entity.Property(e => e.Sale_Date)
                .HasColumnType("datetime");
            entity.Property(e => e.BS6_Model);

            entity.HasOne(d => d.BodyManufacturer).WithMany(p => p.TblVehicleMasterBodyManufacturers)
                .HasForeignKey(d => d.BodyManufacturerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_Vehicle_Master_tbl_Code_Master2");

            entity.HasOne(d => d.BodyType).WithMany(p => p.TblVehicleMasterBodyTypes)
                .HasForeignKey(d => d.BodyTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_Vehicle_Master_tbl_Code_Master1");

            entity.HasOne(d => d.Make).WithMany(p => p.TblVehicleMasterMakes)
                .HasForeignKey(d => d.MakeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_Vehicle_Master_tbl_Make_Master");

            entity.HasOne(d => d.Model).WithMany(p => p.TblVehicleMasters)
                .HasForeignKey(d => d.ModelId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_Vehicle_Master_tbl_Model_Master");

            entity.HasOne(d => d.VehicleType).WithMany(p => p.TblVehicleMasterVehicleTypes)
                .HasForeignKey(d => d.VehicleTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_Vehicle_Master_tbl_Code_Master");
        });

        modelBuilder.Entity<TblServiceDueMaster>(entity =>
        {
            entity.ToTable("tbl_ServiceDue_Master");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.VehicleId).HasColumnName("Vehicle_Id");
            entity.Property(e => e.PurchaseDate).HasColumnName("Purchase_Date");
            entity.Property(e => e.ServiceCode).HasColumnName("Service_Code");
            entity.Property(e => e.IntervalKm).HasColumnName("Interval_Km");
            entity.Property(e => e.IntervalMonth).HasColumnName("Interval_Month");
            entity.Property(e => e.DueDate).HasColumnName("Due_Date");
            entity.Property(e => e.PartCost).HasColumnName("Parts_Cost");
            entity.Property(e => e.LabourCost).HasColumnName("Labour_Cost");
            entity.Property(e => e.TotalCost).HasColumnName("Total_Cost");
            entity.Property(e => e.Workshop).HasColumnName("Workshop");
            entity.Property(e => e.Remarks).HasColumnName("Remarks");           

            entity.Property(e => e.IsActive).HasColumnName("Is_Active");
            entity.Property(e => e.CreatedBy).HasColumnName("Created_By");
            entity.Property(e => e.CreationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Creation_Date");
            entity.Property(e => e.UpdatedBy).HasColumnName("Updated_By");
            entity.Property(e => e.UpdateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Update_Date");
        });

        modelBuilder.Entity<TblServiceCompletion>(entity =>
        {
            entity.ToTable("tbl_Service_Completion");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.VehicleId).HasColumnName("Vehicle_Id");
            entity.Property(e => e.ServiceDate).HasColumnName("Service_Date");
            entity.Property(e => e.KmReadingOnService).HasColumnName("Km_Reading_On_Service");
            entity.Property(e => e.IntervalKm).HasColumnName("Interval_Km");
            entity.Property(e => e.IntervalMonth).HasColumnName("Interval_Month");
            entity.Property(e => e.DueDate).HasColumnName("Due_Date");
            entity.Property(e => e.PartCost).HasColumnName("Parts_Cost");
            entity.Property(e => e.LabourCost).HasColumnName("Labour_Cost");
            entity.Property(e => e.TotalCost).HasColumnName("Total_Cost");
            entity.Property(e => e.Workshop).HasColumnName("Workshop");
            entity.Property(e => e.Remarks).HasColumnName("Remarks");

            entity.Property(e => e.IsActive).HasColumnName("Is_Active");
            entity.Property(e => e.CreatedBy).HasColumnName("Created_By");
            entity.Property(e => e.CreationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Creation_Date");
            entity.Property(e => e.UpdatedBy).HasColumnName("Updated_By");
            entity.Property(e => e.UpdateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Update_Date");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
