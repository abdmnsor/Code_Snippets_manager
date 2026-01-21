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
    /// Interaction logic for TagForm.xaml
    /// </summary>
    public partial class TagForm : Window , INotifyPropertyChanged
    {
        public ObservableCollection<Tag> Tags { get; set; } = new ObservableCollection<Tag>();
        private Tag _selectedTag;
        private string _newTagName;
        private Int64 tagID;
        TagsContext tg = new TagsContext();

        public TagForm()
        {
            InitializeComponent();

            loaddata();
            this.DataContext = this;
            //SaveTagCommand = new RelayCommand(SaveTag);
            //DeleteTagCommand = new RelayCommand(DeleteTag);
        }


        public Tag SelectedTag
        {
            get { return _selectedTag; }
            set
            {
                _selectedTag = value;
                if (_selectedTag == null)
                    return;
                NewTagName = _selectedTag.Name;
                tagID = _selectedTag.Id;
                OnPropertyChanged(nameof(SelectedTag));
            }
        }


        public string NewTagName
        {
            get { return _newTagName; }
            set { _newTagName = value; OnPropertyChanged(nameof(NewTagName)); }
        }

        public ICommand SaveTagCommand { get; }
        public ICommand DeleteTagCommand { get; }

        private void loaddata()
        {
            Tags.Clear();
            DataTable dt = tg.GetAllTags();
            if (dt == null)
                return;

            if (dt.Rows.Count < 1)
                return;

            foreach (DataRow row in dt.Rows)
            {
                Tags.Add(new Tag {
                    Id = row.Field<Int64>("Id") ,
                    Name = row.Field<string>("Tag"),
                    UpdatedAt = row.Field<DateTime>("UpdatedAt").ToString(),
                    CreatedAt = row.Field<DateTime>("CreatedAt").ToString()
                });
            }

        }

        private void SaveTag()
        {
            if (!string.IsNullOrWhiteSpace(NewTagName))
            {
                if (SelectedTag != null)
                {
                    // Update existing tag
                    tg.EditeTag(tagID, NewTagName);
                }
                else
                {
                    // Add new tag
                    tg.AddTags(NewTagName);
                   
                }
                loaddata();
                NewTagName = "";
            }
        }

        private void DeleteTag()
        {
            if (SelectedTag != null)
            {
               
                tg.DeleteTag(tagID);
                NewTagName = "";
                loaddata();
                SelectedTag = null;
            }
        }


        // Notify UI when property changes
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DeleteTag();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (txt_TagName.Text == "")
                return;

            SaveTag();
        }
    }
}
