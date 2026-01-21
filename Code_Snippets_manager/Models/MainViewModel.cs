using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Code_Snippets_manager.Context;
using Code_Snippets_manager.Models;

namespace Code_Snippets_manager.Models
{
    public class MainViewModel : INotifyPropertyChanged
    {

        TagsContext tg = new TagsContext();
        LanguagesContext lng = new LanguagesContext();
        SnippetsContext snp = new SnippetsContext();

        DataTable tagstable ;
        DataTable languagestable ;
        DataTable snippetstable ;

        public ObservableCollection<Snippet> FilteredSnippets { get; set; } = new ObservableCollection<Snippet>();
        public ObservableCollection<string> Languages { get; set; } = new ObservableCollection<string>();


        private ObservableCollection<string> _selectedTags = new ObservableCollection<string>();
        public ObservableCollection<Snippet> Snippets { get; set; } = new ObservableCollection<Snippet>();
        public ObservableCollection<string> Tags { get; set; } = new ObservableCollection<string>();

        private string _codeText;
        public string CodeText
        {
            get => _codeText;
            set
            {
                if (_codeText != value)
                {
                    _codeText = value;
                    SelectedSnippet.SnippetCode = _codeText;
                    OnPropertyChanged(nameof(CodeText));
                }
            }
        }

        private string _selectedLanguage;
        public string SelectedLanguage
        {
            get { return _selectedLanguage; }
            set
            {
                _selectedLanguage = value;
                OnPropertyChanged(nameof(SelectedLanguage));
                ApplyFilter();
            }
        }


        public ObservableCollection<string> SelectedTags
        {
            get { return _selectedTags; }
            set
            {
                _selectedTags = value;
                OnPropertyChanged(nameof(SelectedTags));
                ApplyFilter();
            }
        }



        private Snippet _selectedSnippet;
        public Snippet SelectedSnippet
        {
            get { return _selectedSnippet; }
            set
            {
                _selectedSnippet = value;
                
                OnPropertyChanged(nameof(SelectedSnippet));
                if(value != null)
                    CodeText = _selectedSnippet.SnippetCode;
                //ApplyFilter();
            }
        }

        public MainViewModel()
        {     
            LoadTags();
            LoadLanguage();
            LoadSnippets();
        }

        private void LoadSnippets()
        {

            snippetstable = snp.GetAllSnippet();
            if (snippetstable != null)
            {
                if (snippetstable.Rows.Count > 0)
                {
                    foreach (DataRow row in snippetstable.Rows)
                    {

                        Snippets.Add(new Snippet {
                            id = row.Field<Int64>("Id"),
                            Title = row.Field<string>("Title"),
                            Tags = row.Field<string>("Tags"),
                            Description = row.Field<string>("Description"),
                            SnippetCode = row.Field<string>("Snippet"),
                            UpdatedAt = row.Field<DateTime>("UpdatedAt").ToString(),
                            CreatedAt = row.Field<DateTime>("CreatedAt").ToString(),
                            Language = row.Field<string>("Language") });
                    }
                }
            }


            
        }

        private void LoadTags()
        {
            tagstable = tg.GetAllTags();

            if (tagstable != null)
            {
                if (tagstable.Rows.Count > 0)
                {
                    foreach (DataRow row in tagstable.Rows)
                    {

                        Tags.Add("#" + row.Field<string>("Tag").ToUpper().ToString());


                    }
                }
            }
        }
        public void ApplyFilter()
        {
            if (FilteredSnippets == null)
                return;
            if (FilteredSnippets.Count > 0)
                FilteredSnippets.Clear();

            

            
            var filteredByLanguage = Snippets.Where(s => SelectedLanguage == "All Languages" || s.Language.ToLower() == SelectedLanguage?.ToLower());

            if (SelectedLanguage == "All Languages")
                filteredByLanguage = Snippets;

            //var filtered = filteredByLanguage.Where(s => (SelectedTags.Count == 0 ||
            //s.Tags.Any(tag =>SelectedTags.Contains(tag.ToString())))).ToList();

            //if (SelectedTags.Count < 1)
            //   {
            //    FilteredSnippets = Snippets;
            //    return; 
            //} 

          //  var filtered = filteredByLanguage.Where(s => s.Tags.Contains(SelectedTags.) ).ToList();


            foreach (var snippet in filteredByLanguage)
            {
                if (SelectedTags.Count < 1)
                    FilteredSnippets.Add(snippet);

                foreach (var item in SelectedTags)
                {
                    if (snippet.Tags.Contains(item.Replace("#", "").ToString()))
                        FilteredSnippets.Add(snippet);

                   // FilteredSnippets.Add(snippet.Tags.Contains(item.Replace("#","").ToString() ));
                }
                
            }
        }


