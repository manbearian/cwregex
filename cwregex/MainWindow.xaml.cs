using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace cwregex;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{

    readonly Puzzle p = new Puzzle();
    readonly TextBox[] textBoxes;
    readonly Label[,] labels = new Label[3,13];

    public MainWindow()
    {
        InitializeComponent();

        labels[0, 0] = r0_00;
        labels[0, 1] = r0_01;
        labels[0, 2] = r0_02;
        labels[0, 3] = r0_03;
        labels[0, 4] = r0_04;
        labels[0, 5] = r0_05;
        labels[0, 6] = r0_06;
        labels[0, 7] = r0_07;
        labels[0, 8] = r0_08;
        labels[0, 9] = r0_09;
        labels[0, 10] = r0_10;
        labels[0, 11] = r0_11;
        labels[0, 12] = r0_12;

        labels[1, 0] = r1_00;
        labels[1, 1] = r1_01;
        labels[1, 2] = r1_02;
        labels[1, 3] = r1_03;
        labels[1, 4] = r1_04;
        labels[1, 5] = r1_05;
        labels[1, 6] = r1_06;
        labels[1, 7] = r1_07;
        labels[1, 8] = r1_08;
        labels[1, 9] = r1_09;
        labels[1, 10] = r1_10;
        labels[1, 11] = r1_11;
        labels[1, 12] = r1_12;

        labels[2, 0] = r2_00;
        labels[2, 1] = r2_01;
        labels[2, 2] = r2_02;
        labels[2, 3] = r2_03;
        labels[2, 4] = r2_04;
        labels[2, 5] = r2_05;
        labels[2, 6] = r2_06;
        labels[2, 7] = r2_07;
        labels[2, 8] = r2_08;
        labels[2, 9] = r2_09;
        labels[2, 10] = r2_10;
        labels[2, 11] = r2_11;
        labels[2, 12] = r2_12;

        for (int i = 0; i < 3; ++i)
        {
            for (int j = 0; j < 13; ++j)
            {
                labels[i, j].Content = p.GetClue((Direction)i, j);
            }
        }

        textBoxes = new TextBox[p.MaxIndex];
        int ix = 0;
        foreach (var child in canvas.Children)
        {
            if (child is TextBox tb)
            {
                textBoxes[ix++] = tb;
            }
        }

        RestoreState();
    }

    private void RestoreState()
    {
        var savedValues = Properties.Settings.Default.values;
        if (savedValues?.Length == p.MaxIndex)
        {
            for (int i = 0; i < p.MaxIndex; ++i)
            {
                textBoxes[i].Text = savedValues[i]?.ToString() ?? "";
                p.Set(i, savedValues[i]);
            }
        }
    }

    private void SaveState()
    {
        Properties.Settings.Default.values = new char?[p.MaxIndex];
        for (int i = 0; i < p.MaxIndex; ++i)
        {
            Properties.Settings.Default.values[i] = p.Get(i);
        }
        Properties.Settings.Default.Save();
    }

    private void TextBox_PreviewGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
    {
        (sender as TextBox)?.SelectAll();
    }

    private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        // fill the full puzzle for lack of a better option
        for (int i = 0; i < p.MaxIndex; ++i)
        {
            string text = textBoxes[i].Text;
            p.Set(i, string.IsNullOrEmpty(text) ? null : text[0]);
        }

        SaveState();

        if (sender is TextBox tb && tb.Text != "")
        {
            tb.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
        }

        for (int i = 0; i < 3; ++i)
        {
            for (int j = 0; j < 13; ++j)
            {
                var valid = p.Validate((Direction)i, j);
                labels[i, j].Foreground = valid ? Brushes.Green : Brushes.Black;
            }
        }
    }
}
