using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
var builder = WebApplication.CreateBuilder(args);

// Register MongoDB Guid serializer
BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddInfrastructure();

builder.Services.AddHealthChecks();

var app = builder.Build();


// Adiciona o health check na rota "/health"
app.MapHealthChecks("/health");

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
