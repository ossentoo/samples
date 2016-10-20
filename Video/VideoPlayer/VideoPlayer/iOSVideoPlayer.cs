using System;

using Xamarin.Forms;
using VideoSamples.Views;
using VideoSamples.Controls;

namespace VideoSamples
{
	public class iOSVideoPlayer : ContentPage
	{
		private readonly VideoPlayerView _player;

		public iOSVideoPlayer ()
		{
			ToolbarItems.Add (new ToolbarItem {
				Text = "Actions",
				Command = new Command (async () => {
						var result = await DisplayActionSheet("Select", "Cancel", null, 
							"Full UIScreen", 
							"Full Screen",
							"Fit Screen",
							"Restart",
							"Play",
							"Stop",
							"Resize",
							"Pause",
							"StatusBar Hide");

					if (result == "Full UIScreen")
					{
						// this uiscreen.MainScreen.Bounds
						_player.VideoPlayer.ContentHeight = -1;
						_player.VideoPlayer.ContentWidth = -1;
					}
					else if (result == "Full Screen")
					{
						// this goes to full device screen
						_player.VideoPlayer.FullScreen = !_player.VideoPlayer.FullScreen;
					}
					else if (result == "Fit Screen")
					{
						_player.VideoPlayer.FitInWindow = !_player.VideoPlayer.FitInWindow;
					}
					else if (result == "Restart")
					{
						_player.VideoPlayer.Seek = 0;
					}
					else if (result == "Play")
					{
						_player.VideoPlayer.PlayerAction = Library.VideoState.PLAY;
					}
					else if (result == "Stop")
					{
						_player.VideoPlayer.PlayerAction = Library.VideoState.STOP;
					}
					else if (result == "Resize")
					{
						if (test == 0)
						{
							_player.HeightRequest = 400;
							_player.VideoPlayer.ContentHeight = 400;
						}
						else if (test == 1)
						{
							_player.HeightRequest = 200;
							_player.VideoPlayer.ContentHeight = 100;
						}
						else if (test == 2)
						{
							_player.HeightRequest = 100;
							_player.VideoPlayer.ContentHeight = 100;
							test = -1;
						}
						test++;
					}
					else if (result == "Pause")
					{
						_player.VideoPlayer.PlayerAction = Library.VideoState.PAUSE;
					}
					else if (result == "StatusBar Hide")
					{
						_player.VideoPlayer.ActionBarHide = !_player.VideoPlayer.ActionBarHide;
					}
				})
			});


		    _player = new VideoPlayerView {HeightRequest = 200};

		    _player.VideoPlayer.AddVideoController = true;
            _player.VideoPlayer.FileSource = "http://clips.vorwaerts-gmbh.de/VfE_html5.mp4";
            // _player.VideoPlayer.FileSource = "http://www.jplayer.org/video/m4v/Big_Buck_Bunny_Trailer.m4v";
            // _player.VideoPlayer.FileSource = "sample.m4v";
                // "http://spacelinxtrainingdev.streaming.mediaservices.windows.net/19ad507a-573d-4888-8e0b-38c67e9638b2/Oscar%20Ssentoogo-Octopus%20Deploy_320x180_400.mp4";
                    // "http://spacelinxtrainingdev.streaming.mediaservices.windows.net/19ad507a-573d-4888-8e0b-38c67e9638b2/Oscar%20Ssentoogo-Octopus%20Deploy.ism/manifest(format=m3u8-aapl)";// "sample.m4v"; //"http://192.168.202.78/sample.m4v";

			// autoplay video
			_player.VideoPlayer.AutoPlay = true;

			Content = new StackLayout
			{
				VerticalOptions = LayoutOptions.StartAndExpand,
				Spacing = 0,
				Padding = new Thickness(0,0),
				Children =  
				{
					_player
				}
			};
		}

		private Int32 test = 0;

		protected override void OnAppearing ()
		{
			base.OnAppearing ();
			_player.VideoPlayer.OnTap += (MyVideoPlayer player, bool IsDoubleTap) => {
				System.Diagnostics.Debug.WriteLine("tapped fired, is double: " + IsDoubleTap);
			};

			this._player.VideoPlayer.PropertyChanged += (object sender, System.ComponentModel.PropertyChangedEventArgs e) => {
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


