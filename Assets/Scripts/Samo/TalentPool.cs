using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class TalentPool : Singleton<TalentPool>
{
    private readonly List<TalentSkillSpecific> SkillSpecificTalents;

    private TalentPool()
    {

        // Add skill specific talents to the pool
        SkillSpecificTalents = new List<TalentSkillSpecific>
        {
            new TalentCarryingCapacity("Increased carrying capacity talent", 5)
        };

        // Add unit specific talents to the pool TODO
    }

    // generate random new skill specific talent
    public TalentSkillSpecific GetNewSkillSpecificTalent(List<TalentSkillSpecific> SkillAppliedTalents, int Level)
    {
        // create copy to delete from
        List<TalentSkillSpecific> SkillSpecificTalentsFree = new List<TalentSkillSpecific>();

        // iterate through all talents and those which aren't already applied add to free pool
        if (SkillAppliedTalents.Count > 0)
        {
            foreach (var talentAll in this.SkillSpecificTalents)
            {
                foreach (var talentApplied in SkillAppliedTalents)
                {
                    if (talentAll.GetType() != talentApplied.GetType())
                    {
                        SkillSpecificTalentsFree.Add(talentAll);
                        break;
                    }
                }
            }
        }
        // all talents are available
        else
        {
            SkillSpecificTalentsFree = this.SkillSpecificTalents;
        }

        // generate random index (new talent)
        TalentSkillSpecific ChoosenTalent = null;
        if (SkillSpecificTalentsFree.Count() > 0)
        {
            ChoosenTalent = SkillSpecificTalentsFree.ElementAt(Random.Range(0, SkillSpecificTalentsFree.Count()));
        }

        return ChoosenTalent == null ? null : ChoosenTalent.CreateNewInstanceOfSelf(Level);
    }

}
