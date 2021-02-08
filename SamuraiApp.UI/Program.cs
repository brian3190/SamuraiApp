using SamuraiApp.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;


namespace SamuraiApp.UI
{
    internal class Program
    {
        private static SamuraiContext _context = new SamuraiContext();

        //NoTrackingContext
        //private static SamuraiContext _contextNT = new SamuraiContextNoTracking();

        private static void Main(string[] args)
        {
            //QuerySamuraiBattleStats();
            //QueryUsingRawlSql();
            //QueryRelatedUsingRawSql();
            //QueryUsingRawSqlWithInterpolation();
            //DANGERQueryUsingRawSqlWithInterpolation();
            QueryUsingFromSqlIntStoredProc();
            //ExecuteSomeRawSql();//RawSql features only work in relational database
            //AddSamuraiByName("Shimada", "Okamoto", "Kikuchio", "Hayashida");
        }

        //private static void AddSamuraiByName(params string[] names)
        //{
            
        //}

        private static void QuerySamuraiBattleStats()
        {
            //var stats = _context.SamuraiBattleStats.ToList();
            //var firststat = _context.SamuraiBattleStats.FirstOrDefault();
            //var sampsonState = _context.SamuraiBattleStats
            //    .FirstOrDefault(b => b.Name == "SampsonSan");
            var findone = _context.SamuraiBattleStats.Find(2);
        }

        //Querying with Raw SQL
        private static void QueryUsingRawlSql()
        {
            var samurais = _context.Samurais.FromSqlRaw("Select * from samurais").ToList();
        }
        private static void QueryRelatedUsingRawSql()
        {
            var samurais = _context.Samurais.FromSqlRaw(
                "Select Id, Name from Samurais").Include(s => s.Quotes).ToList();//EF core translate include to left join
        }
        private static void QueryUsingRawSqlWithInterpolation()
        {
            string name = "Kikuchyo";
            var samurais = _context.Samurais
                .FromSqlInterpolated($"Select * from Samurais Where Name= {name}")
                .ToList();
        }

        private static void DANGERQueryUsingRawSqlWithInterpolation()
        {
            string name = "Kikuchyo";
            var samurais = _context.Samurais
                .FromSqlRaw("INTERPOLATION HERE WILL CAUSE SQL INJECTION")
                .ToList();
        }

        //Running Stored Procedure Queries with Raw SQL
        private static void QueryUsingFromSqlRawStoredProc()
        {
            var text = "Happy";
            var samurais = _context.Samurais.FromSqlRaw(
                "EXEC dbo.SamuraisWhoSaidAWord {0}", text).ToList();
        }
        private static void QueryUsingFromSqlIntStoredProc()
        {
            var text = "Happy";
            var samurais = _context.Samurais.FromSqlInterpolated(
                $"EXEC dbo.SamuraisWhoSaidAWord {text}").ToList();
        }

        //Executing non-Query Raw SQL 
        private static void ExecuteSomeRawSql()
        {
            var samuraiId = 2;
            //Database method accesses the db configured for the DbContext instance
            //var affected = _context.Database.ExecuteSqlRaw("EXEC DeleteQuotesForSamurai {0}", samuraiId); //will also return number of rows affected
            var affected = _context.Database
                .ExecuteSqlInterpolated($"EXEC DeleteQuotesForSamurai {samuraiId}");
        }
    }
}


