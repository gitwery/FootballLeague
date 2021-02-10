using FootballLeagueAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FootballLeagueAPI.DBContexts
{
    public class SqlDbContext : DbContext
    {
        public DbSet<Team> Teams { get; set; }
        public DbSet<Match> Matches { get; set; }

        public SqlDbContext(DbContextOptions<SqlDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Team>().ToTable("Teams");
            modelBuilder.Entity<Match>().ToTable("Matches");

            modelBuilder.Entity<Team>().HasKey(team => team.Id).HasName("PK_Teams");
            modelBuilder.Entity<Match>().HasKey(match => match.Id).HasName("PK_Matches");

            modelBuilder.Entity<Team>().HasIndex(team => team.Name).IsUnique();

            modelBuilder.Entity<Team>().Property(team => team.Id).HasColumnType("int").UseMySqlIdentityColumn().IsRequired();
            modelBuilder.Entity<Team>().Property(team => team.Name).HasColumnType("nvarchar(50)").IsRequired();
            modelBuilder.Entity<Team>().Property(team => team.Position).HasColumnType("int").IsRequired().HasDefaultValue(0);
            modelBuilder.Entity<Team>().Property(team => team.Points).HasColumnType("int").IsRequired().HasDefaultValue(0);
            modelBuilder.Entity<Team>().Property(team => team.Wins).HasColumnType("int").IsRequired().HasDefaultValue(0);
            modelBuilder.Entity<Team>().Property(team => team.Draws).HasColumnType("int").IsRequired().HasDefaultValue(0);
            modelBuilder.Entity<Team>().Property(team => team.Loses).HasColumnType("int").IsRequired().HasDefaultValue(0);
            modelBuilder.Entity<Team>().Property(team => team.GoalsScored).HasColumnType("int").IsRequired().HasDefaultValue(0);
            modelBuilder.Entity<Team>().Property(team => team.GoalsRecieved).HasColumnType("int").IsRequired().HasDefaultValue(0);
            modelBuilder.Entity<Team>().Property(team => team.GoalDifference).HasColumnType("int").IsRequired().HasDefaultValue(0);


            modelBuilder.Entity<Match>().Property(match => match.Id).HasColumnType("int").UseMySqlIdentityColumn().IsRequired();
            modelBuilder.Entity<Match>().Property(match => match.HomeTeam).HasColumnType("nvarchar(50)").IsRequired();
            modelBuilder.Entity<Match>().Property(match => match.AwayTeam).HasColumnType("nvarchar(50)").IsRequired();
            modelBuilder.Entity<Match>().Property(match => match.HomeGoals).HasColumnType("int").IsRequired();
            modelBuilder.Entity<Match>().Property(match => match.AwayGoals).HasColumnType("int").IsRequired();
            modelBuilder.Entity<Match>().Property(match => match.MatchResult).HasColumnType("int").IsRequired();

        }
    }
}
