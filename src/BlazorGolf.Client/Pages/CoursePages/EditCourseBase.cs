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
        public NavigationManager? NavigationManager { get; set; }

        [Inject]
        public ICourseService? CourseService { get; set; }

        [Inject]
        public ILogger<CourseBase>? Logger { get; set; }

        [Parameter]
        public Guid CourseId { get; set; }

        public MudForm form;
        public bool FormValid { get; set; }
        public bool IsNewCourse { get; set; } = true;
        public Course? model { get; set; } = new Course();
        public CourseValidator courseValidator = new CourseValidator();

        public string ButtonText { get; set; } = "Create";

        protected override async Task OnInitializedAsync()
        {
            if (CourseId != Guid.Empty)
            {
                model = await CourseService!.GetCourseAsync(CourseId);
                ButtonText = "Update";
                IsNewCourse = false;
            }
        }

        public async Task HandleSubmit()
        {
            await form.Validate();
            if (form.IsValid)
            {
                if (IsNewCourse)
                {
                    await CourseService!.CreateCourseAsync(model);
                    Logger?.LogInformation($"Added course {model.Name}");
                    Snackbar?.Add($"Added {@model.Name}!");
                }
                else
                {
                    await CourseService!.UpdateCourseAsync(model);
                    Logger?.LogInformation($"Updated course {model.Name}");
                    Snackbar?.Add($"Updated {@model.Name}!");
                }
                NavigationManager.NavigateTo("/coursepages/courses");
            }
        }

        public async Task HandleDelete()
        {
            await form.Validate();
            if (!IsNewCourse)
            {
                await CourseService!.DeleteCourseAsync(model);
                Logger?.LogInformation($"Deleted course {model.Name}");
                Snackbar.Add($"Deleted {@model.Name}!");
            }
            NavigationManager.NavigateTo("/coursepages/courses");
        }

    }
}
