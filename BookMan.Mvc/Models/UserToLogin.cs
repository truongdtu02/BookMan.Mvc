using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookMan.Mvc.Models
{
    public class UserToLogin
    {
        public string UserName { get; set; }
        public string Password { get; set; }

        public string Role { get; set; } 

    }
}
