using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xyToolz
{
    public class xyDBContext
    {
        public String Description { get; set; } = "Generic DBcontext for easier interactions via EF-Core";

        public DbSet<object> Entities { get; set; }

        public String pathForDB { get; set; }

        public xyDBContext()
        {
        }

        public String GetPathForLocalDbFromWithinApplicationFolder(String pathDB = @"\DB\xyLagerverwaltung.mdf")
        {
            try
            {
                String appFolder = xyFolder.GetInnerApplicationFolder();
                xyLog.Log(pathForDB = Path.Join(appFolder, pathDB));
            }
            catch (Exception ex)
            {
                xyLog.ExLog(ex);
            }
            return pathForDB;
        }
    }
}

