using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
// new...
using AutoMapper;
using Assignmnet9.Models;
using System.Security.Claims;


//Excel using statements
using Excel;
using System.IO;
using System.Data;
using System.Reflection;

namespace Assignmnet9.Controllers
{
    public class Manager
    {
        // Reference to the data context
        private ApplicationDbContext ds = new ApplicationDbContext();

        // Declare a property to hold the user account for the current request
        // Can use this property here in the Manager class to control logic and flow
        // Can also use this property in a controller 
        // Can also use this property in a view; for best results, 
        // near the top of the view, add this statement:
        // var userAccount = new ConditionalMenu.Controllers.UserAccount(User as System.Security.Claims.ClaimsPrincipal);
        // Then, you can use "userAccount" anywhere in the view to render content
        public UserAccount UserAccount { get; private set; }

        public Manager()
        {
            // If necessary, add constructor code here

            // Initialize the UserAccount property
            UserAccount = new UserAccount(HttpContext.Current.User as ClaimsPrincipal);

            // Turn off the Entity Framework (EF) proxy creation features
            // We do NOT want the EF to track changes - we'll do that ourselves
            ds.Configuration.ProxyCreationEnabled = false;

            // Also, turn off lazy loading...
            // We want to retain control over fetching related objects
            ds.Configuration.LazyLoadingEnabled = false;
        }

        // ############################################################
        // RoleClaim

        public List<string> RoleClaimGetAllStrings()
        {
            return ds.RoleClaims.OrderBy(r => r.Name).Select(r => r.Name).ToList();
        }

        // ############################################################
        // Team

        public IEnumerable<TeamBase> TeamGetAll()
        {
            var obj = ds.Teams.OrderByDescending(t => t.TeamID);

            return (obj == null) ? null : Mapper.Map<IEnumerable<TeamBase>>(obj);
        }

 
        public TeamWithDetail TeamGetOne(int id)
        {
            var obj = ds.Teams.Include("Players").Include("Coaches").SingleOrDefault(t => t.TeamID == id);

            return (obj == null) ? null : Mapper.Map<TeamWithDetail>(obj);
        }

        public TeamWithMediaInfo TeamGetOneWithMediaInfo(int? id)
        {
            var obj = ds.Teams.Include("Players").Include("MediaItems").Include("Coaches").SingleOrDefault(t => t.TeamID == id);

            return (obj == null) ? null : Mapper.Map<TeamWithMediaInfo>(obj);
        }

        public TeamWithDetail TeamCreate(TeamCreate newItem)
        {
            var addedTeam = ds.Teams.Add(Mapper.Map<Team>(newItem));

            //added these two lines because it wasnt being added with above mapper for some reason
            addedTeam.History = newItem.History;

            ds.SaveChanges();

            return (addedTeam == null) ? null : Mapper.Map<TeamWithDetail>(addedTeam);
        }


        //Team edit
        public TeamWithDetail TeamEdit(TeamEdit newItem)
        {
            //get associated item
            var o = ds.Teams.Include("Players").Include("Coaches").SingleOrDefault(t => t.TeamID == newItem.TeamId);

            if (o == null)
            {
                return null;
            }
            else
            {
                //clear players
                o.Players.Clear();

                foreach (var item in newItem.PlayerIds)
                {
                    var add = ds.Players.Find(item);
                    add.TeamName = o.TeamName;
                    o.Players.Add(add);
                }

                ds.SaveChanges();

            }

            //now save changes made to team that doesnt deal with players
            var obj = ds.Teams.Find(newItem.TeamId);

            if (obj == null)
                return null;
            else
            {
                ds.Entry(obj).CurrentValues.SetValues(newItem);
                ds.SaveChanges();

                return Mapper.Map<TeamWithDetail>(o);
            }
        }


