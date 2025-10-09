using DnD_Character_Sheet.Factories;
using DnD_Character_Sheet.Models;
using DnD_Character_Sheet.Pages;
using System.Globalization;

namespace DnD_Character_Sheet.Services;

public static class GenerateCarouselViewService
{
    public static CarouselView LoadCharacterIntoView(CharacterSheet character)
    {
        var items = new List<ViewFactoryWrapper>
        {
            new() { CreateView = () => HomePageBuilder.Build(character) },
            new() { CreateView = () => SkillsPageBuilder.Build(character) },
            new() { CreateView = () => FeaturesPageBuilder.Build(character) },
            new() { CreateView = () => WeaponsPageBuilder.Build(character) },
            new() { CreateView = () => InventoryPageBuilder.Build(character) },
            new() { CreateView = () => BackstoryPageBuilder.Build(character) },
        };

        // Conditionally add spells page
        if (character.SpellCasting != null)
        {
            items.Add(new ViewFactoryWrapper
            {
                CreateView = () => SpellsPageBuilder.Build(character)
            });
        }

        return new CarouselView
        {
            ItemsSource = items,
            IsBounceEnabled = true,
            IsSwipeEnabled = true,
            HorizontalScrollBarVisibility = ScrollBarVisibility.Never,
            VerticalScrollBarVisibility = ScrollBarVisibility.Never,
            HeightRequest = Application.Current.MainPage.Height, // Optional if layout handles it
            ItemTemplate = new DataTemplate(() =>
            {
                var contentView = new ContentView();
                contentView.SetBinding(ContentView.ContentProperty, new Binding("CreateView", BindingMode.OneTime, new FuncToViewConverter()));
                return contentView;
            })
        };
    }

    public class FuncToViewConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Func<View> factory)
                return factory.Invoke();
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
