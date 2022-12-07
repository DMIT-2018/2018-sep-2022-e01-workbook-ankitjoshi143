using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

#region Additional 
using EmployeeRegistrationSystem.BLL;
using EmployeeRegistrationSystem.ViewModels;
#endregion

namespace WebApp.Pages.Employee
{
    public class EmployeeSkillsModel : PageModel
    {
        #region Messaging and Error Handling
        [TempData]
        public string FeedBackMessage { get; set; }

        public string ErrorMessage { get; set; }

        //a get property that returns the result of the lamda action
        public bool HasError => !string.IsNullOrWhiteSpace(ErrorMessage);
        public bool HasFeedBack => !string.IsNullOrWhiteSpace(FeedBackMessage);

        //used to display any collection of errors on web page
        //whether the errors are generated locally OR come form the class library
        //      service methods
        public List<string> ErrorDetails { get; set; } = new();

        //PageModel local error list for collection 
        public List<Exception> Errors { get; set; } = new();

        #endregion

        #region Private variables and DI constructor
        private readonly FetchEmployeeSkillServices _fetchEmployeeSkill;

        public EmployeeSkillsModel(FetchEmployeeSkillServices fetchskillservices)
        {
            _fetchEmployeeSkill = fetchskillservices;
        }
        #endregion
        [BindProperty(SupportsGet = true)]
        public string phonenumber { get; set; }
        public List<FetchEmployeeSkill> skillslist { get; set; } = new();
        public void GetSkillsList()
        {
            if(!string.IsNullOrWhiteSpace(phonenumber))
            {
                skillslist = _fetchEmployeeSkill.Skills_FetchSkillBy(phonenumber);
            }
        }
        public IActionResult OnPostFetchSkill()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(phonenumber))
                {
                    throw new Exception("Enter a phonenumber to fetch.");
                }
                return RedirectToPage(new
                {
                    phonenumber = phonenumber.Trim()
                });
            }
            catch (Exception ex)
            {
                ErrorMessage = GetInnerException(ex).Message;
                return Page();

            }
        }
        private Exception GetInnerException(Exception ex)
        {
            while (ex.InnerException != null)
                ex = ex.InnerException;
            return ex;
        }
        public void OnGet()
        {
            GetSkillsList();
        }

        public IActionResult OnPostClear()
        {
            ModelState.Clear();
            FeedBackMessage = "Form Cleared";
            return RedirectToPage(new
            {
                phonenumber = ""
            });
        }
    }
}
