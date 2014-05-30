using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace ToastNotification
{
	public static class Toast
	{
		private static Windows.UI.Notifications.ToastNotification _CreateToastNotification (string message) {
			var template =
				"<toast duration=\"long\">" +
				"  <visual>" +
				"    <binding template=\"ToastImageAndText02\">" +
				"      <image id=\"1\" src=\"ms-appx:///assets/toast.png\" alt=\"Toast\" />" +
				"      <text id=\"1\">Toast Notification</text>" +
				"      <text id=\"2\">{0}</text>" +
				"    </binding>" +
				"  </visual>" +
				"  <audio src=\"ms-winsoundevent:Notification.Looping.Call\" loop=\"true\" />" +
				"</toast>";
			var templateXml = new XmlDocument ();
			templateXml.LoadXml (string.Format (template, message));
			return new Windows.UI.Notifications.ToastNotification (templateXml);
		}
		public static void Show(string message)
		{
			var toast = _CreateToastNotification (message);
			ToastNotificationManager.CreateToastNotifier ().Show (toast);
		}
		public static void Show (string message, Action activated, Action dismissed, Action failed)
		{
			var toast = _CreateToastNotification (message);
			if (activated != null) toast.Activated += (s, a) => activated ();
			if (dismissed != null) toast.Dismissed += (s, a) => dismissed ();
			if (failed != null) toast.Failed += (s, a) => failed ();
			ToastNotificationManager.CreateToastNotifier ().Show (toast);
		}
	}
}
