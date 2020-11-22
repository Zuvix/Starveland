using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class TalentPool : Singleton<TalentPool>
{
    private readonly List<TalentSkillSpecific> SkillSpecificTalents;
    private readonly List<TalentUnitSpecific> UnitSpecificTalents;

    private TalentPool()
    {

        // Add skill specific talents to the pool
        SkillSpecificTalents = new List<TalentSkillSpecific>
        {
            new TalentCarryingCapacity("Carrying capacity ",
                                       GameConfigManager.Instance.GameConfig.CarryingCapacityTalent,
                                       GameConfigManager.Instance.GameConfig.CarryingCapacityTalentIcon
                                       ),
            new TalentGatheringSpeed("Gathering speed ", 
                                     GameConfigManager.Instance.GameConfig.GatheringSpeedTalent,
                                     GameConfigManager.Instance.GameConfig.GatheringSpeedTalentIcon)
        };

        // Add unit specific talents to the pool
        UnitSpecificTalents = new List<TalentUnitSpecific>
        {
            new TalentMovementSpeed("Movement speed ", 
                                    GameConfigManager.Instance.GameConfig.MovementSpeedTalent,
                                    GameConfigManager.Instance.GameConfig.MovementSpeedTalentIcon)
        };
    }

    public TalentSkillSpecific GetNewSkillSpecificTalent(List<TalentSkillSpecific> SkillAppliedTalents, int Level)
    {
        TalentSkillSpecific t = (TalentSkillSpecific)GetNewTalent(
            SkillAppliedTalents.Cast<Talent>().ToList(), this.SkillSpecificTalents.Cast<Talent>().ToList());

        return t?.CreateNewInstanceOfSelf(Level);
    }

    public TalentUnitSpecific GetNewUnitSpecificTalent(List<TalentUnitSpecific> UnitAppliedTalents, int Level)
    {
        TalentUnitSpecific t = (TalentUnitSpecific)GetNewTalent(
            UnitAppliedTalents.Cast<Talent>().ToList(), this.UnitSpecificTalents.Cast<Talent>().ToList());

        return t?.CreateNewInstanceOfSelf(Level);
    }

    // generate random new skill specific talent
    private Talent GetNewTalent(List<Talent> AppliedTalents, List<Talent> AllTalents)
    {
        // create empty talent pool
        List<Talent> TalentsFree = new List<Talent>();

        // iterate through all talents and those which aren't already applied add to the free pool
        if (AppliedTalents.Count > 0)
        {
            foreach (var talentAll in AllTalents)
            {
                bool IsThere = false;
                foreach (var talentApplied in AppliedTalents)
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
                    TalentsFree.Add(talentAll);
                }
            }
        }
        // all talents are available
        else
        {
            TalentsFree = AllTalents;
        }

        // generate random index (new talent)
        Talent ChoosenTalent = null;
        if (TalentsFree.Count() > 0)
        {
            ChoosenTalent = TalentsFree.ElementAt(Random.Range(0, TalentsFree.Count()));
        }

        return ChoosenTalent;
    }

}
