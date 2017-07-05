namespace TaskZilla.Persistence
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class TaskZillaContext : DbContext
    {
        public TaskZillaContext()
            : base("name=TaskZillaContext")
        {
        }

        public virtual DbSet<Priority> Priorities { get; set; }
        public virtual DbSet<Task> Tasks { get; set; }
        public virtual DbSet<AspNetUser> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }

        
    }
}
