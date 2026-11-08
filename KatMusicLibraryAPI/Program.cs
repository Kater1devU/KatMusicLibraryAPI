using KatMusicLibraryAPI.Models;
using KatMusicLibraryAPI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddSingleton<SongService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/api/songs", (SongService service) =>
    Results.Ok(service.GetAll()));

app.MapGet("/api/songs/{id:int}", (int id, SongService service) =>
{
    var song = service.GetById(id);
    return song is null ? Results.NotFound() : Results.Ok(song);
});

app.MapPost("/api/songs", (SongRequest request, SongService service) =>
{
    if (string.IsNullOrWhiteSpace(request.Title) ||
        string.IsNullOrWhiteSpace(request.Artist))
        return Results.BadRequest();

    var song = service.Create(
        request.Title, request.Artist, request.Album ?? "");

    return Results.Created($"/api/songs/{song.Id}", song);
});

app.MapPut("/api/songs/{id:int}", (int id, SongRequest request, SongService service) =>
{
    if (string.IsNullOrWhiteSpace(request.Title) ||
        string.IsNullOrWhiteSpace(request.Artist))
        return Results.BadRequest();

    var song = service.Update(
        id, request.Title, request.Artist, request.Album ?? "");

    return song is null ? Results.NotFound() : Results.Ok(song);
});

app.MapDelete("/api/songs/{id:int}", (int id, SongService service) =>
    service.Delete(id) ? Results.NoContent() : Results.NotFound());

app.Run();

record SongRequest(string Title, string Artist, string? Album);
