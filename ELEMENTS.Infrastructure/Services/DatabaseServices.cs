﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELEMENTS.Infrastructure
{

    public interface ISQLiteService
    {
        IFactoryStatusInfo CreateDatabase(string name);
        IFactoryStatusInfo MigrateDatabase(string name);
        string GetMigrationVersion();
        IFactory Factory { get; set; }
    }
}
