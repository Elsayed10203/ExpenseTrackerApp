using ExpenseTrackerApp.Languages;
using ExpenseTrackerApp.UI.FontAwesome;

namespace ExpenseTrackerApp.UI.Controls;

public partial class EmptyView : StackLayout
{
    public EmptyView()
    {
        InitializeComponent();
    }

    public string Text
    {
        get { return (string)GetValue(TextProperty); }
        set { SetValue(TextProperty, value); }
    }

    public ImageSource ImageSource
    {
        get { return (ImageSource)GetValue(ImageSourceProperty); }
        set { SetValue(ImageSourceProperty, value); }
    }

    public static readonly BindableProperty TextProperty =
    BindableProperty.Create(nameof(Text), typeof(string), typeof(EmptyView), LanguagesResources.NoDataMsg, BindingMode.TwoWay);

    public static readonly BindableProperty ImageSourceProperty =
        BindableProperty.Create(nameof(ImageSource), typeof(ImageSource), typeof(EmptyView), new FontImageSource()
        {
            FontFamily = "FAPL",
            Glyph = Glyph.FileMedical,
            Size = 60,
            Color = Color.FromArgb("#adb5bd")
        }, BindingMode.TwoWay);
}