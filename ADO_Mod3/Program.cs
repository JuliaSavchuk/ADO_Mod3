﻿using System;
using System.Data;
using System.Data.SqlClient;

class Program
{
    static void Main()
    {
        string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;Connect Timeout=30;";
        SqlConnection connection = new SqlConnection(connectionString);

        while (true)
        {
            Console.WriteLine("\n--- Menu ---");
            Console.WriteLine("1. Connect to the database");
            Console.WriteLine("2. Disconnect from the database");
            Console.WriteLine("3. Display all information");
            Console.WriteLine("4. Display the full name of all students");
            Console.WriteLine("5. Display all average grades");
            Console.WriteLine("6. Show students with minimum grade > N");
            Console.WriteLine("7. Show unique items with minimum ratings");
            Console.WriteLine("8. Show minimum grade point average");
            Console.WriteLine("9. Show maximum average score");
            Console.WriteLine("10. Number of students from min. grade in mathematics");
            Console.WriteLine("11. Number of students with max. grade in mathematics");
            Console.WriteLine("12. Number of students in each group");
            Console.WriteLine("13. The average score of the group");
            Console.WriteLine("0. Exit");

            Console.Write("Choice : ");
            string choice = Console.ReadLine();

            try
            {
                switch (choice)
                {
                    case "1":
                        ConnectToDatabase(connection);
                        break;
                    case "2":
                        DisconnectFromDatabase(connection);
                        break;
                    case "3":
                        ExecuteQuery(connection, "SELECT * FROM Grades");
                        break;
                    case "4":
                        ExecuteQuery(connection, "SELECT FullName FROM Grades");
                        break;
                    case "5":
                        ExecuteQuery(connection, "SELECT Mark FROM Grades");
                        break;
                    case "6":
                        Console.Write("Enter a minimum score: ");
                        decimal minMark = decimal.Parse(Console.ReadLine());
                        ExecuteQuery(connection, $"SELECT FullName FROM Grades WHERE Mark > {minMark}");
                        break;
                    case "7":
                        ExecuteQuery(connection, "SELECT DISTINCT MinSubject FROM Grades");
                        break;
                    case "8":
                        ExecuteQuery(connection, "SELECT MIN(Mark) AS MinAverage FROM Grades");
                        break;
                    case "9":
                        ExecuteQuery(connection, "SELECT MAX(Mark) AS MaxAverage FROM Grades");
                        break;
                    case "10":
                        ExecuteQuery(connection, "SELECT COUNT(*) FROM Grades WHERE MinSubject = 'Math'");
                        break;
                    case "11":
                        ExecuteQuery(connection, "SELECT COUNT(*) FROM Grades WHERE MaxSubject = 'Math'");
                        break;
                    case "12":
                        ExecuteQuery(connection, "SELECT GroupName, COUNT(*) AS StudentCount FROM Grades GROUP BY GroupName");
                        break;
                    case "13":
                        ExecuteQuery(connection, "SELECT GroupName, AVG(Mark) AS AvgYearGrade FROM Grades GROUP BY GroupName");
                        break;
                    case "0":
                        Console.WriteLine("Goodbye!");
                        return;
                    default:
                        Console.WriteLine("Incorrect selection, try again.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }

    static void ConnectToDatabase(SqlConnection connection)
    {
        try
        {
            connection.Open();
            Console.WriteLine("Database connection successful!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Connection error: {ex.Message}");
        }
    }

    static void DisconnectFromDatabase(SqlConnection connection)
    {
        try
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
                Console.WriteLine("Disconnect from the database is successful!");
            }
            else
            {
                Console.WriteLine("The connection is already closed.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Disconnect error: {ex.Message}");
        }
    }

    static void ExecuteQuery(SqlConnection connection, string query)
    {
        try
        {
            if (connection.State != ConnectionState.Open)
            {
                Console.WriteLine("Please connect to the database first.");
                return;
            }

            SqlCommand command = new SqlCommand(query, connection);
            SqlDataReader reader = command.ExecuteReader();

            DataTable table = new DataTable();
            table.Load(reader);
            reader.Close();

            foreach (DataColumn column in table.Columns)
            {
                Console.Write($"{column.ColumnName}\t");
            }
            Console.WriteLine();

            foreach (DataRow row in table.Rows)
            {
                foreach (var item in row.ItemArray)
                {
                    Console.Write($"{item}\t");
                }
                Console.WriteLine();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Request execution error: {ex.Message}");
        }
    }
}