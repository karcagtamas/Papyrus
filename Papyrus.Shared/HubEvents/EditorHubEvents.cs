namespace Papyrus.Shared.HubEvents;

public static class EditorHubEvents
{
    public static readonly string EditorConnect = "Connect"; 
    public static readonly string EditorDisconnect = "Disconnect";
    public static readonly string EditorShare = "Share";
    public static readonly string EditorChanged = "Changed";
    public static readonly string EditorMemberChange = "MemberChange";
}

public enum EditorMemberChange
{
    Join,
    Leave
}