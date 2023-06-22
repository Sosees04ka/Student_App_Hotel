using HotelBusinessLogic.OfficePackage;
using HotelBusinessLogic.OfficePackage.HelperModels;
using HotelContracts.BindingModels;
using HotelContracts.BusinessLogicsContracts;
using HotelContracts.SearchModels;
using HotelContracts.StoragesContracts;
using HotelContracts.ViewModels;
using HotelDataBaseImplement;
using HotelDataBaseImplement.Models;

namespace HotelBusinessLogic.BusinessLogics
{
    public class ReportLogicOrganiser : IReportOrganiserLogic
    {
        private readonly IMealPlanStorage _mealPlanStorage;
        private readonly IMemberStorage _memberStorage;
        private readonly IConferenceStorage _conferenceStorage;
        private readonly AbstractSaveToExcelOrganiser _saveToExcel;
        private readonly AbstractSaveToWordOrganiser _saveToWord;
        private readonly AbstractSaveToPdfOrganiser _saveToPdf;
        public ReportLogicOrganiser(IMealPlanStorage mealPlanStorage, IMemberStorage memberStorage, IConferenceStorage conferenceStorage, AbstractSaveToExcelOrganiser saveToExcel, AbstractSaveToWordOrganiser saveToWord, AbstractSaveToPdfOrganiser saveToPdf)
        {
            _mealPlanStorage = mealPlanStorage;
            _memberStorage = memberStorage;
            _conferenceStorage = conferenceStorage;
            _saveToExcel = saveToExcel;
            _saveToWord = saveToWord;
            _saveToPdf = saveToPdf;
        }
        public List<ReportMemberConferenceViewModel> GetMemberConference(List<int> Ids)
        {
            if (Ids == null)
            {
                return new List<ReportMemberConferenceViewModel>();
            }
            var conferences = _conferenceStorage.GetFullList();
            List<MemberViewModel> members = new List<MemberViewModel>();
            foreach (var memId in Ids)
            {
                var res = _memberStorage.GetElement(new MemberSearchModel { Id = memId });
                if (res != null)
                {
                    members.Add(res);
                }
            }
            var list = new List<ReportMemberConferenceViewModel>();
            foreach (var member in members)
            {
                var record = new ReportMemberConferenceViewModel
                {
                    MemberFIO = member.MemberFIO,
                    Conferences = new List<Tuple<string, DateTime>>()
                };
                foreach (var conference in conferences)
                {
                    if (conference.ConferenceMembers.ContainsKey(member.Id))
                    {
                        record.Conferences.Add(new Tuple<string, DateTime>(conference.ConferenceName, conference.StartDate));
                    }
                }
                list.Add(record);
            }
            return list;
        }

        public List<ReportMembersViewModel> GetMembers(ReportBindingModel model)
        {
            var listAll = new List<ReportMembersViewModel>();

            var listСonferences = _conferenceStorage.GetFilteredList(new ConferenceSearchModel
            {
                OrganiserId = model.OrganiserId,
                DateFrom = model.DateFrom,
                DateTo = model.DateTo
            });

            foreach (var conference in listСonferences)
            {
                foreach (var m in conference.ConferenceMembers.Values)
                {
                    listAll.Add(new ReportMembersViewModel
                    {
                        StartDate = conference.StartDate,
                        ConferenceName = conference.ConferenceName,
                        MemberFIO = m.MemberFIO
                    });
                }
            }

            var listMealPlans = _mealPlanStorage.GetFilteredList(new MealPlanSearchModel
            {
                OrganiserId = model.OrganiserId,
            });

            foreach (var mealPlan in listMealPlans)
            {
                foreach (var mp in mealPlan.MealPlanMembers.Values)
                {
                    listAll.Add(new ReportMembersViewModel
                    {
                        MemberFIO = mp.MemberFIO,
                        MealPlanName = mealPlan.MealPlanName,
                        MealPlanPrice = mealPlan.MealPlanPrice
                    });
                }
            }

            return listAll;
        }

        public void SaveMemberConferenceToExcelFile(ReportBindingModel model)
        {
            _saveToExcel.CreateReport(new ExcelInfoOrganiser
            {
                FileName = model.FileName,
                Title = "Список конференций",
                MemberConferences = GetMemberConference(model.Ids)
            });
        }

        public void SaveMemberConferenceToWordFile(ReportBindingModel model)
        {
            _saveToWord.CreateDoc(new WordInfoOrganiser
            {
                FileName = model.FileName,
                Title = "Список конференций",
                MemberConferences = GetMemberConference(model.Ids)
            }) ;
        }

        public void SaveMembersToPdfFile(ReportBindingModel model)
        {
            if (model.DateFrom == null)
            {
                throw new ArgumentException("Дата начала не задана");
            }

            if (model.DateTo == null)
            {
                throw new ArgumentException("Дата окончания не задана");
            }
            _saveToPdf.CreateDoc(new PdfInfoOrganiser
            {
                FileName = model.FileName,
                Title = "Список участников",
                DateFrom = model.DateFrom!.Value,
                DateTo = model.DateTo!.Value,
                Members = GetMembers(model)
            });
        }
    }
}
