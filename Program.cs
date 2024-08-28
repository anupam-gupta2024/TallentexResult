using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using Amazon.S3;
using System.Drawing;
using TallentexResult.Entities;
using TallentexResult.Interface;
using TallentexResult.Models;
using TallentexResult.Repository;
using TallentexResult.Servies;

var builder = WebApplication.CreateBuilder(args);

// Add Session
//builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

#region Session and cookies setting

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(option =>
{
    option.IOTimeout = TimeSpan.FromMinutes(30);    // You can set Time in seconds, minutes
});

#endregion

#region Configure AWS
// COnfigure AWS S3 Client
//var awsSettings = builder.Configuration.GetSection(key: "AWS");
//var credential = new BasicAWSCredentials(awsSettings[key: "AccessKeyId"], awsSettings[key: "SecretAccessKey"]);
var credential = new BasicAWSCredentials(
    builder.Configuration.GetConnectionString("AWS:AccessKeyId"), 
    builder.Configuration.GetConnectionString("AWS:SecretAccessKey")
    );

// Configure AWS options
var awsOptions = builder.Configuration.GetAWSOptions();
awsOptions.Credentials = credential;
awsOptions.Region = Amazon.RegionEndpoint.APSouth1;
builder.Services.AddDefaultAWSOptions(awsOptions);

// Add the AWS S3 service
builder.Services.AddAWSService<IAmazonS3>();

#endregion

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

// Get our SQL Connection
builder.Services.Configure<DataConnection>(builder.Configuration.GetSection("DataConnection"));

// Add SMS Setting
builder.Services.Configure<SMSSetting>(builder.Configuration.GetSection("SMSSettingGupshup"));


//// Add AWS
//builder.Services.AddDefaultAWSOptions(builder.Configuration.GetAWSOptions());
//builder.Services.AddAWSService<IAmazonS3>();

// Add Dependency Injection
builder.Services.AddScoped<IDataService, DataService>();
builder.Services.AddTransient<ISMSService, SMSService>();
builder.Services.AddScoped<IAmazonUploader, AmazonUploader>();
builder.Services.AddSingleton<IDropdown, Dropdown>();
builder.Services.AddSingleton<IAdmissionInfo, AdmissionInfo>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
