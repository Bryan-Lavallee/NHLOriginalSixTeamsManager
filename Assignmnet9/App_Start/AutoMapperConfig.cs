using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
// new...
using AutoMapper;

namespace Assignmnet9
{
    public static class AutoMapperConfig
    {
        public static void RegisterMappings()
        {
            // Add map creation statements here
            // Mapper.CreateMap< FROM , TO >();

            // Disable AutoMapper v4.2.x warnings
#pragma warning disable CS0618

            // AutoMapper create map statements

            Mapper.CreateMap<Models.RegisterViewModel, Models.RegisterViewModelForm>();

            // Add more below...

            //Team
            Mapper.CreateMap<Controllers.TeamAdd, Models.Team >();
            Mapper.CreateMap<Models.Team, Controllers.TeamBase>();
            Mapper.CreateMap<Models.Team, Controllers.TeamWithDetail>();
            Mapper.CreateMap<Controllers.TeamBase, Controllers.TeamCreateForm>();
            Mapper.CreateMap<Models.Team, Controllers.TeamWithMediaInfo>();

            //Coach
            Mapper.CreateMap<Controllers.CoachAdd, Models.Coach>();
            Mapper.CreateMap<Models.Coach, Controllers.CoachBase>();
            Mapper.CreateMap<Models.Coach, Controllers.CoachWithDetail>();
            Mapper.CreateMap<Models.Coach, Controllers.CoachPhoto>();
            Mapper.CreateMap<Controllers.CoachWithDetail, Controllers.CoachEditForm>();



            //Player
            Mapper.CreateMap<Controllers.PlayerAdd, Models.Player>();
            Mapper.CreateMap<Controllers.PlayerCreate, Models.Player>();
            Mapper.CreateMap<Models.Player, Controllers.PlayerBase>();
            Mapper.CreateMap<Models.Player, Controllers.PlayerWithDetail>();
            Mapper.CreateMap<Controllers.PlayerBase, Controllers.PlayerCreateForm>();
            Mapper.CreateMap<Models.Player, Controllers.PlayerWithPosition>();
            Mapper.CreateMap<Controllers.PlayerWithDetail, Controllers.PlayerEditForm>();
            Mapper.CreateMap<Models.Player, Controllers.PlayerPhoto>();

            //Position
            Mapper.CreateMap<Models.Position, Controllers.PositionBase>();

            //MediaItem
            Mapper.CreateMap<Models.MediaItem, Controllers.MediaItemBase>();
            Mapper.CreateMap<Models.MediaItem, Controllers.MediaItemContent>();

            
#pragma warning restore CS0618
        }
    }
}