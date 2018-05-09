using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace SaveData.Models
{
    public class SentenceContext : DbContext
    {
        public DbSet<Sentence> Sentences { get; set; }

    }
    public class SentenceDbInitializer : DropCreateDatabaseAlways<SentenceContext>
    {
        //protected override void Seed(SentenceContext db)
        //{
        //    db.Sentences.Add(new Sentence { SentenceData = "Null" });
        //    base.Seed(db);
        //}
    }
}