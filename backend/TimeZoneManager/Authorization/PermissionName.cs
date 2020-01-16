namespace TimeZoneManager.Authorization
{
    public enum PermissionName
    {
        Unknown = 0,

        PermissionCreate, PermissionRead,

        RoleCreate, RoleRead, RoleUpdate, RoleDelete,

        UserCreate, UserRead, UserUpdate, UserDelete,

        TimeZoneCreate, TimeZoneRead, TimeZoneUpdate, TimeZoneDelete, TimeZoneAdmin
    }
}
