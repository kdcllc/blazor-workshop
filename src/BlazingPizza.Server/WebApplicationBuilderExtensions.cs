using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;

namespace BlazingPizza.Server
{
    public static class WebApplicationBuilderExtensions
    {
        public static WebApplicationBuilder AddServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();

            builder.Services.AddDbContext<PizzaStoreContext>(options =>
                options.UseSqlite("Data Source=pizza.db"));

            builder.Services.AddDefaultIdentity<PizzaStoreUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<PizzaStoreContext>();

            builder.Services.AddIdentityServer()
                .AddApiAuthorization<PizzaStoreUser, PizzaStoreContext>();

            builder.Services.AddAuthentication()
                .AddIdentityServerJwt();

            return builder;
        }
    }
}
