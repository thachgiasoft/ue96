using System;

namespace Icson.BLL
{
	/// <summary>
	/// Summary description for BizException.
	/// </summary>
	[Serializable]
	public class BizException : ApplicationException
	{
		public BizException() {}

		public BizException(string message) : base(message) {}

		public BizException(string message, Exception e) : base(message, e) {}
	}
}
