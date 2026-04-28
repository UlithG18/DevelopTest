using Microsoft.AspNetCore.Mvc;
using DevelopTest.Services;
using DevelopTest.Models;

namespace DevelopTest.Controllers;

public class UserController : Controller
{
    
    private readonly UserService _userService;

    public UserController(UserService userService)
    {
        _userService = userService;
    }

    public IActionResult Index()
    {
        var response = _userService.GetAllUsers();
        return View(response.Data);
    }
    
    public IActionResult Create()
    {
        return View();
    }
    
    [HttpPost]
    public IActionResult Store(User user)
    {
        var newUser = _userService.SaveUser(user);
        if (newUser.Success == true)
        {
            TempData["message"] = newUser.Message;
            return RedirectToAction("Index");
        }
        else
        {
            TempData["message"] = newUser.Message;
            return RedirectToAction("Create");
        }
    }
    
    public IActionResult Show(int id)
    {
        var result = _userService.GetUserById(id);
        return View(result.Data);
    }
    
    
    public IActionResult Edit(int id)
    {
        var result = _userService.GetUserById(id);
        return View(result.Data);
    }

    [HttpPost]
    public IActionResult Update(User user)
    {
        var result = _userService.UpdateUser(user);
        return RedirectToAction("Index");
    }
    
    [HttpPost]
    public IActionResult Destroy(User user)
    {
        var result = _userService.DeleteUser(user);
        TempData["message"] = result.Message;
        return RedirectToAction("Index");
    }
}