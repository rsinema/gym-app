using GymApp.Services.UserService;
using Microsoft.AspNetCore.Mvc;

namespace GymApp.Web.Controllers;
public class UserController : Controller
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    public IActionResult Index()
    {
        var user = _userService.GetUser();
        return View(user);
    }
}
