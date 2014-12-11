using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoLLauncher.RiotObjects.Platform.Statistics
{

    public class SummaryAggStats : RiotGamesObject
    {
        public override string TypeName
        {
            get
            {
                return this.type;
            }
        }

        private string type = "com.riotgames.platform.statistics.SummaryAggStats";

        public SummaryAggStats()
        {
        }

        public SummaryAggStats(Callback callback)
        {
            this.callback = callback;
        }

        public SummaryAggStats(TypedObject result)
        {
            base.SetFields(this, result);
        }

        public delegate void Callback(SummaryAggStats result);

        private Callback callback;

        public override void DoCallback(TypedObject result)
        {
            base.SetFields(this, result);
            callback(this);
        }

        [InternalName("statsJson")]
        public object StatsJson { get; set; }

        [InternalName("stats")]
        public List<SummaryAggStat> Stats { get; set; }

    }
}
