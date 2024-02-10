using Microsoft.AspNetCore.Http.Features;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();

builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = 4 * 1600 * 1200;
});

builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 4 * 1600 * 1200;
});

builder.Services.Configure<IISServerOptions>(options =>
{
    options.MaxRequestBodySize = 4 * 1600 * 1200;
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(policyBuilder => policyBuilder.AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader()
);
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers(); 

app.Run();