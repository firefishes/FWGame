#define _TEST_MOVER

using System;
using ShipDock.Applications;
using ShipDock.Notices;
using ShipDock.Pooling;
using UnityEngine;
using UnityEngine.AI;

namespace FWGame
{
    public class Role : MonoBehaviour
    {

        private const float k_Half = 0.5f;

#if TEST_MOVER
        [SerializeField]
        private Transform m_Mover;
#endif
        [SerializeField]
        private NavMeshAgent m_NavMeshAgent;
        [SerializeField]
        private float m_Hp;
        [SerializeField]
        private float m_Speed;
        [SerializeField]
        private int m_Camp;
        [SerializeField]
        private float m_RunCycleLegOffset = 0.2f;
        [SerializeField]
        private float m_AnimSpeedMultiplier = 1f;
        [SerializeField]
        private float m_MoveSpeedMultiplier = 1f;
        [SerializeField]
        private FWRoleMustSubgroup m_RoleMustSubgroup;
        [SerializeField]
        private Rigidbody m_RoleRigidbody;
        [SerializeField]
        private CapsuleCollider m_RoleCollider;
        [SerializeField]
        private Animator m_RoleAnimator;
        [SerializeField]
        private Transform m_CameraNode;

        private float mGroundCheckDistance = 0.3f;
        private Ray mCrouchRay;
        private RaycastHit mGroundHitInfo;
        private RoleAnimatorInfo mAnimatorInfo;
        private FWRole mRole;
        private FWRoleInput mRoleInput;
        private RoleData mRoleData = new RoleData();
        private Vector3 mInitPosition;
        private ComponentBridge mBrigae;
        private AnimatorStateInfo mAnimatorStateInfo;

        private void Awake()
        {
            Init();
            mBrigae = new ComponentBridge(OnInited);
            mBrigae.Start();
        }

        private void OnDestroy()
        {
            mBrigae?.Dispose();
            mBrigae = default;
            
            GetInstanceID().Remove(OnRoleNotices);
        }

        private void Init()
        {
            m_Camp = UnityEngine.Random.Range(0, 2);
            m_RoleMustSubgroup = new FWRoleMustSubgroup
            {
                roleColliderID = m_RoleCollider.GetInstanceID(),
                animatorID = m_RoleAnimator.GetInstanceID(),
                rigidbodyID = m_RoleRigidbody.GetInstanceID(),
                origGroundCheckDistance = mGroundCheckDistance
            };

            m_RoleRigidbody.constraints = RigidbodyConstraints.FreezeRotationX |
                                          RigidbodyConstraints.FreezeRotationY |
                                          RigidbodyConstraints.FreezeRotationZ;

            m_RoleMustSubgroup.Init(ref m_RoleCollider);
        }

        void Start()
        {
            mInitPosition = transform.localPosition;
        }

        private void OnInited()
        {
            mRole = new BananaRole
            {
                Position = mInitPosition
            };
            mRole.RoleMustSubgroup = m_RoleMustSubgroup;
            mRole.InitComponents();
            mRole.SpeedCurrent = mRole.Speed;
            mRoleData = mRole.RoleDataSource;
            mRole.Camp = m_Camp;
            mRole.SetSourceID(GetInstanceID());

            InitNotices();

            if(mRole.Camp == 0)
            {
                ParamNotice<FWRole> notice = Pooling<ParamNotice<FWRole>>.From();
                notice.ParamValue = mRole;
                FWConsts.COMPONENT_ROLE_CONTROLLABLE.Dispatch(notice);
                Pooling<ParamNotice<FWRole>>.To(notice);
            }
        }

        private void InitNotices()
        {
            GetInstanceID().Add(OnRoleNotices);
        }

        private void OnRoleNotices(INoticeBase<int> obj)
        {
            IParamNotice<int> notice = obj as IParamNotice<int>;
            switch(notice.ParamValue)
            {
                case FWConsts.NOTICE_PLAYER_ROLE_CHOOSEN:
                    FWConsts.SERVER_FW_LENS.DeliveParam<FWCamerasServer, Role>("SetChoosenPlayer", "PlayerRoleChoosen", OnRoleChoosen);
                    break;
            }
        }

        private void OnRoleChoosen(ref IParamNotice<Role> target)
        {
            (target as IParamNotice<Role>).ParamValue = this;
        }

