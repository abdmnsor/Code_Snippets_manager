using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Code_Snippets_manager.Models;
using Code_Snippets_manager.Services;

namespace Code_Snippets_manager.Context
{

    class SnippetsContext
    {
        private enum table_column
        {
            Id, Tags, Language, Title, Description, Snippet, CreatedAt, UpdatedAt
        }
        DatabassManager db = new DatabassManager();
        string table_name = "Snippets";

        public DataTable GetAllSnippet()
        {
            DataTable dt = db.Select(table_name);
            if (dt.Rows.Count < 1)
                return null;

            return dt;
        }

        public string AddSnippet(Snippet _snippet)
        {
            if (_snippet == null)
                return null;
            

            Dictionary<string, object> keyValuePairs = new Dictionary<string, object>();
            keyValuePairs.Add(table_column.Language.ToString(), _snippet.Language);
            keyValuePairs.Add(table_column.Snippet.ToString(), _snippet.SnippetCode);
            keyValuePairs.Add(table_column.Tags.ToString(), _snippet.Tags);
            keyValuePairs.Add(table_column.Title.ToString(), _snippet.Title);
            keyValuePairs.Add(table_column.Description.ToString(), _snippet.Description);

            db.Insert(table_name, keyValuePairs);
            return "ok";
        }

        public string UpdateSnippet(Snippet _snippet)
        {
            if (_snippet == null)
                return null;


            Dictionary<string, object> keyValuePairs = new Dictionary<string, object>();
            keyValuePairs.Add(table_column.Language.ToString(), _snippet.Language);
            keyValuePairs.Add(table_column.Snippet.ToString(), _snippet.SnippetCode);
            keyValuePairs.Add(table_column.Tags.ToString(), _snippet.Tags);
            keyValuePairs.Add(table_column.Title.ToString(), _snippet.Title);
            keyValuePairs.Add(table_column.Description.ToString(), _snippet.Description);

            db.Update(table_name, keyValuePairs, "id = " + _snippet.id);
            return "ok";
        }


        public string DeleteSnippet(Int64 id)
        {
            db.Delete(table_name, "id = " + id);
            return "ok";
        }
        //public int EditLanguage(Int64 id, string newlanguage)
        //{
        //    Dictionary<string, object> keyValuePairs = new Dictionary<string, object>();
        //    keyValuePairs.Add(table_column, newlanguage);
        //    var IsUpdeted = db.Update(table_name, keyValuePairs, "id = " + id);
        //    return IsUpdeted;
        //}




        //public Boolean DeleteLanguage(Int64 id)
        //{
        //    int IsDeleted = db.Delete(table_name, "Id = " + id);

        //    if (IsDeleted == 0)
        //        return false;
        //    return true;
        //}

    }
}
