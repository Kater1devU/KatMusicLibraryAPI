using KatMusicLibraryAPI.Models;

namespace KatMusicLibraryAPI.Services;

public class SongService
{
    private readonly List<Song> _songs = new()
    {
        new(1, "Blinding Lights", "The Weeknd", "After Hours"),
        new(2, "Starboy", "The Weeknd", "Starboy"),
        new(3, "Die For You", "The Weeknd", "Starboy")
    };

    private int _nextId = 4;

    public List<Song> GetAll() => _songs;

    public Song? GetById(int id) =>
        _songs.FirstOrDefault(s => s.Id == id);

    public Song Create(string title, string artist, string album)
    {
        var song = new Song(_nextId++, title, artist, album);
        _songs.Add(song);
        return song;
    }

    public Song? Update(int id, string title, string artist, string album)
    {
        var index = _songs.FindIndex(s => s.Id == id);

        if (index == -1)
            return null;

        var song = new Song(id, title, artist, album);
        _songs[index] = song;

        return song;
    }

    public bool Delete(int id)
    {
        var song = GetById(id);

        if (song is null)
            return false;

        _songs.Remove(song);
        return true;
    }
}
