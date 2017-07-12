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

        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<Priority> Priorities { get; set; }
        public virtual DbSet<Task> Tasks { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.Tasks)
                .WithRequired(e => e.AspNetUser)
                .HasForeignKey(e => e.AssignedToUserId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Priority>()
                .HasMany(e => e.Tasks)
                .WithRequired(e => e.Priority)
                .WillCascadeOnDelete(false);
        }
    }
}
