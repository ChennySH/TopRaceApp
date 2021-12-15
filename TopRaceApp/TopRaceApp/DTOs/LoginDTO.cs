using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TopRaceApp.DTOs
{
    public class LoginDTO
    {
        public string UserNameOrEmail { get; set; }
        public string Password { get; set; }
    }
}
