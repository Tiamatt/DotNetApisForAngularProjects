using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DotNetApisForAngularProjects.HomeCuisineDbModels
{
    public partial class HomeCuisineDbContext : DbContext
    {
        public HomeCuisineDbContext()
        {
        }

        public HomeCuisineDbContext(DbContextOptions<HomeCuisineDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<Error> Error { get; set; }
        public virtual DbSet<Ingredient> Ingredient { get; set; }
        public virtual DbSet<Measure> Measure { get; set; }
        public virtual DbSet<Recipe> Recipe { get; set; }
        public virtual DbSet<RecipeCategory> RecipeCategory { get; set; }
        public virtual DbSet<RecipeDirection> RecipeDirection { get; set; }
        public virtual DbSet<RecipeFrontImage> RecipeFrontImage { get; set; }
        public virtual DbSet<RecipeIngredientMeasure> RecipeIngredientMeasure { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .HasName("UC_Category_name")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Active)
                    .IsRequired()
                    .HasColumnName("active")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(128);
            });

            modelBuilder.Entity<Error>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("description");

                entity.Property(e => e.Timestamp)
                    .HasColumnName("timestamp")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<Ingredient>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .HasName("UC_Ingredient_name")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Active)
                    .IsRequired()
                    .HasColumnName("active")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(128);
            });

            modelBuilder.Entity<Measure>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .HasName("UC_Measure_name")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Active)
                    .IsRequired()
                    .HasColumnName("active")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(128);
            });

            modelBuilder.Entity<Recipe>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .HasName("UC_Recipe_name")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Active)
                    .IsRequired()
                    .HasColumnName("active")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(128);

                entity.Property(e => e.PreparationTime).HasColumnName("preparationTime");

                entity.Property(e => e.Servings).HasColumnName("servings");
            });

            modelBuilder.Entity<RecipeCategory>(entity =>
            {
                entity.HasKey(e => e.Guid);

                entity.Property(e => e.Guid)
                    .HasColumnName("guid")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Category).HasColumnName("category");

                entity.Property(e => e.Recipe).HasColumnName("recipe");

                entity.HasOne(d => d.CategoryNavigation)
                    .WithMany(p => p.RecipeCategory)
                    .HasForeignKey(d => d.Category)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RecipeCategory_category");

                entity.HasOne(d => d.RecipeNavigation)
                    .WithMany(p => p.RecipeCategory)
                    .HasForeignKey(d => d.Recipe)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RecipeCategory_recipe");
            });

            modelBuilder.Entity<RecipeDirection>(entity =>
            {
                entity.HasKey(e => e.Guid);

                entity.HasIndex(e => new { e.Recipe, e.Sort })
                    .HasName("UC_RecipeSteps_recipe_sort")
                    .IsUnique();

                entity.Property(e => e.Guid)
                    .HasColumnName("guid")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Direction)
                    .IsRequired()
                    .HasColumnName("direction")
                    .HasMaxLength(3000);

                entity.Property(e => e.Recipe).HasColumnName("recipe");

                entity.Property(e => e.Sort).HasColumnName("sort");

                entity.HasOne(d => d.RecipeNavigation)
                    .WithMany(p => p.RecipeDirection)
                    .HasForeignKey(d => d.Recipe)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RecipeSteps_recipe");
            });

            modelBuilder.Entity<RecipeFrontImage>(entity =>
            {
                entity.HasKey(e => e.Guid);

                entity.Property(e => e.Guid)
                    .HasColumnName("guid")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.FrontImage)
                    .IsRequired()
                    .HasColumnName("frontImage");

                entity.Property(e => e.Recipe).HasColumnName("recipe");

                entity.HasOne(d => d.RecipeNavigation)
                    .WithMany(p => p.RecipeFrontImage)
                    .HasForeignKey(d => d.Recipe)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RecipeFrontImage_recipe");
            });

            modelBuilder.Entity<RecipeIngredientMeasure>(entity =>
            {
                entity.HasKey(e => e.Guid);

                entity.Property(e => e.Guid)
                    .HasColumnName("guid")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Amount)
                    .IsRequired()
                    .HasColumnName("amount")
                    .HasMaxLength(30);

                entity.Property(e => e.Ingredient).HasColumnName("ingredient");

                entity.Property(e => e.Measure).HasColumnName("measure");

                entity.Property(e => e.Recipe).HasColumnName("recipe");

                entity.HasOne(d => d.IngredientNavigation)
                    .WithMany(p => p.RecipeIngredientMeasure)
                    .HasForeignKey(d => d.Ingredient)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RecipeIngredientMeasure_ingredient");

                entity.HasOne(d => d.MeasureNavigation)
                    .WithMany(p => p.RecipeIngredientMeasure)
                    .HasForeignKey(d => d.Measure)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RecipeIngredientMeasure_measure");

                entity.HasOne(d => d.RecipeNavigation)
                    .WithMany(p => p.RecipeIngredientMeasure)
                    .HasForeignKey(d => d.Recipe)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RecipeIngredientMeasure_recipe");
            });
        }
    }
}
