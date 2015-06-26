/*****************************************************************************\
>
>
>
>
>
>
>
\*****************************************************************************/

// IHtmlParser.cs: implementation of the IHtmlParser interface.
//
///////////////////////////////////////////////////////////////////////////////

using Cloud9.Parser.Html;

namespace Cloud9.Parser.Html
{
    public delegate void ElementEventHandler(CHtmlElement element);
    public delegate void StyleEventHandler(CHtmlStyle style);
    public delegate void ScriptEventHandler(CHtmlScript script);
    public delegate void CommentEventHandler(CHtmlComment comment);
    public delegate void ProcessingInstructionEventHandler(CHtmlProcessingInstruction pi);
    public delegate void TextEventHandler(CHtmlText text);

	/// <summary>
	/// 파서 인터페이스
	/// </summary>
    public interface IHtmlParser
	{
        /// <summary>
        /// 파싱하고 CHtmlNodeCollection 로 결과 반환
        /// </summary>
        /// <param name="html"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        void Parse(string html, CHtmlNodeCollection result);

        /// <summary>
        /// 파싱하고  CHtmlNodeCollection 로 반환
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        CHtmlNodeCollection Parse(string html);

        /// <summary>
        /// 
        /// </summary>
        event ElementEventHandler ElementCreatedEvent;
        /// <summary>
        /// 
        /// </summary>
        event StyleEventHandler StyleCreatedEvent;
        /// <summary>
        /// 
        /// </summary>
        event ScriptEventHandler ScriptCreatedEvent;
        /// <summary>
        /// 
        /// </summary>
        event CommentEventHandler CommentCreatedEvent;        
        /// <summary>
        /// 
        /// </summary>
        event ProcessingInstructionEventHandler ProcessingInstructionCreatedEvent;
        /// <summary>
        /// 
        /// </summary>
        event TextEventHandler TextCreatedEvent;

	}
}
