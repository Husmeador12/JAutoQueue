using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoLLauncher.RiotObjects.Platform.Summoner
{

    public class Summoner : RiotGamesObject
    {
        public override string TypeName
        {
            get
            {
                return this.type;
            }
        }

        private string type = "com.riotgames.platform.summoner.Summoner";

        public Summoner()
        {
        }

        public Summoner(Callback callback)
        {
            this.callback = callback;
        }

        public Summoner(TypedObject result)
        {
            base.SetFields(this, result);
        }

        public delegate void Callback(Summoner result);

        private Callback callback;

        public override void DoCallback(TypedObject result)
        {
            base.SetFields(this, result);
            callback(this);
        }

        [InternalName("seasonTwoTier")]
        public String SeasonTwoTier { get; set; }

        [InternalName("internalName")]
        public String InternalName { get; set; }

        [InternalName("acctId")]
        public Double AcctId { get; set; }

        [InternalName("helpFlag")]
        public Boolean HelpFlag { get; set; }

        [InternalName("sumId")]
        public Double SumId { get; set; }

        [InternalName("profileIconId")]
        public Int32 ProfileIconId { get; set; }

        [InternalName("displayEloQuestionaire")]
        public Boolean DisplayEloQuestionaire { get; set; }

        [InternalName("lastGameDate")]
        public DateTime LastGameDate { get; set; }

        [InternalName("advancedTutorialFlag")]
        public Boolean AdvancedTutorialFlag { get; set; }

        [InternalName("revisionDate")]
        public DateTime RevisionDate { get; set; }

        [InternalName("revisionId")]
        public Double RevisionId { get; set; }

        [InternalName("seasonOneTier")]
        public String SeasonOneTier { get; set; }

        [InternalName("name")]
        public String Name { get; set; }

        [InternalName("nameChangeFlag")]
        public Boolean NameChangeFlag { get; set; }

        [InternalName("tutorialFlag")]
        public Boolean TutorialFlag { get; set; }

        [InternalName("socialNetworkUserIds")]
        public List<object> SocialNetworkUserIds { get; set; }

    }
}
