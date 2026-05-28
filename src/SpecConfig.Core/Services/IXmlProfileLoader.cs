using System.Threading.Tasks;
using SpecConfig.Core.Models;
namespace SpecConfig.Core.Services
{
    public interface IXmlProfileLoader
    {
        Task<ExportProfile> LoadExportAsync(string path);
        Task<SpecifierProfile> LoadSpecifierAsync(string path);
        Task SaveExportAsync(ExportProfile profile, string path);
        Task SaveSpecifierAsync(SpecifierProfile profile, string path);
    }
    public interface ISqlDataAccessor { }
}
