using BooksListApi.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Підключення In-Memory бази
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseInMemoryDatabase("BookList"));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllersWithViews();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Swagger доступний лише в режимі розробки
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "BookStoreApi");
        c.RoutePrefix = "swagger"; // Swagger буде доступний за /swagger
    });
}

// Middleware
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Books}/{action=Index}/{id?}");

app.MapControllers();


// ?? Ініціалізація початкових книг
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    if (!context.Books.Any())
    {
        context.Books.AddRange(
            new Book
            {
                Title = "Місто",
                Author = "Валер’ян Підмогильний",
                Year = 1927,
                Genre = "Роман"
            },
            new Book
            {
                Title = "Тигролови",
                Author = "Іван Багряний",
                Year = 1944,
                Genre = "Пригодницький"
            }
        );

        context.SaveChanges();
    }
}

app.Run();
