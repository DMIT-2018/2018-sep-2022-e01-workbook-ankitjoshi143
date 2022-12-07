using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

#region Additional 
using EmployeeRegistrationSystem.ViewModels;
using EmployeeRegistrationSystem.BLL;
#endregion

namespace WebApp.Pages.Employee
{
    public class EmployeeRegistrationModel : PageModel
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
        private readonly SkillServices _skillServices;


        public EmployeeRegistrationModel(SkillServices skillservices)
        {
            _skillServices = skillservices;
        }
        #endregion

        [BindProperty]
        public List<SkillList> displayskilllist { get; set; }
        public void OnGet()
        {
            displayskilllist = _skillServices.Skills_OrderedList();
        }

        [BindProperty(SupportsGet = true)]
        public string firstname { get; set; }

        [BindProperty(SupportsGet = true)]
        public string lastname { get; set; }

        [BindProperty(SupportsGet = true)]
        public string homephone { get; set; }

        [BindProperty]
        public List<EmpSkill> skilllistselected { get; set; } = new();
        public IActionResult OnPostAddEmployee()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(firstname))
                {
                    throw new ArgumentNullException("No First Name submitted");
                }
                if (string.IsNullOrWhiteSpace(lastname))
                {
                    throw new ArgumentNullException("No Last Name submitted");
                }
                if (string.IsNullOrWhiteSpace(homephone))
                {
                    throw new ArgumentNullException("No Home Phone submitted");
                }

                int skillselected = skilllistselected
                                        .Where(s => s.SelectedSkill)
                                        .Count();

                if (skillselected == 0)
                {
                    throw new ArgumentNullException("No skill list submitted");
                }
                //call your service sending in the expected data
                _skillServices.EmployeeSkills_RegisterTRX(firstname, lastname, homephone, skilllistselected);
                FeedBackMessage = "Added to the record";
                return RedirectToPage(new
                {
                    firstname = "",
                    lastname = "",
                    homephone = ""
                });
            }
            catch (AggregateException ex)
            {

                ErrorMessage = "Unable to process the record";
                foreach (var error in ex.InnerExceptions)
                {
                    ErrorDetails.Add(error.Message);

                }
                OnGet();
                return Page();
            }
            catch (Exception ex)
            {
                ErrorMessage = GetInnerException(ex).Message;
                OnGet();
                return Page();
            }

        }

        private Exception GetInnerException(Exception ex)
        {
            while (ex.InnerException != null)
                ex = ex.InnerException;
            return ex;
        }

        public IActionResult OnPostClear()
        {
            ModelState.Clear();
            FeedBackMessage = "Form Cleared";
            return RedirectToPage(new
            {
                firstname = "",
                lastname = "",
                homephone = ""
            });
        }
    }
}
