using System.Threading;
using System.Threading.Tasks;

namespace Aiva.Core.Twitch.Tasks {
    public class Statistics
    {
        private readonly Database.Handlers.Statistics _statisticsDatabaseHandler;

        public Statistics() {
            _statisticsDatabaseHandler = new Database.Handlers.Statistics();

            Task.Factory.StartNew(() => StartViewerStaticRecording());
        }

        private async void StartViewerStaticRecording() {
            while(true) {
                Thread.Sleep(300000); // wait 5 min to update this

                var stream = await AivaClient.Instance.TwitchApi.Streams.v5.GetStreamByUserAsync
                    (AivaClient.Instance.ChannelId).ConfigureAwait(false);

                if(stream != null) { // check if offline
                    _statisticsDatabaseHandler.AddViewerCountToDatabase(stream.Stream.Viewers);
                }
            }
        }
    }
}