        private void LoadLanguage()
        {
            languagestable = lng.GetAllLanguages();
            


            if (languagestable != null)
            {
                Languages.Clear(); // Ensure old data is removed before adding new ones.
                if (languagestable.Rows.Count > 0)
                {

                    foreach (DataRow row in languagestable.Rows)
                    {
                        Languages.Add(row.Field<string>("Language").ToString());
                    }
                }
            }

        }
        public void LoadData()
        {
            Snippets.Clear();
            Languages.Clear();
            Tags.Clear();

            LoadTags();
            LoadLanguage();
            
            LoadSnippets();

            ApplyFilter(); // Call filter after loading
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void CopyCodeSnippet()
        {
            if (SelectedSnippet == null)
                return;

            Clipboard.SetText(SelectedSnippet.SnippetCode);
        }

        public void SnippetDelete()
        {
            if (SelectedSnippet == null)
                return;

            snp.DeleteSnippet(SelectedSnippet.id);
        }

        public void SnippetSaveData()
        {
            if (SelectedSnippet == null)
                return;

            snp.UpdateSnippet(SelectedSnippet);
        }

        public void ViewSnippetData()
        {
            if (SelectedSnippet == null)
                return;

            SnippetEditorWindow des = new SnippetEditorWindow(SelectedSnippet.Title , SelectedSnippet.SnippetCode);
            des.ShowDialog();
            SelectedSnippet.Description = des.snippetdescription;
            snp.UpdateSnippet(SelectedSnippet);

        }
        private bool CheckString(Snippet snippet ,string search_Value) 
        {
            var title = snippet.Title.ToLower();
            if (title.Contains(search_Value.ToLower()))
                return true;

            return false;

        }
        public void SearchSnippet(string text)
        {
            ObservableCollection<Snippet> f = new ObservableCollection<Snippet>();
            if (FilteredSnippets == null)
                return;

            if (FilteredSnippets.Count > 0)
                    FilteredSnippets.Clear();

            var filteredByLanguage = Snippets.Where(s => SelectedLanguage == "All Languages" || s.Language.ToLower() == SelectedLanguage?.ToLower());

            if (SelectedLanguage == "All Languages")
                filteredByLanguage = Snippets;

            foreach (var snippet in filteredByLanguage)
            {
                if (SelectedTags.Count < 1)
                    f.Add(snippet);

                foreach (var item in SelectedTags)
                {
                    if (snippet.Tags.Contains(item.Replace("#", "").ToString()))
                        f.Add(snippet);
                }

            }

            var filterbytext = f.Where(s => CheckString(s , text));

        
            FilteredSnippets.Clear();
            foreach (var item in filterbytext)
            {
                
                FilteredSnippets.Add(item);
            }

            

        }
    }


public class SnippetEditorWindow : Window
    {
        private TextBox txtTitle, txtDescription;

