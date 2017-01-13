using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EleWise.ELMA.ComponentModel;
using EleWise.ELMA.Elmagram.Web.Controllers;
using EleWise.ELMA.Web.Content.Menu;

namespace EleWise.ELMA.Elmagram.Web.Components
{
    [Component]
    public class ElmagramMenuItemProvider : IMenuItemsProvider
    {
        public void Items(MenuItemFactory factory)
        {
            factory.Action<HomeController>(c => c.Index()).Order(100).Container("left");
        }

        public List<string> LocalizedItemsNames
        {
            get
            {
                return new List<string> { SR.T("Elmagram") };
            }
        }

        public List<string> LocalizedItemsDescriptions
        {
            get
            {
                return new List<string> { SR.T("Elmagram - онлайн чат с пользователями ELMA!") };
            }
        }
    }
}