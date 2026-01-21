using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Code_Snippets_manager.Context;
using Code_Snippets_manager.Models;
using Code_Snippets_manager.Services;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using System.IO;
using System.Xml;
using ICSharpCode.AvalonEdit.CodeCompletion;

namespace Code_Snippets_manager;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    /// <summary>
    /// VIEW MODEL TO MANAGE THE SNIPPETS 
    /// </summary>
    MainViewModel ViewModel = new MainViewModel();

    public MainWindow()
    {
        InitializeComponent();
        this.DataContext = ViewModel;
    }
    /// <summary>
    /// ON LOAD CHECK IF THERE ANY NEW LOANGUAGE HIGHLIGHTING 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        
        CBX_Languages.SelectedIndex = 0;
        string languagefolder = Environment.CurrentDirectory + "\\CustomSyntaxHighlighting\\";
        
        if (!Directory.Exists(languagefolder))
        {
            Directory.CreateDirectory(languagefolder);
        }
        string[] subfiles = Directory.GetFiles(languagefolder);
        foreach (var item in subfiles)
        {
            FileInfo fileinfo = new FileInfo(item);
            if (fileinfo.Extension == ".xshd")
            {
                //new NotificationWindow($"Loading Syntax: {item}").Show(); // Debugging output
                LoadCustomSyntax(CodeEditor, item);
            }
        }

        // JavaScript, XML, HTML
        var definition = HighlightingManager.Instance.GetDefinition("VB.NET");
        if (definition == null)
        {
            new NotificationWindow("Syntax Definition 'VB.NET' not found!").Show(); // Debugging output
        }
        CodeEditor.SyntaxHighlighting = definition;

    }


    /// <summary>
    /// TAGS CHENG VALUE 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void TagsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var listBox = sender as ListBox;
        if (listBox != null && listBox.SelectedItems != null)
        {
            ViewModel.SelectedTags.Clear();
            foreach (var item in listBox.SelectedItems)
            {
                ViewModel.SelectedTags.Add(item.ToString().Replace("#" , ""));
            }
            ViewModel.ApplyFilter();
        }
    }



    /// <summary>
    /// OPEN THE NEW TAGS FORM
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Button_Click(object sender, RoutedEventArgs e)
    {
        TagForm tagfrm = new TagForm();
        tagfrm.ShowDialog();
        ViewModel.LoadData();
        CBX_Languages.SelectedIndex = 0;
    }



    /// <summary>
    /// OPEN THE NEW LANGUAGE FORM
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Button_Click_1(object sender, RoutedEventArgs e)
    {
        LanguageForm lngfrm = new LanguageForm();
        lngfrm.ShowDialog();
        ViewModel.LoadData();
        CBX_Languages.SelectedIndex = 0;
    }

    /// <summary>
    /// OPEN THE NEW SNIPPET FORM
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Button_Click_2(object sender, RoutedEventArgs e)
    {
        SnippetForm snipfrm = new SnippetForm();
        snipfrm.ShowDialog();
        ViewModel.LoadData();
        CBX_Languages.SelectedIndex = 0;
    }


    /// <summary>
    /// COPY THE CURRENT SNIPPET TO THE CLIPBOARD 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Button_Click_3(object sender, RoutedEventArgs e)
    {
        new NotificationWindow("Code Snippet Is In Ur Clipboard!").Show();
        ViewModel.CopyCodeSnippet();
    }


    /// <summary>
    /// DELETE THE SELECTED SNIPPET 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Button_Click_4(object sender, RoutedEventArgs e)
    {
        new NotificationWindow("Snippet Deleted Success!").Show();
        ViewModel.SnippetDelete();
    }


    /// <summary>
    /// SAVE SNIPPETS IN DATABASS 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Button_Click_5(object sender, RoutedEventArgs e)
    {
        ViewModel.SnippetSaveData();
        new NotificationWindow("Snippet Saved Success!").Show();
    }


    /// <summary>
    /// VIEW THE SELECTED SNIPPET
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Button_Click_6(object sender, RoutedEventArgs e)
    {
     
        ViewModel.ViewSnippetData();
       
    }
    /// <summary>
    /// SEARCH BAR 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void txtSearchbar_TextChanged(object sender, TextChangedEventArgs e)
    {
        ViewModel.SearchSnippet(txtSearchbar.Text);
    }

    private void CodeEditor_TextChanged(object sender, EventArgs e)
    {
    
        if (sender is TextEditor editor)
        {
            //CompletionWindow completionWindow = new CompletionWindow(CodeEditor.TextArea);
            //IList<ICompletionData> data = completionWindow.CompletionList.CompletionData;

            TextBindingBehavior.SetBoundText(editor, editor.Text);
            BindingExpression binding = BindingOperations.GetBindingExpression(editor, TextBindingBehavior.BoundTextProperty);
            binding?.UpdateSource();

            //foreach (var item in ViewModel.Snippets)
            //{
            //    data.Add(new CompletionData(item));
            //}
            
            //completionWindow.Show();
         
        }
    }


    /// <summary>
    /// FUNCTION TO LOAD THE NEW LANGUAGES 
    /// </summary>
    /// <param name="editor"></param>
    /// <param name="filePath"></param>
    public static void LoadCustomSyntax(TextEditor editor, string filePath)
    {
        try
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                using (XmlTextReader xmlReader = new XmlTextReader(reader))
                {
                    var customSyntax = HighlightingLoader.Load(xmlReader, HighlightingManager.Instance);

                    if (customSyntax == null)
                    {
                        new NotificationWindow($"Failed to load syntax from: {filePath}").Show();
                    }
                    else
                    {
                        //new NotificationWindow($"Successfully loaded syntax: {customSyntax.Name}").Show();
                        HighlightingManager.Instance.RegisterHighlighting(customSyntax.Name, new string[] { ".custom" }, customSyntax);
                        editor.SyntaxHighlighting = customSyntax;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            new NotificationWindow($"Error loading XSHD: {ex.Message}").Show();
        }
    }




    /// <summary>
    /// LANGUAGE CHENG THE COMBOBOX CHENG THE VALUE 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void CBX_Snippet_Language_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (CBX_Snippet_Language.SelectedValue == "")
            return;

        if (CBX_Snippet_Language.SelectedValue == "All Languages")
            return;

        if(CBX_Snippet_Language.SelectedValue != null)
            CodeEditor.SyntaxHighlighting = HighlightingManager.Instance.GetDefinition(CBX_Snippet_Language.SelectedValue.ToString());

    }
}