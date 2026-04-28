using Microsoft.AspNetCore.Mvc;
using DevelopTest.Services;
using DevelopTest.Models;

namespace DevelopTest.Controllers;

public class SportAreaController : Controller
{
    
    private readonly SportAreaService _sportAreaService;

    public SportAreaController(SportAreaService sportAreaService)
    {
        _sportAreaService = sportAreaService;
    }

    public IActionResult Index()
    {
        var response = _sportAreaService.GetAllSportAreas();
        return View(response.Data);
    }
    
    public IActionResult Create()
    {
        return View();
    }
    
    [HttpPost]
    public IActionResult Store(SportArea sportArea)
    {
        var newSportArea = _sportAreaService.SaveSportArea(sportArea);
        if (newSportArea.Success == true)
        {
            TempData["message"] = newSportArea.Message;
            return RedirectToAction("Index");
        }
        else
        {
            TempData["message"] = newSportArea.Message;
            return RedirectToAction("Create");
        }
    }
    
    public IActionResult Show(int id)
    {
        var result = _sportAreaService.GetSportAreaById(id);
        return View(result.Data);
    }
    
    
    public IActionResult Edit(int id)
    {
        var result = _sportAreaService.GetSportAreaById(id);
        return View(result.Data);
    }

    [HttpPost]
    public IActionResult Update(SportArea sportArea)
    {
        var result = _sportAreaService.UpdateSportArea(sportArea);
        return RedirectToAction("Index");
    }
    
    [HttpPost]
    public IActionResult Destroy(SportArea sportArea)
    {
        var result = _sportAreaService.DeleteSportArea(sportArea);
        TempData["message"] = result.Message;
        return RedirectToAction("Index");
    }
}