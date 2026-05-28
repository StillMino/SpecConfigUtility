using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
namespace SpecConfig.Infrastructure.Com
{
    public class NanoCadComAdapter
    {
        private readonly StaComDispatcher _dispatcher;
        public NanoCadComAdapter(StaComDispatcher dispatcher) { _dispatcher = dispatcher; }
        public bool IsConnected { get; private set; }
        public async Task<bool> ConnectAsync() => await _dispatcher.InvokeAsync(() => { IsConnected = true; return true; });
        public async Task<string> GetActiveDocumentNameAsync() => await _dispatcher.InvokeAsync(() => "Test.dwg");
        public void Disconnect() => IsConnected = false;
    }
}
