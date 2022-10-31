namespace Papyrus.Shared.Enums.Groups;

public static class GroupActionLogTypeConverter
{
    public static string GetLogKey(GroupActionLogType type)
    {
        return type switch
        {
            GroupActionLogType.Create => "Create",
            GroupActionLogType.Close => "Close",
            GroupActionLogType.Open => "Open",
            GroupActionLogType.RoleCreate => "RoleCreate",
            GroupActionLogType.RoleEdit => "RoleEdit",
            GroupActionLogType.RoleRemove => "RoleRemove",
            GroupActionLogType.MemberAdd => "MemberAdd",
            GroupActionLogType.MemberEdit => "MemberEdit",
            GroupActionLogType.MemberRemove => "MemberRemove",
            GroupActionLogType.TagCreate => "TagCreate",
            GroupActionLogType.TagEdit => "TagEdite",
            GroupActionLogType.TagRemove => "TagRemove",
            GroupActionLogType.DataEdit => "DataEdit",
            GroupActionLogType.NoteCreate => "NoteCreate",
            GroupActionLogType.Remove => "Remove",
            _ => "",
        };
    }
}
