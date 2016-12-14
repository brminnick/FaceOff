using System.Windows.Input;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

using Xamarin.Forms;

namespace FaceOff
{
	public class WelcomeViewModel : INotifyPropertyChanged
	{
		#region Fields
		string _player1, player2;
		ICommand _startGame;
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
				OnPropertyChanged();
				OnPropertyChanged(nameof(IsGameReady));
			}
		}

		public string Player2
		{
			get { return player2; }
			set
			{
				player2 = value;
				OnPropertyChanged();
				OnPropertyChanged(nameof(IsGameReady));
			}
		}

		public bool IsGameReady
		{
			get
			{
				return !string.IsNullOrWhiteSpace(_player1) && !string.IsNullOrWhiteSpace(player2);
			}
		}

		public ICommand StartGame
		{
			get
			{
				return _startGame ?? (_startGame = new Command(async () => await ExecuteStartGame()));
			}
		}
		#endregion

		#region Methods
		async Task ExecuteStartGame()
		{
			if (string.IsNullOrWhiteSpace(_player1))
			{
				await DisplayEmptyPlayerNameAlert("Player 1");
			}
			else if (string.IsNullOrWhiteSpace(player2))
			{
				await DisplayEmptyPlayerNameAlert("Player 2");
			}
			else
			{
				await Application.Current.MainPage.Navigation.PushAsync(new PicturePage(_player1, player2));
			}
		}

		void OnPropertyChanged([CallerMemberName]string name = "")
		{
			var handle = PropertyChanged;
			handle?.Invoke(this, new PropertyChangedEventArgs(name));
		}

		async Task DisplayEmptyPlayerNameAlert(string playerName)
		{
			await Application.Current.MainPage.DisplayAlert("Error", $"{playerName} is blank", "OK");
		}
		#endregion
	}
}
