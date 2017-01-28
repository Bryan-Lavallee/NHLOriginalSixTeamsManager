using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Assignmnet9.Controllers
{
    public class PlayerAdd
    {
        public PlayerAdd()
        {
            Position = "";
            Name = "";
        }

        [Key]
        public int PlayerId { get; set; }
        public int Number { get; set; }
        public string Position { get; set; }
        public string Name { get; set; }
        public int GamesPlayed { get; set; }
        public int Goals { get; set; }
        public int Assists { get; set; }
        public int Points { get; set; }
    }

    public class PlayerBase : PlayerAdd
    {
        public PlayerBase()
        {
            TeamName = "";
        }

        public string TeamName { get; set; }
    }

    public class PlayerWithDetail : PlayerBase
    {
        public PlayerWithDetail()
        {
            Profile = "";
        }

        [Display(Name = "Profile of Player")]
        [DataType(DataType.MultilineText)]
        public string Profile { get; set; }

        public string PlayerURL
        {
            get
            {
                return $"/photo/{PlayerId}";
            }
        }
    }

    public class PlayerWithPosition : PlayerBase
    {
        public int PositionId {get; set; }
        public bool Selected { get; set; }
    }

    public class PlayerCreateForm
    {
        public PlayerCreateForm()
        {
            TeamName = "";
        }

        [Key]
        public int PlayerId { get; set; }

        public int Number { get; set; }
        public string Name { get; set; }
        public int GamesPlayed { get; set; }
        public int Goals { get; set; }
        public int Assists { get; set; }
        public int Points { get; set; }

        [Display(Name = "Profile of Player")]
        [DataType(DataType.MultilineText)]
        public string Profile { get; set; }

        public string TeamName { get; set; }

        [Range(1, Int32.MaxValue)]
        public int TeamId { get; set; }

        [Required]
        [Display(Name = "Player Photo")]
        [DataType(DataType.Upload)]
        public string PhotoUpload { get; set; }

        public SelectList PositionList { get; set; }
    }

    public class PlayerCreate : PlayerCreateForm
    {
        public PlayerCreate()
        {
            
        }
        
        //to hold the data being sent to us via the PositionList
        public string Position { get; set; }

        [Required]
        public HttpPostedFileBase PhotoUpload { get; set; }
    }

    public class PlayerEditForm 
    {
        public PlayerEditForm()
        {
            Name = "";
            TeamName = "";
            Profile = "";
        }

        [Key]
        public int PlayerId { get; set; }
        public int Number { get; set; }
        public string Name { get; set; }
        public int GamesPlayed { get; set; }
        public int Goals { get; set; }
        public int Assists { get; set; }
        public int Points { get; set; }

        [Display(Name = "Profile of Player")]
        [DataType(DataType.MultilineText)]
        public string Profile { get; set; }
        public string TeamName { get; set; }

        [Required]
        [Display(Name = "Player Photo")]
        [DataType(DataType.Upload)]
        public string PhotoUpload { get; set; }

        public SelectList PositionList { get; set; }
    }

    public class PlayerEdit : PlayerEditForm
    {
        public PlayerEdit()
        {
            Position = "";
            Profile = "";
        }

        public string Position { get; set; }

        [Required]
        public HttpPostedFileBase PhotoUpload { get; set; }
    }

    public class PlayerPhoto
    {
        public int Id { get; set; }
        public string PhotoContentType { get; set; }
        public byte[] Photo { get; set; }
    }

    public class PlayerSearchForm
    {
        [Required, StringLength(200)]
        public string SearchText { get; set; }
    }
}