using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Side
{
    [SerializeField] public EnemySides side;
    private List<string> enemyTags = new List<string>();
    private List<string> enemyLayers = new List<string>();
    private List<string> alliesTags = new List<string>();
    /// <summary>
    /// ???
    /// </summary>
    private string ownSideLayer;

    private int maskVision;

    //private void Awake()
    //{
    //    Init(side, true);
    //}
    public void Init(EnemySides side, bool isPlayerEnemy)
    {
        switch (side)
        {
            case EnemySides.believers:
                if (isPlayerEnemy)
                {
                    maskVision = Helper.SetMask(new string[] { "Player", "Default", "EnemyFalcons" }, "EnemyBeliever");
                    enemyTags.Add("Player");
                    enemyLayers.Add("Player");
                }
                else
                {
                    maskVision = Helper.SetMask(new string[] { "Default", "EnemyFalcons" }, "EnemyBeliever");
                }
                alliesTags.Add("EnemyBeliever");
                enemyTags.Add("EnemyFalcons");
                enemyLayers.Add("EnemyFalcons");
                ownSideLayer = "EnemyBeliever";
                break;
            case EnemySides.falcons:
                if (isPlayerEnemy)
                {
                    maskVision = Helper.SetMask(new string[] { "Player", "Default", "EnemyBeliever" }, "EnemyFalcons");
                    enemyTags.Add("Player");
                    enemyLayers.Add("Player");
                }
                else
                {
                    maskVision = Helper.SetMask(new string[] { "Default", "EnemyBeliever" }, "EnemyFalcons");
                }
                alliesTags.Add("EnemyFalcons");
                enemyTags.Add("EnemyBeliever");
                enemyLayers.Add("EnemyBeliever");
                ownSideLayer = "EnemyFalcons";
                break;
            case EnemySides.neutral:
                maskVision = Helper.SetMask(new string[] { "EnemyFalcons", "EnemyBeliever", "Default" }, new string[] { "Player" });
                enemyTags.Add("EnemyFalcons");
                enemyTags.Add("EnemyBeliever");
                enemyLayers.Add("EnemyFalcons");
                enemyLayers.Add("EnemyBeliever");
                ownSideLayer = "EnemyNeutral";
                break;
            case EnemySides.agressive:
                maskVision = Helper.SetMask(new string[] { "Player", "EnemyFalcons", "EnemyBeliever", "Default" }, new string[] { });
                enemyTags.Add("Player");
                enemyTags.Add("EnemyFalcons");
                enemyTags.Add("EnemyBeliever");
                enemyLayers.Add("Player");
                enemyLayers.Add("EnemyFalcons");
                enemyLayers.Add("EnemyBeliever");
                ownSideLayer = "Enemy";
                break;
            case EnemySides.playerNeutral:
                enemyTags.Add("EnemyFalcons");
                enemyTags.Add("EnemyBeliever");
                enemyLayers.Add("EnemyFalcons");
                enemyLayers.Add("EnemyBeliever");
                ownSideLayer = "Player";
                break;
            case EnemySides.playerBeliever:
                enemyTags.Add("EnemyFalcons");
                enemyLayers.Add("EnemyFalcons");
                ownSideLayer = "Player";
                break;
            case EnemySides.playerFalcons:
                enemyTags.Add("EnemyBeliever");
                enemyLayers.Add("EnemyBeliever");
                ownSideLayer = "Player";
                break;
        }
    }
    public Side()
    {

    }

    public List<string> GetTagsEnemy()
    {
        return enemyTags;
    }
    public Side(EnemySides side, bool isPlayerEnemy)
    {
        Init(side, isPlayerEnemy);
    }

    public bool IsEnemyMask(string mask)
    {
        return enemyLayers.Contains(mask);
    }
    public bool IsEnemyMask(int mask)
    {
        return enemyLayers.Contains(LayerMask.LayerToName(mask));
    }
    public bool IsEnemyTag(string tag)
    {
        return enemyTags.Contains(tag);
    }

    public int GetTargetMask()
    {
        return maskVision;
    }

    public string GetOwnLayer()
    {
        return ownSideLayer;
    }

    public Side CreateSideBullet()
    {
        var sideBullet = new Side();
        sideBullet.maskVision = this.maskVision;
        sideBullet.enemyLayers = this.enemyLayers;
        sideBullet.enemyTags = this.enemyTags;
        if(this.ownSideLayer == "EnemyBeliever")
        {
            sideBullet.ownSideLayer = "BelieverProjectile";
        }
        else if(this.ownSideLayer == "EnemyFalcons")
        {
            sideBullet.ownSideLayer = "FalconsProjectile";
        }
        else if(this.ownSideLayer == "Player")
        {
            sideBullet.ownSideLayer = "PlayerProjectile";
        }


        return sideBullet;
    }

    //public virtual bool IsNotMineSide(string layer)
    //{
    //    return layer != this.transform.
    //}
}
