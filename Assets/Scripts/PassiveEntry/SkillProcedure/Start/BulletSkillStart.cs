
using UnityEngine;

public class BulletSkillStart : SkillStart
{

    //public Component addComponent;
    public override void AdditionalProcedure(GameObject target)
    {
        Debug.Log("BulletSkillStart: "+target.name);
        target.AddComponent<BulletSkillStart>();
    }

}

