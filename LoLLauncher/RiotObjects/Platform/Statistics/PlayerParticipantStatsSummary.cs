using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoLLauncher.RiotObjects.Platform.Statistics
{

    public class PlayerParticipantStatsSummary : RiotGamesObject
    {
        public override string TypeName
        {
            get
            {
                return this.type;
            }
        }

        private string type = "com.riotgames.platform.statistics.PlayerParticipantStatsSummary";

        public PlayerParticipantStatsSummary()
        {
        }

        public PlayerParticipantStatsSummary(Callback callback)
        {
            this.callback = callback;
        }

        public PlayerParticipantStatsSummary(TypedObject result)
        {
            base.SetFields(this, result);
        }

        public delegate void Callback(PlayerParticipantStatsSummary result);

        private Callback callback;

        public override void DoCallback(TypedObject result)
        {
            base.SetFields(this, result);
            callback(this);
        }

        [InternalName("skinName")]
        public String SkinName { get; set; }

        [InternalName("gameId")]
        public Double GameId { get; set; }

        [InternalName("profileIconId")]
        public Int32 ProfileIconId { get; set; }

        [InternalName("elo")]
        public Int32 Elo { get; set; }

        [InternalName("leaver")]
        public Boolean Leaver { get; set; }

        [InternalName("leaves")]
        public Double Leaves { get; set; }

        [InternalName("teamId")]
        public Double TeamId { get; set; }

        [InternalName("eloChange")]
        public Int32 EloChange { get; set; }

        [InternalName("statistics")]
        public List<RawStatDTO> Statistics { get; set; }

        [InternalName("level")]
        public Double Level { get; set; }

        [InternalName("botPlayer")]
        public Boolean BotPlayer { get; set; }

        [InternalName("userId")]
        public Double UserId { get; set; }

        [InternalName("spell2Id")]
        public Double Spell2Id { get; set; }

        [InternalName("losses")]
        public Double Losses { get; set; }

        [InternalName("summonerName")]
        public String SummonerName { get; set; }

        [InternalName("wins")]
        public Double Wins { get; set; }

        [InternalName("spell1Id")]
        public Double Spell1Id { get; set; }

    }
}
