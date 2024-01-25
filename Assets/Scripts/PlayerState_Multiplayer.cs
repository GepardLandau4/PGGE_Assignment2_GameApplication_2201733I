using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PGGE.Patterns;

//Renamed some things in this script so that the names will be more consistent with the other scripts
public class PlayerState_Multiplayer : FSMState
{
    protected Player_Multiplayer mPlayer = null;

    public PlayerState_Multiplayer(Player_Multiplayer player) 
        : base()
    {
        mPlayer = player;
        mFsm = mPlayer.mFsm;
    }

    public override void Enter()
    {
        base.Enter();
    }
    public override void Exit()
    {
        base.Exit();
    }
    public override void Update()
    {
        base.Update();
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}

//changed class name to better fit naming convention
public class PlayerState_Multiplayer_Movement : PlayerState_Multiplayer
{
    public PlayerState_Multiplayer_Movement(Player_Multiplayer player) : base(player)
    {
        mId = (int)(PlayerStateType.movement);
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        mPlayer.Move();

        for (int i = 0; i < mPlayer.mAttackButtons.Length; ++i)
        {
            if (mPlayer.mAttackButtons[i])
            {
                if (mPlayer.mBulletsInMagazine > 0)
                {
                    PlayerState_Multiplayer_Attack attack =
                  (PlayerState_Multiplayer_Attack)mFsm.GetState(
                            (int)PlayerStateType.attack);

                    attack.attackID = i;
                    mPlayer.mFsm.SetCurrentState(
                        (int)PlayerStateType.attack);
                }
                else
                {
                    Debug.Log("No more ammo left");
                }
            }
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}

//changed class name to better fit naming convention
public class PlayerState_Multiplayer_Attack : PlayerState_Multiplayer
{
    private int mAttackID = 0;
    private string mAttackName;

    public int attackID
    {
        get
        {
            return mAttackID;
        }
        set
        {
            mAttackID = value;
            mAttackName = "Attack" + (mAttackID + 1).ToString();
        }
    }

    public PlayerState_Multiplayer_Attack(Player_Multiplayer player) : base(player)
    {
        mId = (int)(PlayerStateType.attack);
    }

    public override void Enter()
    {
        mPlayer.mAnimator.SetBool(mAttackName, true);
    }
    public override void Exit()
    {
        mPlayer.mAnimator.SetBool(mAttackName, false);
    }
    public override void Update()
    {
        base.Update();

        Debug.Log("Ammo count: " + mPlayer.mAmunitionCount + ", In Magazine: " + mPlayer.mBulletsInMagazine);
        if (mPlayer.mBulletsInMagazine == 0 && mPlayer.mAmunitionCount > 0)
        {
            mPlayer.mFsm.SetCurrentState((int)PlayerStateType.reload);
            return;
        }

        if (mPlayer.mAmunitionCount <= 0 && mPlayer.mBulletsInMagazine <= 0)
        {
            mPlayer.mFsm.SetCurrentState((int)PlayerStateType.movement);
            mPlayer.NoAmmo();
            return;
        }

        if (mPlayer.mAttackButtons[mAttackID])
        {
            mPlayer.mAnimator.SetBool(mAttackName, true);
            mPlayer.Fire(attackID);
        }
        else
        {
            mPlayer.mAnimator.SetBool(mAttackName, false);
            mPlayer.mFsm.SetCurrentState((int)PlayerStateType.movement);
        }
    }
}

//changed class name to better fit naming convention
public class PlayerState_Multiplayer_Reload : PlayerState_Multiplayer
{
    //changed variable name to better fit naming convention
    public float mReloadTime = 3.0f;
    float dt = 0.0f;
    public int previousState;
    public PlayerState_Multiplayer_Reload(Player_Multiplayer player) : base(player)
    {
        mId = (int)(PlayerStateType.reload);
    }

    public override void Enter()
    {
        mPlayer.mAnimator.SetTrigger("Reload");
        mPlayer.Reload();
        dt = 0.0f;
    }
    public override void Exit()
    {
        if (mPlayer.mAmunitionCount > mPlayer.mMaxAmunitionBeforeReload)
        {
            mPlayer.mBulletsInMagazine += mPlayer.mMaxAmunitionBeforeReload;
            mPlayer.mAmunitionCount -= mPlayer.mBulletsInMagazine;
        }
        else if (mPlayer.mAmunitionCount > 0 && mPlayer.mAmunitionCount < mPlayer.mMaxAmunitionBeforeReload)
        {
            mPlayer.mBulletsInMagazine += mPlayer.mAmunitionCount;
            mPlayer.mAmunitionCount = 0;
        }
    }

    public override void Update()
    {
        dt += Time.deltaTime;
        if (dt >= mReloadTime)
        {
            mPlayer.mFsm.SetCurrentState((int)PlayerStateType.movement);
        }
    }

    public override void FixedUpdate()
    {
    }
}
