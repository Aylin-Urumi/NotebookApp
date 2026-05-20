using System.Collections.ObjectModel;
using ReactiveUI;
 
namespace NotebookApp.ViewModels;
 
/// <summary>
/// ViewModel for the main window. Handles all business logic and UI state.
/// This is the "V" in MVVM — it bridges the Model (Notebook) and View (XAML).
/// </summary>
public class MainViewModel : ReactiveObject
{
    private readonly Notebook _notebook;
    private Note? _selectedNote;
    private string _searchKeyword = string.Empty;
    private bool _isEditMode = false;
    private string _editTitle = string.Empty;
    private string _editContent = string.Empty;
 
    public ObservableCollection<Note> Notes { get; }
    public ObservableCollection<Note> FilteredNotes { get; }
 
    // ── Properties (Reactive) ─────────────────────────────────────────────
 
    public Note? SelectedNote
    {
        get => _selectedNote;
        set
        {
            this.RaiseAndSetIfChanged(ref _selectedNote, value);
            if (value is not null)
            {
                EditTitle   = value.Title;
                EditContent = value.Content;
                IsEditMode  = false;
            }
        }
    }
 
    public string SearchKeyword
    {
        get => _searchKeyword;
        set
        {
            this.RaiseAndSetIfChanged(ref _searchKeyword, value);
            ApplyFilter();
        }
    }
 
    public bool IsEditMode
    {
        get => _isEditMode;
        set => this.RaiseAndSetIfChanged(ref _isEditMode, value);
    }
 
    public string EditTitle
    {
        get => _editTitle;
        set => this.RaiseAndSetIfChanged(ref _editTitle, value);
    }
 
    public string EditContent
    {
        get => _editContent;
        set => this.RaiseAndSetIfChanged(ref _editContent, value);
    }
 
    // ── Constructor ───────────────────────────────────────────────────────
    public MainViewModel()
    {
        _notebook = new Notebook("notes.json");
        Notes = new ObservableCollection<Note>(_notebook.GetAll());
        FilteredNotes = new ObservableCollection<Note>(Notes);
 
        // Load initial notes
        RefreshNotesList();
    }
 
    // ── Commands / Actions ────────────────────────────────────────────────
 
    public void AddNote(string title, string content)
    {
        if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(content))
        {
            return; // Validation should happen in UI
        }
 
        var newNote = _notebook.Add(title, content);
        Notes.Add(newNote);
        ApplyFilter();
        EditTitle = string.Empty;
        EditContent = string.Empty;
    }
 
    public void DeleteNote()
    {
        if (SelectedNote is null) return;
 
        _notebook.Delete(SelectedNote.Id);
        Notes.Remove(SelectedNote);
        ApplyFilter();
        SelectedNote = null;
    }
 
    public void SaveEdit()
    {
        if (SelectedNote is null) return;
 
        _notebook.Update(SelectedNote.Id, EditTitle, EditContent);
        SelectedNote.Title = EditTitle;
        SelectedNote.Content = EditContent;
        SelectedNote.UpdatedAt = DateTime.Now;
        IsEditMode = false;
        ApplyFilter(); // Refresh display
    }
 
    public void CancelEdit()
    {
        IsEditMode = false;
        if (SelectedNote is not null)
        {
            EditTitle = SelectedNote.Title;
            EditContent = SelectedNote.Content;
        }
    }
 
    public void EnterEditMode()
    {
        IsEditMode = true;
    }
 
    private void ApplyFilter()
    {
        FilteredNotes.Clear();
        IEnumerable<Note> filtered = string.IsNullOrWhiteSpace(SearchKeyword)
            ? Notes
            : _notebook.Search(SearchKeyword);
 
        foreach (var note in filtered)
        {
            FilteredNotes.Add(note);
        }
    }
 
    private void RefreshNotesList()
    {
        Notes.Clear();
        foreach (var note in _notebook.GetAll())
        {
            Notes.Add(note);
        }
        ApplyFilter();
    }
}