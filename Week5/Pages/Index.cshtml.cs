using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Week5.Data;
using Week5.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Week5.Pages
{
    public class IndexModel : PageModel
    {
        private readonly SchoolDbContext _context;

        public IndexModel(SchoolDbContext context)
        {
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public string? SearchTerm { get; set; }

        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;

        private const int PageSize = 10;

        [BindProperty]
        public Class NewClass { get; set; } = new Class();

        public List<Class> PagedClasses { get; set; } = new List<Class>();

        public int TotalPages { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var sessionToken = HttpContext.Session.GetString("token");
            var sessionUsername = HttpContext.Session.GetString("username");
            var sessionId = HttpContext.Session.GetString("session_id");

            var cookieToken = Request.Cookies["token"];
            var cookieUsername = Request.Cookies["username"];
            var cookieSessionId = Request.Cookies["session_id"];

            if (string.IsNullOrEmpty(sessionToken) ||
                string.IsNullOrEmpty(sessionUsername) ||
                sessionToken != cookieToken ||
                sessionUsername != cookieUsername ||
                sessionId != cookieSessionId)
            {
                return RedirectToPage("/Login");
            }

            var query = _context.Classes.AsQueryable();

            if (!string.IsNullOrWhiteSpace(SearchTerm))
            {
                query = query.Where(c => c.Name.Contains(SearchTerm));
            }

            int totalRecords = await query.CountAsync();
            TotalPages = (int)System.Math.Ceiling(totalRecords / (double)PageSize);

            PagedClasses = await query
                .Skip((PageNumber - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAddAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Classes.Add(NewClass);
            await _context.SaveChangesAsync();

            return RedirectToPage(new { PageNumber, SearchTerm });
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var item = await _context.Classes.FindAsync(id);
            if (item != null)
            {
                _context.Classes.Remove(item);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage(new { PageNumber, SearchTerm });
        }

        public async Task<IActionResult> OnPostEditAsync(int id)
        {
            var item = await _context.Classes.FindAsync(id);
            if (item != null)
            {
                NewClass = new Class
                {
                    Id = item.Id,
                    Name = item.Name,
                    PersonCount = item.PersonCount,
                    Description = item.Description,
                    IsActive = item.IsActive
                };
            }

            return Page();
        }

        public async Task<IActionResult> OnPostUpdateAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var existing = await _context.Classes.FindAsync(NewClass.Id);
            if (existing != null)
            {
                existing.Name = NewClass.Name;
                existing.PersonCount = NewClass.PersonCount;
                existing.Description = NewClass.Description;
                existing.IsActive = NewClass.IsActive;

                await _context.SaveChangesAsync();
            }

            return RedirectToPage(new { PageNumber, SearchTerm });
        }

        public async Task<IActionResult> OnPostExportAllAsync()
        {
            var allData = await _context.Classes.ToListAsync();
            var json = Utilities.Utils.Instance.ExportToJson(allData);
            var bytes = System.Text.Encoding.UTF8.GetBytes(json);
            return File(bytes, "application/json", "all_classes.json");
        }

        public async Task<IActionResult> OnPostExportSelectedAsync(string SelectedColumns)
        {
            if (string.IsNullOrWhiteSpace(SelectedColumns))
            {
                TempData["Error"] = "Please select at least one column to export.";
                return RedirectToPage(new { PageNumber, SearchTerm });
            }

            var selectedList = SelectedColumns.Split(',').ToList();
            var allData = await _context.Classes.ToListAsync();

            var projected = allData.Select(item =>
            {
                var dict = new Dictionary<string, object>();

                if (selectedList.Contains("Id"))
                    dict["Id"] = item.Id;
                if (selectedList.Contains("Name"))
                    dict["Name"] = item.Name;
                if (selectedList.Contains("PersonCount"))
                    dict["PersonCount"] = item.PersonCount;
                if (selectedList.Contains("Description"))
                    dict["Description"] = item.Description;
                if (selectedList.Contains("IsActive"))
                    dict["IsActive"] = item.IsActive;

                return dict;
            }).ToList();

            var json = Utilities.Utils.Instance.ExportToJson(projected);
            var bytes = System.Text.Encoding.UTF8.GetBytes(json);

            return File(bytes, "application/json", "selected_columns.json");
        }
    }
}
