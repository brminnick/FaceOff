using System.Windows.Input;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

using Xamarin.Forms;

namespace FaceOff
{
	public class WelcomeViewModel : BaseViewModel
	{
		#region Fields
		string _player1, _player2;
		ICommand _startGame;
		bool _isGameReady;
		#endregion

		#region Events
		public event PropertyChangedEventHandler PropertyChanged;
		#endregion

		#region Properties
		public string Player1
		{
			get { return _player1; }
			set
			{
				_player1 = value;
				SetProperty(ref _player1, value);
			}
		}

		public string Player2
		{
			get { return _player2; }
			set
			{
				_player2 = value;
				SetProperty(ref _player2, value);
			}
		}
		#endregion
	}
}
