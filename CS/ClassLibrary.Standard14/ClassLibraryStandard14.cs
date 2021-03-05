using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClassLibrary.Standard14
{
    public static class ClassLibraryStandard14
    {
        public static ToastContentBuilder GenerateBasicToast()
        {
            return new ToastContentBuilder()
                .AddText("Created from .NET Standard 1.4");
        }
    }
}
