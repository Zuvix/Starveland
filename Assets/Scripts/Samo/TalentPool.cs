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
            new TalentCarryingCapacity("Increased carrying capacity talent", 5),
            new TalentGatheringSpeed("Increased gathering speed talent", 0.5f)
        };

        // Add unit specific talents to the pool TODO
    }

    // generate random new skill specific talent
    public TalentSkillSpecific GetNewSkillSpecificTalent(List<TalentSkillSpecific> SkillAppliedTalents, int Level)
    {
        // create empty talent pool
        List<TalentSkillSpecific> SkillSpecificTalentsFree = new List<TalentSkillSpecific>();

        // iterate through all talents and those which aren't already applied add to the free pool
        if (SkillAppliedTalents.Count > 0)
        {
            foreach (var talentAll in this.SkillSpecificTalents)
            {
                bool IsThere = false;
                foreach (var talentApplied in SkillAppliedTalents)
                {
                    if (talentAll.GetType() == talentApplied.GetType())
                    {
                        // talent of this effect is already applied, skip
                        IsThere = true;
                        break;
                    }
                }
                if (!IsThere) 
                {
                    SkillSpecificTalentsFree.Add(talentAll);
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

        return ChoosenTalent?.CreateNewInstanceOfSelf(Level);
    }

}
