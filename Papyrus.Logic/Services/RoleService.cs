using AutoMapper;
using KarcagS.Common.Tools.Repository;
using KarcagS.Common.Tools.Services;
using Papyrus.DataAccess;
using Papyrus.DataAccess.Entities;
using Papyrus.Logic.Services.Interfaces;
using Papyrus.Shared.DTOs;

namespace Papyrus.Logic.Services;

public class RoleService : MapperRepository<Role, string, string>, IRoleService
{
    private readonly ILanguageService languageService;
    private readonly ITranslationService translationService;
    private readonly string TranslationSegment = "Role";

    public RoleService(PapyrusContext context, ILoggerService logger, IUtilsService<string> utils, IMapper mapper, ILanguageService languageService, ITranslationService translationService) : base(context, logger, utils, mapper, "Role")
    {
        this.languageService = languageService;
        this.translationService = translationService;
    }

    public List<RoleDTO> GetAllTranslated(string? lang = null)
    {
        var roles = GetAll();

        string current = languageService.GetLangOrUserLang(lang);

        var translations = translationService.GetValues(TranslationSegment, current);

        return roles.Select(role =>
        {
            var dto = Mapper.Map<RoleDTO>(role);

            var t = translations.Where(x => x.Key == role.Name).FirstOrDefault()?.Value ?? role.Name ?? string.Empty;
            dto.Name = t;
            dto.NameEN = role.Name ?? string.Empty;

            return dto;
        }).ToList();
    }
}
