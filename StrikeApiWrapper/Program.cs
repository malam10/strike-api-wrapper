using StrikeApiWrapper.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

// Add HttpClient and register StrikeApiService
builder.Services.AddHttpClient<IStrikeApiService, StrikeApiService>();

// Configure environment and API key through configuration
builder.Services.AddSingleton(provider =>
    new StrikeApiService(
        provider.GetRequiredService<HttpClient>(),
        builder.Configuration["Strike:ApiKey"],
        builder.Configuration["Strike:Environment"]
    )
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();