        public TeamWithDetail TeamTrade(TeamTrade newItem)
        {
            // find the two teams in the trade 
            var team1 = ds.Teams.Include("Players").SingleOrDefault(t => t.TeamID == newItem.TeamId);
            var team2 = ds.Teams.Include("Players").SingleOrDefault(t => t.TeamID == newItem.SecondTeamId);

            // make sure valid teams were selected 
            if (team1 == null || team2 == null)
            {
                return null;

            }

            else
            {
                foreach(var selectedPlayer in newItem.PlayerIds)
                {
                    var player = ds.Players.Find(selectedPlayer); 

                    // if valid player 
                    if(player != null)
                    {
                        // you can d othe same swap you're doing here

                        if(player.TeamName != team1.TeamName)
                        {
                            // player is moving to team 1 
                            team2.Players.Remove(player);
                            team1.Players.Add(player);
                            // update player as well ? 
                            player.TeamName = team1.TeamName;
                            player.Teams.Remove(team2); 
                        }
                        else
                        {
                            // player is moving to team 12
                            team1.Players.Remove(player);
                            team2.Players.Add(player);
                            // update player as well ? 
                            player.TeamName = team2.TeamName;
                            player.Teams.Remove(team1);
                        }
                    }
                    ds.SaveChanges();   
                }

                return Mapper.Map<TeamWithDetail>(team1); 
            }

        }


        public TeamBase TeamMediaItemAdd(MediaItemAdd newItem)
        {
            //get associated item
            var team = ds.Teams.Find(newItem.TeamId);

            if (team == null)
            {
                return null;
            }
            else
            {
                //attempt to add item
                var addedMediaItem = new MediaItem();
                ds.MediaItems.Add(addedMediaItem);

                //add data properties
                addedMediaItem.Caption = newItem.Caption;
                //add associated team
                addedMediaItem.Team = team;

                //upload the media item
                byte[] mediaItemBytes = new byte[newItem.MediaItemUpload.ContentLength];
                newItem.MediaItemUpload.InputStream.Read(mediaItemBytes, 0, newItem.MediaItemUpload.ContentLength);

                //configure the media item properties
                addedMediaItem.Content = mediaItemBytes;
                addedMediaItem.ContentType = newItem.MediaItemUpload.ContentType;

                ds.SaveChanges();

                return (addedMediaItem == null) ? null : Mapper.Map<TeamBase>(team);
            }
        }

        public MediaItemContent TeamMediaItemGetById(string stringId)
        {
            var obj = ds.MediaItems.SingleOrDefault(media => media.StringId == stringId);

            return (obj == null) ? null : Mapper.Map<MediaItemContent>(obj);
        }

        //Team Delete
        public bool TeamDelete(int id)
        {
            var itemToDelete = ds.Teams.Include("Players").SingleOrDefault(t => t.TeamID == id);

            if (itemToDelete == null)
                return false;
            else
            {
                //Remove track
                ds.Teams.Remove(itemToDelete);
                ds.SaveChanges();

                return true;
            }
        }

        // ############################################################
        // Player
        public IEnumerable<PlayerBase> PlayerGetAll()
        {
            var obj = ds.Players.OrderByDescending(t => t.Points);

            return (obj == null) ? null : Mapper.Map<IEnumerable<PlayerBase>>(obj);
        }

        public IEnumerable<PlayerWithPosition> PlayerWithPositionGetAll()
        {
            var obj = ds.Players.Include("Positions").OrderBy(p => p.Points);

            return Mapper.Map<IEnumerable<PlayerWithPosition>>(obj);
        }

        public PlayerWithDetail PlayerGetOne(int? id)
        {
            var obj = ds.Players.Find(id);
            return (obj == null) ? null : Mapper.Map<PlayerWithDetail>(obj);
        }

        public PlayerPhoto PlayerPhotoGetById(int id)
        {
            var o = ds.Players.Find(id);

            return (o == null) ? null : Mapper.Map<PlayerPhoto>(o);
        }

        public PlayerWithDetail PlayerAdd(PlayerCreate newItem)
        {

            //fetch associated team first
            var team = ds.Teams.Find(newItem.TeamId);

            if(team == null)
            {
                return null;
            }

            var addedPlayer = ds.Players.Add(Mapper.Map<Player>(newItem));

            byte[] photobytes = new byte[newItem.PhotoUpload.ContentLength];
            newItem.PhotoUpload.InputStream.Read(photobytes, 0, newItem.PhotoUpload.ContentLength);

            addedPlayer.Photo = photobytes;
            addedPlayer.PhotoContentType = newItem.PhotoUpload.ContentType;

            addedPlayer.Teams.Add(team);

            ds.SaveChanges();

            return (addedPlayer == null) ? null : Mapper.Map<PlayerWithDetail>(addedPlayer);
        }

