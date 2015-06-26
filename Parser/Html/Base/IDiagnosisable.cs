/*****************************************************************************\
>
>
>	 Cloud9.Parser.Html.Base.IDiagnosisable Class
>
>
>
>
\*****************************************************************************/

// Cloud9.Parser.Html.Base.IDiagnosisable.cs: implementation of the Cloud9.Parser.Html.Base.IDiagnosisable class.
//
///////////////////////////////////////////////////////////////////////////////

using System.Text;

namespace Cloud9.Parser.Html.Base
{
	/// <summary>
	/// Cloud9.Parser.Html.Base.IDiagnosisable
	/// </summary>
	public interface IDiagnosisable
	{

	/////////////////////////////////////////////////////////////////////////////////
	#region �����	

		/////////////////////////////////////////////////////////////////////////////        
		/// <summary>
		/// 
		/// </summary>
		void AssertValid();

		/////////////////////////////////////////////////////////////////////////////       
		/// <summary>
		/// 
		/// </summary>
		/// <param name="buffer">  </param>
		/// <param name="prefix">  </param>
		void Dump(StringBuilder buffer, string prefix);

    #endregion

	}
}
