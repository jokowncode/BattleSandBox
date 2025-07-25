
using System;
using UnityEngine;

public class Fighter : MonoBehaviour{

    [SerializeField] protected FighterData InitialData;

    private FighterData CurrentData;
    
    protected virtual void Awake(){
        // Clone Fighter 
        this.CurrentData = Instantiate(this.InitialData);
    }

    // Initial Property
    public float InitialHealth{ 
        get => InitialData.Health;
        set => InitialData.Health=value;
    }
    public float InitialPhysicsAttack{ 
        get => InitialData.PhysicsAttack;
        set => InitialData.PhysicsAttack=value;
    }
    public float InitialMagicAttack{ 
        get => InitialData.MagicAttack;
        set => InitialData.MagicAttack=value;
    }
    public float InitialCooldown{ 
        get => InitialData.Cooldown;
        set => InitialData.Cooldown=value;
    }
    public float InitialAttackRadius{ 
        get => InitialData.AttackRadius;
        set => InitialData.AttackRadius=value;
    }
    public float InitialCritical{ 
        get => InitialData.Critical;
        set => InitialData.Critical=value;
    }
    
    // Runtime Property
    public float Health{ 
        get => CurrentData.Health;
        set => CurrentData.Health=value;
    }
    public float PhysicsAttack{ 
        get => CurrentData.PhysicsAttack;
        set => CurrentData.PhysicsAttack=value;
    }
    public float MagicAttack{ 
        get => CurrentData.MagicAttack;
        set => CurrentData.MagicAttack=value;
    }
    public float Cooldown{ 
        get => CurrentData.Cooldown;
        set => CurrentData.Cooldown=value;
    }
    public float AttackRadius{ 
        get => CurrentData.AttackRadius;
        set => CurrentData.AttackRadius=value;
    }
    public float Critical{ 
        get => CurrentData.Critical;
        set => CurrentData.Critical=value;
    }
}

