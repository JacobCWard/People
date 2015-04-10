﻿using Starcounter;
using PolyjuiceNamespace;
using Simplified.Ring2;
using Simplified.Ring3;

namespace People {
    internal class PolyjuiceHandlers : IHandlers {
        public void Register() {
            // App name required for Launchpad
            Handle.GET("/people/app-name", () => {
                return new AppName();
            });

            Handle.GET("/people/app-icon", () => {
                Page p = new Page() {
                    Html = "/People/viewmodels/AppIcon.html"
                };

                return p;
            });

            Handle.GET("/people/menu", () => {
                Page p = new Page() {
                    Html = "/People/viewmodels/Menu.html"
                };

                return p;
            });

            Handle.GET("/people/dashboard", () => {
                Page p = new Page() {
                    Html = "/People/viewmodels/Dashboard.html"
                };

                return p;
            });
        }
    }
}
