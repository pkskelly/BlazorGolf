using Microsoft.AspNetCore.Components;
using BlazorGolf.Client.Services;
using BlazorGolf.Core.Models;
using MudBlazor;
using Microsoft.AspNetCore.Components.Authorization;
using BlazorGolf.Client.Components;

namespace BlazorGolf.Client.Pages
{
    public class EditCourseBase : ComponentBase
    {
        [Inject]
        ISnackbar? Snackbar { get; set; }

        [Inject]
        public NavigationManager? NavigationManager { get; set; }

        [Inject]
        public IDialogService DialogService {get; set;} = null!;

        [Inject]
        public ICourseService? CourseService { get; set; }

        [Inject]
        public ILogger<CourseBase>? Logger { get; set; }

        [Parameter]
        public Guid CourseId { get; set; }

        public string? SelectedTee { get; set; }

        public MudForm Form = null!;
        public bool FormValid { get; set; }
        public bool IsNewCourse { get; set; } = true;
        public Course? CurrentCourse { get; set; } = new Course();
        public CourseValidator courseValidator = new();
        public IEnumerable<Tee>? Tees
        {
            get => CurrentCourse?.Tees;
        }

        public string ButtonText { get; set; } = "Create";

        protected override async Task OnInitializedAsync()
        {
            if (CourseId != Guid.Empty)
            {
                CurrentCourse = await CourseService!.GetCourseAsync(CourseId);
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
                    await CourseService!.CreateCourseAsync(CurrentCourse);
                    Logger?.LogInformation($"Added course {CurrentCourse.Name}");
                    Snackbar?.Add($"Added {CurrentCourse.Name}!");
                }
                else
                {
                    await CourseService!.UpdateCourseAsync(CurrentCourse);
                    Logger?.LogInformation($"Updated course {CurrentCourse.Name}");
                    Snackbar?.Add($"Updated {CurrentCourse.Name}!");
                }
                NavigationManager?.NavigateTo("/coursepages/courses");
            }
        }


        public async Task HandleDeleteCourse()
        {
            await Form.Validate();
            if (!IsNewCourse)
            {
                await CourseService!.DeleteCourseAsync(CurrentCourse);
                Logger?.LogInformation($"Deleted course {CurrentCourse.Name}");
                Snackbar?.Add($"Deleted {CurrentCourse.Name}!");
            }
            NavigationManager?.NavigateTo("/coursepages/courses");
        }

        public async Task HandleNewTee()
        {
            Logger?.LogInformation($"New Tee clicked with {CurrentCourse?.Tees.ToList().Count} tees in collection.");
            var newTee = new Tee()
            {
                TeeId = Guid.NewGuid().ToString(),
                Name = "Red",
                Par = 72,
                Slope = 136,
                Rating = 69.0,
                BogeyRating = 79.2,
                FrontNineRating = 65.0,
                FrontNineSlope = 155,
                BackNineRating = 64.5,
                BackNineSlope = 152
            };
            List<Tee> tees = CurrentCourse?.Tees.ToList();
            tees.Add(newTee);
            CurrentCourse.Tees = tees;
            await Form.Validate();
            Logger?.LogInformation($"Form is valid: {Form.IsValid}");
            Logger?.LogInformation($"New Tee added - {CurrentCourse?.Tees.ToList().Count} tees in collection.");
            //--- Refresh Page...
            await InvokeAsync(() => StateHasChanged());
            Snackbar?.Add("New Tee added!");
        }

        public async Task HandleEditTee(string teeId)
        {
            await Form.Validate();
            Logger?.LogInformation($"Form is valid: {Form.IsValid}");
            Logger?.LogInformation($"Editing TeeId {teeId}");

            //Edit Tee
            // var parameters = new DialogParameters();
            // parameters.Add("ContentText", "Do you really want to delete these records? This process cannot be undone.");
            // parameters.Add("ButtonText", "Delete");
            // parameters.Add("Color", Color.Error);

            // var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.ExtraSmall };

            // DialogService.Show<EditTeeDialog>("Add", parameters, options);

            Logger?.LogInformation($"Edited Tee added - {CurrentCourse?.Tees.ToList().Count} tees in collection.");

            //--- Refresh Page...
            await InvokeAsync(() => StateHasChanged());
            Snackbar?.Add("Tee updated!");

        }

        public async Task HandleDeleteTee(string teeId)
        {
            Logger?.LogInformation($"Removing TeeId {teeId}");

            var parameters = new DialogParameters();
            parameters.Add("ContentText", "Do you really want to delete this Tee?");
            parameters.Add("ButtonText", "Delete");
            parameters.Add("Color", Color.Error);

            var dialog = DialogService.Show<ConfirmationDialog>("Delete Server", parameters);
            var result = await dialog.Result;

            var resultMessage = "Remove cancelled!";
            if (!result.Cancelled)
            {
                //Remove tee with teeId and reset Course.Tees value
                CurrentCourse.Tees = CurrentCourse.Tees.Where(t => t.TeeId != teeId).ToList();
                Logger?.LogInformation($"Removed Tee - currently {CurrentCourse?.Tees.ToList().Count} tees in collection.");
                resultMessage = "Removal complete!";
            }
            else
            {
                Logger?.LogInformation(resultMessage);
                //--- Refresh Page...
            }
            await InvokeAsync(() => StateHasChanged());
            Snackbar?.Add(resultMessage);
        }
    }
}
