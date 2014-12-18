using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace hellou_rss
{
  [Activity (Label = "FeedContent")]            
  public class FeedContent : Activity
  {
    protected override void OnCreate(Bundle bundle)
    {
      base.OnCreate(bundle);
      SetContentView(Resource.Layout.newsfeed);
      TextView txtHeadline = FindViewById<TextView>(Resource.Id.textHeadline);
      TextView txtContent = FindViewById<TextView>(Resource.Id.textContent);
      Button btnReturn = FindViewById<Button>(Resource.Id.btnBack);
      btnReturn.Click += delegate {
        Finish();
      };

      txtHeadline.Text = base.Intent.GetStringExtra("headline");
      txtContent.Text = base.Intent.GetStringExtra("message");
    }
  }
}