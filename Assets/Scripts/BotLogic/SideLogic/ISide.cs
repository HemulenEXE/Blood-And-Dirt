using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Side : MonoBehaviour
{
    [SerializeField] public EnemySides side;
    private List<string> enemyTags = new List<string>();
    private List<string> enemyLayers = new List<string>();
    private List<string> alliesTags = new List<string>();
    /// <summary>
    /// Свой слой
    /// </summary>
    private string ownSideLayer;

    private int maskVision;

    private void Awake()
    {
        Init(side, true);
    }
    public void Init(EnemySides side, bool isPlayerEnemy)
    {
        switch (side)
        {
            case EnemySides.believers:
                if (isPlayerEnemy)
                {
                    maskVision = Helper.SetMask(new string[] { "Player", "Default", "EnemyFalcons" }, "EnemyBelievers");
                    enemyTags.Add("Player");
                    enemyLayers.Add("Player");
                }
                else
                {
                    maskVision = Helper.SetMask(new string[] { "Default", "EnemyFalcons" }, "EnemyBelievers");
                }
                alliesTags.Add("EnemyBelievers");
                enemyTags.Add("EnemyFalcons");
                enemyLayers.Add("EnemyFalcons");
                ownSideLayer = "EnemyBelievers";
                break;
            case EnemySides.falcons:
                if (isPlayerEnemy)
                {
                    maskVision = Helper.SetMask(new string[] { "Player", "Default", "EnemyBelievers" }, "EnemyFalcons");
                    enemyTags.Add("Player");
                    enemyLayers.Add("Player");
                }
                else
                {
                    maskVision = Helper.SetMask(new string[] { "Default", "EnemyBelievers" }, "EnemyFalcons");
                }
                alliesTags.Add("EnemyFalcons");
                enemyTags.Add("EnemyBelievers");
                enemyLayers.Add("EnemyBelievers");
                ownSideLayer = "EnemyFalcons";
                break;
            case EnemySides.neutral:
                maskVision = Helper.SetMask(new string[] { "EnemyFalcons", "EnemyBelievers", "Default" }, new string[] { "Player" });
                enemyTags.Add("EnemyFalcons");
                enemyTags.Add("EnemyBelievers");
                enemyLayers.Add("EnemyFalcons");
                enemyLayers.Add("EnemyBelievers");
                ownSideLayer = "EnemyNeutral";
                break;
            case EnemySides.agressive:
                maskVision = Helper.SetMask(new string[] { "Player", "EnemyFalcons", "EnemyBelievers", "Default" }, new string[] { });
                enemyTags.Add("Player");
                enemyTags.Add("EnemyFalcons");
                enemyTags.Add("EnemyBelievers");
                enemyLayers.Add("Player");
                enemyLayers.Add("EnemyFalcons");
                enemyLayers.Add("EnemyBelievers");
                ownSideLayer = "Enemy";
                break;
            case EnemySides.playerNeutral:
                enemyTags.Add("EnemyFalcons");
                enemyTags.Add("EnemyBelievers");
                enemyLayers.Add("EnemyFalcons");
                enemyLayers.Add("EnemyBelievers");
                ownSideLayer = "Player";
                break;
            case EnemySides.playerBeliever:
                enemyTags.Add("EnemyFalcons");
                enemyLayers.Add("EnemyFalcons");
                ownSideLayer = "Player";
                break;
            case EnemySides.playerFalcons:
                enemyTags.Add("EnemyBelievers");
                enemyLayers.Add("EnemyBelievers");
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
        if(this.ownSideLayer == "EnemyBelievers")
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
