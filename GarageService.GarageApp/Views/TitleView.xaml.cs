using System.Windows.Input;

namespace GarageService.GarageApp.Views;

public partial class TitleView : FlexLayout
{
    public static readonly BindableProperty TitleProperty = BindableProperty.Create(
            nameof(Title),
            typeof(string),
            typeof(TitleView),
            string.Empty,
            propertyChanged: OnTitleChanged);
    public static readonly BindableProperty PremuimCommandProperty = BindableProperty.Create(
    nameof(PremuimCommand),
    typeof(ICommand),
    typeof(TitleView));

    public static readonly BindableProperty SubmitCommandProperty = BindableProperty.Create(
        nameof(SubmitCommand),
        typeof(ICommand),
        typeof(TitleView));

    public static readonly BindableProperty BackCommandProperty = BindableProperty.Create(
        nameof(BackCommand),
        typeof(ICommand),
        typeof(TitleView));
    public TitleView()
    {
        InitializeComponent();

        // Set default back command if none provided
        if (BackCommand == null)
        {
            BackCommand = new Command(async () =>
            {
                if (Shell.Current?.Navigation != null && Shell.Current.Navigation.NavigationStack.Count > 1)
                {
                    await Shell.Current.GoToAsync("..");
                }
            });
        }
    }

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }
    public ICommand PremuimCommand
    {
        get => (ICommand)GetValue(PremuimCommandProperty);
        set => SetValue(PremuimCommandProperty, value);
    }

    public ICommand SubmitCommand
    {
        get => (ICommand)GetValue(SubmitCommandProperty);
        set => SetValue(SubmitCommandProperty, value);
    }

    public ICommand BackCommand
    {
        get => (ICommand)GetValue(BackCommandProperty);
        set => SetValue(BackCommandProperty, value);
    }

    private static void OnTitleChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (TitleView)bindable;
        control.TitleLabel.Text = (string)newValue;
    }
}