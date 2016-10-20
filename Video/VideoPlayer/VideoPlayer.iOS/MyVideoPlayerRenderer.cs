using System;
using Xamarin.Forms;
using VideoSamples.Controls;
using VideoSamples.Library;
using Xamarin.Forms.Platform.iOS;
using UIKit;
using AVFoundation;
using Foundation;

[assembly: ExportRenderer(typeof(MyVideoPlayer), typeof(VideoSamples.iOS.Controls.MyVideoPlayerRenderer))]
namespace VideoSamples.iOS.Controls
{
	public class MyVideoPlayerRenderer : ViewRenderer<MyVideoPlayer, UIView>
	{
		private MyPlayerView _playerView;
		private DateTime _touchStart = DateTime.MinValue;
		private bool _didDouble;

	    protected override void Dispose (bool disposing)
		{
			if (_playerView != null) {
				_playerView.Dispose ();
				_playerView = null;
			}
			base.Dispose (disposing);
		}

		protected override void OnElementChanged (ElementChangedEventArgs<MyVideoPlayer> e)
		{
			base.OnElementChanged (e);
			if (Control == null) {						
				_playerView = new MyPlayerView (Element);
				SetNativeControl (_playerView);
			}
				
			_playerView.Load(new NSString(Element.FileSource));
			_playerView.FitToWindow = Element.FitInWindow;
			_playerView.AddController = Element.AddVideoController;

			// autoplay
			if (Element.AutoPlay) {
				// _playerView.Start ();
			}
		}

		public override void TouchesBegan (NSSet touches, UIEvent evt)
		{
			if (DateTime.Now.Subtract(_touchStart).Milliseconds <= 500 && _didDouble == false) {	
				_didDouble = true;
				_touchStart = DateTime.MinValue;
				// double tap
				Element.FireTap(true);
			} else {
				_didDouble = false;
				_touchStart = DateTime.Now;
				Element.FireTap(false);
			}
			base.TouchesBegan (touches, evt);
		}

		protected override void OnElementPropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged (sender, e);
			var source = Element;
			if (source != null && _playerView != null) {
				if (e.PropertyName == MyVideoPlayer.SeekProperty.PropertyName) {
					_playerView.SeekTo ((int)Element.Seek);
				} else if (e.PropertyName == MyVideoPlayer.FileSourceProperty.PropertyName) {
					_playerView.Load (new NSString(Element.FileSource));
				} else if (e.PropertyName == MyVideoPlayer.PlayerActionProperty.PropertyName) {
					if (source.PlayerAction == VideoState.PAUSE) {
						// _playerView.Pause ();
					} else if (source.PlayerAction == VideoState.PLAY) {
						// _playerView.Start ();
					} else if (source.PlayerAction == VideoState.RESTART) {
						_playerView.SeekTo (0);
					} else if (source.PlayerAction == VideoState.STOP) {
						// _playerView.Stop ();
					}
				} else if (e.PropertyName == MyVideoPlayer.FullScreenProperty.PropertyName) {
					_playerView.FullScreen = source.FullScreen;
				} else if (e.PropertyName == MyVideoPlayer.FitInWindowProperty.PropertyName) {
					_playerView.FitToWindow = source.FitInWindow;
				} else if (e.PropertyName == MyVideoPlayer.AddVideoControllerProperty.PropertyName) {
					_playerView.AddController = source.AddVideoController;
				} else if (e.PropertyName == MyVideoPlayer.ActionBarHideProperty.PropertyName)
				{
					UIApplication.SharedApplication.StatusBarHidden = source.ActionBarHide;	
				} else if (e.PropertyName == MyVideoPlayer.ContentHeightProperty.PropertyName || e.PropertyName ==
				           MyVideoPlayer.ContentWidthProperty.PropertyName) {
					_playerView.UpdateFrame (source);
				}
			}
		}
	}
}

