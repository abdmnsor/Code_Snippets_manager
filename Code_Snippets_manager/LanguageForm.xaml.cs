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

namespace Code_Snippets_manager
{
    /// <summary>
    /// Interaction logic for LanguageForm.xaml
    /// </summary>
    public partial class LanguageForm : Window , INotifyPropertyChanged
    {
        public ObservableCollection<Language> Languages { get; set; } = new ObservableCollection<Language>();
        private Language _selectedLanguage;
        private string _newLanguageName;
        LanguagesContext lng = new LanguagesContext();
        Int64 languageID;
        public Language SelectedLanguage
        {
            get { return _selectedLanguage; }
            set
            {
                _selectedLanguage = value;
                if (_selectedLanguage == null)
                    return;
                languageID = _selectedLanguage.Id;
                NewLanguageName = _selectedLanguage.TheLanguage;
                OnPropertyChanged(nameof(SelectedLanguage));
            }
        }

        public string NewLanguageName
        {
            get { return _newLanguageName; }
            set { _newLanguageName = value; OnPropertyChanged(nameof(NewLanguageName)); }
        }

        public ICommand SaveLanguageCommand { get; }
        public ICommand DeleteLanguageCommand { get; }


        public LanguageForm()
        {
            InitializeComponent();
            loaddata();
            this.DataContext = this;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SaveLanguage();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DeleteLanguage();
        }

        private void SaveLanguage()
        {
            if (!string.IsNullOrWhiteSpace(NewLanguageName))
            {
                if (SelectedLanguage != null)
                {
                    // Update existing language
                    SelectedLanguage.TheLanguage = NewLanguageName;
                    lng.EditLanguage(languageID, NewLanguageName);
                }
                else
                {
                    lng.AddLanguage(NewLanguageName);
                }
                NewLanguageName = "";
                loaddata();
            }
        }

        private void DeleteLanguage()
        {
            if (SelectedLanguage != null)
            {
                lng.DeleteLanguage(languageID);
                NewLanguageName = "";
                SelectedLanguage = null;
            }
            loaddata();
        }
        private void loaddata()
        {
            Languages.Clear();
            DataTable dt = lng.GetAllLanguages();
            if (dt == null)
                return;

            if (dt.Rows.Count < 1)
                return;

            foreach (DataRow row in dt.Rows)
            {
                Languages.Add(new Language
                {
                    Id = row.Field<Int64>("Id"),
                    TheLanguage = row.Field<string>("Language"),
                    UpdatedAt = row.Field<DateTime>("UpdatedAt").ToString(),
                    CreatedAt = row.Field<DateTime>("CreatedAt").ToString()
                });
            }

        }

        // Notify UI when property changes
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
