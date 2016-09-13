using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using ReqTrackit.website.Models.ReqTrackModel;
using ReqTrackit.website.Models.ViewModels;

namespace ReqTrackit.website.Controllers.Request
{
    //[Authorize]
    public class RequestController : Controller
    {
       
        private ReqTrackEnities db=new ReqTrackEnities();
        // GET: Request
        public ActionResult Index()
        {
            var model = new SubmitRequestViewModel();
            return View(model);
        }

        
        public ActionResult About()
        {
            return View();
        }

        public ActionResult RequestDetails(int requestid)
        {
            var reqTracker = db.request_tracker.Find(requestid);
            var nugetReqTracker = db.nuget_request_tracker.Find(reqTracker.DetailsID);
            var reqDetailsViewModel = new RequestDetailsViewModel
            {
                RequestId = requestid,
                PackageName = db.nuget_request_tracker.Join(db.package_details, nrt => nrt.PackageID, pd => pd.Id, (nrt, pd) => new { nrt, pd }).Where(i => i.nrt.Id == reqTracker.DetailsID).Select(k => k.pd.Name).FirstOrDefault(),
                TfsBranchName = nugetReqTracker.TFS_BranchName,
                TfsChangeSet = nugetReqTracker.TFS_changeset,
                VersionRequested = nugetReqTracker.Version_requested,
                Description = nugetReqTracker.Description
            };
            
            
            return PartialView("_RequestDetails",reqDetailsViewModel);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult SubmitRequest(SubmitRequestViewModel model)
        {
            var userid = User.Identity.GetUserId();
            var status = db.request_status.ToList();
            ViewBag.Message = "Inserted Sucessfully";
            var id = Guid.NewGuid().ToString();
            var firstOrDefault = status.FirstOrDefault(i=>i.Name=="RequestMade");
            if (firstOrDefault != null)
            {

                var nugetrequest = new nuget_request_tracker
                {
                    Id = id,
                    PackageID = model.PackageId,
                    TFS_BranchName = model.TfsBranchName,
                    TFS_changeset = model.TfsChangeSet,
                    Version_requested = model.VersionRequested,
                    Description = model.Description
                };
                var request=new request_tracker
                {
                    
                    Date_requested = DateTime.Now,
                    TypeID = model.RequestTypeId,
                    StatusID = firstOrDefault.Id,
                    UserID = userid,
                    DetailsID = id
                };
                
            
                db.nuget_request_tracker.Add(nugetrequest);
                db.request_tracker.Add(request);
                
            }
            try
            {
                db.SaveChanges();
                ViewBag.Message = "";
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.InnerException.Message;
                throw;
            }
          

            return RedirectToAction("Index",ViewBag);
        }

        public ActionResult Displayrequest()
        {
            var model = new SubmitRequestViewModel {Requests = new List<RequestViewModel>()};
            var userId = User.Identity.GetUserId();
            var req = new List<request_tracker>();
            foreach (var tracker in db.request_tracker.Where(i => i.UserID == userId))
                req.Add(tracker);

            //var requests = req.Join(db.nuget_request_tracker, i => i.DetailsID, j => j.Id, (i, j) => new { i, j }).
            //                Join(db.request_status, k => k.i.StatusID, rs => rs.Id, (k, rs) => new { k.i, k.j, rs })
            //                .Select(result => new { result.i, result.j, result.rs }).ToList();

            foreach (var r in req)
            {
                var requestViewModel = new RequestViewModel()
                {
                    RequestId = r.Id,
                    RequestType = db.request_type.Where(rt=>rt.Id==r.TypeID).Select(k=>k.Name).FirstOrDefault(),
                    PackageName = db.nuget_request_tracker.Join(db.package_details,nrt=>nrt.PackageID, pd=>pd.Id, (nrt,pd)=>new {nrt,pd} ).Where(i=>i.nrt.Id==r.DetailsID).Select(k=>k.pd.Name).FirstOrDefault(),
                    Status = db.request_status.FirstOrDefault(rs=>rs.Id==r.StatusID).Name,
                    DateRequested = r.Date_requested
                };
                if (r.Date_completed.HasValue)
                {
                    requestViewModel.DateCompleted = r.Date_completed.Value;
                }
                model.Requests.Add(requestViewModel);

            }

            
            return PartialView("_DisplayRequests", model.Requests);
        }
    }
}