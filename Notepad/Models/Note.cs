namespace Notepad.Models
{
   
    public class Note
    {
      
        public int Id { get; set; }

      
       public string Tittle { get; set; } = string.Empty;

   
        public string Content { get; set; } = string.Empty;

    
        public DateTime DateCreated { get; set; }


        public string? DeviceId { get; set; }= string.Empty;
    }
}
