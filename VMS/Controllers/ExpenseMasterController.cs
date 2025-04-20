using VMS.Helper;
using VMS.Models;
using VMS.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Reflection.Metadata;
using System.Reflection.Emit;

namespace VMS.Controllers
{
    public class ExpenseMasterController : Controller
    {
        private readonly ILogger<ExpenseMasterController> _logger;
        private readonly VmsDbContext _context;

        public ExpenseMasterController(VmsDbContext context)
        {
            _context = context;
        }

        public ActionResult Index()
        {
            //Code for User Access Function
            //VMExpenseMasterAccess userAcc = HttpContext.Session.GetObjectFromJson<VMExpenseMasterAccess>("userAccess");
            //if (userAcc != null)
            //{
            //    ViewBag.AddExpenseMasterFullAccess = userAcc.AddExpenseMasterFullAccess;
            //    ViewBag.AddExpenseMasterAddAccess = userAcc.AddExpenseMasterAddAccess;
            //    ViewBag.AddExpenseMasterUpdateAccess = userAcc.AddExpenseMasterUpdateAccess;
            //    ViewBag.AddExpenseMasterDeleteAccess = userAcc.AddExpenseMasterDeleteAccess;
            //    ViewBag.AddExpenseMasterViewAccess = userAcc.AddExpenseMasterViewAccess;
            //}
            //else
            //{
            //    //Logging out a user because user sesstion is null
            //    return RedirectToAction("Logout", "Login");
            //}
            return View();
        }

        public ActionResult Details(String expenseMasterId)
        {
            //Code for User Access Function
            //VMExpenseMasterAccess userAcc = HttpContext.Session.GetObjectFromJson<VMExpenseMasterAccess>("userAccess");
            //if (userAcc != null)
            //{
            //    ViewBag.AddExpenseMasterFullAccess = userAcc.AddExpenseMasterFullAccess;
            //    ViewBag.AddExpenseMasterAddAccess = userAcc.AddExpenseMasterAddAccess;
            //    ViewBag.AddExpenseMasterUpdateAccess = userAcc.AddExpenseMasterUpdateAccess;
            //    ViewBag.AddExpenseMasterDeleteAccess = userAcc.AddExpenseMasterDeleteAccess;
            //    ViewBag.AddExpenseMasterViewAccess = userAcc.AddExpenseMasterViewAccess;
            //}
            //else
            //{
            //    //Logging out a user because user sesstion is null
            //    return RedirectToAction("Logout", "Login");
            //}
            ViewBag.expenseMasterId = expenseMasterId;
            return View("Details");
        }

        public ActionResult Update(String expenseMasterId)
        {
            //Code for User Access Function
            //VMExpenseMasterAccess userAcc = HttpContext.Session.GetObjectFromJson<VMExpenseMasterAccess>("userAccess");
            //if (userAcc != null)
            //{
            //    ViewBag.AddExpenseMasterFullAccess = userAcc.AddExpenseMasterFullAccess;
            //    ViewBag.AddExpenseMasterAddAccess = userAcc.AddExpenseMasterAddAccess;
            //    ViewBag.AddExpenseMasterUpdateAccess = userAcc.AddExpenseMasterUpdateAccess;
            //    ViewBag.AddExpenseMasterDeleteAccess = userAcc.AddExpenseMasterDeleteAccess;
            //    ViewBag.AddExpenseMasterViewAccess = userAcc.AddExpenseMasterViewAccess;
            //}
            //else
            //{
            //    //Logging out a user because user sesstion is null
            //    return RedirectToAction("Logout", "Login");
            //}
            ViewBag.expenseMasterId = expenseMasterId;
            return View("Update");
        }

        public ActionResult List()
        {
            //Code for User Access Function
            //VMExpenseMasterAccess userAcc = HttpContext.Session.GetObjectFromJson<VMExpenseMasterAccess>("userAccess");
            //if (userAcc != null)
            //{
            //    ViewBag.AddExpenseMasterFullAccess = userAcc.AddExpenseMasterFullAccess;
            //    ViewBag.AddExpenseMasterAddAccess = userAcc.AddExpenseMasterAddAccess;
            //    ViewBag.AddExpenseMasterUpdateAccess = userAcc.AddExpenseMasterUpdateAccess;
            //    ViewBag.AddExpenseMasterDeleteAccess = userAcc.AddExpenseMasterDeleteAccess;
            //    ViewBag.AddExpenseMasterViewAccess = userAcc.AddExpenseMasterViewAccess;
            //}
            //else
            //{
            //    //Logging out a user because user sesstion is null
            //    return RedirectToAction("Logout", "Login");
            //}
            return View();
        }

