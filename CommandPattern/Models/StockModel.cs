using System;

namespace CommandPattern.Models
{
	public sealed class StockModel
	{
		#region 非表示メンバ

		private decimal _Price;

		#endregion

		public string Symbol { get; set; }
		public decimal Price
		{
			get
			{
				return _Price;
			}
			set
			{
				if (_Price != value) {
					_Price = value;
					if (DayOpen == 0) {
						DayOpen = value;
					}
				}
			}
		}
		public decimal DayOpen { get; private set; }
		public decimal Change { get { return Price - DayOpen; } }
		public double PercentChange { get { return (double)Math.Round (Change / Price, 4) * 100.0; } }
	}
}
