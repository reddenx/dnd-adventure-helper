using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdventureHelper.Website.Models.Character
{
    public class PlayerDto : IIdentifiable
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }

        public int Strength { get; set; }
        public int Dexterity { get; set; }
        public int Constitution { get; set; }
        public int Intelligence { get; set; }
        public int Wisdom { get; set; }
        public int Charisma { get; set; }

        public int ProficiencyBonus { get; set; }

        public SkillDto Acrobatics { get; set; }

        public string[] Proficiencies { get; set; }
        public string[] Languages { get; set; }

        public int ArmorClassModifier { get; set; }
        public int InitiativeModifier { get; set; }
        public int Speed { get; set; }

        public int HpMax { get; set; }
        public int HpCurrent { get; set; }
        public int TempHp { get; set; }
        public int HitDiceType { get; set; }
        public int HitDiceCount { get; set; }

        public int DeathSaveSuccess { get; set; }
        public int DeathSaveFailure { get; set; }

        public string PersonalityTraits { get; set; }
        public string Ideals { get; set; }
        public string Bonds { get; set; }
        public string Flaws { get; set; }
    }

    public class SkillDto
    {
        public bool Proficient { get; set; }
        public int Modifier { get; set; }
    }
}