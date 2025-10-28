using ExpenseTrackerApp.Languages;
using ExpenseTrackerApp.UI.FontAwesome;
using ExpenseTrackerApp.UI.Modules.charts;
using ExpenseTrackerApp.UI.Modules.ExpenseList;
using FreshMvvm.Maui;
using FreshMvvm.Maui;
#if IOS
using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Platform;
using System.Collections.Specialized;
#endif
#if ANDROID
using Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific;
#endif
using MauiControls = Microsoft.Maui.Controls;
 
namespace ExpenseTrackerApp.UI.Modules.TabbedHome;

public partial class HomeTabbedPage : Microsoft.Maui.Controls.TabbedPage, IFreshNavigationService
{
	public HomeTabbedPage()
	{
		InitializeComponent();

        BackgroundColor = (Color)MauiControls.Application.Current.Resources["PageBG"];
        BarBackgroundColor = (Color)MauiControls.Application.Current.Resources["TabBarBgColor"];
        SelectedTabColor = (Color)MauiControls.Application.Current.Resources["Accent"];
        BarTextColor = (Color)MauiControls.Application.Current.Resources["SubTextColor"];
        UnselectedTabColor = (Color)MauiControls.Application.Current.Resources["SubTextColor"];

#if ANDROID
        On<Microsoft.Maui.Controls.PlatformConfiguration.Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);
        On<Microsoft.Maui.Controls.PlatformConfiguration.Android>().SetIsSwipePagingEnabled(false);
#endif

        var walletIcon = new FontImageSource() { FontFamily = "FAPR", Glyph = Glyph.Wallet, Size = 25, Color = Colors.Gray };
        var chartIcon = new FontImageSource() { FontFamily = "FAPR", Glyph = Glyph.ChartBar, Size = 25, Color = Colors.Gray };

        AddTab(typeof(ExpenseListPageModel), LanguagesResources.Expenses, walletIcon);
        AddTab(typeof(ChartPageModel), LanguagesResources.Chart, chartIcon);

    }
    private readonly List<Page> _tabs = [];

    protected Page AddTab(Type pageModelType, string title, ImageSource icon, object data = null)
    {
        var page = FreshPageModelResolver.ResolvePageModel(pageModelType, data);
        page.GetModel().SetCurrentNavigationService(this);
        _tabs.Add(page);

        var navigationContainer = CreateContainerPageSafe(page);
        navigationContainer.Title = title;
        navigationContainer.IconImageSource = icon;

        Children.Add(navigationContainer);
        return navigationContainer;
    }

    private Page CreateContainerPageSafe(Page page)
    {
        if (page is NavigationPage || page is FlyoutPage || page is MauiControls.TabbedPage)
            return page;

        return CreateContainerPage(page);
    }

    protected virtual Page CreateContainerPage(Page page)
    {
        return new NavigationPage(page);
    }

   public Task PushPage(Page page, FreshBasePageModel model, bool modal = false, bool animate = true)
    {
        if (modal)
            return CurrentPage.Navigation.PushModalAsync(CreateContainerPageSafe(page));
        return CurrentPage.Navigation.PushAsync(page);
    }

    public Task PopPage(bool modal = false, bool animate = true)
    {
        if (modal)
            return CurrentPage.Navigation.PopModalAsync(animate);
        return CurrentPage.Navigation.PopAsync(animate);
    }

    public Task PopToRoot(bool animate = true)
    {
        return CurrentPage.Navigation.PopToRootAsync(animate);
    }

    public string NavigationServiceName { get; private set; }

    public void NotifyChildrenPageWasPopped()
    {
        foreach (var page in Children)
        {
            if (page is NavigationPage)
                ((NavigationPage)page).NotifyAllChildrenPopped();
        }
    }

    public Task<FreshBasePageModel> SwitchSelectedRootPageModel<T>() where T : FreshBasePageModel
    {
        var page = _tabs.FindIndex(o => o.GetModel().GetType().FullName == typeof(T).FullName);

        if (page > -1)
        {
            CurrentPage = Children[page];
            var topOfStack = CurrentPage.Navigation.NavigationStack.LastOrDefault();
            if (topOfStack != null)
                return Task.FromResult(topOfStack.GetModel());

        }
        return null;
    }





}