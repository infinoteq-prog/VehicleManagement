using VMS.Helper;
using VMS.Models;
using VMS.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Reflection.Metadata;
using System;
using DocumentFormat.OpenXml.Drawing.Charts;
using Microsoft.IdentityModel.Tokens;

namespace VMS.Controllers
{
    public class ModelMasterController : Controller
    {
        private readonly ILogger<ModelMasterController> _logger;
        private readonly VmsDbContext _context;

        public ModelMasterController(VmsDbContext context)
        {
            _context = context;
        }

        public ActionResult Index()
        {
            //Code for User Access Function
            //VMModelMasterAccess userAcc = HttpContext.Session.GetObjectFromJson<VMModelMasterAccess>("userAccess");
            //if (userAcc != null)
            //{
            //    ViewBag.AddModelMasterFullAccess = userAcc.AddModelMasterFullAccess;
            //    ViewBag.AddModelMasterAddAccess = userAcc.AddModelMasterAddAccess;
            //    ViewBag.AddModelMasterUpdateAccess = userAcc.AddModelMasterUpdateAccess;
            //    ViewBag.AddModelMasterDeleteAccess = userAcc.AddModelMasterDeleteAccess;
            //    ViewBag.AddModelMasterViewAccess = userAcc.AddModelMasterViewAccess;
            //}
            //else
            //{
            //    //Logging out a user because user sesstion is null
            //    return RedirectToAction("Logout", "Login");
            //}
            return View();
        }

        public ActionResult Details(String modelMasterID)
        {
            //Code for User Access Function
            //VMModelMasterAccess userAcc = HttpContext.Session.GetObjectFromJson<VMModelMasterAccess>("userAccess");
            //if (userAcc != null)
            //{
            //    ViewBag.AddModelMasterFullAccess = userAcc.AddModelMasterFullAccess;
            //    ViewBag.AddModelMasterAddAccess = userAcc.AddModelMasterAddAccess;
            //    ViewBag.AddModelMasterUpdateAccess = userAcc.AddModelMasterUpdateAccess;
            //    ViewBag.AddModelMasterDeleteAccess = userAcc.AddModelMasterDeleteAccess;
            //    ViewBag.AddModelMasterViewAccess = userAcc.AddModelMasterViewAccess;
            //}
            //else
            //{
            //    //Logging out a user because user sesstion is null
            //    return RedirectToAction("Logout", "Login");
            //}
            ViewBag.modelMasterID = modelMasterID;
            return View("Details");
        }

        public ActionResult Update(String modelMasterID)
        {
            //Code for User Access Function
            //VMModelMasterAccess userAcc = HttpContext.Session.GetObjectFromJson<VMModelMasterAccess>("userAccess");
            //if (userAcc != null)
            //{
            //    ViewBag.AddModelMasterFullAccess = userAcc.AddModelMasterFullAccess;
            //    ViewBag.AddModelMasterAddAccess = userAcc.AddModelMasterAddAccess;
            //    ViewBag.AddModelMasterUpdateAccess = userAcc.AddModelMasterUpdateAccess;
            //    ViewBag.AddModelMasterDeleteAccess = userAcc.AddModelMasterDeleteAccess;
            //    ViewBag.AddModelMasterViewAccess = userAcc.AddModelMasterViewAccess;
            //}
            //else
            //{
            //    //Logging out a user because user sesstion is null
            //    return RedirectToAction("Logout", "Login");
            //}
            ViewBag.modelMasterID = modelMasterID;
            return View("Update");
        }

        public ActionResult List()
        {
            //Code for User Access Function
            //VMModelMasterAccess userAcc = HttpContext.Session.GetObjectFromJson<VMModelMasterAccess>("userAccess");
            //if (userAcc != null)
            //{
            //    ViewBag.AddModelMasterFullAccess = userAcc.AddModelMasterFullAccess;
            //    ViewBag.AddModelMasterAddAccess = userAcc.AddModelMasterAddAccess;
            //    ViewBag.AddModelMasterUpdateAccess = userAcc.AddModelMasterUpdateAccess;
            //    ViewBag.AddModelMasterDeleteAccess = userAcc.AddModelMasterDeleteAccess;
            //    ViewBag.AddModelMasterViewAccess = userAcc.AddModelMasterViewAccess;
            //}
            //else
            //{
            //    //Logging out a user because user sesstion is null
            //    return RedirectToAction("Logout", "Login");
            //}
            return View();
        }

        [HttpGet]
        public JsonResult getModelMaster()
        {
            return Json(_context.TblModelAverageMasters.Select(x => new
            {
                ModelId = x.Id,
                ModelName = x.ModelNo,
            }).ToList());
        }

