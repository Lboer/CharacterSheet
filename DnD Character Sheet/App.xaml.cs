namespace DnD_Character_Sheet;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
    }

    // Override CreateWindow to set up the main window and page
    protected override Window CreateWindow(IActivationState activationState)
    {
        var window = new Window(new AppShell());

        // Optional: Hook into Created event if you need to access Page safely
        window.Created += (s, e) =>
        {
            var page = window.Page;
            if (page is AppShell shell)
            {
                // Example: perform initialization or update UI
                System.Diagnostics.Debug.WriteLine("Main window and AppShell are ready!");
            }
        };

        return window;
    }
}