        [HttpGet]
        public JsonResult getExpenseMaster()
        {
            return Json(_context.TblExpenseMasters.Select(x => new
            {
                ExpenseId = x.Id,
                ExpCode = x.ExpCode,
                ExpType = x.ExpType,
                ExpOther = x.ExpOther,
                ExpDescription = x.ExpDescription
            }).ToList());
        }

        [HttpGet]
        public JsonResult getExpenseMasterByID(string expenseMasterId)
        {
            VMExpenseMaster model = new VMExpenseMaster();
            int ExpenseMasterId = Convert.ToInt32(expenseMasterId);
            model = _context.TblExpenseMasters.Where(x => x.Id.Equals(ExpenseMasterId)).Select(x => new VMExpenseMaster
            {
                Id = x.Id,
                ExpCode = x.ExpCode,
                ExpType = x.ExpType,
                ExpOther = x.ExpOther,
                ExpDescription = x.ExpDescription,
                IsActive = x.IsActive,
                CreationDate = x.CreationDate,
                UpdateDate = x.UpdateDate,
                CreatedBy = x.CreatedBy,
                UpdatedBy = x.UpdatedBy,
                CreatedByName = _context.TblUserMasters
                            .Where(p => p.Id == x.CreatedBy)
                            .Select(p => p.UserName).FirstOrDefault(),
                UpdatedByName = _context.TblUserMasters
                            .Where(p => p.Id == x.UpdatedBy)
                            .Select(p => p.UserName).FirstOrDefault(),
                ExpTypeName = _context.TblCodeMasters
                            .Where(p => p.Id == x.ExpType)
                            .Select(p => p.Description).FirstOrDefault(),
            }).FirstOrDefault();

            if (model == null)
            {
                return Json(null);
            }
            else
            {
                return Json(model);
            }
        }

        [HttpGet]
        public JsonResult getExpenseMasterList()
        {
            List<VMExpenseMaster> model = new List<VMExpenseMaster>();

            model = _context.TblExpenseMasters.Select(x => new VMExpenseMaster
            {
                Id = x.Id,
                ExpCode = x.ExpCode,
                ExpType = x.ExpType,
                ExpOther = x.ExpOther,
                ExpDescription = x.ExpDescription,
                IsActive = x.IsActive,
                CreationDate = x.CreationDate,
                UpdateDate = x.UpdateDate,
                CreatedBy = x.CreatedBy,
                UpdatedBy = x.UpdatedBy,
                CreatedByName = _context.TblUserMasters
                            .Where(p => p.Id == x.CreatedBy)
                            .Select(p => p.UserName).FirstOrDefault(),
                UpdatedByName = _context.TblUserMasters
                            .Where(p => p.Id == x.UpdatedBy)
                            .Select(p => p.UserName).FirstOrDefault(),
                ExpTypeName = _context.TblCodeMasters
                            .Where(p => p.Id == x.ExpType)
                            .Select(p => p.Description).FirstOrDefault()
            }).OrderByDescending(n => n.Id).ToList();

            if (model.Count() == 0)
            {
                return Json(null);
            }
            else
            {
                return Json(model);
            }
        }

