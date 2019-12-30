using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace ReflectionDemo.EntityFramework
{
    class Program
    {
        static void Main(string[] args)
        {
            CreateSQLTable(typeof(Person), "People");
            Console.WriteLine("Press any key to close!");
            Console.ReadKey(); 
        }

        static void CreateSQLTable(Type type, string tableName)
        {
            // Create a connection 
            var connection = new SqlConnection("Server=.\\SQLExpress;Database=ReflectionDemoDb;Integrated Security=true");

            // Construct the query 
            // 1- Get the properties 
            var properties = type.GetProperties();

            var queryBuilder = new StringBuilder();
            queryBuilder.Append($"CREATE TABLE {tableName} (");
            var columns = new List<string>();
            foreach (var property in properties)
            {
                // Check if property is primary key 
                if (property.Name.ToLower() == "id" && property.PropertyType.Name == "Int32") // Auto Increment primary key \
                    columns.Add("Id int IDENTITY(1,1) PRIMARY KEY");

                if (property.PropertyType.Name == "String")
                    columns.Add($"{property.Name} nvarchar(max)");
            }
            string columnsString = string.Join(',', columns);
            queryBuilder.Append(columnsString);
            queryBuilder.Append(")");

            var command = new SqlCommand(queryBuilder.ToString(), connection);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();

            Console.WriteLine($"{tableName} has been created successfully!");
        }
    }

    public class Person
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string ZipCode { get; set; }
    }
}
