using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;

namespace KanbanApp
{
    public class TaskRepository
    {
        private const string DatabaseFileName = "Tasks.db";

        public TaskRepository()
        {
            if (!File.Exists(DatabaseFileName))
            {
                SQLiteConnection.CreateFile(DatabaseFileName);
            }

            using (var connection = new SQLiteConnection($"Data Source={DatabaseFileName};Version=3;"))
            {
                connection.Open();

                string sql = "CREATE TABLE IF NOT EXISTS Tasks (Name TEXT, DueDate TEXT, Duration TEXT, Priority TEXT, Status TEXT, ElapsedTime TEXT)";
                using (var command = new SQLiteCommand(sql, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public List<Task> GetAllTasks()
        {
            var tasks = new List<Task>();

            using (var connection = new SQLiteConnection($"Data Source={DatabaseFileName};Version=3;"))
            {
                connection.Open();

                string sql = "SELECT * FROM Tasks";
                using (var command = new SQLiteCommand(sql, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tasks.Add(new Task
                            {
                                Name = (string)reader["Name"],
                                DueDate = DateTime.Parse((string)reader["DueDate"]),
                                Duration = (string)reader["Duration"],
                                Priority = (string)reader["Priority"],
                                Status = (string)reader["Status"],
                                ElapsedTime = TimeSpan.Parse((string)reader["ElapsedTime"])
                            });
                        }
                    }
                }
            }

            return tasks;
        }

        public void AddTask(Task task)
        {
            using (var connection = new SQLiteConnection($"Data Source={DatabaseFileName};Version=3;"))
            {
                connection.Open();

                string sql = $"INSERT INTO Tasks (Name, DueDate, Duration, Priority, Status, ElapsedTime) VALUES ('{task.Name}', '{task.DueDate}', '{task.Duration}', '{task.Priority}', '{task.Status}', '{task.ElapsedTime}')";
                using (var command = new SQLiteCommand(sql, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public void UpdateTask(Task task)
        {
            using (var connection = new SQLiteConnection($"Data Source={DatabaseFileName};Version=3;"))
            {
                connection.Open();

                string sql = $"UPDATE Tasks SET DueDate = '{task.DueDate}', Duration = '{task.Duration}', Priority = '{task.Priority}', Status = '{task.Status}', ElapsedTime = '{task.ElapsedTime}' WHERE Name = '{task.Name}'";
                using (var command = new SQLiteCommand(sql, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteTask(Task task)
        {
            using (var connection = new SQLiteConnection($"Data Source={DatabaseFileName};Version=3;"))
            {
                connection.Open();

                string sql = $"DELETE FROM Tasks WHERE Name = '{task.Name}'";
                using (var command = new SQLiteCommand(sql, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}