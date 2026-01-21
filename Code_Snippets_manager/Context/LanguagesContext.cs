using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Code_Snippets_manager.Services;

namespace Code_Snippets_manager.Context
{
    public class LanguagesContext
    {
        DatabassManager db = new DatabassManager();
        string table_name = "Languages";
        string table_column = "Language";

        public DataTable GetAllLanguages()
        {
            DataTable dt = db.Select(table_name);
            if (dt.Rows.Count < 1)
                return null;

            return dt;
        }

        public string AddLanguage(string language)
        {
            if (string.IsNullOrEmpty(language))
            {
                return null;
            }

            Dictionary<string, object> keyValuePairs = new Dictionary<string, object>();
            keyValuePairs.Add(table_column, language);
            db.Insert(table_name, keyValuePairs);
            return "ok";
        }

        public int EditLanguage(Int64 id, string newlanguage)
        {
            Dictionary<string, object> keyValuePairs = new Dictionary<string, object>();
            keyValuePairs.Add(table_column, newlanguage);
            var IsUpdeted = db.Update(table_name, keyValuePairs, "id = " + id);
            return IsUpdeted;
        }




        public Boolean DeleteLanguage(Int64 id)
        {
            int IsDeleted = db.Delete(table_name, "Id = " + id);

            if (IsDeleted == 0)
                return false;
            return true;
        }


    }
}