        public PlayerWithDetail PlayerEdit(PlayerEdit newItem)
        {
            var obj = ds.Players.Find(newItem.PlayerId);

            if (obj == null)
                return null;
            else
            {
                ds.Entry(obj).CurrentValues.SetValues(newItem);

                byte[] photobytes = new byte[newItem.PhotoUpload.ContentLength];
                newItem.PhotoUpload.InputStream.Read(photobytes, 0, newItem.PhotoUpload.ContentLength);

                obj.Photo = photobytes;
                obj.PhotoContentType = newItem.PhotoUpload.ContentType;

                ds.SaveChanges();

                return Mapper.Map<PlayerWithDetail>(obj);
            }
        }

        public IEnumerable<PlayerBase> PlayerGetAllByText(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return null;
            }
            else
            {
                var obj = ds.Players.Where(p => p.Name.ToLower().Contains(text.Trim().ToLower()));

                if (obj.Count() == 0)
                {
                    //check to see if it was position that was used
                    obj = ds.Players.Where(p => p.Position.ToLower().Contains(text.Trim().ToLower()));

                    if(obj.Count() == 0)
                    {
                        //check lastly for team name
                        obj = ds.Players.Where(p => p.TeamName.ToLower().Contains(text.Trim().ToLower()));
                    }
                }

                return Mapper.Map<IEnumerable<PlayerBase>>(obj.OrderBy(p => p.Name));
            }
        }

        public IEnumerable<PlayerBase> PlayerGetAllByTeamId(int id)
        {
            var team = ds.Teams.Find(id); 

            var obj = ds.Players.Where(p => p.TeamName == team.TeamName);

            return Mapper.Map<IEnumerable<PlayerBase>>(obj.OrderBy(p => p.Name));
        }

        //Player Delete
        public bool PlayerDelete(int id)
        {
            var itemToDelete = ds.Players.Find(id);

            if (itemToDelete == null)
                return false;
            else
            {
                //Remove track
                ds.Players.Remove(itemToDelete);
                ds.SaveChanges();

                return true;
            }
        }

        // ############################################################
        // Position

        //Position Get all
        public IEnumerable<PositionBase> PositionGetAll()
        {
            var obj = ds.Positions.OrderBy(p => p.PositionName);

            return (obj == null) ? null : Mapper.Map<IEnumerable<PositionBase>>(obj);
        }

        // ############################################################
        // Coach
        public IEnumerable<CoachBase> CoachGetAll()
        {
            var obj = ds.Coaches.OrderBy(c => c.Name);

            return (obj == null) ? null : Mapper.Map<IEnumerable<CoachBase>>(obj);
        }

        public CoachWithDetail CoachGetOne(int? id)
        {
            var obj = ds.Coaches.Find(id);
            return (obj == null) ? null : Mapper.Map<CoachWithDetail>(obj);
        }

        public CoachWithDetail CoachAdd(CoachCreate newItem)
        {

            //fetch associated team first
            var team = ds.Teams.Find(newItem.TeamId);

            if (team == null)
            {
                return null;
            }

            var addedCoach = ds.Coaches.Add(Mapper.Map<Coach>(newItem));

            byte[] photobytes = new byte[newItem.PhotoUpload.ContentLength];
            newItem.PhotoUpload.InputStream.Read(photobytes, 0, newItem.PhotoUpload.ContentLength);

            addedCoach.Photo = photobytes;
            addedCoach.PhotoContentType = newItem.PhotoUpload.ContentType;

            addedCoach.CoachProfile = newItem.CoachProfile;

            team.Coaches.Add(addedCoach);

            ds.SaveChanges();

            return (addedCoach == null) ? null : Mapper.Map<CoachWithDetail>(addedCoach);
        }

        public CoachPhoto CoachPhotoGetById(int id)
        {
            var o = ds.Coaches.Find(id);

            return (o == null) ? null : Mapper.Map<CoachPhoto>(o);
        }

        //Coach Edit
        public CoachWithDetail CoachEdit(CoachEdit newItem)
        {
            var obj = ds.Coaches.Find(newItem.CoachId);

            if (obj == null)
                return null;
            else
            {
                ds.Entry(obj).CurrentValues.SetValues(newItem);

                byte[] photobytes = new byte[newItem.PhotoUpload.ContentLength];
                newItem.PhotoUpload.InputStream.Read(photobytes, 0, newItem.PhotoUpload.ContentLength);

                obj.Photo = photobytes;
                obj.PhotoContentType = newItem.PhotoUpload.ContentType;

                obj.CoachProfile = newItem.CoachProfile;

                ds.SaveChanges();

                return Mapper.Map<CoachWithDetail>(obj);
            }
        }

