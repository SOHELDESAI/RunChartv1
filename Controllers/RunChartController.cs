using Microsoft.AspNetCore.Mvc;
using RunChartv1.Models;
using System;
using System.Collections.Generic;
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

        // Generate sample data (x-values: 1 to 10, y-values: random)
        var random = new Random();
        var dataPoints = Enumerable.Range(1, 10)
            .Select(x => new DataPoint { X = x, Y = random.Next(1, 100) })
            .ToList();

        return View(dataPoints);
    }

    [HttpGet]
    public IActionResult RefreshData()
    {
        // Generate new data for the refresh
        var newData = Enumerable.Range(1, 10)
            .Select(x => new DataPoint { X = x, Y = _random.Next(1, 100) })
            .ToList();

        return Json(newData);
    }
}
