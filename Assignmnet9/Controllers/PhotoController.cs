using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Assignmnet9.Controllers
{
    public class PhotoController : Controller
    {
        // Reference to the manager object
        Manager m = new Manager();

        // GET: Photo
        public ActionResult Index()
        {
            return View("index", "home");
        }

        // GET: Photo/5
        // Attention - 8 - Uses attribute routing
        [Route("playerPhoto/{id}")]
        public ActionResult Details(int? id)
        {
            // Attempt to get the matching object
            var o = m.PlayerPhotoGetById(id.GetValueOrDefault());

            if (o == null)
            {
                return HttpNotFound();
            }
            else
            {
                if (o.PhotoContentType == null || o.Photo == null)
                {
                    return null;
                }
                else
                {

                    // Attention - 9 - Return a file content result
                    // Set the Content-Type header, and return the photo bytes
                    return File(o.Photo, o.PhotoContentType);
                }
            }
        }

        [Route("coachPhoto/{id}")]
        public ActionResult CoachDetails(int? id)
        {
            // Attempt to get the matching object
            var o = m.CoachPhotoGetById(id.GetValueOrDefault());

            if (o == null)
            {
                return HttpNotFound();
            }
            else
            {
                if (o.PhotoContentType == null || o.Photo == null)
                {
                    return null;
                }
                else
                {

                    // Attention - 9 - Return a file content result
                    // Set the Content-Type header, and return the photo bytes
                    return File(o.Photo, o.PhotoContentType);
                }
            }
        }


    }
}
