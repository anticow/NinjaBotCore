using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NinjaBotCore;
using NinjaBotCore.Database;
using Xunit;
using System.ComponentModel.DataAnnotations;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace NinjaBotCore.Tests
{
    internal class DbTestCases
    {
        /// <summary>
        /// Get an In memory version of the app db context with some seeded data
        /// </summary>
        //public static AppDbContext GetAppDbContext(string dbName)
        //{
        //    //set up the options to use for this dbcontext
        //    var options = new DbContextOptionsBuilder<AppDbContext>()
        //        .UseInMemoryDatabase(databaseName: dbName)
        //        //.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
        //        .Options;
        //
        //    var dbContext = new AppDbContext(options);
        //    dbContext.SeedAppDbContext();
        //    return dbContext;
        //}
    }
}
