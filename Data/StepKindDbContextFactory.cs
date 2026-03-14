using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace backend_stepkind.Data;

public class StepKindDbContextFactory : IDesignTimeDbContextFactory<StepKindDbContext>
{
    public StepKindDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<StepKindDbContext>();
        optionsBuilder.UseNpgsql(
            "Host=localhost;Port=5432;Database=stepkind;Username=postgres;Password=postgres",
            npgsqlOptions => npgsqlOptions.UseVector());

        return new StepKindDbContext(optionsBuilder.Options);
    }
}
