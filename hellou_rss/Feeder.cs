using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Net;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace hellou_rss
{
  [Activity (MainLauncher = true, Label = "Feeder")]            
  public class Feeder : Activity
  {
    private LinearLayout listWrapper;
    private Context context;
    private static string RSS_FEED = "http://feeds.bbci.co.uk/news/rss.xml";
    private List<quickreader> myList = new List<quickreader>();
    private const int CHOICE = 10;
    // makes no difference!
    protected override void OnCreate(Bundle bundle)
    {
      base.OnCreate(bundle);
      SetContentView(Resource.Layout.MainLayout);
      TextView txtRssTitle = FindViewById<TextView>(Resource.Id.textFeedname);
      TextView txtRssAuthor = FindViewById<TextView>(Resource.Id.textAuthor);
      ImageView imgFeedImage = FindViewById<ImageView>(Resource.Id.imageRSSImage);
      listWrapper = FindViewById<LinearLayout>(Resource.Id.listWrapper);
      EditText editSearch = FindViewById<EditText>(Resource.Id.editSearch);
      Button btnSearch = FindViewById<Button>(Resource.Id.btnSearch);
      btnSearch.Click += delegate {
        SearchHeadLines(editSearch.Text);
      };
      editSearch.TextChanged += delegate {
        if (editSearch.Text.Length == 0)
          myList.Clear();
        getRSS();
      };
      context = listWrapper.Context;
      txtRssTitle.Text = "BBC Newsfeed";
      txtRssAuthor.Text = "BBC";
      getRSS();
    }

    private void getRSS()
    {
      XDocument initial = XDocument.Load(RSS_FEED);
      var header = initial.Descendants("channel");
      string title = header.Elements("title").ToString();
      string copyright = initial.Descendants("channel").Elements("copyright").ToString();
      var query = from item in initial.Descendants("item")
        select new quickreader {
        strHeadline = (string) item.Element("title"),
        strMessage = (string) item.Element("description")
      };

      myList = query.ToList();
      generateUI();
    }

    private void generateUI()
    {
      listWrapper.RemoveAllViews();

      LinearLayout linlayMain = new LinearLayout(context);
      linlayMain.Orientation = Orientation.Vertical;
      linlayMain.LayoutParameters = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.WrapContent);
      linlayMain.SetPadding(5, 5, 5, 5);
      int counter = 0;
      foreach (quickreader qr in myList)
        {
          LinearLayout linlayInner = new LinearLayout(context);
          linlayInner.Orientation = Orientation.Vertical;
          linlayInner.LayoutParameters = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.WrapContent);
          TextView txtTitle = new TextView(context);
          txtTitle.Text = qr.strHeadline;
          TextView txtMessage = new TextView(context);
          txtMessage.Text = qr.strMessage;
          int choice = new int();
          choice = counter;
          linlayInner.Tag = choice;
          linlayInner.Click += ShowMessage;
          linlayInner.AddView(txtTitle);
          linlayInner.AddView(txtMessage);
          linlayMain.AddView(linlayInner);
          counter++;
        }
      listWrapper.AddView(linlayMain);
    }

    private void SearchHeadLines(string search)
    {
      string mySearch = search.ToLower();
      List<quickreader> tmpReader = new List<quickreader>();
      tmpReader = myList.Where(t => t.strHeadline.ToLower().Contains(mySearch)).ToList();

      myList = tmpReader;
      generateUI();
    }

    private void ShowMessage(object s, EventArgs e)
    {
      LinearLayout linlayTemp = (LinearLayout)s;
      int choice = (int)linlayTemp.Tag;
      Intent newView = new Intent(this, typeof(FeedContent));
      newView.PutExtra("headline", myList[choice].strHeadline);
      newView.PutExtra("message", myList[choice].strMessage);
      StartActivityForResult(newView, CHOICE);
    }

    protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
    {
      base.OnActivityResult(requestCode, resultCode, data);
      switch (requestCode)
        {
        case CHOICE:
          getRSS();
          break;
        }
    }
  }
}

