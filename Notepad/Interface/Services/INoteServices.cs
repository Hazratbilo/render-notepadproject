using Notepad.Models;
using Notepad.Models.DTOs;
using Notepad.Models.DTOs.Note;

namespace Notepad.Interface.Services
{
    public interface INoteServices
    {
        public Task<BaseResponse<bool>> AddNote(CreateNoteRequestModel request);
        public Task<BaseResponse<bool>> DeleteNote(int id);
        public Task<BaseResponse<NoteDTO>> GetNoteById(int id);
        public Task<BaseResponse<IEnumerable<NoteDTO>>> GetNotesByDeviceId(string deviceId);
        public Task<NoteDTO> GetNote(int id);
        Task<BaseResponse<IEnumerable<NoteDTO>>> GetAllNoteAsync();
        //Task<BaseResponse<NoteDTO>> UpdateNote(int id, UpdateNoteRequestModel request);

        Task<NoteDTO> UpdateNote(int id, UpdateNoteRequestModel request);
        public Task<BaseResponse<bool>> CopyNoteAsync(int id);
        Task AddNote(Note copy);
    }
}
