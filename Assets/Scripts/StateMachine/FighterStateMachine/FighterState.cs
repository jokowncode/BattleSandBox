
public class FighterState : State {
    
    protected Fighter Controller;

    protected virtual void Awake(){
        Controller = GetComponent<Fighter>();
    }        
}



