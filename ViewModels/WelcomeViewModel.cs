using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace FaceOff
{
	public class WelcomeViewModel : INotifyPropertyChanged
	{

		string player1;
		public string Player1
		{
			get { return player1; }
			set
			{
				player1 = value;
				OnPropertyChanged();
				OnPropertyChanged(nameof(GameIsReady));
			}
		}

		string player2;
		public string Player2
		{
			get { return player2; }
			set
			{
				player2 = value;
				OnPropertyChanged();
				OnPropertyChanged(nameof(GameIsReady));
			}
		}

		public bool GameIsReady
		{
			get 
			{
				return !string.IsNullOrWhiteSpace(player1) && !string.IsNullOrWhiteSpace(player2);
			}
		}

		ICommand startGame;
		public ICommand StartGame
		{
			get
			{
				return startGame ?? (startGame = new Command(async () => await ExecuteStartGame()));
			}
		}

		async Task ExecuteStartGame()
		{
			if (string.IsNullOrWhiteSpace(player1) ||
			   string.IsNullOrWhiteSpace(player2))
			{
				return;
			}

			await Application.Current.MainPage.Navigation.PushAsync(new PicturePage(player1, player2));
		}

		public event PropertyChangedEventHandler PropertyChanged;
		void OnPropertyChanged([CallerMemberName]string name = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
		}
	}
}
