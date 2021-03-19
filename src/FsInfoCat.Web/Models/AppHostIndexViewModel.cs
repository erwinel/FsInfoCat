using FsInfoCat.Models.DB;
using FsInfoCat.Models.HostDevices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FsInfoCat.Web.Models
{
    public class AppHostIndexViewModel : HostDevice
    {
        public bool IsRegistered { get; set; }

        private AppHostIndexViewModel(HostDevice host) : base(host)
        {
            IsRegistered = true;
        }

        private AppHostIndexViewModel(HostDeviceRegRequest request, Guid createdBy) : base(request, createdBy)
        {
            IsRegistered = false;
            AllowCrawl = false;
        }

        public static async Task<AppHostIndexViewModel> Create(DbSet<HostDevice> dbSet, ClaimsPrincipal user)
        {
            // TODO: Need to find some other way to retrieve device reg for local host
#warning Need to find some other way to retrieve device reg for local host
            throw new NotImplementedException();
            // HostDeviceRegRequest regRequest = HostDeviceRegRequest.CreateForLocal();
            // HostDevice host = await ViewModelHelper.LookUp(dbSet, regRequest.MachineName, regRequest.MachineIdentifer);
            // if (host is null)
            //     return new AppHostIndexViewModel(regRequest, new Guid(user.Identity.Name));
            // return new AppHostIndexViewModel(host);
        }
    }
}
