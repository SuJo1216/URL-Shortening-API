using URLShortening.Models;
using Microsoft.EntityFrameworkCore;
using URLShortening;
using URLShortening.Services;
using URLShortening.Entites;
using Microsoft.AspNetCore.Hosting.Server;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

//services.AddControllers();
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDBContext>(o =>
o.UseSqlServer("Server=DESKTOP-PV54501\\MSSQLSERVER01;Database=ShortenURL;User Id=susan;Password=Susan1234;TrustServerCertificate=Yes;Trusted_Connection=Yes"));


builder.Services.AddScoped<URLShorteningServices>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapPost("api/shorten", async (
    ShortenURLRequest request,
    URLShorteningServices uRLShorteningServices,
    ApplicationDBContext DBContext,
    HttpContext httpContext) => 
    {
        if (!Uri.TryCreate(request.Url, UriKind.Absolute, out _))
        {
            return Results.BadRequest("The specified URL is invalid.");
        }
        var code=await uRLShorteningServices.GenerateUniqueCode();

        var shortenedurl = new ShortenURL
        {
            Id = Guid.NewGuid(),
            LongURL = request.Url,
            Code = code,
            ShortURL = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}/api/{code}",
            CreatedOn = DateTime.Now

        };
        DBContext.ShortenURL.Add(shortenedurl);
        await DBContext.SaveChangesAsync();
        return Results.Ok(shortenedurl.ShortURL);
    });

app.MapGet("api/{code}", async (string code, ApplicationDBContext _dbContext) =>
{
    var shortenurl = await _dbContext.ShortenURL
    .FirstOrDefaultAsync(s => s.Code == code);
    if(shortenurl is null)
    {
        return Results.NotFound();
    }
    return Results.Redirect(shortenurl.LongURL);
});

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseCors("AllowAll");
app.MapControllers();

app.Run();




