using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using FYP.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FYP.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<FYPUser> _userManager;
        private readonly SignInManager<FYPUser> _signInManager;

        public IndexModel(
            UserManager<FYPUser> userManager,
            SignInManager<FYPUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }

            [Required(ErrorMessage = "You must enter the name first before submitting your form!")]
            [StringLength(256, ErrorMessage = "You must enter the value between 6 - 256 chars", MinimumLength = 6)]
            [Display(Name = "Your Full Name")] //label
            public string fullname { get; set; }

            [Required]
            [Display(Name = "Your DOB")]
            [DataType(DataType.Date)]
            public DateTime DoB { get; set; }

            [Required]
            [Display(Name = "ID")]
            [StringLength(12, ErrorMessage = "Please enter a valid ID number", MinimumLength = 12)]
            public string ID { get; set; }

        }

        private async Task LoadAsync(FYPUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                fullname = user.FullName,
                DoB = user.DOB,
                ID = user.NRIC,

            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }

            if (Input.fullname != user.FullName)
            {
                user.FullName = Input.fullname;
            }

            if (Input.DoB != user.DOB)
            {
                user.DOB = Input.DoB;
            }

            if (Input.ID != user.NRIC)
            {
                user.NRIC = Input.ID;
            }

            await _userManager.UpdateAsync(user);


            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
