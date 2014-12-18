using System;

namespace hellou_rss
{
  public class rss
  {
    public string strHeadline
    { get; set; }

    public string strAuthor
    { get; set; }

    public DateTime dtPublished
    { get; set; }

    public string strDetails
    { get; set; }

    public byte[] btImage
    { get; set; }
  }

  public class quickreader
  {
    public string strHeadline
    { get; set; }

    public string strMessage
    { get; set; }
  }
}

