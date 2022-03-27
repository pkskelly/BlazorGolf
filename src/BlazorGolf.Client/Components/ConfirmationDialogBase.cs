using BlazorGolf.Core.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace BlazorGolf.Client.Components
{
    public class ConfirmationDialogBase : ComponentBase
    {

        [CascadingParameter]
        MudDialogInstance MudDialog { get; set; } = null!;

        [Parameter]
        public string ContentText { get; set; } = null!;

        [Parameter]
        public string ButtonText { get; set; } = null!;

        [Parameter] public Color Color { get; set; }

        public void Submit() => MudDialog.Close(DialogResult.Ok(true));
        public void Cancel() => MudDialog.Cancel();

    }
}