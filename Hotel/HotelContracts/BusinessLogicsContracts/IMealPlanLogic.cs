using HotelContracts.BindingModels;
using HotelContracts.SearchModels;
using HotelContracts.ViewModels;
using HotelDataModels.Models;

namespace HotelContracts.BusinessLogicsContracts
{
    public interface IMealPlanLogic
    {
        List<MealPlanViewModel>? ReadList(MealPlanSearchModel? model);
        MealPlanViewModel? ReadElement(MealPlanSearchModel model);
        bool AddMemberToMealPlan(MealPlanSearchModel model, IMemberModel member);
        bool Create(MealPlanBindingModel model);
        bool Update(MealPlanBindingModel model);
        bool Delete(MealPlanBindingModel model);
    }
}
