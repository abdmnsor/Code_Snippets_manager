using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Code_Snippets_manager.Services;

namespace Code_Snippets_manager.Context
{

    class TagsContext
    {
        DatabassManager db = new DatabassManager();
        string table_name = "Tags";
        private enum table_column
        {
            Id, Tag
        }
        public DataTable GetAllTags()
        {
            DataTable dt = db.Select(table_name);
            if (dt.Rows.Count < 1)
                return null;

            return dt;
        }

        public string AddTags(string tag)
        {
            if (string.IsNullOrEmpty(tag))
            {
                return null;
            }

            Dictionary<string, object> keyValuePairs = new Dictionary<string, object>();
            keyValuePairs.Add(table_column.Tag.ToString(), tag);
            db.Insert(table_name, keyValuePairs);
            return "ok";
        }

        public int EditeTag(Int64 id, string newlanguage)
        {
            Dictionary<string, object> keyValuePairs = new Dictionary<string, object>();
            keyValuePairs.Add(table_column.Tag.ToString(), newlanguage);
            var IsUpdeted = db.Update(table_name, keyValuePairs, "id = " + id);
            return IsUpdeted;
        }




        public Boolean DeleteTag(Int64 id)
        {
            int IsDeleted = db.Delete(table_name, "Id = " + id);

            if (IsDeleted == 0)
                return false;
            return true;
        }
    }
}
