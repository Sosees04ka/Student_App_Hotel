using HotelBusinessLogic.BusinessLogics;
using HotelContracts.BusinessLogicsContracts;
using HotelContracts.StoragesContracts;
using HotelDataBaseImplement.Implemets;
using Microsoft.OpenApi.Models;
using HotelBusinessLogic.MailWorker;
using HotelContracts.BindingModels;
using HotelBusinessLogic.OfficePackage;
using HotelBusinessLogic.OfficePackage.Implements;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.SetMinimumLevel(LogLevel.Trace);
builder.Logging.AddLog4Net("log4net.config");

// Add services to the container.
builder.Services.AddTransient<IOrganiserStorage, OrganiserStorage>();
builder.Services.AddTransient<IMealPlanStorage, MealPlanStorage>();
builder.Services.AddTransient<IMemberStorage, MemberStorage>();
builder.Services.AddTransient<IConferenceStorage, ConferenceStorage>();

builder.Services.AddTransient<IOrganiserLogic, OrganiserLogic>();
builder.Services.AddTransient<IMealPlanLogic, MealPlanLogic>();
builder.Services.AddTransient<IMemberLogic, MemberLogic>();
builder.Services.AddTransient<IConferenceLogic, ConferenceLogic>();
builder.Services.AddTransient<IReportOrganiserLogic, ReportLogicOrganiser>();
builder.Services.AddTransient<IReportHeadwaiterLogic, ReportLogicHeadwaiter>();

builder.Services.AddTransient<AbstractSaveToExcelOrganiser, SaveToExcelOrganiser>();
builder.Services.AddTransient<AbstractSaveToWordOrganiser, SaveToWordOrganiser>();
builder.Services.AddTransient<AbstractSaveToPdfOrganiser, SaveToPdfOrganiser>();

builder.Services.AddTransient<AbstractSaveToExcelHeadwaiter, SaveToExcelHeadwaiter>();
builder.Services.AddTransient<AbstractSaveToWordHeadwaitre, SaveToWordHeadwaiter>();
builder.Services.AddTransient<AbstractSaveToPdfHeadwaiter, SaveToPdfHeadwaiter>();

builder.Services.AddTransient<IHeadwaiterStorage, HeadwaiterStorage>();
builder.Services.AddTransient<IDinnerStorage, DinnerStorage>();
builder.Services.AddTransient<IRoomStorage, RoomStorage>();
builder.Services.AddTransient<IConferenceBookingStorage, ConferenceBookingStorage>();

builder.Services.AddTransient<IHeadwaiterLogic, HeadwaiterLogic>();
builder.Services.AddTransient<IDinnerLogic, DinnerLogic>();
builder.Services.AddTransient<IRoomLogic, RoomLogic>();
builder.Services.AddTransient<IConferenceBookingLogic, ConferenceBookingLogic>();

builder.Services.AddSingleton<AbstractMailWorker, MailKitWorker>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "HotelRestApi",
        Version = "v1"
    });
});

var app = builder.Build();

var mailSender = app.Services.GetService<AbstractMailWorker>();

mailSender?.MailConfig(new MailConfigBindingModel
{
    MailLogin = builder.Configuration?.GetSection("MailLogin")?.Value?.ToString() ?? string.Empty,
    MailPassword = builder.Configuration?.GetSection("MailPassword")?.Value?.ToString() ?? string.Empty,
    SmtpClientHost = builder.Configuration?.GetSection("SmtpClientHost")?.Value?.ToString() ?? string.Empty,
    SmtpClientPort = Convert.ToInt32(builder.Configuration?.GetSection("SmtpClientPort")?.Value?.ToString()),
    PopHost = builder.Configuration?.GetSection("PopHost")?.Value?.ToString() ?? string.Empty,
    PopPort = Convert.ToInt32(builder.Configuration?.GetSection("PopPort")?.Value?.ToString())
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "HotelRestApi v1"));
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();