using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Assignmnet9.Controllers
{
    public class TeamController : Controller
    {
        Manager m = new Manager();

        // GET: Team
        public ActionResult Index()
        {
            return View(m.TeamGetAll());
        }

        // GET: Team/Details/5
        public ActionResult Details(int? id)
        {
            return View(m.TeamGetOne(id.GetValueOrDefault()));
        }

        public ActionResult DetailsWithMediaItems(int? id)
        {
            var obj = m.TeamGetOneWithMediaInfo(id.GetValueOrDefault());

            if(obj == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(obj);
            }
        }

        // GET: Team/Create
        [Authorize(Roles = "General Manager")]
        public ActionResult Create()
        {
            var form = new TeamCreateForm();

            return View(form);
        }

        // POST: Team/Create
        [Authorize(Roles = "General Manager")]
        [HttpPost, ValidateInput(false)]
        public ActionResult Create(TeamCreate newTeam)
        {
            if(!ModelState.IsValid)
            {
                return View(newTeam);
            }

            var addTeam = m.TeamCreate(newTeam);

            if (addTeam == null)
            {
                return View(newTeam);
            }
            else
            {
                return RedirectToAction("DetailsWithMediaItems", new { id = addTeam.TeamId });
            }
        }

        [Route("teams/{id}/addMediaItem")]
        public ActionResult AddMediaItem(int? id)
        {
            var obj = m.TeamGetOne(id.GetValueOrDefault());

            if(obj == null)
            {
                return HttpNotFound();
            }
            else
            {
                var form = new MediaItemAddForm();
                form.TeamId = obj.TeamId;
                form.TeamInfo = $"{obj.TeamName}, Points: {obj.Points}";

                return View(form);
            }
        }

        [HttpPost]
        [Route("teams/{id}/addMediaItem")]
        public ActionResult AddMediaItem(int? id, MediaItemAdd newItem)
        {
            if (!ModelState.IsValid && id.GetValueOrDefault() == newItem.TeamId)
            {
                var errors = ModelState.Where(x => x.Value.Errors.Any())
                .Select(x => new { x.Key, x.Value.Errors });
                return View(newItem);
            }

            var addedMediaItem = m.TeamMediaItemAdd(newItem);

            if (addedMediaItem == null)
            {
                return View(newItem);
            }
            else
            {
                return RedirectToAction("DetailsWithMediaItems", new { id = addedMediaItem.TeamId });
            }
        }


        [Authorize(Roles = "Manager")]
        [Route("Team/{id}/AddPlayer")]
        public ActionResult AddPlayer(int? id)
        {
            var team = m.TeamGetOne(id.GetValueOrDefault());

            if(team == null)
            {
                return HttpNotFound();
            }
            else
            {
                var obj = new PlayerCreateForm();
                obj.TeamId = team.TeamId;
                obj.TeamName = team.TeamName;

                obj.PositionList = new SelectList(m.PositionGetAll(), "PositionName", "PositionName");
                return View(obj);
            }
        }

        [Authorize(Roles = "Manager")]
        [Route("Team/{id}/AddPlayer")]
        [HttpPost, ValidateInput(false)]
        public ActionResult AddPlayer(PlayerCreate newPlayer)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Where(x => x.Value.Errors.Any())
                  .Select(x => new { x.Key, x.Value.Errors });

                return View(newPlayer);
            }

            var addedPlayer = m.PlayerAdd(newPlayer);

            if (addedPlayer == null)
            {
                return View(newPlayer);
            }
            else
            {
                return RedirectToAction("Details", "Player", new { id = addedPlayer.PlayerId });
            }
        }


       [Authorize(Roles = "Manager")]
        [Route("Team/{id}/AddCoach")]
        public ActionResult AddCoach(int? id)
        {
            var team = m.TeamGetOne(id.GetValueOrDefault());

            if (team == null)
            {
                return HttpNotFound();
            }
            else
            {
                var obj = new CoachCreateForm();
                obj.TeamId = team.TeamId;
                obj.TeamName = team.TeamName;

                return View(obj);
            }
        }

        [Authorize(Roles = "Manager")]
        [Route("Team/{id}/AddCoach")]
        [HttpPost, ValidateInput(false)]
        public ActionResult AddCoach(CoachCreate newCoach)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Where(x => x.Value.Errors.Any())
                  .Select(x => new { x.Key, x.Value.Errors });

                return View(newCoach);
            }

            var addedCoach = m.CoachAdd(newCoach);

            if (addedCoach == null)
            {
                return View(newCoach);
            }
            else
            {
                return RedirectToAction("Details", "Coach", new { id = addedCoach.CoachId });
            }
        }

        //GET: Team/Edit/5
        [Authorize(Roles = "Coach")]
        public ActionResult Edit(int? id)
        {
            var o = m.TeamGetOne(id.GetValueOrDefault());

            if(o == null)
            {
                return null;
            }
            else
            {
                var form = new TeamEditForm();
                form.TeamId = o.TeamId;
                form.TeamName = o.TeamName;
                form.PlayersOnTeamList = o.Players.OrderBy(p => p.Points);

                form.PlayerList = m.PlayerWithPositionGetAll();
                
                foreach(var item in form.PlayersOnTeamList)
                {
                    form.PlayerList.SingleOrDefault(p => p.PlayerId == item.PlayerId).Selected = true; 
                }

                foreach(var item in form.PlayerList)
                {
                    if (item.Position.Equals("Centre"))
                        item.PositionId = 3;
                    else if (item.Position.Equals("Defenseman"))
                        item.PositionId = 4;
                    else if (item.Position.Equals("Goalie"))
                        item.PositionId = 5;
                    else if (item.Position.Equals("Left Wing"))
                        item.PositionId = 1;
                    else
                        item.PositionId = 2;
                }

                form.PositionList = m.PositionGetAll();

                
                return View(form);
            }
        }

        // POST: Team/Edit//
        [Authorize(Roles = "Coach")]
        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(int? id, TeamEdit newItem)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Where(x => x.Value.Errors.Any())
                .Select(x => new { x.Key, x.Value.Errors });
                return RedirectToAction("edit", new { id = newItem.TeamId });
            }

            if (id.GetValueOrDefault() != newItem.TeamId)
            {
                return RedirectToAction("index");
            }

            var editedItem = m.TeamEdit(newItem);

            if(editedItem == null)
            {
                return RedirectToAction("edit", new { id = newItem.TeamId });
            }
            else
            {
                return RedirectToAction("DetailsWithMediaItems", new { id = newItem.TeamId });
            }
        }

        public ActionResult TeamTrade()
        {
            var form = new TeamTradeForm();
            form.TeamList = new SelectList(m.TeamGetAll(), "TeamId", "TeamName");
            form.SecondTeamList = new SelectList(m.TeamGetAll(), "SecondTeamId", "TeamName");

            return View(form);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult TeamTrade(TeamTrade newItem)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Where(x => x.Value.Errors.Any())
                .Select(x => new { x.Key, x.Value.Errors });
                return RedirectToAction("TeamTrade");
            }

            var editedItem = m.TeamTrade(newItem);

            if(editedItem == null)
            {
                return RedirectToAction("TeamTrade");
            }else
            {
                return RedirectToAction("DetailsWithMediaItems", new { id = newItem.TeamId });
            }
        }

        // GET: Team/Delete/5
        [Authorize(Roles = "General Manager")]
        public ActionResult Delete(int? id)
        {
            var itemToDelete = m.TeamGetOne(id.GetValueOrDefault());


            if (itemToDelete == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(itemToDelete);
            }
        }

        // POST: Team/Delete/5
        [HttpPost]
        [Authorize(Roles = "General Manager")]
        public ActionResult Delete(int? id, FormCollection collection)
        {
            var result = m.TeamDelete(id.GetValueOrDefault());

            return RedirectToAction("index");
        }
    }
}
