using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Assignmnet9.Controllers
{
    public class PlayerController : Controller
    {
        Manager m = new Manager();

        // GET: Player
        public ActionResult Index()
        {
            return View(m.PlayerGetAll());
        }

        // GET: Player/Details/5
        public ActionResult Details(int? id)
        {
            return View(m.PlayerGetOne(id.GetValueOrDefault()));
        }

        // GET: Player/Edit/5
        [Authorize(Roles = "Coach")]
        public ActionResult Edit(int? id)
        {
            var form = AutoMapper.Mapper.Map<PlayerEditForm>(m.PlayerGetOne(id));

            form.PositionList = new SelectList(m.PositionGetAll(), "PositionName", "PositionName");

            return View(form);
        }


        // POST: Player/Edit/5
        [Authorize(Roles = "Coach")]
        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(int? id, PlayerEdit newItem)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("edit", new { id = newItem.PlayerId });
            }

            if (id.GetValueOrDefault() != newItem.PlayerId)
            {
                return RedirectToAction("Index");
            }

            var editItem = m.PlayerEdit(newItem);

            if (editItem == null)
            {
                return RedirectToAction("edit", new { id = newItem.PlayerId });
            }
            else
            {
                return RedirectToAction("details", new { id = newItem.PlayerId });
            }
        }

        [Authorize(Roles = "Coach")]
        public ActionResult Delete(int? id)
        {
            var itemToDelete = m.PlayerGetOne(id.GetValueOrDefault());

            if (itemToDelete == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(itemToDelete);
            }

        }

        [Authorize(Roles = "Coach")]
        // POST: Player/Delete/5
        [HttpPost]
        public ActionResult Delete(int? id, FormCollection collection)
        {
            var result = m.PlayerDelete(id.GetValueOrDefault());

            return RedirectToAction("index");
        }

        //Player Search by Name
        public ActionResult Search()
        {
            return View(new PlayerSearchForm());
        }

        [Route("player/players/{searchText}")]
        public ActionResult Players(string searchText = "")
        {
            var obj = m.PlayerGetAllByText(searchText);

            if (obj == null)
            {
                return PartialView("_PlayerList", new List<PlayerBase>());
            }
            else
            {
                return PartialView("_PlayerList", obj);
            }
        }

        [Route("team/playertrade/{teamId}")]
        public ActionResult PlayerTrade(int teamId)
        {
            var obj = m.PlayerGetAllByTeamId(teamId);

            if (obj == null)
            {
                return PartialView("_TeamList", new List<PlayerBase>());
            }
            else
            {
                return PartialView("_TeamList", obj);
            }
        }
    }
}
