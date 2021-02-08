using SamuraiApp.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;


namespace SamuraiApp.UI
{
    internal class Program
    {
        //Refactored out to BusinessDataLogic()
        //private static SamuraiContext _context = new SamuraiContext();
        private static void Main(string[] args)
        {
            AddSamuraiByName("Shimada", "Okamoto", "Kikuchio", "Hayashida");
        }

        private static void AddSamuraiByName(params string[] names)
        {
            //Refactored out to BusinessDataLogic()
            //foreach (string name in names)
            //{
            //    _context.Samurais.Add(new Samurai { Name = name });
            //}
            //_context.SaveChanges();
            var _bizData = new BusinessDataLogic();
            var newSamuraisCreatedCount = _bizData.AddSamuraisByName(names);
        }

       
    }
}


