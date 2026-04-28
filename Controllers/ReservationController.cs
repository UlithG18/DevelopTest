using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using DevelopTest.Services;
using DevelopTest.Models;

namespace DevelopTest.Controllers;

public class ReservationController :  Controller
{
    private readonly ReservationService _reservationService;
    private readonly UserService _userService;
    private readonly SportAreaService _sportAreaService;

    public ReservationController(ReservationService reservationService, UserService userService, SportAreaService sportAreaService)
    {
        _reservationService = reservationService;
        _userService = userService;
        _sportAreaService = sportAreaService;
    }

    public IActionResult Index()
    {
        var response = _reservationService.GetAllReservation();
        return View(response.Data);
    }

    public IActionResult Create()
    {
        SelectLists();
        return View();
    }

    [HttpPost]
    public IActionResult Store(Reservation reservation)
    {
        var result = _reservationService.SaveReservation(reservation);
        if (result.Success)
        {
            TempData["message"] = result.Message;
            return RedirectToAction("Index");
        }
        else
        {
            TempData["message"] = result.Message;
            return RedirectToAction("Create");
        }
    }

    public IActionResult Show(int id)
    {
        var result = _reservationService.GetReservationById(id);
        return View(result.Data);
    }

    public IActionResult Edit(int id)
    {
        var result = _reservationService.GetReservationById(id);
        SelectLists();
        return View(result.Data);
    }

    [HttpPost]
    public IActionResult Update(Reservation reservation)
    {
        var result = _reservationService.UpdateReservation(reservation);
        TempData["message"] = result.Message;
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult Destroy(Reservation reservation)
    {
        var result = _reservationService.DeleteReservation(reservation);
        TempData["message"] = result.Message;
        return RedirectToAction("Index");
    }

    private void SelectLists()
    {
        var users = _userService.GetAllUsers().Data ?? new List<User>();
        var sportAreas = _sportAreaService.GetAllSportAreas().Data ?? new List<SportArea>();

        ViewBag.Users = new SelectList(users, "Id", "Name");
        ViewBag.SportAreas = new SelectList(sportAreas, "Id", "Name");
    }
}