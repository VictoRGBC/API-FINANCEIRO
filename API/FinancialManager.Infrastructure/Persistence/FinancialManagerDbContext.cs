public class FinancialManagerDbContext : DbContext
{
    public FinancialManagerDbContext(DbContextOptions<FinancialManagerDbContext> options)
        : base(options) { }

    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<Transaction> Transactions => Set<Transaction>();
    public DbSet<Category> Categories => Set<Category>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Aplica todas as configurações que criaremos abaixo
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FinancialManagerDbContext).Assembly);
    }
}