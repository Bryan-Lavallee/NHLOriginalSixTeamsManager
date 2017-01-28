using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Assignmnet9.Controllers
{
    public class PositionBase
    {
        public PositionBase()
        {

        }

        public int PositionId { get; set; }
        public string PositionName { get; set; }
    }
}