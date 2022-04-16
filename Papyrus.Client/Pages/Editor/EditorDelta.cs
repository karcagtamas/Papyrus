namespace Papyrus.Client.Pages.Editor;

public class EditorDelta
{
    public EditorDeltaAction Action { get; set; }
    public EditorDeltaContext Context { get; set; }

    public override string ToString()
    {
        return $"[Actions = {Action}, Context = {Context}]";
    }
}

public class EditorDeltaContext
{
    public override string ToString()
    {
        return "[]";
    }
}

public class ClickContext : EditorDeltaContext
{
    public int CursorPosition { get; set; }

    public override string ToString()
    {
        return $"[CursorPosition = {CursorPosition}]";
    }
}

public enum EditorDeltaAction
{
    Click
}
