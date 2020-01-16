using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeZoneManager.Authentication;
using TimeZoneManager.Authorization;

namespace TimeZoneManager.Api.Definitions
{
    public class Constants
    {
        public PermissionName Permission { get; }

        public RoleName Role { get; }

        public JwtTokenPayload Payload { get; }

        public CustomClaimTypesName Claims { get; }
    }
}
