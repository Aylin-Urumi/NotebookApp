namespace NotebookApp;
 
/// <summary>
/// Represents a single note in the notebook.
/// Demonstrates encapsulation with private backing fields and public properties.
/// </summary>
public class Note
{
    // Private backing fields
    private string _title;
    private string _content;
 
    // Auto-assigned unique ID
    public int Id { get; set; }
 
    // Public properties with validation
    public string Title
    {
        get => _title;
        set => _title = string.IsNullOrWhiteSpace(value)
            ? throw new ArgumentException("Title cannot be empty.")
            : value.Trim();
    }
 
    public string Content
    {
        get => _content;
        set => _content = value ?? string.Empty;
    }
 
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
 
    // Parameterless constructor required for JSON deserialization
    public Note()
    {
        _title   = string.Empty;
        _content = string.Empty;
    }
 
    // Convenience constructor for creating new notes
    public Note(int id, string title, string content)
    {
        Id        = id;
        _title    = string.Empty;
        _content  = string.Empty;
        Title     = title;
        Content   = content;
        CreatedAt = DateTime.Now;
        UpdatedAt = DateTime.Now;
    }
 
    public override string ToString() =>
        $"[{Id}] {Title}  ({UpdatedAt:yyyy-MM-dd HH:mm})";
}