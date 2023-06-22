using DocumentFormat.OpenXml.VariantTypes;
using HotelBusinessLogic.BusinessLogics;
using HotelBusinessLogic.OfficePackage;
using HotelBusinessLogic.OfficePackage.Implements;
using HotelContracts.BusinessLogicsContracts;
using HotelContracts.StoragesContracts;
using HotelDataBaseImplement.Implemets;
using HotelOrganiserApp;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddTransient<IReportOrganiserLogic, ReportLogicOrganiser>();
builder.Services.AddTransient<IMemberStorage, MemberStorage>();
builder.Services.AddTransient<IMealPlanStorage, MealPlanStorage>();
builder.Services.AddTransient<IConferenceStorage, ConferenceStorage>();
builder.Services.AddTransient<AbstractSaveToExcelOrganiser, SaveToExcelOrganiser>();
builder.Services.AddTransient<AbstractSaveToPdfOrganiser, SaveToPdfOrganiser>();
builder.Services.AddTransient<AbstractSaveToWordOrganiser, SaveToWordOrganiser>();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();
APIClient.Connect(builder.Configuration);

// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
//}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
