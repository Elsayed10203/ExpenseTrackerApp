using CommunityToolkit.Mvvm.Input;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace ExpenseTrackerApp.UI.Controls;

public partial class FloatingLabelEntry : ContentView
{
    private readonly int _placeholderFontSize = 17;
    private readonly int _titleFontSize = 13;
    private readonly int _topMargin = -35;
    private readonly int _translationX = 0;
    private readonly int _reverseTranslationX = -10;
    private readonly Color focusedColor = Color.FromArgb("#f79520");
    private readonly Color unfocusedColor = Color.FromArgb("#82849c");
    private readonly Color unfocusedBorderColor = Color.FromArgb("#c9c9c9");

    public FloatingLabelEntry()
    {
        InitializeComponent();
        LabelTitle.TranslationX = _translationX;
        LabelTitle.FontSize = _placeholderFontSize;
    }

    public event EventHandler Completed;

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }
    public static readonly BindableProperty TextProperty =
        BindableProperty.Create(nameof(Text), typeof(string), typeof(string), string.Empty, BindingMode.TwoWay, null, HandleBindingPropertyChangedDelegate);

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }
    public static readonly BindableProperty TitleProperty =
        BindableProperty.Create(nameof(Title), typeof(string), typeof(string), string.Empty, BindingMode.TwoWay, null);

    public ReturnType ReturnType
    {
        get => (ReturnType)GetValue(ReturnTypeProperty);
        set => SetValue(ReturnTypeProperty, value);
    }
    public static readonly BindableProperty ReturnTypeProperty =
        BindableProperty.Create(nameof(ReturnType), typeof(ReturnType), typeof(FloatingLabelEntry), ReturnType.Default);

    public bool IsPassword
    {
        get { return (bool)GetValue(IsPasswordProperty); }
        set { SetValue(IsPasswordProperty, value); }
    }
    public static readonly BindableProperty IsPasswordProperty =
        BindableProperty.Create(nameof(IsPassword), typeof(bool), typeof(FloatingLabelEntry), default(bool));

    public ImageSource EndIcon
    {
        get { return (ImageSource)GetValue(EndIconProperty); }
        set { SetValue(EndIconProperty, value); }
    }
    public static readonly BindableProperty EndIconProperty =
        BindableProperty.Create(nameof(EndIcon), typeof(ImageSource), typeof(FloatingLabelEntry), null);

    public bool IsEndIconVisible
    {
        get { return (bool)GetValue(IsEndIconVisibleProperty); }
        set { SetValue(IsEndIconVisibleProperty, value); }
    }
    public static readonly BindableProperty IsEndIconVisibleProperty =
        BindableProperty.Create(nameof(IsEndIconVisible), typeof(bool), typeof(FloatingLabelEntry), default(bool));

    public IRelayCommand EndIconClickCommand
    {
        get { return (IRelayCommand)GetValue(EndIconClickCommandProperty); }
        set { SetValue(EndIconClickCommandProperty, value); }
    }
    public static readonly BindableProperty EndIconClickCommandProperty = BindableProperty.Create(nameof(EndIconClickCommand), typeof(IRelayCommand), typeof(FloatingLabelEntry), null, BindingMode.TwoWay);

    public Microsoft.Maui.Keyboard Keyboard
    {
        get { return (Microsoft.Maui.Keyboard)GetValue(KeyboardProperty); }
        set { SetValue(KeyboardProperty, value); }
    }
    public static readonly BindableProperty KeyboardProperty =
        BindableProperty.Create(nameof(Keyboard), typeof(Microsoft.Maui.Keyboard), typeof(FloatingLabelEntry), Microsoft.Maui.Keyboard.Default, coerceValue: (o, v) => (Microsoft.Maui.Keyboard)v ?? Microsoft.Maui.Keyboard.Default);

    public new void Focus()
    {
        if (IsEnabled)
        {
            EntryField.Focus();
        }
    }

    private static async void HandleBindingPropertyChangedDelegate(BindableObject bindable, object oldValue, object newValue)
    {
        var control = bindable as FloatingLabelEntry;
        if (control.EntryField.IsFocused)
            return;

        if (!string.IsNullOrEmpty((string)newValue))
            await control.TransitionToTitle(false);
        else
            await control.TransitionToPlaceholder(false);
    }

    private async void HandleEntryFocus(object sender, FocusEventArgs e)
    {
        Frm.Stroke = focusedColor;
        if (string.IsNullOrEmpty(Text))
            await TransitionToTitle(true);
    }

    private async void HandleEntryUnFocus(object sender, FocusEventArgs e)
    {
        Frm.Stroke = unfocusedBorderColor;
        if (string.IsNullOrEmpty(Text))
            await TransitionToPlaceholder(true);
    }

    private async Task TransitionToTitle(bool animated)
    {
        var reverseTranslationX = CultureInfo.CurrentCulture.TextInfo.IsRightToLeft ? -1 * _reverseTranslationX : _reverseTranslationX;
        if (animated)
        {
            var t1 = LabelTitle.TranslateTo(reverseTranslationX, _topMargin, 100);
            var t2 = SizeTo(_titleFontSize);
            await Task.WhenAll(t1, t2);
            LabelTitle.TextColor = focusedColor;
        }
        else
        {
            LabelTitle.TranslationX = reverseTranslationX;
            LabelTitle.TranslationY = -30;
            LabelTitle.FontSize = _titleFontSize;
        }
    }

    private async Task TransitionToPlaceholder(bool animated)
    {
        if (animated)
        {
            var t1 = LabelTitle.TranslateTo(_translationX, 0, 100);
            var t2 = SizeTo(_placeholderFontSize);
            await Task.WhenAll(t1, t2);
            LabelTitle.TextColor = unfocusedColor;
        }
        else
        {
            LabelTitle.TranslationX = _translationX;
            LabelTitle.TranslationY = 0;
            LabelTitle.FontSize = _placeholderFontSize;
        }
    }

    private void HandleTapLabelTitle(object sender, EventArgs e)
    {
        if (IsEnabled)
            EntryField.Focus();
    }

    private Task SizeTo(int fontSize)
    {
        var taskCompletionSource = new TaskCompletionSource<bool>();

        // setup information for animation
        void callback(double input) { LabelTitle.FontSize = input; }
        double startingHeight = LabelTitle.FontSize;
        double endingHeight = fontSize;
        uint rate = 5;
        uint length = 100;
        Easing easing = Easing.Linear;

        // now start animation with all the setup information
        LabelTitle.Animate("invis", callback, startingHeight, endingHeight, rate, length, easing, (v, c) => taskCompletionSource.SetResult(c));

        return taskCompletionSource.Task;
    }

    private void HandleEntryCompleted(object sender, EventArgs e)
    {
        Completed?.Invoke(this, e);
    }

    protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        base.OnPropertyChanged(propertyName);

        if (propertyName == nameof(IsEnabled))
            EntryField.IsEnabled = IsEnabled;
    }

    private void EndIconClick_Clicked(object sender, EventArgs e)
    {
        EndIconClickCommand?.Execute(default);
    }


}
