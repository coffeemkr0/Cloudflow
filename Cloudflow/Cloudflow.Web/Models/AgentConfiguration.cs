using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Cloudflow.Web.Models
{
    public class AgentConfiguration
    {
        public int Id { get; set; }

        public bool Enabled { get; set; }

        public string MachineName { get; set; }

        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
    }
}