-- Default roles and their permissions
select Permission.Name as '(Permission) name)', Role.Name as '(Role) name)' from PermissionInRole
join Permission on Permission.Id = PermissionInRole.PermissionId
join Role on Role.Id = PermissionInRole.RoleId

-- Default users and their roles
select [User].Username, Role.Name as '(Role) name' from RoleInUser
join [User] on [User].Id = RoleInUser.UserId
join Role on Role.Id = RoleInUser.RoleId

-- Unused permissions, not indended to be assigned
select Name from Permission
left join PermissionInRole on Id = PermissionInRole.PermissionId
where PermissionInRole.PermissionId is null