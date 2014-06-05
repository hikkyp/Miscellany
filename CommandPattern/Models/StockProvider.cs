using System;
using System.Collections.Generic;
using System.Linq;

namespace CommandPattern.Models
{
	public static class StockProvider
	{
		#region 非表示メンバ

		private static readonly List<StockModel> _Stocks;
		private static readonly Random _Random = new Random ();
		private static readonly double _RangePercent = 0.01;

		static StockProvider ()
		{
			_Stocks = new List<StockModel> {
				new StockModel { Symbol = "ZNGA", Price = 2.97m, },
				new StockModel { Symbol = "RAD", Price = 7.87m, },
				new StockModel { Symbol = "BAC", Price = 15.43m, },
				new StockModel { Symbol = "FB", Price = 63.19m, },
				new StockModel { Symbol = "TWTR", Price = 33.89m, },
			};
		}
		private static void _UpdateStocks ()
		{
			foreach (var stock in _Stocks) {
				if (_Random.NextDouble () <= 0.1) {
					continue;
				}
				var percentChange = _Random.NextDouble () * _RangePercent;
				var pos = _Random.NextDouble () > 0.5;
				var change = Math.Round (stock.Price * (decimal)percentChange, 2);
				change = pos ? change : -change;
				stock.Price += change;
			}
		}

		#endregion

		public static List<StockModel> QueryStocks ()
		{
			_UpdateStocks ();
			return _Stocks.ToList ();
		}
	}
}
