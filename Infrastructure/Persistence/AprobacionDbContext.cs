using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence
{
    public class AprobacionDbContext(DbContextOptions<AprobacionDbContext> options) : DbContext(options)
    {
        public DbSet<Area> Areas { get; set; }
        public DbSet<ProjectType> ProjectTypes { get; set; }
        public DbSet<ApprovalStatus> ApprovalStatuses { get; set; }
        public DbSet<ApproverRole> ApproverRoles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ApprovalRule> ApprovalRules { get; set; }
        public DbSet<ProjectProposal> ProjectProposals { get; set; }
        public DbSet<ProjectApprovalStep> ProjectApprovalSteps { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Area>(entity =>
            {
                entity.ToTable("Area");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Name).HasMaxLength(25).IsRequired();
            });

            modelBuilder.Entity<ProjectType>(entity =>
            {
                entity.ToTable("ProjectType");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Name).HasMaxLength(25).IsRequired();
            });

            modelBuilder.Entity<ApprovalStatus>(entity =>
            {
                entity.ToTable("ApprovalStatus");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Name).HasMaxLength(25).IsRequired();
            });

            modelBuilder.Entity<ApproverRole>(entity =>
            {
                entity.ToTable("ApproverRole");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Name).HasMaxLength(25).IsRequired();
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Name).HasMaxLength(25).IsRequired();
                entity.Property(e => e.Email).HasMaxLength(100).IsRequired();

                entity.HasOne(e => e.ApproverRole)
                      .WithMany(r => r.Users)
                      .HasForeignKey(e => e.Role)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<ApprovalRule>(entity =>
            {
                entity.ToTable("ApprovalRule");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.MinAmount).IsRequired();
                entity.Property(e => e.MaxAmount).IsRequired();
                entity.Property(e => e.StepOrder).IsRequired();

                entity.HasOne(e => e.AreaNavigation)
                      .WithMany(a => a.ApprovalRules)
                      .HasForeignKey(e => e.Area)
                      .IsRequired(false)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.TypeNavigation)
                      .WithMany(t => t.ApprovalRules)
                      .HasForeignKey(e => e.Type)
                      .IsRequired(false)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.ApproverRole)
                      .WithMany(r => r.ApprovalRules)
                      .HasForeignKey(e => e.ApproverRoleId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<ProjectProposal>(entity =>
            {
                entity.ToTable("ProjectProposal");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.Title).HasMaxLength(255).IsRequired();
                entity.Property(e => e.Description).IsRequired();
                entity.Property(e => e.EstimatedAmount).IsRequired();
                entity.Property(e => e.EstimatedDuration).IsRequired();
                entity.Property(e => e.CreateAt).IsRequired();

                entity.HasOne(e => e.AreaNavigation)
                      .WithMany(a => a.ProjectProposals)
                      .HasForeignKey(e => e.Area)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.TypeNavigation)
                      .WithMany(t => t.ProjectProposals)
                      .HasForeignKey(e => e.Type)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.StatusNavigation)
                      .WithMany(s => s.ProjectProposals)
                      .HasForeignKey(e => e.Status)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.CreateByNavigation)
                      .WithMany(u => u.ProjectProposals)
                      .HasForeignKey(e => e.CreateBy)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<ProjectApprovalStep>(entity =>
            {
                entity.ToTable("ProjectApprovalStep");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.StepOrder).IsRequired();
                entity.Property(e => e.DecisionDate).IsRequired(false);
                entity.Property(e => e.Observations).HasMaxLength(int.MaxValue).IsRequired(false);

                entity.HasOne(e => e.ProjectProposal)
                      .WithMany(p => p.ApprovalSteps)
                      .HasForeignKey(e => e.ProjectProposalId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.ApproverUser)
                      .WithMany(u => u.ProjectApprovalSteps)
                      .HasForeignKey(e => e.ApproverUserId)
                      .IsRequired(false)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.ApproverRole)
                      .WithMany(r => r.ProjectApprovalSteps)
                      .HasForeignKey(e => e.ApproverRoleId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.StatusNavigation)
                      .WithMany(s => s.ProjectApprovalSteps)
                      .HasForeignKey(e => e.Status)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Carga de datos
            modelBuilder.Entity<Area>().HasData(
                new Area { Id = 1, Name = "Finanzas" },
                new Area { Id = 2, Name = "Tecnología" },
                new Area { Id = 3, Name = "Recursos Humanos" },
                new Area { Id = 4, Name = "Operaciones" }
            );

            modelBuilder.Entity<ProjectType>().HasData(
                new ProjectType { Id = 1, Name = "Mejora de Procesos" },
                new ProjectType { Id = 2, Name = "Innovación y Desarrollo" },
                new ProjectType { Id = 3, Name = "Infraestructura" },
                new ProjectType { Id = 4, Name = "Capacitación Interna" }
            );

            modelBuilder.Entity<ApprovalStatus>().HasData(
                new ApprovalStatus { Id = 1, Name = "Pending" },
                new ApprovalStatus { Id = 2, Name = "Approved" },
                new ApprovalStatus { Id = 3, Name = "Rejected" },
                new ApprovalStatus { Id = 4, Name = "Observed" }
            );

            modelBuilder.Entity<ApproverRole>().HasData(
                new ApproverRole { Id = 1, Name = "Líder de Área" },
                new ApproverRole { Id = 2, Name = "Gerente" },
                new ApproverRole { Id = 3, Name = "Director" },
                new ApproverRole { Id = 4, Name = "Comité Técnico" }
            );

            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Name = "José Ferreyra", Email = "jferreyra@unaj.com", Role = 2 },
                new User { Id = 2, Name = "Ana Lucero", Email = "alucero@unaj.com", Role = 1 },
                new User { Id = 3, Name = "Gonzalo Molinas", Email = "gmolinas@unaj.com", Role = 2 },
                new User { Id = 4, Name = "Lucas Olivera", Email = "lolivera@unaj.com", Role = 3 },
                new User { Id = 5, Name = "Danilo Fagundez", Email = "dfagundez@unaj.com", Role = 4 },
                new User { Id = 6, Name = "Gabriel Galli", Email = "ggalli@unaj.com", Role = 4 }
            );

            modelBuilder.Entity<ApprovalRule>().HasData(
                new ApprovalRule { Id = 1, MinAmount = 0, MaxAmount = 100000, Area = null, Type = null, StepOrder = 1, ApproverRoleId = 1 },
                new ApprovalRule { Id = 2, MinAmount = 5000, MaxAmount = 20000, Area = null, Type = null, StepOrder = 2, ApproverRoleId = 2 },
                new ApprovalRule { Id = 3, MinAmount = 0, MaxAmount = 20000, Area = 2, Type = 2, StepOrder = 1, ApproverRoleId = 2 },
                new ApprovalRule { Id = 4, MinAmount = 20000, MaxAmount = 0, Area = null, Type = null, StepOrder = 3, ApproverRoleId = 3 },
                new ApprovalRule { Id = 5, MinAmount = 5000, MaxAmount = 0, Area = 1, Type = 1, StepOrder = 2, ApproverRoleId = 2 },
                new ApprovalRule { Id = 6, MinAmount = 0, MaxAmount = 10000, Area = null, Type = 2, StepOrder = 1, ApproverRoleId = 1 },
                new ApprovalRule { Id = 7, MinAmount = 0, MaxAmount = 10000, Area = 2, Type = 1, StepOrder = 1, ApproverRoleId = 4 },
                new ApprovalRule { Id = 8, MinAmount = 10000, MaxAmount = 30000, Area = 2, Type = null, StepOrder = 2, ApproverRoleId = 2 },
                new ApprovalRule { Id = 9, MinAmount = 30000, MaxAmount = 0, Area = 3, Type = null, StepOrder = 2, ApproverRoleId = 3 },
                new ApprovalRule { Id = 10, MinAmount = 0, MaxAmount = 50000, Area = null, Type = 4, StepOrder = 1, ApproverRoleId = 4 }
                );
        }
    }
}
