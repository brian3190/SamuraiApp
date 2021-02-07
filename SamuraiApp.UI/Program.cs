using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SamuraiApp.Data;
using SamuraiApp.Domain;

namespace SamuraiApp.UI
{
    class Program
    {
        private static SamuraiContext _context = new SamuraiContext();

        //NoTrackingContext
        //private static SamuraiContext _contextNT = new SamuraiContextNoTracking();

        private static void Main(string[] args)
        {
            //_context.Database.EnsureCreated();
            //AddSamuraisByName("Shimada", "Okamoto", "Kikuchio", "Hayashida");
            //GetSamurais();
            //QueryFiltersH();
            //QueryFilters();
            //QueryUsingLikeFunctions();
            //QueryUsingContains();
            //QueryAggregates();
            //QueryOne();
            //RetrieveAndUpdateSamurai();
            //RetrieveAndUpdateMultipleSamurai();
            //MultipleDatabaseOperations();
            //Simpler_AddQuoteToExistingSamuraiNotTracked(4);
            Console.Write("Press any key...");
            Console.ReadKey();
        }

        private static void AddVariousTypes()
        {
            _context.Samurais.AddRange(
                new Samurai { Name = "Shimada" },
                new Samurai { Name = "Okamoto" });
            _context.Battles.AddRange(
                new Battle { Name = "Battle of Anegawa" },
                new Battle { Name = "Battle of Nagashino" });
            _context.SaveChanges();
        }

        private static void AddSamuraisByName(params string[] names)
        {
            foreach (string name in names)
            {
                _context.Samurais.Add(new Samurai { Name = name });
            }
            _context.SaveChanges();
        }
        private static void GetSamurais()
        {
            var samurais = _context.Samurais
                .TagWith("ConsoleApp.Program.GetSamurais method")
                .ToList();
            Console.WriteLine($"Samurai count is {samurais.Count}");
            foreach (var samurai in samurais)
            {
                Console.WriteLine(samurai.Name);
            }
        }

        //Hardcoded
        private static void QueryFiltersH()
        {
            var samurais = _context.Samurais.Where(s => s.Name == "Sampson").ToList();
        }

        private static void QueryFilters()
        {
            var name = "Sampson";
            var samurais = _context.Samurais.Where(s => s.Name == name).ToList();
        }

        //Like methods
        private static void QueryUsingLikeFunctions()
        {
            var samurais = _context.Samurais.Where(s =>
            EF.Functions.Like(s.Name, "Sampso%")).ToList();
        }
        
        //Contains methods
        private static void QueryUsingContains()
        {
            var samurais = _context.Samurais.Where(s => s.Name.Contains("%ampson")).ToList();
        }

        //QueryAggregates
        private static void QueryAggregates()
        {
            var name = "Sampson";
            var samurai = _context.Samurais.Where(s => s.Name == name).FirstOrDefault();
        }

        //DbSet Find method
        private static void QueryOne()
        {
            var name = "Sampson";
            var samurai = _context.Samurais.Find(2);
        }

        //Retrive and Update
        private static void RetrieveAndUpdateSamurai()
        {
            var samurai = _context.Samurais.FirstOrDefault();
            samurai.Name += "San";
            _context.SaveChanges();
        }

        //Batch update
        private static void RetrieveAndUpdateMultipleSamurai()
        {
            var samurai = _context.Samurais.Skip(1).Take(4).ToList();
            samurai.ForEach(s => s.Name += "San");
            _context.SaveChanges();
        }

        //Multiple Database Operations()
        private static void MultipleDatabaseOperations()
        {
            var samurai = _context.Samurais.FirstOrDefault();
            samurai.Name += "San";
            _context.Samurais.Add(new Samurai { Name = "Shino" });
            _context.SaveChanges();
        }
      
        //Delete Single Object (Remove vs RemoveRange)
        private static void RetrieveAndDeleteASamurai()
        {
            var samurai = _context.Samurais.Find(18);
            _context.Samurais.Remove(samurai);
            _context.SaveChanges();
        }

        private static void QueryAndUpdateBattles_Disconnected()
        {
            List<Battle> disconnectedBattles;
            using(var context1 = new SamuraiContext())
            {
                disconnectedBattles = _context.Battles.ToList();
            }
            disconnectedBattles.ForEach(b =>
                {
                    b.StartDate = new DateTime(1570, 01, 01);
                    b.EndDate = new DateTime(1570, 12, 1);
                });
            using (var context2 = new SamuraiContext())
            {
                context2.UpdateRange(disconnectedBattles);
                context2.SaveChanges();
            }

        }

        

