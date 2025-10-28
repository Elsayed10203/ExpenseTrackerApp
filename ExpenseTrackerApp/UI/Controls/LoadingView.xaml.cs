using ExpenseTrackerApp.Languages;

namespace ExpenseTrackerApp.UI.Controls;

public partial class LoadingView : StackLayout
{
	public LoadingView()
	{
		InitializeComponent();
	}

    public string LoadingText
    {
        get { return (string)GetValue(LoadingTextProperty); }
        set { SetValue(LoadingTextProperty, value); }
    }

    public bool ShowLoadingText
    {
        get { return (bool)GetValue(ShowLoadingTextProperty); }
        set { SetValue(ShowLoadingTextProperty, value); }
    }


    public static readonly BindableProperty LoadingTextProperty = BindableProperty.Create(
        nameof(LoadingText), typeof(string), typeof(LoadingView), LanguagesResources.Loading, BindingMode.TwoWay);


    public static readonly BindableProperty ShowLoadingTextProperty = BindableProperty.Create(
        nameof(ShowLoadingText), typeof(bool), typeof(LoadingView), true, BindingMode.TwoWay);
}