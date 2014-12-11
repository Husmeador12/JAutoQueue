using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoLLauncher.RiotObjects.Platform.Matchmaking
{

    public class SearchingForMatchNotification : RiotGamesObject
    {
        public override string TypeName
        {
            get
            {
                return this.type;
            }
        }

        private string type = "com.riotgames.platform.matchmaking.SearchingForMatchNotification";

        public SearchingForMatchNotification()
        {
        }

        public SearchingForMatchNotification(Callback callback)
        {
            this.callback = callback;
        }

        public SearchingForMatchNotification(TypedObject result)
        {
            base.SetFields(this, result);
        }

        public delegate void Callback(SearchingForMatchNotification result);

        private Callback callback;

        public override void DoCallback(TypedObject result)
        {
            base.SetFields(this, result);
            callback(this);
        }

        [InternalName("playerJoinFailures")]
        public QueueDodger PlayerJoinFailures { get; set; }

        [InternalName("ghostGameSummoners")]
        public object GhostGameSummoners { get; set; }

        [InternalName("joinedQueues")]
        public List<QueueInfo> JoinedQueues { get; set; }

    }
}
