/*
	FILE: 			MobileScreenTrigger.cs
    PROJECT:		PROG 2120 - A06 - Windows UWP Application

    PROGRAMMER:		Brendan Rushing
	FIRST VERSION:	2018-Dec-06
	DESCRIPTION	:

    This application is based on a tutorial to make a Windwos UWP Application
*/
using Windows.System.Profile;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;

namespace PhotoLab
{
    /// <summary>
    /// Custom trigger for Mobile device UI states.
    /// This trigger is active when the app runs on a mobile device and the
    /// UserInteractionMode is touch, which indicates that the app is showing
    /// on the device screen. When UserInteractionMode is mouse, the app is
    /// using Continuum to show on a larger screen.
    /// https://blogs.windows.com/buildingapps/2015/12/07/optimizing-apps-for-continuum-for-phone/#Yubo3bUdIM4H6Vle.97
    /// </summary>
    public class MobileScreenTrigger : StateTriggerBase
    {
        public MobileScreenTrigger()
        {
            Window.Current.SizeChanged += (s, e) => UpdateTrigger();
        }

        /// <summary>
        /// The target device family.
        /// </summary>
        public UserInteractionMode InteractionMode
        {
            get => _interactionMode;
            set
            {
                _interactionMode = value;
                UpdateTrigger();
            }
        }
        private UserInteractionMode _interactionMode;

        private void UpdateTrigger()
        {
            // Get the current device family and interaction mode.
            var currentDeviceFamily = AnalyticsInfo.VersionInfo.DeviceFamily;
            var currentInteractionMode = UIViewSettings.GetForCurrentView().UserInteractionMode;

            // The trigger will be activated if the current device family is Windows.Mobile
            // and the UserInteractionMode matches the interaction mode value in XAML.
            SetActive(InteractionMode == currentInteractionMode && currentDeviceFamily == "Windows.Mobile");
        }
    }
}
