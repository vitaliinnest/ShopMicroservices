var builder = WebApplication.CreateBuilder(args);

ConfigureServices(builder);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();

static void ConfigureServices(WebApplicationBuilder builder)
{
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddStackExchangeRedisCache(opts =>
    {
        opts.Configuration = builder.Configuration.GetValue<string>("CacheSettings:ConnectionString");
    });
}
