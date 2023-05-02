using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealItemController : BaseController
{
    public PlayerController player;
    public int healValue = 0;
    public StatName addedStat = StatName.None;
    public int addedStatValue = 0;
    public float addedStatMultipli = 0f;
    public long ID { get; set; }
    public override void InitializeController()
    {
        player = SpawnManager.Instance.player.GetComponent<PlayerController>();
        ChangeState(new HealItemIdleState());
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //
        if (collision.gameObject.CompareTag("Player"))
        {
            player.Heal(healValue);
            if(addedStat != StatName.None)
            {
                player.AddStatValue(addedStat, addedStatValue);
                player.AddStatMuliply(addedStat, addedStatMultipli);
            }
            Used();
        }
    }
    public void Used()
    {
        ChangeState(new HealItemUsedState());
    }
}
