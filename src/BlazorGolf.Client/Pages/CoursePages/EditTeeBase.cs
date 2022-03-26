using BlazorGolf.Client.Services;
using BlazorGolf.Core.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace BlazorGolf.Client.Pages.CoursePages
{
    public class EditTeeBase : ComponentBase
    {
        [Inject] 
        ISnackbar? Snackbar { get; set; }
        [Inject]
        public NavigationManager? NavigationManager { get; set; }
        [Inject]
        public ICourseService? CourseService { get; set; }
        [Inject]
        public ILogger<EditTeeBase>? Logger { get; set; }

        public Course? Model { get; set; } = new Course();

        [Parameter]
        public Guid CourseId { get; set; }
        [Parameter]
        public Guid TeeId { get; set; }
        public string? Message { get; set; }
        public IEnumerable<Hole>? Holes;
        public bool ReadOnly = true;

        protected override async Task OnInitializedAsync()
        {
                Holes = new List<Hole> {
                new Hole() { Number = 1, Par = 3, HandicapIndex = 2, Distance = 72 },
                new Hole() { Number = 2, Par = 3, HandicapIndex = 3, Distance = 72 },
                new Hole() { Number = 3, Par = 3, HandicapIndex = 4 , Distance = 72 },
                new Hole() { Number = 4, Par = 3, HandicapIndex = 1 , Distance = 72 },
                new Hole() { Number = 5, Par = 3, HandicapIndex = 6 , Distance = 72 },
                new Hole() { Number = 6, Par = 3, HandicapIndex = 5 , Distance = 72 }
            };
            Logger?.LogInformation($"Initialized holes...");

            if (CourseId != Guid.Empty)
            {
                Logger?.LogInformation($"Getting course {Model.Name}");
                Model = await CourseService!.GetCourseAsync(CourseId);
                Logger?.LogInformation($"Retrieved course {Model.Name}");

            }
        }

    }

    public class Hole {
        public int Number { get; set; }
        public int Par { get; set; }
        public int Distance { get; set; }
        public int HandicapIndex { get; set; }
    }
}