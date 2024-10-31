using DevGuide.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Managers
{
    public class MyRoleManager: BaseManager<IdentityRole>
    {
        RoleManager<IdentityRole> roleManager;
    public MyRoleManager(ProjectContext context, RoleManager<IdentityRole> _roleManager) : base(context)
    {
        roleManager = _roleManager;

    }

    public async Task<IdentityResult> Add(string RoleName)
    {
        var role = new IdentityRole() { Name = RoleName };
        return await roleManager.CreateAsync(role);
    }
}
}
   