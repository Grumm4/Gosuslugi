using System.ComponentModel.DataAnnotations.Schema;

namespace Gosuslugi
{
    public class OrderModel
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public decimal? Price { get; set; }
        public string? Date { get; set; }
        public string? Place { get; set; }
        public string? Contacts { get; set; }
        public string? Comments { get; set;}
        public int? AcceptedUserId { get; set; }
        
        [NotMapped]
        public string? ExecutorName { get; set; }
    }
}