        //Inserting Related Data
        private static void InsertNewSamuraiWithAQuote()
        {
            var samurai = new Samurai
            {
                Name = "Kambei Shimada",
                Quotes = new List<Quote>
                {
                    new Quote { Text = "I've come to save you"}
                }

            };
            _context.Samurais.Add(samurai);
            _context.SaveChanges();
        }

        //Inserting Many
        private static void InsertNewSamuraiWithManyQuotes()
        {
            var samurai = new Samurai
            {
                Name = "Kyuzo",
                Quotes = new List<Quote>
                {
                    new Quote { Text = "Watch out for my sharp sword!"},
                    new Quote { Text = "I told you to watch out for the sharp sword! Oh well!"}
                }

            };
            _context.Samurais.Add(samurai);
            _context.SaveChanges();
        }

        private static void AddQuoteToExistingSamuraiWhileTracked()
        {
            var samurai = _context.Samurais.FirstOrDefault();
            samurai.Quotes.Add(new Quote
            {
                Text = "I bet you're happy that I've saved you!"
            });
            _context.SaveChanges();
        }

        private static void AddQuoteToExistingSamuraiNotTracked(int samuraiId)
        {
            var samurai = _context.Samurais.Find(samuraiId);
            samurai.Quotes.Add(new Quote
            {
                Text = "Now that I saved you, will you feed me dinner?"
            });
            using (var newContext = new SamuraiContext())
            {
                newContext.Samurais.Update(samurai);
                newContext.SaveChanges();
            }
        }

        //Adding to Existing Not Tracked using FK
        private static void Simpler_AddQuoteToExistingSamuraiNotTracked(int samuraiId)
        {
            var quote = new Quote { Text = "Thanks for dinner!", SamuraiId = samuraiId };
            using var newContext = new SamuraiContext();
            newContext.Quotes.Add(quote);
            newContext.SaveChanges();
        }

        //Eager loading
        private static void EagerLoadSamuraiWithQuotes()
        {
            var samuraiWithQuotes = _context.Samurais.Include(s => s.Quotes).ToList();
        }

        //Filtered Eager loading 2020
        private static void FilteredEagerLoadSamuraiWithQuotes()
        {
            var filteredInclude = _context.Samurais
                .Include(s => s.Quotes.Where(q => q.Text.Contains("Thanks"))).ToList();
        }

        //QUERY PROJECTIONS
        private static void ProjectSomeProperties()
        {
            var someProperties = _context.Samurais.Select(s => new { s.Id, s.Name }).ToList(); //Anonymous type
        }

        //Dynamic type query projections
        private static void ProjectSomePropertiesOutsideMethod()
        {
            var someProperties = _context.Samurais.Select(s => new { s.Id, s.Name }).ToList();
            var idAndNames = _context.Samurais.Select(s => new IdAndName(s.Id, s.Name)).ToList(); //cast to dynamic type
        }
        public struct IdAndName
        {
            public IdAndName(int id, string name)
            {
                Id = id;
                Name = name;
            }

            public int Id;
            public string Name;
        }
        // *end Dynamic type


        //Add Quotes property into projection
        private static void ProjectSamuraisWithQuotes()
        {
            var somePropsWithQuotes = _context.Samurais
                .Select(s => new { s.Id, s.Name, s.Quotes })
                .ToList();
        }

        //Navigate into related object without bringing back complete types
        private static void ProjectSamuraisWithQuotesWithoutCompleteTypes()
        {
            var somePropsWithQuotes = _context.Samurais
                .Select(s => new { s.Id, s.Name, NumberOfQuotes=s.Quotes.Count })
                .ToList();
        }

