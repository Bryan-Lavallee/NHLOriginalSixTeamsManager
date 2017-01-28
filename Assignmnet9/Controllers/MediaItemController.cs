using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Win32;

namespace Assignmnet9.Controllers
{
    public class MediaItemController : Controller
    {
        Manager m = new Manager();

        // GET: MediaItem
        public ActionResult Index()
        {
            return View("index", "home");
        }

        [Route("mediaItem/{stringId}")]
        public ActionResult Details(string stringId = "")
        {
            //attempt to get object
            var obj = m.TeamMediaItemGetById(stringId);

            if (obj == null)
            {     
                return HttpNotFound();
            }
            else
            {
                //return file content
                return File(obj.Content, obj.ContentType);
            }
        }

        //Media Item Download
        [Route("mediaItem/{stringId}/download")]
        public ActionResult DetailsDownload(string stringId = "")
        {
            var obj = m.TeamMediaItemGetById(stringId);

            if (obj == null)
            {
                 return HttpNotFound();
            }
            else
            {
                //Get File Extension

                //make some working variables
                string extension;
                RegistryKey key;
                object value;

                //open registry
                key = Registry.ClassesRoot.OpenSubKey(@"MIME\Database\Content Type\" + obj.ContentType, false);
                //attempt to read the value of the key
                value = (key == null) ? null : key.GetValue("Extension", null);
                //build the file extension
                extension = (value == null) ? string.Empty : value.ToString();

                //Create Content-Disposition
                var cd = new System.Net.Mime.ContentDisposition
                {
                    //assemble the file name
                    FileName = $"img-{stringId}{extension}",
                    Inline = false
                };

                //Add header to response
                Response.AppendHeader("Content-Disposition", cd.ToString());

                return File(obj.Content, obj.ContentType);
            }
        }
    }
}
