using ShipDock.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShipDock.Applications
{
    public class HostGameInputer : InputerComponent
    {
        [SerializeField]
        private int m_UserInputComponentName;
        [SerializeField]
        private HostGameInputerButtons m_InputerButtons;

        private string mDirectionKey;
        private string[] mDirectionButtons;
        private IHostGameInputComponent mHostGameInputComp;

        protected override void Awake()
        {
            base.Awake();

            m_InputerButtons.Init();
        }

        public override void CommitAfterSetToServer()
        {
            base.CommitAfterSetToServer();

            SetDirectionsKeys();

            mHostGameInputComp = mRelater.ComponentRef<IHostGameInputComponent>(m_UserInputComponentName);
            mHostGameInputComp.SetUserInputerButtons(m_InputerButtons);
        }

        private void SetDirectionsKeys()
        {
            mDirectionButtons = m_InputerButtons.directionButtons;
        }

        private void Update()
        {
            CheckDirectionsButtons();
        }

        private void CheckDirectionsButtons()
        {
            int max = mDirectionButtons.Length;
            for (int i = 0; i < max; i++)
            {
                mDirectionKey = mDirectionButtons[i];
                bool isDirectionKeyGet = Input.GetButton(mDirectionKey);
                bool isButtonActived = m_InputerButtons.GetButton(mDirectionKey);
                if (isDirectionKeyGet && !isButtonActived)
                {
                    m_InputerButtons.SetActiveButton(mDirectionKey, true);
                }
                else
                {
                    m_InputerButtons.SetActiveButton(mDirectionKey, false);
                }
            }
        }
    }
}