        //Navigate into related object without bringing back complete types and filter
        private static void ProjectSamuraisWithQuotesWithoutCompleteTypesWithFilter()
        {
            var somePropsWithQuotes = _context.Samurais
                .Select(s => new { s.Id, s.Name, HappyQuotes = s.Quotes.Where(q => q.Text.Contains("happy") })
                .ToList();
        }

        //Projecting full entity objects while filtering the related objects that are also returned
        private static void ProjectSamuraisWithQuotesWithFullIncludeFeature()
        {
            var samuraisAndQuotes = _context.Samurais
                .Select(s => new { Samurai=s, HappyQuotes = s.Quotes.Where(q => q.Text.Contains("happy") })
                .ToList();
            var firstsamurai = samuraisAndQuotes[0].Samurai.Name += " The Happiest";
        }

        //LOADING RELATED DATA FOR OBJECTS ALREADY IN MEMORY
        //DbContext.Entry().Collection().Load();
        //DbContext.Entry().Reference().Load();

        //Explicit loading - only load from single object
        private static void ExplicitLoadQuotes()
        {
            //make sure there's a horse in the DB, then clear the context's change tracker
            _context.Set<Horse>().Add(new Horse { SamuraiId = 1, Name = "Mr. Ed" });
            _context.SaveChanges();
            _context.ChangeTracker.Clear();
            //----------------------
            var samurai = _context.Samurais.Find(1);
            _context.Entry(samurai).Collection(s => s.Quotes).Load();
            _context.Entry(samurai).Reference(s => s.Horse).Load();
        }
        //Filter explicit loading
        /* var happyQuotes = context.Entry(samurai)
         *      .Collection(b => b.Quotes)
         *      .Query()
         *      .Where(q => q.Quote.Contains("Happy") //Filter
         *      .ToList();
         */


        //Lazy loading: Enable with these requirements
        //Every navigation property in every entity must be virtual
        //Microsoft.EntityFramework.Proxies package
        //OnConfiguring optionsBuilder.UseLazyLoadingProxies()\
        //private static void FilteringWithRelatedData()
        //{
        //    var samurais = _context.Samurais.Find(2);
        //    var quoteCount = samurai.Quotes.Count(); //won't run without LL setup
        //}

        //Filter with Related Data
        private static void FilteringWithRelatedData()
        {
            var samurais = _context.Samurais
                                .Where(s => s.Quotes.Any(q => q.Text.Contains("happy"))) //subquote
                                .ToList();                                               //only returns single samurai
        }

        //MODIFYING RELATED DATA
        private static void ModifyingRelatedDataWhenTracked()
        {
            var samurai = _context.Samurais.Include(s => s.Quotes)
                                  .FirstOrDefault(s => s.Id == 2);
            samurai.Quotes[0].Text = "Did you hear that?";
            _context.SaveChanges();

        }

        private static void ModifyingRelatedDataWhenNotTracked()
        {
            var samurai = _context.Samurais.Include(s => s.Quotes)
                                  .FirstOrDefault(s => s.Id == 2);
            var quote = samurai.Quotes[0];
            quote.Text += "Did you hear that again?";

            using var newContext = new SamuraiContext();
            //STRANGE HAPPENED
            //If use Update, will update every item - not desired
            //*** newContext.Quotes.Update(quote); 
            //Usually solved using attach when adding new item but not update
            //Don't use Attach here
            //All will be tracked but marked unchanged
            //newContext.Quotes.Attach(quote);

            newContext.Entry(quote).State = EntityState.Modified;
            newContext.SaveChanges();

        }

        //MANY TO MANY RELATIONSHIPS
        private static void AddingNewSamuraiToAnExistingBattle()
        {
            var battle = _context.Battles.FirstOrDefault();
            battle.Samurais.Add(new Samurai { Name = "Takeda Shingen" });
            _context.SaveChanges();
        }

        private static void ReturnBattleWithSamurais()
        {
            var battle = _context.Battles.Include(b => b.Samurais).FirstOrDefault(); //returns object which EFCore worked out JOIN TABLE
        }

        private static void ReturnAllBattlesWithSamurais()
        {
            var battles = _context.Battles.Include(b => b.Samurais).ToList();
        }

        private static void AddAllSamuraisToAllBattles()
        {
            var allbattles = _context.Battles.ToList();
            //May cause SQL exception - already joined one of the Samurais for first battle,
            //     row for that in BattleSamurai table which cause conflict,
            // and for Violation of PRIMARY KEY when inserting duplicate row (also shows value)
            //var allSamurais = _context.Samurais.ToList();

            //hardcoded workaround
            var allSamurais = _context.Samurais.Where(s => s.Id!= 12).ToList();

            //Alternative solution:
            //var allbattles = _context.Battles.Include(b => b.Samurais).ToList();
            //var allSamurais = _context.Samurais.ToList();
            //MAY HURT performance if a lot of related data in eager load.

            foreach (var battle in allbattles)
            {
                battle.Samurais.AddRange(allSamurais);
            }
            _context.SaveChanges();
        }

        //REMOVING MANY TO MANY
        private static void RemoveSamuraiFromABattle()
        {
            var battleWithSamurai = _context.Battles
                .Include(b => b.Samurais.Where(s => s.Id == 12))
                .Single(s => s.BattleId == 1);
            var samurai = battleWithSamurai.Samurais[0];
            battleWithSamurai.Samurais.Remove(samurai);
            _context.SaveChanges();
        }

        //AVOID
        private static void WillNotRemoveSamuraiFromABattle()
        {
            var battle = _context.Battles.Find(1);
            var samurai = _context.Samurais.Find(12);
            battle.Samurais.Remove(samurai);
            _context.SaveChanges(); //the relationship is not being tracked.
        }

        //ADDING MANY TO MANY PAYLOAD TO EXISTING JOIN TABLE & DATA
        //Reinstate mapping
        /* protected override void OnModelCreating(ModelBuilder modelBuilder)
         * {
         *      modelBuilder.Entity<Samurai>()
         *          .HasMany(s => s.Battles)
         *          .WithMany(b => b.Samurais)
         *          .UsingEntity<BattleSamurai>
         *           (bs => bs.HasOne<Battle>().WithMany(),
         *            bs => bs.HasOne<Samurai>().WithMany())
         *          .Property(bs => bs.DateJoined)
         *          .HasDefaultValueSql("getdate()");
         */

        //Migrating to explicit mapping wont impact existing data UNLESS  : Table name in explicit class & table mapping DOES NOT match database name
        // DbSet name (doesnt exist)
        // Class Name (BattleSamurai)

        //WORKING WITH MANY TO MANY PAYLOAD DATA
        //Adding payload mapping to existing skip navigation doesnt break existing code

        //Edit payload data in many-to-many join >>> query using Set<>, modify and save
        private static void RemoveSamuraiFromABattleExplicit()
        {
            var b_s = _context.Set<BattleSamurai>()
                .SingleOrDefault(bs => bs.BattleId == 1 && bs.SamuraiId == 10);
            if (b_s != null)
            {
                _context.Remove(b_s); //_context.Set<BattleSamurai>().Remove works, too
                _context.SaveChanges();
            }
        }

        //PERSISTING DATA IN ONE-TO-ONE RELATIONSHIPS

        private static void AddNewSamuraiWithHorse()
        {
            var samurai = new Samurai { Name = "Jina Ujichika" };
            samurai.Horse = new Horse { Name = "Silver" };
            _context.Samurais.Add(samurai);
            _context.SaveChanges();
        }

        private static void AddNewHorseToSamuraiUsingId() //Samurai not in memory, samurai id present
        {
            var horse = new Horse { Name = "Scout", SamuraiId = 2 };
            _context.Add(horse);
            _context.SaveChanges();
        }

        private static void AddNewHorseToSamuraiObject() //Samurai object in memory
        {
            var samurai = _context.Samurais.Find(12);
            samurai.Horse = new Horse { Name = "Black Beauty" };
            _context.SaveChanges();
        }

        private static void AddNewHorseToDisconnectedSamuraiObject() //Samurai object disconnected
        {
            var samurai = _context.Samurais.AsNoTracking().FirstOrDefault(s => s.Id ==5);
            samurai.Horse = new Horse { Name = "Mr. Ed" };

            using var newContext = new SamuraiContext();
            newContext.Samurais.Attach(samurai);
            newContext.SaveChanges();
        }

        private static void ReplaceHorse() //Samurai and Horse already in memory
        {
            //The following will delete old Horse from database when replacing new Horse (constraints dont allow Horse object to exist without Samurai)
            //var samurai = _context.Samurais.Include(s => s.Horse)
            //                        .FirstOrDefault(s => s.Id == 5);
            //samurai.Horse = new Horse { Name = "Trigger" };
            var horse = _context.Set<Horse>().FirstOrDefault(horse => horse.Name == "Mr. Ed");
            horse.SamuraiId = 5; //owns Trigger!
            _context.SaveChanges();
        }

        //QUERYING ONE-TO-ONE RELATIONSHIPS
        private static void GetSamuraiWithHorse()
        {
            var samurais = _context.Samurais.Include(s => s.Horse).ToList();
        }

        private static void GetHorsesWithSamurai()//neither Horse DbSet nor Samurai property in Horse class
        {
            var horseonly = _context.Set<Horse>().Find(3); //Query using the DbContext Set method
            var horseWithSamurai = _context.Samurais.Include(s => s.Horse)
                                            .FirstOrDefault(s => s.Horse.Id == 3);// No Samurai navigation property

            var horseSamuraiPairs = _context.Samurais
                 .Where(s => s.Horse != null)
                 .Select(s => new { Horse = s.Horse, Samurai = s }) //Projection
                 .ToList();
        }
    }
}


