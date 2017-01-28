using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
// new...
using System.ComponentModel.DataAnnotations;

namespace Assignmnet9.Models
{
    // Add your design model classes below

    // Follow these rules or conventions:

    // To ease other coding tasks, the name of the 
    //   integer identifier property should be "Id"
    // Collection properties (including navigation properties) 
    //   must be of type ICollection<T>
    // Valid data annotations are pretty much limited to [Required] and [StringLength(n)]
    // Required to-one navigation properties must include the [Required] attribute
    // Do NOT configure scalar properties (e.g. int, double) with the [Required] attribute
    // Initialize DateTime and collection properties in a default constructor

    public class RoleClaim
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }
    }

    public class Team
    {
        public Team()
        {
            TeamName = "";
            Division = "";
            History = "";
            Players = new List<Player>();
            Coaches = new List<Coach>();
            MediaItems = new List<MediaItem>();
        }

        public int TeamID { get; set; }

        public string TeamName { get; set; }

        public string Division { get; set; }

        public int Wins { get; set; }

        public int Loss { get; set; }

        public int Points { get; set; }

        //String to hold rich text
        [StringLength(10000)]
        public string History { get; set; }

        //collection of players
        public ICollection<Player> Players { get; set; }

        //collection of MediaItems
        public ICollection<MediaItem> MediaItems { get; set; }

        public ICollection<Coach> Coaches { get; set; }
    }

    public class Coach
    {
        public Coach()
        {
            Name = "";
            TeamName = "";
            CoachProfile = "";
        }

        public int CoachId { get; set; }

        [Required]
        public string Name { get; set; }

        public string TeamName { get; set; }

        public decimal WinPercentage { get; set; }

        public int NumberOfGamesCoached { get; set; }

        public int YearsCoached { get; set; }

        [StringLength(10000)]
        public string CoachProfile { get; set; }

        [StringLength(1000)]
        public string PhotoContentType { get; set; }
        public byte[] Photo { get; set; }
    }


    public class Player
    {
        public Player()
        {
            Position = "";
            Name = "";
            Teams = new List<Team>();
            TeamName = "";
            Positions = new List<Position>();
            Profile = "";
        }

        public int PlayerId { get; set; }
        public int Number { get; set; }

        [Required]
        public string Position { get; set; }

        [Required, StringLength(200)]
        public string Name { get; set; }
        public int GamesPlayed { get; set; }
        public int Goals { get; set; }
        public int Assists { get; set; }
        public int Points { get; set; }

        public ICollection<Position> Positions { get; set; }

        [StringLength(10000)]
        public string Profile { get; set; }

        public string TeamName { get; set; }
        public ICollection<Team> Teams { get; set; }

        [StringLength(1000)]
        public string PhotoContentType { get; set; }
        public byte[] Photo { get; set; }
    }

    public class Position
    {
        public int PositionId { get; set; }

        [Required, StringLength(100)]
        public string PositionName { get; set; }
    }

    public class MediaItem
    {
        public MediaItem()
        {
            TimeStamp = DateTime.Now;

            //make stringId equate to something
            long i = 1;
            foreach (byte b in Guid.NewGuid().ToByteArray())
            {
                i *= ((int)b + 1);
            }

            StringId = string.Format("{0:x}", i - DateTime.Now.Ticks);
        }

        public string Caption { get; set; }

        public byte[] Content { get; set; }

        public string ContentType { get; set; }

        public int Id { get; set; }

        public string StringId { get; set; }

        public DateTime TimeStamp { get; set; }

        public Team Team { get; set; }
    }
}
