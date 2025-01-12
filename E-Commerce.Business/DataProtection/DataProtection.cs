using Microsoft.AspNetCore.DataProtection;

namespace E_Commerce.Business.DataProtection
{
    public class DataProtection : IDataProtection
    {
        private readonly IDataProtector _protector;

        public DataProtection(IDataProtectionProvider provider)
        {
            _protector = provider.CreateProtector("E-commerce-security");
        }

        public string Protect(string text)
        {
            return _protector.Protect(text);
        }

        public string UnProtect(string protectedText)
        {
            return _protector.Unprotect(protectedText);
        }
    }
}
