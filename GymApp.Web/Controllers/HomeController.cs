using Microsoft.AspNetCore.Mvc;

namespace GymApp.Web.Controllers;

public class HomeController : Controller
{
    // GET: HomeController
    public ActionResult Index()
    {
        return View();
    }

}

