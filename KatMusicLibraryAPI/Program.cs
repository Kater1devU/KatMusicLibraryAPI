
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var songs = new List<Song>
{
    new(1, "Blinding Lights", "The Weeknd", "After Hours"),
    new(2, "Starboy", "The Weeknd", "Starboy"),
    new(3, "Die For You", "The Weeknd", "Starboy")
};

// GET: All songs
app.MapGet("/api/songs", () =>
    Results.Ok(songs));

// GET: Song by ID
app.MapGet("/api/songs/{id:int}", (int id) =>
{
    var song = songs.FirstOrDefault(s => s.Id == id);

    return song is null
        ? Results.NotFound()
        : Results.Ok(song);
});

// POST: Create song
app.MapPost("/api/songs", (SongRequest request) =>
{
    if (string.IsNullOrWhiteSpace(request.Title) ||
        string.IsNullOrWhiteSpace(request.Artist))
    {
        return Results.BadRequest("Title and Artist are required.");
    }

    int newId = songs.Count == 0 ? 1 : songs.Max(s => s.Id) + 1;

    var song = new Song(
        newId,
        request.Title,
        request.Artist,
        request.Album ?? ""
    );

    songs.Add(song);

    return Results.Created($"/api/songs/{song.Id}", song);
});

// PUT: Update song
app.MapPut("/api/songs/{id:int}", (int id, SongRequest request) =>
{
    var index = songs.FindIndex(s => s.Id == id);

    if (index == -1)
        return Results.NotFound();

    if (string.IsNullOrWhiteSpace(request.Title) ||
        string.IsNullOrWhiteSpace(request.Artist))
    {
        return Results.BadRequest("Title and Artist are required.");
    }

    var updatedSong = new Song(
        id,
        request.Title,
        request.Artist,
        request.Album ?? ""
    );

    songs[index] = updatedSong;

    return Results.Ok(updatedSong);
});

// DELETE: Delete song
app.MapDelete("/api/songs/{id:int}", (int id) =>
{
    var song = songs.FirstOrDefault(s => s.Id == id);

    if (song is null)
        return Results.NotFound();

    songs.Remove(song);

    return Results.NoContent();
});

app.Run();

record Song(int Id, string Title, string Artist, string Album);

record SongRequest(string Title, string Artist, string? Album);
