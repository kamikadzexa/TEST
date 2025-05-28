using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TEST.Views
{
    public class MessageBox : Window
    {
        public MessageBox(Window parent, string title, string message)
        {
            this.Title = title;
            this.Width = 300;
            this.Height = 150;

            var textBlock = new TextBlock
            {
                Text = message,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
                TextWrapping = TextWrapping.Wrap
            };

            var button = new Button
            {
                Content = "OK",
                Width = 80,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center
            };
            button.Click += (s, e) => this.Close();

            this.Content = new StackPanel
            {
                Children = { textBlock, button },
                Margin = new Thickness(20)
            };
        }
    }
}
