using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;

namespace Code_Snippets_manager.Services
{
    public class DatabassManager
    {


        private readonly string _dbPath;
        private readonly string _connectionString;

        public DatabassManager(string dbPath = "")
        {
            _dbPath = string.IsNullOrEmpty(dbPath) ? Environment.CurrentDirectory + "\\Snippets.db" : dbPath;
            _connectionString = $"Data Source={_dbPath};Version=3;";

            // Check if database exists, if not create it
            if (!File.Exists(_dbPath))
            {
                CreateDatabase();
            }
        }

        private void CreateDatabase()
        {
            SQLiteConnection.CreateFile(_dbPath);

            // Create default tables
            using var connection = new SQLiteConnection(_connectionString);
            connection.Open();

            // Example: Creating a Snippets table
            var createSnippetsTable = @"
            CREATE TABLE Snippets (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Language TEXT NOT NULL,
                Title TEXT NOT NULL,
                Tags TEXT NOT NULL,
                Description TEXT NOT NULL,
                Snippet TEXT NOT NULL,
                UpdatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
                CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP
            );";

            // Example: Creating a Languages table
            var createLanguagesTable = @"
            CREATE TABLE Languages (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Language TEXT NOT NULL,
                UpdatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
                CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP
            );";

            // Example: Creating a Tags table
            var createTagsTable = @"
            CREATE TABLE Tags (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Tag TEXT NOT NULL,
                UpdatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
                CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP
            );";


            using (var command = new SQLiteCommand(connection))
            {
                command.CommandText = createSnippetsTable;
                command.ExecuteNonQuery();

                command.CommandText = createLanguagesTable;
                command.ExecuteNonQuery();

                command.CommandText = createTagsTable;
                command.ExecuteNonQuery();

            }

            var sql = @"
                INSERT INTO Tags (Tag) VALUES
                ('Database'),
                ('GUI'),
                ('Automation'),
                ('API'),
                ('Networking'),
                ('File&Folders')";

            using (var command = new SQLiteCommand(connection))
            {
                command.CommandText = sql;
                command.ExecuteNonQuery();
            }

            sql = @"
                INSERT INTO Languages (Language) VALUES
                ('All Languages'),
                ('C#'),
                ('JavaScript'),
                ('VB.NET'),
                ('VBA'),
                ('CMD'),
                ('AutoHotKey'),
                ('SQL')
                ";

            using (var command = new SQLiteCommand(connection))
            {
                command.CommandText = sql;
                command.ExecuteNonQuery();
            }

            //string insertQuery = @"
            //        INSERT INTO Snippets (Language, Title, Tags, Description, Snippet) VALUES
            //        ('C#', 'Basic Class Example', 'Object-Oriented, .NET, Windows, Web', 'A simple example of a class in C#.', 'public class MyClass { }'),
            //        ('Python', 'Read a File', 'Scripting, AI, Web, Automation', 'How to read a file in Python.', 'with open(""file.txt"", ""r"") as f: print(f.read())'),
            //        ('JavaScript', 'Async Function', 'Web, Frontend, Backend, Node.js', 'Using async/await in JavaScript.', 'async function fetchData() { let data = await fetch(url); }'),
            //        ('Java', 'Hello World', 'Object-Oriented, Mobile, Enterprise', 'A simple Hello World program in Java.', 'public class Main { public static void main(String[] args) { System.out.println(""Hello World""); } }'),
            //        ('VB.NET', 'Message Box Example', 'Windows, .NET, GUI, Enterprise', 'Show a message box in VB.NET.', 'MessageBox.Show(""Hello, World!"")'),
            //        ('SQL', 'Select All Records', 'Database, Queries, Backend, Data', 'Query to select all records from a table.', 'SELECT * FROM users;'),
            //        ('PHP', 'Connect to MySQL', 'Web, Backend, CMS, Scripting', 'PHP code to connect to a MySQL database.', '<?php $conn = new mysqli(""localhost"", ""user"", ""pass"", ""db""); ?>'),
            //        ('C++', 'For Loop Example', 'Game Development, High Performance, Embedded', 'A basic for loop in C++.', 'for(int i = 0; i < 10; i++) { cout << i; }'),
            //        ('Swift', 'Define a Struct', 'Mobile, iOS, Apple', 'How to define a struct in Swift.', 'struct Person { var name: String }'),
            //        ('Go', 'Start an HTTP Server', 'Cloud, Networking, High Performance', 'Create a simple HTTP server in Go.', 'http.ListenAndServe("":8080"", nil)');
            //        ";
            //using (var command = new SQLiteCommand(connection))
            //{
            //    command.CommandText = insertQuery;
            //    command.ExecuteNonQuery();
            //}

        }

