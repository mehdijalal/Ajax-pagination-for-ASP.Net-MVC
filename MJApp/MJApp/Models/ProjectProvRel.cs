using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MJApp.Models
{
    public class ProjectProvRel
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public int ProvinceIds { get; set; }
    }
}