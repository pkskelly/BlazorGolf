using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using BlazorGolf.Client.Services;
using BlazorGolf.Core.Models;
using MudBlazor;
using Microsoft.AspNetCore.Components.Forms;

namespace BlazorGolf.Client.Pages
{
    public class EditCourseBase : ComponentBase
    {
        [Inject] ISnackbar? Snackbar { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public ICourseService? CourseService { get; set; }

        [Inject]
        public ILogger<CourseBase>? Logger { get; set; }

        [Parameter]
        public Guid CourseId { get; set; }

        public MudForm form;
        public bool FormValid { get; set; }
        public Course? model { get; set; } = new Course();
        public CourseValidator courseValidator = new CourseValidator();
        protected bool Saved;

        protected override async Task OnInitializedAsync()
        {
            Saved = false;
            if (CourseId != Guid.Empty)
            {
                model = await CourseService!.GetCourse(CourseId);
            }
        }

        public async Task HandleSubmit()
        {
            await form.Validate();
            if (form.IsValid)
            {
                Snackbar.Add("Submitted!");
            }
        }
    }
}
