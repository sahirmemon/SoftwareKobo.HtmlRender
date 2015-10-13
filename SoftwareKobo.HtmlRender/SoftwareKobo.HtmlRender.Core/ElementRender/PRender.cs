using AngleSharp.DOM;
using SoftwareKobo.HtmlRender.Core.Extension;
using SoftwareKobo.HtmlRender.Core.Interface;
using SoftwareKobo.HtmlRender.Core.TextContainer;
using System.Globalization;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media;

namespace SoftwareKobo.HtmlRender.Core.ElementRender
{
    public class PRender : IElementRender
    {
        public string TagName
        {
            get
            {
                return "p";
            }
        }

        //public virtual void RenderElement(IElement element, ITextContainer parent, RenderContextBase context)
        //{
        //    var paragraph = new Paragraph
        //    {
        //        Margin = new Thickness(0, 10, 0, 10)
        //    };

        //    parent.Add(paragraph);
        //    context.RenderNode(element, new ParagraphContainer(paragraph));
        //}

        public virtual void RenderElement(IElement element, ITextContainer parent, RenderContextBase context)
        {
            var parentContainer = (parent as RichTextBlockContainer).Get();

            Span span = new Span();
            Underline underline = null;

            var textDecoration = element.Style("text-decoration");
            if (!string.IsNullOrEmpty(textDecoration))
            {
                if (textDecoration == "underline")
                {
                    underline = new Underline();
                    underline.Inlines.Add(span);
                }
            }

            var color = element.Style("color");
            if (string.IsNullOrEmpty(color) == false)
            {
                color = color.Trim().TrimStart('#');
                if (color.Length == 6)
                {
                    var sR = color.Substring(0, 2);
                    var sG = color.Substring(2, 2);
                    var sB = color.Substring(4, 2);
                    byte r, g, b;
                    if (byte.TryParse(sR, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out r)
                        && byte.TryParse(sG, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out g)
                        && byte.TryParse(sB, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out b))
                    {
                        var value = Color.FromArgb(255, r, g, b);
                        if (value != Colors.White && value != Colors.Black)
                        {
                            if (underline != null)
                            {
                                underline.Foreground = new SolidColorBrush(value);
                            }
                            else
                            {
                                span.Foreground = new SolidColorBrush(value);
                            }
                        }
                    }
                }
            }

            var fontSize = element.Style("font-size");
            if (!string.IsNullOrEmpty(fontSize))
            {
                fontSize = fontSize.Replace("px", "");
                if (underline != null)
                {
                    underline.FontSize = double.Parse(fontSize);
                }
                else
                {
                    span.FontSize = double.Parse(fontSize);
                }
            }

            var fontWeight = element.Style("font-weight");
            if (!string.IsNullOrEmpty(fontWeight))
            {
                if (underline != null)
                {
                    underline.FontWeight = FontWeights.Bold;
                }
                else
                {
                    span.FontWeight = FontWeights.Bold;
                }
            }


            var textAlign = element.Style("text-align");
            if (!string.IsNullOrEmpty(textAlign))
            {
                if (textAlign == "left")
                {
                    parentContainer.TextAlignment = Windows.UI.Xaml.TextAlignment.Left;
                }
                else if (textAlign == "right")
                {
                    parentContainer.TextAlignment = Windows.UI.Xaml.TextAlignment.Right;
                }
                else
                {
                    parentContainer.TextAlignment = Windows.UI.Xaml.TextAlignment.Center;
                }
            }

            var textStyle = element.Style("text-style");
            if (!string.IsNullOrEmpty(textStyle))
            {
                if (textStyle == "italic")
                {
                    if (underline != null)
                    {
                        underline.FontStyle = FontStyle.Italic;
                    }
                    else
                    {
                        span.FontStyle = FontStyle.Italic;
                    }
                }
            }

            if (underline != null)
            {
                parent.Add(underline);
                context.RenderNode(element, new SpanContainer(underline));
            }
            else
            {
                parent.Add(span);
                context.RenderNode(element, new SpanContainer(span));
            }
        }
    }
}