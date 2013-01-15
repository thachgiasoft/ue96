using System;

using Icson.Utils;

namespace Icson.Objects.Basic
{
	/// <summary>
	/// Summary description for LogInfo.
	/// </summary>
	public class LogInfo
	{
		public LogInfo()
		{
			Init();
		}
		public LogInfo(SessionInfo oSessionInfo)
		{
			Init();
			_optip = oSessionInfo.IpAddress;
			_optusersysno = oSessionInfo.User.SysNo;
		}
		public LogInfo(int paramTicketSysno, int paramTicketType, SessionInfo oSessionInfo) : this(oSessionInfo)
		{
			_ticketsysno = paramTicketSysno;
			_tickettype = paramTicketType;
		}
		public LogInfo(int paramTicketSysno, int paramTicketType, string paramNote, SessionInfo oSessionInfo) : this(paramTicketSysno, paramTicketType,oSessionInfo)
		{
			_note = paramNote;
		}

		private int _sysno;
		private DateTime _opttime;
		private int _optusersysno;
		private string _optip;
		private int _tickettype;
		private int _ticketsysno;
		private string _note;

		public void Init()
		{
			_sysno = AppConst.IntNull;
			_opttime = AppConst.DateTimeNull;
			_optusersysno = AppConst.IntNull;
			_optip = AppConst.StringNull;
			_tickettype = AppConst.IntNull;
			_ticketsysno = AppConst.IntNull;
			_note = AppConst.StringNull;
		}

		public int SysNo
		{
			get
			{
				return _sysno;
			}
			set
			{
				_sysno = value;
			}
		}
		public DateTime OptTime
		{
			get
			{
				return _opttime;
			}
			set
			{
				_opttime = value;
			}
		}
		public int OptUserSysNo
		{
			get
			{
				return _optusersysno;
			}
			set
			{
				_optusersysno = value;
			}
		}
		public string OptIP
		{
			get
			{
				return _optip;
			}
			set
			{
				_optip = value;
			}
		}
		public int TicketType
		{
			get
			{
				return _tickettype;
			}
			set
			{
				_tickettype = value;
			}
		}
		public int TicketSysNo
		{
			get
			{
				return _ticketsysno;
			}
			set
			{
				_ticketsysno = value;
			}
		}
		public string Note
		{
			get
			{
				return _note;
			}
			set
			{
				_note = value;
			}
		}

	}
}
