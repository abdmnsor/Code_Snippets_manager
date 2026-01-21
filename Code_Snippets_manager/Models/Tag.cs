using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Code_Snippets_manager.Models
{
    public class Tag
    {
        public Int64 Id { get; set; }
        public string Name { get; set; }

        public string UpdatedAt { get; set; }
        public string CreatedAt { get; set; }
    }
}
