using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Code_Snippets_manager.Context;
using Code_Snippets_manager.Models;
using Code_Snippets_manager.Services;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Highlighting;

namespace Code_Snippets_manager
{
    /// <summary>
    /// Interaction logic for SnippetForm.xaml
    /// </summary>
    public partial class SnippetForm : Window
    {
        public ObservableCollection<string> Languages { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<string> AvailableTags { get; set; } = new ObservableCollection<string>();

        private ObservableCollection<string> _selectedTags = new ObservableCollection<string>();
        public ObservableCollection<string> SelectedTags
        {
            get { return _selectedTags; }
            set
            {
                _selectedTags = value;
                OnPropertyChanged(nameof(SelectedTags));
            }
        }
        public Snippet NewSnippet { get; set; } = new Snippet();
        //public ICommand SaveCommand { get; } = new RelayCommand(SaveSnippet);

        TagsContext tg = new TagsContext();
        LanguagesContext lng = new LanguagesContext();
        SnippetsContext snp = new SnippetsContext();

        DataTable tagstable;
        DataTable languagestable;
        DataTable snippetstable;


        public SnippetForm()
        {
            InitializeComponent();
            LoadLanguage();
            LoadTags();
            this.DataContext = this;
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

                        AvailableTags.Add("#" + row.Field<string>("Tag").ToUpper().ToString());


                    }
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
        private void SaveSnippet()
        {
            if (string.IsNullOrWhiteSpace(NewSnippet.Title) || string.IsNullOrWhiteSpace(NewSnippet.SnippetCode))
            {
                MessageBox.Show("Please enter a title and snippet code.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }


            snp.AddSnippet(NewSnippet);
            MessageBox.Show($"Snippet saved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SaveSnippet();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            SaveSnippet();
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string strTags = "";
            var listBox = sender as ListBox;
            if (listBox != null && listBox.SelectedItems != null)
            {
                SelectedTags.Clear();
                foreach (var item in listBox.SelectedItems)
                {
                    if (strTags == "")
                    {
                        strTags = item.ToString().Replace("#", "");
                        continue;
                    }
                    strTags += ", " + item.ToString().Replace("#", "");
                }

            }
            NewSnippet.Tags = strTags.ToString();
        }

        private void CodeEditor_TextChanged(object sender, EventArgs e)
        {
                NewSnippet.SnippetCode = CodeEditor.Text;    
        }



        private void CBX_LanguageAdd_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (CBX_LanguageAdd.SelectedValue == "")
                return;

            if (CBX_LanguageAdd.SelectedValue == "All Languages")
                return;


            CodeEditor.SyntaxHighlighting = HighlightingManager.Instance.GetDefinition(CBX_LanguageAdd.SelectedValue.ToString());
        }
    }
}
