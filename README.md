# 📝 Code Snippet Manager

A powerful and intuitive **WPF desktop application** for managing, organizing, and quickly accessing your code snippets across multiple programming languages.

![Code Snippet Manager](screenshot/fullProject.png)

## ✨ Features

### 🎯 Core Functionality
- **Multi-Language Support** - Store snippets for any programming language with syntax highlighting
- **Smart Organization** - Organize snippets using tags and language categories
- **Advanced Search** - Quickly find snippets by title, tags, or content
- **Syntax Highlighting** - Built-in syntax highlighting powered by AvalonEdit
- **Custom Syntax Support** - Load custom syntax highlighting definitions (.xshd files)
- **One-Click Copy** - Instantly copy snippets to clipboard for use

### 🎨 User Interface
- **Clean & Modern Design** - Intuitive WPF interface with easy navigation
- **Code Editor** - Full-featured code editor with syntax highlighting
- **Tag-Based Filtering** - Filter snippets by multiple tags simultaneously
- **Language Filtering** - View snippets by programming language
- **Real-Time Preview** - See your code with proper syntax highlighting

### 💾 Data Management
- **SQLite Database** - Lightweight, local storage for all your snippets
- **CRUD Operations** - Create, Read, Update, and Delete snippets with ease
- **Persistent Storage** - All snippets are saved locally and persist between sessions
- **Metadata Tracking** - Automatic tracking of creation and update timestamps

## 🖼️ Screenshots

<details>
<summary>View Screenshots</summary>

### Main Application
![Full Project View](screenshot/fullProject.png)

### Adding New Snippets
![Add New Snippet](screenshot/add%20new%20snippet.png)

### Managing Languages
![Add New Languages](screenshot/add%20new%20languages.png)

### Managing Tags
![Add New Tags](screenshot/add%20new%20tags.png)

### Viewing Stored Code
![Show Stored Code](screenshot/show%20stored%20code.png)

</details>

## 🚀 Getting Started

### Prerequisites

- **Windows OS** (Windows 10 or later recommended)
- **.NET 8.0 Runtime** or SDK
- **Visual Studio 2022** (for development)

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/yourusername/Code_Snippets_manager.git
   cd Code_Snippets_manager
   ```

2. **Open the solution**
   ```bash
   Code_Snippets_manager.sln
   ```

3. **Restore NuGet packages**
   - Visual Studio will automatically restore packages
   - Or manually run: `dotnet restore`

4. **Build and Run**
   - Press `F5` in Visual Studio
   - Or run: `dotnet run --project Code_Snippets_manager`

### Dependencies

The project uses the following NuGet packages:

- **[AvalonEdit](https://www.nuget.org/packages/AvalonEdit/)** (v6.3.0.90) - Advanced WPF text editor with syntax highlighting
- **[System.Data.SQLite](https://www.nuget.org/packages/System.Data.SQLite/)** (v1.0.119) - SQLite database engine for .NET

## 📖 Usage

### Creating a New Snippet

1. Click the **"+ New Snippet"** button
2. Fill in the snippet details:
   - **Title** - A descriptive name for your snippet
   - **Language** - Select the programming language
   - **Tags** - Add relevant tags for organization
   - **Description** - Brief description of what the snippet does
   - **Code** - Paste or write your code snippet
3. Click **"Save"** to store the snippet

### Managing Languages

1. Click **"Manage Language"** button
2. Add new programming languages to the system
3. Languages will appear in the language filter dropdown

### Managing Tags

1. Click **"Manage Tags"** button
2. Create new tags for organizing your snippets
3. Use tags to categorize snippets by topic, framework, or use case

### Searching and Filtering

- **Search Bar** - Type to search snippets by title or content
- **Language Filter** - Select a language from the dropdown to filter snippets
- **Tag Filter** - Select one or more tags to filter snippets
- **Combined Filters** - Use search, language, and tags together for precise filtering

### Using Snippets

1. Select a snippet from the list
2. Click **"Preview"** to view the code with syntax highlighting
3. Click **"Copy"** to copy the snippet to your clipboard
4. Paste the snippet into your IDE or text editor

### Custom Syntax Highlighting

To add custom syntax highlighting:

1. Create an `.xshd` file with your syntax definition
2. Place it in the `CustomSyntaxHighlighting` folder (created automatically on first run)
3. Restart the application to load the custom syntax

## 🏗️ Project Structure

```
Code_Snippets_manager/
├── Context/              # Database contexts
│   ├── LanguagesContext.cs
│   ├── SnippetsContext.cs
│   └── TagsContext.cs
├── Models/               # Data models
│   ├── Language.cs
│   ├── MainViewModel.cs
│   ├── Snippet.cs
│   └── Tag.cs
├── Services/             # Business logic and utilities
│   ├── CompletionData.cs
│   ├── DatabaseManager.cs
│   ├── NotificationWindow.cs
│   └── TextBindingBehavior.cs
├── screenshot/           # Application screenshots
├── MainWindow.xaml       # Main application window
├── SnippetForm.xaml      # Snippet creation/edit form
├── LanguageForm.xaml     # Language management form
└── TagForm.xaml          # Tag management form
```

## 🛠️ Technology Stack

- **Framework**: .NET 8.0
- **UI Framework**: WPF (Windows Presentation Foundation)
- **Architecture**: MVVM (Model-View-ViewModel)
- **Database**: SQLite
- **Code Editor**: AvalonEdit
- **Language**: C# 12

## 🤝 Contributing

Contributions are welcome! Here's how you can help:

1. **Fork the repository**
2. **Create a feature branch** (`git checkout -b feature/AmazingFeature`)
3. **Commit your changes** (`git commit -m 'Add some AmazingFeature'`)
4. **Push to the branch** (`git push origin feature/AmazingFeature`)
5. **Open a Pull Request**

### Ideas for Contributions

- 🌐 Add export/import functionality (JSON, XML)
- 🔍 Implement full-text search with regex support
- 🎨 Add themes and customizable UI
- ☁️ Cloud sync capabilities
- 📱 Cross-platform support with Avalonia UI
- 🔐 Encryption for sensitive snippets
- 📊 Statistics and usage analytics

## 📝 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 👤 Author

**Your Name**
- GitHub: [@yourusername](https://github.com/yourusername)

## 🙏 Acknowledgments

- [AvalonEdit](https://github.com/icsharpcode/AvalonEdit) - Excellent WPF text editor component
- [SQLite](https://www.sqlite.org/) - Reliable embedded database
- All contributors who help improve this project

## 📧 Support

If you have any questions or run into issues, please:
- Open an [issue](https://github.com/yourusername/Code_Snippets_manager/issues)
- Check existing issues for solutions
- Contribute to discussions

---

<div align="center">

**⭐ Star this repository if you find it helpful!**

Made with ❤️ for developers who love organized code

</div>
