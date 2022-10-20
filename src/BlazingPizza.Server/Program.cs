using BlazingPizza;
using BlazingPizza.Server;

using Microsoft.EntityFrameworkCore;

using System.ComponentModel.DataAnnotations;

var builder = WebApplication.CreateBuilder(args);

builder.AddServices();

await using var app = builder.Build();

app.EnsureDb();

app.Configure();

app.MapGet("/specials", async (PizzaStoreContext db) =>
{
    return (await db.Specials.ToListAsync()).OrderByDescending(s => s.BasePrice).ToList();
});

app.MapGet("/toppings", async (PizzaStoreContext db) =>
{
    return await db.Toppings.OrderBy(t => t.Name).ToListAsync();
});

app.MapPost("order", (Address address) =>
{
    return !MinimalValidation.TryValidate(address, out var errors)
           ? Results.BadRequest(errors)
           : Results.Ok(address);
});

await app.RunAsync();