using BlazorGolf.Core.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace BlazorGolf.Client.Components
{
    public class EditTeeDialogBase : ComponentBase
    {
        [Inject]
        ISnackbar? Snackbar { get; set; }

        [CascadingParameter]
        MudDialogInstance MudDialog { get; set; } = null!;

        [Inject]
        public NavigationManager? NavigationManager { get; set; }

        [Inject]
        public ILogger<EditTeeDialogBase>? Logger { get; set; }

        [Parameter]
        public Tee? Model { get; set; }

        public MudForm Form = null!;
        public bool FormValid { get; set; }
        public TeeValidator TeeValidator = new();

        public async Task Submit()
        {
            await Form.Validate();
            if (Form.IsValid)
            {
                Logger?.LogInformation("Valid form for updating Tee");
                MudDialog.Close(DialogResult.Ok(Model));
            }
            else
            {
                Logger?.LogInformation("Invalid form for updating Tee");
            }
        }
    }
}