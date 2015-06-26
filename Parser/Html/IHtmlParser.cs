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
	/// �ļ� �������̽�
	/// </summary>
    public interface IHtmlParser
	{
        /// <summary>
        /// �Ľ��ϰ� CHtmlNodeCollection �� ��� ��ȯ
        /// </summary>
        /// <param name="html"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        void Parse(string html, CHtmlNodeCollection result);

        /// <summary>
        /// �Ľ��ϰ�  CHtmlNodeCollection �� ��ȯ
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
