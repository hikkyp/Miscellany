using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Data;

namespace CommandPattern.ViewModels
{
	using Common;
	using Models;

	public class StockViewModel : BindableBase
	{
		#region 非表示メンバ

		private DelegateCommand _RefreshCommand;
		private DelegateCommand<string> _SortStocksCommand;
		private string _SortStocksColumn;
		private ListSortDirection _SortStocksDirection;
		private bool _CanRefreshFlag = true;
		private string _RefreshButtonContent;
		private ObservableCollection<StockModel> _Stocks = new ObservableCollection<StockModel> ();
		private CollectionViewSource _StocksViewSource;

		private bool _CanRefresh ()
		{
			Debug.WriteLine ("RefreshCommand.CanExecute");

			return _CanRefreshFlag;
		}
		private Task _Refresh ()
		{
			Debug.WriteLine ("RefreshCommand.Execute");

			var syncContext = TaskScheduler.FromCurrentSynchronizationContext ();

			_UpdateRefreshButtonContent (3);
			_UpdateStocks ();
			_CanRefreshFlag = false;
			_RefreshCommand.RaiseCanExecuteChanged ();
			return Task
				.Delay (1000)
				.ContinueWith (a => {
					_UpdateRefreshButtonContent (2);
					return Task
						.Delay (1000)
						.ContinueWith (b => {
							_UpdateRefreshButtonContent (1);
							return Task
								.Delay (1000)
								.ContinueWith (c => {
									_UpdateRefreshButtonContent (0);
									_CanRefreshFlag = true;
									_RefreshCommand.RaiseCanExecuteChanged ();
								}, syncContext);
						}, syncContext);
				}, syncContext);
		}
		private void _SortStocks (string parameter)
		{
			if (_SortStocksColumn == parameter) {
				_SortStocksDirection = _SortStocksDirection == ListSortDirection.Ascending
					? ListSortDirection.Descending
					: ListSortDirection.Ascending;
			} else {
				_SortStocksColumn = parameter;
				_SortStocksDirection = ListSortDirection.Ascending;
			}

			_StocksViewSource.SortDescriptions.Clear ();
			_StocksViewSource.SortDescriptions.Add (new SortDescription (_SortStocksColumn, _SortStocksDirection));
		}
		private void _UpdateStocks ()
		{
			_Stocks.Clear ();
			StockProvider
				.QueryStocks ()
				.ForEach (stock => { _Stocks.Add (stock); });
		}
		private void _UpdateRefreshButtonContent (int count)
		{
			if (count > 0) {
				RefreshButtonContent = string.Format ("Refresh ({0})", count);
			} else {
				RefreshButtonContent = string.Format ("Refresh");
			}
		}

		#endregion

		public DelegateCommand RefreshCommand
		{
			get
			{
				if (_RefreshCommand == null) {
					_RefreshCommand = DelegateCommand.FromAsyncHandler (_Refresh, _CanRefresh);
				}
				return _RefreshCommand;
			}
		}
		public DelegateCommand<string> SortStocksCommand
		{
			get
			{
				if (_SortStocksCommand == null) {
					_SortStocksCommand = new DelegateCommand<string> (_SortStocks);
				}
				return _SortStocksCommand;
			}
		}
		public string RefreshButtonContent
		{
			get
			{
				return _RefreshButtonContent;
			}
			set
			{
				SetProperty (ref _RefreshButtonContent, value);
			}
		}
		public ListCollectionView Stocks
		{
			get
			{
				if (_StocksViewSource == null) {
					_StocksViewSource = new CollectionViewSource ();
					_StocksViewSource.Source = _Stocks;
				}
				return (ListCollectionView)_StocksViewSource.View;
			}
		}

		public StockViewModel ()
		{
			_UpdateStocks ();
			_UpdateRefreshButtonContent (0);
		}
	}
}
