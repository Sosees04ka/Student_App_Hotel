using HotelContracts.BindingModels;
using HotelContracts.BusinessLogicsContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DocumentFormat.OpenXml.Presentation;
using DocumentFormat.OpenXml.Spreadsheet;
using HotelBusinessLogic.MailWorker;

namespace HotelRestApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ReportController : Controller
    {
        private readonly ILogger _logger;
        private readonly IReportOrganiserLogic _reportOrganiserLogic;
        private readonly IReportHeadwaiterLogic _reportHeadwaiterLogic;
        private readonly AbstractMailWorker _mailWorker;
        public ReportController(ILogger<ReportController> logger, AbstractMailWorker mailWorker, IReportOrganiserLogic reportOrganiserLogic, IReportHeadwaiterLogic reportHeadwaiterLogic)
        {
            _logger = logger;
            _reportOrganiserLogic = reportOrganiserLogic;
            _reportHeadwaiterLogic = reportHeadwaiterLogic;
            _mailWorker = mailWorker;
        }

        [HttpPost]
        public void CreateReportToPdfFile(ReportBindingModel model)
        {
            try
            {
                _reportOrganiserLogic.SaveMembersToPdfFile(new ReportBindingModel
                {
                    DateFrom = model.DateFrom,
                    DateTo = model.DateTo,
                    OrganiserId = model.OrganiserId,
                    FileName = "C:\\ReportsCourseWork\\pdffile.pdf",
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка создания отчета");
                throw;
            }
        }

        [HttpPost]
        public void SendPdfToMail(MailSendInfoBindingModel model)
        {
            try
            {
                _mailWorker.MailSendAsync(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка отправки письма");
                throw;
            }
        }

        [HttpPost]
        public void CreateReportToWordFile(ReportBindingModel model)
        {
            try
            {
                _reportOrganiserLogic.SaveMemberConferenceToWordFile(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка создания отчета");
                throw;
            }
        }

        [HttpPost]
        public void CreateReportToExcelFile(ReportBindingModel model)
        {
            try
            {
                _reportOrganiserLogic.SaveMemberConferenceToExcelFile(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка создания отчета");
                throw;
            }
        }


        [HttpPost]
        public void CreateHeadwaiterReportToWordFile(ReportHeadwaiterBindingModel model)
        {
            try
            {
                _reportHeadwaiterLogic.SaveDinnerRoomToWordFile(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка создания отчета");
                throw;
            }
        }
        [HttpPost]
        public void CreateHeadwaiterReportToExcelFile(ReportHeadwaiterBindingModel model)
        {
            try
            {
                _reportHeadwaiterLogic.SaveDinnerRoomToExcelFile(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка создания отчета");
                throw;
            }
        }

        [HttpPost]
        public void CreateHeadwaiterReportToPdfFile(ReportHeadwaiterBindingModel model)
        {
            try
            {
                _reportHeadwaiterLogic.SaveDinnersToPdfFile(new ReportHeadwaiterBindingModel
                {
                    FileName = "C:\\ReportsCourseWork\\pdffile.pdf",
                    DateFrom = model.DateFrom,
                    DateTo = model.DateTo,
                    HeadwaiterId = model.HeadwaiterId,
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка создания отчета");
                throw;
            }
        }
    }
}
