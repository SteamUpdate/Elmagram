using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EleWise.ELMA.Extensions;
using EleWise.ELMA.Web.Content;
using EleWise.ELMA.Web.Mvc.Extensions;
using EleWise.ELMA.Web.Mvc.Attributes;
using EleWise.ELMA.Web.Mvc.Controllers;
using Orchard;
using Orchard.Themes;

namespace EleWise.ELMA.Elmagram.Web.Controllers
{
    [Themed]
    public class HomeController : BaseController
    {
        [ContentItem(Name = "Elmagram",
                    Image32 = RouteProvider.ImagesFolder + "x32/elmagram.png")]
        public ActionResult Index()
        {
            return View();
        }

    }
}
