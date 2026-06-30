📓 My Notebook App

A modern, cross-platform note-taking application built with C#, Avalonia, and .NET 10. This project demonstrates core Object-Oriented Programming (OOP) concepts including encapsulation, single responsibility, MVVM architecture, file I/O, and JSON serialization.

🎯 Features


✅ Create Notes — Add new notes with title and content
✅ View Notes — Click a note to display its full content
✅ Edit Notes — Modify title and content of existing notes
✅ Delete Notes — Remove notes permanently
✅ Search Notes — Find notes by keyword (title or content)
✅ Persistent Storage — All notes saved to notes.json file
✅ Automatic Reload — Notes load automatically on app startup
✅ Modern UI — Built with Avalonia with Fluent design theme


🛠️ Tech Stack

TechnologyPurposeC#Programming language.NET 10FrameworkAvalonia 11.1.3GUI Framework (cross-platform)ReactiveUIMVVM reactive bindingsSystem.Text.JsonJSON serialization/deserialization

📋 Requirements


.NET 10 SDK or higher
macOS, Windows, or Linux
VS Code (or any text editor)


Check your .NET version:

bashdotnet --version

🚀 Getting Started

1. Clone or open the project

bashcd NotebookApp

2. Restore NuGet packages

bashdotnet restore

3. Build the project

bashdotnet build

4. Run the application

bashdotnet run

The app window should open. Start creating notes! 📝

📁 Project Structure

NotebookApp/
├── Note.cs                          # Data model - represents a single note
├── Notebook.cs                      # Business logic - handles File I/O & serialization
├── ViewModels/
│   └── MainViewModel.cs             # MVVM ViewModel - manages UI state
├── Views/
│   ├── MainWindow.axaml             # Main UI layout (XAML)
│   └── MainWindow.axaml.cs          # Code-behind
├── App.axaml                        # Application theme & config
├── App.axaml.cs                     # Application entry point
├── Program.cs                       # .NET entry point
├── NotebookApp.csproj               # Project configuration
├── notes.json                       # Generated at runtime (stores notes)
└── .gitignore                       # Git ignore file

🔑 Key OOP Concepts

1. Encapsulation — Note.cs

The Note class hides internal data with private fields and exposes them through validated public properties:

csharppublic class Note
{
    private string _title;  // Private field
    
    public string Title     // Public property with validation
    {
        get => _title;
        set => _title = string.IsNullOrWhiteSpace(value)
            ? throw new ArgumentException("Title cannot be empty.")
            : value.Trim();
    }
}

Why? Ensures data integrity. Users can't set invalid titles.


2. Single Responsibility Principle — Notebook.cs

The Notebook class has one job: manage notes and handle persistence. It doesn't deal with UI.

csharppublic class Notebook
{
    // Only responsible for:
    public Note Add(string title, string content) { ... }
    public bool Delete(int id) { ... }
    public IEnumerable<Note> Search(string keyword) { ... }
    
    private void Save() { ... }   // File I/O + Serialization
    private void Load() { ... }   // File I/O + Deserialization
}

Why? If we need to change how notes are saved (e.g., to a database), we only modify this class.


3. MVVM Architecture — MainViewModel.cs

The ViewModel separates business logic from UI. It uses reactive properties that automatically notify the UI when values change:

csharppublic class MainViewModel : ReactiveObject
{
    private Note? _selectedNote;
    
    public Note? SelectedNote
    {
        get => _selectedNote;
        set
        {
            this.RaiseAndSetIfChanged(ref _selectedNote, value);
            if (value is not null)
            {
                EditTitle = value.Title;
                EditContent = value.Content;
            }
        }
    }
}

Why? The UI (MainWindow) is decoupled from logic. Easy to test and maintain.


💾 File I/O & Serialization

Serialization (Object → JSON)

When you create/edit a note, it's automatically serialized to JSON and saved:

csharpprivate void Save()
{
    var data = new NotebookData { NextId = _nextId, Notes = _notes };
    string json = JsonSerializer.Serialize(data, JsonOpts);      // ← SERIALIZATION
    File.WriteAllText(_filePath, json);                          // ← FILE I/O
}

This generates notes.json:

json{
  "nextId": 2,
  "notes": [
    {
      "id": 1,
      "title": "My First Note",
      "content": "Hello, World!",
      "createdAt": "2026-05-21T15:30:00",
      "updatedAt": "2026-05-21T15:30:00"
    }
  ]
}

Deserialization (JSON → Object)

When the app starts, it reads the JSON file and converts it back to C# objects:

csharpprivate void Load()
{
    if (!File.Exists(_filePath)) return;
    
    string json = File.ReadAllText(_filePath);                   // ← FILE I/O
    var data = JsonSerializer.Deserialize<NotebookData>(json);   // ← DESERIALIZATION
    if (data is not null)
    {
        _notes = data.Notes ?? new List<Note>();
        _nextId = data.NextId;
    }
}

Result: Your notes persist across app restarts! 🎉


🎮 How to Use the App

Create a Note


