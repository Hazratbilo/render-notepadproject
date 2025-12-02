using Notepad.Models;

namespace Notepad.Interface.Repository
{
    public interface INoteRepository
    {
        public Task<Note> AddNote(Note note);
        public Task<bool> DeleteNote(int id);
        public Task<Note> GetNoteById(int id);

        Task<List<Note>> GetAllNotes();

        Task<Note> UpdateNote(Note note);
    }
}
