using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Code_Snippets_manager.Models
{
    public class Snippet  // ✅ Change from private to public
    {
        public Int64 id { get; set; }
        public string Title { get; set; }
        public string Tags { get; set; }
        public string Language { get; set; }
        public string Description { get; set; }
        public string SnippetCode { get; set; }
        public string UpdatedAt { get; set; }
        public string CreatedAt { get; set; }
    }

}
