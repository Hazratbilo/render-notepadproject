using System.ComponentModel.DataAnnotations;

namespace Notepad.Models.DTOs.Note
{
    public class NoteDTO
    {
        public int Id { get; set; }


        public string? Tittle { get; set; }


        public string? Content { get; set; }


        public DateTime DateCreated { get; set; }


        public string? DeviceId { get; set; } = string.Empty;
    }
    public class CreateNoteRequestModel
    {
        [Required]
        //[Length(3,50)]
        public string Tittle { get; set; }

        [Required]
        //[Length(4,10000)]
        public string Content { get; set; } 
        public string? DeviceId { get; set; }
    }
    public class UpdateNoteRequestModel
    {
        //public int Id { get; set; }
        [Required]
        [StringLength(maximumLength: 50, MinimumLength = 3)]
        public string Tittle { get; set; } 
        [Required]
        [StringLength(maximumLength: 2000, MinimumLength = 2)]
        public string Content { get; set; }
    }
}
