using GarageService.GarageLib.Models;
using System.Windows.Input;

namespace GarageService.GarageApp.Views;

public partial class DashBoardTitle : FlexLayout
{
    public static readonly BindableProperty TitleProperty = BindableProperty.Create(
            nameof(Title),
            typeof(string),
            typeof(DashBoardTitle),
            string.Empty,
            propertyChanged: OnTitleChanged);

    public static readonly BindableProperty IsPremiumProperty =
        BindableProperty.Create(
            nameof(IsPremium),
            typeof(bool),
            typeof(DashBoardTitle),
            false,
            defaultBindingMode: BindingMode.OneWay,
            propertyChanged: OnIsPremiumChanged);

    public static readonly BindableProperty SaveCommandProperty = BindableProperty.Create(
        nameof(SaveCommand),
        typeof(ICommand),
        typeof(DashBoardTitle));



    public static readonly BindableProperty PremuimCommandProperty = BindableProperty.Create(
        nameof(PremuimCommand),
        typeof(ICommand),
        typeof(DashBoardTitle));

    public static readonly BindableProperty GaragePremiumRegistrationProperty = BindableProperty.Create(
        nameof(GaragePremiumRegistration),
        typeof(GaragePremiumRegistration),
        typeof(DashBoardTitle),
        default(GaragePremiumRegistration),
        propertyChanged: OnGarageProfileChanged);

    public static readonly BindableProperty BackCommandProperty = BindableProperty.Create(
    nameof(BackCommand),
    typeof(ICommand),
    typeof(TitleView));

    public DashBoardTitle()
    {
        InitializeComponent();
    }

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }
    public bool IsPremium
    {
        get => (bool)GetValue(IsPremiumProperty);
        set => SetValue(IsPremiumProperty, value);
    }
    public ICommand SaveCommand
    {
        get => (ICommand)GetValue(SaveCommandProperty);
        set => SetValue(SaveCommandProperty, value);
    }

    public ICommand BackCommand
    {
        get => (ICommand)GetValue(BackCommandProperty);
        set => SetValue(BackCommandProperty, value);
    }

    public ICommand PremuimCommand
    {
        get => (ICommand)GetValue(PremuimCommandProperty);
        set => SetValue(PremuimCommandProperty, value);
    }

    public GaragePremiumRegistration GaragePremiumRegistration
    {
        get => (GaragePremiumRegistration)GetValue(GaragePremiumRegistrationProperty);
        set => SetValue(GaragePremiumRegistrationProperty, value);
    }

    private static void OnTitleChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (DashBoardTitle)bindable;
        control.TitleLabel.Text = (string)newValue;
    }

    private static void OnIsPremiumChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (DashBoardTitle)bindable;
        // Optional: Add any logic that should run when IsPremium changes
    }
    private static void OnGarageProfileChanged(BindableObject bindable, object oldValue, object newValue)
    {
        // UI is bound directly to GarageProfile via XAML (see DataTriggers). Keep for future logic.
    }
}