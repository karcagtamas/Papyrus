using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Papyrus.Shared.Enums.Security;

namespace Papyrus.Logic.Authorization;

public class NoteRequirement : OperationAuthorizationRequirement
{
    public NoteRight Right { get; set; }
}

public static class NoteOperations
{
    public static readonly NoteRequirement ReadNoteRequirement = new() { Name = "ReadNote", Right = NoteRight.Read };
    public static readonly NoteRequirement EditNoteRequirement = new() { Name = "EditNote", Right = NoteRight.Edit };
    public static readonly NoteRequirement DeleteNoteRequirement = new() { Name = "DeleteNote", Right = NoteRight.Delete };
    public static readonly NoteRequirement ReadNoteLogsRequirement = new() { Name = "ReadNoteLogs", Right = NoteRight.ReadLogs };
}

public static class NotePolicies
{
    
    public static readonly AuthorizationPolicy ReadNote = new AuthorizationPolicyBuilder()
           .AddRequirements(NoteOperations.ReadNoteRequirement)
           .Build();
    public static readonly AuthorizationPolicy EditNote = new AuthorizationPolicyBuilder()
           .AddRequirements(NoteOperations.EditNoteRequirement)
           .Build();
    public static readonly AuthorizationPolicy DeleteNote = new AuthorizationPolicyBuilder()
           .AddRequirements(NoteOperations.DeleteNoteRequirement)
           .Build();
    public static readonly AuthorizationPolicy ReadNoteLogs = new AuthorizationPolicyBuilder()
           .AddRequirements(NoteOperations.ReadNoteLogsRequirement)
           .Build();
}
