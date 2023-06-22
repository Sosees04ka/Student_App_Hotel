using HotelContracts.BindingModels;
using HotelContracts.SearchModels;
using HotelContracts.ViewModels;

namespace HotelContracts.StoragesContracts
{
    public interface IMealPlanStorage
    {
        List<MealPlanViewModel> GetFullList();

        List<MealPlanViewModel> GetFilteredList(MealPlanSearchModel model);

        MealPlanViewModel? GetElement(MealPlanSearchModel model);

        MealPlanViewModel? Insert(MealPlanBindingModel model);

        MealPlanViewModel? Update(MealPlanBindingModel model);

        MealPlanViewModel? Delete(MealPlanBindingModel model);
    }
}
