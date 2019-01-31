using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Piri.Framework.Scheduler.Quartz.Model
{
    public partial class QuartzDataContext : DbContext
    {
        public QuartzDataContext()
        {
        }

        public QuartzDataContext(DbContextOptions<QuartzDataContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Job> Job { get; set; }
        public virtual DbSet<JobData> JobData { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=10.0.130.5;Database=DB_Scheduler;User Id=pirischeduler;Password=pirischeduler5*;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.1-servicing-10028");

            modelBuilder.Entity<Job>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasColumnType("date");

                entity.Property(e => e.Guid).HasColumnName("GUID");

                entity.Property(e => e.LastEndTime).HasColumnType("date");

                entity.Property(e => e.LastRunTime).HasColumnType("date");

                entity.Property(e => e.UpdatedDate).HasColumnType("date");
            });

            modelBuilder.Entity<JobData>(entity =>
            {
                entity.Property(e => e.Method).HasMaxLength(10);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.TimerRegex)
                    .IsRequired()
                    .HasMaxLength(25);

                entity.Property(e => e.Url).IsRequired();

                entity.HasOne(d => d.Job)
                    .WithMany(p => p.JobData)
                    .HasForeignKey(d => d.JobId)
                    .HasConstraintName("FK_JobData_Job");
            });
        }
    }
}
