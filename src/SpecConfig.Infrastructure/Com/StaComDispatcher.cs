using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace SpecConfig.Infrastructure.Com
{
    public sealed class StaComDispatcher : IDisposable
    {
        private readonly Thread _thread;
        private readonly AutoResetEvent _ready = new(false);
        private SynchronizationContext? _ctx;
        public StaComDispatcher()
        {
            _thread = new Thread(() => { _ctx = new WindowsFormsSynchronizationContext(); SynchronizationContext.SetSynchronizationContext(_ctx); _ready.Set(); Application.Run(); });
            _thread.SetApartmentState(ApartmentState.STA);
            _thread.Start();
            _ready.WaitOne();
        }
        public async Task<T> InvokeAsync<T>(Func<T> func, int timeoutMs = 10000)
        {
            var tcs = new TaskCompletionSource<T>();
            using var cts = new CancellationTokenSource(timeoutMs);
            cts.Token.Register(() => tcs.TrySetException(new TimeoutException("Timeout")));
            _ctx?.Post(_ => { try { tcs.SetResult(func()); } catch (Exception ex) { tcs.SetException(ex); } }, null);
            return await tcs.Task;
        }
        public void Dispose() { _ctx?.Post(_ => Application.Exit(), null); _thread.Join(3000); }
    }
}
