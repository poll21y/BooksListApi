using BooksListApi.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ϳ��������� In-Memory ����
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseInMemoryDatabase("BookList"));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllersWithViews();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Swagger ��������� ���� � ����� ��������
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "BookStoreApi");
        c.RoutePrefix = "swagger"; // Swagger ���� ��������� �� /swagger
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


// ?? ����������� ���������� ����
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    if (!context.Books.Any())
    {
        context.Books.AddRange(
            new Book
            {
                Title = "̳���",
                Author = "������� ϳ����������",
                Year = 1927,
                Genre = "�����"
            },
            new Book
            {
                Title = "���������",
                Author = "���� ��������",
                Year = 1944,
                Genre = "�������������"
            }
        );

        context.SaveChanges();
    }
}

app.Run();
