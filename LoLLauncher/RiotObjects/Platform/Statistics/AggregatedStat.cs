using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoLLauncher.RiotObjects.Platform.Statistics
{

    public class AggregatedStat : RiotGamesObject
    {
        public override string TypeName
        {
            get
            {
                return this.type;
            }
        }

        private string type = "com.riotgames.platform.statistics.AggregatedStat";

        public AggregatedStat()
        {
        }

        public AggregatedStat(Callback callback)
        {
            this.callback = callback;
        }

        public AggregatedStat(TypedObject result)
        {
            base.SetFields(this, result);
        }

        public delegate void Callback(AggregatedStat result);

        private Callback callback;

        public override void DoCallback(TypedObject result)
        {
            base.SetFields(this, result);
            callback(this);
        }

        [InternalName("statType")]
        public String StatType { get; set; }

        [InternalName("count")]
        public Double Count { get; set; }

        [InternalName("value")]
        public Double Value { get; set; }

        [InternalName("championId")]
        public Int32 ChampionId { get; set; }

    }
}
