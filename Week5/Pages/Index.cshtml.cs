using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Week5.Models;
using Week5.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Week5.Pages
{
    public class IndexModel : PageModel
    {
        
         private static List<ClassInformationModel> ClassList = new List<ClassInformationModel>();

        
        public static List<ClassInformationModel> GetClassList()
        {
            return ClassList;
        }

    // Public getter for filtered list
    public static List<ClassInformationModel> GetFilteredList(string? searchTerm)
    {
        var query = ClassList.AsQueryable();
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(c => c.ClassName.Contains(searchTerm, System.StringComparison.OrdinalIgnoreCase));
        }
        return query.ToList();
    }
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

        /*Write an OnGet method in Razor Pages that creates 100 sample class records just once, 
        filters by ClassName,calculates the total number of pages based on the page size, 
        and returns the data for the current page. */
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

        /* Write an OnPostEdit method in Razor Pages that finds a class by its ID from an in-memory list, 
        and if found, populates a form model (NewClass) with its data to allow editing on the page.*/
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

        /*In Razor Pages, write an OnPostUpdate method that updates 
        the existing class using the New Class pattern from the form. */
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
        /* Create a POST method, get all the data and convert it to JSON format. 
        Convert the JSON data to a file and present it to the user for download. 
        Set the name of the file to 'all_classes.json'*/
        public IActionResult OnPostExportAll()
        {
            var allData = GetClassList();
            var json = Utilities.Utils.Instance.ExportToJson(allData);
            var bytes = System.Text.Encoding.UTF8.GetBytes(json);
            return File(bytes, "application/json", "all_classes.json");
        }
        /* Create a POST method, take a SelectedColumns parameter to get the columns selected by the user.
        Create a project that contains only the selected columns.
        Export this project in JSON format and present the JSON file to the user to download."*/
       public IActionResult OnPostExportSelected(string SelectedColumns)
        {
            if (string.IsNullOrWhiteSpace(SelectedColumns))
            {
                TempData["Error"] = "Please select at least one column to export.";
                return RedirectToPage(new { PageNumber, SearchTerm });
            }

            var selectedList = SelectedColumns.Split(',').ToList();

            var allData = GetClassList(); // tüm 100 veri
            var projected = allData.Select(item =>
            {
                var dict = new Dictionary<string, object>();

                if (selectedList.Contains("Id"))
                    dict["Id"] = item.Id;
                if (selectedList.Contains("ClassName"))
                    dict["ClassName"] = item.ClassName;
                if (selectedList.Contains("StudentCount"))
                    dict["StudentCount"] = item.StudentCount;
                if (selectedList.Contains("Description"))
                    dict["Description"] = item.Description;

                return dict;
            }).ToList();

            var json = Utilities.Utils.Instance.ExportToJson(projected);
            var bytes = System.Text.Encoding.UTF8.GetBytes(json);

            return File(bytes, "application/json", "selected_columns.json");
        }


      
}

}