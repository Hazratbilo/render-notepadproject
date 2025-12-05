using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Notepad.Context;
using Notepad.Interface.Services;
using Notepad.Models;
using Notepad.Models.DTOs;
using Notepad.Models.DTOs.Note;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notepad.Controllers
{
    public class NotesController : Controller
    {
        private readonly INoteServices _noteServices;

        public NotesController(INoteServices noteServices)
        {
            _noteServices = noteServices;
        }

        private string? GetDeviceId()
        {
            return Request.Cookies["DeviceId"];
        }
        public async Task<IActionResult> Index(string searchString)
        {
            //var query = _Context.Notes.AsQueryable();

            //if (!string.IsNullOrWhiteSpace(searchString))
            //{
            //    var pattern = $"%{searchString}%";
            //    query = query.Where(n => EF.Functions.Like(n.Tittle, pattern));
            // } 


            //var notes = await query.OrderByDescending(n => n.DateCreated).ToListAsync();
            var allNotes = await _noteServices.GetNotesByDeviceId(GetDeviceId());
            if (allNotes == null)
            {
                ViewBag.Message = "No data found";
            }

            return View(allNotes);
            
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateNoteRequestModel model)
        {
            if(!ModelState.IsValid)
            {
               return View(model);
            }
            model.DeviceId = GetDeviceId();
            await _noteServices.AddNote(model);


            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {

            var note = await _noteServices.GetNote(id);

            // var response = new BaseResponse<UpdateNoteRequestModel>
            // {
            //     Data = new UpdateNoteRequestModel
            //     {
            //         Tittle = note.Data.Tittle,
            //         Content = note.Data.Content
            //     },
            //     Message = note.Message,
            //     Status = note.Status
            // };
            if (note is null) return NotFound();
            return View(note);
            
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateNoteRequestModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            await _noteServices.UpdateNote(id, model);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var dept = await _noteServices.GetNoteById(id);
            if (!dept.Status) return NotFound();

            return View(dept);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>DeleteConfirmed(int id)
        {
            await _noteServices.DeleteNote(id);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Details(int id)
        {
            var department = await _noteServices.GetNoteById(id);
            if (department == null)
            {
                throw new Exception("Request not found!");
            }

            return View(department);
        }

        public async Task<IActionResult> DownloadTxt(int id)
        {
            var note = await _noteServices.GetNoteById(id);
            var content = $"Title: {note.Data.Tittle}\n\n{note.Data.Content}";
            var bytes = Encoding.UTF8.GetBytes(content);
            var fileName = SanitizeFileName(note.Data.Tittle) + ".txt";
            return File(bytes, "text/plain; charset=utf-8", fileName);
        }

        public async Task<IActionResult> DownloadPdf(int id)
        {
            var note = await _noteServices.GetNoteById(id);
            using var ms = new MemoryStream();
            using var writer = new PdfWriter(ms);
            using var pdf = new PdfDocument(writer);
            using var document = new Document(pdf);

            document.Add(new Paragraph(note.Data.Tittle).SetFontSize(18));
            document.Add(new Paragraph(note.Data.Content));

            document.Close();

            var fileName = SanitizeFileName(note.Data.Tittle) + ".pdf";
            return File(ms.ToArray(), "application/pdf", fileName);
        }

        public async Task<IActionResult> Copy(int id)
        {
            await _noteServices.CopyNoteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private static string SanitizeFileName(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return "note";
            foreach (var c in System.IO.Path.GetInvalidFileNameChars())
                input = input.Replace(c, '-');
            return input;
        }
    }
}
