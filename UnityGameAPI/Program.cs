
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

// 1. Database setup
string connectionString = "mongodb://localhost:27017";

var mongoClient = new MongoClient(connectionString);
var mongoDatabase = mongoClient.GetDatabase("unity_game");
var leaderboardCollection = mongoDatabase.GetCollection<ScoreEntry>("leaderboard");

// 2. Default initial data
var defaultLeaderboard = new List<ScoreEntry>
{
    new(DateTime.UtcNow, 2500),
    new(DateTime.UtcNow, 1800),
    new(DateTime.UtcNow, 1500)
};

// Seed the collection if it's currently empty
if (!await leaderboardCollection.Find(_ => true).AnyAsync())
{
    await leaderboardCollection.InsertManyAsync(defaultLeaderboard);
}

// 3. GET Endpoint: Retrieve top 5 scores
app.MapGet("/api/leaderboard", async () =>
{
    var sort = Builders<ScoreEntry>.Sort
        .Descending(s => s.Score);

    var players = await leaderboardCollection
        .Find(_ => true)
        .Sort(sort)
        .Limit(5)
        .ToListAsync();

    return Results.Ok(new { players });
});

// 4. POST Endpoint: Insert a new score
app.MapPost("/api/scores", async (ScoreEntry score) =>
{
    if (score.Score < 0)
    {
        return Results.BadRequest(new { error = "Invalid score data" });
    }

    await leaderboardCollection.InsertOneAsync(score);

    return Results.Json(
        new { message = "Score saved" },
        statusCode: 201
    );
});

app.Run("http://localhost:9999");

// 5. Score model
[BsonIgnoreExtraElements]
public record ScoreEntry(DateTime AchievedAt, int Score);
