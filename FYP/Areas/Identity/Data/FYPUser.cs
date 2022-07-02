using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace FYP.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the FYPUser class
    public class FYPUser : IdentityUser
    {
        [PersonalData]
        public string FullName { get; set; }

        [PersonalData]
        public DateTime DOB { get; set; }

        //[PersonalData]
        //public string Contact { get; set; }

        [PersonalData]
        public string NRIC { get; set; }
    }
}
