using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Assignmnet9.Controllers
{
   public class CoachAdd
    {
        public CoachAdd()
        {
            Name = "";
        }

        [Key]
        public int CoachId { get; set; }

        [Required]
        [Display(Name = "Coach Name")]
        public string Name { get; set; }

        [Display(Name = "Coaching Team")]
        public string TeamName { get; set; }

        [Display(Name = "Win Percentage")]
        public decimal WinPercentage { get; set; }

        [Display(Name = "Number of Games Coached (All Seasons)")]
        public int NumberOfGamesCoached { get; set; }

        [Display(Name = "Number of Seasons Coached")]
        public int YearsCoached { get; set; }
    }

    public class CoachBase : CoachAdd
    {

    }

    public class CoachWithDetail : CoachBase
    {
        public CoachWithDetail()
        {
            CoachProfile = "";
        }


        [Display(Name = "Profile of Coach")]
        [DataType(DataType.MultilineText)]
        public string CoachProfile { get; set; }

        public string CoachURL
        {
            get
            {
                return $"/photo/{CoachId}";
            }
        }
    }

    public class CoachCreateForm : CoachBase
    {
        public CoachCreateForm()
        {

        }

        [Range(1, Int32.MaxValue)]
        public int TeamId { get; set; }

        [Display(Name = "Profile of Coach")]
        [DataType(DataType.MultilineText)]
        public string CoachProfile { get; set; }

        [Required]
        [Display(Name = "Player Photo")]
        [DataType(DataType.Upload)]
        public string PhotoUpload { get; set; }
    }

    public class CoachCreate : CoachCreateForm
    {
        [Required]
        public HttpPostedFileBase PhotoUpload { get; set; }

    }

    public class CoachPhoto
    {
        public int Id { get; set; }
        public string PhotoContentType { get; set; }
        public byte[] Photo { get; set; }
    }

    public class CoachEditForm : CoachBase
    {
        public CoachEditForm()
        {
            CoachProfile = "";
        }

        [Display(Name = "Profile of Coach")]
        [DataType(DataType.MultilineText)]
        public string CoachProfile { get; set; }

        [Required]
        [Display(Name = "Player Photo")]
        [DataType(DataType.Upload)]
        public string PhotoUpload { get; set; }
    }

    public class CoachEdit : CoachEditForm
    {
        public CoachEdit()
        {

        }

        [Required]
        public HttpPostedFileBase PhotoUpload { get; set; }
    }
}
