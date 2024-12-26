using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MasterPol.Models;

namespace MasterPol.DataBaseConnect
{
    public static class AppConnect
    {
        public static MasterPolEntities MasterPolBD = new MasterPolEntities();
    }
}
