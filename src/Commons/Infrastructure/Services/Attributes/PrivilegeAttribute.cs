using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace Infrastructure.Services.Attributes
{
    public class PrivilegeAttribute : TypeFilterAttribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="privilegeValue"></param>
        public PrivilegeAttribute(string privilegeValue, bool isApi) : base(typeof(AuthorizeFilter))
        {
            Arguments = new object[] { privilegeValue, isApi };
        }

        public PrivilegeAttribute(string privilegeValue) : base(typeof(AuthorizeFilter))
        {
            Arguments = new object[] { privilegeValue };
        }
    }
}
