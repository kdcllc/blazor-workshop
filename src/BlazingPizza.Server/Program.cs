using BlazingPizza.Server;

using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServices();

await using var app = builder.Build();

// Initialize the database
var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
using (var scope = scopeFactory.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<PizzaStoreContext>();
    if (db.Database.EnsureCreated())
    {
        SeedData.Initialize(db);
    }
}

app.Configure();

app.MapGet("/specials", async (PizzaStoreContext db) =>
{
    return (await db.Specials.ToListAsync()).OrderByDescending(s => s.BasePrice).ToList();
});

app.MapGet("/toppings", async (PizzaStoreContext db) =>
{
    return await db.Toppings.OrderBy(t => t.Name).ToListAsync();
});

await app.RunAsync();