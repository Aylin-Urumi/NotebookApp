using System.Text.Json;
 
namespace NotebookApp;
 
/// <summary>
/// Holds a collection of notes and handles File I/O + JSON serialization.
/// </summary>
public class Notebook
{
    // ── Fields ────────────────────────────────────────────────────────────
    private List<Note> _notes = new();
    private int        _nextId = 1;
    private readonly string _filePath;
 
    private static readonly JsonSerializerOptions JsonOpts = new()
    {
        WriteIndented = true
    };
 
    // ── Constructor ───────────────────────────────────────────────────────
    public Notebook(string filePath = "notes.json")
    {
        _filePath = filePath;
        Load();           // Load existing notes from disk on startup
    }
 
    // ── Public API ────────────────────────────────────────────────────────
 
    /// <summary>Returns a read-only view of all notes.</summary>
    public IReadOnlyList<Note> GetAll() => _notes.AsReadOnly();
 
    /// <summary>Returns a single note by ID, or null if not found.</summary>
    public Note? GetById(int id) => _notes.FirstOrDefault(n => n.Id == id);
 
    /// <summary>Adds a new note and persists to disk.</summary>
    public Note Add(string title, string content)
    {
        var note = new Note(_nextId++, title, content);
        _notes.Add(note);
        Save();
        return note;
    }
 
    /// <summary>Updates an existing note and persists to disk.</summary>
    public bool Update(int id, string? newTitle, string? newContent)
    {
        var note = GetById(id);
        if (note is null) return false;
 
        if (!string.IsNullOrWhiteSpace(newTitle))   note.Title   = newTitle;
        if (newContent is not null)                  note.Content = newContent;
        note.UpdatedAt = DateTime.Now;
 
        Save();
        return true;
    }
 
    /// <summary>Deletes a note by ID and persists to disk.</summary>
    public bool Delete(int id)
    {
        var note = GetById(id);
        if (note is null) return false;
 
        _notes.Remove(note);
        Save();
        return true;
    }
 
    /// <summary>Searches notes by keyword (title or content).</summary>
    public IEnumerable<Note> Search(string keyword) =>
        _notes.Where(n =>
            n.Title.Contains(keyword,   StringComparison.OrdinalIgnoreCase) ||
            n.Content.Contains(keyword, StringComparison.OrdinalIgnoreCase));
 
    // ── Serialization / File I/O ──────────────────────────────────────────
 
    /// <summary>
    /// Serializes the note list to JSON and writes it to disk.
    /// Demonstrates File I/O (File.WriteAllText) + JSON serialization.
    /// </summary>
    private void Save()
    {
        // Wrap notes + metadata into a simple container object
        var data = new NotebookData { NextId = _nextId, Notes = _notes };
        string json = JsonSerializer.Serialize(data, JsonOpts);
        File.WriteAllText(_filePath, json);
    }
 
    /// <summary>
    /// Reads the JSON file from disk and deserializes it into the note list.
    /// Demonstrates File I/O (File.ReadAllText) + JSON deserialization.
    /// </summary>
    private void Load()
    {
        if (!File.Exists(_filePath)) return;
 
        try
        {
            string json = File.ReadAllText(_filePath);
            var data = JsonSerializer.Deserialize<NotebookData>(json, JsonOpts);
            if (data is not null)
            {
                _notes  = data.Notes ?? new List<Note>();
                _nextId = data.NextId;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Warning] Could not load notes: {ex.Message}");
        }
    }
 
    // ── Inner DTO used only for serialization ─────────────────────────────
    private class NotebookData
    {
        public int        NextId { get; set; } = 1;
        public List<Note>? Notes  { get; set; }
    }
}