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

        [BindProperty]
        public ClassInformationModel NewClass { get; set; } = new ClassInformationModel();

        public List<ClassInformationModel> Classes => ClassList;  

        public void OnGet()
        {
        }

        public IActionResult OnPostAdd()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            NewClass.Id = NextId++;
            ClassList.Add(NewClass);
            NewClass = new ClassInformationModel(); // Formu sıfırla
            return RedirectToPage();
        }

        public IActionResult OnPostDelete(int id)
        {
            var item = ClassList.FirstOrDefault(c => c.Id == id);
            if (item != null)
            {
                ClassList.Remove(item);
            }
            return RedirectToPage();
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

            var existingClass = ClassList.FirstOrDefault(c => c.Id == NewClass.Id);
            if (existingClass != null)
            {
                existingClass.ClassName = NewClass.ClassName;
                existingClass.StudentCount = NewClass.StudentCount;
                existingClass.Description = NewClass.Description;
            }

            NewClass = new ClassInformationModel(); // Formu sıfırla
            return RedirectToPage();
        }
    }
}
