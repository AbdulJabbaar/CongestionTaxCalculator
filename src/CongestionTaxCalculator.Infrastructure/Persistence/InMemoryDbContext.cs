using CongestionTaxCalculator.Domain.City;
using CongestionTaxCalculator.Domain.City.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace CongestionTaxCalculator.Infrastructure.Persistence;

public class InMemoryDbContext : DbContext
{
    public DbSet<City> Cities { get; set; }
    public InMemoryDbContext(DbContextOptions<InMemoryDbContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<City>().HasKey(p => p.Id);
        modelBuilder.Entity<City>().Property(p => p.Id).ValueGeneratedNever().HasConversion(id => id.Value, value => CityId.Create(value));

        modelBuilder.Entity<City>().OwnsMany(x => x.CongestionTaxRulePerYears, congestionTaxRulePerYearsBuilder =>
        {
            congestionTaxRulePerYearsBuilder.WithOwner().HasForeignKey("CityId");
            congestionTaxRulePerYearsBuilder.HasKey("Id");
            congestionTaxRulePerYearsBuilder.Property(p => p.Id).ValueGeneratedNever().HasConversion(id => id.Value, value => TaxRulesId.Create(value));

            congestionTaxRulePerYearsBuilder.OwnsMany(y => y.CongestionTaxAmounts, congestionTaxAmountsBuilder =>
            {
                congestionTaxAmountsBuilder.WithOwner().HasForeignKey("Id");
                congestionTaxAmountsBuilder.HasKey(nameof(CongestionTaxAmount.StartTime), nameof(CongestionTaxAmount.EndTime), "Id");
            });

            congestionTaxRulePerYearsBuilder.OwnsMany(y => y.TollFreeVehicle, tollFreeVehicleBuilder =>
            {
                tollFreeVehicleBuilder.WithOwner().HasForeignKey("Id");
                tollFreeVehicleBuilder.HasKey(nameof(Vehicle.VehicleType), "Id");
            });
        });
        base.OnModelCreating(modelBuilder);
    }
}