        [HttpGet]
        public JsonResult searchExpenseMaster(string expCode, int expType, string expOther, string expDescription, Boolean isActive)
        {
            List<VMExpenseMaster> model = new List<VMExpenseMaster>();

            var searchModel = _context.TblExpenseMasters.Select(x => new VMExpenseMaster
            {
                Id = x.Id,
                ExpCode = x.ExpCode,
                ExpType = x.ExpType,
                ExpOther = x.ExpOther,
                ExpDescription = x.ExpDescription,
                IsActive = x.IsActive,
                CreationDate = x.CreationDate,
                UpdateDate = x.UpdateDate,
                CreatedBy = x.CreatedBy,
                UpdatedBy = x.UpdatedBy,
                CreatedByName = _context.TblUserMasters
                            .Where(p => p.Id == x.CreatedBy)
                            .Select(p => p.UserName).FirstOrDefault(),
                UpdatedByName = _context.TblUserMasters
                            .Where(p => p.Id == x.UpdatedBy)
                            .Select(p => p.UserName).FirstOrDefault(),
                ExpTypeName = _context.TblCodeMasters
                            .Where(p => p.Id == x.ExpType)
                            .Select(p => p.Description).FirstOrDefault()
            });

            if (!string.IsNullOrEmpty(expCode))
            {
                searchModel = searchModel.Where(s => s.ExpCode.Equals(expCode));
            }

            if (expType != 0)
            {
                searchModel = searchModel.Where(s => s.ExpType.Equals(expType));
            }

            if (!string.IsNullOrEmpty(expOther))
            {
                searchModel = searchModel.Where(s => s.ExpOther.Equals(expOther));
            }

            if (!string.IsNullOrEmpty(expDescription))
            {
                searchModel = searchModel.Where(s => s.ExpDescription.Equals(expDescription));
            }

            //searchModel = searchModel.Where(s => s.IsActive == isActive);

            model = searchModel.ToList();

            if (model.Count() == 0)
            {
                return Json(null);
            }
            else
            {
                return Json(model);
            }
        }

        [HttpPost]
        public ActionResult Save(string expCode, int expType, string expOther, string expDescription, Boolean isActive)
        {
            VMExpenseMaster model = new VMExpenseMaster();
            int userID = 0;
            VMLogin userDetails = HttpContext.Session.GetObjectFromJson<VMLogin>("userDetails");
            if (userDetails != null)
            {
                userID = userDetails.Id;
                var user = _context.TblExpenseMasters.Select(x => new
                {
                    Id = x.Id,
                    ExpCode = x.ExpCode,
                    ExpType = x.ExpType,
                    ExpOther = x.ExpOther,
                    ExpDescription = x.ExpDescription,
                    IsActive = x.IsActive,
                    CreationDate = x.CreationDate,
                    UpdateDate = x.UpdateDate,
                    CreatedBy = x.CreatedBy,
                    UpdatedBy = x.UpdatedBy,
                    CreatedByName = _context.TblUserMasters
                            .Where(p => p.Id == x.CreatedBy)
                            .Select(p => p.UserName).FirstOrDefault(),
                    UpdatedByName = _context.TblUserMasters
                            .Where(p => p.Id == x.UpdatedBy)
                            .Select(p => p.UserName).FirstOrDefault(),
                    ExpTypeName = _context.TblCodeMasters
                            .Where(p => p.Id == x.ExpType)
                            .Select(p => p.Description).FirstOrDefault()
                }).Where(x => x.ExpCode.Equals(expCode)).ToList();

                if (user.Count() == 0)
                {
                    try
                    {
                        //Insert Into Expense Master
                        var ExpenseMaster = new TblExpenseMaster();
                        //ExpenseMaster.Id = id;
                        ExpenseMaster.ExpCode = expCode;
                        ExpenseMaster.ExpType = expType;
                        ExpenseMaster.ExpOther = expOther;
                        ExpenseMaster.ExpDescription = expDescription;
                        ExpenseMaster.IsActive = isActive;
                        ExpenseMaster.CreationDate = utilityHelper.CurrentDateTime;
                        ExpenseMaster.UpdateDate = utilityHelper.CurrentDateTime;
                        ExpenseMaster.CreatedBy = userID;
                        ExpenseMaster.UpdatedBy = userID;

                        _context.TblExpenseMasters.Add(ExpenseMaster);
                        _context.SaveChanges();
                        model.Id = ExpenseMaster.Id;

                        model.TransactionMessage.Status = TransactionStatus.Success;
                        model.TransactionMessage.Message = "Expense Master has been saved successfully.";
                    }
                    catch (Exception ex)
                    {
                        model.TransactionMessage.Status = TransactionStatus.Error;
                        model.TransactionMessage.Message = "Expense Master not saved due to some technical Issue. Please try again.";
                    }
                }
                else
                {
                    model.TransactionMessage.Status = TransactionStatus.Failed;
                    model.TransactionMessage.Message = "Expense Master Already Exist! Please try again with diffrent username.";
                }
            }
            else
            {
                return RedirectToAction("Logout", "Login");
            }

            return Json(model);
        }

