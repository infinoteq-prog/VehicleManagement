using VMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using VMS.Helper;
using VMS.ViewModel;

namespace VMS.Controllers
{
    public class MasterController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly VmsDbContext _context;

        public MasterController(VmsDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult getCountryList()
        {
            return Json(_context.TblCountryMasters.Select(x => new
            {
                Id = x.Id,
                Name = x.CountryName,
            }).ToList());
        }

        [HttpGet]
        public JsonResult getStateList()
        {
            return Json(_context.TblStates.Select(x => new
            {
                Id = x.Id,
                Name = x.StateName,
            }).ToList());
        }

        [HttpGet]
        public JsonResult getDistrictList()
        {
            return Json(_context.TblDistricts.Select(x => new
            {
                Id = x.Id,
                Name = x.DistrictName,
            }).ToList());
        }

        [HttpGet]
        public JsonResult getCityList()
        {
            return Json(_context.TblCities.Select(x => new
            {
                Id = x.Id,
                Name = x.CityName,
            }).ToList());
        }

        [HttpGet]
        public JsonResult getCompanyMasterList()
        {
            return Json(_context.TblBillToMasters.Select(x => new
            {
                Id = x.Id,
                CompanyName = x.BillToCompany + " - " + x.BillToCode,
                CompanyCode = x.BillToCode,
                IsActive = x.IsActive
            }).Where(p => p.IsActive == true).ToList());
        }

        [HttpGet]
        public JsonResult getStateByCountryId(Int32 countryId)
        {
            return Json(_context.TblStates.Select(x => new
            {
                Id = x.Id,
                Name = x.StateName,
                CountryId = x.CountryId
            }).Where(p => p.CountryId == countryId).ToList());
        }

        [HttpGet]
        public JsonResult getDistrictByStateId(Int32 stateId)
        {
            return Json(_context.TblDistricts.Select(x => new
            {
                Id = x.Id,
                Name = x.DistrictName,
                StateId = x.StateId
            }).Where(p => p.StateId == stateId).ToList());
        }

        [HttpGet]
        public JsonResult getCityByDisctrictId(Int32 districtId)
        {
            return Json(_context.TblCities.Select(x => new
            {
                Id = x.Id,
                Name = x.CityName,
                DistrictId = x.DistrictId
            }).Where(p => p.DistrictId == districtId).ToList());
        }

        [HttpGet]
        public JsonResult getUserRole(int userId)
        {
            return Json(_context.TblUserMasters.Select(x => new
            {
                Id = x.Id,
                RoleId = x.RoleId
            }).Where(p => p.Id == userId).FirstOrDefault());
        }

        [HttpGet]
        public JsonResult getCodeMaster(string codeType)
        {
            return Json(_context.TblCodeMasters.Select(x => new
            {
                Id = x.Id,
                Code = x.Code,
                CodeType = x.CodeType
            }).Where(p => p.CodeType.Equals(codeType)).ToList());
        }

        [HttpGet]
        public JsonResult getDisctrict(Int32 districtId)
        {
            return Json(_context.TblDistricts.Select(x => new
            {
                Id = x.Id,
                Name = x.DistrictName,
            }).Where(p => p.Id == districtId).ToList());
        }

        [HttpGet]
        public JsonResult getCity(Int32 cityId)
        {
            return Json(_context.TblCities.Select(x => new
            {
                Id = x.Id,
                Name = x.CityName,
            }).Where(p => p.Id == cityId).ToList());
        }

        [HttpGet]
        public JsonResult getTransporterList()
        {
            List<VMTransporterMaster> transporterList = new List<VMTransporterMaster>();
            
            var model = _context.TblTransporterMasters.Select(x => new VMTransporterMaster
            {
                Id = x.Id,
                UserId = x.UserId,
                TransporterCode = x.TransporterCode,
                TransporterName = x.TransporterName + " - " + x.TransporterCode,
            });

            VMLogin userDetails = utilityHelper.getCurrentUserSession();
            if (userDetails != null)
            {
                if (userDetails.RoleName.Equals(SiteConstants.User))
                {
                    model = model.Where(s => s.UserId.Equals(userDetails.Id));
                }
            }

            transporterList = model.ToList();

            if (model.Count() == 0)
            {
                return Json(null);
            }
            else
            {
                return Json(model);
            }
        }
    }
}
