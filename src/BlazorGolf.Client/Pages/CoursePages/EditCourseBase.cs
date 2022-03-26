using Microsoft.AspNetCore.Components;
using BlazorGolf.Client.Services;
using BlazorGolf.Core.Models;
using MudBlazor;
using Microsoft.AspNetCore.Components.Authorization;

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

        public string? SelectedTee {get; set;}

        public MudForm Form;
        public bool FormValid { get; set; }
        public bool IsNewCourse { get; set; } = true;
        public Course? Model { get; set; } = new Course();
        public CourseValidator courseValidator = new CourseValidator();
        public IEnumerable<Tee>? Tees
        {
            get => Model?.Tees;
        }

        public string ButtonText { get; set; } = "Create";

        protected override async Task OnInitializedAsync()
        {
            if (CourseId != Guid.Empty)
            {
                Model = await CourseService!.GetCourseAsync(CourseId);
                ButtonText = "Update";
                IsNewCourse = false;
            }
        }

        public async Task HandleSubmit()
        {
            await Form.Validate();
            if (Form.IsValid)
            {
                if (IsNewCourse)
                {
                    await CourseService!.CreateCourseAsync(Model);
                    Logger?.LogInformation($"Added course {Model.Name}");
                    Snackbar?.Add($"Added {Model.Name}!");
                }
                else
                {
                    await CourseService!.UpdateCourseAsync(Model);
                    Logger?.LogInformation($"Updated course {Model.Name}");
                    Snackbar?.Add($"Updated {Model.Name}!");
                }
                NavigationManager?.NavigateTo("/coursepages/courses");
            }
        }

        public async Task Edit(string teeId){
            await Form.Validate();
            Logger?.LogInformation($"Edit course with TeeId {teeId}");
        }

        public async Task HandleDelete()
        {
            await Form.Validate();
            if (!IsNewCourse)
            {
                await CourseService!.DeleteCourseAsync(Model);
                Logger?.LogInformation($"Deleted course {Model.Name}");
                Snackbar?.Add($"Deleted {Model.Name}!");
            }
            NavigationManager?.NavigateTo("/coursepages/courses");
        }
        
        public async Task HandleNewTee()
        {
            Logger?.LogInformation($"New Tee clicked with {Model?.Tees.ToList().Count} tees in collection.");
            var newTee = new Tee()
                    {
                        TeeId = Guid.NewGuid().ToString(),
                        Name = "Red",
                        Par = 72,
                        Slope = 136,
                        Rating = 69.0,
                        BogeyRating  = 79.2,
                        FrontNineRating = 69.0,
                        FrontNineSlope = 155,
                        BackNineRating = 64.5,
                        BackNineSlope  = 152
                    };
            List<Tee> tees = Model?.Tees.ToList();
            tees.Add(newTee);
            Model.Tees = tees;
            await Form.Validate();
            Logger?.LogInformation($"Form is valid: {Form.IsValid}");
            Logger?.LogInformation($"New Tee added - {Model?.Tees.ToList().Count} tees in collection.");
            //--- Refresh Page...
            await InvokeAsync(() => StateHasChanged());
            Snackbar?.Add("New Tee added!");
        }
    }
}