        public string snippetdescription = "";
        public SnippetEditorWindow(string title , string description)
        {
            // Window Properties
            this.Title = "Snippet Editor";
            this.Width = 800;
            this.Height = 500;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.Background = Brushes.White;
            this.ResizeMode = ResizeMode.NoResize;
            this.WindowStyle = WindowStyle.None; // Removes the default window border

            // Main Grid
            Grid mainGrid = new Grid()
            {
                Margin = new Thickness(20)
            };
            this.Content = mainGrid;

            // Define Rows
            mainGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(60) }); // Header
            mainGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(50) }); // Title
            mainGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) }); // Description
            mainGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(80) }); // Buttons

            // Header
            Border headerBorder = new Border()
            {
                Height = 50,
                Background = new LinearGradientBrush(Colors.Blue, Colors.LightBlue, 45),
                CornerRadius = new CornerRadius(10, 10, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Margin = new Thickness(0, 0, 0, 10)
            };
            Grid.SetRow(headerBorder, 0);
            mainGrid.Children.Add(headerBorder);

            TextBlock lblHeader = new TextBlock()
            {
                Text = "Snippet Editor",
                FontSize = 24,
                FontWeight = FontWeights.Bold,
                Foreground = Brushes.White,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
            headerBorder.Child = lblHeader;

            // Title Input
            txtTitle = CreateModernTextBox(title);
            Grid.SetRow(txtTitle, 1);
            mainGrid.Children.Add(txtTitle);

            // Description Input
            txtDescription = CreateModernTextBox(description);
            txtDescription.Height = 250;
            txtDescription.AcceptsReturn = true;
            txtDescription.TextWrapping = TextWrapping.Wrap;
            Grid.SetRow(txtDescription, 2);
            mainGrid.Children.Add(txtDescription);

            // Buttons Grid
            Grid buttonGrid = new Grid();
            buttonGrid.ColumnDefinitions.Add(new ColumnDefinition());
            buttonGrid.ColumnDefinitions.Add(new ColumnDefinition());
            Grid.SetRow(buttonGrid, 3);
            mainGrid.Children.Add(buttonGrid);

            // Save Button
            Button btnSave = CreateModernButton("Save", Colors.Green);
            btnSave.Click += (s, e) => ChengDescription(txtDescription.Text); //MessageBox.Show("Snippet Saved!");
            Grid.SetColumn(btnSave, 0);
            buttonGrid.Children.Add(btnSave);

            // Close Button
            Button btnClose = CreateModernButton("Close", Colors.Red);
            btnClose.Click += (s, e) => this.Close();
            Grid.SetColumn(btnClose, 1);
            buttonGrid.Children.Add(btnClose);
        }

        private void ChengDescription(string newdesc) 
        {
            snippetdescription = txtDescription.Text;
            MessageBox.Show("Description Updated!");
            this.Close();
        }
        private TextBox CreateModernTextBox(string placeholder)
        {
            TextBox textBox = new TextBox()
            {
                FontSize = 16,
                Padding = new Thickness(10),
                Background = Brushes.White,
                BorderThickness = new Thickness(2),
                BorderBrush = Brushes.LightGray,
                Foreground = Brushes.Gray,
                Text = placeholder
            };

            // Remove placeholder on focus
            textBox.GotFocus += (s, e) =>
            {
                if (textBox.Text == placeholder)
                {
                    textBox.Text = "";
                    textBox.Foreground = Brushes.Black;
                    textBox.BorderBrush = Brushes.Blue;
                }
            };

            // Restore placeholder if empty
            textBox.LostFocus += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(textBox.Text))
                {
                    textBox.Text = placeholder;
                    textBox.Foreground = Brushes.Gray;
                    textBox.BorderBrush = Brushes.LightGray;
                }
            };

            return textBox;
        }

        
        private Button CreateModernButton(string text, Color color)
        {
            return new Button()
            {
                Content = text,
                FontSize = 16,
                Padding = new Thickness(10),
                Background = new SolidColorBrush(color),
                Foreground = Brushes.White,
                BorderThickness = new Thickness(0),
                Margin = new Thickness(20, 10, 20, 10),
                Cursor = System.Windows.Input.Cursors.Hand,
                Height = 40
            };
        }


    }



}
