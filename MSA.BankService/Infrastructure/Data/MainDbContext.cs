using Microsoft.EntityFrameworkCore;
using MSA.BankService.Entities;
using MSA.Common.PostgresMassTransit.PostgresDB;

namespace MSA.BankService.Infrastructure.Data 
{
    public class MainDbContext : AppDbContextBase
    {
        private readonly string _uuidGenerator = "uuid-ossp";
        private readonly string _uuidAlgorithm = "uuid_generate_v4()";
        private readonly IConfiguration configuration;
        public MainDbContext(
            IConfiguration configuration,
            DbContextOptions<MainDbContext> options): base(configuration, options)
        {
            this.configuration = configuration;
        }

        public DbSet<Account> Accounts { get; set;} = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasPostgresExtension(_uuidGenerator);

            //Accounts
            modelBuilder.Entity<Account>().ToTable("accounts");
            modelBuilder.Entity<Account>().HasKey(x => x.Id);
            modelBuilder.Entity<Account>().Property(x => x.Id)
                .HasColumnType("uuid");
                //.HasDefaultValueSql(_uuidAlgorithm);
        }
    }
}