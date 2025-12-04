using Microsoft.EntityFrameworkCore;
using Notepad.Context;
using Notepad.Interface.Repository;
using Notepad.Models;

namespace Notepad.Implementation.Repository
{
    public class NoteRepository : INoteRepository
    {
        private readonly NoteContext _context;

        public NoteRepository(NoteContext context)
        {
            _context = context;

        }
        public async Task<Note> AddNote(Note note)
        {
            await _context.AddAsync(note);
            await _context.SaveChangesAsync();
            return note;
        }

        public async Task<bool> DeleteNote(int id)
        {
            var note = await _context.Notes.FindAsync(id);
            _context.Notes.Remove(note);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Note>> GetAllNotes()
        {
            var note = await _context.Notes.ToListAsync();
            return note;
        }

        public async Task<Note> GetNoteById(int id)
        {
            return await _context.Notes.FirstOrDefaultAsync(x => x.Id == id);
            
        }

        public async Task<List<Note>> GetNotesByDeviceId(string deviceId)
        {
           return await _context.Notes.Where(x => x.DeviceId == deviceId)
                .OrderByDescending(x => x.DateCreated)
               . ToListAsync();
        }

        public async Task<Note> UpdateNote(Note note)
        {
            _context.Notes.Update(note);
            await _context.SaveChangesAsync();
            return note;
        }
    }
}