        //Coach Delete
        public bool CoachDelete(int id)
        {
            var itemToDelete = ds.Coaches.Find(id);

            if (itemToDelete == null)
                return false;
            else
            {
                //Remove track
                ds.Coaches.Remove(itemToDelete);
                ds.SaveChanges();

                return true;
            }
        }

        // Add some programmatically-generated objects to the data store
        // Can write one method, or many methods - your decision
        // The important idea is that you check for existing data first
        // Call this method from a controller action/method

        public bool LoadData()
        {
            // User name
            var user = HttpContext.Current.User.Identity.Name;

            // Monitor the progress
            bool done = false;

            // ############################################################
            // RoleClaims

            if (ds.RoleClaims.Count() == 0)
            {
                //Add Roles

                ds.RoleClaims.Add(new RoleClaim { Name = "Manager" });
                ds.RoleClaims.Add(new RoleClaim { Name = "General Manager" });
                ds.RoleClaims.Add(new RoleClaim { Name = "Coach" });
                ds.RoleClaims.Add(new RoleClaim { Name = "Player" });
                ds.RoleClaims.Add(new RoleClaim { Name = "Referee" });
                ds.RoleClaims.Add(new RoleClaim { Name = "League Rep" });

                ds.SaveChanges();
                done = true;
            }

            if(ds.Positions.Count() == 0)
            {
                ds.Positions.Add(new Position { PositionName = "Left Wing" });
                ds.Positions.Add(new Position { PositionName = "Right Wing" });
                ds.Positions.Add(new Position { PositionName = "Centre" });
                ds.Positions.Add(new Position { PositionName = "Defenseman" });
                ds.Positions.Add(new Position { PositionName = "Goalie" });
            }

            if (ds.Teams.Count() == 0)
            {
                //Add teams

                //Path to the XLSX file
                var path = HttpContext.Current.Server.MapPath("~/App_Data/TeamData.xlsx");

                //Load workbook into a System.Data.Dataset "sourceData"
                var stream = File.Open(path, FileMode.Open, FileAccess.Read);
                IExcelDataReader reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                reader.IsFirstRowAsColumnNames = true;
                DataSet sourceData = reader.AsDataSet();
                reader.Close();

                //Worksheet name
                string worksheetName;

                //Get workseet by its name
                worksheetName = "Teams";
                var worksheet = sourceData.Tables[worksheetName];

                //Conver it to a collection of team add
                List<TeamAdd> items = worksheet.DataTableToList<TeamAdd>();

                //go thorugh collection and add items
                foreach (var item in items)
                {
                    // Double fixDate = double.Parse(item.StartDate);
                    ds.Teams.Add(Mapper.Map<Team>(item));
                }

                //save changes
                ds.SaveChanges();

                //if got this far make done = true
                done = true;
            }

            if(ds.Coaches.Count() == 0)
            {
                //Add teams

                //Path to the XLSX file
                var path = HttpContext.Current.Server.MapPath("~/App_Data/CoachData.xlsx");

                //Load workbook into a System.Data.Dataset "sourceData"
                var stream = File.Open(path, FileMode.Open, FileAccess.Read);
                IExcelDataReader reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                reader.IsFirstRowAsColumnNames = true;
                DataSet sourceData = reader.AsDataSet();
                reader.Close();

                //Worksheet name
                string worksheetName;

                //Get workseet by its name
                worksheetName = "Coach";
                var worksheet = sourceData.Tables[worksheetName];

                //Conver it to a collection of team add
                List<CoachAdd> items = worksheet.DataTableToList<CoachAdd>();

                //go thorugh collection and add items
                foreach (var item in items)
                {
                    var addedCoach = ds.Coaches.Add(Mapper.Map<Coach>(item));
                    var addToTeam = ds.Teams.SingleOrDefault(t => t.TeamName == item.TeamName);
                    addToTeam.Coaches.Add(addedCoach);

                    //save changes in loop to ensure Coaches collection is filled properly
                    ds.SaveChanges();
                }

           

                //if got this far make done = true
                done = true;
            }

            if (ds.Players.Count() == 0)
            {
                //Add Players

                //Path to the XLSX file
                var path = HttpContext.Current.Server.MapPath("~/App_Data/PlayerData.xlsx");

                //Load workbook into a System.Data.Dataset "sourceData"
                var stream = File.Open(path, FileMode.Open, FileAccess.Read);
                IExcelDataReader reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                reader.IsFirstRowAsColumnNames = true;
                DataSet sourceData = reader.AsDataSet();
                reader.Close();

                //Worksheet name
                string worksheetName;

                //Get workseet by its name
                worksheetName = "CAN";
                var worksheet = sourceData.Tables[worksheetName];

                //Conver it to a collection of team add
                List<PlayerAdd> items = worksheet.DataTableToList<PlayerAdd>();

                var montreal = ds.Teams.SingleOrDefault(t => t.TeamName == "Montreal Canadiens");

                //go thorugh collection and add items
                foreach (var item in items)
                {
                    var addedPlayer = ds.Players.Add(Mapper.Map<Player>(item));
                    montreal.Players.Add(addedPlayer);
                    addedPlayer.TeamName = montreal.TeamName;
                }

                //save changes
                ds.SaveChanges();

                // Load the next worksheet...
                // ==========================

                //Get workseet by its name
                worksheetName = "TOR";
                worksheet = sourceData.Tables[worksheetName];

                var toronto = ds.Teams.SingleOrDefault(t => t.TeamName == "Toronto Maple Leafs");

                //Conver it to a collection of team add
                items = worksheet.DataTableToList<PlayerAdd>();

                //go thorugh collection and add items
                foreach (var item in items)
                {
                    var addedPlayer = ds.Players.Add(Mapper.Map<Player>(item));
                    toronto.Players.Add(addedPlayer);
                    addedPlayer.TeamName = toronto.TeamName;
                }

                //save changes
                ds.SaveChanges();

                // Load the next worksheet...
                // ==========================

                //Get workseet by its name
                worksheetName = "BOS";
                worksheet = sourceData.Tables[worksheetName];

                var boston = ds.Teams.SingleOrDefault(t => t.TeamName == "Boston Bruins");

                //Conver it to a collection of team add
                items = worksheet.DataTableToList<PlayerAdd>();

                //go thorugh collection and add items
                foreach (var item in items)
                {
                    var addedPlayer = ds.Players.Add(Mapper.Map<Player>(item));
                    boston.Players.Add(addedPlayer);
                    addedPlayer.TeamName = boston.TeamName;
                }

                //save changes
                ds.SaveChanges();

                // Load the next worksheet...
                // ==========================

                //Get workseet by its name
                worksheetName = "NYR";
                worksheet = sourceData.Tables[worksheetName];

                var newyork = ds.Teams.SingleOrDefault(t => t.TeamName == "New York Rangers");

                //Conver it to a collection of team add
                items = worksheet.DataTableToList<PlayerAdd>();

                //go thorugh collection and add items
                foreach (var item in items)
                {
                    var addedPlayer = ds.Players.Add(Mapper.Map<Player>(item));
                    newyork.Players.Add(addedPlayer);
                    addedPlayer.TeamName = newyork.TeamName;
                }

                //save changes
                ds.SaveChanges();

                // Load the next worksheet...
                // ==========================

                //Get workseet by its name
                worksheetName = "RED";
                worksheet = sourceData.Tables[worksheetName];

                var red = ds.Teams.SingleOrDefault(t => t.TeamName == "Detroit Red Wings");


                //Conver it to a collection of team add
                items = worksheet.DataTableToList<PlayerAdd>();

                //go thorugh collection and add items
                foreach (var item in items)
                {
                    var addedPlayer = ds.Players.Add(Mapper.Map<Player>(item));
                    red.Players.Add(addedPlayer);
                    addedPlayer.TeamName = red.TeamName;
                }

                //save changes
                ds.SaveChanges();

                // Load the next worksheet...
                // ==========================

                //Get workseet by its name
                worksheetName = "CHG";
                worksheet = sourceData.Tables[worksheetName];

                var chicago = ds.Teams.SingleOrDefault(t => t.TeamName == "Chicago Black Hawks");

                //Conver it to a collection of team add
                items = worksheet.DataTableToList<PlayerAdd>();

                //go thorugh collection and add items
                foreach (var item in items)
                {
                    var addedPlayer = ds.Players.Add(Mapper.Map<Player>(item));
                    chicago.Players.Add(addedPlayer);
                    addedPlayer.TeamName = chicago.TeamName;
                }

                //save changes
                ds.SaveChanges();

                //if got this far make done = true
                done = true;
            }

            return done;
        }

