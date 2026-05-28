using System.Threading.Tasks;
using SpecConfig.Core.Models;
using SpecConfig.Core.Services;
namespace SpecConfig.Infrastructure.Xml
{
    public class XmlProfileLoader : IXmlProfileLoader
    {
        public Task<ExportProfile> LoadExportAsync(string path) => Task.FromResult(new ExportProfile());
        public Task<SpecifierProfile> LoadSpecifierAsync(string path) => Task.FromResult(new SpecifierProfile());
        public Task SaveExportAsync(ExportProfile profile, string path) => Task.CompletedTask;
        public Task SaveSpecifierAsync(SpecifierProfile profile, string path) => Task.CompletedTask;
    }
}
