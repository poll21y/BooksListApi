namespace BooksListApi.Models
{
    public class Book
    {
        public int Id { get; set; } // Унікальний ідентифікатор
        public string Title { get; set; } // Назва
        public string Author { get; set; } // Автор
        public int Year { get; set; } // Рік видання
        public string Genre { get; set; } // Жанр
    }

}