        [HttpPost]
        public ActionResult Update(int id, string expCode, int expType, string expOther, string expDescription, Boolean isActive)
        {
            VMExpenseMaster model = new VMExpenseMaster();
            int userID = 0;
            VMLogin userDetails = HttpContext.Session.GetObjectFromJson<VMLogin>("userDetails");
            if (userDetails != null)
            {
                userID = userDetails.Id;
                try
                {
                    var ExpenseMaster = _context.TblExpenseMasters.Where(x => x.Id == Convert.ToInt32(id)).FirstOrDefault();
                    if (ExpenseMaster == null)
                    {
                        model.TransactionMessage.Status = TransactionStatus.Error;
                        model.TransactionMessage.Message = "Expense Master does not Exists! Please check and try again.";
                        return Json(model);
                    }
                    else
                    {
                        //Updateing Existing User Details
                        ExpenseMaster.ExpCode = expCode;
                        ExpenseMaster.ExpType = expType;
                        ExpenseMaster.ExpOther = expOther;
                        ExpenseMaster.ExpDescription = expDescription;
                        ExpenseMaster.IsActive = isActive;
                        //ExpenseMaster.CreateDate = utilityHelper.CurrentDateTime;
                        ExpenseMaster.UpdateDate = utilityHelper.CurrentDateTime;
                        //ExpenseMaster.CreatedBy = userID;
                        ExpenseMaster.UpdatedBy = userID;

                        _context.TblExpenseMasters.Update(ExpenseMaster);
                        _context.SaveChanges();
                        model.Id = id;
                    }
                    model.TransactionMessage.Status = TransactionStatus.Success;
                    model.TransactionMessage.Message = "Expense Master has been updated successfully.";
                }
                catch (Exception ex)
                {
                    model.TransactionMessage.Status = TransactionStatus.Error;
                    model.TransactionMessage.Message = "Expense Master not saved due to some technical Issue. Please try again.";
                }
            }
            else
            {
                return RedirectToAction("Logout", "Login");
            }
            return Json(model);
        }

        [HttpPost]
        public JsonResult deleteExpenseMaster(string expenseMasterId)
        {
            VMExpenseMaster model = new VMExpenseMaster();
            try
            {
                var ExpenseMaster = _context.TblExpenseMasters.Where(x => x.Id == Convert.ToInt32(expenseMasterId));
                _context.TblExpenseMasters.RemoveRange(ExpenseMaster);
                _context.SaveChanges();

                model.TransactionMessage.Status = TransactionStatus.Success;
                model.TransactionMessage.Message = "Expense Master has been deleted successfully.";
            }
            catch (Exception ex)
            {
                model.TransactionMessage.Status = TransactionStatus.Error;
                model.TransactionMessage.Message = "Expense Master has not been deleted. Please try again.";
            }
            return Json(model);
        }

        [HttpGet]
        public JsonResult getTopExpenseMasterList()
        {
            List<VMExpenseMaster> model = new List<VMExpenseMaster>();

            //Query to fetch last 10 saved records in trasporter bill table
            model = _context.TblExpenseMasters.Select(x => new VMExpenseMaster
            {
                Id = x.Id,
                ExpCode = x.ExpCode,
                ExpType = x.ExpType,
                ExpOther = x.ExpOther,
                ExpDescription = x.ExpDescription,
                IsActive = x.IsActive,
                CreationDate = x.CreationDate,
                UpdateDate = x.UpdateDate,
                CreatedBy = x.CreatedBy,
                UpdatedBy = x.UpdatedBy,
                CreatedByName = _context.TblUserMasters
                            .Where(p => p.Id == x.CreatedBy)
                            .Select(p => p.UserName).FirstOrDefault(),
                UpdatedByName = _context.TblUserMasters
                            .Where(p => p.Id == x.UpdatedBy)
                            .Select(p => p.UserName).FirstOrDefault(),
                ExpTypeName = _context.TblCodeMasters
                            .Where(p => p.Id == x.ExpType)
                            .Select(p => p.Description).FirstOrDefault()
            }).OrderByDescending(n => n.Id).Take(10).ToList();

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
