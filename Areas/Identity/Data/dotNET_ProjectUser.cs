using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace dotNET_Project.Areas.Identity.Data;

// Add profile data for application users by adding properties to the dotNET_ProjectUser class
public class dotNET_ProjectUser : IdentityUser
{
    public string Nickname { get; set; }

    public string Permissions { get; set; }
}

