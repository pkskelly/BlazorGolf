using BlazorGolf.Core.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace BlazorGolf.Client.Components
{
    public class EditTeeDialogBase : ComponentBase
    {
        [Inject]
        ISnackbar? Snackbar { get; set; }

        [Inject]
        public NavigationManager? NavigationManager { get; set; }

        [Inject]
        public ILogger<EditTeeDialogBase>? Logger { get; set; }


        public MudForm Form = null!;
        public bool FormValid { get; set; }
        public bool IsNewCourse { get; set; } = true;
        public Tee? Model { get; set; } = new Tee();
        public TeeValidator TeeValidator = new();
 

        public async Task Submit()
        {
            await Form.Validate();
            if (Form.IsValid)
            {
                Snackbar.Add("New Tee submitted!");
            }
        }
    }
}