using System;
using System.Collections.Generic;
using System.Text;
using Cloud9.Checker;
using Cloud9.Parser.Html;

namespace Checker.Checkers
{
    public class ImageChecker : IChecker
    {

        public override bool Perform(CHtmlDocument doc)
        {
            CheckImageAlt(doc, doc.Nodes);
            return true;
        }

        public override string GetCheckerName()
        {
            return "ImageChecker";
        }

        private void CheckImageAlt(CHtmlDocument doc, CHtmlNodeCollection nodes)
        {
            foreach (CHtmlNode node in nodes)
            {
                if (node is CHtmlElement)
                {
                    CHtmlElement element = node as CHtmlElement;
                    if ("img".Equals(element.Name.ToLower(), StringComparison.OrdinalIgnoreCase))
                    {
                        if (element.Attributes.HasAttribute("alt") == false)
                        {
                            // 리포터 모듈 작성 할것
                            AddReportItem(doc.HRef, element.HTML, "["+1001+"] img태그에 Alt가 없습니다.");
                        }

                        if(element.Attributes.HasAttribute("height") == false)
                        {
                            AddReportItem(doc.HRef, element.HTML, "["+1002+"] img태그에 Height속성이 없습니다.");
                        }
                        
                        if(element.Attributes.HasAttribute("width") == false)
                        {
                            AddReportItem(doc.HRef, element.HTML, "[" + 1003 + "] img태그에 Width속성이 없습니다.");
                        }
                    }

                    if (element.Nodes.Count > 0)
                        CheckImageAlt(doc, element.Nodes);
                }
            }
        }
    }
}