        private void UpdateByPositionComponent()
        {
            if (mRole.PositionEnabled)
            {
                mRole.Direction = transform.forward;
                mRole.Position = transform.position;
                if (mRole.FindngPath)
                {
                    if (mRole.EnemyMainLockDown != default)
                    {
                        m_NavMeshAgent.destination = mRole.EnemyMainLockDown.Position;
                        mRoleInput.move = m_NavMeshAgent.velocity;
                    }
                }
                else
                {
                    if (mRoleInput != default)
                    {
                        mRoleInput.move = Vector3.zero;
                    }
                }
            }
            else
            {
                Vector3 d = new Vector3(mRoleInput.userInput.x, 0, mRoleInput.userInput.y);
                mRoleInput.SetMoveValue(d);
                m_RoleRigidbody.velocity = d * mRole.SpeedCurrent * 10;
            }
        }

        private void CheckRoleGrounding()
        {
            transform.Rotate(0, mRoleInput.ExtraTurnRotationOut, 0);

            mAnimatorInfo = mRole.RoleAnimatorInfo;
            mAnimatorStateInfo = m_RoleAnimator.GetCurrentAnimatorStateInfo(0);
            mAnimatorInfo.IsNameGrounded = mAnimatorStateInfo.IsName("Grounded");

            Vector3 velocity = m_RoleRigidbody.velocity;
            if (mRole.IsGrounded)
            {
                bool flag = mRoleInput.HandleGroundedMovement(ref mRoleInput, ref mAnimatorInfo);
                if (flag)
                {
                    // jump!
                    mRole.IsGrounded = false;
                    m_RoleRigidbody.velocity = new Vector3(velocity.x, mRoleData.JumpPower, velocity.z);
                    m_RoleAnimator.applyRootMotion = false;
                    mGroundCheckDistance = 0.3f;
                }
            }
            else
            {
                mRoleInput.HandleAirborneMovement(ref mRoleData);
                m_RoleRigidbody.AddForce(mRoleInput.ExtraGravityForceOut);
                mGroundCheckDistance = velocity.y < 0 ? m_RoleMustSubgroup.origGroundCheckDistance : 0.01f;
            }
            mRoleInput.UpdateMovePhase();
        }

        private void CheckRoleCrouching()
        {
            if (mRole.IsGroundedAndCrouch)
            {
                if (!mRoleInput.crouching)
                {
                    mRoleInput.crouching = true;
                    m_RoleCollider.height = m_RoleMustSubgroup.capsuleHeight / 2f;
                    m_RoleCollider.center = m_RoleMustSubgroup.capsuleCenter / 2f;
                }
            }
            else
            {
                if(!UpdateCrouchingByRay())
                {
                    mRoleInput.crouching = false;
                    m_RoleCollider.height = m_RoleMustSubgroup.capsuleHeight;
                    m_RoleCollider.center = m_RoleMustSubgroup.capsuleCenter;
                }
            }
            // prevent standing up in crouch-only zones
            if (!mRoleInput.crouching)
            {
                UpdateCrouchingByRay();
            }
            UpdateAnimator();
            mRoleInput.UpdateMovePhase();
        }

        private bool UpdateCrouchingByRay()
        {
            mCrouchRay = new Ray(m_RoleRigidbody.position + Vector3.up * m_RoleCollider.radius * k_Half, Vector3.up);
            float crouchRayLength = m_RoleMustSubgroup.capsuleHeight - m_RoleCollider.radius * k_Half;
            bool flag = Physics.SphereCast(mCrouchRay, m_RoleCollider.radius * k_Half, crouchRayLength, Physics.AllLayers, QueryTriggerInteraction.Ignore);
            if (flag)
            {
                mRoleInput.crouching = true;
            }
            return flag;
        }

        private void Update()
        {
            if (mRole != default)
            {
                m_Hp = mRole.RoleDataSource.Hp;
                m_Speed = mRole.SpeedCurrent;
                m_NavMeshAgent.speed = mRole.SpeedCurrent;
                mRoleInput = mRole.RoleInput;

                UpdateByPositionComponent();

                //if(mRole.IsUserControlling)
                //{
                //    transform.localScale = Vector3.one * 1.2f;
                //}
                //else
                //{
                //    transform.localScale = Vector3.one;
                //}

                if (mRoleInput != default)
                {
                    switch (mRoleInput.RoleMovePhase)
                    {
                        case UserInputComponent.ROLE_INPUT_PHASE_MOVE_READY:
                            CheckMoveAndGroundStatus();
                            break;
                        case UserInputComponent.ROLE_INPUT_PHASE_CHECK_GROUNDE:
                            CheckRoleGrounding();
                            break;
                        case UserInputComponent.ROLE_INPUT_PHASE_CHECK_CROUCH:
                            CheckRoleCrouching();
                            break;
                    }
                }
            }
        }

