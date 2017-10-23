using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdventureHelper.Website.Controllers
{
    [RoutePrefix("journal")]
    public class JournalController : Controller
    {
        [Route("")]
        public ViewResult Journal()
        {
            return View();
        }
    }
}