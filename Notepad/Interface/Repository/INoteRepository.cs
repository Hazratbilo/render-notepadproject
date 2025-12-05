using Notepad.Models;

namespace Notepad.Interface.Repository
{
    public interface INoteRepository
    {
        public Task<Note> AddNote(Note note);
        public Task<bool> DeleteNote(int id);
        public Task<Note> GetNoteById(int id);
        public Task<List<Note>> GetNotesByDeviceId(string deviceId);
        Task<List<Note>> GetAllNotes();
        Task<Note> GetItemsByIdAsync(int id);


        Task<Note> UpdateNote(Note note);
    }
}
