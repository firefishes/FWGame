namespace ShipDock.Applications
{
    public class HostGameInputComponent<S> : UserInputComponent<S>, IHostGameInputComponent where S : MainServer
    {
        protected override float GetHorizontal()
        {
            if(IsJoypad())
            {
                return UserInputButtons.GetAxis(InputerButtonsKeys.DIRECTION_AXIS_H_KEY);
            }
            else
            {
                if(UserInputButtons.GetButton(InputerButtonsKeys.DIRECTION_KEYS[0]))
                {
                    return -1f;
                }
                else if(UserInputButtons.GetButton(InputerButtonsKeys.DIRECTION_KEYS[1]))
                {
                    return 1f;
                }
                else
                {
                    return 0f;
                }
            }
        }

        public void SetUserInputerButtons(IUserInputerButtons target)
        {
            UserInputButtons = target;
        }
        
        private bool IsJoypad()
        {
            return false;
        }

        public IUserInputerButtons UserInputButtons { get; private set; }

    }
}