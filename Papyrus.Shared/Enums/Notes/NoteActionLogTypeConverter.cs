namespace Papyrus.Shared.Enums.Notes;

public static class NoteActionLogTypeConverter
{
    public static string GetLogKey(NoteActionLogType type)
    {
        return type switch
        {
            NoteActionLogType.Create => "Create",
            NoteActionLogType.TitleEdit => "TitleEdit",
            NoteActionLogType.ContentEdit => "ContentEdit",
            NoteActionLogType.TagEdit => "TagEdit",
            NoteActionLogType.Delete => "Delete",
            NoteActionLogType.Publish => "Publish",
            NoteActionLogType.Archived => "Archived",
            _ => "",
        };
    }
}
