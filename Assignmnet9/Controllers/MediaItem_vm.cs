using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Assignmnet9.Controllers
{
    public class MediaItemBase
    {
        public MediaItemBase()
        {
            TimeStamp = DateTime.Now;
        }

        public string Caption { get; set; }

        public byte[] Content { get; set; }

        public string ContentType { get; set; }

        public int Id { get; set; }

        [Required]
        [Display(Name = "Unique identifier")]
        public string StringId { get; set; }

        public DateTime TimeStamp { get; set; }
    }

    public class MediaItemAddForm
    {
        public int TeamId { get; set; }
        public string TeamInfo { get; set; }
        public string Caption { get; set; }

        [Required]
        [DataType(DataType.Upload)]
        public string MediaItemUpload { get; set; }

    }

    public class MediaItemAdd
    {
        public int TeamId { get; set; }
        public string Caption { get; set; }

        [Required]
        public HttpPostedFileBase MediaItemUpload { get; set; }
    }

    public class MediaItemContent
    {
        public int Id { get; set; }
        public string ContentType { get; set; }
        public byte[] Content { get; set; }
    }
}