        // Create operation
        public long Insert(string tableName, Dictionary<string, object> data)
        {
            using var connection = new SQLiteConnection(_connectionString);
            connection.Open();

            var columns = string.Join(", ", data.Keys);
            var values = string.Join(", ", data.Keys.Select(k => "@" + k));
            var query = $"INSERT INTO {tableName} ({columns}) VALUES ({values}); SELECT last_insert_rowid();";

            using var command = new SQLiteCommand(query, connection);
            foreach (var item in data)
            {
                command.Parameters.AddWithValue("@" + item.Key, item.Value ?? DBNull.Value);
            }

            return (long)command.ExecuteScalar();
        }

        // Read operation
        public DataTable Select(string tableName, string whereClause = "", object[] parameters = null)
        {
            using var connection = new SQLiteConnection(_connectionString);
            connection.Open();

            var query = $"SELECT * FROM {tableName}";
            if (!string.IsNullOrWhiteSpace(whereClause))
            {
                query += " WHERE " + whereClause;
            }

            using var command = new SQLiteCommand(query, connection);
            if (parameters != null)
            {
                for (int i = 0; i < parameters.Length; i++)
                {
                    command.Parameters.AddWithValue($"@p{i}", parameters[i] ?? DBNull.Value);
                }
            }

            var adapter = new SQLiteDataAdapter(command);
            var table = new DataTable();
            adapter.Fill(table);

            return table;
        }

        // Update operation
        public int Update(string tableName, Dictionary<string, object> data, string whereClause, object[] parameters = null)
        {
            using var connection = new SQLiteConnection(_connectionString);
            connection.Open();

            var setClause = string.Join(", ", data.Keys.Select(k => $"{k} = @{k}"));
            var query = $"UPDATE {tableName} SET {setClause}";

            if (!string.IsNullOrWhiteSpace(whereClause))
            {
                query += " WHERE " + whereClause;
            }

            using var command = new SQLiteCommand(query, connection);

            // Add SET parameters
            foreach (var item in data)
            {
                command.Parameters.AddWithValue("@" + item.Key, item.Value ?? DBNull.Value);
            }

            // Add WHERE parameters
            if (parameters != null)
            {
                for (int i = 0; i < parameters.Length; i++)
                {
                    command.Parameters.AddWithValue($"@p{i}", parameters[i] ?? DBNull.Value);
                }
            }
            var res = command.ExecuteNonQuery();
            return res;
        }

        // Delete operation
        public int Delete(string tableName, string whereClause, object[] parameters = null)
        {
            using var connection = new SQLiteConnection(_connectionString);
            connection.Open();

            var query = $"DELETE FROM {tableName}";
            if (!string.IsNullOrWhiteSpace(whereClause))
            {
                query += " WHERE " + whereClause;
            }

            using var command = new SQLiteCommand(query, connection);
            if (parameters != null)
            {
                for (int i = 0; i < parameters.Length; i++)
                {
                    command.Parameters.AddWithValue($"@p{i}", parameters[i] ?? DBNull.Value);
                }
            }

            return command.ExecuteNonQuery();
        }

        // Execute custom query
        public DataTable ExecuteQuery(string query, object[] parameters = null)
        {
            using var connection = new SQLiteConnection(_connectionString);
            connection.Open();

            using var command = new SQLiteCommand(query, connection);
            if (parameters != null)
            {
                for (int i = 0; i < parameters.Length; i++)
                {
                    command.Parameters.AddWithValue($"@p{i}", parameters[i] ?? DBNull.Value);
                }
            }

            var adapter = new SQLiteDataAdapter(command);
            var table = new DataTable();
            adapter.Fill(table);

            return table;

        }

    }
}
