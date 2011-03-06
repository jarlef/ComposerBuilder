using EPiServer.Cms.Shell.UI;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;

namespace ComposerBuilder.Synchronization
{
    [ModuleDependency(typeof(PageTypeBuilder.Initializer), typeof(InitializableModule))]
    public class SynchronizationModule : IInitializableModule
    {
        public void Initialize(InitializationEngine context)
        {
            var pageUpdater = new ComposerPageUpdater();
            pageUpdater.SyncPages();

            var functionUpdater = new ComposerFunctionUpdater();
            functionUpdater.SyncFunctions();

        }

        public void Uninitialize(InitializationEngine context)
        {
        }

        public void Preload(string[] parameters)
        {
        }
    }
}