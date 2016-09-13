
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ReqTrackit.website.Models.ViewModels
{
    public class SubmitRequestViewModel
    {
        [Required]
        [Display(Name= "Request Type") ]
        public int RequestTypeId { get; set; }

   
        [Required]
        [Display(Name = "Package Name")]
        public int PackageId { get; set; }


        [Required(ErrorMessage = "TFS Branch Name required")]
        [Display(Name= "TFS Branch Name")]
        public string TfsBranchName { get; set; }

        [Required(ErrorMessage ="Change Set required")]
        [Display(Name= "TFS Change Set#")]
        public string TfsChangeSet { get; set; }

        [Required(ErrorMessage= "Version required")]
        [Display(Name= "Version Requested#")]
        public string VersionRequested { get; set; }

        public string Description { get; set; }

        public IList<RequestViewModel> Requests { get; set; }

    }

    public class RequestViewModel
    {
        public int RequestId { get; set; }

        public string RequestType { get; set; }

        //public string UserName { get; set; }

        public string Status { get; set; }

        public string PackageName { get; set; }

        //public string PackageVersion { get; set; }

        public DateTime DateRequested { get; set; }

        public DateTime DateCompleted { get; set; }

    }

    public class RequestDetailsViewModel
    {
        public int RequestId { get; set; }

        public string PackageName { get; set; }

        [Display(Name = "TFS Branch Name")]
        public string TfsBranchName { get; set; }

        [Display(Name = "TFS Change Set#")]
        public string TfsChangeSet { get; set; }
        
        [Display(Name = "Version Requested#")]
        public string VersionRequested { get; set; }

        public string Description { get; set; }


    }

    
    }


 