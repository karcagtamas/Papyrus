using AutoMapper;
using Papyrus.DataAccess.Entities.Notes;
using Papyrus.Shared.DTOs.Notes;
using Papyrus.Shared.Models.Notes;

namespace Papyrus.Logic.Mappers;

public class FolderMapper : Profile
{
    public FolderMapper()
    {
        CreateMap<Folder, FolderDTO>();
        CreateMap<FolderModel, Folder>();
    }
}
