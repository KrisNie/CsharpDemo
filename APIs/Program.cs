using MinimalAPIs.Extensions;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.RegisterServices(builder.Configuration);

var application = builder.Build();

// Configure the HTTP request pipeline.
if (application.Environment.IsDevelopment())
{
    application.UseSwagger();
    application.UseSwaggerUI();
}

// Map the Endpoints
application.EndpointsMap();
application.UseHttpsRedirection();
application.UseAuthorization();
application.MapControllers();
application.Run();