        public bool RemoveData()
        {
            try
            {
                foreach (var e in ds.RoleClaims)
                {
                    ds.Entry(e).State = System.Data.Entity.EntityState.Deleted;
                }
                ds.SaveChanges();

                foreach (var e in ds.Teams)
                {
                    ds.Entry(e).State = System.Data.Entity.EntityState.Deleted;
                }
                ds.SaveChanges();

                foreach (var e in ds.Players)
                {
                    ds.Entry(e).State = System.Data.Entity.EntityState.Deleted;
                }

                ds.SaveChanges();

                foreach (var e in ds.MediaItems)
                {
                    ds.Entry(e).State = System.Data.Entity.EntityState.Deleted;
                }

                ds.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool RemoveDatabase()
        {
            try
            {
                return ds.Database.Delete();
            }
            catch (Exception)
            {
                return false;
            }
        }

    }

    // New "UserAccount" class for the authenticated user
    // Includes many convenient members to make it easier to render user account info
    // Study the properties and methods, and think about how you could use it
    public class UserAccount
    {
        // Constructor, pass in the security principal
        public UserAccount(ClaimsPrincipal user)
        {
            if (HttpContext.Current.Request.IsAuthenticated)
            {
                Principal = user;

                // Extract the role claims
                RoleClaims = user.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);

                // User name
                Name = user.Identity.Name;

                // Extract the given name(s); if null or empty, then set an initial value
                string gn = user.Claims.SingleOrDefault(c => c.Type == ClaimTypes.GivenName).Value;
                if (string.IsNullOrEmpty(gn)) { gn = "(empty given name)"; }
                GivenName = gn;

                // Extract the surname; if null or empty, then set an initial value
                string sn = user.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Surname).Value;
                if (string.IsNullOrEmpty(sn)) { sn = "(empty surname)"; }
                Surname = sn;

                IsAuthenticated = true;
                IsAdmin = user.HasClaim(ClaimTypes.Role, "Admin") ? true : false;
            }
            else
            {
                RoleClaims = new List<string>();
                Name = "anonymous";
                GivenName = "Unauthenticated";
                Surname = "Anonymous";
                IsAuthenticated = false;
                IsAdmin = false;
            }

            NamesFirstLast = $"{GivenName} {Surname}";
            NamesLastFirst = $"{Surname}, {GivenName}";
        }

