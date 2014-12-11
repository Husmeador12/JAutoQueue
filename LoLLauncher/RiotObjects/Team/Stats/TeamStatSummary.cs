using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LoLLauncher.RiotObjects.Team;

namespace LoLLauncher.RiotObjects.Team.Stats
{

    public class TeamStatSummary : RiotGamesObject
    {
        public override string TypeName
        {
            get
            {
                return this.type;
            }
        }

        private string type = "com.riotgames.team.stats.TeamStatSummary";

        public TeamStatSummary()
        {
        }

        public TeamStatSummary(Callback callback)
        {
            this.callback = callback;
        }

        public TeamStatSummary(TypedObject result)
        {
            base.SetFields(this, result);
        }

        public delegate void Callback(TeamStatSummary result);

        private Callback callback;

        public override void DoCallback(TypedObject result)
        {
            base.SetFields(this, result);
            callback(this);
        }

        [InternalName("teamStatDetails")]
        public List<TeamStatDetail> TeamStatDetails { get; set; }

        [InternalName("teamIdString")]
        public String TeamIdString { get; set; }

        [InternalName("teamId")]
        public TeamId TeamId { get; set; }

    }
}
