using System;
using System.Collections.Generic;
using FlexProviders.Membership;

namespace FlexProviders.Tests.Integration.Raven
{
    public class User : IFlexMembershipUser
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public string PasswordResetToken { get; set; }
        public DateTime? PasswordResetTokenExpiration { get; set; }
        public bool IsLocal { get; set; }
        public ICollection<FlexOAuthAccount> OAuthAccounts { get; set; }
    }
}