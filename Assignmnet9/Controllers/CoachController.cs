using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Assignmnet9.Controllers
{
    public class CoachController : Controller
    {
        Manager m = new Manager();

        // GET: Coach
        public ActionResult Index()
        {
            return View(m.CoachGetAll());
        }

        // GET: Coach/Details/5
        public ActionResult Details(int id)
        {
            return View(m.CoachGetOne(id));
        }

        // GET: Coach/Edit/5
        public ActionResult Edit(int? id)
        {
            var form = AutoMapper.Mapper.Map<CoachEditForm>(m.CoachGetOne(id));

            return View(form);
        }

        // POST: Coach/Edit/5
        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(int? id, CoachEdit newItem)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Where(x => x.Value.Errors.Any())
             .Select(x => new { x.Key, x.Value.Errors });

                return RedirectToAction("edit", new { id = newItem.CoachId });
            }

            if (id.GetValueOrDefault() != newItem.CoachId)
            {
                return RedirectToAction("Index");
            }

            var editItem = m.CoachEdit(newItem);

            if (editItem == null)
            {
                return RedirectToAction("edit", new { id = newItem.CoachId });
            }
            else
            {
                return RedirectToAction("details", new { id = newItem.CoachId });
            }
        }
        // GET: Coach/Delete/5
        public ActionResult Delete(int? id)
        {
            var itemToDelete = m.CoachGetOne(id.GetValueOrDefault());

            if (itemToDelete == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(itemToDelete);
            }
        }

        // POST: Coach/Delete/5
        [HttpPost]
        public ActionResult Delete(int? id, FormCollection collection)
        {
            var result = m.CoachDelete(id.GetValueOrDefault());

            return RedirectToAction("index");
        }
    }
}
