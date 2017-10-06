using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;

namespace xamarinmap.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();

            LoadApplication(new App());

            // TODO : Map Initialization
            Xamarin.FormsMaps.Init();

            return base.FinishedLaunching(app, options);
        }
    }
}