        // Public properties
        public ClaimsPrincipal Principal { get; private set; }
        public IEnumerable<string> RoleClaims { get; private set; }

        public string Name { get; set; }

        public string GivenName { get; private set; }
        public string Surname { get; private set; }

        public string NamesFirstLast { get; private set; }
        public string NamesLastFirst { get; private set; }

        public bool IsAuthenticated { get; private set; }

        // Add other role-checking properties here as needed
        public bool IsAdmin { get; private set; }

        public bool HasRoleClaim(string value)
        {
            if (!IsAuthenticated) { return false; }
            return Principal.HasClaim(ClaimTypes.Role, value) ? true : false;
        }

        public bool HasClaim(string type, string value)
        {
            if (!IsAuthenticated) { return false; }
            return Principal.HasClaim(type, value) ? true : false;
        }
    }

    public static class Helper
    {
        public static List<T> DataTableToList<T>(this DataTable table) where T : class, new()
        {
            try
            {
                List<T> list = new List<T>();

                foreach (var row in table.AsEnumerable())
                {
                    T obj = new T();

                    foreach (var prop in obj.GetType().GetProperties())
                    {
                        try
                        {
                            PropertyInfo propertyInfo = obj.GetType().GetProperty(prop.Name);
                            propertyInfo.SetValue(obj, Convert.ChangeType(row[prop.Name], propertyInfo.PropertyType), null);
                        }
                        catch
                        {
                            continue;
                        }
                    }

                    list.Add(obj);
                }

                return list;
            }
            catch
            {
                return null;
            }
        }

    } // public static class Helper
}