        private void CheckMoveAndGroundStatus()
        {
            mRoleInput.deltaTime = Time.deltaTime;

            Vector3 v = transform.InverseTransformDirection(mRoleInput.move);
            mRoleInput.SetMoveValue(v);

            CheckGroundStatus();
            mRoleInput.UpdateMovePhase();
        }

        private void CheckGroundStatus()
        {
            
#if UNITY_EDITOR
            // helper to visualise the ground check ray in the scene view
            Debug.DrawLine(transform.localPosition + (Vector3.up * 0.1f), transform.localPosition + (Vector3.up * 0.1f) + (Vector3.down * mGroundCheckDistance));
#endif
            // 0.1f is a small offset to start the ray from inside the character
            // it is also good to note that the transform position in the sample assets is at the base of the character
            
            //Debug.Log(transform.localPosition + (Vector3.up * 0.1f));
            //Debug.Log(Physics.Raycast(transform.localPosition + (Vector3.up * 0.1f), Vector3.down, out mGroundHitInfo, mGroundCheckDistance));

            bool flag = Physics.Raycast(transform.localPosition + (Vector3.up * 0.1f), Vector3.down, out mGroundHitInfo, mGroundCheckDistance);
            SetRoleGround(flag);
        }

        private void SetRoleGround(bool value)
        {
            mRole.IsGrounded = value;
            m_RoleAnimator.applyRootMotion = value;
            mRole.GroundNormal = value ? mGroundHitInfo.normal : Vector3.up;
        }

        private void UpdateAnimator()
        {
            // update the animator parameters
            m_RoleAnimator.SetFloat("Forward", mRoleInput.ForwardAmount, 0.1f, Time.deltaTime);
            m_RoleAnimator.SetFloat("Turn", mRoleInput.TurnAmount, 0.1f, Time.deltaTime);
            m_RoleAnimator.SetBool("Crouch", mRoleInput.crouching);
            m_RoleAnimator.SetBool("OnGround", mRole.IsGrounded);
            if (!mRole.IsGrounded)
            {
                m_RoleAnimator.SetFloat("Jump", m_RoleRigidbody.velocity.y);
            }

            // calculate which leg is behind, so as to leave that leg trailing in the jump animation
            // (This code is reliant on the specific run cycle offset in our animations,
            // and assumes one leg passes the other at the normalized clip times of 0.0 and 0.5)
            m_RunCycleLegOffset = mRole.RoleAnimatorInfo.RunCycleLegOffset;
            float runCycle = Mathf.Repeat(m_RoleAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime + m_RunCycleLegOffset, 1);
            float jumpLeg = (runCycle < k_Half ? 1 : -1) * mRoleInput.ForwardAmount;
            if (mRole.IsGrounded)
            {
                m_RoleAnimator.SetFloat("JumpLeg", jumpLeg);
            }

            // the anim speed multiplier allows the overall speed of walking/running to be tweaked in the inspector,
            // which affects the movement speed because of the root motion.
            if (mRole.IsGrounded && mRoleInput.move.magnitude > 0)
            {
                m_AnimSpeedMultiplier = mRole.RoleAnimatorInfo.AnimSpeedMultiplier;
                m_RoleAnimator.speed = m_AnimSpeedMultiplier;
            }
            else
            {
                // don't use that while airborne
                m_RoleAnimator.speed = 1;
            }
        }

        public void OnAnimatorMove()
        {
            // we implement this function to override the default root motion.
            // this allows us to modify the positional speed before it's applied.
            if (mRole != default && mRole.IsGrounded && Time.deltaTime > 0)
            {
                m_MoveSpeedMultiplier = mRole.RoleAnimatorInfo.MoveSpeedMultiplier;
                Vector3 v = m_RoleAnimator.deltaPosition * m_MoveSpeedMultiplier / Time.deltaTime;

                // we preserve the existing y part of the current velocity.
                v.y = m_RoleRigidbody.velocity.y;
                m_RoleRigidbody.velocity = v;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            int id = other.GetInstanceID();
            if (mRole != default && !mRole.CollidingRoles.Contains(id))
            {
                mRole.CollidingRoles.Add(id);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            int id = other.GetInstanceID();
            if (mRole != default && mRole.CollidingRoles.Contains(id))
            {
                mRole.CollidingRoles.Remove(id);
            }
        }

        public Transform CameraNode
        {
            get
            {
                return m_CameraNode;
            }
        }
    }
}

