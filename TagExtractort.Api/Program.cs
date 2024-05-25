using Hangfire;
using TagExtractort.Application.Interfaces;
using TagExtractort.Application.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IPageService, PageService>();
builder.Services.AddScoped<IKafkaProduceService, KafkaProduceService>();
builder.Services.AddScoped<IKafkaConsumeService, KafkaConsumeService>();
builder.Services.AddScoped<IKeywordTargetingJobBase, KeywordTargetingJobBase>();
builder.Services.AddScoped<IElasticSearchService, ElasticSearchService>();

builder.Services.AddHangfire(config => config.UseSqlServerStorage("Server=localhost;Database=HangFireDb;Integrated Security=True;TrustServerCertificate=True;"));
builder.Services.AddHangfireServer();

builder.Services.AddSingleton<CancellationTokenSource>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseHangfireDashboard();

app.Run();
