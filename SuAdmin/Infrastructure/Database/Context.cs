using Microsoft.EntityFrameworkCore;

namespace SuAdmin.Infrastructure.Database;

public class Context(DbContextOptions<Context> contextOptions) : DbContext(contextOptions)
{
}