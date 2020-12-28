using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Geex.Data.Migrations;

using MongoDB.Entities;

namespace Geex.Data
{
    class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("confirm migration?");
            Console.ReadKey();
            Migrate().Wait();

        }

        public static async Task Migrate()
        {
            await DB.MigrateAsync();
        }
    }
}
