
using KatMusicLibraryAPI.Services;

namespace KatMusicLibraryAPI.Tests;

public class SongServiceTests
{
    [Fact]
    public void GetAll_ReturnsThreeDefaultSongs()
    {
        var service = new SongService();

        var songs = service.GetAll();

        Assert.Equal(3, songs.Count);
    }

    [Fact]
    public void GetById_ReturnsCorrectSong()
    {
        var service = new SongService();

        var song = service.GetById(1);

        Assert.NotNull(song);
        Assert.Equal("Blinding Lights", song.Title);
    }

    [Fact]
    public void GetById_ReturnsNullWhenSongNotFound()
    {
        var service = new SongService();

        var song = service.GetById(999);

        Assert.Null(song);
    }

    [Fact]
    public void Create_AddsNewSong()
    {
        var service = new SongService();

        var song = service.Create(
            "Save Your Tears",
            "The Weeknd",
            "After Hours");

        Assert.Equal(4, song.Id);
        Assert.Equal(4, service.GetAll().Count);
    }

    [Fact]
    public void Update_ChangesSongInformation()
    {
        var service = new SongService();

        var updated = service.Update(
            1,
            "Updated Song",
            "New Artist",
            "New Album");

        Assert.NotNull(updated);
        Assert.Equal("Updated Song", service.GetById(1)!.Title);
    }

    [Fact]
    public void Delete_RemovesSong()
    {
        var service = new SongService();

        var result = service.Delete(1);

        Assert.True(result);
        Assert.Null(service.GetById(1));
    }
}
