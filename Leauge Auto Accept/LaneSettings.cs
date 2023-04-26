using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Leauge_Auto_Accept
{
    public class LaneSettings
    {
        public string Lane { get; set; }
        public string ChampName { get; set; }
        public string ChampId { get; set; }
        public string BanName { get; set; }
        public string BanId { get; set; }
        public string Spell1Name { get; set; }
        public string Spell1Id { get; set; }
        public string Spell2Name { get; set; }
        public string Spell2Id { get; set; }

        public LaneSettings(string lane = "Top", string champName = "None", string champId = "0", string banName = "None", string banId = "0", string spell1Name = "None", string spell1Id = "0", string spell2Name = "None", string spell2Id = "0")
        {
            Lane = lane;
            ChampName = champName;
            ChampId = champId;
            BanName = banName;
            BanId = banId;
            Spell1Name = spell1Name;
            Spell1Id = spell1Id;
            Spell2Name = spell2Name;
            Spell2Id = spell2Id;
        }
    }

}
