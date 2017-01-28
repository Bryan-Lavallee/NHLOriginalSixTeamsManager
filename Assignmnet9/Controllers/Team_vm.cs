using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Assignmnet9.Controllers
{
   public class TeamAdd
    {
        public TeamAdd()
        {
            TeamName = "";
            Division = "";
        }

        [Key]
        public int TeamId { get; set; }
        [Display(Name = "Team Name")]
        public string TeamName { get; set; }
        public string Division { get; set; }
        public int Wins { get; set; }
        public int Loss { get; set; }
        public int Points { get; set; }
    }

    public class TeamBase : TeamAdd
    {

    }

    public class TeamWithDetail : TeamBase
    {
        public TeamWithDetail()
        {
            History = "";
            Players = new List<PlayerBase>();
            Coaches = new List<CoachBase>();
        }

        [Display(Name = "History of Team")]
        [DataType(DataType.MultilineText)]
        public string History { get; set; }

        public ICollection<PlayerBase> Players { get; set; }

        public ICollection<CoachBase> Coaches { get; set; }
    }

    public class TeamCreateForm : TeamBase
    {
        public TeamCreateForm()
        {
            History = "";
        }

        [Display(Name = "History of Team")]
        [DataType(DataType.MultilineText)]
        public string History { get; set; }
    }

    public class TeamCreate : TeamAdd
    {
        public TeamCreate()
        {
            History = "";
        }

        [Display(Name = "History of Team")]
        [DataType(DataType.MultilineText)]
        public string History { get; set; }
    }

    public class TeamTradeForm
    {
        public TeamTradeForm()
        { 
        }


        [Key]
        public int TeamId { get; set; }


        [Key]
        public int SecondTeamId { get; set; }

        public int TeamName { get; set; }

        public int SecondTeamName { get; set; }

        public SelectList TeamList { get; set; }

        public SelectList SecondTeamList { get; set; }
    }


    public class TeamTrade
    {
        public TeamTrade()
        {
            PlayerIds = new List<int>();
        }


        [Key]
        public int TeamId { get; set; }


        [Key]
        public int SecondTeamId { get; set; }

        public int TeamName { get; set; }

        
        public int SecondTeamName { get; set; }

        public IEnumerable<int> PlayerIds { get; set; }
    }

    public class TeamEditForm
    {
        public TeamEditForm()
        {
            PlayersOnTeamList = new List<PlayerBase>();
            History = "";
        }

        [Key]
        public int TeamId { get; set; }

        [Display(Name = "Team Name")]
        public string TeamName { get; set; }
        public string Division { get; set; }
        public int Wins { get; set; }
        public int Loss { get; set; }
        public int Points { get; set; }

        [Display(Name = "History of Team")]
        [DataType(DataType.MultilineText)]
        public string History { get; set; }

        //list of all players
        [Display(Name ="List of All Players")]
        public IEnumerable<PlayerWithPosition> PlayerList { get; set; }

        public IEnumerable<PositionBase> PositionList { get; set; }

        //list of players currently on team
        [Display(Name = "Players currently on team")]
        public IEnumerable<PlayerBase> PlayersOnTeamList { get; set; }
    }

 
    public class TeamEdit
    {
        public TeamEdit()
        {
            PlayerIds = new List<int>();
            History = "";
        }

        [Key]
        public int TeamId { get; set; }
        public IEnumerable<int> PlayerIds { get; set; }

        [Display(Name = "Team Name")]
        public string TeamName { get; set; }
        public string Division { get; set; }
        public int Wins { get; set; }
        public int Loss { get; set; }
        public int Points { get; set; }

        [Display(Name = "History of Team")]
        [DataType(DataType.MultilineText)]
        public string History { get; set; }
    }

    public class TeamWithMediaInfo : TeamWithDetail
    {
        public TeamWithMediaInfo()
        {
            MediaItems = new List<MediaItemBase>();
        }

        public IEnumerable<MediaItemBase> MediaItems { get; set; }
    }
}