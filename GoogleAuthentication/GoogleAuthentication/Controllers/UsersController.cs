using System.Linq;
using System.Web.Mvc;

namespace GoogleAuthentication.Controllers
{
    public class UsersController : Controller
    {
        public ActionResult Index()
        {
            if (User.IsInRole("Admin"))
            {
              
                webEntities2 db = new webEntities2();
                var list = db.UserAccounts.ToList();
                return View(list);
            }
            else 
            {
                ViewData["FirstName"] = User.Identity.Name;
                return RedirectToAction("MyView", "Home");
            }
         

        }
    }
}