Click "+ New Note" button in header
Enter title and content
Click "Create"


View a Note


Click a note in the left sidebar
Full content displays on the right


Edit a Note


Select a note
Click "Edit" button
Modify title/content
Click "Save"


Delete a Note


Select a note
Click "Delete" button
Confirm deletion


Search Notes


Type in the search box on the left
Notes are filtered in real-time



🏗️ Architecture Diagram

┌─────────────────────────────────────────┐
│         MainWindow.axaml                │ ← VIEW (UI)
│      (XAML + Code-behind)               │
└──────────────┬──────────────────────────┘
               │ (Binds to)
┌──────────────▼──────────────────────────┐
│      MainViewModel.cs                   │ ← VIEWMODEL (Logic & State)
│  - AddNote()                            │
│  - SaveEdit()                           │
│  - DeleteNote()                         │
│  - SelectedNote (Reactive)              │
└──────────────┬──────────────────────────┘
               │ (Uses)
┌──────────────▼──────────────────────────┐
│       Notebook.cs                       │ ← MODEL (Business Logic)
│  - Add()                                │
│  - Delete()                             │
│  - Save()  ← File I/O + Serialization   │
│  - Load()                               │
└──────────────┬──────────────────────────┘
               │ (Uses)
┌──────────────▼──────────────────────────┐
│   Note.cs                               │ ← MODEL (Data)
│  - Title (Encapsulated)                 │
│  - Content                              │
│  - CreatedAt, UpdatedAt                 │
└─────────────────────────────────────────┘


📝 Example Workflow


User clicks "+ New Note"

MainWindow.axaml triggers ShowNewNoteForm() command
MainViewModel sets ShowNewNoteDialog = true
Modal dialog appears



User enters title and content, clicks "Create"

MainViewModel.AddNote() is called
Calls _notebook.Add(title, content)
Notebook.Add() creates a new Note object
Notebook.Save() is called:

Serializes notes to JSON
Writes to notes.json file (FILE I/O)



Note appears in left sidebar



User closes and reopens app

Notebook.Load() is called on startup:

Reads notes.json file (FILE I/O)
Deserializes JSON back to Note objects



Your notes are still there! ✅






🧪 Testing the App

Test File I/O


Create a note
Close the app
Reopen it
Result: Note still exists ✅


Test Serialization


Create a note
Open notes.json in a text editor
Result: See valid JSON with your note data ✅


Test Encapsulation


Try to create a Note with an empty title
The property setter throws an ArgumentException ✅


Test MVVM


Click a note → SelectedNote updates → UI refreshes automatically ✅



🔧 Configuration

Change the notes file path

In MainViewModel.cs, change:

csharpvar notebook = new Notebook("notes.json");  // ← Default location

To:

csharpvar notebook = new Notebook("/path/to/my/notes.json");  // ← Custom location

Modify the theme

In App.axaml, change:

xml<FluentTheme />  <!-- Light theme (default) -->

To:

xml<FluentTheme PreferredTheme="Dark" />  <!-- Dark theme -->


📦 NuGet Packages

PackageVersionPurposeAvalonia11.1.3GUI FrameworkAvalonia.Themes.Fluent11.1.3Modern Fluent design themeAvalonia.ReactiveUI11.1.3Reactive MVVM bindingsAvalonia.Desktop11.1.3Desktop platform supportAvalonia.Fonts.Inter11.1.3Font library

All included in NotebookApp.csproj.


🐛 Troubleshooting

App won't start

bashdotnet clean
dotnet restore
dotnet build
dotnet run

"ClassicDesktopApplicationLifetime not found"

Make sure Avalonia.Desktop package is installed:

bashdotnet add package Avalonia.Desktop --version 11.1.3

Notes not saving


Check that you have write permissions in the project directory
Ensure notes.json exists or can be created
Check file paths in MainViewModel.cs



📚 Learning Resources


Avalonia Documentation
ReactiveUI
MVVM Pattern
JSON Serialization in .NET



📄 License

This project is for educational purposes as part of a university OOP course.


✍️ Author

Aylin — Fırat University, 2nd Year Software Engineering Student


Course: Object-Oriented Programming (OOP)
Project Type: Weekly Assignment
Date: May 2026



🎓 What You'll Learn

By studying this codebase, you'll understand:


✅ Encapsulation — Protecting data with properties
✅ Single Responsibility — Each class does one thing
✅ MVVM Architecture — Separating UI from logic
✅ File I/O — Reading/writing files with File class
✅ JSON Serialization — Converting objects ↔ JSON
✅ Reactive Programming — UI automatically responds to state changes
✅ Dependency Injection — Passing dependencies to constructors
✅ Data Validation — Ensuring data integrity



🚀 Future Enhancements


 Add note categories/tags
 Dark mode theme toggle
 Export notes to PDF
 Cloud sync (OneDrive, Google Drive)
 Rich text editor (bold, italic, etc.)
 Note templates
 Markdown support
 Database backend (SQLite) instead of JSON



Enjoy building! Happy coding! 🎉
