using Xamarin.Forms;
using VideoSamples.Controls;

namespace VideoSamples.Views
{
	public class VideoPlayerView : ContentView
	{
		/// <summary>
		/// attached event handlers to this object
		/// </summary>
		private readonly MyVideoPlayer _videoPlayer;

		public MyVideoPlayer VideoPlayer => _videoPlayer;

	    public VideoPlayerView ()
		{
            _videoPlayer = new MyVideoPlayer();
			HorizontalOptions = LayoutOptions.FillAndExpand;
			VerticalOptions = LayoutOptions.FillAndExpand;

			Content = _videoPlayer;
		}
	}
}

