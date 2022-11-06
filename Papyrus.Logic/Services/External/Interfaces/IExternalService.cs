using Papyrus.Shared.DTOs.Notes;
using Papyrus.Shared.Models.Profile;

namespace Papyrus.Logic.Services.External.Interfaces;

public interface IExternalService
{
    List<NoteLightDTO> GetNotes(ApplicationQueryModel query);
}
