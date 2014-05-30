using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace TileNotification
{
	public static class Tile
	{
		public static void UpdateBadgeCounter (int count) {
			var template = "<badge value=\"" + count + "\" />";
			var templateXml = new XmlDocument ();
			templateXml.LoadXml (template);
			var badge = new BadgeNotification (templateXml);
			BadgeUpdateManager.CreateBadgeUpdaterForApplication ().Update (badge);
		}
	}
}
