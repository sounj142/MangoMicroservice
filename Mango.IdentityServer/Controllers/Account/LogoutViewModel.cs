// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

namespace Mango.IdentityServer.UI
{
    public class LogoutViewModel : LogoutInputModel
    {
        public bool ShowLogoutPrompt { get; set; } = true;
    }
}