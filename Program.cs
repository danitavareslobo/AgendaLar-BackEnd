using AgendaLarAPI.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureApi(builder.Configuration);
builder.Services.ConfigureIdentity(builder.Configuration);
builder.Services.ConfigureSwagger();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseApiSwagger();
app.UseApiConfiguration();

app.Run();
