using Microsoft.Extensions.DependencyInjection;
using SignalRWebHub;
using SignalRWebHub.DataService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();
builder.Services.AddSingleton<SharedDb>();

builder.Services.AddSingleton<List<Stakeholders>>(_ =>
{
    var stakeholder = new Stakeholders();
    var list = stakeholder.LoadNotifiersFromJson();

    foreach (var item in list)
    {
        Console.WriteLine(item.Room);
        foreach (var participant in item.Participants)
        {
            Console.WriteLine(participant.Email);
        }
    }
    return list;
}
);


//builder.Services.AddHostedService<ContinuosSignal>();
builder.Services.AddSignalR();
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
    {
        builder.AllowAnyMethod()
            .AllowAnyHeader()
            .WithOrigins("http://localhost:5173")
            .AllowCredentials();
    });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapHub<ChatHub>("/chatHub");
app.UseCors("CorsPolicy");
app.Run();
