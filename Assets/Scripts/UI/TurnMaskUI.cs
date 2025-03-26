using UnityEngine;

public class TurnMaskUI : MonoBehaviour
{
    HealthBarUI barUI;

    private void Awake() {
        barUI = GetComponentInChildren<HealthBarUI>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //HACK: this is a very hack way but here we go
        if(CombatManager.Instance.IsCombatStarted && TurnManager.Instance.IsActorTurn(GameManager.Instance.PlayerCharacter))
        {
            barUI.gameObject.SetActive(true);
        }else{
            barUI.gameObject.SetActive(false);
        }
        
    }
}
