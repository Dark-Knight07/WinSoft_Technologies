using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace EmployeeManagementApp
{
    public partial class MainForm : Form
    {
        private string connectionString = "YourConnectionString";
        private SqlConnection connection;

        public MainForm()
        {
            InitializeComponent();
            connection = new SqlConnection(connectionString);
            connection.Open();

            // Step 1: Create Database
            CreateDatabase();

            // Step 2: Create Table with Primary Key Constraint
            CreateTable();

            // Step 3: Print All Information
            PrintAllInformation();

            // Step 4: CRUD Operations
            InsertEmployee(1, "John", "Doe", 25, "IT");
            UpdateEmployee(1, "John", "Doe", 26, "IT");
            DeleteEmployee(1);

            // Step 5: Display Information in DataGridView
            DisplayInformationInDataGridView();
        }

        private void CreateDatabase()
        {
            using (SqlCommand command = new SqlCommand("CREATE DATABASE IF NOT EXISTS EmployeeDatabase", connection))
            {
                command.ExecuteNonQuery();
            }
        }

        private void CreateTable()
        {
            using (SqlCommand command = new SqlCommand(
                @"USE EmployeeDatabase;
                  CREATE TABLE IF NOT EXISTS Employee (
                      Id INT PRIMARY KEY,
                      FirstName NVARCHAR(50),
                      LastName NVARCHAR(50),
                      Age INT,
                      Department NVARCHAR(50)
                  )", connection))
            {
                command.ExecuteNonQuery();
            }
        }

        private void PrintAllInformation()
        {
            using (SqlCommand command = new SqlCommand("SELECT * FROM Employee", connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"{reader["Id"]}, {reader["FirstName"]}, {reader["LastName"]}, {reader["Age"]}, {reader["Department"]}");
                    }
                }
            }
        }

        private void InsertEmployee(int id, string firstName, string lastName, int age, string department)
        {
            using (SqlCommand command = new SqlCommand(
                "INSERT INTO Employee VALUES (@Id, @FirstName, @LastName, @Age, @Department)", connection))
            {
                command.Parameters.AddWithValue("@Id", id);
                command.Parameters.AddWithValue("@FirstName", firstName);
                command.Parameters.AddWithValue("@LastName", lastName);
                command.Parameters.AddWithValue("@Age", age);
                command.Parameters.AddWithValue("@Department", department);

                command.ExecuteNonQuery();
            }
        }

        private void UpdateEmployee(int id, string firstName, string lastName, int age, string department)
        {
            using (SqlCommand command = new SqlCommand(
                "UPDATE Employee SET FirstName = @FirstName, LastName = @LastName, Age = @Age, Department = @Department WHERE Id = @Id", connection))
            {
                command.Parameters.AddWithValue("@Id", id);
                command.Parameters.AddWithValue("@FirstName", firstName);
                command.Parameters.AddWithValue("@LastName", lastName);
                command.Parameters.AddWithValue("@Age", age);
                command.Parameters.AddWithValue("@Department", department);

                command.ExecuteNonQuery();
            }
        }

        private void DeleteEmployee(int id)
        {
            using (SqlCommand command = new SqlCommand("DELETE FROM Employee WHERE Id = @Id", connection))
            {
                command.Parameters.AddWithValue("@Id", id);

                command.ExecuteNonQuery();
            }
        }

        private void DisplayInformationInDataGridView()
        {
            using (SqlCommand command = new SqlCommand("SELECT * FROM Employee", connection))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                // Assuming dataGridView1 is the name of your DataGridView control
                dataGridView1.DataSource = dataTable;
            }
        }
    }
}
