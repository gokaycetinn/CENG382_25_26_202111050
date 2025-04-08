using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Week5.Models;
using System.Collections.Generic;
using System.Linq;

namespace Week5.Pages
{
    public class IndexModel : PageModel
    {
        private static List<ClassInformationModel> ClassList = new List<ClassInformationModel>();
        private static int NextId = 1;
        private static bool IsSeeded = false;

        [BindProperty(SupportsGet = true)]
        public string? SearchTerm { get; set; }

        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;

        private const int PageSize = 10;

        [BindProperty]
        public ClassInformationModel NewClass { get; set; } = new ClassInformationModel();

        public List<ClassInformationModel> PagedClasses { get; set; } = new List<ClassInformationModel>();

        public int TotalPages { get; set; }

        public void OnGet()
        {
            
            if (!IsSeeded)
            {
                for (int i = 1; i <= 100; i++)
                {
                    ClassList.Add(new ClassInformationModel
                    {
                        Id = NextId++,
                        ClassName = $"Class {i}",
                        StudentCount = 10 + (i % 30),
                        Description = $"Description-generated class {i}"
                    });
                }
                IsSeeded = true;
            }

            var query = ClassList.AsQueryable();

            if (!string.IsNullOrWhiteSpace(SearchTerm))
            {
                query = query.Where(c => c.ClassName.Contains(SearchTerm, System.StringComparison.OrdinalIgnoreCase));
            }

            int totalRecords = query.Count();
            TotalPages = (int)System.Math.Ceiling(totalRecords / (double)PageSize);

            PagedClasses = query
                .Skip((PageNumber - 1) * PageSize)
                .Take(PageSize)
                .ToList();
        }

        public IActionResult OnPostAdd()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            NewClass.Id = NextId++;
            ClassList.Add(NewClass);
            return RedirectToPage(new { PageNumber, SearchTerm });
        }

        public IActionResult OnPostDelete(int id)
        {
            var item = ClassList.FirstOrDefault(c => c.Id == id);
            if (item != null)
                ClassList.Remove(item);

            return RedirectToPage(new { PageNumber, SearchTerm });
        }

        public IActionResult OnPostEdit(int id)
        {
            var item = ClassList.FirstOrDefault(c => c.Id == id);
            if (item != null)
            {
                NewClass = new ClassInformationModel
                {
                    Id = item.Id,
                    ClassName = item.ClassName,
                    StudentCount = item.StudentCount,
                    Description = item.Description
                };
            }

            return Page();
        }

        public IActionResult OnPostUpdate()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var existing = ClassList.FirstOrDefault(c => c.Id == NewClass.Id);
            if (existing != null)
            {
                existing.ClassName = NewClass.ClassName;
                existing.StudentCount = NewClass.StudentCount;
                existing.Description = NewClass.Description;
            }

            return RedirectToPage(new { PageNumber, SearchTerm });
        }
    }
}
