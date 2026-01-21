using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Code_Snippets_manager.Models
{
    public class Language
    {
        public Int64 Id { get; set; }
        public string TheLanguage { get; set; }
        public string UpdatedAt { get; set; }
        public string CreatedAt { get; set; }
        public override string ToString()
        {
            return TheLanguage.ToUpper(); // ✅ This ensures the correct text appears
        }
    }

}
