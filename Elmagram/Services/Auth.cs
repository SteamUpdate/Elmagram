using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elmagram.Services
{
    public class Auth
    {
        public string AuthToken { get; set; }
        public string CurrentUserId { get; set; }
        public string SessionToken { get; set; }
    }
}
