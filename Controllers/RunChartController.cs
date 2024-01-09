using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using RunChartv1.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

public class RunChartController : Controller
{
    private readonly IConfiguration _configuration;
    private readonly Random _random = new Random();

    public RunChartController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IActionResult Index()
    {
        var dataRefreshIntervalMinutes = _configuration.GetValue<int>("RunChartSettings:DataRefreshIntervalMinutes");
        var connectionstrings = _configuration.GetValue<string>("connectionstrings:defaultconnection");

        //  string query = "SELECT *,100 as Max,80 as USL ,60 as UCL,50 as Mean,30 as LCL,10 as LSL,0 as Min FROM runchartdata";


        // Get the entire query string
        string queryString = HttpContext.Request.QueryString.Value;

        // Parse the query string into a dictionary
        //var queryParams = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(queryString);

        var queryParams = QueryHelpers.ParseQuery(queryString);

        // Access specific parameters
        

        if (queryParams.ContainsKey("Name"))
        {
            
            string nameParam = queryParams["Name"].ToString();

            
        }

        try
        {
            List< DataPoint > points = new List< DataPoint >();
            using (SqlConnection connection = new SqlConnection(connectionstrings))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("GetRunchartData", connection))
                {

                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("reccount",10);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            points.Add(new DataPoint()
                            {

                                X = Convert.ToDouble(reader["X"]),
                                Y = Convert.ToDouble(reader["Y"]),
                                Max = Convert.ToDouble(reader["Max"]),
                                USL = Convert.ToDouble(reader["USL"]),
                                UCL = Convert.ToDouble(reader["UCL"]),
                                Mean = Convert.ToDouble(reader["Mean"]),
                                LCL = Convert.ToDouble(reader["LCL"]),
                                LSL = Convert.ToDouble(reader["LSL"]),
                                Min = Convert.ToDouble(reader["Min"]),
                            });
                            
                        }
                        return View(points);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }


        // Generate sample data (x-values: 1 to 10, y-values: random)
        //var random = new Random();
        //var dataPoints = Enumerable.Range(1, 10)
        //    .Select(x => new DataPoint { X = x, Y = random.Next(1, 100) , Max=100,USL = 80,UCL=60,Mean=50,LCL=30,LSL=10,Min=0})
        //    .ToList();

       return View();
    }

    [HttpGet]
    //public IActionResult RefreshData()
    //{
    //    // Generate new data for the refresh
    //    var newData = Enumerable.Range(1, 10)
    //        .Select(x => new DataPoint { X = x, Y = _random.Next(1, 100) })
    //        .ToList();

    //    return Json(newData);
    //}

    public IActionResult RefreshData()
    {
        // Generate new data for the refresh
        var newData = Enumerable.Range(1, 10)
            .Select(x => new DataPoint { X = x, Y = _random.NextDouble() * (90.5 - 1.5) + 1.5 })
            .ToList();

        return Json(newData);
    }

}
