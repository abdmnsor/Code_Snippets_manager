using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;
using System.Windows.Media;
using Code_Snippets_manager.Models;

namespace Code_Snippets_manager.Services
{
    public class CompletionData : ICompletionData
    {
        public CompletionData(Snippet snippet)
        {
            Text = snippet.SnippetCode;
            Description = snippet.Description;
            Content = snippet.Title;
        }

        public ImageSource Image => null; // You can add an icon if needed.

        public string Text { get; }

        public object Content { get; set; } // Displayed in the completion list.

        public object Description { get; set; }

        public double Priority => 0;

        public void Complete(TextArea textArea, ISegment completionSegment, EventArgs insertionRequestEventArgs)
        {
            textArea.Document.Replace(completionSegment, Text);
        }
    }
}
