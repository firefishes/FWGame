using ShipDock.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace FWGame
{
    public class UIRoleCard : UI, IPointerClickHandler
    {
        [SerializeField]
        private Image m_RoleCardImage;

        private CampRoleModel mRoleCardData;

        protected override void Purge()
        {
            base.Purge();

            RoleCardSelectedEvent?.RemoveAllListeners();

            mRoleCardData = default;
            m_RoleCardImage = default;
        }

        public override void UpdateUI()
        {
            base.UpdateUI();

            RoleCardImage();
        }

        private void RoleCardImage()
        {
            int id = RoleCardData.GetRoleConfigID();
            Sprite raw = ABs.Get<Sprite>(FWConsts.ASSET_UI_MAIN, "ui_role_card_".Append(id.ToString()));
            m_RoleCardImage.sprite = raw;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            RoleCardSelectedEvent?.Invoke(RoleCardData);
        }

        public CampRoleModel RoleCardData
        {
            set
            {
                mRoleCardData = value;
                UIChanged = true;
            }
            get
            {
                return mRoleCardData;
            }
        }

        public OnRoleCardSelected RoleCardSelectedEvent { get; } = new OnRoleCardSelected();
    }

}