using System;
using Xamarin.Forms;
using VideoSamples.Views;
using VideoSamples.Controls;

namespace VideoSamples
{
	public class AndroidVideoPlayer : ContentPage
	{
		private readonly VideoPlayerView _player;

		public AndroidVideoPlayer ()
		{
			_player = new VideoPlayerView ();

			ToolbarItems.Add (new ToolbarItem {
				Order = ToolbarItemOrder.Secondary,
				Text = "Controller",
				Command = new Command( () => {
					_player.VideoPlayer.AddVideoController = !_player.VideoPlayer.AddVideoController;
				})
			});

			ToolbarItems.Add (new ToolbarItem {
				Order = ToolbarItemOrder.Secondary,
				Text = "Full Screen",
				Command = new Command( () => {

					// resize the Content for full screen mode
					_player.VideoPlayer.FullScreen = !_player.VideoPlayer.FullScreen;
					if (_player.VideoPlayer.FullScreen)
					{
						_player.HeightRequest = -1;
						Content.VerticalOptions = LayoutOptions.FillAndExpand;
						_player.VideoPlayer.FullScreen = true;
					}
					else
					{
						_player.HeightRequest = 200;
						Content.VerticalOptions = LayoutOptions.StartAndExpand;
						_player.VideoPlayer.FullScreen = false;
					}
				})
			});

			ToolbarItems.Add (new ToolbarItem {
				Order = ToolbarItemOrder.Secondary,
				Text = "Play",
				Command = new Command( () => {
					_player.VideoPlayer.PlayerAction = Library.VideoState.PLAY;
				})
			});

			ToolbarItems.Add (new ToolbarItem {
				Order = ToolbarItemOrder.Secondary,
				Text = "Stop",
				Command = new Command( () => {
					_player.VideoPlayer.PlayerAction = Library.VideoState.STOP;
				})
			});

			ToolbarItems.Add (new ToolbarItem {
				Order = ToolbarItemOrder.Secondary,
				Text = "Pause",
				Command = new Command( () => {
					_player.VideoPlayer.PlayerAction = Library.VideoState.PAUSE;
				})
			});

			ToolbarItems.Add (new ToolbarItem {
				Order = ToolbarItemOrder.Secondary,
				Text = "Restart",
				Command = new Command( () => {
					_player.VideoPlayer.PlayerAction = Library.VideoState.RESTART;
				})
			});

			// heightRequest must be set it not full screen
			_player.HeightRequest = 200;
			_player.VideoPlayer.AddVideoController = true;


			// location in Raw folder. no extension
			_player.VideoPlayer.FileSource = "sample";

			// autoplay video
			_player.VideoPlayer.AutoPlay = true;

			Content = new StackLayout
			{				
				VerticalOptions = LayoutOptions.StartAndExpand,
				Children =  
				{
					_player
				}
			};
		}

		protected override void OnAppearing ()
		{
			base.OnAppearing ();
			_player.VideoPlayer.PropertyChanged += (sender, e) => {
				if (e.PropertyName == MyVideoPlayer.StateProperty.PropertyName)
				{
					var s = _player.VideoPlayer.State;
					if (s == Library.VideoState.ENDED)
					{
						System.Diagnostics.Debug.WriteLine("State: ENDED");
					}
					else if (s == Library.VideoState.PAUSE)
					{
						System.Diagnostics.Debug.WriteLine("State: PAUSE");
					}
					else if (s == Library.VideoState.PLAY)
					{
						System.Diagnostics.Debug.WriteLine("State: PLAY");
					}
					else if (s == Library.VideoState.STOP)
					{
						System.Diagnostics.Debug.WriteLine("State: STOP");
					}
				}
				else if (e.PropertyName == MyVideoPlayer.InfoProperty.PropertyName)
				{
					System.Diagnostics.Debug.WriteLine("Info:\r\n" + _player.VideoPlayer.Info);
				}
			};
		}

		protected override void OnSizeAllocated (double width, double height)
		{
			_player.VideoPlayer.ContentHeight = height;
			_player.VideoPlayer.ContentWidth = width;
			if (width < height) {
				
				_player.VideoPlayer.Orientation = MyVideoPlayer.ScreenOrientation.PORTRAIT;
			} else {
				_player.VideoPlayer.Orientation = MyVideoPlayer.ScreenOrientation.LANDSCAPE;
			}
			_player.VideoPlayer.OrientationChanged ();
			base.OnSizeAllocated (width, height);
		}
	}
}


