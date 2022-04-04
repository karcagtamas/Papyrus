using Karcags.Common.Tools.Entities;
using Microsoft.AspNetCore.Identity;

namespace Papyrus.DataAccess.Entities;

public class Role : IdentityRole<string>, IEntity<string>
{
}
