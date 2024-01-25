using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PGGE.Patterns;

//Renamed some things in this script so that the names will be more consistent with the other scripts
public enum PlayerStateType
{
    movement = 0,
    attack,
    reload,
}

//changed class name to better fit naming convention
public class PlayerState : FSMState
{
    protected Player mPlayer = null;

    public PlayerState(Player player) 
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
public class PlayerState_Movement : PlayerState
{
    public PlayerState_Movement(Player player) : base(player)
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
                    PlayerState_Attack attack =
                        (PlayerState_Attack)mFsm.GetState(
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
public class PlayerState_Attack : PlayerState
{
    private int mAttackID = 0;
    private string mAttackName;

    //changed variable name to better fit naming convention
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

    public PlayerState_Attack(Player player) : base(player)
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
public class PlayerState_Reload : PlayerState
{
    //changed variable name to better fit naming convention
    public float mReloadTime = 3.0f;
    float dt = 0.0f;
    public int previousState;
    public PlayerState_Reload(Player player) : base(player)
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
