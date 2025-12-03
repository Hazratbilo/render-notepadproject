using iText.StyledXmlParser.Jsoup.Nodes;
using Notepad.Interface.Repository;
using Notepad.Interface.Services;
using Notepad.Models;
using Notepad.Models.DTOs;
using Notepad.Models.DTOs.Note;
using System.Text;
using System.Text.RegularExpressions;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace Notepad.Implementation.Services
{
    public class NoteServices : INoteServices
    {
        private readonly ILogger<NoteServices> _logger;
        private readonly INoteRepository _noteRepository;
        public NoteServices(ILogger<NoteServices> logger, INoteRepository noteRepository)
        {
            _logger = logger;
            _noteRepository = noteRepository;
        }

        public async Task<BaseResponse<bool>> AddNote(CreateNoteRequestModel request)
        {
            if (request == null)
            {
                _logger.LogError("Create Note Failed");
                return new BaseResponse<bool>
                {
                    Message = "Fields cannot be empty",
                    Status = false,
                };
            }


            var newNote = new Note
            {
                Tittle = request.Tittle,
                Content = request.Content,
                DateCreated = DateTime.UtcNow,
                DeviceId = request.DeviceId
            };
            var createNote = await _noteRepository.AddNote(newNote);
            if (createNote == null)
            {
                _logger.LogError("Create Note Failed");
                return new BaseResponse<bool>
                {
                    Message = "Create Note Failed",
                    Status = false
                };
            }
            _logger.LogInformation("Create Note Success");
            return new BaseResponse<bool>
            {
                Message = "Create Note Success",
                Status = true,

            };
        }

        public Task AddNote(Note copy)
        {
            throw new NotImplementedException();
        }

        public async Task<BaseResponse<bool>> CopyNoteAsync(int id)
        {
            
            var note = await _noteRepository.GetNoteById(id);
            if (note == null)
            {
                return new BaseResponse<bool>
                {
                    Message = "Fields cannot be empty",
                    Status = false,
                
                };
            }
            Note copy;
            if(note.Tittle.EndsWith("(Copy)"))
            {
                copy = new Note
                {
                    Tittle = note.Tittle + "",
                    Content = note.Content,
                    DateCreated = DateTime.UtcNow,
                };
            }
            else
            {
                copy = new Note
                {
                    Tittle = note.Tittle + (" (Copy)"),
                    Content = note.Content,
                    DateCreated = DateTime.UtcNow,
                };
            }
            await _noteRepository.AddNote(copy);
            if (CopyNoteAsync == null)
            {
                _logger.LogError("Copy Note Failed");
                return new BaseResponse<bool>
                {
                    Message = "Copy Note Failed",
                    Status = false
                };
            }
            _logger.LogInformation("Copy Note Success");
            return new BaseResponse<bool>
            {
                Message = "Copy Note Success",
                Status = true,

            };
        }


     

        public async Task<BaseResponse<bool>> DeleteNote(int id)
        {
            var getNote = await _noteRepository.GetNoteById(id);
            if (getNote == null)
            {
                _logger.LogError($"Note with id {id} not found");
                return new BaseResponse<bool>
                {
                    Message = $"Note with id {id} not found",
                    Status = false
                };
            }

            var deleteNote = await _noteRepository.DeleteNote(id);
            if (deleteNote)
            {
                _logger.LogInformation("Delete Note Success");
                return new BaseResponse<bool>
                {
                    Message = "Delete Note Success",
                    Status = true
                };
            }
            _logger.LogError("Delete Note Failed");
            return new BaseResponse<bool>
            {
                Message = "Delete Note Failed",
                Status = false
            };
        }

        public async Task<BaseResponse<IEnumerable<NoteDTO>>> GetAllNoteAsync()
        {
            var getAllNotes = await _noteRepository.GetAllNotes();
            if (!getAllNotes.Any())
            {
                _logger.LogError($"No Note found");
                return new BaseResponse<IEnumerable<NoteDTO>>
                {
                    Message = $"No Note found",
                    Status = false,
                };
            }
            _logger.LogInformation("All Note fetched successfully");
            return new BaseResponse<IEnumerable<NoteDTO>>
            {
                Message = "All Note fetched successfully",
                Status = true,
                Data = getAllNotes.Select(dpt => new NoteDTO
                {
                    Id = dpt.Id,
                    Tittle = dpt.Tittle,
                    Content = dpt.Content,
                    DateCreated = dpt.DateCreated,
                }).ToList()
            };
        }

        public async Task<BaseResponse<NoteDTO>> GetNoteById(int id)
        {
            var getNote = await _noteRepository.GetNoteById(id);
            if (getNote == null)
            {
                _logger.LogError($"Note with id {id} not found");
                return new BaseResponse<NoteDTO>
                {
                    Message = $"Note with id {id} not found",
                    Status = false,
                };
            }
            _logger.LogInformation("Note fetched successfully");
            return new BaseResponse<NoteDTO>
            {
                Message = "Note fetched successfully",
                Status = true,
                Data = new NoteDTO
                {
                    Id = getNote.Id,
                    Tittle = getNote.Tittle,
                    Content = getNote.Content,
                    DateCreated = getNote.DateCreated,
                }
            };
        }


        public async Task<NoteDTO> GetNote(int id)
        {
            var getNote = await _noteRepository.GetNoteById(id);
            if (getNote == null)
            {
                _logger.LogError($"Note with id {id} not found");
                
            }
            _logger.LogInformation("Note fetched successfully");
            return new NoteDTO
            {     
                Id = getNote.Id,
                Tittle = getNote.Tittle,
                Content = getNote.Content,
                DateCreated = getNote.DateCreated,
            };
        }

        // public async Task<BaseResponse<NoteDTO>> UpdateNote(int id, UpdateNoteRequestModel request)
        // {
        //     var getNote = await _noteRepository.GetNoteById(id);
        //     if (getNote == null)
        //     {
        //         _logger.LogError($"Note with id {id} not found");
        //         return new BaseResponse<NoteDTO>
        //         {
        //             Message = $"Note with id {id} not found",
        //             Status = false,
        //         };
        //     }
        //     if (request == null)
        //     {
        //         return new BaseResponse<NoteDTO>
        //         {
        //             Message = "Fields cannot be empty",
        //             Status = false,
        //         };
        //     }
        //     getNote.Tittle = request.Tittle;
        //     getNote.Content = request.Content;

        //     var updateNote = await _noteRepository.UpdateNote(getNote);
        //     if (updateNote == null)
        //     {
        //         _logger.LogError($"Note update unsuccessful");
        //         return new BaseResponse<NoteDTO>
        //         {
        //             Message = "Note update unsuccessful",
        //             Status = false,

        //         };
        //     }
        //     _logger.LogInformation("Note update successfully");
        //     return new BaseResponse<NoteDTO>
        //     {
        //         Message = "Note update successfully",
        //         Status = true,
        //         Data = new NoteDTO
        //         {
        //             Id = updateNote.Id,
        //             Tittle = updateNote.Tittle,
        //             Content = updateNote.Content,
        //             DateCreated = updateNote.DateCreated,
        //         }
        //     };
        // }

        public async Task<NoteDTO> UpdateNote(int id, UpdateNoteRequestModel request)
        {
            var getNote = await _noteRepository.GetNoteById(id);
            if (getNote == null)
            {
                _logger.LogError($"Note with id {id} not found");
                
            }
            
            getNote.Tittle = request.Tittle;
            getNote.Content = request.Content;

            var updateNote = await _noteRepository.UpdateNote(getNote);
            _logger.LogInformation("Note update successfully");
            return new NoteDTO
            {
                Id = updateNote.Id,
                Tittle = updateNote.Tittle,
                Content = updateNote.Content,
                DateCreated = updateNote.DateCreated,
            };
        }
    }
}
