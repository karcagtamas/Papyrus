using Papyrus.Shared.DTOs.Notes;

namespace Papyrus.Shared.HubEvents;

public static class EditorHubEvents
{
    public static readonly string EditorConnect = "Connect";
    public static readonly string EditorDisconnect = "Disconnect";
    public static readonly string EditorShare = "Share";
    public static readonly string EditorUpdateNote = "UpdateNote";

    public static readonly string EditorChanged = "Changed";
    public static readonly string EditorMemberJoined = "MemberJoined";
    public static readonly string EditorMemberLeft = "MemberLeft";
    public static readonly string EditorNoteUpdated = "NoteUpdated";
}

public class NoteChangeEventArgs
{
    public string Title { get; set; } = default!;
    public List<NoteTagDTO> Tags { get; set; } = new();
}