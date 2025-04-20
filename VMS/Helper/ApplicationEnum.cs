using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VMS.Helper
{
    /// <summary>
    /// database record status
    /// </summary>
    public enum Status
    {
        Deleted = 0,
        Active = 1,
        ApprovalPending = 2,
        Rejected = 3,
        NotExist = 99,
    }

    /// <summary>
    /// transaction message status
    /// </summary>
    public enum TransactionStatus
    {
        Error = 0,
        Success = 1,
        InvalidRequest = 2,
        Warning = 3,
        Information = 4,
        Failed = 5
    }

    /// <summary>
    /// User type
    /// </summary>
    public enum UserType
    {
        None = 0,
        ERikshawDriver = 2,
        User = 1,
        BusDriver = 3,
        Officer = 4,
        Cleaner = 5,
    }

    public enum MobileApp
    {
        None = 0,
        User = 1,
        ERikshawDriver = 2,
        BusDriver = 3,
        Officer = 4,
        ConsoleApp = 5,
    }

    public enum VehicleTypeAvailable
    {
        User = 1,
        eRickshaw = 2,
        Bus = 3,
        SwmVehicle = 4,
        Other = 5,
        FireTruck = 6
    }

    /// <summary>
    /// MasterData EntryType status
    /// </summary>
    public enum MasterDataEntryType
    {
        None = 0,
        BusStop = 1,
        Bin = 2
    }
    /// <summary>
    /// User type
    /// </summary>
    public enum AdminUserType
    {
        None = 0,
        SuperAdmin = 2,
        RTOAdmin = 3,
        PoliceAdmin = 4,
        AdminUser = 5,
        User = 6
    }
    public enum AppMenus
    {
        Master = 1,
        Manageerickshaw = 2,
        Manage_Dustbin = 3,
        Manage_busstop = 4,
        Citizen = 5,
        ManageSOS = 6,
        Feedback = 7,
        User = 8,
        Admin = 9,
        Manage_SOS = 10,
        Role = 11,
        Role_Permission = 12,
        Dashboard = 13,
        SwmVehicle = 14,
        Reports = 15,
        Trip_Summary = 16,
        Travel_Summary = 17,
        Idle_Summary = 18,
        Stoppage_Summary = 19,
        Vehicle_Status = 20,
        User_SOS_Request = 21,
        Alert_Summary = 22,
        Bus_Deviation = 23,
        Map_View = 24,
        Manage_Routes = 25,
        Manage_Vehicle = 26,
        Manage_Vehicle_Driver = 27,
    }
    public enum SuggestionType
    {
        Suggestion = 2,
        Complain = 1
    }
}