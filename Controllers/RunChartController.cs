﻿using Microsoft.AspNetCore.Mvc;
using RunChartv1.Models;
using System;
using System.Collections.Generic;
using System.Linq;

public class RunChartController : Controller
{
    public IActionResult Index()
    {
        // Generate sample data (x-values: 1 to 10, y-values: random)
        var random = new Random();
        var dataPoints = Enumerable.Range(1, 10)
            .Select(x => new DataPoint { X = x, Y = random.Next(1, 100) })
            .ToList();

        return View(dataPoints);
    }
}
