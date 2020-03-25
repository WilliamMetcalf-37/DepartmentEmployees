using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data;
using Microsoft.Data.SqlClient;
using DepartmentExercises.Models;
using Microsoft.AspNetCore.Http;

namespace DepartmentExercises.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IConfiguration _config;

        public EmployeeController(IConfiguration config)
        {
            _config = config;
        }
        public SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }
        
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT e.Id, e.FirstName, e.LastName, e.DepartmentId, d.Id, d.DeptName
                        FROM Employee e
                        LEFT JOIN Department d
                        ON e.DepartmentId = d.Id";
                    
                    SqlDataReader reader = cmd.ExecuteReader();
                    List<Employee> employees = new List<Employee>();
                    // The reader will read the returned data from the database if it finds the single row we're looking for. If it doesn't find the employee with the given Id, reader.Read() will return false
                    while (reader.Read())
                    {
                        // Get ordinal returns us what "position" the Id column is in
                        int idColumn = reader.GetOrdinal("Id");
                        int idValue = reader.GetInt32(idColumn);

                       
                        int firstNameColumn = reader.GetOrdinal("FirstName");
                        string firstNameValue = reader.GetString(firstNameColumn);

                        int lastNameColumn = reader.GetOrdinal("LastName");
                        string lastNameValue = reader.GetString(lastNameColumn);

                        int departmentIdColumn = reader.GetOrdinal("DepartmentId");
                        int departmentValue = reader.GetInt32(departmentIdColumn);

                        int departmentNameColumn = reader.GetOrdinal("DeptName");
                        string departmentNameValue = reader.GetString(departmentNameColumn);

                       
                        var employee = new Employee()
                        {
                            Id = idValue,
                            FirstName = firstNameValue,
                            LastName = lastNameValue,
                            DepartmentId = departmentValue,
                            Department = new Department()
                            {
                                Id = departmentValue,
                                DeptName = departmentNameValue
                            }
                        };
                        employees.Add(employee);
                    }
                        // Now we can close the reader
                        reader.Close();

                        return Ok(employees);
                    
                    
                }
            }
        }
        [HttpGet("{id}", Name = "GetEmployee")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT e.Id, e.FirstName, e.LastName, e.DepartmentId, d.Id, d.DeptName
                        FROM Employee e
                        LEFT JOIN Department d
                        ON e.DepartmentId = d.Id
                        WHERE e.Id = @id";

                    cmd.Parameters.Add(new SqlParameter("@id", id));

                    SqlDataReader reader = cmd.ExecuteReader();
                    List<Employee> employees = new List<Employee>();
                    // The reader will read the returned data from the database if it finds the single row we're looking for. If it doesn't find the employee with the given Id, reader.Read() will return false
                    while (reader.Read())
                    {
                        // Get ordinal returns us what "position" the Id column is in
                        int idColumn = reader.GetOrdinal("Id");
                        int idValue = reader.GetInt32(idColumn);


                        int firstNameColumn = reader.GetOrdinal("FirstName");
                        string firstNameValue = reader.GetString(firstNameColumn);

                        int lastNameColumn = reader.GetOrdinal("LastName");
                        string lastNameValue = reader.GetString(lastNameColumn);

                        int departmentIdColumn = reader.GetOrdinal("DepartmentId");
                        int departmentValue = reader.GetInt32(departmentIdColumn);

                        int departmentNameColumn = reader.GetOrdinal("DeptName");
                        string departmentNameValue = reader.GetString(departmentNameColumn);


                        var employee = new Employee()
                        {
                            Id = idValue,
                            FirstName = firstNameValue,
                            LastName = lastNameValue,
                            DepartmentId = departmentValue,
                            Department = new Department()
                            {
                                Id = departmentValue,
                                DeptName = departmentNameValue
                            }
                        };
                        employees.Add(employee);
                    }
                    // Now we can close the reader
                    reader.Close();

                    return Ok(employees);


                }
            }
        }
    }
}