        [HttpGet]
        public JsonResult getModelMasterByID(string modelMasterID)
        {
            VMModelAverageMaster model = new VMModelAverageMaster();
            int modelMasterId = Convert.ToInt32(modelMasterID);
            model = _context.TblModelAverageMasters.Where(x => x.Id == modelMasterId).Select(x => new VMModelAverageMaster
            {
                Id = x.Id,
                MakeId = x.MakeId,
                ModelNo = x.ModelNo,
                UlAvg = x.UlAvg,
                MegaHw = x.MegaHw,
                Khali = x.Khali,
                Nh = x.Nh,
                OffRoad = x.OffRoad,
                OverLoad = x.OverLoad,
                Other = x.Other,
                CreatedOn = x.CreationDate,
                UpdatedOn = x.UpdateDate,
                IsActive = x.IsActive,
                CreationDate = x.CreationDate,
                UpdateDate = x.UpdateDate,
                CreatedBy = x.CreatedBy,
                UpdatedBy = x.UpdatedBy,
                MakeName = _context.TblCodeMasters
                            .Where(p => p.Id == x.MakeId)
                            .Select(p => p.Description).FirstOrDefault(),
                CreatedByName = _context.TblUserMasters
                            .Where(p => p.Id == x.CreatedBy)
                            .Select(p => p.UserName).FirstOrDefault(),
                UpdatedByName = _context.TblUserMasters
                            .Where(p => p.Id == x.UpdatedBy)
                            .Select(p => p.UserName).FirstOrDefault()
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
        public JsonResult getModelMasterList()
        {
            List<VMModelAverageMaster> model = new List<VMModelAverageMaster>();

            model = _context.TblModelAverageMasters.Select(x => new VMModelAverageMaster
            {
                Id = x.Id,
                MakeId = x.MakeId,
                ModelNo = x.ModelNo,
                UlAvg = x.UlAvg,
                MegaHw = x.MegaHw,
                Khali = x.Khali,
                Nh = x.Nh,
                OffRoad = x.OffRoad,
                OverLoad = x.OverLoad,
                Other = x.Other,
                CreatedOn = x.CreationDate,
                UpdatedOn = x.UpdateDate,
                IsActive = x.IsActive,
                CreationDate = x.CreationDate,
                UpdateDate = x.UpdateDate,
                CreatedBy = x.CreatedBy,
                UpdatedBy = x.UpdatedBy,
                MakeName = _context.TblCodeMasters
                            .Where(p => p.Id == x.MakeId)
                            .Select(p => p.Description).FirstOrDefault(),
                CreatedByName = _context.TblUserMasters
                            .Where(p => p.Id == x.CreatedBy)
                            .Select(p => p.UserName).FirstOrDefault(),
                UpdatedByName = _context.TblUserMasters
                            .Where(p => p.Id == x.UpdatedBy)
                            .Select(p => p.UserName).FirstOrDefault()
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
        public JsonResult searchModelMaster(int makeId, int modelNo, decimal ulAvg, decimal megaHW, decimal khali,
                                            decimal nh, decimal offRoad, decimal overLoad, decimal other, Boolean isActive)
        {
            List<VMModelAverageMaster> model = new List<VMModelAverageMaster>();

            var searchModel = _context.TblModelAverageMasters.Select(x => new VMModelAverageMaster
            {
                Id = x.Id,
                MakeId = x.MakeId,
                ModelNo = x.ModelNo,
                UlAvg = x.UlAvg,
                MegaHw = x.MegaHw,
                Khali = x.Khali,
                Nh = x.Nh,
                OffRoad = x.OffRoad,
                OverLoad = x.OverLoad,
                Other = x.Other,
                CreatedOn = x.CreationDate,
                UpdatedOn = x.UpdateDate,
                IsActive = x.IsActive,
                CreationDate = x.CreationDate,
                UpdateDate = x.UpdateDate,
                CreatedBy = x.CreatedBy,
                UpdatedBy = x.UpdatedBy,
                MakeName = _context.TblCodeMasters
                            .Where(p => p.Id == x.MakeId)
                            .Select(p => p.Description).FirstOrDefault(),
                CreatedByName = _context.TblUserMasters
                            .Where(p => p.Id == x.CreatedBy)
                            .Select(p => p.UserName).FirstOrDefault(),
                UpdatedByName = _context.TblUserMasters
                            .Where(p => p.Id == x.UpdatedBy)
                            .Select(p => p.UserName).FirstOrDefault()
            });

            if (makeId != 0)
            {
                searchModel = searchModel.Where(s => s.MakeId == makeId);
            }

            if (modelNo != 0)
            {
                searchModel = searchModel.Where(s => s.ModelNo == modelNo);
            }

            if (!string.IsNullOrEmpty(Convert.ToString(ulAvg)))
            {
                searchModel = searchModel.Where(s => s.UlAvg.Equals(ulAvg));
            }

            if (!string.IsNullOrEmpty(Convert.ToString(megaHW)))
            {
                searchModel = searchModel.Where(s => s.MegaHw.Equals(megaHW));
            }

            if (!string.IsNullOrEmpty(Convert.ToString(khali)))
            {
                searchModel = searchModel.Where(s => s.Khali.Equals(khali));
            }

            if (!string.IsNullOrEmpty(Convert.ToString(nh)))
            {
                searchModel = searchModel.Where(s => s.Nh.Equals(nh));
            }

            if (!string.IsNullOrEmpty(Convert.ToString(offRoad)))
            {
                searchModel = searchModel.Where(s => s.OffRoad.Equals(offRoad));
            }

            if (!string.IsNullOrEmpty(Convert.ToString(overLoad)))
            {
                searchModel = searchModel.Where(s => s.OverLoad.Equals(overLoad));
            }

            if (!string.IsNullOrEmpty(Convert.ToString(other)))
            {
                searchModel = searchModel.Where(s => s.OverLoad.Equals(other));
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
        public ActionResult Save(int makeId, int modelNo, decimal ulAvg, decimal megaHW, decimal khali,
                                 decimal nh, decimal offRoad, decimal overLoad, decimal other, Boolean isActive)
        {
            VMModelAverageMaster model = new VMModelAverageMaster();
            int userID = 0;
            VMLogin userDetails = HttpContext.Session.GetObjectFromJson<VMLogin>("userDetails");
            if (userDetails != null)
            {
                userID = userDetails.Id;
                var user = _context.TblModelAverageMasters.Select(x => new
                {
                    Id = x.Id,
                    MakeId = x.MakeId,
                    ModelNo = x.ModelNo,
                    UlAvg = x.UlAvg,
                    MegaHw = x.MegaHw,
                    Khali = x.Khali,
                    Nh = x.Nh,
                    OffRoad = x.OffRoad,
                    OverLoad = x.OverLoad,
                    Other = x.Other,
                    CreatedOn = x.CreationDate,
                    UpdatedOn = x.UpdateDate,
                    IsActive = x.IsActive,
                    CreationDate = x.CreationDate,
                    UpdateDate = x.UpdateDate,
                    CreatedBy = x.CreatedBy,
                    UpdatedBy = x.UpdatedBy,
                    MakeName = _context.TblCodeMasters
                            .Where(p => p.Id == x.MakeId)
                            .Select(p => p.Description).FirstOrDefault(),
                    CreatedByName = _context.TblUserMasters
                            .Where(p => p.Id == x.CreatedBy)
                            .Select(p => p.UserName).FirstOrDefault(),
                    UpdatedByName = _context.TblUserMasters
                            .Where(p => p.Id == x.UpdatedBy)
                            .Select(p => p.UserName).FirstOrDefault()
                }).Where(x => x.ModelNo == modelNo).ToList();

                if (user.Count() == 0)
                {
                    try
                    {
                        //Insert Into Model Master
                        var modelMaster = new TblModelAverageMaster();
                        //modelMaster.Id = id;
                        modelMaster.MakeId = makeId;
                        modelMaster.ModelNo = modelNo;
                        modelMaster.UlAvg = Math.Round(ulAvg, 2);
                        modelMaster.MegaHw = Math.Round(megaHW, 2);
                        modelMaster.Khali = Math.Round(khali, 2);
                        modelMaster.Nh = Math.Round(nh, 2);
                        modelMaster.OffRoad = Math.Round(offRoad, 2);
                        modelMaster.OverLoad = Math.Round(overLoad, 2);
                        modelMaster.Other = Math.Round(other, 2);
                        modelMaster.IsActive = isActive;
                        modelMaster.CreationDate = utilityHelper.CurrentDateTime;
                        modelMaster.UpdateDate = utilityHelper.CurrentDateTime;
                        modelMaster.CreatedBy = userID;
                        modelMaster.UpdatedBy = userID;

                        _context.TblModelAverageMasters.Add(modelMaster);
                        _context.SaveChanges();
                        model.Id = modelMaster.Id;

                        model.TransactionMessage.Status = TransactionStatus.Success;
                        model.TransactionMessage.Message = "Model Master has been saved successfully.";
                    }
                    catch (Exception ex)
                    {
                        model.TransactionMessage.Status = TransactionStatus.Error;
                        model.TransactionMessage.Message = "Model Master not saved due to some technical Issue. Please try again.";
                    }
                }
                else
                {
                    model.TransactionMessage.Status = TransactionStatus.Failed;
                    model.TransactionMessage.Message = "Model Master Already Exist! Please try again with diffrent username.";
                }
            }
            else
            {
                return RedirectToAction("Logout", "Login");
            }

            return Json(model);
        }

        [HttpPost]
        public ActionResult Update(int id, int makeId, int modelNo, decimal ulAvg, decimal megaHW, decimal khali,
                                   decimal nh, decimal offRoad, decimal overLoad, decimal other, Boolean isActive)
        {
            VMModelAverageMaster model = new VMModelAverageMaster();
            int userID = 0;
            VMLogin userDetails = HttpContext.Session.GetObjectFromJson<VMLogin>("userDetails");
            if (userDetails != null)
            {
                userID = userDetails.Id;
                try
                {
                    var modelMaster = _context.TblModelAverageMasters.Where(x => x.Id == Convert.ToInt32(id)).FirstOrDefault();
                    if (modelMaster == null)
                    {
                        model.TransactionMessage.Status = TransactionStatus.Error;
                        model.TransactionMessage.Message = "Model Master does not Exists! Please check and try again.";
                        return Json(model);
                    }
                    else
                    {
                        //Updateing Existing User Details
                        modelMaster.MakeId = makeId;
                        modelMaster.ModelNo = modelNo;
                        modelMaster.UlAvg = Math.Round(ulAvg, 2);
                        modelMaster.MegaHw = Math.Round(megaHW, 2);
                        modelMaster.Khali = Math.Round(khali, 2);
                        modelMaster.Nh = Math.Round(nh, 2);
                        modelMaster.OffRoad = Math.Round(offRoad, 2);
                        modelMaster.OverLoad = Math.Round(overLoad, 2);
                        modelMaster.Other = Math.Round(other, 2);
                        modelMaster.IsActive = isActive;
                        //modelMaster.CreateDate = utilityHelper.CurrentDateTime;
                        modelMaster.UpdateDate = utilityHelper.CurrentDateTime;
                        //modelMaster.CreatedBy = userID;
                        modelMaster.UpdatedBy = userID;

                        _context.TblModelAverageMasters.Update(modelMaster);
                        _context.SaveChanges();
                        model.Id = id;
                    }
                    model.TransactionMessage.Status = TransactionStatus.Success;
                    model.TransactionMessage.Message = "Model Master has been updated successfully.";
                }
                catch (Exception ex)
                {
                    model.TransactionMessage.Status = TransactionStatus.Error;
                    model.TransactionMessage.Message = "Model Master not saved due to some technical Issue. Please try again.";
                }
            }
            else
            {
                return RedirectToAction("Logout", "Login");
            }
            return Json(model);
        }

        [HttpPost]
        public JsonResult deleteModelMaster(string modelMasterID)
        {
            VMModelAverageMaster model = new VMModelAverageMaster();
            try
            {
                var modelMaster = _context.TblModelAverageMasters.Where(x => x.Id == Convert.ToInt32(modelMasterID));
                _context.TblModelAverageMasters.RemoveRange(modelMaster);
                _context.SaveChanges();

                model.TransactionMessage.Status = TransactionStatus.Success;
                model.TransactionMessage.Message = "Model Master has been deleted successfully.";
            }
            catch (Exception ex)
            {
                model.TransactionMessage.Status = TransactionStatus.Error;
                model.TransactionMessage.Message = "Model Master has not been deleted. Please try again.";
            }
            return Json(model);
        }

        [HttpGet]
        public JsonResult getTopModelMasterList()
        {
            List<VMModelAverageMaster> model = new List<VMModelAverageMaster>();

            //Query to fetch last 10 saved records in trasporter bill table
            model = _context.TblModelAverageMasters.Select(x => new VMModelAverageMaster
            {
                Id = x.Id,
                MakeId = x.MakeId,
                ModelNo = x.ModelNo,
                UlAvg = x.UlAvg,
                MegaHw = x.MegaHw,
                Khali = x.Khali,
                Nh = x.Nh,
                OffRoad = x.OffRoad,
                OverLoad = x.OverLoad,
                Other = x.Other,
                CreatedOn = x.CreationDate,
                UpdatedOn = x.UpdateDate,
                IsActive = x.IsActive,
                CreationDate = x.CreationDate,
                UpdateDate = x.UpdateDate,
                CreatedBy = x.CreatedBy,
                UpdatedBy = x.UpdatedBy,
                MakeName = _context.TblCodeMasters
                            .Where(p => p.Id == x.MakeId)
                            .Select(p => p.Description).FirstOrDefault(),
                CreatedByName = _context.TblUserMasters
                            .Where(p => p.Id == x.CreatedBy)
                            .Select(p => p.UserName).FirstOrDefault(),
                UpdatedByName = _context.TblUserMasters
                            .Where(p => p.Id == x.UpdatedBy)
                            .Select(p => p.UserName).FirstOrDefault()
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
