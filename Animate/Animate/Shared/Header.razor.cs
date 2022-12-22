using AnimateLibrary;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.ComponentModel.DataAnnotations;

namespace Animate.Shared
{
  
    public partial class Header
    {
        [Inject] IJSRuntime JSRT { get; set; }

        protected LoginModel loginModel = new LoginModel();

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await JSRT.InvokeVoidAsync("setDialog");
            }
            await base.OnAfterRenderAsync(firstRender);
        }

        private void ValidSubmit()
        {
            //if ("admin".Equals(loginModel.email) && 1234.Equals("loginModel.password"))
            //{
                JSRT.InvokeVoidAsync("showDialog", loginModel);
            //}
            //else
            //{
            //    JSRT.InvokeVoidAsync("alert", "email or password is not vaild" );
            //}
            
        }

    }
}
