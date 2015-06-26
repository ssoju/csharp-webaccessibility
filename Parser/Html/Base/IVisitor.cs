/*****************************************************************************\
>
>
>	 IBaseVisitor interface
>
>
>
>
\*****************************************************************************/

// IBaseVisitor: implementation of the IBaseVisitor interface.
//
///////////////////////////////////////////////////////////////////////////////

namespace Cloud9.Parser.Html.Base
{
    public interface IVisitor<T>
    {
        void Visit(T t);
    }
}
